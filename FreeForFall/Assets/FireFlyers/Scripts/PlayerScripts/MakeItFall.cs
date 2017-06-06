using UnityEngine;


public class MakeItFall : MonoBehaviour {

    public bool powerground = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            other.gameObject.BroadcastMessage("CallDrop");

            if (powerground)
            {
                other.GetComponent<WaitToFall>().powerup = true;

            }
        }
    }

    public void reset()
    {
        Invoke("resetinvoke", 10);
    }

    public void resetinvoke()
    {
        powerground = false;
    }
}
