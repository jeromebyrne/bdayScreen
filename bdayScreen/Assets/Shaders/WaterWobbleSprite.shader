Shader "BdayScreen/WaterWobbleSprite"
{
    Properties
    {
        [MainTexture] _MainTex("Sprite Texture", 2D) = "white" {}
        [MainColor] _Color("Tint", Color) = (1,1,1,1)
        _NoiseTex("Noise Texture", 2D) = "gray" {}
        _Distortion("Distortion Strength", Range(0, 0.1)) = 0.02
        _NoiseScale("Noise Scale", Vector) = (1,1,0,0)
        _NoiseSpeed("Noise Speed", Vector) = (0.2,0.1,0,0)
        _UVScale("UV Scale", Vector) = (1,1,0,0)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderPipeline" = "UniversalPipeline"
            "CanUseSpriteAtlas" = "True"
            "UniversalMaterialType" = "Unlit"
            "PreviewType" = "Plane"
        }

        Pass
        {
            Name "SpriteUnlit"
            Tags { "LightMode" = "Universal2D" }

            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;
            TEXTURE2D(_NoiseTex);
            SAMPLER(sampler_NoiseTex);
            float4 _NoiseTex_ST;
            float _Distortion;
            float4 _NoiseScale;
            float4 _NoiseSpeed;
            float4 _UVScale;

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex) * _UVScale.xy;
                output.color = input.color * _Color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uv = input.uv;
                float2 noiseUv = TRANSFORM_TEX(input.uv, _NoiseTex) * _NoiseScale.xy;
                noiseUv += _Time.y * _NoiseSpeed.xy;
                noiseUv = frac(noiseUv);
                float noise = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, noiseUv).r * 2.0 - 1.0;
                uv.x += noise * _Distortion;

                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv) * input.color;
                return col;
            }
            ENDHLSL
        }
    }
}
