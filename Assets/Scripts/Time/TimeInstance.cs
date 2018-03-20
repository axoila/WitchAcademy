using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TimeInstance : MonoBehaviour {

    public TimeManager manager;

    // Use this for initialization
    void Awake () {
		if(TimeManager.worldTime == null){
            TimeManager.worldTime = manager;
        } else if(TimeManager.worldTime != manager) {
            TimeManager.worldTime = manager;
            Debug.LogWarning("new worldTime set");
        }

        manager.Reset();
	}
	
	void Update () {
        if(Application.isPlaying)
            manager.Tick();
        else
            manager.ResetLight();
    }
}
