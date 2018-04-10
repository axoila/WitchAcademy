using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : Character {

    public void Update(){
        Move();

        //print(Time.timeScale);
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

		//move
		agent.Move(velocity * Time.deltaTime);
	}
}
