Shader "ZooWorld/VertexColorSurface"
{
    Properties
    {
        _Glossiness ("Smoothness", Range(0, 1)) = 0.05
        _Metallic ("Metallic", Range(0, 1)) = 0
        _DetailScale ("Detail Scale", Float) = 4
        _DetailStrength ("Detail Strength", Range(0, 1)) = 0.4
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float4 color : COLOR;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        float _DetailScale;
        half _DetailStrength;

        float Hash(float2 p)
        {
            float3 p3 = frac(float3(p.x, p.y, p.x) * 0.1031);
            p3 += dot(p3, p3.yzx + 33.33);
            return frac((p3.x + p3.y) * p3.z);
        }

        float ValueNoise(float2 p)
        {
            float2 cell = floor(p);
            float2 blend = frac(p);
            blend = blend * blend * (3.0 - 2.0 * blend);

            float bottom = lerp(Hash(cell), Hash(cell + float2(1, 0)), blend.x);
            float top = lerp(Hash(cell + float2(0, 1)), Hash(cell + float2(1, 1)), blend.x);
            return lerp(bottom, top, blend.y);
        }

        float Detail(float2 p)
        {
            float broad = ValueNoise(p);
            float medium = ValueNoise(p * 2.7 + 17.3);
            float fine = ValueNoise(p * 7.1 + 41.9);
            float combined = broad * 0.45 + medium * 0.3 + fine * 0.25;
            return smoothstep(0.25, 0.75, combined);
        }

        void surf(Input input, inout SurfaceOutputStandard output)
        {
            float detail = Detail(input.worldPos.xz * _DetailScale);
            float shade = lerp(1.0 - _DetailStrength, 1.0 + _DetailStrength, detail);

            output.Albedo = input.color.rgb * shade;
            output.Metallic = _Metallic;
            output.Smoothness = _Glossiness;
            output.Alpha = 1;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
