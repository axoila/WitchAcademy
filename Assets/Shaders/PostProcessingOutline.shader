Shader "Hidden/PostProcessingOutline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_EdgeSettings ("depth scale/pow | normals scale/pow", Vector) = (1,1,1,1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend Zero SrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float4 _EdgeSettings;
			sampler2D_float _CameraDepthNormalsTexture;

			inline void getDepthNormal(float2 uv, out float depth, out float3 normal){
				fixed4 depthNormal = tex2D(_CameraDepthNormalsTexture, uv);

				DecodeDepthNormal(depthNormal, depth, normal);
				depth *= _ProjectionParams.z;
			}

			inline float pow2(float x){
				return x*x;
			}

			inline float pow4(float x){
				return x*x*x*x;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float2 pixelSize = _ScreenParams.zw - 1;

				float depth;
				float3 normal;

				float centerDepth;
				float3 centerNormal;
				float depthEdge = 0;
				float3 normalEdge = 0;

				getDepthNormal(i.uv, centerDepth, centerNormal);

				getDepthNormal(i.uv + float2(pixelSize.x, 0), depth, normal);
				depthEdge += centerDepth - depth;
				normalEdge += centerNormal - normal;

				getDepthNormal(i.uv + float2(-pixelSize.x, 0), depth, normal);
				depthEdge += centerDepth - depth;
				normalEdge += centerNormal - normal;

				getDepthNormal(i.uv + float2(0, pixelSize.y), depth, normal);
				depthEdge += centerDepth - depth;
				normalEdge += centerNormal - normal;

				getDepthNormal(i.uv + float2(0, -pixelSize.y), depth, normal);
				depthEdge += centerDepth - depth;
				normalEdge += centerNormal - normal;

				float edge = pow2(depthEdge * _EdgeSettings.x) + pow4((normalEdge.x + normalEdge.y + normalEdge.z) * _EdgeSettings.z);

				//col.rgb = 1 - col.rgb;
				return saturate(1-edge);//normal.rgbr;
			}
			ENDCG
		}
	}
}
