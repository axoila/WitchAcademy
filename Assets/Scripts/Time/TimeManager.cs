using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenuAttribute]
public class TimeManager : ScriptableObject {
    [Header("Constants")]
	public Gradient ambientLight;
    [Tooltip("the length of a day in seconds")]
	public float dayLength;
	[Tooltip("the amount of time events sent per minute")]
	public float timeBetweenUpdates;

    [Header("Variables")]
    public int dayIndex;
    //public Color currentSky;
    public float currentTime;
    public Day currentDay;

    public TimeUpdate update = new TimeUpdate();

    public Day day{
		get{
            return (Day)Mathf.Min(dayIndex%5, dayIndex%7);
        }
	}

    private float lastTimeUpdate;

	/*void OnEnable(){
        update.AddListener((value) => Debug.Log("update"));
	}*/

    public void Tick(){
        currentTime += Time.deltaTime;

        if(currentTime >= dayLength){
            StartNewDay();
        }

		if(lastTimeUpdate + timeBetweenUpdates <= currentTime){
            UpdateTime();
        }

        //UpdateSky();
    }

    public void ResetLight(){
        Shader.SetGlobalColor("_AmbientColor", Color.white);
    }

	public void StartNewDay(){
        currentTime = 0;
        dayIndex++;
        lastTimeUpdate = 0;
		update.Invoke(this);
        currentDay = day;
    }

    public void UpdateTime(){
        Debug.Log(lastTimeUpdate + " | " + currentTime);
        if(update != null)
            update.Invoke(this);
        while(lastTimeUpdate + timeBetweenUpdates <= currentTime){
            lastTimeUpdate += timeBetweenUpdates;
        }
    }

    /*public void UpdateSky(){
        Shader.SetGlobalColor("_AmbientColor", ambientLight.Evaluate(currentTime/dayLength));
        currentSky = ambientLight.Evaluate(currentTime / dayLength);
    }*/
}

public enum Day{
	FREE_DAY,
	DAY1,
	DAY2,
	DAY3,
	DAY4
}

public class TimeUpdate : UnityEvent<TimeManager> {}