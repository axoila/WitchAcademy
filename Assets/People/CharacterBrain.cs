using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBrain : ScriptableObject {
    public abstract void Tick(Character chara);
    public abstract void Setup(Character chara);
}
