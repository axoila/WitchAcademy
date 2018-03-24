using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character {

    AppointmentList appointments = new AppointmentList();
    Appointment activeAppointment;

    public Player player; //for debugging

    void Start()
    {
        appointments.Add(
            new Appointment(
                TimeManager.worldTime.HourMinuteToTime(10)
                , "dance"
                , () => PointOfInterrest.GetClosestByName("Well", transform.position)));

        StartCoroutine(TalkTo(player));
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

    public IEnumerator TalkTo(Character chara){
        yield return GoTo(chara.transform);

        Debug.Log("starting at " + TimeManager.worldTime.minutes);
        yield return TimeManager.worldTime.FastForward(2);
        Debug.Log("asfa");
        yield return TimeManager.worldTime.FastForward(2);
        Debug.Log("vsg");
        yield return TimeManager.worldTime.FastForward(2);
        Debug.Log("finishing at " + TimeManager.worldTime.minutes);
    }

    public IEnumerator GoTo(Transform goal){
        agent.SetDestination(goal.position);
        while(Vector3.Distance(transform.position, goal.position) > 2){
            if(agent.destination != goal.position)
                agent.SetDestination(goal.position);
            yield return null;
        }
    }
}
