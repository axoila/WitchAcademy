using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public static TimeManager worldTime;

    [Tooltip("Start of the day in hours")]
    public float dayStart = 9;
    [Tooltip("End of the day in hours")]
	public float dayEnd = 21;
    [Tooltip("How much faster than real time is the ingame time")]
    public float timeAcceleration = 30;
    [Tooltip("how much faster the game plays during speedup")]
    public float fastForwardMultiplier = 10;
    [Tooltip("the amount of time events sent per minute")]
	public float timeBetweenUpdates;

    [Header("Variables")]
    public int dayIndex;
    public float currentTime;
    public Day currentDay;

    public TimeUpdate update;
    public TimeUpdate startDay;

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

    [HideInInspector]public bool paused = false;
    private float lastTimeUpdate;
    bool fastForwarding = false;
    

    // Use this for initialization
    void Awake () {
		if(worldTime == null){
            worldTime = this;
        } else if(worldTime != this) {
            worldTime = this;
            Debug.LogWarning("new worldTime set");
        }

        Reset();
	}
	
	void Update () {
        if(!fastForwarding)
            currentTime += Time.deltaTime * timeAcceleration;

        if(hours >= dayEnd){
            StartNewDay();
        }

		if(lastTimeUpdate + timeBetweenUpdates <= currentTime){
            UpdateTime();
        }
        
        if(!fastForwarding){
            if(paused)
                Time.timeScale = 0;
            else
                if(Input.GetKey(KeyCode.LeftShift)){
                    Time.timeScale = 10;
                } else {
                    Time.timeScale = 1;
                }
        }
    }

    public void UpdateTime(){
        //Debug.Log(lastTimeUpdate + " | " + currentTime);
        if(update != null)
            update.Invoke(this, currentTime);
        while(lastTimeUpdate + timeBetweenUpdates <= currentTime){
            lastTimeUpdate += timeBetweenUpdates;
        }
    }

    public void StartNewDay(){
        currentTime = 0;
        dayIndex++;
        lastTimeUpdate = 0;
		update(this, currentTime);
        startDay(this, 0);
        currentDay = day;
    }

    public void Reset(){
        currentTime = 0;
		currentDay = 0;
		dayIndex = 0;
    }

    public float HourMinuteToTime(float hour, float minute = 0){
        return (hour-dayStart)*3600 + minute * 60;
    }

    public IEnumerator FastForward(float minutes){
        fastForwarding = true;
        //StopCoroutine("FastForward");
        currentTime += minutes * 60;
        Time.timeScale = fastForwardMultiplier;
        yield return new WaitForSeconds((minutes * 60) / timeAcceleration);
        fastForwarding = false;
    }
    
    public delegate void TimeUpdate(TimeManager manager, float currentTime);

    public enum Day{
        FREE_DAY,
        DAY1,
        DAY2,
        DAY3,
        DAY4
    }
}