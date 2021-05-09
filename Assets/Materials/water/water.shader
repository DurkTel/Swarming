Shader "Unlit/water"
{
    Properties
    {
        _Depth             ("Depth"              , float) = 1
        _pow               ("pow"                , float) = 1

        _shalllow_water    ("shalllow water"     , color) = (1,1,1,1)
        _Deep_water        ("Deep water"         , color) = (0,0,0,0)

        _NoiseMap          ("Noise Map"          , 2D)    = "black" {}
        _ShoreRange        ("ShoreRange"         , float) = 1
        _Speed             ("Speed"              , float) = 1
        _Speed2            ("Speed2"             , float) = 1

        _NormalMap         ("Normal Map"         , 2D)    = "bump"  {}
        _WarpInt           ("WarpInt"            , float) = 1
        _WarpMidVal        ("_WarpMidVal"        , float) = 0


        _FoamColor         ("FoamColor"          , color) = (1,1,1,1)


        _fresnel_color     ("fresnel color"      , color) = (1,1,1,1)
        _fresnelpow        ("fresnelpow"         , float) = 1

        _WorleyNoiseSize   ("Worley Noise Size"  , float) = 1
        _CausticsRange     ("Caustics Range"     , float) = 1
        _CausticsInt       ("Caustics Int"       , float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        LOD 100
        GrabPass{"_RefractionTex"}

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
                float4 vertex  : POSITION;
                float2 uv      : TEXCOORD0;
                float3 normal  : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv           : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex       : SV_POSITION;
                float4 screen_pos   : TEXCOORD2;
                float4 ScreenPos    : TEXCOORD3;
                float2 noise_uv     : TEXCOORD4;
                float2 normal_uv    : TEXCOORD5;
                 float2 normal_uv2   : TEXCOORD6;
                float3 normal_dir   : TEXCOORD7;
                float3 pos_world    : TEXCOORD8;
                float3 tangent_dir  : TEXCOORD9;
                float3 binormal_dir : TEXCOORD10;
            };

            float  _ShoreRange;
            float4 _FoamColor;

            float  _Depth;
            float  _pow;

            sampler2D _NoiseMap;
            float4 _NoiseMap_ST;
            float4 _shalllow_water;
            float4 _Deep_water;
            float  _Speed;
            
             float  _Speed2;
             float  _WarpInt;
             float  _WarpMidVal;

            sampler2D _NormalMap;
            float4 _NormalMap_ST;

            float4 _LightColor0;

            float4 _fresnel_color;
            float  _fresnelpow;

            float  _WorleyNoiseSize;

            sampler2D _RefractionTex;
            float4 _RefractionTex_TexelSize;

            sampler2D _CameraDepthTexture;

            float  _CausticsRange;
            float  _CausticsInt;
            

            float2 random(float2 p) {
				p = float2(dot(p, float2(127.1, 311.7)),
					dot(p, float2(269.5, 183.3)));
 
				return frac(sin(p)*43758.5453123);
			}

            //生成worleyNoise
			float worleyNoise(float2 uv) 
            {
				float2 i = floor(uv);
				float2 f = frac(uv);
				float m_dist =1;
				float2 m_point;
				for (int x = -1; x < 2;x++) 
                {
					for (int y = -1; y < 2; y++) 
                    {
						float2 neighbor = float2(x, y);
						float2 neighborP = random(i + neighbor) ;
						neighborP = 0.5 + 0.5*cos(_Time.y + 6.2831*neighborP);
						float dist = distance(f,neighborP+ neighbor);
						if (dist<m_dist) 
                        {
							m_dist = dist;
							m_point = neighborP;
						}
					}
				}
				return m_dist;
			}

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex     = UnityObjectToClipPos(v.vertex);
                o.noise_uv   = TRANSFORM_TEX(v.uv, _NoiseMap);
                o.normal_uv  = TRANSFORM_TEX(v.uv, _NormalMap);
                o.normal_uv2 = TRANSFORM_TEX(v.uv, _NormalMap);

                o.normal_dir   = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                o.tangent_dir  = normalize(mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0.0)).xyz);
                o.binormal_dir = normalize(cross(o.normal_dir, o.tangent_dir)) * v.tangent.w;
                o.pos_world    = mul(unity_ObjectToWorld, v.vertex).xyz;

                UNITY_TRANSFER_FOG(o,o.vertex);

                o.screen_pos = o.vertex;
                o.screen_pos.y = -o.screen_pos.y;
                o.ScreenPos  = ComputeScreenPos(o.screen_pos);


                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                 half4 normalmap     = tex2D(_NormalMap,i.normal_uv  + _Time.y * _Speed);
                 half4 normalmap2    = tex2D(_NormalMap,i.normal_uv2 + _Time.y * _Speed2);
                 half3 ver_warp1     = normalmap.rgb;
                 half3 ver_warp2     = normalmap2.rgb;

                half3 normal_data   = UnpackNormal(normalmap);
                half3 normal_dir    = normalize(i.normal_dir);
                half3 tangent_dir   = normalize(i.tangent_dir);
                half3 binormal_dir  = normalize(i.binormal_dir);

                normal_dir = normalize(tangent_dir * normal_data.x + binormal_dir * normal_data.y + normal_dir * normal_data.z);

                half3 view_dir      = normalize(_WorldSpaceCameraPos.xyz - i.pos_world);
                half3 light_dir     = normalize(_WorldSpaceLightPos0.xyz);
                half3 reflect_dir   = reflect(-light_dir,normal_dir);

                //phong
                half  RdotV         = dot(reflect_dir, view_dir);
                half3 spec_color    = step(0.9, max(0.0, RdotV)) ;

                //fresnel
                half  VdotN         = dot(view_dir, normal_dir);
                half  fresnel       = step(_fresnelpow, max(0, 1 - VdotN));
                half3 fresnel_color = max(0.0, fresnel) * _fresnel_color.xyz;

                //深度差
                half2 screen_uv       = i.screen_pos.xy / i.screen_pos.w;
                      screen_uv       = (screen_uv + 1.0) * 0.5;
                half  screne_depth    = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screen_uv));
                half  surfaceDepth    = UNITY_Z_0_FAR_FROM_CLIPSPACE(i.ScreenPos.z);
                half  waterDepth      = saturate(abs((screne_depth - surfaceDepth) / _Depth));
                half4 waterDepthColor = lerp(_shalllow_water, _Deep_water, pow(waterDepth, _pow));

                //近岸泡沫
                half  shore   = waterDepth * 2 * _ShoreRange;
                half4 Noise   = tex2D(_NoiseMap, i.noise_uv + _Time.y * _Speed);
                half  foam    = step(shore, Noise.x);
                half3 foamCol = foam * _FoamColor.a;
    
                //扰动混合
                
                 half2 warp   = (ver_warp1.xy - _WarpMidVal) * _WarpInt +
                                (ver_warp2.xy - _WarpMidVal) * _WarpInt;
                 half2 warpUV = screen_uv + warp;

                //焦散
                half3 viewVector      = mul(unity_CameraInvProjection,float4(warpUV * 2 - 1 , 0 , -1));
                half3 viewVectordir   = normalize(mul(unity_CameraToWorld, viewVector));
                half3 screen_worldpos = viewVectordir * screne_depth + _WorldSpaceCameraPos.xyz;
                //生成worleyNoise
                float3 worleynoise    = max(0, worleyNoise(screen_worldpos.xz * _WorleyNoiseSize));

                //折射
                half3 var_RefractionTex = tex2D(_RefractionTex, warpUV).rgb;
                half3 Refraction        = lerp(var_RefractionTex, waterDepthColor, waterDepthColor.a);

                half3 Caustics          = (((1 - waterDepth) * pow(worleynoise, _CausticsRange)) / _CausticsInt + Refraction);

                half4 finalColor2 = float4(lerp(Caustics, waterDepthColor.rgb, waterDepthColor.a), waterDepthColor.a) + float4(foamCol, _FoamColor.a) + float4(fresnel_color, fresnel_color.r) + float4(spec_color * 0.3,1);

                UNITY_APPLY_FOG(i.fogCoord, finalColor2);

                return finalColor2;
            }
            ENDCG
        }
    }
}
