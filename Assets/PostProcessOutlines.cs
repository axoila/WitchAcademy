using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessOutlines : MonoBehaviour {

	public Shader outlineShader;

	Material outlineMat;
	Camera cam;

	public float depthScale = 1;
	public float normalScale = 1;

	// Use this for initialization
	void Awake () {
		cam = GetComponent<Camera>();
		cam.depthTextureMode |= DepthTextureMode.DepthNormals;
		outlineMat = new Material(outlineShader);
		outlineMat.SetVector("_EdgeSettings", new Vector4(depthScale, 1, normalScale, 1));

		CommandBuffer buf = new CommandBuffer();
		buf.Blit(null, BuiltinRenderTextureType.CameraTarget, outlineMat);
		cam.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque, buf);
	}
	
}
