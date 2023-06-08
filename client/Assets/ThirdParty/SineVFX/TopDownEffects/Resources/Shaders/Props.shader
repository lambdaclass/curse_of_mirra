// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SineVFX/TopDownEffects/Props"
{
	Properties
	{
		_AlbedoDecalMask("AlbedoDecalMask", 2D) = "white" {}
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_AlbedoColorTint("Albedo Color Tint", Color) = (1,1,1,1)
		_AlbedoDecalColor("Albedo Decal Color", Color) = (1,0,0,1)
		_AlbedoDecalNegate("Albedo Decal Negate", Range( 0 , 1)) = 0
		_Normal("Normal", 2D) = "bump" {}
		_MetallicSmoothness("MetallicSmoothness", 2D) = "white" {}
		_Metallic("Metallic", Range( 0 , 1)) = 1
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_Emission("Emission", 2D) = "white" {}
		_EmissionColor("Emission Color", Color) = (0,0,0,0)
		_EmissionPower("Emission Power", Range( 0 , 60)) = 0
		[Toggle(_RIMEMISSIONENABLED_ON)] _RimEmissionEnabled("Rim Emission Enabled", Float) = 0
		[Toggle(_RIMEMISSIONAFFECTSMASK_ON)] _RimEmissionAffectsMask("Rim Emission Affects Mask", Float) = 0
		_RimEmissionNormal("Rim Emission Normal", Range( 0 , 1)) = 1
		_RimEmissionColor("Rim Emission Color", Color) = (1,0,0,1)
		_RimEmissionExp("Rim Emission Exp", Range( 0.1 , 10)) = 3
		_RimEmissionPower("Rim Emission Power", Range( 0 , 60)) = 6
		_DissolveTexture("Dissolve Texture", 2D) = "white" {}
		_DissolveTextureScaleU("Dissolve Texture Scale U", Float) = 1
		_DissolveTextureScaleV("Dissolve Texture Scale V", Float) = 1
		[Toggle(_DISSOLVETEXTUREFLIP_ON)] _DissolveTextureFlip("Dissolve Texture Flip", Float) = 0
		_DissolveDirectionMask("Dissolve Direction Mask", 2D) = "black" {}
		[Toggle(_DISSOLVEDIRECTIONMASKFLIP_ON)] _DissolveDirectionMaskFlip("Dissolve Direction Mask Flip", Float) = 0
		_DissolveDirectionScale("Dissolve Direction Scale", Range( 0 , 4)) = 1
		_DissolveEmissionOffset("Dissolve Emission Offset", Float) = 0
		_DissolveEmissionColor("Dissolve Emission Color", Color) = (1,0,0,1)
		_DissolveEmissionPower("Dissolve Emission Power", Range( 0 , 80)) = 1
		_DissolveEmissionRemapMax("Dissolve Emission Remap Max", Range( 0 , 1)) = 0.25
		_DissolveEmissionExp("Dissolve Emission Exp", Range( 0.2 , 8)) = 1
		[Toggle(_DISSOLVEMANUALCONTROL_ON)] _DissolveManualControl("Dissolve Manual Control", Float) = 0
		_DissolveManualProgress("Dissolve Manual Progress", Range( -2 , 2)) = 0
		[Toggle(_EMISSIONVSCONTROLENABLED_ON)] _EmissionVSControlEnabled("Emission VS Control Enabled", Float) = 0
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature _EMISSIONVSCONTROLENABLED_ON
		#pragma shader_feature _RIMEMISSIONAFFECTSMASK_ON
		#pragma shader_feature _RIMEMISSIONENABLED_ON
		#pragma shader_feature _DISSOLVETEXTUREFLIP_ON
		#pragma shader_feature _DISSOLVEMANUALCONTROL_ON
		#pragma shader_feature _DISSOLVEDIRECTIONMASKFLIP_ON
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
			half ASEVFace : VFACE;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
			float4 vertexColor : COLOR;
			float4 uv_tex4coord;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _AlbedoDecalMask;
		uniform float4 _AlbedoDecalMask_ST;
		uniform float4 _AlbedoDecalColor;
		uniform float _AlbedoDecalNegate;
		uniform float4 _AlbedoColorTint;
		uniform sampler2D _Emission;
		uniform float4 _Emission_ST;
		uniform float4 _EmissionColor;
		uniform float _EmissionPower;
		uniform float _RimEmissionNormal;
		uniform float _RimEmissionExp;
		uniform float4 _RimEmissionColor;
		uniform float _RimEmissionPower;
		uniform float _DissolveEmissionPower;
		uniform sampler2D _DissolveTexture;
		uniform float4 _DissolveTexture_ST;
		uniform float _DissolveTextureScaleU;
		uniform float _DissolveTextureScaleV;
		uniform float _DissolveManualProgress;
		uniform sampler2D _DissolveDirectionMask;
		uniform float4 _DissolveDirectionMask_ST;
		uniform float _DissolveDirectionScale;
		uniform float _DissolveEmissionOffset;
		uniform float _DissolveEmissionRemapMax;
		uniform float _DissolveEmissionExp;
		uniform float4 _DissolveEmissionColor;
		uniform sampler2D _MetallicSmoothness;
		uniform float4 _MetallicSmoothness_ST;
		uniform float _Metallic;
		uniform float _Smoothness;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 tex2DNode11 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			float3 switchResult90 = (((i.ASEVFace>0)?(( tex2DNode11 * float3(1,1,1) )):(( tex2DNode11 * float3(-1,-1,-1) ))));
			o.Normal = switchResult90;
			float2 uv_AlbedoDecalMask = i.uv_texcoord * _AlbedoDecalMask_ST.xy + _AlbedoDecalMask_ST.zw;
			float4 tex2DNode1 = tex2D( _AlbedoDecalMask, uv_AlbedoDecalMask );
			float4 lerpResult21 = lerp( tex2DNode1 , _AlbedoDecalColor , ( tex2DNode1.a * ( 1.0 - _AlbedoDecalNegate ) ));
			o.Albedo = ( lerpResult21 * _AlbedoColorTint ).rgb;
			float2 uv_Emission = i.uv_texcoord * _Emission_ST.xy + _Emission_ST.zw;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 lerpResult32 = lerp( (WorldNormalVector( i , tex2DNode11 )) , ase_worldNormal , ( 1.0 - _RimEmissionNormal ));
			float3 switchResult136 = (((i.ASEVFace>0)?(lerpResult32):(-lerpResult32)));
			float fresnelNdotV22 = dot( switchResult136, ase_worldViewDir );
			float fresnelNode22 = ( 0.0 + 1.0 * pow( 1.0 - fresnelNdotV22, _RimEmissionExp ) );
			float clampResult145 = clamp( fresnelNode22 , 0.0 , 1.0 );
			#ifdef _RIMEMISSIONAFFECTSMASK_ON
				float staticSwitch38 = clampResult145;
			#else
				float staticSwitch38 = 1.0;
			#endif
			float4 temp_cast_1 = (0.0).xxxx;
			#ifdef _RIMEMISSIONENABLED_ON
				float4 staticSwitch24 = ( clampResult145 * _RimEmissionColor * _RimEmissionPower * i.vertexColor );
			#else
				float4 staticSwitch24 = temp_cast_1;
			#endif
			float2 uv0_DissolveTexture = i.uv_texcoord * _DissolveTexture_ST.xy + _DissolveTexture_ST.zw;
			float2 appendResult115 = (float2(_DissolveTextureScaleU , _DissolveTextureScaleV));
			float4 tex2DNode40 = tex2D( _DissolveTexture, ( uv0_DissolveTexture * appendResult115 ) );
			#ifdef _DISSOLVETEXTUREFLIP_ON
				float staticSwitch46 = ( 1.0 - tex2DNode40.r );
			#else
				float staticSwitch46 = tex2DNode40.r;
			#endif
			#ifdef _DISSOLVEMANUALCONTROL_ON
				float staticSwitch100 = _DissolveManualProgress;
			#else
				float staticSwitch100 = i.uv_tex4coord.z;
			#endif
			float2 uv_DissolveDirectionMask = i.uv_texcoord * _DissolveDirectionMask_ST.xy + _DissolveDirectionMask_ST.zw;
			float4 tex2DNode49 = tex2D( _DissolveDirectionMask, uv_DissolveDirectionMask );
			#ifdef _DISSOLVEDIRECTIONMASKFLIP_ON
				float staticSwitch51 = ( 1.0 - tex2DNode49.r );
			#else
				float staticSwitch51 = tex2DNode49.r;
			#endif
			float clampResult62 = clamp( ( ( (-1.0 + (staticSwitch100 - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) + staticSwitch51 ) * _DissolveDirectionScale ) , 0.0 , 1.0 );
			float temp_output_77_0 = ( ( staticSwitch46 * clampResult62 ) + clampResult62 );
			float clampResult80 = clamp( ( temp_output_77_0 + _DissolveEmissionOffset ) , 0.0 , 1.0 );
			float temp_output_85_0 = (0.0 + (clampResult80 - 0.0) * (1.0 - 0.0) / (_DissolveEmissionRemapMax - 0.0));
			float4 switchResult98 = (((i.ASEVFace>0)?(( _DissolveEmissionPower * pow( temp_output_85_0 , _DissolveEmissionExp ) * _DissolveEmissionColor * i.vertexColor )):(( _DissolveEmissionPower * _DissolveEmissionColor * i.vertexColor ))));
			float4 temp_output_25_0 = ( ( tex2D( _Emission, uv_Emission ).r * _EmissionColor * _EmissionPower * staticSwitch38 * i.vertexColor ) + staticSwitch24 + switchResult98 );
			float4 temp_cast_2 = (100.0).xxxx;
			float4 clampResult143 = clamp( ( i.uv_tex4coord.w * temp_output_25_0 ) , float4( 0,0,0,0 ) , temp_cast_2 );
			#ifdef _EMISSIONVSCONTROLENABLED_ON
				float4 staticSwitch140 = clampResult143;
			#else
				float4 staticSwitch140 = temp_output_25_0;
			#endif
			o.Emission = staticSwitch140.rgb;
			float2 uv_MetallicSmoothness = i.uv_texcoord * _MetallicSmoothness_ST.xy + _MetallicSmoothness_ST.zw;
			float4 tex2DNode12 = tex2D( _MetallicSmoothness, uv_MetallicSmoothness );
			o.Metallic = ( tex2DNode12.r * _Metallic );
			o.Smoothness = ( tex2DNode12.a * _Smoothness );
			o.Alpha = 1;
			float clampResult81 = clamp( ( 1.0 - temp_output_77_0 ) , 0.0 , 1.0 );
			clip( clampResult81 - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

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
1920;0;1920;1018;2110.533;-736.491;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;117;-3889.353,2942.72;Inherit;False;1477.641;370.8835;;8;40;47;46;112;54;113;114;115;Dissolve Texture;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;105;-3961.939,3463.851;Inherit;False;1576.49;756.1741;;11;101;42;100;43;51;49;53;50;61;62;57;Dissolve Direction Mask;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;101;-3911.939,3513.851;Inherit;False;Property;_DissolveManualProgress;Dissolve Manual Progress;31;0;Create;True;0;0;False;0;False;0;0;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-3839.353,3198.604;Inherit;False;Property;_DissolveTextureScaleV;Dissolve Texture Scale V;20;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;42;-3849.072,3620.39;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;113;-3839.353,3122.604;Inherit;False;Property;_DissolveTextureScaleU;Dissolve Texture Scale U;19;0;Create;True;0;0;False;0;False;1;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;49;-3847.057,3943.83;Inherit;True;Property;_DissolveDirectionMask;Dissolve Direction Mask;22;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;53;-3542.355,4106.457;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;54;-3828.939,2997.657;Inherit;False;0;40;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;115;-3576.352,3146.604;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;100;-3577.656,3574.718;Inherit;False;Property;_DissolveManualControl;Dissolve Manual Control;30;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;43;-3236.605,3575.065;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;51;-3394.091,3968.459;Inherit;False;Property;_DissolveDirectionMaskFlip;Dissolve Direction Mask Flip;23;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;112;-3351.398,3038.639;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;50;-2984.419,3762.308;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;40;-3205.489,2992.72;Inherit;True;Property;_DissolveTexture;Dissolve Texture;18;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;57;-3049.998,4105.025;Inherit;False;Property;_DissolveDirectionScale;Dissolve Direction Scale;24;0;Create;True;0;0;False;0;False;1;0.25;0;4;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;103;-1036.222,-850.1656;Inherit;False;1254.974;632.8292;;6;11;95;92;93;91;90;Normal;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;111;-2177.115,416.9764;Inherit;False;2060.823;1429.637;;25;33;30;34;31;32;28;26;22;23;110;27;24;29;25;39;38;97;20;19;17;18;136;137;138;145;Emission;1,1,1,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;47;-2875.077,3167.451;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-2719.237,3877.458;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;11;-986.2218,-630.7053;Inherit;True;Property;_Normal;Normal;5;0;Create;True;0;0;False;0;False;-1;a687007e1e43afe499e3fea697d19f7f;3e17fc28f403d3a4cbd3e26510e4dd5c;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;33;-2127.115,1437.305;Inherit;False;Property;_RimEmissionNormal;Rim Emission Normal;14;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;62;-2560.449,3877.113;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch;46;-2742.711,3014.618;Inherit;False;Property;_DissolveTextureFlip;Dissolve Texture Flip;21;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;107;-2424.842,2022.516;Inherit;False;1702.48;767.8119;;14;98;82;83;99;84;87;88;86;78;85;80;79;109;118;Dissolve;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-2072.973,3338.445;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;30;-1879.315,1140.617;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;34;-1842.887,1438.664;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;31;-1876.889,1290.427;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-1877.122,3444.023;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-2374.842,2300.782;Inherit;False;Property;_DissolveEmissionOffset;Dissolve Emission Offset;25;0;Create;True;0;0;False;0;False;0;-0.25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;32;-1645.78,1205.798;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;79;-2077.032,2281.022;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;138;-1632.545,992.2723;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;80;-1947.62,2276.193;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;136;-1463.739,1047.841;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;28;-1613.586,1345.04;Inherit;False;Property;_RimEmissionExp;Rim Emission Exp;16;0;Create;True;0;0;False;0;False;3;4;0.1;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-2117.641,2409.535;Inherit;False;Property;_DissolveEmissionRemapMax;Dissolve Emission Remap Max;28;0;Create;True;0;0;False;0;False;0.25;0.25;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;22;-1287.621,1211.195;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-1839.841,2497.438;Inherit;False;Property;_DissolveEmissionExp;Dissolve Emission Exp;29;0;Create;True;0;0;False;0;False;1;4;0.2;8;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;85;-1779.369,2270.284;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;87;-1568.741,2281.637;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1486.24,2072.516;Inherit;False;Property;_DissolveEmissionPower;Dissolve Emission Power;27;0;Create;True;0;0;False;0;False;1;6;0;80;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-1203.223,904.9998;Inherit;False;Constant;_Float1;Float 1;17;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1325.726,1560.859;Inherit;False;Property;_RimEmissionPower;Rim Emission Power;17;0;Create;True;0;0;False;0;False;6;16;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;145;-1077.533,1090.491;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;26;-1272.468,1392.974;Inherit;False;Property;_RimEmissionColor;Rim Emission Color;15;0;Create;True;0;0;False;0;False;1,0,0,1;1,0.1648435,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;84;-1514.245,2415.326;Inherit;False;Property;_DissolveEmissionColor;Dissolve Emission Color;26;0;Create;True;0;0;False;0;False;1,0,0,1;1,0.1265773,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;110;-1225.764,1644.613;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;109;-1439.372,2592.596;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-1125.988,2331.388;Inherit;False;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;19;-931.3857,665.4919;Inherit;False;Property;_EmissionColor;Emission Color;10;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;29;-972.8679,1249.396;Inherit;False;Constant;_Float0;Float 0;13;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;97;-897.3526,1010.949;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-999.8845,837.0757;Inherit;False;Property;_EmissionPower;Emission Power;11;0;Create;True;0;0;False;0;False;0;0;0;60;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1128.449,2182.036;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-958.3439,1379.507;Inherit;False;4;4;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;102;-960.9005,-1541.848;Inherit;False;1241.621;617.238;;8;36;1;37;35;5;21;4;2;Albedo;1,1,1,1;0;0
Node;AmplifyShaderEditor.StaticSwitch;38;-1055.573,915.1864;Inherit;False;Property;_RimEmissionAffectsMask;Rim Emission Affects Mask;13;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;-1017.233,466.9764;Inherit;True;Property;_Emission;Emission;9;0;Create;True;0;0;False;0;False;-1;4a0283a562862ec4a83d7f636c6ca353;4a0283a562862ec4a83d7f636c6ca353;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;36;-910.9005,-1041.911;Inherit;False;Property;_AlbedoDecalNegate;Albedo Decal Negate;4;0;Create;True;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;98;-941.3636,2254.688;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;-560.0816,671.3485;Inherit;False;5;5;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StaticSwitch;24;-794.7859,1319.386;Inherit;False;Property;_RimEmissionEnabled;Rim Emission Enabled;12;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-271.2911,1155.967;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;1;-726.6796,-1491.848;Inherit;True;Property;_AlbedoDecalMask;AlbedoDecalMask;0;0;Create;True;0;0;False;0;False;-1;eeee2f2b873f0ef48bfee601a9db2d49;b18c72f96b1edb446ad9d28833e2c218;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;37;-638.2935,-1034.609;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;142;-90.79956,844.4626;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;108;-505.968,2606.764;Inherit;False;544.4161;240.6371;;3;44;81;45;Opacity;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;104;-428.3597,-133.1724;Inherit;False;610.463;429.7406;;5;15;16;12;14;13;Metallic Smoothness;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-453.3085,-1114.931;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;92;-486.9574,-520.9141;Inherit;False;Constant;_Vector0;Vector 0;28;0;Create;True;0;0;False;0;False;-1,-1,-1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;5;-643.5815,-1304.647;Inherit;False;Property;_AlbedoDecalColor;Albedo Decal Color;3;0;Create;True;0;0;False;0;False;1,0,0,1;1,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;34.87073,1184.545;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-455.968,2656.764;Inherit;False;Constant;_Float2;Float 2;18;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;95;-480.5492,-676.5618;Inherit;False;Constant;_Vector1;Vector 1;28;0;Create;True;0;0;False;0;False;1,1,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;144;24.81201,1297.316;Inherit;False;Constant;_Float4;Float 4;34;0;Create;True;0;0;False;0;False;100;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-273.6159,-800.1656;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;4;-248.079,-1191.346;Inherit;False;Property;_AlbedoColorTint;Albedo Color Tint;2;0;Create;True;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;15;-341.9748,107.5413;Inherit;False;Property;_Metallic;Metallic;7;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;12;-378.3596,-83.17242;Inherit;True;Property;_MetallicSmoothness;MetallicSmoothness;6;0;Create;True;0;0;False;0;False;-1;a5768a63f6d0d7e48b5f52e1db919297;82995db2e2a1dfa4387c843b7dc32422;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-271.1487,-350.3366;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;44;-281.5261,2693.088;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;143;194.812,1202.316;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-336.9562,181.568;Inherit;False;Property;_Smoothness;Smoothness;8;0;Create;True;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;21;-283.2945,-1403.987;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;133;642.2697,2115.75;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;132;-220.1584,1888.405;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;120;-112.5262,2027.202;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;45.72075,-1287.646;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;130;383.3191,2043.822;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;11.84851,115.0693;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;90;-2.247931,-597.1598;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ClampOpNode;81;-136.5517,2691.401;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;13.10333,-7.890445;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;390.2134,2245.485;Inherit;False;Property;_Float3;Float 3;33;0;Create;True;0;0;False;0;False;1;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;118;-1640.587,2153.101;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;129;-294.2269,2022.901;Inherit;False;FLOAT3;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;127;-512.1315,1899.601;Inherit;False;0;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;131;36.67528,2028.092;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;128;-514.309,2059.67;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;137;-1872.785,936.4926;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.WorldNormalVector;122;-397.3716,2290.537;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.StaticSwitch;140;160.0827,1071.756;Inherit;False;Property;_EmissionVSControlEnabled;Emission VS Control Enabled;32;0;Create;True;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;False;9;1;COLOR;0,0,0,0;False;0;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;4;COLOR;0,0,0,0;False;5;COLOR;0,0,0,0;False;6;COLOR;0,0,0,0;False;7;COLOR;0,0,0,0;False;8;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;84.32593,1387.665;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;123;231.0763,2024.291;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;575.5947,1332.097;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SineVFX/TopDownEffects/Props;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;53;0;49;1
WireConnection;115;0;113;0
WireConnection;115;1;114;0
WireConnection;100;1;42;3
WireConnection;100;0;101;0
WireConnection;43;0;100;0
WireConnection;51;1;49;1
WireConnection;51;0;53;0
WireConnection;112;0;54;0
WireConnection;112;1;115;0
WireConnection;50;0;43;0
WireConnection;50;1;51;0
WireConnection;40;1;112;0
WireConnection;47;0;40;1
WireConnection;61;0;50;0
WireConnection;61;1;57;0
WireConnection;62;0;61;0
WireConnection;46;1;40;1
WireConnection;46;0;47;0
WireConnection;75;0;46;0
WireConnection;75;1;62;0
WireConnection;30;0;11;0
WireConnection;34;0;33;0
WireConnection;77;0;75;0
WireConnection;77;1;62;0
WireConnection;32;0;30;0
WireConnection;32;1;31;0
WireConnection;32;2;34;0
WireConnection;79;0;77;0
WireConnection;79;1;78;0
WireConnection;138;0;32;0
WireConnection;80;0;79;0
WireConnection;136;0;32;0
WireConnection;136;1;138;0
WireConnection;22;0;136;0
WireConnection;22;3;28;0
WireConnection;85;0;80;0
WireConnection;85;2;86;0
WireConnection;87;0;85;0
WireConnection;87;1;88;0
WireConnection;145;0;22;0
WireConnection;99;0;82;0
WireConnection;99;1;84;0
WireConnection;99;2;109;0
WireConnection;83;0;82;0
WireConnection;83;1;87;0
WireConnection;83;2;84;0
WireConnection;83;3;109;0
WireConnection;27;0;145;0
WireConnection;27;1;26;0
WireConnection;27;2;23;0
WireConnection;27;3;110;0
WireConnection;38;1;39;0
WireConnection;38;0;145;0
WireConnection;98;0;83;0
WireConnection;98;1;99;0
WireConnection;18;0;17;1
WireConnection;18;1;19;0
WireConnection;18;2;20;0
WireConnection;18;3;38;0
WireConnection;18;4;97;0
WireConnection;24;1;29;0
WireConnection;24;0;27;0
WireConnection;25;0;18;0
WireConnection;25;1;24;0
WireConnection;25;2;98;0
WireConnection;37;0;36;0
WireConnection;35;0;1;4
WireConnection;35;1;37;0
WireConnection;139;0;142;4
WireConnection;139;1;25;0
WireConnection;93;0;11;0
WireConnection;93;1;95;0
WireConnection;91;0;11;0
WireConnection;91;1;92;0
WireConnection;44;0;45;0
WireConnection;44;1;77;0
WireConnection;143;0;139;0
WireConnection;143;2;144;0
WireConnection;21;0;1;0
WireConnection;21;1;5;0
WireConnection;21;2;35;0
WireConnection;133;0;130;0
WireConnection;133;1;134;0
WireConnection;132;0;129;0
WireConnection;120;0;132;0
WireConnection;120;1;122;0
WireConnection;2;0;21;0
WireConnection;2;1;4;0
WireConnection;130;0;123;0
WireConnection;14;0;12;4
WireConnection;14;1;16;0
WireConnection;90;0;93;0
WireConnection;90;1;91;0
WireConnection;81;0;44;0
WireConnection;13;0;12;1
WireConnection;13;1;15;0
WireConnection;118;0;85;0
WireConnection;129;0;127;4
WireConnection;129;1;128;1
WireConnection;129;2;128;2
WireConnection;131;0;120;0
WireConnection;137;0;11;0
WireConnection;122;0;90;0
WireConnection;140;1;25;0
WireConnection;140;0;143;0
WireConnection;124;0;25;0
WireConnection;124;1;133;0
WireConnection;123;0;131;0
WireConnection;0;0;2;0
WireConnection;0;1;90;0
WireConnection;0;2;140;0
WireConnection;0;3;13;0
WireConnection;0;4;14;0
WireConnection;0;10;81;0
ASEEND*/
//CHKSM=90249B445612147A4C4FBAB585360CFC62339401