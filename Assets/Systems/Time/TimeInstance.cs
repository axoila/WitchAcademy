using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInstance : MonoBehaviour {

    public TimeManager manager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        manager.Tick();
    }
}
