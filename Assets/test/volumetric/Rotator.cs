using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public Vector3 rotSpeed;

    // Update is called once per frame
    void Update () {
        transform.eulerAngles = rotSpeed * Time.time;
    }
}
