Shader "Unlit/EmissiveShader_eye"
{
    Properties
    {
        [HDR]_Color   ("Color"  , color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"  "Queue"="AlphaTest+20"}
        LOD 100

        Pass
        {
            Stencil
            {
                Ref 3
                Comp GEqual
            }
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

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv     = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 final_Color = _Color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, final_Color);
                return float4(final_Color);
            }
            ENDCG
        }
    }
}
