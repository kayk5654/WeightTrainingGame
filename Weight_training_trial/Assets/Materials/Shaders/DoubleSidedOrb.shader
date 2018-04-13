Shader "WeightTrainingGame/DoubleSidedOrb" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BaseAlpha ("Base Alpha", Range(0, 0.3)) = 0.1
		_RimPower ("Rim Power", Range(0.5, 5)) = 1
		_AlphaMul ("Alpha Multiplier", Range(0, 2)) = 2
		_Cube("Reflection Map", CUBE) = "" {}
		_Reflection("Reflection Level", Range(0, 1)) = 0
	}
	SubShader {
		Tags { "Queue"="Transparent" }

		ZWrite On
		Cull Off
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha:fade vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _BaseAlpha;
		float _RimPower;
		float _AlphaMul;
		samplerCUBE _Cube;
		float _Reflection;
		fixed4 _Color;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
			float3 worldRefl;
			float3 pos;
		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void vert (inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.pos = v.vertex;
		}

		float rand(float3 co){
			return frac(sin(dot(co.xyz, float3(12.9898, 78.233, 56.787))) * 43758.5433);
		}

		float noise(float3 pos){
			float3 ip = floor(pos);
			float3 fp = smoothstep(0, 1, frac(pos));

			float4 a = float4(
				rand(ip + float3(0, 0, 0)),
				rand(ip + float3(1, 0, 0)),
				rand(ip + float3(0, 1, 0)),
				rand(ip + float3(1, 1, 0)));

			float4 b = float4(
				rand(ip + float3(0, 0, 1)),
				rand(ip + float3(1, 0, 1)),
				rand(ip + float3(0, 1, 1)),
				rand(ip + float3(1, 1, 1)));

			a = lerp(a, b, fp.z);
			a.xy = lerp(a.xy, a.zw, fp.y);
			return lerp(a.x, a.y, fp.x);
		}

		float perlin(float3 pos){
			return (noise(pos*16) * 32 +
				noise(pos * 32) * 16 +
				noise(pos * 64) * 8 +
				noise(pos * 128) * 4 +
				noise(pos * 256) * 2 +
				noise(pos * 512)) / 63;
		}

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = _Color;
			float3 noisePos = float3(IN.pos.x + _Time.x, IN.pos.y + _Time.x, IN.pos.z + _Time.x);
			c.a = pow(perlin(IN.pos), 2);
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 r = texCUBE(_Cube, IN.worldRefl) * _Reflection;
			o.Emission = c.rgb + r.rgb;

			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));

			o.Alpha = c.a * _AlphaMul * pow(rim, _RimPower)+ _BaseAlpha;
			//o.Alpha = dot (normalize(IN.viewDir), o.Normal) < 0.1 ? ;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
