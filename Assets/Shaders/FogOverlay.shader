Shader "Custom/FogOverlay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Float) = 12
        _PlayerPos ("Player Position", Vector) = (0, 0, 0, 0)
        _Color ("Fog Color", Color) = (0, 0, 0, 0.95)
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float _Radius;
            float4 _PlayerPos;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 playerPos2D = _PlayerPos.xy;
                float2 fragPos2D = i.worldPos.xy;

                float dist = distance(fragPos2D, playerPos2D);

                float holeRadius = _Radius;
                float edgeWidth = 1.5;

                float alpha = 1.0 - smoothstep(holeRadius - edgeWidth, holeRadius, dist);

                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}
