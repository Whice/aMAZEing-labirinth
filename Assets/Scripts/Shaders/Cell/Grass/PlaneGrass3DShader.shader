Shader "Labirinth/Enviroment/PlaneGrass3DShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Height("Height", 2D) = "white" {}
    }
    SubShader
    {
        //”честь наличие прозрачности у текстуры
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }
            ZWrite Off // Disable writing to the depth buffer
            Blend SrcAlpha OneMinusSrcAlpha // Use standard alpha blending
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _Height;
            float4 _MainTex_ST;
            float4 _Height_ST;

            v2f vert (appdata v)
            {
                v2f o;
                fixed2 uv = v.uv;
                fixed2 uv2 = _Height_ST.xy;

                fixed4 hCol = tex2Dlod(_Height, fixed4(v.uv, 0, 0));

                //h = h / 2;
                fixed h = hCol.r * 5;
                
                //ѕрименить сдвиг в координатах мира
                o.vertex = UnityObjectToClipPos(v.vertex + fixed4(0, h, 0, 0));

                o.uv = uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col2 = tex2Dlod(_Height, fixed4(i.uv, 0, 0));
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
