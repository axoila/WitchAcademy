Shader "Custom/Window" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		uv("uv bs", 2D) = "white" {}
		thickness("thickness", range(0, 0.5)) = 0.1
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Geometry"}

		ZWrite On

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uvuv : TEXCOORD0;
		};

		fixed4 _Color;
		float thickness;
		//static float4 uv_ST = float4(1,1,0,0);

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			IN.uvuv = 0.5-abs(frac(IN.uvuv)-0.5);
			float border = thickness - min(IN.uvuv.x, IN.uvuv.y);
			clip(border);
			fixed4 c = _Color;
			o.Albedo = c.rgb;
			
		}
		ENDCG
	}
	FallBack "Diffuse"
}
