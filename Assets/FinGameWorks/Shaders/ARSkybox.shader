// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Width("Image XRes", Float)  = 512
        _Height("Image YRes", Float) = 512
        _Radius("Radius", Range(0, 50)) = 4
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            

	#include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 uv : TEXCOORD0;
//                float3 texcoord : TEXCOORD0;
                float3 normal : NORMAL;
//                float4 position : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
//                float4 position : SV_POSITION;
                float4 vertex : SV_POSITION;
//                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4x4 _WorldToCameraMatrix;
            float4 _MainTex_ST;
            float _Width;
            float _Height;
            float _Radius;

float2 SphereMapUVCoords( float3 viewDir, float3 normal )
	{
		// Sphere mapping. Find reflection and tranform into UV coords.
		// Heavily inspired by https://www.clicktorelease.com/blog/creating-spherical-environment-mapping-shader/
		float3 reflection = reflect(viewDir, normal);
		float m = 2. * sqrt(
			pow(reflection.x, 2.) +
			pow(reflection.y, 2.) +
			pow(reflection.z + 1., 2.)
		);
		return reflection.xy / m + .5;
	}
	
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                float3 viewDir = -normalize(WorldSpaceViewDir(v.vertex));
		        viewDir = mul(_WorldToCameraMatrix, float4(viewDir,0));
                o.uv = SphereMapUVCoords(viewDir, v.normal);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float horizontal = 1 / _Width;
                float vertical = 1 / _Height;

                int radius = _Radius;
                int count = (2 * radius + 1) * (2 * radius + 1);
                fixed x = i.uv.x;
                fixed y = i.uv.y;

                fixed4 col = fixed4(0,0,0,0);

                for (int m = -radius; m <= radius; ++m) {
                    fixed u = clamp(x + m * horizontal, 0, 1);
                    for (int n = -radius; n <= radius; ++n) {
                        fixed v = clamp(y + n * vertical, 0, 1);
                        col += tex2D(_MainTex, fixed2(u, v));
                    }
                }

                return col / count;
            }

            ENDCG
        }
    }
}