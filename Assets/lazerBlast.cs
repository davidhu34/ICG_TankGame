using UnityEngine;
using System.Collections;

public class lazerBlast : MonoBehaviour {

	void Start () {
        Destroy(gameObject, 1f);
	}
    void OnTriggerEnter(Collider enter)
    {
        TankHealth targetHealth = enter.gameObject.GetComponent<TankHealth>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(60);
        }
    }
	void Update () {
	}
}
