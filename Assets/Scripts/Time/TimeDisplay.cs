using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour {

	public Text displayText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(displayText)
			displayText.text = string.Format("{0:D}:{1:D2}", (int)TimeManager.worldTime.hours, (int)TimeManager.worldTime.minutes);
	}
}
