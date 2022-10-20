Shader "Unlit/Color Highlight"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Scale ("Scale", float) = 1.1
        _SinPower ("SinPower", float) = 0.2
        _SinTimeScale ("SinTimeScale", float) = 1
        _Offset ("Offset", Vector) = (0,1,0,0)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 _Color;
            float4 _Offset;
            float _Scale;
            float _SinPower;
            float _SinTimeScale;

            struct vertdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            v2f vert (vertdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex * _Scale + _Offset);

                float3 c = _Color.rgb * (1.0 + _SinPower * sin(_Time*_SinTimeScale));
                
                o.color = float4(c, _Color.a);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return i.color;
            }

            ENDCG
        }
    }
}
