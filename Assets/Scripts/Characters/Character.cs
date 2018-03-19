using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour {

    public CharacterBrain brain;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public NavMeshObstacle obstacle;
    [HideInInspector] public Transform trans;

    private int brainHash;

    void Awake(){
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        trans = transform;
    }

    // Use this for initialization
    void Start () {
        SetupBrain();
    }
	
	// Update is called once per frame
	void Update () {
        if(brain.GetHashCode() != brainHash)
            SetupBrain();

        if(brain)
            brain.Tick(this);
    }

    void SetupBrain(){
        if (brain != null)
            brain.Setup(this);
        brainHash = brain?brain.GetHashCode():0;
    }
}
