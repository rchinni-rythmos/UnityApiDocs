using System;

namespace UnityEngine.Rendering
{
    public partial class CommandBuffer
    {
        /// <summary>
        /// This functionality is deprecated, and should no longer be used. Please use [[CommandBuffer.CreateGraphicsFence]].
        /// </summary>
        [Obsolete("CommandBuffer.CreateGPUFence has been deprecated. Use CreateGraphicsFence instead (UnityUpgradable) -> CreateAsyncGraphicsFence(*)", false)]
        public GPUFence CreateGPUFence(SynchronisationStage stage) { return new GPUFence(); }

        /// <summary>
        /// This functionality is deprecated, and should no longer be used. Please use [[CommandBuffer.CreateGraphicsFence]].
        /// </summary>
        [Obsolete("CommandBuffer.CreateGPUFence has been deprecated. Use CreateGraphicsFence instead (UnityUpgradable) -> CreateAsyncGraphicsFence()", false)]
        public GPUFence CreateGPUFence() { return new GPUFence(); }

        /// <summary>
        /// This functionality is deprecated, and should no longer be used. Please use [[CommandBuffer.WaitOnAsyncGraphicsFence]].
        /// </summary>
        /// <param name="fence">
        /// The [[GPUFence]] that the GPU will be instructed to wait upon.
        /// </param>
        /// <param name="stage">
        /// On some platforms there is a significant gap between the vertex processing completing and the pixel processing completing for a given draw call. This parameter allows for requested wait to be before the next items vertex or pixel processing begins. Some platforms can not differentiate between the start of vertex and pixel processing, these platforms will wait before the next items vertex processing. If a compute shader dispatch is the next item to be submitted then this parameter is ignored.
        /// </param>
        [Obsolete("CommandBuffer.WaitOnGPUFence has been deprecated. Use WaitOnGraphicsFence instead (UnityUpgradable) -> WaitOnAsyncGraphicsFence(*)", false)]
        public void WaitOnGPUFence(GPUFence fence, SynchronisationStage stage) {}

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("CommandBuffer.WaitOnGPUFence has been deprecated. Use WaitOnGraphicsFence instead (UnityUpgradable) -> WaitOnAsyncGraphicsFence(*)", false)]
        public void WaitOnGPUFence(GPUFence fence) {}
    }
}
