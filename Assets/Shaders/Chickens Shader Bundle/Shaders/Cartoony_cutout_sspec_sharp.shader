Shader "Chickenlord/Toony/Transparent/Cutout/Lighted Outline Specularmap Sharp" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_SpecularMap("Specular Color(RGB) Power (A)",2D) = ""{}
		_OLC ("Outline Color",Color) = (0,0,0,1)
		_OLP ("Outline Strength",Range(5,32)) = 25
		_Sharpness("Outline Sharpness",Range(1,15)) = 1
		_Steps("Lighting Steps",Range(1,128)) = 32
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	SubShader {
		Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf ToonyBlinnPhong alphatest:_Cutoff addshadow fullforwardshadows
		#pragma target 3.0
		#include "UnityCG.cginc"

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _SpecularMap;
		half _Shininess;
		fixed4 _OLC;
		half _OLP;
		half _Steps;
		half _Sharpness;
		
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
			float3 viewDir;
		};
		
		struct SurfaceOutputPS {
			fixed3 Albedo;
			fixed3 Normal;
			fixed Gloss;
			half Specular;	
			fixed Alpha;
			fixed3 Emission;
			fixed3 Spec;
		};
		
		inline fixed4 LightingToonyBlinnPhong (SurfaceOutputPS s, fixed3 lightDir, half3 viewDir, fixed atten)
		{
			half3 h = normalize (lightDir + viewDir);
			float stx = _Steps/256;
			fixed diff = max (0, dot (s.Normal, lightDir));
			diff = floor(((diff*256+_Steps/2)/_Steps))*stx;
			
			float nh = max (0, dot (s.Normal, h));
			float spec = pow (nh, s.Specular*128.0) * s.Gloss;
			spec = floor(((spec*256+_Steps/2)/_Steps))*stx;
			
			fixed4 c;
			c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * _SpecColor.rgb* s.Spec.rgb * spec) * (atten*2);
			c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
			return c;
		}
		
		inline fixed4 LightingToonyBlinnPhong_PrePass (SurfaceOutputPS s, half4 light)
		{
			fixed spec = light.a * s.Gloss;
			half stx = _Steps/256;
			spec = (floor((spec*256+_Steps/2)/_Steps))*stx;
			fixed diff = Luminance(light.rgb);
			diff = clamp((floor((diff*256+_Steps/2)/(_Steps)))*(stx),0,1);
			
			fixed4 c;
			c.rgb = (s.Albedo * light.rgb*diff + light.rgb * _SpecColor.rgb* s.Spec.rgb * spec);
			c.a = s.Alpha + spec * _SpecColor.a;
			return c;
		}
		
		void surf (Input IN, inout SurfaceOutputPS o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 spec = tex2D(_SpecularMap, IN.uv_MainTex);
			o.Albedo = c.rgb * _Color.rgb;
			o.Gloss = 	spec.a;
			o.Alpha = c.a * _Color.a;
			o.Specular = _Shininess;
			o.Spec = spec.rgb;
			o.Normal = UnpackNormal(tex2D(_BumpMap,IN.uv_BumpMap));
			fixed mult = 1-clamp(dot(o.Normal,normalize(IN.viewDir)),0,1);
			mult = pow(mult,_OLP);
			mult = min(100*mult,1);
			mult = pow(mult,_Sharpness);
			o.Albedo = (1-mult)*o.Albedo+(mult)*_OLC.rgb;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
