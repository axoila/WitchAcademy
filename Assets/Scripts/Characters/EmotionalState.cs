using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "new Emotional State", menuName = "Characters/Emotional State", order = 1)]
public class EmotionalState : ScriptableObject {
    public CharacterRelationshipDict relationshipData = new CharacterRelationshipDict();
	private Dictionary<string, Relationship> relationships
    {
        get { return relationshipData.dictionary; }
    }

	public Relationship GetRelationship(string characterName){
		if(!relationships.ContainsKey(characterName)){
            relationships.Add(characterName, new Relationship());
        }
		return relationships[characterName];
	}
}

// ---------------
//  Character => Relationship
// ---------------
[Serializable]
public class CharacterRelationshipDict : SerializableDictionary<string, Relationship> { }