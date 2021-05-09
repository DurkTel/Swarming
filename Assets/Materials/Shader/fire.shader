// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "fire"
{
	Properties
	{
		_Tintcolor("Tintcolor", Color) = (0.8980392,0.02745098,0.01960784,0)
		_GradientEndControl("GradientEndControl", Float) = 1.9
		_Noise("Noise", 2D) = "white" {}
		_Gradlent("Gradlent", 2D) = "white" {}
		_EmissIntensity("EmissIntensity", Float) = 3.51
		_EndMiss("EndMiss", Float) = 3
		_Softness("Softness", Range( 0 , 1)) = 0
		_speed("speed", Float) = -1
		_mask("mask", 2D) = "white" {}
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_NoiseIntensity("NoiseIntensity", Float) = 0.2
		_sharpen("sharpen", Float) = 2.3
		_Fire_heart("Fire_heart", Float) = 0.51
		_Float3("Float 3", Float) = 0.06
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform float4 _Tintcolor;
		uniform float _EmissIntensity;
		uniform float _EndMiss;
		uniform sampler2D _TextureSample0;
		uniform sampler2D _Noise;
		uniform float _speed;
		uniform float4 _Noise_ST;
		uniform float _NoiseIntensity;
		uniform sampler2D _Gradlent;
		uniform float4 _Gradlent_ST;
		uniform float _GradientEndControl;
		uniform float _Fire_heart;
		uniform float _Float3;
		uniform float _Softness;
		uniform sampler2D _mask;
		uniform float _sharpen;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 break33 = ( _Tintcolor * _EmissIntensity );
			float2 appendResult87 = (float2(2.0 , 2.0));
			float2 uv_TexCoord85 = i.uv_texcoord * appendResult87;
			float2 appendResult8 = (float2(0.0 , _speed));
			float2 uv_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float2 panner7 = ( 1.0 * _Time.y * appendResult8 + uv_Noise);
			float Noise21 = tex2D( _Noise, panner7 ).r;
			float2 uv_Gradlent = i.uv_texcoord * _Gradlent_ST.xy + _Gradlent_ST.zw;
			float4 tex2DNode13 = tex2D( _Gradlent, uv_Gradlent );
			float GradientEnd29 = ( ( 1.0 - tex2DNode13.r ) * _GradientEndControl );
			float temp_output_52_0 = ( ( ( Noise21 * 2.0 ) - 1.0 ) * _NoiseIntensity * GradientEnd29 );
			float2 appendResult91 = (float2(( -0.5 + uv_TexCoord85.x + temp_output_52_0 ) , uv_TexCoord85.y));
			float4 tex2DNode84 = tex2D( _TextureSample0, appendResult91 );
			float clampResult95 = clamp( ( ( tex2DNode84.r * tex2DNode84.r ) * 2.3 ) , 0.0 , 1.0 );
			float eeeee96 = clampResult95;
			float4 appendResult34 = (float4(break33.r , ( break33.g + ( _EndMiss * step( 0.04 , ( 1.0 - eeeee96 ) ) * step( _Fire_heart , Noise21 ) * step( _Float3 , GradientEnd29 ) ) ) , break33.b , 0.0));
			o.Emission = appendResult34.xyz;
			float clampResult19 = clamp( ( Noise21 - _Softness ) , 0.0 , 1.0 );
			float Gradlent20 = tex2DNode13.r;
			float smoothstepResult15 = smoothstep( clampResult19 , Noise21 , Gradlent20);
			float2 appendResult51 = (float2(( i.uv_texcoord.x + temp_output_52_0 ) , i.uv_texcoord.y));
			float4 tex2DNode46 = tex2D( _mask, appendResult51 );
			float clampResult62 = clamp( ( ( tex2DNode46.r * tex2DNode46.r ) * _sharpen ) , 0.0 , 1.0 );
			float FireShape63 = clampResult62;
			o.Alpha = ( smoothstepResult15 * FireShape63 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Unlit alpha:fade keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18800
42;366;1675;583;3270.922;201.8036;1.793459;True;False
Node;AmplifyShaderEditor.CommentaryNode;25;-2918.085,-330.8087;Inherit;False;1207.203;737.7701;GradientAndNoise;14;21;4;7;5;8;10;9;20;29;30;31;39;13;14;GradientAndNoise;0,0.5760117,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2872.306,205.9715;Inherit;False;Constant;_Float1;Float 1;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-2868.462,289.1226;Inherit;False;Property;_speed;speed;7;0;Create;True;0;0;0;False;0;False;-1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-2767.267,61.50582;Inherit;False;0;4;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;8;-2687.406,210.5596;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;14;-2879.393,-256.144;Inherit;False;0;13;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;7;-2493.451,66.0585;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;-2276.88,37.17693;Inherit;True;Property;_Noise;Noise;2;0;Create;True;0;0;0;False;0;False;-1;0a0a9a7a4e998044298a642dea69972c;0a0a9a7a4e998044298a642dea69972c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;13;-2643.969,-280.8089;Inherit;True;Property;_Gradlent;Gradlent;3;0;Create;True;0;0;0;False;0;False;-1;9971192f55af76841b5df6f3b43e70a8;9971192f55af76841b5df6f3b43e70a8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;21;-1955.68,61.31989;Inherit;False;Noise;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-2549.949,-75.38705;Inherit;False;Property;_GradientEndControl;GradientEndControl;1;0;Create;True;0;0;0;False;0;False;1.9;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;65;-2922.899,474.7844;Inherit;False;2220.616;487.3514;Shape;17;56;43;58;55;57;53;54;52;47;50;51;46;61;59;60;62;63;Shape;1,0,0,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;39;-2305.531,-163.5677;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2862.406,742.0441;Inherit;False;Constant;_Float0;Float 0;10;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-2872.899,641.2501;Inherit;False;21;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-2133.196,-95.38707;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;58;-2674.932,742.0438;Inherit;False;Constant;_Float2;Float 2;10;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;55;-2677.772,646.8874;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;-1955.95,-101.3871;Inherit;True;GradientEnd;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-2093.813,1026.443;Inherit;False;Constant;_Float5;Float 5;14;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;87;-1947.821,1020.457;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;53;-2524.179,771.535;Inherit;False;Property;_NoiseIntensity;NoiseIntensity;10;0;Create;True;0;0;0;False;0;False;0.2;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;-2527.423,846.1356;Inherit;False;29;GradientEnd;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;57;-2510.183,646.8874;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;85;-1797.709,980.7977;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;52;-2285.065,753.5577;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-1704.9,904.3821;Inherit;False;Constant;_Float6;Float 6;14;0;Create;True;0;0;0;False;0;False;-0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-1519.419,971.3948;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;91;-1356.675,1003.704;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;84;-1208.889,974.1107;Inherit;True;Property;_TextureSample0;Texture Sample 0;9;0;Create;True;0;0;0;False;0;False;-1;d440ba0457a84fa4c8af475b1008984f;d440ba0457a84fa4c8af475b1008984f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;47;-2357.927,529.6931;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;94;-896.4775,1151.714;Inherit;False;Constant;_Float7;Float 7;14;0;Create;True;0;0;0;False;0;False;2.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;92;-888.54,991.5779;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-2071.299,533.3401;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-716.975,993.1967;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;51;-1898.4,554.1411;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ClampOpNode;95;-560.089,992.4901;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;96;-304.4704,986.6359;Inherit;False;eeeee;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;46;-1735.713,524.7843;Inherit;True;Property;_mask;mask;8;0;Create;True;0;0;0;False;0;False;-1;d440ba0457a84fa4c8af475b1008984f;d440ba0457a84fa4c8af475b1008984f;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;61;-1496.206,745.0299;Inherit;False;Property;_sharpen;sharpen;11;0;Create;True;0;0;0;False;0;False;2.3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;-1637.642,-63.42952;Inherit;False;96;eeeee;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-1424.711,540.5843;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1178.594,-177.2947;Inherit;False;Property;_EmissIntensity;EmissIntensity;4;0;Create;True;0;0;0;False;0;False;3.51;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;119;-1019.282,162.2189;Inherit;False;Property;_Float3;Float 3;13;0;Create;True;0;0;0;False;0;False;0.06;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;-1057.931,254.6403;Inherit;False;29;GradientEnd;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;98;-1433.493,-57.78443;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;23;-628.2128,141.223;Inherit;False;21;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;-736.7932,238.1055;Inherit;False;Property;_Softness;Softness;6;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;-1044.851,76.20931;Inherit;False;21;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;60;-1251.115,540.1712;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;121;-1387.28,-149.3701;Inherit;False;Constant;_Float4;Float 4;14;0;Create;True;0;0;0;False;0;False;0.04;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-1014.285,-1.308044;Inherit;False;Property;_Fire_heart;Fire_heart;12;0;Create;True;0;0;0;False;0;False;0.51;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-1202.878,-370.7109;Inherit;False;Property;_Tintcolor;Tintcolor;0;0;Create;True;0;0;0;False;0;False;0.8980392,0.02745098,0.01960784,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;18;-416.2043,218.7495;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;72;-834.2137,3.476654;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;20;-1955.85,-257.0334;Inherit;False;Gradlent;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-849.5991,-169.1665;Inherit;False;Property;_EndMiss;EndMiss;5;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-919.0403,-364.8487;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;118;-832.224,141.3562;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;120;-1218.107,-72.61543;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;62;-1092.912,540.1832;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;22;-277.5746,122.9459;Inherit;False;20;Gradlent;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;19;-253.2274,219.0754;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;38;-666.5238,-103.2828;Inherit;True;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;63;-925.2838,535.0554;Inherit;True;FireShape;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;33;-657.244,-364.6534;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.GetLocalVarNode;24;-277.2119,355.238;Inherit;False;21;Noise;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;36;-494.9074,-250.6726;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;15;-31.82159,196.255;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;64;-26.8372,456.7868;Inherit;False;63;FireShape;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;34;-258.9812,-363.5024;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;223.1152,343.7655;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;486.4899,-409.9714;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;fire;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;8;0;10;0
WireConnection;8;1;9;0
WireConnection;7;0;5;0
WireConnection;7;2;8;0
WireConnection;4;1;7;0
WireConnection;13;1;14;0
WireConnection;21;0;4;1
WireConnection;39;0;13;1
WireConnection;30;0;39;0
WireConnection;30;1;31;0
WireConnection;55;0;43;0
WireConnection;55;1;56;0
WireConnection;29;0;30;0
WireConnection;87;0;86;0
WireConnection;87;1;86;0
WireConnection;57;0;55;0
WireConnection;57;1;58;0
WireConnection;85;0;87;0
WireConnection;52;0;57;0
WireConnection;52;1;53;0
WireConnection;52;2;54;0
WireConnection;89;0;90;0
WireConnection;89;1;85;1
WireConnection;89;2;52;0
WireConnection;91;0;89;0
WireConnection;91;1;85;2
WireConnection;84;1;91;0
WireConnection;92;0;84;1
WireConnection;92;1;84;1
WireConnection;50;0;47;1
WireConnection;50;1;52;0
WireConnection;93;0;92;0
WireConnection;93;1;94;0
WireConnection;51;0;50;0
WireConnection;51;1;47;2
WireConnection;95;0;93;0
WireConnection;96;0;95;0
WireConnection;46;1;51;0
WireConnection;59;0;46;1
WireConnection;59;1;46;1
WireConnection;98;0;97;0
WireConnection;60;0;59;0
WireConnection;60;1;61;0
WireConnection;18;0;23;0
WireConnection;18;1;17;0
WireConnection;72;0;73;0
WireConnection;72;1;40;0
WireConnection;20;0;13;1
WireConnection;27;0;12;0
WireConnection;27;1;28;0
WireConnection;118;0;119;0
WireConnection;118;1;37;0
WireConnection;120;0;121;0
WireConnection;120;1;98;0
WireConnection;62;0;60;0
WireConnection;19;0;18;0
WireConnection;38;0;35;0
WireConnection;38;1;120;0
WireConnection;38;2;72;0
WireConnection;38;3;118;0
WireConnection;63;0;62;0
WireConnection;33;0;27;0
WireConnection;36;0;33;1
WireConnection;36;1;38;0
WireConnection;15;0;22;0
WireConnection;15;1;19;0
WireConnection;15;2;24;0
WireConnection;34;0;33;0
WireConnection;34;1;36;0
WireConnection;34;2;33;2
WireConnection;45;0;15;0
WireConnection;45;1;64;0
WireConnection;0;2;34;0
WireConnection;0;9;45;0
ASEEND*/
//CHKSM=378E928B7C0206FFE40F3FBB813761475CA4E23C