using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerControlledBrain", menuName = "Characters/PlayerControlledCharacter", order = 1)]
public class PlayerControllerCharacter : CharacterBrain {

    public float speed = 10;
    public float gravity = -10;

    float fallSpeed = 0;

	public override void Setup(Character chara){
        chara.agent.enabled = false;
        chara.charCon.enabled = true;
    }

    public override void Tick(Character chara){
        Move(chara);
    }

	void Move(Character chara){
        //falling
		if (chara.charCon.isGrounded)
            fallSpeed = 0;
        fallSpeed += gravity * Time.deltaTime;

		//input
		Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (velocity.magnitude > 1)
            velocity.Normalize();

        //rotate
        if (velocity != Vector3.zero)
            chara.trans.localEulerAngles = new Vector3(0, Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, 0);
		
		//convert from input to speed
		velocity *= speed;
        velocity.y = fallSpeed;

		//move
		chara.charCon.Move(velocity * Time.deltaTime);
	}
}
