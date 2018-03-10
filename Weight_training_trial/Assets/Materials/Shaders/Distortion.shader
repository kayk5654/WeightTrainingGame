// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "WeightTrainingGame/Distortion"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", COLOR) = (1, 1, 1, 1)
		[Toggle] _DistortionType ("Distortion Type", Float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue" = "Geometry" "LightMode" = "ForwardBase"}
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
			
			#include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 diff: COLOR0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				SHADOW_COORDS(1)
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _DistortionType;
			
			v2f vert (appdata v)
			{
				v2f o;

				//float4 worldPos = mul(UNITY_MATRIX_MVP, v.vertex);
				float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
				//float dist = sqrt(pow(worldpos.x, 2), pow(worldpos.y, 2), pow(worldpos.z, 2));
				float dist = length(worldPos);
				float3 vertOffset;
				if(_DistortionType == 0){ // get more gravity
					
					vertOffset = (0,0,0);
				} else { // get less gravity
					vertOffset = (1,1,1);
				}
				worldPos.xyz += vertOffset;
				//o.vertex = worldPos;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o.uv = TRANSFORM_TEX(v.texcoord.xy, _MainTex);
				half3 worldNormal = UnityObjectToWorldNormal(v.normal);
				half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
				o.diff = nl * _LightColor0;

				UNITY_TRANSFER_FOG(o,o.vertex);

				TRANSFER_SHADOW(o)
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;
				fixed shadow = SHADOW_ATTENUATION(i);
				col.rgb *= i.diff * shadow;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}

		Pass
			{
				Tags {"LightMode" = "ShadowCaster"}

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_shadowcaster
				#include "UnityCG.cginc"

				struct appdata {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
				};

				struct v2f {
					V2F_SHADOW_CASTER;
				};

				v2f vert (appdata v)
				{
					v2f o;
					TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
					return o;
				}

				float4 frag(v2f i) : SV_Target
				{
					SHADOW_CASTER_FRAGMENT(i)
				}

				ENDCG
			}
	}
}
