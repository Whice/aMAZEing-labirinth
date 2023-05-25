Shader "Labirinth/Enviroment/Grass3DShader"
{
    Properties
    {
        //Текстура травы
        _MainTex("Main Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        /// <summary>
        /// Скорость смены угла наклона.
        /// </summary>
        _AngleChangeSpeed("Angle change speed", Range(0, 100)) = 1
        /// <summary>
        /// Высота, с которой будет менятся угол наклона
        /// </summary>
        _AngleChangeHeight("Angle change height", Range(0, 20)) = 1
        /// <summary>
        /// Точка отсчета движения
        /// </summary>
        _MovementReferencePoint("Movement reference point", Range(-3.14, 3.14)) = 1
        /// <summary>
        /// Сила покачивания травы.
        /// </summary>
        _JiggleForce("Jiggle force", Range(0, 2)) = 0.1
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

            //Включить отображение с обеих сторон меша
            Cull Off // Disable backface culling

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_instancing
                #pragma target 3.0
                #include "UnityCG.cginc"

                struct appdata
                {
                    fixed4 vertex : POSITION;
                    fixed2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    fixed2 uv : TEXCOORD0;
                    fixed4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                fixed4 _Color;
                fixed _AngleChangeSpeed;
                fixed _AngleChangeHeight;
                fixed _MovementReferencePoint;
                fixed _JiggleForce;

                v2f vert(appdata v)
                {
                    v2f o;
                    fixed2 uv = v.uv;
                    //Рассчитать угол наклона со временем.
                    fixed deltaAngle = sin(_Time.x * _AngleChangeSpeed + _MovementReferencePoint) * _JiggleForce;

                    //Расчитать налон в зависимости от высоты по uv и скорости движения
                    fixed shift = pow(uv.y, _AngleChangeHeight)* deltaAngle;

                    //Применить сдвиг в координатах мира
                    o.vertex = UnityObjectToClipPos(v.vertex + fixed4(shift, 0, 0, 0));

                    o.uv = uv;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Sample the main texture
                    fixed4 texColor = tex2D(_MainTex, i.uv);

                    fixed4 color = texColor * _Color + 0.1* _Color;
                    // Apply transparency
                    color.a = texColor.a * _Color.a;

                    return color;
                }
            ENDCG
            }
        }
}
