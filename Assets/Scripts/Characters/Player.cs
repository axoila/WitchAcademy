using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Player : MonoBehaviour {

    public float speed = 10;

    NavMeshAgent agent;
    NavMeshObstacle obstacle;

    void Awake(){
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    void Start(){
        agent.enabled = true;
        obstacle.enabled = false;
    }

    public void Update(){
        Move();
    }

	void Move(){

		//input
		Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (velocity.magnitude > 1)
            velocity.Normalize();

        //rotate
        if (velocity != Vector3.zero)
            transform.localEulerAngles = new Vector3(0, Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, 0);
		
		//convert from input to speed
		velocity *= speed;

        if(Input.GetKey(KeyCode.LeftShift))
            velocity *= 10;

		//move
		agent.Move(velocity * Time.deltaTime);
	}
}
