using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenuAttribute]
public class TimeManager : ScriptableObject {

    public static TimeManager worldTime;

    [Header("Constants")]
	public Gradient ambientLight;
    [Tooltip("Start of the day in hours")]
	public float dayStart = 9;
    [Tooltip("End of the day in hours")]
	public float dayEnd = 21;
    [Tooltip("How much faster than real time is the ingame time")]
    public float timeAcceleration = 30;
	[Tooltip("the amount of time events sent per minute")]
	public float timeBetweenUpdates;

    [Header("Variables")]
    public int dayIndex;
    //public Color currentSky;
    public float currentTime;
    public Day currentDay;

    public TimeUpdate update = new TimeUpdate();
    public TimeUpdate startDay = new TimeUpdate();

    public Day day{
		get{
            return (Day)Mathf.Min(dayIndex%5, dayIndex%7);
        }
	}

    public float hours{
        get{
            return dayStart + currentTime / 3600;
        }
    }

    public float minutes{
        get{
            return (currentTime/60)%60;
        }
    }

    private float lastTimeUpdate;

	/*void OnEnable(){
        update.AddListener((value) => Debug.Log("update"));
	}*/

    public void Tick(){
        currentTime += Time.deltaTime * timeAcceleration;

        if(hours >= dayEnd){
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
		update.Invoke(this, currentTime);
        startDay.Invoke(this, 0);
        currentDay = day;
    }

    public void UpdateTime(){
        //Debug.Log(lastTimeUpdate + " | " + currentTime);
        if(update != null)
            update.Invoke(this, currentTime);
        while(lastTimeUpdate + timeBetweenUpdates <= currentTime){
            lastTimeUpdate += timeBetweenUpdates;
        }
    }

    public void Reset(){
        currentTime = 0;
		currentDay = 0;
		dayIndex = 0;
    }

    /*public void UpdateSky(){
        Shader.SetGlobalColor("_AmbientColor", ambientLight.Evaluate(currentTime/dayLength));
        currentSky = ambientLight.Evaluate(currentTime / dayLength);
    }*/

    public float HourMinuteToTime(float hour, float minute = 0){
        return (hour-dayStart)*3600 + minute * 60;
    }
}

public enum Day{
	FREE_DAY,
	DAY1,
	DAY2,
	DAY3,
	DAY4
}

public class TimeUpdate : UnityEvent<TimeManager, float> {}