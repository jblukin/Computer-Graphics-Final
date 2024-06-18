Shader "Unlit/TextureMapping"
{
    Properties
    {
        _Density ("Density", Range(2,50)) = 30
        _MainTex ("Main Texture", 2D) = "white" {}
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "bump" {}
        _MainColor ("Ambient Color", COLOR) = (1, 1, 1, 1)
        _SpecularTint ("Specular Tint", COLOR) = (1, 1, 1, 1)
        _Smoothness ("Smoothness", Range(0.001, 1)) = 0.5
        [Gamma] _Metallic ("Metallicness", Range(0.01, 1)) = 0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct VertexIn
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VertexOut
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float _Density;

            VertexOut vert ( VertexIn v )
            {
                VertexOut o;

                o.vertex = UnityObjectToClipPos( v.vertex );

                o.uv = v.uv * _Density;

                return o;
            }
            
            fixed4 frag ( VertexOut o ) : SV_Target
            {
                float2 check = o.uv;
                check = floor( check ) / 2;
                float value = frac( check.x + check.y ) * 2;
                return value;
            }
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
