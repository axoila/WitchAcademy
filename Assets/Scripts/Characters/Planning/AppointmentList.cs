using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AppointmentList{
	List<Appointment> appointments;
	
	public AppointmentList(){
		appointments = new List<Appointment>();
	}

	public Appointment GetNext(){
		if(appointments.Count == 0)
			return null;
		return appointments[0];
	}

	public void Add(Appointment a){
		for(int i=0;i<appointments.Count;i++){
			if(appointments[i].time > a.time){
				appointments.Insert(i, a);
				return;
			}
		}
		appointments.Add(a);
	}

	public void Update(int index){
		Appointment a = appointments[index];
		appointments.RemoveAt(index);
		Add(a);
	}

	public void Update(Appointment a){
		appointments.Remove(a);
		Add(a);
	}

	public Appointment Pop(){
		if(appointments.Count == 0)
			return null;
		Appointment temp = appointments[0];
		appointments.RemoveAt(0);
		return temp;
	}
}
