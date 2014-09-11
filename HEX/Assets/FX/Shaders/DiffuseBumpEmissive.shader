Shader "Custom/DiffuseBumpEmissive" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_MainColor ("Base Color", Color) = (1,1,1,1)
		_EmissiveTex ("Emissive Texture (RGB)", 2D) = "white" {}
		_EmissiveColor ("Emissive Color", Color) = (1,1,1,1)
		_NormalMap ("Normal Map", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		sampler2D _MainTex;
		sampler2D _EmissiveTex;
		sampler2D _NormalTex;

		float4 _MainColor;
		float4 _EmissiveColor;

		struct Input {
			float2 uv_MainTex;
			float2 uv_NormalTex;
			float2 uv_EmissiveTex;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = _MainColor * tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_NormalTex, IN.uv_NormalTex));
			o.Emission = _EmissiveColor * tex2D(_EmissiveTex, IN.uv_EmissiveTex);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
