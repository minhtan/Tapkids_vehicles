Shader "Custom/Unlit/Color" {
Properties {
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_Color ("Main Color", Color) = (1,1,1,1) 
}

SubShader {
	Tags {"RenderType"="Opaque"}
	LOD 100
	
	ZWrite on

	Pass {
		Lighting Off
		SetTexture [_MainTex] {

            constantColor (0,0,0,1)

            Combine texture * constant, texture * constant 

         } 
	}
}
}