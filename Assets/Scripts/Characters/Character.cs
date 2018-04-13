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
    public PositionChange OnPositionChange;

	[HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public CharacterBlackboards blackboards;

    private Vector3 approximatePosition;

	void Awake(){
        blackboards = GetComponent<CharacterBlackboards>();
        gameObject.name = name;

        agent = GetComponent<NavMeshAgent>();
		agent.speed = speed;
        agent.acceleration = speed * 2;
        agent.angularSpeed = 360;
        agent.stoppingDistance = 1;
    }

    //returns true when a new approx pos was set
    public bool UpdateApproxPosition(){
        if(transform.position == approximatePosition)
            return false;
        if(Vector3.Distance(transform.position, approximatePosition) > 0.5f){
            approximatePosition = transform.position;
            if(OnPositionChange != null)
                OnPositionChange(approximatePosition);
            return true;
        }
        return false;
    }

    public delegate void PositionChange(Vector3 newPosition);
}
