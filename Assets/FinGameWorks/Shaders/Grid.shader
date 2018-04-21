Shader "Unlit/Grid"
{
    Properties
    {
        _GridColour ("Grid Colour", color) = (1, 1, 1, 1)
        _BaseColour ("Base Colour", color) = (1, 1, 1, 0)
        _GridSpacing ("Grid Spacing", float) = 0.1
        _LineThickness ("Line Thickness", float) = 1        
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _GridColour;
            fixed4 _BaseColour;
            float _GridSpacing;
            float _LineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = mul(unity_ObjectToWorld, v.vertex).xz / _GridSpacing;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {               
                float2 wrapped = frac(i.uv) - 0.5f;
                float2 range = abs(wrapped);

                float2 speeds;
                /* // Euclidean norm gives slightly more even thickness on diagonals
                float4 deltas = float4(ddx(i.uv), ddy(i.uv));
                speeds = sqrt(float2(
                            dot(deltas.xz, deltas.xz),
                            dot(deltas.yw, deltas.yw)
                         ));
                */  // Cheaper Manhattan norm in fwidth slightly exaggerates thickness of diagonals
                speeds = fwidth(i.uv);

                float2 pixelRange = range/speeds;
                float lineWeight = saturate(min(pixelRange.x, pixelRange.y) - _LineThickness);

                return lerp(_GridColour, _BaseColour, lineWeight);
            }
            ENDCG
        }
    }
}