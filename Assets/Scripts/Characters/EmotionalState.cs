using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new Emotional State", menuName = "Characters/Emotional State", order = 1)]
public class EmotionalState : ScriptableObject {
    public CharacterRelationshipDict relationshipData = new CharacterRelationshipDict();
	private Dictionary<CharacterBrain, Relationship> relationships
    {
        get { return relationshipData.dictionary; }
    }

	public Relationship GetRelationship(CharacterBrain character){
		if(!relationships.ContainsKey(character)){
            relationships.Add(character, new Relationship());
        }
		return relationships[character];
	}
}

// ---------------
//  Character => Relationship
// ---------------
[Serializable]
public class CharacterRelationshipDict : SerializableDictionary<CharacterBrain, Relationship> { }