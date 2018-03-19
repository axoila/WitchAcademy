using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerBrain", menuName = "Characters/PlayerControlledCharacter", order = 1)]
public class PlayerBrain : CharacterBrain {

    public float speed = 10;

    public override Vector3 position { 
        get{
            return trans ? trans.position : Vector3.zero;
        } 
    }

    Transform trans;

    public override void Setup(Character chara){
        chara.agent.enabled = true;
        chara.obstacle.enabled = true;

        trans = chara.trans;
    }

    public override void Tick(Character chara){
        Move(chara);
    }

	void Move(Character chara){

		//input
		Vector3 velocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (velocity.magnitude > 1)
            velocity.Normalize();

        //rotate
        if (velocity != Vector3.zero)
            chara.trans.localEulerAngles = new Vector3(0, Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, 0);
		
		//convert from input to speed
		velocity *= speed;

        if(Input.GetKey(KeyCode.LeftShift))
            velocity *= 10;

		//move
		chara.agent.Move(velocity * Time.deltaTime);
	}
}
