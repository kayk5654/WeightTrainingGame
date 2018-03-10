Shader "WeightTrainingGame/DoubleSidedOrb" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_BaseAlpha ("Base Alpha", Range(0, 0.3)) = 0.1
		_RimPower ("Rim Power", Range(0.5, 5)) = 1
		_AlphaMul ("Alpha Multiplier", Range(0, 2)) = 2
	}
	SubShader {
		Tags { "Queue"="Transparent" }

		ZWrite On
		Cull Off
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		float _BaseAlpha;
		float _RimPower;
		float _AlphaMul;

		struct Input {
			float2 uv_MainTex;
			float3 viewDir;
		};

		fixed4 _Color;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Emission = c.rgb;

			half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));

			o.Alpha = c.a * _AlphaMul * pow(rim, _RimPower)+ _BaseAlpha;
			//o.Alpha = pow(rim, _RimPower);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
