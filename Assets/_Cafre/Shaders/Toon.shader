// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Toon" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white"{}
		_DiffuseColor ("Diffuse Material Color", Color) = (1,1,1,1)
		_UnlitColor ("Unlit Color", Color) = (0.5,0.5,0.5,1)
		_DiffuseThreshold ("Lighting Threshold", Range(-1.1,1)) = 0.1
		_SpecularColor ("Specular Material Color", Color) = (1,1,1,1)
		_Shininess ("Shininess", Range(0.5,1)) = 1
		_OutlineThickness ("Outline Thickness", Range(0,1)) = 0.1
       
    }
    SubShader {
    	Pass {
    		//	Pass for ambient light and first light source.
    		Tags{ "LightMode" = "ForwardBase" }

    		//	CG Program begins.
    		CGPROGRAM

			    //	Tells the cg to use a vertex-shader called vert.
			    #pragma vertex vert
			    //	And a fragment-shader called frag.
			    #pragma fragment frag
			    //	Including the library UnityCG.
				#include "UnityCG.cginc"

				//	Properties in the current CG context.
			    sampler2D _MainTex;
			    float4 _DiffuseColor;
			    float4 _UnlitColor;
			    float _DiffuseThreshold;
			    float4 _SpecularColor;
			    float _Shininess;
			    float _OutlineThickness;
			 
			    float4 _LightColor0;
			    float4 _MainTex_ST;        

			    //	Structures to comunicate data across the pipe.
			    struct vertexInput {
			       float4 vertex : POSITION;
			       float3 normal : NORMAL;
			       float4 texcoord : TEXCOORD0;
			    };
			    struct vertexOutput {
			        float4 pos : SV_POSITION;
			        float3 normalDir : TEXCOORD1;
			        float4 lightDir : TEXCOORD2;
			        float3 viewDir : TEXCOORD3;
			        float2 uv : TEXCOORD0;
			    };

			    //	Vert Shader
			    vertexOutput vert(vertexInput input,float3 normal : NORMAL){
			        vertexOutput output;
			       
			        //	Normal Direction.
			        output.normalDir = normalize ( UnityObjectToWorldNormal(normal) );
			       
			        //	World position.
			        float4 posWorld = mul(unity_ObjectToWorld, input.vertex);
			       
			        //	View Direction.
			        output.viewDir = normalize( _WorldSpaceCameraPos.xyz - posWorld.xyz ); //vector from object to the camera
			       
			        //	Light Direction.
			        float3 fragmentToLightSource = ( _WorldSpaceCameraPos.xyz - posWorld.xyz);
			        output.lightDir = float4(
			            normalize( lerp(_WorldSpaceLightPos0.xyz , fragmentToLightSource, _WorldSpaceLightPos0.w) ),
			            lerp(1.0 , 1.0/length(fragmentToLightSource), _WorldSpaceLightPos0.w)
			        );
			       
			        //	FragmentInput Output.
			        output.pos = UnityObjectToClipPos( input.vertex );  
			       
			        //	UV-Map.
			        output.uv =input.texcoord;
			       
			        return output;
			     
			    }

			    //	Frag Shader
			    float4 frag(vertexOutput input) : COLOR{

			    	float nDotL = saturate(dot(input.normalDir, input.lightDir.xyz));
			       
					//	Diffuse threshold calculation.
					float diffuseCutoff = saturate( ( max(_DiffuseThreshold, nDotL) - _DiffuseThreshold ) *1000 );
					       
					//	Specular threshold calculation.
					float specularCutoff = saturate( max(_Shininess, dot(reflect(-input.lightDir.xyz, input.normalDir), input.viewDir))-_Shininess ) * 1000;
					       
					//	Calculate Outlines.
					float outlineStrength = saturate( (dot(input.normalDir, input.viewDir ) - _OutlineThickness) * 1000 );

					//	Calculating the Toon Shader.
					float3 ambientLight = (1-diffuseCutoff) * _UnlitColor.xyz * _DiffuseColor.xyz;
					float3 diffuseReflection = (1-specularCutoff) * _DiffuseColor.xyz * diffuseCutoff;
					float3 specularReflection = _SpecularColor.xyz * specularCutoff; 
					float3 combinedLight = (ambientLight + diffuseReflection) * outlineStrength + specularReflection;
					       
					return float4(combinedLight, 1.0) * tex2D(_MainTex, input.uv);
			    }
			   
		    ENDCG
	    }
    }
    Fallback "Diffuse"
}