Shader "Unlit/Grass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VertexLight ("Vertex Light", Vector) = (1, 1, 1, 1)
        _ObjectHeight ("Object Height", Float) = 1.0
        _VertexClusterTranslation ("Vertex Cluster Translation", Vector) = (0, 0, 0)
        _TimeStamp ("Timestamp", Float) = 0.0
        _WindDirection ("Wind Direction", Vector) = (1, 0, 0)
        _WindStrength ("Wind Strength", Float) = 1.0
    }

    SubShader
    {
        LOD 0
        Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" "UniversalMaterialType"="Lit" }
       

        Pass
        {
            CGPROGRAM
             #pragma vertex vert
            //#pragma fragment frag
            #include "UnityCG.cginc"
            
            struct VS_INPUT
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float3 objectPosition : TEXCOORD1;
            };

            struct VS_OUTPUT
            {
                float4 pos : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            float3 CalcTranslation(float3 position, float timeStamp, float3 windDirection, float windStrength)
            {
                float3 translation = windDirection * sin(timeStamp + position.y) * windStrength;
                return position + translation;
            }


       
            sampler2D _MainTex;
            float4 _VertexLight;
            float _ObjectHeight;
            float3 _VertexClusterTranslation;
            float _TimeStamp;
            float3 _WindDirection;
            float _WindStrength;

            VS_OUTPUT vert(VS_INPUT v)
            {
                VS_OUTPUT o;
                
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 normal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
                
                if (v.uv.y >= 0.9)
                {
                    worldPos += _VertexClusterTranslation;
                    normal = normalize(v.normal * _ObjectHeight + _VertexClusterTranslation);
                    
                    float3 vertexTranslation = CalcTranslation(worldPos, _TimeStamp, _WindDirection, _WindStrength);
                    worldPos += vertexTranslation;
                    normal = normalize(v.normal * _ObjectHeight + vertexTranslation);
                    
                    float3 objectTranslation = CalcTranslation(v.objectPosition, _TimeStamp, _WindDirection, _WindStrength);
                    worldPos += objectTranslation;
                    normal = normalize(v.normal * _ObjectHeight + objectTranslation);
                }
                
                o.pos = UnityObjectToClipPos(float4(worldPos, 1.0));
                o.color = dot(_VertexLight, float4(normal, 1.0));
                o.uv = v.uv;
                return o;
            }

        fixed4 frag(VS_OUTPUT i) : SV_Target
             { 
                 
                 fixed4 col = tex2D(_MainTex, i.uv);
                 return col;
             }
            
            ENDCG
        }
    }
}

 
         
       

       