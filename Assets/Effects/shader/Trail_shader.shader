Shader "Unlit/Trail_shader"
{
    Properties
    {
        _MainTex ("Texture", 2D)    = "white" {}
        _Remap   ("Remap"  , 2D)    = "white" {}
        [HDR]_color   ("color"  , color) = (1,1,1,1)
        _Speed   ("Speed"  , float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque"  "Queue" = "Transparent" "ForceNoShadowCasting" = "True" "IgnoreProjector" = "True"}
        LOD 100
        ZWrite Off

        Pass
        {
            Blend One OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv     : TEXCOORD0;
                float2 uv1    : TEXCOORD1;
                float4 color  : COLOR;
            };

            struct v2f
            {
                float3 uv  : TEXCOORD0;
                float2 uv1 : TEXCOORD2;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float4 color  : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Remap;
            float4 _Remap_ST;
            float  _Speed;
            float4 _color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv.xy  = TRANSFORM_TEX(v.uv.xy, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv1, _Remap);
                o.color = v.color;
                o.uv.z = v.uv.z;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half2  uv          = half2(i.uv.x + _Time.x * _Speed, i.uv.y);
                half4  col         = tex2D(_MainTex, uv);
                half4  remap_col   = tex2D(_Remap, i.uv1);
                half4  final_Color = col * remap_col * _color * i.color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, final_Color);
                return float4(final_Color.xyz, final_Color.r);
            }
            ENDCG
        }
    }
}
