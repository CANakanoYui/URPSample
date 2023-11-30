using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GrayScaleRendererPass : ScriptableRenderPass
{
    private const string ProfilerTag = nameof(GrayScaleRendererPass);
    private readonly Material _material;

    private RTHandle _cameraColorTarget;

    public GrayScaleRendererPass(Shader shader)
    {
        _material = CoreUtils.CreateEngineMaterial(shader);
        renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public void SetRenderTarget(RTHandle rtHandle)
    {
        // シーンカメラのRTHandleを取得
        _cameraColorTarget = rtHandle;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        // シーンビューのカメラが無かったら早期return
        if (renderingData.cameraData.isSceneViewCamera)
        {
            return;
        }

        // コマンドバッファーの取得
        var cmd = CommandBufferPool.Get(ProfilerTag);

        Blitter.BlitCameraTexture(cmd, _cameraColorTarget, _cameraColorTarget, _material, 0);

        // DrawCallが実際に呼ばれる(詰まれたコール全て実行する)
        context.ExecuteCommandBuffer(cmd);

        // コマンドバッファーの開放
        CommandBufferPool.Release(cmd);
    }
}
