using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(CharacterBlackboards))]
public class Character : MonoBehaviour {

    new public string name;
	public float speed = 10;

	protected NavMeshAgent agent;
    [HideInInspector] public CharacterBlackboards blackboards;

	void Awake(){
        blackboards = GetComponent<CharacterBlackboards>();
        gameObject.name = name;

        agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
        agent.acceleration = speed * 2;
        agent.angularSpeed = 360;
        agent.stoppingDistance = 1;
    }
}
