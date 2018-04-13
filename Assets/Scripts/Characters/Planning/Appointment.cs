using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Appointment {
	public float time;
	public string name;
	public ProceduralPoint place;
	public Character conversationPartner;
	public Conversation conversation;

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

	public Appointment(float time, string name, Character character, Conversation conversation){
		this.time = time;
		this.name = name;
		this.conversationPartner = character;
		this.conversation = conversation;
		this.place = () => this.conversationPartner.transform;
	}

	public IEnumerator AttendAppointment(NPC actor){
		//make the npc stop listen to the movement of the next npc it wants to talk to and start listening to the next
		if(conversationPartner != null)
			conversationPartner.OnPositionChange -= actor.UpdateDistance;
		
		Appointment next = actor.appointments.GetNext();
		if(next != null){
			
			if(next.conversation != null){
				actor.appointments.GetNext().conversationPartner.OnPositionChange += actor.UpdateDistance;
			}
		}
		

		if(place != null){
			yield return actor.StartCoroutine(actor.GoTo(place()));
		}
		if(conversation != null){
			yield return actor.StartCoroutine(actor.TalkTo(conversationPartner, conversation));
		}
	}

	public delegate Transform ProceduralPoint();
}
