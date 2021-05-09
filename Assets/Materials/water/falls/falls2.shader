Shader "Unlit/falls2"
{
    Properties
    {
        _MainTex    ("Texture"    , 2D)    = "white" {}
        _fallsColor ("fallsColor" , color) = (0,0,0,0)
        _frothColor ("froth"      , color) = (0,0,0,0)
        _Speed      ("Speed"      , float) = 0
        _Cutout     ("Cutout"     , float) = 0
        _intensity  ("intensity"  , float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        LOD 100

        Pass
        {
            Blend One OneMinusSrcAlpha
            Cull Off
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
            float4 _MainTex_ST;

            float4 _fallsColor;
            float4 _frothColor;

            float  _Speed;
            float  _Cutout;
            float  _intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture

                float2 uv          = float2(i.uv.x , i.uv.y + _Time.x * -_Speed);
                half4  col         = tex2D(_MainTex, uv);

                half   color       = step(_Cutout , col);

                half4 final_Color     = _fallsColor + color * _frothColor * _intensity;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, final_Color);
                return final_Color;
            }
            ENDCG
        }
    }
}
