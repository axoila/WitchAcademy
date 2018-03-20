using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour {

	public float speed = 10;

    AppointmentList appointments = new AppointmentList();
    Appointment activeAppointment;

	NavMeshAgent agent;

	void Awake(){
		agent = GetComponent<NavMeshAgent>();
	}

    void Start()
    {
        agent.enabled = true;
        //obstacle.enabled = false;

        agent.speed = speed;
        agent.acceleration = speed * 2;
        agent.angularSpeed = 360;
        agent.stoppingDistance = 1;

        appointments.Add(
            new Appointment(
                TimeManager.worldTime.HourMinuteToTime(10)
                , "dance"
                , () => PointOfInterrest.GetClosestByName("Well", transform.position)));
    }

    public void Update()
    {
        Appointment next = appointments.GetNext();
        if(next != null && TimeManager.worldTime.currentTime > next.time){
            activeAppointment = appointments.Pop();
            agent.SetDestination(
                activeAppointment.place()
                    .transform.position);
        }
    }
}
