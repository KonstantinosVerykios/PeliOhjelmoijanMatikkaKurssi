Shader "Unlit/DebugUvs"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //Textuuri
        //Aallon amplitudi
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            // The objects data that the shader is applied to
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            // Tämä on varryings
            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            //Muuttujat pitä tässä myös tuoda shaderille samalla nimellä

            // Vertex shader part of the code so x,y,z,w
            v2f vert (appdata v)
            {
                // Vertex : POSITION manipuloimalla (xyz) saadaan liike
                // hae x
                // hae y

                // hae vektorin magnitudi
                // hae verteksin kulma 
                // laske aalto



                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // Fragment shader part => Colour r,g,b,a
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                // apply fog

                fixed4 col = fixed4(i.uv.x, i.uv.y, 0, 0);

                UNITY_APPLY_FOG(i.fogCooed, col);

                return col;
            }

            ENDCG
        }
    }
}
