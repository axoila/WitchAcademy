using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Appointment {
	public float time;
	public string name;
	public ProceduralPoint place;

	public Appointment(){
		time = 0;
		name = "unnamed";
		place = null;
	}

	public Appointment(float time, string name, ProceduralPoint place){
		this.time = time;
		this.name = name;
		this.place = place;
	}

	public delegate PointOfInterrest ProceduralPoint();
}
