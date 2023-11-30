using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GrayScaleRenderFeature : ScriptableRendererFeature
{
    [SerializeField] private Shader _shader;
    
    private GrayScaleRendererPass _pass;
    
    public override void Create()
    {
        _pass = new GrayScaleRendererPass(_shader);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // パスをレンダラーパスに追加
        renderer.EnqueuePass(_pass);
    }
    
    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        _pass.SetRenderTarget(renderer.cameraColorTargetHandle);
    }
}
