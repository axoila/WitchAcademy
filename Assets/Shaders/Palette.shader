Shader "Palette"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Palette", 2D) =  "white" {}
		_PaletteSize ("Palette Size", Vector) = (1,1,1,1)
		[PerRendererData] _Offset("Color position(ZW)", Vector) = (0,0,0,0)
		//[HideInInspector] _AmbientColor("Ambient color", Color) = (1,0,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "DisableBatching"="True"}
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
                //UNITY_VERTEX_INPUT_INSTANCE_ID // necessary only if you want to access instanced properties in fragment Shader.
            };

			sampler2D _MainTex;
			float2 _PaletteSize;
			uniform float4 _AmbientColor;

            UNITY_INSTANCING_CBUFFER_START(MyProperties)
                UNITY_DEFINE_INSTANCED_PROP(int2, _Offset)
            UNITY_INSTANCING_CBUFFER_END
           
            v2f vert(appdata v)
            {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                //UNITY_TRANSFER_INSTANCE_ID(v, o); // necessary only if you want to access instanced properties in the fragment Shader.

                o.vertex = UnityObjectToClipPos(v.vertex);

				float2 offset = UNITY_ACCESS_INSTANCED_PROP(_Offset) / _PaletteSize;
				o.uv = v.uv + offset;
                return o;
            }
           
            fixed4 frag(v2f i) : SV_Target
            {
                //UNITY_SETUP_INSTANCE_ID(i); // necessary only if any instanced properties are going to be accessed in the fragment Shader.
				return tex2D(_MainTex, i.uv) * _AmbientColor;
            }
            ENDCG
        }
    }
}