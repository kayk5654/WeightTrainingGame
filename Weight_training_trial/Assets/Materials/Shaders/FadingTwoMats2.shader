// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// normal mapping reference: https://qiita.com/edo_m18/items/458e3b1992ef8a3f37a5
// https://qiita.com/sune2/items/fa5d50d9ea9bd48761b2

Shader "WeightTrainingGame/FadingTwoMats2"
{
	Properties
	{
		_MainTex ("Main Texture 1", 2D) = "white" {}
		_Color ("Color", COLOR) = (1, 1, 1, 1)
		_Mask ("Mask", 2D) = "black" {}
		_MainTex2 ("Main Texture 2", 2D) = "black" {}
		_Normal ("Normal Map", 2D) = "bump" {}
		_MaskPhase("Mask Phase", Float) = 0
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True" "Queue" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On
		LOD 100

		// base material
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha vertex:vert

		struct Input {
			float2 uv_MainTex;
			float2 uv_Normal;
			float3 pos;
		};

		sampler2D _MainTex2;
		sampler2D _Normal;
		half _Glossiness;
		half _Metallic;

		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void vert (inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.pos = v.vertex;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex2, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));

			// fade out on the bottom
			c.a = IN.pos.y < 0 ? saturate(c.a + IN.pos.y * 1.2) : c.a;

			o.Alpha = c.a;

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}

		ENDCG

		// unlit graphic pattern
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			float4x4 InvTangentMatrix(float3 tan, float3 bin, float3 nor){
				float4x4 mat = float4x4(
					float4(tan, 0), float4(bin, 0), float4(nor, 0), float4(0, 0, 0, 1)
				);

				return transpose(mat);
			}

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD2;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float2 uv2 : TEXCOORD2;
				float4 vertex : SV_POSITION;
				float4 worldSpacePos : TEXCOORD3;
				float3 normal : TEXCOORD4;
				float3 lightDir : TEXCOORD5;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			sampler2D _Mask;
			float4 _Mask_ST;
			float _MaskPhase;
			sampler2D _MainTex2;
			sampler2D _Normal;

			// noise functions
			fixed2 random2(fixed2 st){
				st = fixed2(dot(st, fixed2(127.1, 311.7)), dot(st, fixed2(269.5, 183.3)));
				return -1.0 + 2.0 * frac(sin(st)*43758.5453123);
			}

			float perlinNoise(fixed2 st){
				fixed2 p = floor(st);
				fixed2 f = frac(st);
				fixed2 u = f*f*(3.0 - 2.0 * f);

				float v00 = random2(p+fixed2(0,0));
				float v10 = random2(p+fixed2(1,0));
				float v01 = random2(p+fixed2(0,1));
				float v11 = random2(p+fixed2(1,1));

				return lerp(lerp(dot(v00, f - fixed2(0,0)), dot(v10, f- fixed2(1,0)), u.x),
							lerp(dot(v01, f- fixed2(0,1)), dot(v11, f- fixed2(1,1)), u.x),
							u.y) + 0.5f;
			}

			float fBm(fixed2 st){
				float f = 0;
				fixed2 q = st;

				f += 0.5000 * perlinNoise(q); q = q*2.01;
				f += 0.2500 * perlinNoise(q); q = q*2.02;
				f += 0.1250 * perlinNoise(q); q = q*2.03;
				f += 0.0625 * perlinNoise(q); q = q*2.01;

				return f;
			}

			v2f vert (appdata v)
			{
				v2f o;
				//o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = v.normal;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv2 = TRANSFORM_TEX(v.uv2, _Mask);

				float3 n = normalize(v.normal);
				float3 t = v.tangent;
				float3 b = cross(n, t);

				float3 localLight = mul(unity_WorldToObject, _WorldSpaceLightPos0);
				o.lightDir = mul(localLight, InvTangentMatrix(t, b, n));

				UNITY_TRANSFER_FOG(o,o.vertex);
				o.worldSpacePos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) * _Color;

				// apply alpha of the mask
				// fixed maskAlpha = sin(sqrt(pow(i.worldSpacePos.x, 2) + pow(i.worldSpacePos.z, 2))*10);
				float maskAlpha = saturate(fBm(i.worldSpacePos.xz*4) * sin(sqrt(pow(i.worldSpacePos.x, 2) + pow(i.worldSpacePos.z, 2))*10 + _MaskPhase)+0.15); // noise
				col.a *= maskAlpha;

				// fade out on the bottom
				col.a = i.worldSpacePos.y < 0 ? saturate(col.a + i.worldSpacePos.y * 1.2) : col.a;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
