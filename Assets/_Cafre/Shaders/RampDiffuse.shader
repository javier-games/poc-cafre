Shader "Custom/RampDiffse" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white"{}
		_RampTex("Ramp Texture", 2D) = "white"{}
		_EmissiveColor("Emissive Color",COLOR) = (1,1,1,1)
		_AmbientColor("Ambient Color",COLOR) = (1,1,1,1)
		_SliderValue("Pow",Range(0,10)) = 2.5
	}
	SubShader {

		Tags { "RenderType"="Opaque" }

		//	CG Program begins.
		CGPROGRAM
		//	Tells the cg to use a surface-shader called surf with Lighting Ramp Diffuse.
		#pragma surface surf RampDiffuse

		//	Properties in the current CG context.
		float4 _EmissiveColor;
		float4 _AmbientColor;
		float  _SliderValue;
		sampler2D _RampTex;
		sampler2D _MainTex;

		//	Custom Light
		inline float4 LightingRampDiffuse(SurfaceOutput o ,fixed3 lightDir, fixed atten){
			//	Obtaining lighting according to the angle between normal surface and light direction.
			float hLambert = max(0,dot(o.Normal, lightDir)) * 0.5 + 0.5;
			//	Returning the color mapped from the ramp map.
			float3 ramp = tex2D(_RampTex,float2( hLambert, hLambert)).rgb;
			float4 col;
			col.rgb = o.Albedo * _LightColor0.rgb * ramp;
			col.a = o.Alpha;
			return col;
		}

		//	Structures to comunicate data across the pipe.
		struct Input{
			float2 uv_MainTex;
		};

		//	Surf Shader
		void surf(Input IN, inout SurfaceOutput o){
			//	Applying custom colors.
			float4 c = pow((_EmissiveColor + _AmbientColor), _SliderValue);
			o.Albedo = c.rgb *tex2D (_MainTex, IN.uv_MainTex);
			o.Alpha = c.a;
		}

		ENDCG
	}
	FallBack "Diffuse"
}