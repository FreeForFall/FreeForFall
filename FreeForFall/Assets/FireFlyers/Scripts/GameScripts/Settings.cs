using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AssemblyCSharp;

public class Settings : MonoBehaviour
{

    public bool OnlineMode;

    public float Sensitivity;

    public bool FpsMode;
	
    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("MusicMenu(Clone)") == null)
            Instantiate(Resources.Load("MusicMenu"));
        UnityEngine.Cursor.visible = true;
        var net = GameObject.Find("NetworkManager");
        if (net != null)
            Destroy(net);
        var chat = GameObject.Find("ChatManager");
        if (chat != null)
            Destroy(chat);
        if (GameObject.FindGameObjectsWithTag("Settings").Length > 1)
        {
            Destroy(this);
            return;
        }
        OnlineMode = false;
        Sensitivity = 1f;
        FpsMode = false;
        Debug.Log(OnlineMode);
        DontDestroyOnLoad(transform.gameObject);

        Physics.gravity = new Vector3(0, -Constants.PHYSICS_GRAVITY, 0);
    }

    public void PlayOnline()
    {
        Debug.Log("GONNA PLAY ONLINE");
        OnlineMode = true;
    }
}
