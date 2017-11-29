using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Relationship", menuName = "Characters/Relationship", order = 1)]
public class Relationship : ScriptableObject {
    public float trust;
    public float affection;
}
