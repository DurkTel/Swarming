// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Diamond"
{
	Properties
	{
		_Color("Color", Color) = (1,0,0,0)
		_RefractTex("RefractTex", CUBE) = "white" {}
		_ReflectTex("ReflectTex", CUBE) = "white" {}
		_RefractIntensity("RefractIntensity", Float) = 2
		_ReflectionStrength("ReflectionStrength", Float) = 1
		_RimBias("RimBias", Float) = 0
		_RimPower("RimPower", Float) = 10
		_RimScale("RimScale", Float) = 10
		_Rimcolor("Rimcolor", Color) = (0,0,0,0)

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" "Queue"="AlphaTest+20" }
	LOD 100

		
		
		Pass
		{
			Stencil
            {
                Ref 3
                Comp GEqual
            }
			Name "Unlit"
			Blend Off
			ZWrite On
			ZTest LEqual
			Cull Front
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _Color;
			uniform samplerCUBE _RefractTex;
			uniform samplerCUBE _ReflectTex;
			uniform float _RefractIntensity;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float3 ase_worldNormal = i.ase_texcoord1.xyz;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldReflection = reflect(-ase_worldViewDir, ase_worldNormal);
				float4 texCUBENode11 = texCUBE( _ReflectTex, ase_worldReflection );
				float4 temp_output_18_0 = ( _Color * texCUBE( _RefractTex, ase_worldReflection ) * texCUBENode11 * _RefractIntensity );
				
				
				finalColor = temp_output_18_0;
				return finalColor;
			}
			ENDCG
		}
		
		Pass
		{
			Stencil
            {
                Ref 3
                Comp GEqual
            }
			Name "Second"
			Blend One One
			ZWrite On
			ZTest LEqual
			Cull Back
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform float4 _Color;
			uniform samplerCUBE _RefractTex;
			uniform samplerCUBE _ReflectTex;
			uniform float _RefractIntensity;
			uniform float _ReflectionStrength;
			uniform float _RimPower;
			uniform float _RimScale;
			uniform float _RimBias;
			uniform float4 _Rimcolor;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord1.xyz = ase_worldNormal;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.w = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float3 ase_worldNormal = i.ase_texcoord1.xyz;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldReflection = reflect(-ase_worldViewDir, ase_worldNormal);
				float4 texCUBENode11 = texCUBE( _ReflectTex, ase_worldReflection );
				float4 temp_output_18_0 = ( _Color * texCUBE( _RefractTex, ase_worldReflection ) * texCUBENode11 * _RefractIntensity );
				float dotResult26 = dot( ase_worldNormal , ase_worldViewDir );
				float clampResult28 = clamp( dotResult26 , 0.0 , 1.0 );
				float temp_output_29_0 = ( 1.0 - clampResult28 );
				float4 temp_output_23_0 = ( temp_output_18_0 + ( texCUBENode11 * _ReflectionStrength * temp_output_29_0 ) );
				float saferPower30 = max( temp_output_29_0 , 0.0001 );
				
				
				finalColor = ( temp_output_23_0 + ( temp_output_23_0 * ( ( ( max( pow( saferPower30 , _RimPower ) , 0.0 ) * _RimScale ) + _RimBias ) * _Rimcolor ) ) );
				return finalColor;
			}
			ENDCG
		}
		
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18800
0;38;1675;571;670.5165;-500.256;1.09532;True;False
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;27;-1054.01,727.7993;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;25;-1077.512,567.7341;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DotProductOpNode;26;-839.9108,568.4504;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;28;-683.074,568.1713;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;29;-541.1039,567.706;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-543.9902,824.4199;Inherit;False;Property;_RimPower;RimPower;6;0;Create;True;0;0;0;False;0;False;10;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;30;-255.2436,678.6926;Inherit;False;True;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldReflectionVector;19;-1055.527,138.2843;Inherit;False;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;34;-101.2896,826.3973;Inherit;False;Property;_RimScale;RimScale;7;0;Create;True;0;0;0;False;0;False;10;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMaxOpNode;32;-85.45314,678.7115;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;109.6039,679.1072;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;104.822,821.6873;Inherit;False;Property;_RimBias;RimBias;5;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-674.9485,197.4914;Inherit;True;Property;_ReflectTex;ReflectTex;2;0;Create;True;0;0;0;False;0;False;-1;bd2d79b4d82156e48b95d07d4c7ddf65;bd2d79b4d82156e48b95d07d4c7ddf65;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;21;-318.2669,247.9692;Inherit;False;Property;_RefractIntensity;RefractIntensity;3;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;10;-677.0698,6.162971;Inherit;True;Property;_RefractTex;RefractTex;1;0;Create;True;0;0;0;False;0;False;-1;575ee2660bd6fc147b498950e0e8c9aa;575ee2660bd6fc147b498950e0e8c9aa;True;0;False;white;LockedToCube;False;Object;-1;Auto;Cube;8;0;SAMPLERCUBE;;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-586.1534,399.3745;Inherit;False;Property;_ReflectionStrength;ReflectionStrength;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;6;-595.074,-165.9315;Inherit;False;Property;_Color;Color;0;0;Create;True;0;0;0;False;0;False;1,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;35;309.8384,679.4491;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-237.0759,-13.3476;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-244.9572,380.7509;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;40;175.0707,920.8592;Inherit;False;Property;_Rimcolor;Rimcolor;8;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;23;73.97684,355.326;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;39;500.3811,718.2249;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;37;468.8581,463.3528;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;20;-2045.612,-105.359;Inherit;False;629;435;Comment;4;12;14;17;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;835.2419,354.9728;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ReflectOpNode;16;-1615.612,2.64097;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;12;-1995.612,-55.35903;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NegateNode;14;-1812.612,-25.35903;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldNormalVector;17;-1917.612,146.641;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;7;0,0;Float;False;True;-1;2;ASEMaterialInspector;100;10;Diamond;2e5224a14043c8f4ca959e941821d11e;True;Unlit;0;0;Unlit;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;False;0;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;2;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;8;1065.726,372.0166;Float;False;False;-1;2;ASEMaterialInspector;100;10;New Amplify Shader;2e5224a14043c8f4ca959e941821d11e;True;Second;0;1;Second;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;False;0;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;True;0;False;-1;True;0;False;-1;False;False;False;0;;0;0;Standard;0;False;0
WireConnection;26;0;25;0
WireConnection;26;1;27;0
WireConnection;28;0;26;0
WireConnection;29;0;28;0
WireConnection;30;0;29;0
WireConnection;30;1;31;0
WireConnection;32;0;30;0
WireConnection;33;0;32;0
WireConnection;33;1;34;0
WireConnection;11;1;19;0
WireConnection;10;1;19;0
WireConnection;35;0;33;0
WireConnection;35;1;36;0
WireConnection;18;0;6;0
WireConnection;18;1;10;0
WireConnection;18;2;11;0
WireConnection;18;3;21;0
WireConnection;22;0;11;0
WireConnection;22;1;24;0
WireConnection;22;2;29;0
WireConnection;23;0;18;0
WireConnection;23;1;22;0
WireConnection;39;0;35;0
WireConnection;39;1;40;0
WireConnection;37;0;23;0
WireConnection;37;1;39;0
WireConnection;38;0;23;0
WireConnection;38;1;37;0
WireConnection;16;0;14;0
WireConnection;16;1;17;0
WireConnection;14;0;12;0
WireConnection;7;0;18;0
WireConnection;8;0;38;0
ASEEND*/
//CHKSM=54724AA53C5E4718D7C00E53CF6E9FE2C89E3171