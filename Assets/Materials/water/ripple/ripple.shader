// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader"LO1/ripple"
{
    Properties
    {
        _gradient ("gradient", 2D) = "black"{}
        _NosieTex ("Nosie"  , 2D) = "black"{}
        _Color    ("Color"  , Color) = (1.0,1.0,1.0,1.0)
        _Cutout   ("Cutout" , Range(-1,1.5)) = 0.5
        _Speed    ("Speed"  , Vector) = (0,0,0,0)
        _pow      ("pow"    , float)  = 1
        [Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode",float) = 2
    }
    SubShader
    {
        pass
        {
            //Cull Front
            Cull [_CullMode]
            CGPROGRAM
            #pragma  vertex   vert
            #pragma  fragment frag
            #include "unityCG.cginc"

            sampler2D _gradient;
            float4    _gradient_ST;
            sampler2D _NosieTex;
            float4    _NosieTex_ST;
            float4    _Color;
            float     _Cutout;
            float4    _Speed;
            float     _pow;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
                float2 uv1    : TEXCOORD1;
            };

            struct v2f
            {
                float4 pos    : SV_POSITION;
                float2 uv     : TEXCOORD0;
                float2 uv1    : TEXCOORD1;
            };

            v2f vert(appdata i)
            {
                v2f o;
                o.pos = UnityObjectToClipPos( i.vertex);
                o.uv  = i.uv * _gradient_ST.xy + _gradient_ST.zw ;
                o.uv1  = i.uv1 * _NosieTex_ST.xy +  _NosieTex_ST.zw ;
                return o;
            }

            float4 frag(v2f v) : SV_Target
            {
                float4 col = _Color ;
                float4 gradient = tex2D(_gradient,  v.uv).r;
                float4 nosie = pow(tex2D(_NosieTex, v.uv1 + _Time.y * _Speed.zw).r, _pow);
                clip(gradient * nosie - _Cutout );
                return col ;
            }
            ENDCG
        }
    }
}