using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PointOfInterrest : MonoBehaviour {

	public static HashSet<PointOfInterrest> points = new HashSet<PointOfInterrest>();
	
	public string description;

	void Awake(){
		gameObject.name = description;
	}

	void OnEnable () {
		points.Add(this);
	}

	void OnDisable(){
		points.Remove(this);
	}

	public static PointOfInterrest GetByName(string name){
		IEnumerable<PointOfInterrest> applicablePoints = 
			points.Where(p => p.description == name);
		PointOfInterrest point = applicablePoints.FirstOrDefault();
		if(point == null)
			Debug.LogWarning("point of interrest "+name+"coudn't be found");
		return point;
	}

	public static PointOfInterrest GetClosestByName(string name, Vector3 position){
		IEnumerable<PointOfInterrest> applicablePoints = 
			points.Where(p => p.description == name);
		
		PointOfInterrest closest = null;
		float closestSqrDistance = float.PositiveInfinity;
		foreach(PointOfInterrest p in applicablePoints){
			float sqrDist = Vector3.SqrMagnitude(p.transform.position-position);
			if(sqrDist < closestSqrDistance){
				closest = p;
				closestSqrDistance = sqrDist;
			}
		}
		return closest;
	}
}
