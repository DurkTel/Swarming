Shader "Unlit/waterfall"
{
    Properties
    {
        _MainTex            ("Texture"           , 2D)         = "white" {}
        _Mask               ("Mask"              , 2D)         = "white" {}
        [HDR]_Emissivecolor ("Emissivecolor"     , color)      = (0,0,0,0)
        _Alpha              ("Alpha"             , range(0,0.4)) = 0
        _Cutout             ("Cutout"            , float)      = 0
        _Speed              ("Speed"             , float)      = 1
        _intensity          ("intensity"         , float)      = 0
    }
    SubShader
    {
        LOD 100

        Pass
        {
            Tags{ "RenderType" = "Opaque"  "Queue" = "AlphaTest"}
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
                float2 uv1 : TEXCOORD1;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Emissivecolor;
            float _Cutout;
            float _Alpha;
            sampler2D _Mask;
            float4 _Mask_ST;
            float  _Speed;
            float  _intensity;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1    = TRANSFORM_TEX(v.uv1, _Mask);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float2 uv          = float2(i.uv.x + _Time.x * _Speed, i.uv.y);
                half4  col         = tex2D(_MainTex, uv);
                half4  Mask        = tex2D(_Mask   , i.uv1);
                half   color       = step(_Cutout , col);
                half   color_R     = col.r * _Alpha * Mask;
                half4  final_Color = color * _intensity * _Emissivecolor;
                clip(color_R - _Cutout);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, final_Color);
                return final_Color;
            }
            ENDCG
        }
    }
}
