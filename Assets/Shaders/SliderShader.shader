Shader "Unlit/SliderShader"
{
    Properties
    {
        _GradientMiddleColor("Gradient middle color: ", color) = (1,1,1,1)
        _GradientSuccessColor("Gradient success color: ", color) = (1,1,1,1)
        _GradientOffColor("Gradient off color: ", color) = (1,1,1,1)
        _SuccessNumber("Success number(between 0 and 1): ", Float) = 0.5
        _SuccessMargin("Success margin(between 0 and 1): ", Float) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
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
            };

            fixed4 _GradientMiddleColor;
            fixed4 _GradientOffColor;
            fixed4 _GradientSuccessColor;
            float _SuccessNumber;
            float _SuccessMargin;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                //fixed4 col = fixed4(i.uv.xy,0, 1);
                fixed4 col = (1,1,1,1);


                if (i.uv.x < _SuccessNumber - _SuccessMargin)
                {
                    col = lerp(_GradientOffColor, _GradientMiddleColor, i.uv.x / (_SuccessNumber - _SuccessMargin));
                }
                else if (i.uv.x < _SuccessNumber) {
                    col = lerp(_GradientMiddleColor, _GradientSuccessColor, (i.uv.x - (_SuccessNumber - _SuccessMargin)) / (_SuccessMargin));
                }
                else if (i.uv.x < _SuccessNumber + _SuccessMargin) {
                    col = lerp(_GradientSuccessColor, _GradientMiddleColor, (i.uv.x - _SuccessNumber) / (_SuccessMargin));
                }
                else 
                {
                    col = lerp(_GradientMiddleColor, _GradientOffColor, (i.uv.x - (_SuccessNumber + _SuccessMargin)) / (1.0 - _SuccessNumber - _SuccessMargin));
                }
                
                return col;
            }
            ENDCG
        }
    }
}
