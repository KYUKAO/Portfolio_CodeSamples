Shader "Custom/LowPolyBasedOnHeight" {
    Properties {
        _Color1 ("Color 1", Color) = (1, 0, 0, 1)
        _Color2 ("Color 2", Color) = (0, 1, 0, 1)
        _Color3 ("Color 3", Color) = (0, 0, 1, 1)
        _Threshold1 ("Threshold 1", Range(-1, 1)) = 0.33
        _Threshold2 ("Threshold 2", Range(-1, 1)) = 0.66
        _Rate("Rate", Range(0.001,4)) = 1
    }

    SubShader {
        Tags { "LightMode"="ForwardBase" }

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include <UnityShaderUtilities.cginc>
                   #include "Lighting.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
               float4 tangent :TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 color : COLOR0;
                float3 normalColor:COLOR1;
            float3 lightDir :TEXCOORD1;
            float3 viewDir :TEXCOORD2;
                
            };

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float _Threshold1;
            float _Threshold2;
            float _Rate;

            v2f vert (appdata v)
            {
                v2f o;
                _Threshold1*= _Rate;
                _Threshold2*= _Rate;
                //MVP
                o.vertex = UnityObjectToClipPos(v.vertex);

                float height = v.vertex.y; // Get Vertex Height
                float3 color;

                // Set Color based on the height
                if (height < _Threshold1)
                    color = _Color1.rgb;
                else if (height < _Threshold2)
                    color = _Color2.rgb;
                else
                    color = _Color3.rgb;

                o.color = color;
                o.normalColor=v.normal.x * 0.5 + fixed3(0.5, 0.5, 0.5);

                TANGENT_SPACE_ROTATION;
                o.lightDir = mul(rotation,ObjSpaceLightDir(v.vertex)).xyz;
                o.viewDir = mul(rotation,ObjSpaceViewDir(v.vertex)).xyz;
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                
                // fixed3 tangentLightDir = normalize(i.lightDir);
                // fixed3 tangentViewDir = normalize(i.viewDir);
                
                float4 color = fixed4(i.color, 1.0);
                //Check which of the three colour is the closest
                float3 diff1 = abs(color - _Color1);
                float3 diff2 = abs(color - _Color2);
                float3 diff3 = abs(color - _Color3);
                
                float diffMin = min(min(diff1.r + diff1.g + diff1.b, diff2.r + diff2.g + diff2.b), diff3.r + diff3.g + diff3.b);
                
                if (diffMin == diff1.r + diff1.g + diff1.b && diffMin<0.001)
                color=_Color1;
                
                else if(diffMin == diff3.r + diff3.g + diff3.b&&diffMin<0.001)
                color= _Color3;
                else
                {
                     color= _Color2;
                }
                color*=fixed4(i.normalColor,1.0) ;

                 fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT.xyz *color;
                 fixed3 diffuse = _LightColor0*color;
                return fixed4( ambient,1.0)+fixed4( diffuse,1.0);
            }
            ENDCG
        }
    }
    
    FallBack "Diffuse"
}
