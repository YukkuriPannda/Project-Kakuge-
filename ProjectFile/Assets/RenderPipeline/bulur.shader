Shader "Custom/bulur"
{
    const int BLUR_SAMPLE_COUNT = 4;
    Texture2D tex;
    tex.SetPixel(0,0,In);
    tex.Apply();
    float2 BLUR_KERNEL[BLUR_SAMPLE_COUNT] = {
                    float2(-1.0, -1.0),
                    float2(-1.0, 1.0),
                    float2(1.0, -1.0),
                    float2(1.0, 1.0),
                };

    color = 0;
    for (int j = 0; j < BLUR_SAMPLE_COUNT; j++) 
    {
            color +=  SAMPLE_TEXTURE2D(tex,sam, uv+ BLUR_KERNEL[j] * Scale );
    }
    
    color.rgb /= BLUR_SAMPLE_COUNT;
    color.a = 1;
             
}
