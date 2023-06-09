// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/Crystal"
{
	Properties
	{
		_AlbedoTransparency("AlbedoTransparency", 2D) = "white" {}
		_AlbedoColorTint("Albedo Color Tint", Color) = (1,1,1,1)
		_Normal("Normal", 2D) = "bump" {}
		_MetallicSmoothness("MetallicSmoothness", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_FinalColor("Final Color", Color) = (1,0,0,1)
		_FinalPower("Final Power", Range( 0 , 90)) = 2
		[Toggle(_RAMPENABLED_ON)] _RampEnabled("Ramp Enabled", Float) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_RampColorTint("Ramp Color Tint", Color) = (1,1,1,1)
		_RampRemapMax("Ramp Remap Max", Range( 0 , 1)) = 0.33
		_RampOffsetExp("Ramp Offset Exp", Range( 0.2 , 8)) = 1
		[Toggle(_INNERRIMENABLED_ON)] _InnerRimEnabled("Inner Rim Enabled", Float) = 0
		_InnerRimHackNormals("Inner Rim Hack Normals", Range( 0 , 1)) = 1
		_InnerRimExp("Inner Rim Exp", Range( 0.1 , 16)) = 4
		_InnerRimMask("Inner Rim Mask", 2D) = "white" {}
		[Toggle(_INNERRIMMASKFLIP_ON)] _InnerRimMaskFlip("Inner Rim Mask Flip", Float) = 0
		_InnerRimMaskExp("Inner Rim Mask Exp", Range( 0.1 , 4)) = 1
		_InnerRimMaskNegate("Inner Rim Mask Negate", Range( 0 , 1)) = 0
		[Toggle(_INNERRIMPROFILEENABLED_ON)] _InnerRimProfileEnabled("Inner Rim Profile Enabled", Float) = 0
		_InnerRimProfile("Inner Rim Profile", 2D) = "white" {}
		[Toggle(_VERTICALGRADIENTENABLED_ON)] _VerticalGradientEnabled("Vertical Gradient Enabled", Float) = 0
		[Toggle(_VERTICALGRADIENTWORLDPOSITION_ON)] _VerticalGradientWorldPosition("Vertical Gradient World Position", Float) = 0
		_VerticalGradientFlipSwitch("Vertical Gradient Flip Switch", Range( 0 , 1)) = 0
		_VerticalGradientRemapMax("Vertical Gradient Remap Max", Float) = 1
		_VerticalGradientOffset("Vertical Gradient Offset", Float) = 0
		_VerticalGradientExp("Vertical Gradient Exp", Range( 0.1 , 12)) = 1
		[Toggle(_APPEARGRADIENTENABLED_ON)] _AppearGradientEnabled("Appear Gradient Enabled", Float) = 0
		_AppearGradientFlipSwitch("Appear Gradient Flip Switch", Range( 0 , 1)) = 1
		_AppearGradientAbsSwitch("Appear Gradient Abs Switch", Range( 0 , 1)) = 1
		_AppearGradientRemapMax("Appear Gradient Remap Max", Float) = 4
		_AppearGradientOffset("Appear Gradient Offset", Float) = 1.25
		_AppearGradientExp("Appear Gradient Exp", Range( 0.1 , 12)) = 4
		_AppearGradientPower("Appear Gradient Power", Range( 0 , 10)) = 1
		_AppearVertexYOffset("Appear Vertex Y Offset", Float) = 0
		[Toggle(_PARALLAXNOISEENABLED_ON)] _ParallaxNoiseEnabled("Parallax Noise Enabled", Float) = 0
		_ParallaxNoise("Parallax Noise", 2D) = "white" {}
		_ParallaxNoiseScaleU("Parallax Noise Scale U", Float) = 1
		_ParallaxNoiseScaleV("Parallax Noise Scale V", Float) = 1
		_ParallaxNoiseDepth("Parallax Noise Depth", Float) = 0
		_ParallaxNoiseNegate("Parallax Noise Negate", Range( 0 , 1)) = 0
		_EmissionVSSwitch("Emission VS Switch", Range( 0 , 1)) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _RAMPENABLED_ON
		#pragma shader_feature _INNERRIMENABLED_ON
		#pragma shader_feature _INNERRIMPROFILEENABLED_ON
		#pragma shader_feature _INNERRIMMASKFLIP_ON
		#pragma shader_feature _PARALLAXNOISEENABLED_ON
		#pragma shader_feature _VERTICALGRADIENTENABLED_ON
		#pragma shader_feature _VERTICALGRADIENTWORLDPOSITION_ON
		#pragma shader_feature _APPEARGRADIENTENABLED_ON
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
			float3 viewDir;
			float4 uv_tex4coord;
		};

		uniform float _AppearVertexYOffset;
		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _AlbedoTransparency;
		uniform float4 _AlbedoTransparency_ST;
		uniform float4 _AlbedoColorTint;
		uniform float _FinalPower;
		uniform float4 _FinalColor;
		uniform float _InnerRimHackNormals;
		uniform float _InnerRimExp;
		uniform sampler2D _InnerRimProfile;
		uniform sampler2D _InnerRimMask;
		uniform float4 _InnerRimMask_ST;
		uniform float _InnerRimMaskExp;
		uniform float _InnerRimMaskNegate;
		uniform sampler2D _ParallaxNoise;
		uniform float4 _ParallaxNoise_ST;
		uniform float _ParallaxNoiseScaleU;
		uniform float _ParallaxNoiseScaleV;
		uniform float _ParallaxNoiseDepth;
		uniform float _ParallaxNoiseNegate;
		uniform float _VerticalGradientOffset;
		uniform float _VerticalGradientRemapMax;
		uniform float _VerticalGradientFlipSwitch;
		uniform float _VerticalGradientExp;
		uniform float _AppearGradientAbsSwitch;
		uniform float _AppearGradientOffset;
		uniform float _AppearGradientRemapMax;
		uniform float _AppearGradientFlipSwitch;
		uniform float _AppearGradientExp;
		uniform float _AppearGradientPower;
		uniform sampler2D _Ramp;
		uniform float _RampRemapMax;
		uniform float _RampOffsetExp;
		uniform float4 _RampColorTint;
		uniform float _EmissionVSSwitch;
		uniform sampler2D _MetallicSmoothness;
		uniform float4 _MetallicSmoothness_ST;
		uniform float _Metallic;
		uniform float _Smoothness;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			v.vertex.xyz += ( float3(0,1,0) * _AppearVertexYOffset );
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode11 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			o.Normal = tex2DNode11;
			float2 uv_AlbedoTransparency = i.uv_texcoord * _AlbedoTransparency_ST.xy + _AlbedoTransparency_ST.zw;
			o.Albedo = ( tex2D( _AlbedoTransparency, uv_AlbedoTransparency ) * _AlbedoColorTint ).rgb;
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 lerpResult32 = lerp( (WorldNormalVector( i , tex2DNode11 )) , ase_worldNormal , _InnerRimHackNormals);
			float3 normalizeResult167 = normalize( lerpResult32 );
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 normalizeResult168 = normalize( ase_worldViewDir );
			float dotResult41 = dot( normalizeResult167 , normalizeResult168 );
			float clampResult51 = clamp( dotResult41 , 0.0 , 1.0 );
			float temp_output_42_0 = pow( clampResult51 , _InnerRimExp );
			float2 appendResult61 = (float2(( 1.0 - temp_output_42_0 ) , 0.0));
			#ifdef _INNERRIMPROFILEENABLED_ON
				float staticSwitch60 = tex2D( _InnerRimProfile, appendResult61 ).r;
			#else
				float staticSwitch60 = temp_output_42_0;
			#endif
			float2 uv_InnerRimMask = i.uv_texcoord * _InnerRimMask_ST.xy + _InnerRimMask_ST.zw;
			float clampResult57 = clamp( ( pow( tex2D( _InnerRimMask, uv_InnerRimMask ).r , _InnerRimMaskExp ) + _InnerRimMaskNegate ) , 0.0 , 1.0 );
			#ifdef _INNERRIMMASKFLIP_ON
				float staticSwitch80 = ( 1.0 - clampResult57 );
			#else
				float staticSwitch80 = clampResult57;
			#endif
			float2 uv0_ParallaxNoise = i.uv_texcoord * _ParallaxNoise_ST.xy + _ParallaxNoise_ST.zw;
			float2 appendResult127 = (float2(_ParallaxNoiseScaleU , _ParallaxNoiseScaleV));
			float2 temp_output_124_0 = ( uv0_ParallaxNoise * appendResult127 );
			float2 paralaxOffset44 = ParallaxOffset( tex2D( _ParallaxNoise, temp_output_124_0 ).r , _ParallaxNoiseDepth , i.viewDir );
			float4 tex2DNode64 = tex2D( _ParallaxNoise, ( temp_output_124_0 + paralaxOffset44 ) );
			float clampResult78 = clamp( ( tex2DNode64.r + _ParallaxNoiseNegate ) , 0.0 , 1.0 );
			#ifdef _PARALLAXNOISEENABLED_ON
				float staticSwitch74 = clampResult78;
			#else
				float staticSwitch74 = 1.0;
			#endif
			#ifdef _INNERRIMENABLED_ON
				float staticSwitch101 = ( staticSwitch60 * staticSwitch80 * staticSwitch74 );
			#else
				float staticSwitch101 = 0.0;
			#endif
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			#ifdef _VERTICALGRADIENTWORLDPOSITION_ON
				float staticSwitch113 = ase_worldPos.y;
			#else
				float staticSwitch113 = ase_vertex3Pos.y;
			#endif
			float clampResult94 = clamp( (0.0 + (( staticSwitch113 + _VerticalGradientOffset ) - 0.0) * (1.0 - 0.0) / (_VerticalGradientRemapMax - 0.0)) , 0.0 , 1.0 );
			float lerpResult135 = lerp( clampResult94 , ( 1.0 - clampResult94 ) , round( _VerticalGradientFlipSwitch ));
			#ifdef _VERTICALGRADIENTENABLED_ON
				float staticSwitch90 = ( staticSwitch74 * pow( lerpResult135 , _VerticalGradientExp ) * staticSwitch80 );
			#else
				float staticSwitch90 = 0.0;
			#endif
			float lerpResult161 = lerp( ase_vertex3Pos.y , abs( ase_vertex3Pos.y ) , _AppearGradientAbsSwitch);
			float clampResult143 = clamp( (0.0 + (( lerpResult161 + _AppearGradientOffset ) - 0.0) * (1.0 - 0.0) / (_AppearGradientRemapMax - 0.0)) , 0.0 , 1.0 );
			float lerpResult147 = lerp( clampResult143 , ( 1.0 - clampResult143 ) , round( _AppearGradientFlipSwitch ));
			#ifdef _APPEARGRADIENTENABLED_ON
				float staticSwitch151 = ( pow( lerpResult147 , _AppearGradientExp ) * _AppearGradientPower );
			#else
				float staticSwitch151 = 0.0;
			#endif
			float clampResult86 = clamp( ( staticSwitch101 + staticSwitch90 + staticSwitch151 ) , 0.0 , 1.0 );
			float clampResult173 = clamp( (0.0 + (clampResult86 - 0.0) * (1.0 - 0.0) / (_RampRemapMax - 0.0)) , 0.0 , 1.0 );
			float2 appendResult104 = (float2(( 1.0 - pow( ( 1.0 - clampResult173 ) , _RampOffsetExp ) ) , 0.0));
			#ifdef _RAMPENABLED_ON
				float4 staticSwitch103 = ( tex2D( _Ramp, appendResult104 ) * _RampColorTint * clampResult86 * i.vertexColor );
			#else
				float4 staticSwitch103 = ( _FinalColor * i.vertexColor * clampResult86 );
			#endif
			float lerpResult164 = lerp( 1.0 , i.uv_tex4coord.z , _EmissionVSSwitch);
			o.Emission = ( _FinalPower * staticSwitch103 * lerpResult164 ).rgb;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode12 = tex2D( _MetallicSmoothness, uv_MetallicSmoothness );
			o.Metallic = ( tex2DNode12.r * _Metallic );
			o.Smoothness = ( tex2DNode12.a * _Smoothness );
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows vertex:vertexDataFunc 

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
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 customPack2 : TEXCOORD2;
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				half4 color : COLOR0;
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
				vertexDataFunc( v, customInputData );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.uv_tex4coord;
				o.customPack2.xyzw = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.uv_tex4coord = IN.customPack2.xyzw;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.vertexColor = IN.color;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18200
1920;0;1920;1018;2734.84;-1651.852;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;115;1246.629,-473.3899;Inherit;False;379;280;;1;11;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;120;-4153.9,2117.972;Inherit;False;2748.317;721.2605;;17;71;65;66;69;67;44;68;77;64;76;78;74;75;125;126;127;124;Parallax Noise;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;117;-3147.031,215.9133;Inherit;False;2127.925;652.2051;;15;30;33;31;32;40;41;43;51;42;63;61;62;60;168;167;Inner Rim;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;11;1296.629,-423.3899;Inherit;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;False;-1;a687007e1e43afe499e3fea697d19f7f;f7f6323dbdbf5e94e976a65509805436;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;153;-2801.942,4233.446;Inherit;False;2189.573;548.9917;;18;151;158;152;149;159;147;150;146;145;143;148;142;140;144;141;160;139;162;Appear Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldNormalVector;31;-3015.179,415.7224;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;30;-3017.605,265.9133;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;33;-3097.031,566.4465;Inherit;False;Property;_InnerRimHackNormals;Inner Rim Hack Normals;14;0;Create;True;0;0;False;0;False;1;0.9;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;126;-4113.974,2368.502;Inherit;False;Property;_ParallaxNoiseScaleV;Parallax Noise Scale V;39;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;125;-4113.974,2293.502;Inherit;False;Property;_ParallaxNoiseScaleU;Parallax Noise Scale U;38;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-4103.9,2167.972;Inherit;False;0;71;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;139;-2677.88,4317.454;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;118;-2656.696,3289.544;Inherit;False;2046.616;638.0518;;15;87;112;89;113;88;93;92;96;94;95;100;90;97;136;138;Vertical Gradient;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;32;-2732.305,328.6856;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;127;-3865.866,2315.223;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;40;-2757.394,667.3706;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;162;-2759.673,4466.506;Inherit;False;Property;_AppearGradientAbsSwitch;Appear Gradient Abs Switch;30;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;112;-2600.4,3339.544;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.NormalizeNode;167;-2552.433,277.5065;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;87;-2606.696,3526.572;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;71;-3812.564,2610.617;Inherit;True;Property;_ParallaxNoise;Parallax Noise;37;0;Create;True;0;0;False;0;False;None;f9676d1aabdbcd74e953aff6657e2238;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.NormalizeNode;168;-2547.433,719.5065;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-3701.266,2250.911;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;160;-2460.93,4370.691;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;161;-2288.832,4329.223;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;89;-2337.022,3614.223;Inherit;False;Property;_VerticalGradientOffset;Vertical Gradient Offset;26;0;Create;True;0;0;False;0;False;0;1.85;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;113;-2374.4,3444.544;Inherit;False;Property;_VerticalGradientWorldPosition;Vertical Gradient World Position;23;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;119;-2745.415,1213.796;Inherit;False;1367.247;463.679;;8;54;59;58;56;55;57;79;80;Inner Rim Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode;41;-2474.733,488.3918;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-3302.714,2204.58;Inherit;False;Property;_ParallaxNoiseDepth;Parallax Noise Depth;40;0;Create;True;0;0;False;0;False;0;0.175;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;69;-3517.411,2609.232;Inherit;True;Property;_TextureSample0;Texture Sample 0;16;0;Create;True;0;0;False;0;False;-1;None;b5a57847926df5d40a17fe0708045a6b;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;67;-3467.036,2397.568;Inherit;False;Tangent;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;141;-2352.453,4520.702;Inherit;False;Property;_AppearGradientOffset;Appear Gradient Offset;32;0;Create;True;0;0;False;0;False;1.25;1.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;51;-2309.699,404.5264;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ParallaxOffsetHlpNode;44;-2954.334,2353.268;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;140;-2016.315,4444.35;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-2102.914,3733.578;Inherit;False;Property;_VerticalGradientRemapMax;Vertical Gradient Remap Max;25;0;Create;True;0;0;False;0;False;1;4.63;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-2282.114,4641.098;Inherit;False;Property;_AppearGradientRemapMax;Appear Gradient Remap Max;31;0;Create;True;0;0;False;0;False;4;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-2669.733,1455.708;Inherit;False;Property;_InnerRimMaskExp;Inner Rim Mask Exp;18;0;Create;True;0;0;False;0;False;1;0.33;0.1;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;54;-2695.415,1263.796;Inherit;True;Property;_InnerRimMask;Inner Rim Mask;16;0;Create;True;0;0;False;0;False;-1;None;f9676d1aabdbcd74e953aff6657e2238;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-2417.479,587.5233;Inherit;False;Property;_InnerRimExp;Inner Rim Exp;15;0;Create;True;0;0;False;0;False;4;6;0.1;16;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;88;-1939.378,3539.124;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;92;-1761.85,3605.543;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;42;-2098.358,485.0458;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;142;-1853.253,4544.866;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;58;-2361.589,1355.701;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;68;-2690.18,2558.494;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2492.689,1562.475;Inherit;False;Property;_InnerRimMaskNegate;Inner Rim Mask Negate;19;0;Create;True;0;0;False;0;False;0;0.104;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;-2157.516,1447.601;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;64;-2391.155,2230.365;Inherit;True;Property;_Test;Test;15;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;136;-1916.697,3426.694;Inherit;False;Property;_VerticalGradientFlipSwitch;Vertical Gradient Flip Switch;24;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;94;-1579.952,3606.275;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;63;-1979.469,663.8305;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-2368.643,2422.277;Inherit;False;Property;_ParallaxNoiseNegate;Parallax Noise Negate;41;0;Create;True;0;0;False;0;False;0;0.05;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;143;-1671.355,4545.598;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;148;-1864.734,4364.432;Inherit;False;Property;_AppearGradientFlipSwitch;Appear Gradient Flip Switch;29;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;61;-1820.407,662.7053;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RoundOpNode;145;-1536.321,4384.173;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;57;-2027.771,1448.952;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;138;-1646.452,3351.952;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;146;-1567.465,4303.201;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;76;-2055.342,2344.278;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;137;-1615.308,3432.924;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;62;-1671.166,638.1181;Inherit;True;Property;_InnerRimProfile;Inner Rim Profile;21;0;Create;True;0;0;False;0;False;-1;None;014dca088ec93b54486621ab84890b5d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;78;-1935.742,2348.176;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-1929.415,2200.467;Inherit;False;Constant;_Float0;Float 0;19;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1706.952,3812.596;Inherit;False;Property;_VerticalGradientExp;Vertical Gradient Exp;27;0;Create;True;0;0;False;0;False;1;4;0.1;12;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;135;-1462.705,3389.324;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-1488.572,4591.004;Inherit;False;Property;_AppearGradientExp;Appear Gradient Exp;33;0;Create;True;0;0;False;0;False;4;4;0.1;12;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;147;-1383.718,4340.573;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;79;-1872.999,1546.865;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;149;-1160.34,4493.697;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;159;-1245.839,4673.904;Inherit;False;Property;_AppearGradientPower;Appear Gradient Power;34;0;Create;True;0;0;False;0;False;1;1;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;60;-1374.106,482.7619;Inherit;False;Property;_InnerRimProfileEnabled;Inner Rim Profile Enabled;20;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;80;-1689.168,1454.031;Inherit;False;Property;_InnerRimMaskFlip;Inner Rim Mask Flip;17;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;95;-1355.952,3697.275;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;121;-829.7314,1252.343;Inherit;False;872.8072;312.8461;;5;84;86;83;102;101;Mask Compose;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;74;-1744.583,2264.946;Inherit;False;Property;_ParallaxNoiseEnabled;Parallax Noise Enabled;36;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;97;-1173.655,3541.51;Inherit;False;Constant;_Float2;Float 2;26;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;152;-1167.601,4323.766;Inherit;False;Constant;_Float1;Float 1;34;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;158;-930.5948,4577.727;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-779.7314,1409.189;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;102;-778.7266,1302.343;Inherit;False;Constant;_Float4;Float 4;27;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-1161.048,3701.306;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;90;-973.0792,3583.655;Inherit;True;Property;_VerticalGradientEnabled;Vertical Gradient Enabled;22;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;101;-602.5296,1344.004;Inherit;False;Property;_InnerRimEnabled;Inner Rim Enabled;13;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;151;-951.1993,4405.1;Inherit;False;Property;_AppearGradientEnabled;Appear Gradient Enabled;28;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;122;125.8882,791.535;Inherit;False;1787.11;1308.105;;17;123;111;108;50;106;107;105;104;109;110;85;103;48;163;165;169;172;Ramp And Color;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;-303.3699,1347.621;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;86;-131.9242,1344.927;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;175.8882,1639.826;Inherit;False;Property;_RampRemapMax;Ramp Remap Max;11;0;Create;True;0;0;False;0;False;0.33;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;109;468.6046,1577.612;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;173;250.2521,1810.229;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;169;313.7752,1935.226;Inherit;False;Property;_RampOffsetExp;Ramp Offset Exp;12;0;Create;True;0;0;False;0;False;1;1;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;170;391.6423,1810.229;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;171;547.3763,1807.155;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;172;694.9145,1807.156;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;104;661.9661,1575.398;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;50;648.7139,841.535;Inherit;False;Property;_FinalColor;Final Color;6;0;Create;True;0;0;False;0;False;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;123;905.7984,1912.35;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;107;874.2465,1740.108;Inherit;False;Property;_RampColorTint;Ramp Color Tint;10;0;Create;True;0;0;False;0;False;1,1,1,1;1,0.5,0.5,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;111;678.6509,1009.133;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;105;794.968,1549.356;Inherit;True;Property;_Ramp;Ramp;9;0;Create;True;0;0;False;0;False;-1;None;e180a3aaee927d944a12019e96f39a95;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;116;978.2081,-125.2445;Inherit;False;610.463;429.7405;;5;12;15;16;14;13;Metallic Smoothness;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;163;1321.259,1776.32;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;1009.536,931.8994;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;166;1380.623,1700.406;Inherit;False;Constant;_Float3;Float 3;41;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;1252.623,1941.406;Inherit;False;Property;_EmissionVSSwitch;Emission VS Switch;43;0;Create;True;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;114;1009.751,-1013.775;Inherit;False;669.807;462.8423;;3;1;4;2;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;106;1149.291,1619.411;Inherit;False;4;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;48;1387.097,1191.876;Inherit;False;Property;_FinalPower;Final Power;7;0;Create;True;0;0;False;0;False;2;10;0;90;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;1069.612,189.496;Inherit;False;Property;_Smoothness;Smoothness;5;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;103;1403.868,1335.627;Inherit;False;Property;_RampEnabled;Ramp Enabled;8;0;Create;True;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector3Node;157;1656.062,2272.586;Inherit;False;Constant;_Vector0;Vector 0;39;0;Create;True;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;15;1064.593,115.4691;Inherit;False;Property;_Metallic;Metallic;4;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;164;1652.928,1866.607;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;1059.751,-963.775;Inherit;True;Property;_AlbedoTransparency;AlbedoTransparency;0;0;Create;True;0;0;False;0;False;-1;eeee2f2b873f0ef48bfee601a9db2d49;2b979c87ed8cae549a54cbbbc671c425;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;1150.758,-757.9327;Inherit;False;Property;_AlbedoColorTint;Albedo Color Tint;1;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;156;1576.657,2434.499;Inherit;False;Property;_AppearVertexYOffset;Appear Vertex Y Offset;35;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;1028.208,-75.24454;Inherit;True;Property;_MetallicSmoothness;MetallicSmoothness;3;0;Create;True;0;0;False;0;False;-1;a5768a63f6d0d7e48b5f52e1db919297;ce8df99d45333db4694177295420d1be;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;1419.671,0.03743935;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;1444.558,-854.2325;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;1418.416,122.9971;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;1740.338,1246.429;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;175;-2387.862,1911.268;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;174;-2099.686,1939.757;Inherit;False;Property;_ParallaxNoiseFlip;Parallax Noise Flip;42;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;1882.82,2358.509;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2166.489,1043.744;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SineVFX/TopDownEffects/Crystal;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;30;0;11;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;32;2;33;0
WireConnection;127;0;125;0
WireConnection;127;1;126;0
WireConnection;167;0;32;0
WireConnection;168;0;40;0
WireConnection;124;0;65;0
WireConnection;124;1;127;0
WireConnection;160;0;139;2
WireConnection;161;0;139;2
WireConnection;161;1;160;0
WireConnection;161;2;162;0
WireConnection;113;1;87;2
WireConnection;113;0;112;2
WireConnection;41;0;167;0
WireConnection;41;1;168;0
WireConnection;69;0;71;0
WireConnection;69;1;124;0
WireConnection;51;0;41;0
WireConnection;44;0;69;1
WireConnection;44;1;66;0
WireConnection;44;2;67;0
WireConnection;140;0;161;0
WireConnection;140;1;141;0
WireConnection;88;0;113;0
WireConnection;88;1;89;0
WireConnection;92;0;88;0
WireConnection;92;2;93;0
WireConnection;42;0;51;0
WireConnection;42;1;43;0
WireConnection;142;0;140;0
WireConnection;142;2;144;0
WireConnection;58;0;54;1
WireConnection;58;1;59;0
WireConnection;68;0;124;0
WireConnection;68;1;44;0
WireConnection;55;0;58;0
WireConnection;55;1;56;0
WireConnection;64;0;71;0
WireConnection;64;1;68;0
WireConnection;94;0;92;0
WireConnection;63;0;42;0
WireConnection;143;0;142;0
WireConnection;61;0;63;0
WireConnection;145;0;148;0
WireConnection;57;0;55;0
WireConnection;138;0;94;0
WireConnection;146;0;143;0
WireConnection;76;0;64;1
WireConnection;76;1;77;0
WireConnection;137;0;136;0
WireConnection;62;1;61;0
WireConnection;78;0;76;0
WireConnection;135;0;94;0
WireConnection;135;1;138;0
WireConnection;135;2;137;0
WireConnection;147;0;143;0
WireConnection;147;1;146;0
WireConnection;147;2;145;0
WireConnection;79;0;57;0
WireConnection;149;0;147;0
WireConnection;149;1;150;0
WireConnection;60;1;42;0
WireConnection;60;0;62;1
WireConnection;80;1;57;0
WireConnection;80;0;79;0
WireConnection;95;0;135;0
WireConnection;95;1;96;0
WireConnection;74;1;75;0
WireConnection;74;0;78;0
WireConnection;158;0;149;0
WireConnection;158;1;159;0
WireConnection;83;0;60;0
WireConnection;83;1;80;0
WireConnection;83;2;74;0
WireConnection;100;0;74;0
WireConnection;100;1;95;0
WireConnection;100;2;80;0
WireConnection;90;1;97;0
WireConnection;90;0;100;0
WireConnection;101;1;102;0
WireConnection;101;0;83;0
WireConnection;151;1;152;0
WireConnection;151;0;158;0
WireConnection;84;0;101;0
WireConnection;84;1;90;0
WireConnection;84;2;151;0
WireConnection;86;0;84;0
WireConnection;109;0;86;0
WireConnection;109;2;110;0
WireConnection;173;0;109;0
WireConnection;170;0;173;0
WireConnection;171;0;170;0
WireConnection;171;1;169;0
WireConnection;172;0;171;0
WireConnection;104;0;172;0
WireConnection;105;1;104;0
WireConnection;108;0;50;0
WireConnection;108;1;111;0
WireConnection;108;2;86;0
WireConnection;106;0;105;0
WireConnection;106;1;107;0
WireConnection;106;2;86;0
WireConnection;106;3;123;0
WireConnection;103;1;108;0
WireConnection;103;0;106;0
WireConnection;164;0;166;0
WireConnection;164;1;163;3
WireConnection;164;2;165;0
WireConnection;13;0;12;1
WireConnection;13;1;15;0
WireConnection;2;0;1;0
WireConnection;2;1;4;0
WireConnection;14;0;12;4
WireConnection;14;1;16;0
WireConnection;85;0;48;0
WireConnection;85;1;103;0
WireConnection;85;2;164;0
WireConnection;175;0;64;1
WireConnection;174;1;64;1
WireConnection;174;0;175;0
WireConnection;155;0;157;0
WireConnection;155;1;156;0
WireConnection;0;0;2;0
WireConnection;0;1;11;0
WireConnection;0;2;85;0
WireConnection;0;3;13;0
WireConnection;0;4;14;0
WireConnection;0;11;155;0
ASEEND*/
//CHKSM=55CA25C94403C3A1663566E343017EDE92F1C051