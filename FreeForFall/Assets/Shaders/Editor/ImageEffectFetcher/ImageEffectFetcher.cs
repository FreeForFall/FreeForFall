using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Ionic.Zip;
using SimpleJSON_AvoidNamespaceConflict;
using UnityEditor;

public class ImageEffectFetcher : EditorWindow {

    [MenuItem("Window/ImageEffectFetcher")]
    public static void Init()
    {
        // Get existing open window or if none, make a new one:
        ImageEffectFetcher window = (ImageEffectFetcher)GetWindow(typeof(ImageEffectFetcher));
        window.Show();
    }
    
    private Camera m_Camera;

    private string[] m_Branches = new string[0];
    private int m_SelectedBranch = -1;
    private void OnGUI()
    {
        if (GUILayout.Button("Find Branches"))
            FindBranches();

        if (m_Branches.Length > 0)
        {
            var indicies = new int[m_Branches.Length];
            var i = 0;
            Array.ForEach(m_Branches, x => { indicies[i] = i; i++; });

            m_SelectedBranch = EditorGUILayout.IntPopup(m_SelectedBranch, m_Branches.ToArray(), indicies);
        }
        else
            m_SelectedBranch = -1;

        EditorGUI.BeginDisabledGroup( m_SelectedBranch < 0 || m_SelectedBranch >= m_Branches.Length);
        if (GUILayout.Button("Fetch"))
            FetchImageEffects(m_Branches[m_SelectedBranch]);
        EditorGUI.EndDisabledGroup();

        GUILayout.Space(30);
        GUILayout.Label("Camera Settings", EditorStyles.boldLabel);
        m_Camera = (Camera)EditorGUILayout.ObjectField("Setup Camera", m_Camera, typeof(Camera), true);
        if (GUILayout.Button("Execute"))
            ConfigureCamera(m_Camera);
    }

    private void FindBranches()
    {
        var url = "https://bitbucket.org/api/1.0/repositories/Unity-Technologies/cinematic-image-effects/branches/";

        var form = new WWWForm();
        var headers = form.headers;
        var response = new WWW(url, null, headers);

        while (!response.isDone)
        {}

        if (response.error != null)
        {
            Debug.Log("response error: " + response.error);
            Debug.Log("response error content: " + response.text);
        }
        else
        {
            Debug.Log("response success");
            Debug.Log("returned data" + response.text);

            var result = JSON.Parse(response.text);

            var branches = new List<string>();

            foreach (var thing in result.Children)
            {
                var branch = thing["branch"].Value;
                if (!string.IsNullOrEmpty(branch))
                    branches.Add(branch);
            }
            m_Branches = branches.ToArray();
        }
    }

    private static void ConfigureCamera(Camera c)
    {
        if (c != null)
        {
            c.hdr = true;
            AddImageEffect(c.gameObject, "UnityStandardAssets.CinematicEffects.ScreenSpaceReflection");
            AddImageEffect(c.gameObject, "UnityStandardAssets.CinematicEffects.AntiAliasing");
            AddImageEffect(c.gameObject, "UnityStandardAssets.CinematicEffects.DepthOfField");
            AddImageEffect(c.gameObject, "UnityStandardAssets.CinematicEffects.TonemappingColorGrading");
        }
    }

    private void FetchImageEffects(string branch)
    {
        WWW testwww = new WWW(string.Format("https://bitbucket.org/Unity-Technologies/cinematic-image-effects/get/{0}.zip", branch));
        while (!testwww.isDone)
        {}

        try
        {
            //remove old filed
            var effectsPath = Application.dataPath;
            effectsPath = Path.Combine(effectsPath, "Standard Assets");
            effectsPath = Path.Combine(effectsPath, "Effects");

            if (Directory.Exists(effectsPath))
            {
                //find all paths starting with CinematicEffects and remove them
                foreach (var directory in Directory.GetDirectories(effectsPath))
                {
                    if (directory.Contains("CinematicEffects"))
                        Directory.Delete(directory, true);
                }
            }

            Directory.GetParent(Application.dataPath);
            
            // Open file for reading
            var bytes = testwww.bytes;
            Stream stream = new MemoryStream(bytes);

            using (var zip = ZipFile.Read(stream))
            {
                var basePath = Path.Combine("Standard Assets", "Effects");

                foreach (ZipEntry e in zip.ToArray())
                {
                    if (e.FileName.Contains("UnityProject/Assets/Standard Assets/Effects/"))
                    {
                        var baseFilename = e.FileName;
                        string[] substrings = Regex.Split(baseFilename, "UnityProject/Assets/Standard Assets/Effects/");

                        if (substrings.Length != 2)
                            continue;

                        substrings[1] = substrings[1].Replace('/', Path.DirectorySeparatorChar);
                        var path = Path.Combine(basePath, substrings[1]);

                        e.FileName = path;
                        e.Extract(Application.dataPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                }
            }
        }
        catch (Exception exception)
        {
            // Error
            Debug.LogErrorFormat("Exception caught in process: {0}", exception);
            return;
        }
        AssetDatabase.Refresh();
    }

    private static void AddImageEffect(GameObject camera, string type)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        Type found = null;
        foreach (var a in assemblies)
        {
            found = a.GetType(type);
            if (found != null)
                break;
        }
        
        if (found == null)
        {
            Debug.LogWarningFormat("Could not find effect {0}", type);
            return;
        }

        var oldEffect = camera.GetComponent(found);
        DestroyImmediate(oldEffect);
        camera.AddComponent(found);
    }
}
