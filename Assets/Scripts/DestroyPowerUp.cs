using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyPowerUp : MonoBehaviour
{
	public float timeToDestroy = 5.0f;
	
    // Start is called before the first frame update
    void Start()
    {
		Invoke("DestroyPU", timeToDestroy);
    }

    void DestroyPU()
    {
		Destroy(gameObject);
    }
}
