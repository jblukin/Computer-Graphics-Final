Shader "Unlit/TerrainShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "bump" {}
        _MainColor ("Ambient Color", COLOR) = (1, 1, 1, 1)
        _SpecularTint ("Specular Tint", COLOR) = (1, 1, 1, 1)
        _Smoothness ("Smoothness", Range(0.001, 1)) = 0.5
        [Gamma] _Metallic ("Metallicness", Range(0.01, 1)) = 0

        _PhaseOffset("Phase Offset", Float) = 2
        _Speed("Speed", Float) = 0.5
        _Depth("Depth", Float) = 1
        _Smoothing("Smoothing", Range(.01, .9)) = .5
        _XDrift("XDrift", Float) = 0
        _ZDrift("ZDrift", Float) = 1
        _Scale("Scale", Float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {

            Tags { "LightMode" = "ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define FORWARD_BASE_PASS

            #include "UnityStandardBRDF.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"

            struct VertexIn
            {

                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;

            };

            struct VertexOut
            {

                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;

            };

            float _PhaseOffset;
            float _Speed;
            float _Depth;
            float _Smoothing;
            float _XDrift;
            float _ZDrift;
            float _Scale;

            float4 _MainColor;
            float4 _SpecularTint;
            float _Smoothness;
            float _Metallic;

            VertexOut vert ( VertexIn v )
            {

                VertexOut o;

                o.uv = v.uv;

                float3 v0 = v.vertex.xyz; 

                float3 arbitraryVecA = v0 + float3( 0.05, 0, 0 );
                float3 arbitraryVecB = v0 + float3( 0, 0, 0.05 ); 
    
                float phaseOffsetA  = _PhaseOffset * ( 3.14 * 2 ); 
                float phaseOffsetB = _PhaseOffset * ( 3.14 * 1.1 ); 

                float speedA  = _Time.y * _Speed; 
                float speedB = _Time.y * ( _Speed * 0.5 ); 

                float _DepthB = _Depth * 1.25; 
                float arbVecA_Noise = v0.x * _XDrift + v0.z * _ZDrift;
                float arbVecB_Noise = arbitraryVecA.x * _XDrift + arbitraryVecA.z * _ZDrift;
                float arbVecC_Noise = arbitraryVecB.x * _XDrift + arbitraryVecB.z * _ZDrift;
 
                v0.y += sin( phaseOffsetA  + speedA  + ( v0.x  * _Scale  ) ) * _Depth; 
                v0.y += sin( phaseOffsetB + speedB + ( arbVecA_Noise * _Scale  ) ) * _DepthB; //Extra Complexity Wave
                
                arbitraryVecA.y += sin( phaseOffsetA  + speedA  + ( arbitraryVecA.x  * _Scale  ) ) * _Depth; 
                arbitraryVecA.y += sin( phaseOffsetB + speedB + ( arbVecB_Noise * _Scale  ) ) * _DepthB; 
                
                arbitraryVecB.y += sin( phaseOffsetA  + speedA  + ( arbitraryVecB.x  * _Scale  ) ) * _Depth; 
                arbitraryVecB.y += sin( phaseOffsetB + speedB + ( arbVecC_Noise * _Scale  ) ) * _DepthB;
                
                arbitraryVecA.y -= ( arbitraryVecA.y - v0.y ) * _Smoothing; 
                arbitraryVecB.y -= ( arbitraryVecB.y - v0.y ) * _Smoothing; 
            
                // Solve for corrected normal
                float3 normal = cross( arbitraryVecB-v0, arbitraryVecA-v0 ); 

                v.normal = normalize( normal );

                o.normal = UnityObjectToWorldNormal( v.normal );

                o.vertex = UnityObjectToClipPos( v0.xyz );

                o.worldPos = mul( unity_ObjectToWorld, o.vertex );

                return o;


            }

            fixed4 frag ( VertexOut o ) : SV_Target
            {

                float3 vertex = o.vertex.xyz;
                float normal = normalize( o.normal );

                //Point Light Attenuation and Direction Calc
                float3 lightDir = normalize( _WorldSpaceLightPos0.xyz - o.worldPos );
                UNITY_LIGHT_ATTENUATION( attenuation, 0, o.worldPos );

                float3 lightColor = _LightColor0.rgb * attenuation;

                //Diffuse
                float falloff = DotClamped( normalize( lightDir ), normal );
                float3 diffuse = lightColor * falloff;

                //Specular
                float3 viewDir = normalize( _WorldSpaceCameraPos - o.worldPos );
                float3 reflectionDir = reflect( -lightDir, o.normal );
                float3 specular = _Metallic * _SpecularTint.rgb * lightColor * pow( DotClamped( viewDir, reflectionDir ), _Smoothness * 10 );

                //Ambient
                float3 ambient = _MainColor.rgb;

                //Combine
                return float4( ambient + diffuse + specular, 0 );
            
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

            #include "UnityStandardBRDF.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"

            struct VertexIn
            {

                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;

            };

            struct VertexOut
            {

                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 worldPos : TEXCOORD2;

            };

            float _PhaseOffset;
            float _Speed;
            float _Depth;
            float _Smoothing;
            float _XDrift;
            float _ZDrift;
            float _Scale;

            float4 _MainColor;
            float4 _SpecularTint;
            float _Smoothness;
            float _Metallic;

			VertexOut vert ( VertexIn v )
            {

                VertexOut o;

                o.uv = v.uv;

                float3 v0 = v.vertex.xyz; 

                float3 arbitraryVecA = v0 + float3( 0.05, 0, 0 );
                float3 arbitraryVecB = v0 + float3( 0, 0, 0.05 ); 
    
                float phaseOffsetA  = _PhaseOffset * ( 3.14 * 2 ); 
                float phaseOffsetB = _PhaseOffset * ( 3.14 * 1.1 ); 

                float speedA  = _Time.y * _Speed; 
                float speedB = _Time.y * ( _Speed * 0.5 ); 

                float _DepthB = _Depth * 2.0; 
                float arbVecA_Noise = v0.x * _XDrift + v0.z * _ZDrift;
                float arbVecB_Noise = arbitraryVecA.x * _XDrift + arbitraryVecA.z * _ZDrift;
                float arbVecC_Noise = arbitraryVecB.x * _XDrift + arbitraryVecB.z * _ZDrift;
 
                v0.y += sin( phaseOffsetA  + speedA  + ( v0.x  * _Scale  ) ) * _Depth; 
                v0.y += sin( phaseOffsetB + speedB + ( arbVecA_Noise * _Scale  ) ) * _DepthB; //Extra Complexity Wave
                
                arbitraryVecA.y += sin( phaseOffsetA  + speedA  + ( arbitraryVecA.x  * _Scale  ) ) * _Depth; 
                arbitraryVecA.y += sin( phaseOffsetB + speedB + ( arbVecB_Noise * _Scale  ) ) * _DepthB; 
                
                arbitraryVecB.y += sin( phaseOffsetA  + speedA  + ( arbitraryVecB.x  * _Scale  ) ) * _Depth; 
                arbitraryVecB.y += sin( phaseOffsetB + speedB + ( arbVecC_Noise * _Scale  ) ) * _DepthB;
                
                arbitraryVecA.y -= ( arbitraryVecA.y - v0.y ) * _Smoothing; 
                arbitraryVecB.y -= ( arbitraryVecB.y - v0.y ) * _Smoothing; 
            
                // Solve for corrected normal
                float3 normal = cross( arbitraryVecB-v0, arbitraryVecA-v0 ); 

                v.normal = normalize( normal );

                o.normal = UnityObjectToWorldNormal( v.normal );

                o.vertex = UnityObjectToClipPos( v0.xyz );

                o.worldPos = mul( unity_ObjectToWorld, o.vertex );

                return o;

            }

            fixed4 frag ( VertexOut o ) : SV_Target
            {

                float3 vertex = o.vertex.xyz;
                float normal = normalize( o.normal );

                //Point Light Attenuation and Direction Calc
                float3 lightDir = normalize( _WorldSpaceLightPos0.xyz - o.worldPos );
                UNITY_LIGHT_ATTENUATION( attenuation, 0, o.worldPos );

                float3 lightColor = _LightColor0.rgb * attenuation;

                //Diffuse
                float falloff = DotClamped( normalize( lightDir ), normal );
                float3 diffuse = lightColor * falloff;

                //Specular
                float3 viewDir = normalize( _WorldSpaceCameraPos - o.worldPos );
                float3 reflectionDir = reflect( -lightDir, o.normal );
                float3 specular = _Metallic * _SpecularTint.rgb * lightColor * pow( DotClamped( viewDir, reflectionDir ), _Smoothness * 10 );

                //Ambient
                float3 ambient = _MainColor.rgb;

                //Combine
                return float4( ambient + diffuse + specular, 0 );
            
            }

			ENDCG

		}
        
    }
}
