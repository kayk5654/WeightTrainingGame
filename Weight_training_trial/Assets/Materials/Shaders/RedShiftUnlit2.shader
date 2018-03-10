// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "WeightTrainingGame/RedShiftUnlit2" {
Properties {
    _MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
    _Color ("Color", COLOR) = (1, 1, 1, 1)
	_RedShadeLevel ("Red Shade Level", Range(0.0, 1.0)) = 0.0
}

SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
    LOD 100

    Cull Off
	ZWrite On
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
                float4 worldSpacePos : TEXCOORD2;
            };

            sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _RedShadeLevel;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                o.worldSpacePos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
                col.r = i.worldSpacePos.y < 1 ? saturate(col.r + (1.0 - i.worldSpacePos.y) * _RedShadeLevel): saturate(col.r - i.worldSpacePos.y * 0.16);
				col.gb = i.worldSpacePos.y < 1 ? col.gb * saturate(i.worldSpacePos.y +(1- _RedShadeLevel)): col.gb;
				col.a = i.worldSpacePos.y < 0 ? saturate(col.a + i.worldSpacePos.y * 1.2) : col.a;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
        ENDCG
    }
}

}
