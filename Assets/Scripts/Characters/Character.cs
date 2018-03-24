using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(NavMeshAgent))]
public class Character : MonoBehaviour {

	public float speed = 10;

	protected NavMeshAgent agent;

	void Awake(){
        agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
        agent.acceleration = speed * 2;
        agent.angularSpeed = 360;
        agent.stoppingDistance = 1;
    }
}
