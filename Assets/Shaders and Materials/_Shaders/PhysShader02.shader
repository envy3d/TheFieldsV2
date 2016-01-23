Shader "Custom/Physical Shader 02"
{
	Properties
	{
		_LocalColor("Local Color", Color) = (0.5, 0.5, 0.5, 0.5)
		_LocalTex("Local Color Texture", 2D) = "white" {}
		_Smoothness("Smoothness", Range(1, 12)) = 2.0
		_SmoothMap("Smoothness Map", 2D) = "white" {} 
		_Metalicity("Metalicity", Range(0, 1)) = 0 
		_MetalMap("Metalicity Map", 2D) = "white" {}
		_Wrap("Light Wrap", float) = 0.25
		_NormalMap("Normal Map", 2D) = "bump" {}
		_BumpDepth("Bump Depth", Range(0.1, 4.0)) = 1
		_RSRM("RSRM", 2D) = "gray" {}
		
	}
	
	SubShader
	{
		Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
			
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				#pragma multi_compile_fwdadd_fullshadows
				#include "UnityCG.cginc"
				#include "AutoLight.cginc"
				
				//user defined
				uniform sampler2D _LocalTex;
				uniform sampler2D _NormalMap;
				uniform sampler2D _SmoothMap; 
				uniform sampler2D _MetalMap;
				uniform sampler2D _RSRM;
				uniform float4	_LocalTex_ST;
				uniform float4	_NormalMap_ST;
				uniform float4	_SmoothMap_ST;
				uniform float4	_MetalMap_ST;
				uniform float4 	_LocalColor;
				uniform float 	_Smoothness;
				uniform float	_Wrap;
				uniform float   _BumpDepth;
				uniform float	_Metalicity;
				
				//unity defined
				uniform float4 	_LightColor0;
				
				//base input struct
				struct vertexInput
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
					float4 tangent : TANGENT;
				};
				
				struct vertexOutput
				{
					float4 pos : SV_POSITION;
					float4 tex : TEXCOORD0;
					float4 posWorld : TEXCOORD1;
					float3 normalWorld : TEXCOORD2;
					float3 tangentWorld : TEXCOORD3;
					float3 binormalWorld : TEXCOORD4;
					
					LIGHTING_COORDS(5,6)
				};
				
				//vertex function
				vertexOutput vert (vertexInput v)
				{
					vertexOutput o;
					
					float4x4 modelMatrix 		= _Object2World;
					float4x4 modelMatrixInverse = _World2Object;
					
					o.normalWorld = normalize  (float3 ( mul( float4(v.normal, 0.0	), modelMatrixInverse).xyz));
					o.tangentWorld = normalize (float3 ( mul( float4(v.tangent		), modelMatrix).xyz));
					o.binormalWorld = normalize(cross(o.normalWorld,o.tangentWorld)*v.tangent.w);
					
					o.posWorld = mul(modelMatrix, v.vertex);
					o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
					o.tex = v.texcoord;
					
					TRANSFER_VERTEX_TO_FRAGMENT(o); // for shadows
					
					return o;
					
				}
				
				float clamp01 (float toBeNormalized) //take a -1 to 1 range and fit it 0 to 1
				{
					return toBeNormalized * 0.5 + 0.5;
				}
				
				float3 calculateAmbientReflection( float3 rsrm , float texM ) 
				{
					float  mask = (rsrm.x+rsrm.y+rsrm.z) * 0.33;
					float3 amb  = UNITY_LIGHTMODEL_AMBIENT.xyz;
					return  float3 (1.5 * rsrm * amb + amb * 0.5 * texM);
				}
					
				//fragment function
				float4 frag(vertexOutput i) : COLOR
				{
					float shadAtten = LIGHT_ATTENUATION(i);
					
					float4 tex	= tex2D(_LocalTex,   i.tex.xy * _LocalTex_ST.xy   + _LocalTex_ST.zw);
					tex  = tex  * _LocalColor;
					float  texS	= tex2D(_SmoothMap, i.tex.xy * _SmoothMap_ST.xy + _SmoothMap_ST.zw);
					texS = texS * _Smoothness; 
					float  texM	= tex2D(_MetalMap,  i.tex.xy * _MetalMap_ST.xy  + _MetalMap_ST.zw);
					texM = texM * _Metalicity;
					float4 texN	= tex2D(_NormalMap, i.tex.xy * _NormalMap_ST.xy + _NormalMap_ST.zw);
					float nDepth = 8/(_BumpDepth * 8);
					
					//Unpack Normal
					float3 localCoords = float3(2.0*texN.ag - float2(1.0,1.0), 0.0);
					localCoords.z	   = nDepth;
					
					//normal transpose matrix
					float3x3 local2WorldTranspose = float3x3
					(
						i.tangentWorld,
						i.binormalWorld,
						i.normalWorld
					);
					
					//Calculate normal direction
					float3 normalDir = normalize( mul( localCoords, local2WorldTranspose));
					
					float3 N = normalize( normalDir);
					float3 V = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz);
					float3 fragmentToLight = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
					float  distanceToLight = length(fragmentToLight);
					float  atten = pow(2,-0.1*distanceToLight*distanceToLight)*_WorldSpaceLightPos0.w + 1-_WorldSpaceLightPos0.w; // (-0.1x^2)^2 for pointlights 1 for dirlights
					float3 L =    (normalize(fragmentToLight))*_WorldSpaceLightPos0.w + normalize(_WorldSpaceLightPos0.xyz)*(1-_WorldSpaceLightPos0.w);
					float3 H = normalize( V + L );
					float3 worldReflect = reflect(V,N);
					
					//lighting
					float NdotL 	= dot(N,L);
					float NdotV 	= 1-max	( 0.0, dot(N,V));
					float NdotH 	= clamp	( dot(N,H),0,1);
					float VdotL 	= clamp01( dot(V,L));
					float wrap 		= clamp(_Wrap,-0.25,1.0);
					
					float4 texdesat = dot(tex.rgb,float3(0.3,0.59,0.11));
					
					float3 difftex	= lerp(tex,float4(0,0,0,0),pow(texM,1)).xyz;
					float3 spectex	= lerp(texdesat,tex,texM).xyz;
					
					VdotL 			= pow(VdotL,0.85);
					float smooth 	= pow (1.8,texS-2)+1.5;
					float rim		= texM + (pow(NdotV,1+texS/6))*(1-texM);
					float bellclamp = (1/(1+pow(0.65*acos(dot(N,L)),16)));
					
					float3 rsrm 	= tex2D(_RSRM, float2((1-(texS-1)*0.09),1-clamp01(worldReflect.y)));
					float3 rsrmDiff = tex2D(_RSRM, float2(1,N.y));
					float3 ambReflect     = calculateAmbientReflection( rsrm    , texM);
					float3 ambReflectDiff = calculateAmbientReflection( rsrmDiff, texM);
					
					
					float3 spec = NdotH;
					spec =  pow (spec,smooth*VdotL) * log(smooth*(VdotL+1)) * bellclamp * texS*(1/texS)*0.5;
					spec *= shadAtten * atten * spectex.xyz * _LightColor0.rgb * (2+texM) * spectex.xyz;
					spec += ambReflect * spectex.rgb * rim;
					spec = clamp (spec,0,1.1);
					
					float3 diff = max(0,(pow(max(0,(NdotL * (1-wrap)+wrap)),(2*wrap+1))));
					diff *= shadAtten * atten * difftex.xyz * _LightColor0.rgb * 2 * _LightColor0.rgb * difftex.xyz;
					diff += ambReflect * difftex.xyz * rim + ambReflectDiff * difftex.xyz;
					diff = clamp (diff,0,1.1);
										
					return float4 (spec + diff,1.0);
					
				}
			
			ENDCG
		}
		Pass
		{
			Tags{ "LightMode" = "ForwardAdd"}
			Fog {Mode Off}
			Blend One One
			CGPROGRAM
			
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0
				
				//user defined
				uniform sampler2D _LocalTex;
				uniform sampler2D _NormalMap;
				uniform sampler2D _SmoothMap; 
				uniform sampler2D _MetalMap;
				uniform float4	_LocalTex_ST;
				uniform float4	_NormalMap_ST;
				uniform float4	_SmoothMap_ST;
				uniform float4	_MetalMap_ST;
				uniform float4 	_LocalColor;
				uniform float 	_Smoothness;
				uniform float	_Wrap;
				uniform float   _BumpDepth;
				uniform float	_Metalicity;
				
				//unity defined
				uniform float4 	_LightColor0;
				
				//base input struct
				struct vertexInput
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
					float4 tangent : TANGENT;
				};
				
				struct vertexOutput
				{
					float4 pos : SV_POSITION;
					float4 tex : TEXCOORD0;
					float4 posWorld : TEXCOORD1;
					float3 normalWorld : TEXCOORD2;
					float3 tangentWorld : TEXCOORD3;
					float3 binormalWorld : TEXCOORD4;
				};
				
				//vertex function
				vertexOutput vert (vertexInput input)
				{
					vertexOutput o;
					
					float4x4 modelMatrix = _Object2World;
					float4x4 modelMatrixInverse = _World2Object;
					
					o.normalWorld = normalize  (float3 ( mul( float4(input.normal, 0.0), modelMatrixInverse).xyz));
					o.tangentWorld = normalize (float3 ( mul( float4(input.tangent), modelMatrix       ).xyz));
					o.binormalWorld = normalize(cross(o.normalWorld,o.tangentWorld)*input.tangent.w);
					
					o.posWorld = mul(modelMatrix, input.vertex);
					o.pos = mul(UNITY_MATRIX_MVP, input.vertex);
					o.tex = input.texcoord;
					return o;
					
				}
				
				//fragment function
				float4 frag(vertexOutput i) : COLOR
				{
					float4 tex	= tex2D(_LocalTex,   i.tex.xy * _LocalTex_ST.xy   + _LocalTex_ST.zw);
					tex = tex * _LocalColor;
					float4 texS	= tex2D(_SmoothMap, i.tex.xy * _SmoothMap_ST.xy + _SmoothMap_ST.zw);
					texS = texS * _Smoothness; 
					float4 texM	= tex2D(_MetalMap,  i.tex.xy * _MetalMap_ST.xy  + _MetalMap_ST.zw);
					texM = texM * _Metalicity;
					float4 texN	= tex2D(_NormalMap, i.tex.xy * _NormalMap_ST.xy + _NormalMap_ST.zw);
					float nDepth = 8/(_BumpDepth * 8);
					
					//Unpack Normal
					float3 localCoords = float3(2.0*texN.ag - float2(1.0,1.0), 0.0);
					localCoords.z	   = nDepth;
					
					//normal transpose matrix
					float3x3 local2WorldTranspose = float3x3
					(
						i.tangentWorld,
						i.binormalWorld,
						i.normalWorld
					);
					
					//Calculate normal direction
					float3 normalDir = normalize( mul( localCoords, local2WorldTranspose));
					
					float3 N = normalize( normalDir);
					float3 V = normalize( _WorldSpaceCameraPos.xyz - i.posWorld.xyz);
					float3 fragmentToLight = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
					float  distanceToLight = length(fragmentToLight);
					float  atten = pow(2,-0.1*distanceToLight*distanceToLight)*_WorldSpaceLightPos0.w + 1-_WorldSpaceLightPos0.w;
					float3 L =    (normalize(fragmentToLight))*_WorldSpaceLightPos0.w + normalize(_WorldSpaceLightPos0.xyz)*(1-_WorldSpaceLightPos0.w);
					float3 H = normalize( V + L );
					
					//lighting
					float NdotL 	= dot(N,L);
					float NdotV 	= 1-max	( 0.0, dot(N,V));
					float NdotH 	= clamp	( dot(N,H),0,1);
					float VdotL 	= dot(V,L)*0.5+0.5;
					float wrap 		= clamp(_Wrap,-0.25,1.0);
					
					float4 texdesat = dot(tex.rgb,float3(0.3,0.59,0.11));
					
					float3 difftex	= lerp(tex,float4(0,0,0,0),pow(texM,1)).xyz;
					float3 spectex	= lerp(texdesat,tex,texM).xyz;
					
					VdotL 			= pow(VdotL,0.85);
					float smooth 	= pow (1.8,texS-2)+1.5;
					float rim		= texM + (pow(NdotV,1+texS/6))*(1-texM);
					float bellclamp = (1/(1+pow(0.65*acos(dot(N,L)),16)));
					
					float3 spec = NdotH;
					spec =  pow (spec,smooth*VdotL) * log(smooth*(VdotL+1)) * bellclamp;
					spec *= atten * spectex.xyz * _LightColor0.rgb * (2+_Metalicity) * spectex.xyz;
					spec = clamp (spec,0,1.25);
					
					float3 diff = max(0,(pow(max(0,(NdotL * (1-wrap)+wrap)),(2*wrap+1))));
					diff *= atten * difftex.xyz * _LightColor0.rgb * 2 * difftex.xyz;
					diff = clamp (diff,0,1.25);
										
					return float4 (spec + diff,1.0);
					
				}
			
			ENDCG
		}
		
	}
	Fallback "Diffuse"
}