using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour {

	public GameObject prefab;
    public float distance;
    public float delay;
    public int amount;

    // Use this for initialization
    void Start () {
        StartCoroutine(Spawn());
    }

	IEnumerator Spawn(){
        for (int i = 0; i < amount;i++){
            Instantiate(prefab, transform.position + Random.insideUnitSphere * distance, Quaternion.identity, transform);
            if(delay > 0)
                yield return new WaitForSeconds(delay);
        }
    }
}
