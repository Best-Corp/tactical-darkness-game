Shader "Custom/FogOverlay"
{
    Properties
    {
        _Radius ("Radius", Float) = 12
        _PlayerPos ("Player Position", Vector) = (0, 0, 0, 0)
        _Color ("Fog Color", Color) = (0, 0, 0, 0.95)
        _Softness ("Edge Softness", Float) = 1.5
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            float _Radius;
            float4 _PlayerPos;
            fixed4 _Color;
            float _Softness;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 fragPos2D = i.worldPos.xy;
                float2 playerPos2D = _PlayerPos.xy;

                float dist = distance(fragPos2D, playerPos2D);

                // Inside hole = transparent, outside = opaque
                float alpha = smoothstep(_Radius - _Softness, _Radius, dist);

                return fixed4(_Color.rgb, _Color.a * alpha);
            }
            ENDCG
        }
    }
}
