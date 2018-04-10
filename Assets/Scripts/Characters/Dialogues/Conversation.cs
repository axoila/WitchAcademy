using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Conversation")]
public class Conversation : ScriptableObject {

	[Header("Blackboard tags")]
	public string[] ifCondition;
	public string[] ifNotCondition;

	[Header("Data")]
	public string startNode;
	public TextAsset conversation;
}
