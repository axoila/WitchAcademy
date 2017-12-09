using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public abstract class CharacterBrain : ScriptableObject {
    public abstract Vector3 position { get;}
    public abstract void Tick(Character chara);
    public abstract void Setup(Character chara);
}