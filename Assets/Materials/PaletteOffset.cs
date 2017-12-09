using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PaletteOffset : MonoBehaviour {

    public Renderer thing;
    public Vector2Int colorOffset;

    void OnValidate()
    {
		MaterialPropertyBlock block = new MaterialPropertyBlock();
        block.SetVector("_Offset", new Vector4(colorOffset.x, colorOffset.y, 0, 0));
        thing.SetPropertyBlock(block);
        //print(new Vector4(colorOffset.x, colorOffset.y, 0, 0));
    }
}
