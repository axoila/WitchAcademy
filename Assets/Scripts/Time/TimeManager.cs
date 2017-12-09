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
    public Color currentSky;

    public TimeUpdate update = new TimeUpdate();

    public Day day{
		get{
            return (Day)Mathf.Min(dayIndex%5, dayIndex%7);
        }
	}

	public float time{
		get{
            return Time.time - dayStart;
        }
	}

    private float dayStart;
    private float lastTimeUpdate;

	/*void OnEnable(){
        update.AddListener((value) => Debug.Log("update"));
	}*/

    public void Tick(){
        Shader.SetGlobalColor("_AmbientColor", ambientLight.Evaluate(time/dayLength));
        currentSky = ambientLight.Evaluate(time / dayLength);

		if(lastTimeUpdate + timeBetweenUpdates <= time){
            Debug.Log(lastTimeUpdate + " | " + time);
            if(update != null)
                update.Invoke(this);
            lastTimeUpdate = time;//Mathf.Floor(time / timeBetweenUpdates) * timeBetweenUpdates;
        }
    }

    public void ResetLight(){
        Shader.SetGlobalColor("_AmbientColor", Color.white);
    }

	public void newDay(){
        dayStart = Time.time;
		update.Invoke(this);
    }


}

public enum Day{
	FREE_DAY,
	DAY1,
	DAY2,
	DAY3,
	DAY4
}

public class TimeUpdate : UnityEvent<TimeManager> {}