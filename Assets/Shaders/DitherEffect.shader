Shader "Custom/DitherEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Columns("Pixel columns", Float) = 1000
        _Rows("Pixel Rows", Float) = 1000
    }

    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Columns;
            float _Rows;

            fixed4 getClosestCol(fixed4 col, float factor) {

                float r, g, b;

               /** if (col.r > 0.5) {
                    r = 0.75f;
                } else r = 0.25f;

                if (col.g > 0.5) {
                    g = 0.75f;
                }
                else g = 0.25f;

                if (col.b > 0.5) {
                    b = 0.75f;
                }
                else b = 0.25f;*/

                r = col.r * factor;
                r = round(r);
                r = r / factor;

                g = col.g * factor;
                g = round(g);
                g = g / factor;

                b = col.b * factor;
                b = round(b);
                b = b / factor;


                return fixed4(r, g, b, 1.0f);
            }

            fixed4 frag(v2f i) : SV_Target
            {   

               fixed4 col = tex2D(_MainTex, i.uv);

               /* uint mapper[] = { 1, 9, 3, 11,
                                13, 5, 15, 7,
                                4, 12, 2, 10,
                                16, 8, 14, 6 };
                
                uint mapperLength = 4;

                float2 coords = i.uv;
                uint mapX = (int)(coords.y * _Columns) % mapperLength;
                uint mapY = (int)(coords.x * _Rows) % mapperLength;

                //col.rgb = 1 - col.rgb;
                float val = ((col.r + col.g + col.b) / 3.0f * (float)(mapperLength * mapperLength));
                float4 coo = fixed4(col.r, col.g, col.b, 1.0f);
       
                if (val >= mapper[mapY * mapperLength + mapX]) {
                    return fixed4(1.0, 1.0f, 1.0, 1.0f);
                } else return fixed4(0.3f, 0.0f, 0.0f, 1.0f);*/

                
                
                return getClosestCol(col, 10);
            }
            ENDCG
        }
    }
}
