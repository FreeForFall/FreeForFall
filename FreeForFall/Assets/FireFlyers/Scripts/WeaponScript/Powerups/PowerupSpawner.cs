using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public bool Spawned = false;

    void OnTriggerEnter(Collider c)
    {
        Debug.Log("COLLIDED");
        if (c.gameObject.tag != "Player")
            return;
        if (Spawned)
            Spawned = false;
    }
}
