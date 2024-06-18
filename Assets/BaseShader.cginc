#if !defined(BASE_SHADER_INCLUDED)
#define BASE_SHADER_INCLUDED

#include "UnityStandardBRDF.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"

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

float4 _MainColor;
float4 _SpecularTint;
float _Smoothness;
float _Metallic;

VertexOut vert ( VertexIn v )
{

    VertexOut o;

    o.uv = v.uv;
    
    o.normal = UnityObjectToWorldNormal( v.normal );

    o.worldPos = mul( unity_ObjectToWorld, v.vertex );

    o.vertex = UnityObjectToClipPos( v.vertex );

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



#endif