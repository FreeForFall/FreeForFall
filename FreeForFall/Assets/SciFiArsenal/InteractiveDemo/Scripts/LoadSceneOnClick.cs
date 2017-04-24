using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadScene1()  {
		SceneManager.LoadScene ("scifi_projectiles");
	}
    public void LoadScene2()  {
        SceneManager.LoadScene("scifi_beamup");
	}
    public void LoadScene3()  {
        SceneManager.LoadScene("scifi_buff");
	}
    public void LoadScene4()  {
		SceneManager.LoadScene ("scifi_flamethrowers");
	}
    public void LoadScene5()  {
        SceneManager.LoadScene ("scifi_hexagonzone");
	}
    public void LoadScene6()  {
        SceneManager.LoadScene ("scifi_lightjump");
	}
    public void LoadScene7()  {
        SceneManager.LoadScene ("scifi_loot");
	}
    public void LoadScene8()  {
        SceneManager.LoadScene ("scifi_muzzleflash");
    }
    public void LoadScene9()  {
        SceneManager.LoadScene ("scifi_portals");
    }
    public void LoadScene10() {
        SceneManager.LoadScene("scifi_regenerate");
    }
    public void LoadScene11() {
        SceneManager.LoadScene("scifi_shields");
    }
    public void LoadScene12() {
        SceneManager.LoadScene("scifi_swirlyaura");
    }
    public void LoadScene13() {
        SceneManager.LoadScene("scifi_warpgates");
    }
    public void LoadScene14(){
        SceneManager.LoadScene("scifi_jetflame");
    }
    public void LoadScene15(){
        SceneManager.LoadScene("scifi_ultimatenova");
    }
}