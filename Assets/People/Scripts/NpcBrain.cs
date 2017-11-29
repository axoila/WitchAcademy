using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NpcBrain", menuName = "Characters/Nonplayer Character", order = 1)]
public class NpcBrain : CharacterBrain
{
	//public serialized variables
    public float speed = 10;

    [Header("Prototypebehaviour")]
    public CharacterBrain target;



	//public fields
	public override Vector3 position
    {
        get
        {
            return trans?trans.position:Vector3.zero;
        }
    }

    //private variables
    Transform trans;

    public override void Setup(Character chara)
    {
        chara.agent.enabled = true;
        chara.charCon.enabled = false;
        chara.obstacle.enabled = false;

        trans = chara.trans;

        chara.agent.speed = speed;
        chara.agent.acceleration = speed * 2;
        chara.agent.angularSpeed = 360;
        chara.agent.stoppingDistance = 1;
    }

    public override void Tick(Character chara)
    {
        Move(chara);
    }

    void Move(Character chara)
    {
		chara.agent.SetDestination(target.position);
    }
}
