using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering;
using UnityEngine;

namespace UnityEngine.Rendering
{
    /// <summary>
    /// List of graphics commands to execute.
    /// </summary>
    /// <description>
    /// Command buffers hold list of rendering commands ("set render target, draw mesh, ..."). They can be set to execute at various points during camera rendering (see [[Camera.AddCommandBuffer]]), light rendering (see [[Light.AddCommandBuffer]]) or be executed immediately (see [[Graphics.ExecuteCommandBuffer]]).
    /// Typically they would be used to extend Unity's rendering pipeline in some custom ways. For example, you could render some additional objects into deferred rendering g-buffer after all regular objects are done, or do custom processing of light
    /// shadow maps. See [[wiki:GraphicsCommandBuffers|command buffers overview]] page for more details.
    /// Command buffers can be created and then executed many times if needed.
    /// SA: [[Camera.AddCommandBuffer]], [[Light.AddCommandBuffer]], [[Rendering.CameraEvent]], [[Rendering.LightEvent]], [[Graphics.ExecuteCommandBuffer]], [[wiki:GraphicsCommandBuffers|command buffers overview]].
    /// </description>
    [NativeHeader("Runtime/Shaders/ComputeShader.h")]
    [NativeHeader("Runtime/Export/Graphics/RenderingCommandBuffer.bindings.h")]
    [NativeType("Runtime/Graphics/CommandBuffer/RenderingCommandBuffer.h")]
    [UsedByNativeCode]
    public partial class CommandBuffer : IDisposable
    {
        /// <summary>
        /// Converts and copies a source texture to a destination texture with a different format or dimensions.
        /// </summary>
        /// <param name="src">
        /// Source texture.
        /// </param>
        /// <param name="dst">
        /// Destination texture.
        /// </param>
        /// <description>
        /// This function provides an efficient way to convert between textures of different formats and dimensions. The destination texture format must be uncompressed and correspond to a [[RenderTextureFormat]] supported on the current device. You can use 2D and cubemap textures as the source and 2D, cubemap, 2D array and cubemap array textures as the destination.
        /// Note that due to API limitations, this function is not supported on DX9 or Mac+OpenGL.
        /// </description>
        public void ConvertTexture(RenderTargetIdentifier src, RenderTargetIdentifier dst)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            ConvertTexture_Internal(src, 0, dst, 0);
        }

        /// <summary>
        /// Converts and copies a source texture to a destination texture with a different format or dimensions.
        /// </summary>
        /// <param name="src">
        /// Source texture.
        /// </param>
        /// <param name="srcElement">
        /// Source element (e.g. cubemap face). Set this to 0 for 2D source textures.
        /// </param>
        /// <param name="dst">
        /// Destination texture.
        /// </param>
        /// <param name="dstElement">
        /// Destination element (e.g. cubemap face or texture array element).
        /// </param>
        /// <description>
        /// This function provides an efficient way to convert between textures of different formats and dimensions. The destination texture format must be uncompressed and correspond to a [[RenderTextureFormat]] supported on the current device. You can use 2D and cubemap textures as the source and 2D, cubemap, 2D array and cubemap array textures as the destination.
        /// Note that due to API limitations, this function is not supported on DX9 or Mac+OpenGL.
        /// </description>
        public void ConvertTexture(RenderTargetIdentifier src, int srcElement, RenderTargetIdentifier dst, int dstElement)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            ConvertTexture_Internal(src, srcElement, dst, dstElement);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(ComputeBuffer src, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_1(src, callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(ComputeBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_2(src, size, offset, callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(Texture src, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_3(src, callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(Texture src, int mipIndex, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_4(src, mipIndex, callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(Texture src, int mipIndex, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_5(src, mipIndex, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(Texture src, int mipIndex, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_5(src, mipIndex, dstFormat, callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_6(src, mipIndex, x, width, y, height, z, depth, callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_7(src, mipIndex, x, width, y, height, z, depth, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void RequestAsyncReadback(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_RequestAsyncReadback_7(src, mipIndex, x, width, y, height, z, depth, dstFormat, callback);
        }

        [NativeMethod("AddRequestAsyncReadback")]
        extern private void Internal_RequestAsyncReadback_1([NotNull] ComputeBuffer src, [NotNull] Action<AsyncGPUReadbackRequest> callback);
        [NativeMethod("AddRequestAsyncReadback")]
        extern private void Internal_RequestAsyncReadback_2([NotNull] ComputeBuffer src, int size, int offset, [NotNull] Action<AsyncGPUReadbackRequest> callback);
        [NativeMethod("AddRequestAsyncReadback")]
        extern private void Internal_RequestAsyncReadback_3([NotNull] Texture src, [NotNull] Action<AsyncGPUReadbackRequest> callback);
        [NativeMethod("AddRequestAsyncReadback")]
        extern private void Internal_RequestAsyncReadback_4([NotNull] Texture src, int mipIndex, [NotNull] Action<AsyncGPUReadbackRequest> callback);
        [NativeMethod("AddRequestAsyncReadback")]
        extern private void Internal_RequestAsyncReadback_5([NotNull] Texture src, int mipIndex, GraphicsFormat dstFormat, [NotNull] Action<AsyncGPUReadbackRequest> callback);
        [NativeMethod("AddRequestAsyncReadback")]
        extern private void Internal_RequestAsyncReadback_6([NotNull] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, [NotNull] Action<AsyncGPUReadbackRequest> callback);
        [NativeMethod("AddRequestAsyncReadback")]
        extern private void Internal_RequestAsyncReadback_7([NotNull] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, [NotNull] Action<AsyncGPUReadbackRequest> callback);


        /// <summary>
        /// Add a "set invert culling" command to the buffer.
        /// </summary>
        /// <param name="invertCulling">
        /// A boolean indicating whether to invert the backface culling (true) or not (false).
        /// </param>
        /// <description>
        /// When the command buffer is executed, the backface culling is either inverted (when invertCulling is set to true) or not (when invertCulling is set to false) (see [[GL.invertCulling]]).
        /// </description>
        [NativeMethod("AddSetInvertCulling")]
        public extern void SetInvertCulling(bool invertCulling);

        extern void ConvertTexture_Internal(RenderTargetIdentifier src, int srcElement, RenderTargetIdentifier dst, int dstElement);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetSinglePassStereo", HasExplicitThis = true)]
        extern private void Internal_SetSinglePassStereo(SinglePassStereoMode mode);

        [FreeFunction("RenderingCommandBuffer_Bindings::InitBuffer")]
        extern private static IntPtr InitBuffer();

        [FreeFunction("RenderingCommandBuffer_Bindings::CreateGPUFence_Internal", HasExplicitThis = true)]
        extern private IntPtr CreateGPUFence_Internal(GraphicsFenceType fenceType, SynchronisationStageFlags stage);

        [FreeFunction("RenderingCommandBuffer_Bindings::WaitOnGPUFence_Internal", HasExplicitThis = true)]
        extern private void WaitOnGPUFence_Internal(IntPtr fencePtr, SynchronisationStageFlags stage);

        [FreeFunction("RenderingCommandBuffer_Bindings::ReleaseBuffer", HasExplicitThis = true, IsThreadSafe = true)]
        extern private void ReleaseBuffer();

        /// <summary>
        /// Adds a command to set a float parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="val">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetComputeFloatParam", HasExplicitThis = true)]
        extern public void SetComputeFloatParam([NotNull] ComputeShader computeShader, int nameID, float val);

        /// <summary>
        /// Adds a command to set an integer parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="val">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetComputeIntParam", HasExplicitThis = true)]
        extern public void SetComputeIntParam([NotNull] ComputeShader computeShader, int nameID, int val);

        /// <summary>
        /// Adds a command to set a vector parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="val">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetComputeVectorParam", HasExplicitThis = true)]
        extern public void SetComputeVectorParam([NotNull] ComputeShader computeShader, int nameID, Vector4 val);

        /// <summary>
        /// Adds a command to set a vector array parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="values">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.µ
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetComputeVectorArrayParam", HasExplicitThis = true)]
        extern public void SetComputeVectorArrayParam([NotNull] ComputeShader computeShader, int nameID, Vector4[] values);

        /// <summary>
        /// Adds a command to set a matrix parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="val">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetComputeMatrixParam", HasExplicitThis = true)]
        extern public void SetComputeMatrixParam([NotNull] ComputeShader computeShader, int nameID, Matrix4x4 val);

        /// <summary>
        /// Adds a command to set a matrix array parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="values">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetComputeMatrixArrayParam", HasExplicitThis = true)]
        extern public void SetComputeMatrixArrayParam([NotNull] ComputeShader computeShader, int nameID, Matrix4x4[] values);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetComputeFloats", HasExplicitThis = true)]
        extern private void Internal_SetComputeFloats([NotNull] ComputeShader computeShader, int nameID, float[] values);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetComputeInts", HasExplicitThis = true)]
        extern private void Internal_SetComputeInts([NotNull] ComputeShader computeShader, int nameID, int[] values);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_SetComputeTextureParam", HasExplicitThis = true)]
        extern private void Internal_SetComputeTextureParam([NotNull] ComputeShader computeShader, int kernelIndex, int nameID, ref UnityEngine.Rendering.RenderTargetIdentifier rt, int mipLevel);

        /// <summary>
        /// Adds a command to set an input or output buffer parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="kernelIndex">
        /// Which kernel the buffer is being set for. See [[ComputeShader.FindKernel]].
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="buffer">
        /// Buffer to set.
        /// </param>
        /// <description>
        /// Buffers and textures are set per-kernel. Use [[ComputeShader.FindKernel]] to find kernel index by function name.
        /// Setting a compute buffer to a kernel will leave the append/consume counter value unchanged. To set or reset the value, use [[ComputeBuffer.SetCounterValue]].
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetComputeBufferParam", HasExplicitThis = true)]
        extern public void SetComputeBufferParam([NotNull] ComputeShader computeShader, int kernelIndex, int nameID, ComputeBuffer buffer);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_DispatchCompute", HasExplicitThis = true, ThrowsException = true)]
        extern private void Internal_DispatchCompute([NotNull] ComputeShader computeShader, int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_DispatchComputeIndirect", HasExplicitThis = true, ThrowsException = true)]
        extern private void Internal_DispatchComputeIndirect([NotNull] ComputeShader computeShader, int kernelIndex, ComputeBuffer indirectBuffer, uint argsOffset);

        [NativeMethod("AddGenerateMips")]
        extern private void Internal_GenerateMips(RenderTexture rt);

        [NativeMethod("AddResolveAntiAliasedSurface")]
        extern private void Internal_ResolveAntiAliasedSurface(RenderTexture rt, RenderTexture target);

        /// <summary>
        /// Adds a command to copy [[ComputeBuffer]] counter value.
        /// </summary>
        /// <param name="src">
        /// Append/consume buffer to copy the counter from.
        /// </param>
        /// <param name="dst">
        /// A buffer to copy the counter to.
        /// </param>
        /// <param name="dstOffsetBytes">
        /// Target byte offset in /dst/ buffer.
        /// </param>
        /// <description>
        /// Note: this command can not be used on [[LightEvent]] command buffers.
        /// SA: [[ComputeBuffer.CopyCount]].
        /// </description>
        [NativeMethod("AddCopyCounterValue")]
        extern public void CopyCounterValue(ComputeBuffer src, ComputeBuffer dst, uint dstOffsetBytes);

        /// <summary>
        /// Name of this command buffer.
        /// </summary>
        /// <description>
        /// This can be used for debugging, so that command buffer activity in Profiler or Frame Debugger can be seen easier. The name does not affect rendering at all.
        /// </description>
        extern public string name { get; set; }
        /// <summary>
        /// Size of this command buffer in bytes (RO).
        /// </summary>
        extern public int sizeInBytes {[NativeMethod("GetBufferSize")] get; }

        /// <summary>
        /// Clear all commands in the buffer.
        /// </summary>
        /// <description>
        /// Removes all previously added commands from the buffer and makes it empty.
        /// </description>
        [NativeMethod("ClearCommands")]
        extern public void Clear();

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawMesh", HasExplicitThis = true)]
        extern private void Internal_DrawMesh([NotNull] Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties);

        [NativeMethod("AddDrawRenderer")]
        extern private void Internal_DrawRenderer([NotNull] Renderer renderer, Material material, int submeshIndex, int shaderPass);

        private void Internal_DrawRenderer(Renderer renderer, Material material, int submeshIndex)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_DrawRenderer(renderer, material, submeshIndex, -1);
        }

        private void Internal_DrawRenderer(Renderer renderer, Material material)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            Internal_DrawRenderer(renderer, material, 0);
        }

        [NativeMethod("AddDrawProcedural")]
        extern private void Internal_DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties);

        [NativeMethod("AddDrawProceduralIndexed")]
        extern private void Internal_DrawProceduralIndexed(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount, int instanceCount, MaterialPropertyBlock properties);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawProceduralIndirect", HasExplicitThis = true)]
        extern private void Internal_DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawProceduralIndexedIndirect", HasExplicitThis = true)]
        extern private void Internal_DrawProceduralIndexedIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawMeshInstanced", HasExplicitThis = true)]
        extern private void Internal_DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawMeshInstancedIndirect", HasExplicitThis = true)]
        extern private void Internal_DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties);

        [FreeFunction("RenderingCommandBuffer_Bindings::Internal_DrawOcclusionMesh", HasExplicitThis = true)]
        extern private void Internal_DrawOcclusionMesh(RectInt normalizedCamViewport);

        [FreeFunction("RenderingCommandBuffer_Bindings::SetRandomWriteTarget_Texture", HasExplicitThis = true, ThrowsException = true)]
        extern private void SetRandomWriteTarget_Texture(int index, ref UnityEngine.Rendering.RenderTargetIdentifier rt);

        [FreeFunction("RenderingCommandBuffer_Bindings::SetRandomWriteTarget_Buffer", HasExplicitThis = true, ThrowsException = true)]
        extern private void SetRandomWriteTarget_Buffer(int index, ComputeBuffer uav, bool preserveCounterValue);

        /// <summary>
        /// Clear random write targets for [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level pixel shaders.
        /// </summary>
        /// <description>
        /// This function clears any "random write" targets that were previously set with SetRandomWriteTarget.
        /// This function clears any "random write" targets that were previously set with SetRandomWriteTarget.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::ClearRandomWriteTargets", HasExplicitThis = true, ThrowsException = true)]
        extern public void ClearRandomWriteTargets();

        /// <summary>
        /// Add a command to set the rendering viewport.
        /// </summary>
        /// <param name="pixelRect">
        /// Viewport rectangle in pixel coordinates.
        /// </param>
        /// <description>
        /// By default after render target changes the viewport is set to encompass the whole render target.
        /// This command can be used to limit further rendering to the specified pixel rectangle.
        /// SA: SetRenderTarget.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetViewport", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetViewport(Rect pixelRect);

        /// <summary>
        /// Add a command to enable the hardware scissor rectangle.
        /// </summary>
        /// <param name="scissor">
        /// Viewport rectangle in pixel coordinates.
        /// </param>
        /// <description>
        /// This command can be used to clip regions of the screen from rendering.
        /// SA: DisableScissorRect.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::EnableScissorRect", HasExplicitThis = true, ThrowsException = true)]
        extern public void EnableScissorRect(Rect scissor);

        /// <summary>
        /// Add a command to disable the hardware scissor rectangle.
        /// </summary>
        /// <description>
        /// SA: EnableScissorRect.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::DisableScissorRect", HasExplicitThis = true, ThrowsException = true)]
        extern public void DisableScissorRect();

        [FreeFunction("RenderingCommandBuffer_Bindings::CopyTexture_Internal", HasExplicitThis = true)]
        extern private void CopyTexture_Internal(ref UnityEngine.Rendering.RenderTargetIdentifier src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight,
            ref UnityEngine.Rendering.RenderTargetIdentifier dst, int dstElement, int dstMip, int dstX, int dstY, int mode);

        [FreeFunction("RenderingCommandBuffer_Bindings::Blit_Texture", HasExplicitThis = true)]
        extern private void Blit_Texture(Texture source, ref UnityEngine.Rendering.RenderTargetIdentifier dest, Material mat, int pass, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice);

        [FreeFunction("RenderingCommandBuffer_Bindings::Blit_Identifier", HasExplicitThis = true)]
        extern private void Blit_Identifier(ref UnityEngine.Rendering.RenderTargetIdentifier source, ref UnityEngine.Rendering.RenderTargetIdentifier dest, Material mat, int pass, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [FreeFunction("RenderingCommandBuffer_Bindings::GetTemporaryRT", HasExplicitThis = true)]
        extern public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite, RenderTextureMemoryless memorylessMode, bool useDynamicScale);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite, RenderTextureMemoryless memorylessMode)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, antiAliasing, enableRandomWrite, memorylessMode, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, antiAliasing, enableRandomWrite, RenderTextureMemoryless.None);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, antiAliasing, false, RenderTextureMemoryless.None);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, GraphicsFormat format)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, 1);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, bool enableRandomWrite, RenderTextureMemoryless memorylessMode, bool useDynamicScale)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), antiAliasing, enableRandomWrite, memorylessMode, useDynamicScale);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, bool enableRandomWrite, RenderTextureMemoryless memorylessMode)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite, memorylessMode, false);
        }

        /// <summary>
        /// Add a "get a temporary render texture" command.
        /// </summary>
        /// <param name="nameID">
        /// Shader property name for this texture.
        /// </param>
        /// <param name="width">
        /// Width in pixels, or -1 for "camera pixel width".
        /// </param>
        /// <param name="height">
        /// Height in pixels, or -1 for "camera pixel height".
        /// </param>
        /// <param name="depthBuffer">
        /// Depth buffer bits (0, 16 or 24).
        /// </param>
        /// <param name="filter">
        /// Texture filtering mode (default is Point).
        /// </param>
        /// <param name="format">
        /// Format of the render texture (default is ARGB32).
        /// </param>
        /// <param name="readWrite">
        /// Color space conversion mode.
        /// </param>
        /// <param name="antiAliasing">
        /// Anti-aliasing (default is no anti-aliasing).
        /// </param>
        /// <param name="enableRandomWrite">
        /// Should random-write access into the texture be enabled (default is false).
        /// </param>
        /// <description>
        /// This creates a temporary render texture with given parameters, and sets it up as a global shader property with nameID. Use [[Shader.PropertyToID]] to create the integer name.
        /// Release the temporary render texture using ReleaseTemporaryRT, passing the same nameID. Any temporary textures that were not explicitly released will be removed after camera is done rendering, or after [[Graphics.ExecuteCommandBuffer]] is done.
        /// After getting a temporary render texture, you can set it as active (.SetRenderTarget) or blit to/from it (.Blit). You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// SA: ReleaseTemporaryRT, SetRenderTarget, Blit.
        /// </description>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, bool enableRandomWrite)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, enableRandomWrite, RenderTextureMemoryless.None);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, antiAliasing, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, format, readWrite, 1);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter, RenderTextureFormat format)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, RenderTextureReadWrite.Default));
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer, FilterMode filter)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, filter, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR));
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height, int depthBuffer)
        {
            GetTemporaryRT(nameID, width, height, depthBuffer, FilterMode.Point);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, int width, int height)
        {
            GetTemporaryRT(nameID, width, height, 0);
        }

        [FreeFunction("RenderingCommandBuffer_Bindings::GetTemporaryRTWithDescriptor", HasExplicitThis = true)]
        extern private void GetTemporaryRTWithDescriptor(int nameID, RenderTextureDescriptor desc, FilterMode filter);

        /// <summary>
        /// Add a "get a temporary render texture" command.
        /// </summary>
        /// <param name="nameID">
        /// Shader property name for this texture.
        /// </param>
        /// <param name="desc">
        /// Use this RenderTextureDescriptor for the settings when creating the temporary RenderTexture.
        /// </param>
        /// <param name="filter">
        /// Texture filtering mode (default is Point).
        /// </param>
        /// <description>
        /// This creates a temporary render texture with given parameters, and sets it up as a global shader property with nameID. Use [[Shader.PropertyToID]] to create the integer name.
        /// Release the temporary render texture using ReleaseTemporaryRT, passing the same nameID. Any temporary textures that were not explicitly released will be removed after camera is done rendering, or after [[Graphics.ExecuteCommandBuffer]] is done.
        /// After getting a temporary render texture, you can set it as active (.SetRenderTarget) or blit to/from it (.Blit). You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// SA: ReleaseTemporaryRT, SetRenderTarget, Blit.
        /// </description>
        public void GetTemporaryRT(int nameID, RenderTextureDescriptor desc, FilterMode filter)
        {
            GetTemporaryRTWithDescriptor(nameID, desc, filter);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRT(int nameID, RenderTextureDescriptor desc)
        {
            GetTemporaryRT(nameID, desc, FilterMode.Point);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [FreeFunction("RenderingCommandBuffer_Bindings::GetTemporaryRTArray", HasExplicitThis = true)]
        extern public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite, bool useDynamicScale);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing, bool enableRandomWrite)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, format, antiAliasing, enableRandomWrite, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, GraphicsFormat format, int antiAliasing)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, format, antiAliasing, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, GraphicsFormat format)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, format, 1);
        }

        /// <summary>
        /// Add a "get a temporary render texture array" command.
        /// </summary>
        /// <param name="nameID">
        /// Shader property name for this texture.
        /// </param>
        /// <param name="width">
        /// Width in pixels, or -1 for "camera pixel width".
        /// </param>
        /// <param name="height">
        /// Height in pixels, or -1 for "camera pixel height".
        /// </param>
        /// <param name="slices">
        /// Number of slices in texture array.
        /// </param>
        /// <param name="depthBuffer">
        /// Depth buffer bits (0, 16 or 24).
        /// </param>
        /// <param name="filter">
        /// Texture filtering mode (default is Point).
        /// </param>
        /// <param name="format">
        /// Format of the render texture (default is ARGB32).
        /// </param>
        /// <param name="readWrite">
        /// Color space conversion mode.
        /// </param>
        /// <param name="antiAliasing">
        /// Anti-aliasing (default is no anti-aliasing).
        /// </param>
        /// <param name="enableRandomWrite">
        /// Should random-write access into the texture be enabled (default is false).
        /// </param>
        /// <description>
        /// This creates a temporary render texture array with given parameters, and sets it up as a global shader property with nameID. Use [[Shader.PropertyToID]] to create the integer name.
        /// Release the temporary render texture array using ReleaseTemporaryRT, passing the same nameID. Any temporary textures that were not explicitly released will be removed after camera is done rendering, or after [[Graphics.ExecuteCommandBuffer]] is done.
        /// After getting a temporary render texture array, you can set it as active (.SetRenderTarget) or blit to/from it (.Blit). You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// SA: ReleaseTemporaryRT, SetRenderTarget, Blit.
        /// </description>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, bool enableRandomWrite)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), antiAliasing, enableRandomWrite, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), antiAliasing, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, RenderTextureFormat format, RenderTextureReadWrite readWrite)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), 1, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter, RenderTextureFormat format)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, GraphicsFormatUtility.GetGraphicsFormat(format, RenderTextureReadWrite.Default), 1, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer, FilterMode filter)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, filter, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR), 1, false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices, int depthBuffer)
        {
            GetTemporaryRTArray(nameID, width, height, slices, depthBuffer, FilterMode.Point);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetTemporaryRTArray(int nameID, int width, int height, int slices)
        {
            GetTemporaryRTArray(nameID, width, height, slices, 0);
        }

        /// <summary>
        /// Add a "release a temporary render texture" command.
        /// </summary>
        /// <param name="nameID">
        /// Shader property name for this texture.
        /// </param>
        /// <description>
        /// Releases a temporary render texture with given name. Presumably you have called ::GetTemporaryRT to create it before.
        /// Any temporary textures that were not explicitly released will be removed after camera is done rendering, or after [[Graphics.ExecuteCommandBuffer]] is done.
        /// SA: GetTemporaryRT.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::ReleaseTemporaryRT", HasExplicitThis = true)]
        extern public void ReleaseTemporaryRT(int nameID);

        /// <summary>
        /// Adds a "clear render target" command.
        /// </summary>
        /// <param name="clearDepth">
        /// Should clear depth buffer?
        /// </param>
        /// <param name="clearColor">
        /// Should clear color buffer?
        /// </param>
        /// <param name="backgroundColor">
        /// Color to clear with.
        /// </param>
        /// <param name="depth">
        /// Depth to clear with (default is 1.0).
        /// </param>
        [FreeFunction("RenderingCommandBuffer_Bindings::ClearRenderTarget", HasExplicitThis = true)]
        extern public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor, float depth);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void ClearRenderTarget(bool clearDepth, bool clearColor, Color backgroundColor)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            ClearRenderTarget(clearDepth, clearColor, backgroundColor, 1.0f);
        }

        /// <summary>
        /// Add a "set global shader float property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader float property will be set at this point. The effect is as if [[Shader.SetGlobalFloat]] was called.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalFloat", HasExplicitThis = true)]
        extern public void SetGlobalFloat(int nameID, float value);

        /// <summary>
        /// Sets the given global integer property for all shaders.
        /// </summary>
        /// <description>
        /// Internally, float and integer shader properties are treated exactly the same, so this function is just an alias to SetGlobalFloat.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalInt", HasExplicitThis = true)]
        extern public void SetGlobalInt(int nameID, int value);

        /// <summary>
        /// Add a "set global shader vector property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader vector property will be set at this point. The effect is as if [[Shader.SetGlobalVector]] was called.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalVector", HasExplicitThis = true)]
        extern public void SetGlobalVector(int nameID, Vector4 value);

        /// <summary>
        /// Add a "set global shader color property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader color property will be set at this point. The effect is as if [[Shader.SetGlobalColor]] was called.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalColor", HasExplicitThis = true)]
        extern public void SetGlobalColor(int nameID, Color value);

        /// <summary>
        /// Add a "set global shader matrix property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader matrix property will be set at this point. The effect is as if [[Shader.SetGlobalMatrix]] was called.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalMatrix", HasExplicitThis = true)]
        extern public void SetGlobalMatrix(int nameID, Matrix4x4 value);

        /// <summary>
        /// Adds a command to enable global shader keyword.
        /// </summary>
        /// <param name="keyword">
        /// Shader keyword to enable.
        /// </param>
        /// <description>
        /// SA: DisableShaderKeyword, [[Shader.EnableKeyword]].
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::EnableShaderKeyword", HasExplicitThis = true)]
        extern public void EnableShaderKeyword(string keyword);

        /// <summary>
        /// Adds a command to disable global shader keyword.
        /// </summary>
        /// <param name="keyword">
        /// Shader keyword to disable.
        /// </param>
        /// <description>
        /// SA: EnableShaderKeyword, [[Shader.DisableKeyword]].
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::DisableShaderKeyword", HasExplicitThis = true)]
        extern public void DisableShaderKeyword(string keyword);

        /// <summary>
        /// Add a command to set the view matrix.
        /// </summary>
        /// <param name="view">
        /// View (world to camera space) matrix.
        /// </param>
        /// <description>
        /// View matrix is the matrix that transforms from world space into camera space.
        /// Note that when setting both view and projection matrices, it is slightly more efficient to use SetViewProjectionMatrices.
        /// SA: SetProjectionMatrix, SetViewProjectionMatrices, [[Camera.worldToCameraMatrix]].
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetViewMatrix", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetViewMatrix(Matrix4x4 view);

        /// <summary>
        /// Add a command to set the projection matrix.
        /// </summary>
        /// <param name="proj">
        /// Projection (camera to clip space) matrix.
        /// </param>
        /// <description>
        /// View matrix is the matrix that transforms from view space into homogeneous clip space.
        /// Note that when setting both view and projection matrices, it is slightly more efficient to use SetViewProjectionMatrices.
        /// SA: SetViewMatrix, SetViewProjectionMatrices, [[Camera.projectionMatrix]], [[Matrix4x4.Perspective]].
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetProjectionMatrix", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetProjectionMatrix(Matrix4x4 proj);

        /// <summary>
        /// Add a command to set the view and projection matrices.
        /// </summary>
        /// <param name="view">
        /// View (world to camera space) matrix.
        /// </param>
        /// <param name="proj">
        /// Projection (camera to clip space) matrix.
        /// </param>
        /// <description>
        /// This function is equivalent to calling SetViewMatrix and SetProjectionMatrix. It is slightly more efficient when changing both matrices at once.
        /// SA: SetViewMatrix, SetProjectionMatrix, [[Camera.projectionMatrix]], [[Matrix4x4.Perspective]].
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetViewProjectionMatrices", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetViewProjectionMatrices(Matrix4x4 view, Matrix4x4 proj);

        /// <summary>
        /// Add a command to set global depth bias.
        /// </summary>
        /// <param name="bias">
        /// Constant depth bias.
        /// </param>
        /// <param name="slopeBias">
        /// Slope-dependent depth bias.
        /// </param>
        /// <description>
        /// Global depth bias is added to the state specified in shaders (see [[wiki:SL-CullAndDepth|cull and depth state]]). This is typically useful
        /// when rendering shadow caster objects, to slightly push them away from the light source in order to prevent shadow acne. Built-in
        /// shadow caster rendering in Unity sets global depth bias of (1.0, 1.0) when rendering the shadow casters.
        /// </description>
        [NativeMethod("AddSetGlobalDepthBias")]
        extern public void SetGlobalDepthBias(float bias, float slopeBias);

        /// <summary>
        /// Set flags describing the intention for how the command buffer will be executed.
        /// </summary>
        /// <param name="flags">
        /// The flags to set.
        /// </param>
        /// <description>
        /// Setting execution flags to any value other than none allows exceptions to be thrown when issuing commands that are not compatible with the intended method of execution.
        /// For example, command buffers intended for use with async compute cannot contain commands that are used purely for rendering.
        /// This method can only be called against empty command buffers, so call it immediately after construction or after calling [[CommandBuffer.Clear]].
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetExecutionFlags", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetExecutionFlags(CommandBufferExecutionFlags flags);

        [FreeFunction("RenderingCommandBuffer_Bindings::ValidateAgainstExecutionFlags", HasExplicitThis = true, ThrowsException = true)]
        extern private bool ValidateAgainstExecutionFlags(CommandBufferExecutionFlags requiredFlags, CommandBufferExecutionFlags invalidFlags);

        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalFloatArrayListImpl", HasExplicitThis = true)]
        extern private void SetGlobalFloatArrayListImpl(int nameID, object values);

        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalVectorArrayListImpl", HasExplicitThis = true)]
        extern private void SetGlobalVectorArrayListImpl(int nameID, object values);

        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalMatrixArrayListImpl", HasExplicitThis = true)]
        extern private void SetGlobalMatrixArrayListImpl(int nameID, object values);

        /// <summary>
        /// Add a "set global shader float array property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader float array property will be set at this point. The effect is as if [[Shader.SetGlobalFloatArray]] was called.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalFloatArray", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetGlobalFloatArray(int nameID, float[] values);

        /// <summary>
        /// Add a "set global shader vector array property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader vector array property will be set at this point. The effect is as if [[Shader.SetGlobalVectorArray]] was called.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalVectorArray", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetGlobalVectorArray(int nameID, Vector4[] values);

        /// <summary>
        /// Add a "set global shader matrix array property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader matrix array property will be set at this point. The effect is as if [[Shader.SetGlobalMatrixArray]] was called.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalMatrixArray", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetGlobalMatrixArray(int nameID, Matrix4x4[] values);

        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalTexture_Impl", HasExplicitThis = true)]
        extern private void SetGlobalTexture_Impl(int nameID, ref UnityEngine.Rendering.RenderTargetIdentifier rt);

        /// <summary>
        /// Add a "set global shader buffer property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader buffer property will be set at this point. The effect is as if [[Shader.SetGlobalBuffer]] was called.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalBuffer", HasExplicitThis = true)]
        extern public void SetGlobalBuffer(int nameID, ComputeBuffer value);

        [FreeFunction("RenderingCommandBuffer_Bindings::SetShadowSamplingMode_Impl", HasExplicitThis = true)]
        extern private void SetShadowSamplingMode_Impl(ref UnityEngine.Rendering.RenderTargetIdentifier shadowmap, ShadowSamplingMode mode);

        [FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginEventInternal", HasExplicitThis = true)]
        extern private void IssuePluginEventInternal(IntPtr callback, int eventID);

        /// <summary>
        /// Adds a command to begin profile sampling.
        /// </summary>
        /// <param name="name">
        /// Name of the profile information used for sampling.
        /// </param>
        /// <description>
        /// Schedules a performance profiling to end when the command buffer execution reaches this point. This is useful for measuring CPU and GPU time spent by one or more commands in the command buffer.
        /// A sampling started with BeginSample always has to be ended with a corresponding call to [[CommandBuffer.EndSample]] with the same name argument.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::BeginSample", HasExplicitThis = true)]
        extern public void BeginSample(string name);

        /// <summary>
        /// Adds a command to begin profile sampling.
        /// </summary>
        /// <param name="name">
        /// Name of the profile information used for sampling.
        /// </param>
        /// <description>
        /// Schedules a performance profiling to end when the command buffer execution reaches this point. This is useful for measuring CPU and GPU time spent by one or more commands in the command buffer.
        /// A sampling started with BeginSample always has to be ended with a corresponding call to [[CommandBuffer.EndSample]] with the same name argument.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::EndSample", HasExplicitThis = true)]
        extern public void EndSample(string name);

        [FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginEventAndDataInternal", HasExplicitThis = true)]
        extern private void IssuePluginEventAndDataInternal(IntPtr callback, int eventID, IntPtr data);

        [FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginCustomBlitInternal", HasExplicitThis = true)]
        extern private void IssuePluginCustomBlitInternal(IntPtr callback, uint command, ref UnityEngine.Rendering.RenderTargetIdentifier source, ref UnityEngine.Rendering.RenderTargetIdentifier dest, uint commandParam, uint commandFlags);

        [FreeFunction("RenderingCommandBuffer_Bindings::IssuePluginCustomTextureUpdateInternal", HasExplicitThis = true)]
        extern private void IssuePluginCustomTextureUpdateInternal(IntPtr callback, Texture targetTexture, uint userData, bool useNewUnityRenderingExtTextureUpdateParamsV2);

        /// <summary>
        /// Add a command to bind a global constant buffer.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to bind.
        /// </param>
        /// <param name="nameID">
        /// Name of the constant buffer (use [[Shader.PropertyToID]] to generate).
        /// </param>
        /// <param name="offset">
        /// Offset from the start of the ComputeBuffer in bytes.
        /// </param>
        /// <param name="size">
        /// Size in bytes of the area to bind.
        /// </param>
        /// <description>
        /// See [[Shader.SetGlobalConstantBuffer]] for usage.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::SetGlobalConstantBuffer", HasExplicitThis = true)]
        extern public void SetGlobalConstantBuffer(ComputeBuffer buffer, int nameID, int offset, int size);

        /// <summary>
        /// Increments the updateCount property of a Texture.
        /// </summary>
        /// <param name="dest">
        /// Increments the updateCount for this Texture.
        /// </param>
        /// <description>
        /// This is useful if the execution of a command buffer mutates a Texture because you can rely on the Texture's updateCount to detect any changes.
        /// </description>
        [FreeFunction("RenderingCommandBuffer_Bindings::IncrementUpdateCount", HasExplicitThis = true)]
        extern public void IncrementUpdateCount(UnityEngine.Rendering.RenderTargetIdentifier dest);

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="rt">
        /// Render target to set for both color & depth buffers.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier rt)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            SetRenderTargetSingle_Internal(rt, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="rt">
        /// Render target to set for both color & depth buffers.
        /// </param>
        /// <param name="loadAction">
        /// Load action that is used for color and depth/stencil buffers.
        /// </param>
        /// <param name="storeAction">
        /// Store action that is used for color and depth/stencil buffers.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier rt, RenderBufferLoadAction loadAction, RenderBufferStoreAction storeAction)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (loadAction == RenderBufferLoadAction.Clear)
                throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
            SetRenderTargetSingle_Internal(rt, loadAction, storeAction, loadAction, storeAction);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="rt">
        /// Render target to set for both color & depth buffers.
        /// </param>
        /// <param name="colorLoadAction">
        /// Load action that is used for the color buffer.
        /// </param>
        /// <param name="colorStoreAction">
        /// Store action that is used for the color buffer.
        /// </param>
        /// <param name="depthLoadAction">
        /// Load action that is used for the depth/stencil buffer.
        /// </param>
        /// <param name="depthStoreAction">
        /// Store action that is used for the depth/stencil buffer.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier rt,
            RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction,
            RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (colorLoadAction == RenderBufferLoadAction.Clear || depthLoadAction == RenderBufferLoadAction.Clear)
                throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
            SetRenderTargetSingle_Internal(rt, colorLoadAction, colorStoreAction, depthLoadAction, depthStoreAction);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="rt">
        /// Render target to set for both color & depth buffers.
        /// </param>
        /// <param name="mipLevel">
        /// The mip level of the render target to render into.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (mipLevel < 0)
                throw new ArgumentException(String.Format("Invalid value for mipLevel ({0})", mipLevel));
            SetRenderTargetSingle_Internal(new RenderTargetIdentifier(rt, mipLevel),
                RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="rt">
        /// Render target to set for both color & depth buffers.
        /// </param>
        /// <param name="mipLevel">
        /// The mip level of the render target to render into.
        /// </param>
        /// <param name="cubemapFace">
        /// The cubemap face of a cubemap render target to render into.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (mipLevel < 0)
                throw new ArgumentException(String.Format("Invalid value for mipLevel ({0})", mipLevel));
            SetRenderTargetSingle_Internal(new RenderTargetIdentifier(rt, mipLevel, cubemapFace),
                RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="rt">
        /// Render target to set for both color & depth buffers.
        /// </param>
        /// <param name="mipLevel">
        /// The mip level of the render target to render into.
        /// </param>
        /// <param name="cubemapFace">
        /// The cubemap face of a cubemap render target to render into.
        /// </param>
        /// <param name="depthSlice">
        /// Slice of a 3D or array render target to set.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier rt, int mipLevel, CubemapFace cubemapFace, int depthSlice)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (depthSlice < -1)
                throw new ArgumentException(String.Format("Invalid value for depthSlice ({0})", depthSlice));
            if (mipLevel < 0)
                throw new ArgumentException(String.Format("Invalid value for mipLevel ({0})", mipLevel));
            SetRenderTargetSingle_Internal(new RenderTargetIdentifier(rt, mipLevel, cubemapFace, depthSlice),
                RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="color">
        /// Render target to set as a color buffer.
        /// </param>
        /// <param name="depth">
        /// Render target to set as a depth buffer.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            SetRenderTargetColorDepth_Internal(color, depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="color">
        /// Render target to set as a color buffer.
        /// </param>
        /// <param name="depth">
        /// Render target to set as a depth buffer.
        /// </param>
        /// <param name="mipLevel">
        /// The mip level of the render target to render into.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (mipLevel < 0)
                throw new ArgumentException(String.Format("Invalid value for mipLevel ({0})", mipLevel));
            SetRenderTargetColorDepth_Internal(new RenderTargetIdentifier(color, mipLevel),
                depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="color">
        /// Render target to set as a color buffer.
        /// </param>
        /// <param name="depth">
        /// Render target to set as a depth buffer.
        /// </param>
        /// <param name="mipLevel">
        /// The mip level of the render target to render into.
        /// </param>
        /// <param name="cubemapFace">
        /// The cubemap face of a cubemap render target to render into.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (mipLevel < 0)
                throw new ArgumentException(String.Format("Invalid value for mipLevel ({0})", mipLevel));
            SetRenderTargetColorDepth_Internal(new RenderTargetIdentifier(color, mipLevel, cubemapFace),
                depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="color">
        /// Render target to set as a color buffer.
        /// </param>
        /// <param name="depth">
        /// Render target to set as a depth buffer.
        /// </param>
        /// <param name="mipLevel">
        /// The mip level of the render target to render into.
        /// </param>
        /// <param name="cubemapFace">
        /// The cubemap face of a cubemap render target to render into.
        /// </param>
        /// <param name="depthSlice">
        /// Slice of a 3D or array render target to set.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier color, RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace, int depthSlice)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (depthSlice < -1)
                throw new ArgumentException(String.Format("Invalid value for depthSlice ({0})", depthSlice));
            if (mipLevel < 0)
                throw new ArgumentException(String.Format("Invalid value for mipLevel ({0})", mipLevel));
            SetRenderTargetColorDepth_Internal(new RenderTargetIdentifier(color, mipLevel, cubemapFace, depthSlice),
                depth, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="color">
        /// Render target to set as a color buffer.
        /// </param>
        /// <param name="colorLoadAction">
        /// Load action that is used for the color buffer.
        /// </param>
        /// <param name="colorStoreAction">
        /// Store action that is used for the color buffer.
        /// </param>
        /// <param name="depth">
        /// Render target to set as a depth buffer.
        /// </param>
        /// <param name="depthLoadAction">
        /// Load action that is used for the depth/stencil buffer.
        /// </param>
        /// <param name="depthStoreAction">
        /// Store action that is used for the depth/stencil buffer.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier color, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction,
            RenderTargetIdentifier depth, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (colorLoadAction == RenderBufferLoadAction.Clear || depthLoadAction == RenderBufferLoadAction.Clear)
                throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
            SetRenderTargetColorDepth_Internal(color, depth, colorLoadAction, colorStoreAction, depthLoadAction, depthStoreAction);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="colors">
        /// Render targets to set as color buffers (MRT).
        /// </param>
        /// <param name="depth">
        /// Render target to set as a depth buffer.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier[] colors, Rendering.RenderTargetIdentifier depth)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (colors.Length < 1)
                throw new ArgumentException(string.Format("colors.Length must be at least 1, but was {0}", colors.Length));
            if (colors.Length > SystemInfo.supportedRenderTargetCount)
                throw new ArgumentException(string.Format("colors.Length is {0} and exceeds the maximum number of supported render targets ({1})", colors.Length, SystemInfo.supportedRenderTargetCount));

            SetRenderTargetMulti_Internal(colors, depth, null, null, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="colors">
        /// Render targets to set as color buffers (MRT).
        /// </param>
        /// <param name="depth">
        /// Render target to set as a depth buffer.
        /// </param>
        /// <param name="mipLevel">
        /// The mip level of the render target to render into.
        /// </param>
        /// <param name="cubemapFace">
        /// The cubemap face of a cubemap render target to render into.
        /// </param>
        /// <param name="depthSlice">
        /// Slice of a 3D or array render target to set.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetIdentifier[] colors, Rendering.RenderTargetIdentifier depth, int mipLevel, CubemapFace cubemapFace, int depthSlice)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (colors.Length < 1)
                throw new ArgumentException(string.Format("colors.Length must be at least 1, but was {0}", colors.Length));
            if (colors.Length > SystemInfo.supportedRenderTargetCount)
                throw new ArgumentException(string.Format("colors.Length is {0} and exceeds the maximum number of supported render targets ({1})", colors.Length, SystemInfo.supportedRenderTargetCount));
            SetRenderTargetMultiSubtarget(colors, depth, null, null, RenderBufferLoadAction.Load, RenderBufferStoreAction.Store, mipLevel, cubemapFace, depthSlice);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <param name="mipLevel">
        /// The mip level of the render target to render into.
        /// </param>
        /// <param name="cubemapFace">
        /// The cubemap face of a cubemap render target to render into.
        /// </param>
        /// <param name="depthSlice">
        /// Slice of a 3D or array render target to set.
        /// </param>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetBinding binding, int mipLevel, CubemapFace cubemapFace, int depthSlice)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (binding.colorRenderTargets.Length < 1)
                throw new ArgumentException(string.Format("The number of color render targets must be at least 1, but was {0}", binding.colorRenderTargets.Length));
            if (binding.colorRenderTargets.Length > SystemInfo.supportedRenderTargetCount)
                throw new ArgumentException(string.Format("The number of color render targets ({0}) and exceeds the maximum supported number of render targets ({1})", binding.colorRenderTargets.Length, SystemInfo.supportedRenderTargetCount));
            if (binding.colorLoadActions.Length != binding.colorRenderTargets.Length)
                throw new ArgumentException(string.Format("The number of color load actions provided ({0}) does not match the number of color render targets ({1})", binding.colorLoadActions.Length, binding.colorRenderTargets.Length));
            if (binding.colorStoreActions.Length != binding.colorRenderTargets.Length)
                throw new ArgumentException(string.Format("The number of color store actions provided ({0}) does not match the number of color render targets ({1})", binding.colorLoadActions.Length, binding.colorRenderTargets.Length));
            if (binding.depthLoadAction == RenderBufferLoadAction.Clear || Array.IndexOf(binding.colorLoadActions, RenderBufferLoadAction.Clear) > -1)
                throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");
            if (binding.colorRenderTargets.Length == 1) // non-MRT case respects mip/face/slice of color's RenderTargetIdentifier
                SetRenderTargetColorDepthSubtarget(binding.colorRenderTargets[0], binding.depthRenderTarget, binding.colorLoadActions[0], binding.colorStoreActions[0], binding.depthLoadAction, binding.depthStoreAction, mipLevel, cubemapFace, depthSlice);
            else
                SetRenderTargetMultiSubtarget(binding.colorRenderTargets, binding.depthRenderTarget, binding.colorLoadActions, binding.colorStoreActions, binding.depthLoadAction, binding.depthStoreAction, mipLevel, cubemapFace, depthSlice);
        }

        /// <summary>
        /// Add a "set active render target" command.
        /// </summary>
        /// <description>
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// You do not explicitly need to preserve active render targets during command buffer execution (current render targets are saved & restored afterwards).
        /// Variations of this method are available which take extra arguments such as mipLevel (int) and cubemapFace to enable rendering into a specific mipmap level of a RenderTexture, or specific cubemap face of a cubemap RenderTexture.
        /// Overloads setting a single RenderTarget and without explicit mipLevel, cubemapFace and depthSlice respect the mipLevel, cubemapFace and depthSlice values that were specified when creating the [[Rendering.RenderTargetIdentifier]].
        /// Overloads setting multiple render targets will set mipLevel, cubemapFace, and depthSlice to 0, Unknown, and 0 unless otherwise specified.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// [[Rendering.RenderTargetIdentifier.Clear]] is currently not supported. A subsequent call to ClearRenderTarget has the same effect and is optimized on graphics APIs that support /clear/ load actions.
        /// SA: GetTemporaryRT, ClearRenderTarget, Blit, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void SetRenderTarget(RenderTargetBinding binding)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);
            if (binding.colorRenderTargets.Length < 1)
                throw new ArgumentException(string.Format("The number of color render targets must be at least 1, but was {0}", binding.colorRenderTargets.Length));
            if (binding.colorRenderTargets.Length > SystemInfo.supportedRenderTargetCount)
                throw new ArgumentException(string.Format("The number of color render targets ({0}) and exceeds the maximum supported number of render targets ({1})", binding.colorRenderTargets.Length, SystemInfo.supportedRenderTargetCount));
            if (binding.colorLoadActions.Length != binding.colorRenderTargets.Length)
                throw new ArgumentException(string.Format("The number of color load actions provided ({0}) does not match the number of color render targets ({1})", binding.colorLoadActions.Length, binding.colorRenderTargets.Length));
            if (binding.colorStoreActions.Length != binding.colorRenderTargets.Length)
                throw new ArgumentException(string.Format("The number of color store actions provided ({0}) does not match the number of color render targets ({1})", binding.colorLoadActions.Length, binding.colorRenderTargets.Length));
            if (binding.depthLoadAction == RenderBufferLoadAction.Clear || Array.IndexOf(binding.colorLoadActions, RenderBufferLoadAction.Clear) > -1)
                throw new ArgumentException("RenderBufferLoadAction.Clear is not supported");

            if (binding.colorRenderTargets.Length == 1) // non-MRT case respects mip/face/slice of color's RenderTargetIdentifier
                SetRenderTargetColorDepth_Internal(binding.colorRenderTargets[0], binding.depthRenderTarget, binding.colorLoadActions[0], binding.colorStoreActions[0], binding.depthLoadAction, binding.depthStoreAction);
            else
                SetRenderTargetMulti_Internal(binding.colorRenderTargets, binding.depthRenderTarget, binding.colorLoadActions, binding.colorStoreActions, binding.depthLoadAction, binding.depthStoreAction);
        }

        extern private void SetRenderTargetSingle_Internal(RenderTargetIdentifier rt,
            RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction,
            RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction);

        extern private void SetRenderTargetColorDepth_Internal(RenderTargetIdentifier color, RenderTargetIdentifier depth,
            RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction,
            RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction);

        extern private void SetRenderTargetMulti_Internal(RenderTargetIdentifier[] colors, Rendering.RenderTargetIdentifier depth,
            RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions,
            RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction);
        extern private void SetRenderTargetColorDepthSubtarget(RenderTargetIdentifier color, RenderTargetIdentifier depth,
            RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction,
            RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction,
            int mipLevel, CubemapFace cubemapFace, int depthSlice);
        extern private void SetRenderTargetMultiSubtarget(RenderTargetIdentifier[] colors, Rendering.RenderTargetIdentifier depth,
            RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions,
            RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction,
            int mipLevel, CubemapFace cubemapFace, int depthSlice);
    }
}
