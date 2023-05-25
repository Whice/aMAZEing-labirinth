Shader "Labirinth/Enviroment/PlaneGrass3DShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _HeightTexture("HeightTexture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        
        /// <summary>
        /// Высота травы
        /// </summary>
        _Height("Height", Range(0, 3)) = 1
    }
    SubShader
    {
        //Учесть наличие прозрачности у текстуры
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
                fixed4 vertex : POSITION;
                fixed2 uv : TEXCOORD0;
            };

            struct v2f
            {
                fixed2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                fixed4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _HeightTexture;
            fixed4 _MainTex_ST;
            fixed4 _Height_ST;
                fixed _Height;
                fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                fixed2 uv = v.uv;
                fixed2 uv2 = _Height_ST.xy;

                fixed4 hCol = tex2Dlod(_HeightTexture, fixed4(v.uv, 0, 0));

                //h = h / 2;
                fixed h = hCol.r * _Height;
                
                //Применить сдвиг в координатах мира
                o.vertex = UnityObjectToClipPos(v.vertex + fixed4(0, h, 0, 0));

                o.uv = uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                col.rgb = col.rgb*_Color.rgb;
                col.a *= _Color.a;

                return col;
            }
            ENDCG
        }
    }
}
