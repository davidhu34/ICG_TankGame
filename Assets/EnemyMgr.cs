using UnityEngine;
using System.Collections;

public class EnemyMgr : MonoBehaviour {
    public GameObject enemyTank;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Spawn", 5f, 5f);
    }


    void Spawn()
    {
        Vector3 SpawnPosition = new Vector3(PosNeg() * Random.Range(35f, 45f), 0f, PosNeg() * Random.Range(35f, 45f));
        Quaternion SpawnRotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
        Instantiate( enemyTank, SpawnPosition, SpawnRotation);
    }

    private float PosNeg()
    {
        float random = Random.Range(-1f, 1f);
        if (random < 0) return -1f;
        else return 1f;
    }
}