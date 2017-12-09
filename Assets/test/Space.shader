// Upgrade NOTE: replaced '_CameraToWorld' with 'unity_CameraToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Space" {
	Properties {
		_Color ("Base Color", Color) = (1,1,1,1)
		_Fresnel ("Fresnel Color", Color) = (1,1,1,1)

		_Scale("Frequency", Range(1, 10)) = 10.0
		_StarScale("Star Frequency", Range(1, 100)) = 10.0
		_Jitter("Jitter", Range(0,1)) = 1.0
		_Intensity("Intensity", Range(0, 5)) = 1.0
		_StarThreshold("Star threshold", Range(0.5, 1.)) = 0.85
	}
	SubShader {
		Cull Back ZTest Less

		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
		//Blend SrcAlpha OneMinusSrcAlpha
		
		Pass
        {
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma glsl

			#include "GPUVoronoiNoise3D.cginc"
			#include "perlinNoise.cginc"
			//#include "Lighting.cginc"
			#include "UnityCG.cginc"

			#define STEPS 32
			#define OCTAVES 1

			fixed4 _Color;
			fixed4 _Fresnel;
			float _Scale;
			float _Intensity;
			float _StarThreshold;
			float _StarScale;
			sampler2D _Random;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 normal : NORMAL;
			};

			struct v2f 
			{
				float4 pos : SV_POSITION; // Clip space
				float3 oPos : TEXCOORD0; // Object position
				float3 oCamPos : TEXCOORD1;
				float3 oNormal : NORMAL;
			};

			#define RADIUS 0.5

			float exitDistance(float3 origin, float3 dir){
				float dirDotOrigin = dot(dir, origin);
				return -dirDotOrigin + sqrt(dirDotOrigin * dirDotOrigin - origin * origin + RADIUS * RADIUS);
			}

			float isInSphere(float3 oPos){
				return length(oPos) < 0.5;
			}

			float density(float3 oPos){
				float noise = cnoise(oPos.zyx * _Scale);
				return noise;
			}

			// Vertex function
			v2f vert (appdata v) 
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.oPos = v.vertex.xyz;
				float4 worldPos  = mul(unity_CameraToWorld, float4(0, 0, 0, 1));   // Transform View to World
				o.oCamPos = mul(unity_WorldToObject, worldPos); 
				o.oNormal = UnityObjectToWorldNormal(v.normal);
				return o;
			}

			fixed4 raycast(float3 position, float3 direction)
			{
				float stepLength = exitDistance(position, direction) / STEPS;
				direction = direction * stepLength;

				fixed value = 0;
				fixed localDensity = 0;
				fixed localStarDensity = 0;

				[loop]
				for(int i = 0; i < STEPS; i++){
					localDensity = cnoise(position * _Scale)*0.5+0.5;
					localStarDensity = fBm_F0(position * _StarScale, OCTAVES);
					//return localDensity;
					value += localDensity * _Intensity * stepLength;
					if(localStarDensity > _StarThreshold)
						return 2;
					
					position += direction;
				}
				return fixed4(_Color.rgb* value, 1);
			}

			fixed4 frag (v2f i) : SV_TARGET
			{
				float3 position = i.oPos;
				float3 viewDirection = normalize(i.oPos - i.oCamPos);
				fixed4 col = raycast(position, viewDirection);

				float3 worldViewDir = normalize(WorldSpaceViewDir(float4(i.oPos, 1)));
				float fresnel = saturate((1-dot(i.oNormal, worldViewDir))*_Fresnel.a);
				col = lerp(col,_Fresnel,fresnel);
				return col;
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
