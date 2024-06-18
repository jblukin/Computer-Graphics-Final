Shader "Unlit/BaseShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "bump" {}
        _MainColor ("Ambient Color", COLOR) = (1, 1, 1, 1)
        _SpecularTint ("Specular Tint", COLOR) = (1, 1, 1, 1)
        _Smoothness ("Smoothness", Range(0.001, 1)) = 0.5
        [Gamma] _Metallic ("Metallicness", Range(0.01, 1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

       Pass 
       {
			
            Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM

			#pragma target 3.0

			#pragma vertex vert
			#pragma fragment frag

            #define FORWARD_BASE_PASS

			#include "BaseShader.cginc"

			ENDCG

		}

        Pass 
        {
			
            Tags { "LightMode" = "ForwardAdd" }

            Blend One One
            ZWrite Off

			CGPROGRAM

			#pragma target 3.0

			#pragma vertex vert
			#pragma fragment frag

            #pragma multi_compile_fwdadd

			#include "BaseShader.cginc"

			ENDCG

		}
        
    }
}
