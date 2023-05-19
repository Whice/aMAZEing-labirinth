Shader "Labirinth/Enviroment/Grass3DShader"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
        _WaveSpeed("Wave Speed", Range(0, 10)) = 1
        _WaveHeight("Wave Height", Range(0, 1)) = 0.1
    }

        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 100

            Cull Off // Disable backface culling

            ZWrite Off // Disable writing to the depth buffer
            Blend SrcAlpha OneMinusSrcAlpha // Use standard alpha blending

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 3.0
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _WaveSpeed;
                float _WaveHeight;

                // ‘ункци€ дл€ вычислени€ прозрачности текстуры
                fixed CalculateAlpha(fixed2 uv : TEXCOORD0)
                {
                    // ѕолучение вертикальной координаты текстуры
                    fixed v = uv.y;

                    // ¬ычисление прозрачности на основе вертикальной координаты
                    fixed delta = smoothstep(0, 1.0, v);

                    return delta;
                }

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    o.uv = v.uv + fixed2(0,v.uv.y);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Sample the main texture
                    fixed4 texColor = tex2D(_MainTex, i.uv);

                // Apply transparency
                fixed4 color = texColor;
                color.a *= texColor.a;

                return color;
            }
            ENDCG
        }
    }
}
