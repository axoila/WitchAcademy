using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBlackboards : MonoBehaviour {

	Dictionary<Character, Blackboard> boards = new Dictionary<Character, Blackboard>();

	public Blackboard GetBoard(Character chara){
		if(!boards.ContainsKey(chara)){
			boards.Add(chara, new Blackboard());
		}
		return boards[chara];
	}
}

[System.Serializable]
public class Blackboard : Yarn.VariableStorage{
	Dictionary<string, BlackboardValue> values = new Dictionary<string, BlackboardValue>();

	public void SetNumber(string key, float number){

	}

	public float GetNumber(string key){
		return -1;
	}

	public void SetValue(string key, Yarn.Value val){
		if(val.type == Yarn.Value.Type.Null)
			values[key] = null;
		else
			values[key] = new BlackboardValue(val);
	}

	public Yarn.Value GetValue(string key){
		if(!values.ContainsKey(key))
			return Yarn.Value.NULL;
		return new Yarn.Value(values[key].GetNativeVariable());
	}

	public void Clear(){
		Debug.LogWarning("Yarn tried to clear a blackboard!");
	}
}

[System.Serializable]
class BlackboardValue{
	public Type type;
	public string stringValue;
	public float numberValue;
	public bool boolValue;

	public BlackboardValue(Yarn.Value val){
		switch(val.type){
			case Yarn.Value.Type.Number:
				type = Type.Number;
				numberValue = val.AsNumber;
				break;
			case Yarn.Value.Type.String:
				type = Type.String;
				stringValue = val.AsString;
				break;
			case Yarn.Value.Type.Bool:
				type = Type.Bool;
				boolValue = val.AsBool;
				break;
			case Yarn.Value.Type.Null:
				type = Type.Null;
				break;
		}
	}

	public object GetNativeVariable(){
		switch(type){
			case Type.Number:
				return numberValue;
			case Type.String:
				return stringValue;
			case Type.Bool:
				return boolValue;
			case Type.Null:
				return null;
			default:
				return null;
		}
	}

	public enum Type{
		Number,
		String,
		Bool,
		Null
	}
}