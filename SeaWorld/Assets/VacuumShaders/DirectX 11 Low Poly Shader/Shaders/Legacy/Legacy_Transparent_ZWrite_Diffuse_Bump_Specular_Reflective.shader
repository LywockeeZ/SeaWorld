Shader "Hidden/VacuumShaders/DirectX 11 Low Poly/Legacy_Transparent_ZWrite_Diffuse_Bump_Specular_Reflective"   
{
	Properties 
	{
		[VacuumShadersShaderType] _SHADER_TYPE_LABEL("", float) = 0
		[VacuumShadersRenderingMode] _RENDERING_MODE_LABEL("", float) = 0

		[VacuumShadersLabel] _VERTEX_LABEL("Low Poly", float) = 0
		[Enum(Triangle,0,Quad,1)] _SamplingType("Sampling Type", Float) = 0
		_MainTex ("Texture #1", 2D) = "white" {}
		[VacuumShadersUVScroll] _MainTex_Scroll("    ", vector) = (0, 0, 0, 0)		
		  
		[VacuumShadersSecondVertexTexture] _SecondTextureID("", Float) = 0
		[HideInInspector] _SecondTex_BlendType("", Float) = 0
		[HideInInspector] _SecondTex_AlphaOffset("", Range(-1, 1)) = 0
		[HideInInspector] _SecondTex ("", 2D) = "white" {}
		[HideInInspector] _SecondTex_Scroll("", vector) = (0, 0, 0, 0) 
		 
		[VacuumShadersLabel] _PIXEL_LABEL("Fragment", float) = 0
		_Color ("Tint Color", Color) = (1,1,1,1)	
		[VacuumShadersToggleSimple] _VertexColor("Mesh Vertex Color", Float) = 0

		[VacuumShadersPixelTexture] _PixelTextureID("", Float) = 0
		[HideInInspector] _PixelTex_BlendType("Blend Type", Float) = 0
		[HideInInspector] _PixelTex_AlphaOffset("", Range(-1, 1)) = 0
		[HideInInspector] _PixelTex ("  Texture", 2D) = "white" {}
		[HideInInspector] _PixelTex_Scroll("    ", vector) = (0, 0, 0, 0)

		[VacuumShadersLargeLabel] _ALPHA_LABEL(" Alpha", float) = 0
		[VacuumShadersToggleSimple] _AlphaFromVertex("    Use Low Poly Alpha", Float) = 0
		[VacuumShadersToggleZWrite] _ZWrite_LABEL("    ZWrite", float) = 0

		[VacuumShadersToggleEffect] _SPECULAR_LABEL("Specular", float) = 0
		_SpecColor ("    Color", Color) = (0.5, 0.5, 0.5, 1)
		[Enum(Original,0,Lowpoly,1)] _SpecTextureUV("    Map UV Type", float) = 0
		[NoScaleOffset] _SpecTex ("    Map", 2D) = "white" {}
	    _Shininess ("    Shininess", Range (0.01, 1)) = 0.078125
		_GlossOffset("    Gloss Offset", Range(-1, 1)) = 0
				
		[VacuumShadersToggleEffect] _BUMP_LABEL("Bump", float) = 0	
		_BumpStrength("    Strength", float) = 1
		_BumpTex (" ", 2D) = "bump" {}
		[VacuumShadersUVScroll] _BumpTex_Scroll("    ", vector) = (0, 0, 0, 0)

		[VacuumShadersReflection] _REFLECTION_LABEL("Reflective", float) = 0				
		[HideInInspector] _ReflectColor ("  Color", Color) = (1,1,1,0.5)		
		[HideInInspector] _ReflectionRoughness ("  Roughness", Range(0, 1)) = 0
		[HideInInspector] _ReflectionFresnel("  Fresnel Pow", Range(0, 8.0)) = 1
		[HideInInspector] _ReflectionStrengthOffset("  Strength Offset", Range(-1, 1)) = 0
		[HideInInspector] _CubeIsHDR("  Is HDR", Float) = 0
		[HideInInspector] _Cube("  Cubemap", Cube) = "_Skybox" {} 
		[HideInInspector] _ReflectionTex ("Internal reflection", 2D) = "black" {}
		[HideInInspector] _ReflectionDistortion("Distortion", Float) = 1

		[VacuumShadersEmission] _EMISSION_TOGGLE("Emission", float) = 0	
		[HideInInspector] _EmissionTex ("", 2D) = "white" {}
		[HideInInspector] _EmissionTex_Scroll("", vector) = (0, 0, 0, 0)
		[HideInInspector] _EmissionColor("", color) = (1, 1, 1, 1)
		[HideInInspector] _EmissionStrength("", float) = 1

		[VacuumShadersLabel] _Dsiplace_LABEL("Displace", float) = 0	
		[VacuumShadersDisplaceType] _DisplaceType("", Float) = 0
		[HideInInspector] _DisplaceTex_1 ("", 2D) = "gray" {}
		[HideInInspector] _DisplaceTex_1_Scroll("", vector) = (0, 0, 0, 0)
		[HideInInspector] _DisplaceTex_2 ("", 2D) = "gray" {}
		[HideInInspector] _DisplaceTex_2_Scroll("", vector) = (0, 0, 0, 0)
        [HideInInspector] _DisplaceBlendType ("Blend Type", Float) = 1
		[HideInInspector] _DisplaceStrength ("", float) = 1

		[HideInInspector] _DisplaceDirection("", Range(0, 360)) = 45
		[HideInInspector] _DisplaceScriptSynchronize("", Float) = 0
		[HideInInspector] _DisplaceSpeed("", Float) = 1
		[HideInInspector] _DisplaceAmplitude ("", Float) = 0.5
		[HideInInspector] _DisplaceFrequency("", Float) = 0.2
		[HideInInspector] _DisplaceNoiseCoef("", Float) = -0.5

		[VacuumShadersLabel] _FORWARD_RENDERING_LABEL("Forward Rendering Options", float) = 0
		[VacuumShadersLowPolyLight] _LowPolyLightID("", Float) = 0

		//PaperCraft
		[VacuumShadersLabel] _PAPERCRAFT_LABEL("Wireframe", float) = 0
		[VacuumShadersPaperCraft] _PaperCrat("", float) = 0
		[HideInInspector] _V_WIRE_FixedSize("Fixed Size", float) = 0
		[HideInInspector] _V_WIRE_Size("Wire Size", Float) = 1
		[HideInInspector] _V_WIRE_Color("Wire Color", color) = (0, 0, 0, 1)
		[HideInInspector] _V_WIRE_TryQuad("Try Quad", Float) = 0

		[VacuumShadersLabel] _UNITY_ARO("Unity Advanced Rendering Options", float) = 0
	}

	SubShader 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PaperCraft" = "Off" }
		LOD 400

		UsePass "Hidden/VacuumShaders/DirectX 11 Low Poly/ZWrite/MAIN"


		
	
			ZWrite Off ColorMask RGB

			// ---- forward rendering base pass:
			Pass{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma multi_compile_fog
#pragma multi_compile_fwdbasealpha noshadow
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
			// Surface shader code generated based on:
			// writes to per-pixel normal: YES
			// writes to emission: YES
			// needs world space reflection vector: YES
			// needs world space normal vector: no
			// needs screen space position: no
			// needs world space position: no
			// needs view direction: no
			// needs world space view direction: no
			// needs world space position for lighting: YES
			// needs world space view direction for lighting: YES
			// needs world space view direction for lightmaps: no
			// needs vertex color: no
			// needs VFACE: no
			// passes tangent-to-world matrix to pixel shader: YES
			// reads from normal: no
			// 2 texcoords actually used
			//   float2 _MainTex
			//   float2 _BumpMap

#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"


			// vertex-to-fragment interpolation data
			// no lightmaps:
#ifdef LIGHTMAP_OFF
			struct v2f_surf {
			float4 pos : SV_POSITION;
			float4 pixelTexUV : TEXCOORD0; // _MainTex _BumpMap
			float4 tSpace0 : TEXCOORD1;
			float4 tSpace1 : TEXCOORD2;
			float4 tSpace2 : TEXCOORD3;
#if UNITY_SHOULD_SAMPLE_SH
#define V_GEOMETRY_SAVE_SPHERICAL_HARMONICS

			half3 sh : TEXCOORD4; // SH
#endif
			UNITY_FOG_COORDS(6)
#if SHADER_TARGET >= 30
				float4 lmap : TEXCOORD7;
#endif

			float3 worldPos : TEXCOORD8;
			float3 worldPosOrig : TEXCOORD9;
			fixed4 color : COLOR0;
			float4 screenPos : TEXCOORD10;
			float3 mass : TEXCOORD11;
			float3 objectPos : TEXCOORD12;
		};
#endif
		// with lightmaps:
#ifndef LIGHTMAP_OFF
		struct v2f_surf {
			float4 pos : SV_POSITION;
			float4 pixelTexUV : TEXCOORD0; // _MainTex _BumpMap
			float4 tSpace0 : TEXCOORD1;
			float4 tSpace1 : TEXCOORD2;
			float4 tSpace2 : TEXCOORD3;
			float4 lmap : TEXCOORD4;
			UNITY_FOG_COORDS(6)

				fixed4 color : COLOR0;
			float3 worldPos : TEXCOORD7;
			float3 worldPosOrig : TEXCOORD8;
			float4 screenPos : TEXCOORD9;
			float3 mass : TEXCOORD10;
			float3 objectPos : TEXCOORD11;
		};
#endif


#pragma shader_feature _ V_LP_LIGHT_ON
#pragma shader_feature _ V_LP_SECOND_TEXTURE_ON
#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
#pragma shader_feature _ V_LP_DISPLACE_PARAMETRIC V_LP_DISPLACE_TEXTURE
#pragma shader_feature V_LP_REFLECTIVE_CUBE_MAP V_LP_REFLECTIVE_PROBE V_LP_REFLECTIVE_REALTIME
#define V_LP_TRANSPARENT
#define V_LP_BUMP
#define V_LP_SPECULAR
#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_SAVE_LOWPOLY_UV
#ifdef V_LP_LIGHT_ON
#define V_GEOMETRY_SAVE_WORLD_POSITION_WORLD_POSITION
#endif
#if defined(V_LP_DISPLACE_PARAMETRIC) || defined(V_LP_DISPLACE_TEXTURE)
#define V_GEOMETRY_SAVE_TANGENT_BINORMAL_NORMAL
#else
#define V_GEOMETRY_SAVE_NORMAL_T_SPACE
#endif
		//#pragma shader_feature	V_WIRE_TRY_QUAD_OFF V_WIRE_TRY_QUAD_ON
		//#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/PaperCraft.cginc"
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


		// vertex shader
		v2f_surf vert_surf(appdata_full v) {

			SET_UP_LOW_POLY_DATA(v)

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

#ifndef V_GEOMETRY_SAVE_TANGENT_BINORMAL_NORMAL
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0.xyz = float3(worldTangent.x, worldBinormal.x, worldNormal.x);
				o.tSpace1.xyz = float3(worldTangent.y, worldBinormal.y, worldNormal.y);
				o.tSpace2.xyz = float3(worldTangent.z, worldBinormal.z, worldNormal.z);
#endif

				o.tSpace0.w = worldNormal.x;
				o.tSpace1.w = worldNormal.y;
				o.tSpace2.w = worldNormal.z;

				o.worldPos = worldPos;
				o.worldPosOrig = worldPos;

#ifndef DYNAMICLIGHTMAP_OFF
				o.lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
#endif
#ifndef LIGHTMAP_OFF
				o.lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
#endif

				// SH/ambient and vertex lights
#ifdef LIGHTMAP_OFF
#if UNITY_SHOULD_SAMPLE_SH
				o.sh = 0;
				// Approximated illumination from non-important point lights
#ifdef VERTEXLIGHT_ON
				o.sh += Shade4PointLights(
					unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
					unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
					unity_4LightAtten0, worldPos, worldNormal);
#endif
				o.sh = ShadeSHPerVertex(worldNormal, o.sh);
#endif
#endif // LIGHTMAP_OFF

				UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
				return o;
		}

		// fragment shader
		fixed4 frag_surf(v2f_surf IN) : SV_Target{
			// prepare and unpack data
			float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
			fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
			fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
			fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(IN.worldPosOrig));
#ifdef UNITY_COMPILER_HLSL
			SurfaceOutput o = (SurfaceOutput)0;
#else
			SurfaceOutput o;
#endif
			o.Albedo = 0.0;
			o.Emission = 0.0;
			o.Specular = 0.0;
			o.Alpha = 0.0;
			o.Gloss = 0.0;
			fixed3 normalWorldVertex = fixed3(0,0,1);


			//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			fixed4 lowpolyColor = GetLowpolyPixelColor(IN.pixelTexUV.xy, IN.color);

			//PaperCraft
			//			MakePaperCraft(IN.mass, worldPos, lowpolyColor);

			//Albedo & Alpha
			o.Albedo = lowpolyColor.rgb;
			o.Alpha = lowpolyColor.a;

			//Gloss & Specular
			o.Gloss = saturate(lowpolyColor.a + _GlossOffset);
			o.Specular = _Shininess;

			_SpecColor.rgb *= tex2D(_SpecTex, lerp(IN.pixelTexUV.xy, IN.pixelTexUV.zw, _SpecTextureUV));

			//Normal 
			o.Normal = GetLowpolyBump(IN.pixelTexUV.xy);

			//Reflection 
			fixed3 viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;

			float3 worldRefl = 0;
#if defined(V_LP_REFLECTIVE_CUBE_MAP) || defined(V_LP_REFLECTIVE_PROBE)
			half3 n1 = half3(dot(IN.tSpace0.xyw, o.Normal), dot(IN.tSpace1.xyw, o.Normal), dot(IN.tSpace2.xyw, o.Normal));
			half3 n2 = half3(dot(IN.tSpace0.xyz, o.Normal), dot(IN.tSpace1.xyz, o.Normal), dot(IN.tSpace2.xyz, o.Normal));

			worldRefl = reflect(-worldViewDir, lerp(n1, n2, _ReflectionDistortion));
#endif

			float3 wNormal = 0;
#ifdef V_LP_REFLECTIVE_REALTIME
			wNormal = fixed3(IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z);
#endif

			o.Emission = GetLowpolyReflectionColor(worldRefl, o.Normal, wNormal, viewDir, lowpolyColor.a, IN.screenPos);

			//Emission
			o.Emission += GetLowpolyEmission(IN.pixelTexUV.xy);

			//Restore
			worldViewDir = normalize(UnityWorldSpaceViewDir(IN.worldPos));
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


			// compute lighting & shadowing factor
			UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
				fixed4 c = 0;
			fixed3 worldN;
			worldN.x = dot(IN.tSpace0.xyz, o.Normal);
			worldN.y = dot(IN.tSpace1.xyz, o.Normal);
			worldN.z = dot(IN.tSpace2.xyz, o.Normal);
			o.Normal = worldN;

			// Setup lighting environment
			UnityGI gi;
			UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
			gi.indirect.diffuse = 0;
			gi.indirect.specular = 0;
#if !defined(LIGHTMAP_ON)
			gi.light.color = _LightColor0.rgb;
			gi.light.dir = lightDir;
			gi.light.ndotl = LambertTerm(o.Normal, gi.light.dir);
#endif
			// Call GI (lightmaps/SH/reflections) lighting function
			UnityGIInput giInput;
			UNITY_INITIALIZE_OUTPUT(UnityGIInput, giInput);
			giInput.light = gi.light;
			giInput.worldPos = worldPos;
			giInput.worldViewDir = worldViewDir;
			giInput.atten = atten;
#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON)
			giInput.lightmapUV = IN.lmap;
#else
			giInput.lightmapUV = 0.0;
#endif 

giInput.ambient.rgb = 0.0;
#ifdef LIGHTMAP_OFF
	#if UNITY_SHOULD_SAMPLE_SH
			giInput.ambient = IN.sh;		
	#endif
#endif


			giInput.probeHDR[0] = unity_SpecCube0_HDR;
			giInput.probeHDR[1] = unity_SpecCube1_HDR;
#if UNITY_SPECCUBE_BLENDING || UNITY_SPECCUBE_BOX_PROJECTION
			giInput.boxMin[0] = unity_SpecCube0_BoxMin; // .w holds lerp value for blending
#endif
#if UNITY_SPECCUBE_BOX_PROJECTION
			giInput.boxMax[0] = unity_SpecCube0_BoxMax;
			giInput.probePosition[0] = unity_SpecCube0_ProbePosition;
			giInput.boxMax[1] = unity_SpecCube1_BoxMax;
			giInput.boxMin[1] = unity_SpecCube1_BoxMin;
			giInput.probePosition[1] = unity_SpecCube1_ProbePosition;
#endif
			LightingBlinnPhong_GI(o, giInput, gi);

			// realtime lighting: call lighting function
			c += LightingBlinnPhong(o, worldViewDir, gi);
			c.rgb += o.Emission;
			UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
			return c;
		}

			ENDCG

		}

			// ---- forward rendering additive lights pass:
			Pass{
			Name "FORWARD"
			Tags{ "LightMode" = "ForwardAdd" }
			ZWrite Off Blend One One
			Blend SrcAlpha One

			CGPROGRAM
			// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma multi_compile_fog
#pragma multi_compile_fwdadd noshadow
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
			// Surface shader code generated based on:
			// writes to per-pixel normal: YES
			// writes to emission: YES
			// needs world space reflection vector: no
			// needs world space normal vector: no
			// needs screen space position: no
			// needs world space position: no
			// needs view direction: no
			// needs world space view direction: no
			// needs world space position for lighting: YES
			// needs world space view direction for lighting: YES
			// needs world space view direction for lightmaps: no
			// needs vertex color: no
			// needs VFACE: no
			// passes tangent-to-world matrix to pixel shader: YES
			// reads from normal: no
			// 2 texcoords actually used
			//   float2 _MainTex
			//   float2 _BumpMap

#include "UnityCG.cginc"
#include "Lighting.cginc"
#include "AutoLight.cginc"


			// vertex-to-fragment interpolation data
			struct v2f_surf {
			float4 pos : SV_POSITION;
			float4 pixelTexUV : TEXCOORD0; // _MainTex _BumpMap
			fixed3 tSpace0 : TEXCOORD1;
			fixed3 tSpace1 : TEXCOORD2;
			fixed3 tSpace2 : TEXCOORD3;
			float3 worldPos : TEXCOORD4;
			UNITY_FOG_COORDS(6)
				float3 mass : TEXCOORD7;
			float3 objectPos : TEXCOORD8;
			fixed4 color : COLOR0;
		};


#pragma shader_feature _ V_LP_LIGHT_ON
#pragma shader_feature _ V_LP_SECOND_TEXTURE_ON
#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
#pragma shader_feature _ V_LP_DISPLACE_PARAMETRIC V_LP_DISPLACE_TEXTURE
#define V_LP_TRANSPARENT
#define V_LP_BUMP
#define V_LP_SPECULAR
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_SAVE_LOWPOLY_UV
#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#ifdef V_LP_LIGHT_ON
#define V_GEOMETRY_SAVE_WORLD_POSITION_WORLD_POSITION
#endif
#if defined(V_LP_DISPLACE_PARAMETRIC) || defined(V_LP_DISPLACE_TEXTURE)
#define V_GEOMETRY_SAVE_TANGENT_BINORMAL_NORMAL
#else 
#define V_GEOMETRY_SAVE_NORMAL_T_SPACE
#endif
			//#pragma shader_feature	V_WIRE_TRY_QUAD_OFF V_WIRE_TRY_QUAD_ON
		//#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/PaperCraft.cginc"
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


		// vertex shader
		v2f_surf vert_surf(appdata_full v) {

			SET_UP_LOW_POLY_DATA(v)

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);

#ifndef V_GEOMETRY_SAVE_TANGENT_BINORMAL_NORMAL
				fixed3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross(worldNormal, worldTangent) * tangentSign;
				o.tSpace0 = fixed3(worldTangent.x, worldBinormal.x, worldNormal.x);
				o.tSpace1 = fixed3(worldTangent.y, worldBinormal.y, worldNormal.y);
				o.tSpace2 = fixed3(worldTangent.z, worldBinormal.z, worldNormal.z);
#endif

				o.worldPos = worldPos;

				UNITY_TRANSFER_FOG(o,o.pos); // pass fog coordinates to pixel shader
				return o;
		}

		// fragment shader
		fixed4 frag_surf(v2f_surf IN) : SV_Target{
			// prepare and unpack data
			float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
			fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
			fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
			fixed3 worldViewDir = normalize(UnityWorldSpaceViewDir(worldPos));
#ifdef UNITY_COMPILER_HLSL
			SurfaceOutput o = (SurfaceOutput)0;
#else
			SurfaceOutput o;
#endif
			o.Albedo = 0.0;
			o.Emission = 0.0;
			o.Specular = 0.0;
			o.Alpha = 0.0;
			o.Gloss = 0.0;
			fixed3 normalWorldVertex = fixed3(0,0,1);


			//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			fixed4 lowpolyColor = GetLowpolyPixelColor(IN.pixelTexUV.xy, IN.color);

			//PaperCraft
			//			MakePaperCraft(IN.mass, worldPos, lowpolyColor);

			//Albedo & Alpha
			o.Albedo = lowpolyColor.rgb;
			o.Alpha = lowpolyColor.a;

			//Gloss & Specular
			o.Gloss = saturate(lowpolyColor.a + _GlossOffset);
			o.Specular = _Shininess;

			_SpecColor.rgb *= tex2D(_SpecTex, lerp(IN.pixelTexUV.xy, IN.pixelTexUV.zw, _SpecTextureUV));
			
			//Normal 
			o.Normal = GetLowpolyBump(IN.pixelTexUV.xy);
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


			UNITY_LIGHT_ATTENUATION(atten, IN, worldPos)
				fixed4 c = 0;
			fixed3 worldN;
			worldN.x = dot(IN.tSpace0.xyz, o.Normal);
			worldN.y = dot(IN.tSpace1.xyz, o.Normal);
			worldN.z = dot(IN.tSpace2.xyz, o.Normal);
			o.Normal = worldN;

			// Setup lighting environment
			UnityGI gi;
			UNITY_INITIALIZE_OUTPUT(UnityGI, gi);
			gi.indirect.diffuse = 0;
			gi.indirect.specular = 0;
#if !defined(LIGHTMAP_ON)
			gi.light.color = _LightColor0.rgb;
			gi.light.dir = lightDir;
			gi.light.ndotl = LambertTerm(o.Normal, gi.light.dir);
#endif
			gi.light.color *= atten;
			c += LightingBlinnPhong(o, worldViewDir, gi);
			UNITY_APPLY_FOG(IN.fogCoord, c); // apply fog
			return c;
		}

			ENDCG

		}

			// ---- meta information extraction pass:
			Pass{
			Name "Meta"
			Tags{ "LightMode" = "Meta" }
			Cull Off

			CGPROGRAM
			// compile directives
#pragma vertex vert_surf
#pragma geometry geom
#pragma fragment frag_surf
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Platform.cginc"
#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
#include "HLSLSupport.cginc"
#include "UnityShaderVariables.cginc"
			// Surface shader code generated based on:
			// writes to per-pixel normal: no
			// writes to emission: YES
			// needs world space reflection vector: YES
			// needs world space normal vector: no
			// needs screen space position: no
			// needs world space position: no
			// needs view direction: no
			// needs world space view direction: no
			// needs world space position for lighting: YES
			// needs world space view direction for lighting: no
			// needs world space view direction for lightmaps: no
			// needs vertex color: no
			// needs VFACE: no
			// passes tangent-to-world matrix to pixel shader: no
			// reads from normal: no
			// 1 texcoords actually used
			//   float2 _MainTex

#include "UnityCG.cginc"
#include "Lighting.cginc"


#include "UnityMetaPass.cginc"

			// vertex-to-fragment interpolation data
			struct v2f_surf {
			float4 pos : SV_POSITION;
			float4 pixelTexUV : TEXCOORD0;
			half3 worldRefl : TEXCOORD1;
			float3 worldPos : TEXCOORD2;
			fixed4 color : COLOR0;

			float4 screenPos : TEXCOORD3;
		};


#pragma shader_feature _ V_LP_SECOND_TEXTURE_ON
#pragma shader_feature _ V_LP_PIXEL_TEXTURE_ON
#pragma shader_feature _ V_LP_DISPLACE_PARAMETRIC V_LP_DISPLACE_TEXTURE
#pragma shader_feature V_LP_REFLECTIVE_CUBE_MAP V_LP_REFLECTIVE_PROBE V_LP_REFLECTIVE_REALTIME
#define V_LP_TRANSPARENT
#define V_GEOMETRY_SAVE_LOWPOLY_COLOR
#define V_GEOMETRY_READ_WORLD_POSITION_WORLD_POSITION
#define V_GEOMETRY_SAVE_REFLECTION_WORLD_REFLECTION
#include "Assets/VacuumShaders/DirectX 11 Low Poly Shader/Shaders/cginc/Core.cginc"


		// vertex shader
		v2f_surf vert_surf(appdata_full v) {

			SET_UP_LOW_POLY_DATA(v)

				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = worldPos;
				float3 worldViewDir = UnityWorldSpaceViewDir(worldPos);
				o.worldRefl = reflect(-worldViewDir, worldNormal);

				return o;
		}

		// fragment shader
		fixed4 frag_surf(v2f_surf IN) : SV_Target{
			// prepare and unpack data
			float3 worldPos = IN.worldPos;
#ifndef USING_DIRECTIONAL_LIGHT
			fixed3 lightDir = normalize(UnityWorldSpaceLightDir(worldPos));
#else
			fixed3 lightDir = _WorldSpaceLightPos0.xyz;
#endif
#ifdef UNITY_COMPILER_HLSL
			SurfaceOutput o = (SurfaceOutput)0;
#else
			SurfaceOutput o;
#endif
			o.Albedo = 0.0;
			o.Emission = 0.0;
			o.Specular = 0.0;
			o.Alpha = 0.0;
			o.Gloss = 0.0;
			fixed3 normalWorldVertex = fixed3(0,0,1);


			//DirectX 11 Low Poly//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
			fixed4 lowpolyColor = GetLowpolyPixelColor(IN.pixelTexUV.xy, IN.color);

			//Albedo & Alpha  
			o.Albedo = lowpolyColor.rgb;
			o.Alpha = lowpolyColor.a;

			//Reflection
			o.Emission = GetLowpolyReflectionColor(IN.worldRefl, o.Normal, 0, normalize(UnityWorldSpaceViewDir(worldPos)), lowpolyColor.a, IN.screenPos);

			//Emission
			o.Emission += GetLowpolyEmission(IN.pixelTexUV.xy);
			////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


			UnityMetaInput metaIN;
			UNITY_INITIALIZE_OUTPUT(UnityMetaInput, metaIN);
			metaIN.Albedo = o.Albedo;
			metaIN.Emission = o.Emission;
			return UnityMetaFragment(metaIN);
		}

			ENDCG

		}


	}

	Fallback "Legacy Shaders/Transparent/VertexLit"
}
