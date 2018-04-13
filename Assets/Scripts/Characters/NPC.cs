using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Character {
    
    public Conversation leavingDialogue;
    

    [Header("Debug Data")]
    public Player player;
    public Conversation confession;


    Coroutine appointmentRoutine;
    Coroutine walkRoutine = null;
    Coroutine talkRoutine = null;
    Coroutine cancelRoutine = null;
    [HideInInspector] public AppointmentList appointments = new AppointmentList();
    Appointment activeAppointment;

    Vector3 distancePosition;
    float nextAppointmentWalkDuration = -1;
    Coroutine updateDistanceRoutine;

    void Start()
    {
        appointments.Add(
                new Appointment(
                    TimeManager.worldTime.HourMinuteToTime(9, 10), "confess",
                    player, confession) );

        appointments.Add(new Appointment(
                TimeManager.worldTime.HourMinuteToTime(9, 20), "dance"
                , () => PointOfInterrest.GetClosestByName("Well", transform.position).transform) );

        OnPositionChange += UpdateDistance;
        if(appointments.GetNext().conversationPartner != null)
			appointments.GetNext().conversationPartner.OnPositionChange += UpdateDistance;
        UpdateDistance(transform.position);
    }

    public void Update()
    {
        UpdateApproxPosition();
        Appointment next = appointments.GetNext();

        //go to the next appointment when it's time
        if(next != null && TimeManager.worldTime.currentTime > next.time - nextAppointmentWalkDuration * TimeManager.worldTime.timeAcceleration && cancelRoutine == null){
            cancelRoutine = StartCoroutine(CancelCurrentAppointment());
            appointmentRoutine = StartCoroutine(StartNewAppointment());
        }
    }

    public void UpdateDistance(Vector3 newPos){
        if(updateDistanceRoutine != null)
            StopCoroutine(updateDistanceRoutine);
        updateDistanceRoutine = StartCoroutine(UpdateDistanceCo());
    }

    IEnumerator StartNewAppointment()
    {
        Appointment newAppointment = appointments.Pop();
        yield return cancelRoutine;
        cancelRoutine = null;

        activeAppointment = newAppointment;
        yield return StartCoroutine(activeAppointment.AttendAppointment(this));
    }

    public IEnumerator TalkTo(Character chara, Conversation dialogue)
    {
        walkRoutine = StartCoroutine(GoTo(chara.transform));
        yield return walkRoutine;
        walkRoutine = null;

        talkRoutine = DialogueManager.instance.StartDialogue(this, chara, dialogue);
        yield return talkRoutine;
        talkRoutine = null;

        activeAppointment = null;
    }

    public IEnumerator GoTo(Transform goal)
    {
       
        agent.SetDestination(goal.position);
        while(Vector3.Distance(transform.position, goal.position) > 1){
            if(agent.destination != goal.position)
                agent.SetDestination(goal.position);
            yield return null;
        }
        agent.SetDestination(transform.position);
    }

    public IEnumerator GoTo(Vector3 goal)
    {
        agent.SetDestination(goal);
        yield return new WaitUntil(() => Vector3.Distance(transform.position, goal) < 1);
        agent.SetDestination(transform.position);
    }

    IEnumerator CancelCurrentAppointment()
    {
        if(appointmentRoutine != null)
            StopCoroutine(appointmentRoutine);
        if(walkRoutine != null){
            StopCoroutine(walkRoutine);
        }
        if(talkRoutine != null){
            DialogueManager.instance.StopDialogue();
            talkRoutine = DialogueManager.instance.StartDialogue(this, DialogueManager.instance.listener, leavingDialogue);
            yield return talkRoutine;
            talkRoutine = null;
        }
        activeAppointment = null;
    }

    IEnumerator UpdateDistanceCo(){
        distancePosition = transform.position;
        NavMeshPath path = new NavMeshPath();
        Appointment next = appointments.GetNext();
        if(next == null)
            yield break;
        agent.CalculatePath(next.place().position, path);
        while(path.status == NavMeshPathStatus.PathPartial)
            yield return null;

        float len = 0;
        if(path.status != NavMeshPathStatus.PathInvalid && path.corners.Length > 1){
            for ( int i = 1; i < path.corners.Length; ++i ){
                len += Vector3.Distance( path.corners[i-1], path.corners[i] );
                //Debug.DrawLine(path.corners[i-1], path.corners[i], Color.red, 10, false);
            }
        }
        nextAppointmentWalkDuration = len / speed;
    }

    void OnDrawGizmos(){
        Gizmos.DrawWireSphere(distancePosition, 1);
    }
}
