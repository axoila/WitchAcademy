using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour {

	public TimeManager time;

	public Text displayText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(displayText)
			displayText.text = string.Format("{0:D}:{1:D2}", (int)time.hours, (int)time.minutes);
	}
}
