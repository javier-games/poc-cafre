Shader "Custom/BasicShader"{
	Properties{
		_Color("Main Color",COLOR) = (1,1,1,1)
	}
	SubShader{
		Tags{
			"Queue"="Transparent"
			"RenderType"="Transparent"
		}
		Pass{
			Blend One One
			Material{
				Diffuse[_Color]
				Ambient[_Color]
			}
			Lighting On
			Cull Off
		}
		
	}
	Fallback "Diffuse"
}