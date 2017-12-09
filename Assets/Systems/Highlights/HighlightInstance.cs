using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class HighlightInstance : MonoBehaviour {

    public Highlight highlight;

    void Update(){
		if(!Application.isPlaying)
        	UpdatePoint();
    }

	public void UpdatePoint(){
        highlight.pos = transform.position;
        gameObject.name = highlight.name;
    }
}