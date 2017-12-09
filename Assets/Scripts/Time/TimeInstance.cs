using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TimeInstance : MonoBehaviour {

    public TimeManager manager;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(Application.isPlaying)
            manager.Tick();
        else
            manager.ResetLight();
    }
}
