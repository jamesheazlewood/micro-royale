Shader "Hidden/Custom/Microscope"
{
    HLSLINCLUDE
    // StdLib.hlsl holds pre-configured vertex shaders (VertDefault), varying structs 
    // (VaryingsDefault), and most of the data you need to write common effects.
    #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

    TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

    // Lerp the pixel color with the luminance using the _Blend uniform.
    float _Blend;
    float _Spread;
    float4 _MainTex_TexelSize; // Automatically provided by Unity
    float4 Frag(VaryingsDefault i) : SV_Target
    {
        float2 uv = i.texcoord;
        float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
        float2 texelSize = _MainTex_TexelSize.xy * _Spread;

        // Compute the luminance for the current pixel
        // float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));

        // color.rgb = lerp(color.rgb, luminance.xxx, _Blend.xxx);
        // color.rgb = color.xxx;

        // float4 color = tex2D(_MainTex, uv) * 0.2;
        color -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - float2(texelSize.x * 0.5, 0)) * _Blend;
        color -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv - float2(texelSize.x, 0)) * _Blend;
        color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x * 0.5, 0)) * _Blend;
        color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv + float2(texelSize.x, 0)) * _Blend;

        // Return the result
        return color;
    }

    // float4 frag (v2f i) : SV_Target {
    //     float2 uv = i.uv;
    //     float2 texelSize = _MainTex_TexelSize.xy * _Spread;

    //     float4 color = tex2D(_MainTex, uv) * 0.2;
    //     color += tex2D(_MainTex, uv + float2(texelSize.x, 0)) * 0.2;
    //     color += tex2D(_MainTex, uv - float2(texelSize.x, 0)) * 0.2;
    //     color += tex2D(_MainTex, uv + float2(0, texelSize.y)) * 0.2;
    //     color += tex2D(_MainTex, uv - float2(0, texelSize.y)) * 0.2;

    //     return color;
    // }

    ENDHLSL

    SubShader {
        Cull Off ZWrite Off ZTest Always
        Pass {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}
