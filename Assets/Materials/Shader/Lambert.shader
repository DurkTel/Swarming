Shader "Unlit/Lambert"
{
    Properties
    {
        _MainTex              ("Texture"             , 2D)         = "white" {}
        _lightMapping         ("lightMapping"        , 2D)         = "black" {}
        _lightMapping_Offset  ("lightMapping_Offset" , range(0,1)) = 0
        _NormalMap            ("Norma Map"           , 2D)         = "bump"  {}
        _AmbientColor         ("AmbientColor"        , color)      = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Tags{"LightMode" = "ForwardBase"}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_fwdbase
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"


            struct appdata
            {
                float4 vertex       : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normal       : NORMAL;
                float4 tangent      : TANGENT;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(3)
                float4 pos          : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 normal_dir   : TEXCOORD1;
                float3 pos_world    : TEXCOORD2;
                float3 tangent_dir  : TEXCOORD4;
                float3 binormal_dir : TEXCOORD5;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _LightColor0;
            float4 _AmbientColor;
            sampler2D _NormalMap;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos          = UnityObjectToClipPos(v.vertex);
                o.uv           = v.uv;
                o.normal_dir   = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                o.tangent_dir  = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.binormal_dir = normalize(cross(o.normal_dir, o.tangent_dir)) * v.tangent.w;
                o.pos_world    = mul(unity_ObjectToWorld, v.vertex).xyz;

                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 base_color    = tex2D(_MainTex, i.uv);
                half4 normalmap     = tex2D(_NormalMap, i.uv);
                half3 normal_data   = UnpackNormal(normalmap);

                //Normalmap
                half3 normal_dir    = normalize(i.normal_dir);
                half3 tangent_dir   = normalize(i.tangent_dir);
                half3 binormal_dir  = normalize(i.binormal_dir);

                normal_dir          = normalize(tangent_dir * normal_data.x + binormal_dir * normal_data.y + normal_dir * normal_data.z);

                half3 view_dir      = normalize(_WorldSpaceCameraPos.xyz - i.pos_world);
                half3 light_dir     = normalize(_WorldSpaceLightPos0.xyz);
                half  NdotL         = dot(normal_dir, light_dir);
                half3 diffuse_color = ((NdotL)*0.5 + 0.5) * _LightColor0.xyz * base_color;

                half3 final_color   = diffuse_color + _AmbientColor;


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, final_color);

                return half4(final_color, 1.0);
            }
            ENDCG
        }
        Pass
        {
            Tags{"LightMode" = "ForwardAdd"}
            Blend One One
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_fwdadd_fullshadows
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"


            struct appdata
            {
                float4 vertex       : POSITION;
                float2 uv           : TEXCOORD0;
                float3 normal       : NORMAL;
                float4 tangent      : TANGENT;
            };

            struct v2f
            {
                UNITY_FOG_COORDS(3)
                float4 pos          : SV_POSITION;
                float2 uv           : TEXCOORD0;
                float3 normal_dir   : TEXCOORD1;
                float3 pos_world    : TEXCOORD2;
                float3 tangent_dir  : TEXCOORD4;
                float3 binormal_dir : TEXCOORD5;
                LIGHTING_COORDS(6, 7)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _LightColor0;
            float4 _AmbientColor;
            sampler2D _lightMapping;
            float4 _lightMapping_ST;
            float  _lightMapping_Offset;
            sampler2D _NormalMap;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos          = UnityObjectToClipPos(v.vertex);
                o.uv           = v.uv;
                o.normal_dir   = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                o.tangent_dir  = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.binormal_dir = normalize(cross(o.normal_dir, o.tangent_dir)) * v.tangent.w;
                o.pos_world    = mul(unity_ObjectToWorld, v.vertex).xyz;

                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half  shadow          = LIGHT_ATTENUATION(i);
                half4 base_color      = tex2D(_MainTex, i.uv);
                half4 normalmap       = tex2D(_NormalMap, i.uv);
                half3 normal_data     = UnpackNormal(normalmap);

                //Normalmap
                half3 normal_dir      = normalize(i.normal_dir);
                half3 tangent_dir     = normalize(i.tangent_dir);
                half3 binormal_dir    = normalize(i.binormal_dir);

                normal_dir            = normalize(tangent_dir * normal_data.x + binormal_dir * normal_data.y + normal_dir * normal_data.z);

                half3 view_dir        = normalize(_WorldSpaceCameraPos.xyz - i.pos_world);

                #if defined (DIRECTIONAL)
                half3 light_dir       = normalize(_WorldSpaceLightPos0.xyz);
                // half  attuenation     = 1.0;

                #elif defined (POINT)
                half3 light_dir       = normalize(_WorldSpaceLightPos0.xyz - i.pos_world);
                // half  distance        = length(_WorldSpaceLightPos0.xyz - i.pos_world);
                // half  range           = 1.0 / unity_WorldToLight[0][0];
                // half  attuenation     = saturate((range - distance) / range);

                #endif

                half  NdotL           = (dot(normal_dir, light_dir) * 0.5 + 0.5);
                half  diff_term       = min(shadow, NdotL);
                half  Lambert_map     = tex2D(_lightMapping, float2(diff_term,_lightMapping_Offset));
                half3 diffuse_color   = Lambert_map * _LightColor0.xyz * base_color;

                // half3 reflect_dir     = reflect(-light_dir,normal_dir);
                // half  RdotV           = dot(reflect_dir, view_dir);
                // half3 spec_color      = pow(max(0.0, RdotV), _Shininess) * _LightColor0.xyz * _SpecIntensity * attuenation;

                half3 final_color     = diffuse_color;


                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, final_color);

                return half4(final_color, 1.0);
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}
