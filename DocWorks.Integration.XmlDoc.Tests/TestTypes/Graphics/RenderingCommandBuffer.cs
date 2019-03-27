using System;
using System.Collections.Generic;

namespace UnityEngine.Rendering
{
    public partial class CommandBuffer
    {
 #pragma warning disable 414
        internal IntPtr m_Ptr;
 #pragma warning restore 414

        // --------------------------------------------------------------------
        // IDisposable implementation, with Release() for explicit cleanup.

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        ~CommandBuffer()
        {
            Dispose(false);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // we don't have any managed references, so 'disposing' part of
            // standard IDisposable pattern does not apply

            // Release native resources
            ReleaseBuffer();
            m_Ptr = IntPtr.Zero;
        }

        // --------------------------------------------------------------------
        // Actual API


        /// <summary>
        /// Create a new empty command buffer.
        /// </summary>
        /// <description>
        /// You might want to set name for the buffer, so it is easier to see its activity in Profiler or Frame Debugger.
        /// </description>
        public CommandBuffer()
        {
            //m_Ptr = IntPtr.Zero;
            m_Ptr = InitBuffer();
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void Release()
        {
            Dispose();
        }

        /// <summary>
        /// Shortcut for calling [[GommandBuffer.CreateGraphicsFence]] with [[GraphicsFenceType.AsyncQueueSynchronization]] as the first parameter.
        /// </summary>
        /// <returns>
        /// Returns a new [[GraphicsFence]].
        /// </returns>
        public GraphicsFence CreateAsyncGraphicsFence()
        {
            return CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
        }

        /// <summary>
        /// Shortcut for calling [[GommandBuffer.CreateGraphicsFence]] with [[GraphicsFenceType.AsyncQueueSynchronization]] as the first parameter.
        /// </summary>
        /// <param name="stage">
        /// The synchronization stage. See [[Graphics.CreateGraphicsFence]].
        /// </param>
        /// <returns>
        /// Returns a new [[GraphicsFence]].
        /// </returns>
        public GraphicsFence CreateAsyncGraphicsFence(SynchronisationStage stage)
        {
            return CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, GraphicsFence.TranslateSynchronizationStageToFlags(stage));
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public GraphicsFence CreateGraphicsFence(GraphicsFenceType fenceType, SynchronisationStageFlags stage)
        {
            GraphicsFence newFence = new GraphicsFence();
            newFence.m_Ptr = CreateGPUFence_Internal(fenceType, stage);
            newFence.InitPostAllocation();
            newFence.Validate();
            return newFence;
        }

        /// <summary>
        /// Instructs the GPU to wait until the given [[GraphicsFence]] is passed.
        /// </summary>
        /// <param name="fence">
        /// The [[GraphicsFence]] that the GPU will be instructed to wait upon before proceeding with its processing of the graphics queue.
        /// </param>
        /// <description>
        /// If this [[CommandBuffer]] is executed using [[Graphics.ExecuteCommandBuffer]] or [[ScriptableRenderContext.ExecuteCommandBuffer]] then the processing of the graphics queue will wait.
        /// The [[GraphicsFence]] given as a parameter to this function must have been created with a [[GraphicsFenceType.AsyncQueueSynchronization]] fence type.
        /// If this [[CommandBuffer]] is executed using [[Graphics.ExecuteCommandBufferAsync]] or [[ScriptableRenderContext.ExecuteCommandBufferAsyn]] then the queue on which the command buffer has been executed will wait.
        /// On platforms which do not support GraphicsFences, this call does nothing see:  [[SystemInfo.supportsGraphicsFence]].
        /// This function returns immediately on the CPU. Only GPU processing is effected by the fence.
        /// SA: [[Graphics.ExecuteCommandBufferAsync]] [[Graphics.CreateGraphicsFence]], [[ScriptableRenderContext.ExecuteCommandBufferAsync]], [[ScriptableRenderContext.CreateGraphicsFence]].
        /// </description>
        public void WaitOnAsyncGraphicsFence(GraphicsFence fence)
        {
            WaitOnAsyncGraphicsFence(fence, SynchronisationStage.VertexProcessing);
        }

        /// <summary>
        /// Instructs the GPU to wait until the given [[GraphicsFence]] is passed.
        /// </summary>
        /// <param name="fence">
        /// The [[GraphicsFence]] that the GPU will be instructed to wait upon before proceeding with its processing of the graphics queue.
        /// </param>
        /// <param name="stage">
        /// On some platforms there is a significant gap between the vertex processing completing and the pixel processing beginning for a given draw call. This parameter allows for a requested wait to be made before the next item's vertex or pixel processing begins. If a compute shader dispatch is the next item to be submitted then this parameter is ignored.
        /// </param>
        /// <description>
        /// If this [[CommandBuffer]] is executed using [[Graphics.ExecuteCommandBuffer]] or [[ScriptableRenderContext.ExecuteCommandBuffer]] then the processing of the graphics queue will wait.
        /// The [[GraphicsFence]] given as a parameter to this function must have been created with a [[GraphicsFenceType.AsyncQueueSynchronization]] fence type.
        /// If this [[CommandBuffer]] is executed using [[Graphics.ExecuteCommandBufferAsync]] or [[ScriptableRenderContext.ExecuteCommandBufferAsyn]] then the queue on which the command buffer has been executed will wait.
        /// On platforms which do not support GraphicsFences, this call does nothing see:  [[SystemInfo.supportsGraphicsFence]].
        /// This function returns immediately on the CPU. Only GPU processing is effected by the fence.
        /// SA: [[Graphics.ExecuteCommandBufferAsync]] [[Graphics.CreateGraphicsFence]], [[ScriptableRenderContext.ExecuteCommandBufferAsync]], [[ScriptableRenderContext.CreateGraphicsFence]].
        /// </description>
        public void WaitOnAsyncGraphicsFence(GraphicsFence fence, SynchronisationStage stage)
        {
            WaitOnAsyncGraphicsFence(fence, GraphicsFence.TranslateSynchronizationStageToFlags(stage));
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void WaitOnAsyncGraphicsFence(GraphicsFence fence, SynchronisationStageFlags stage)
        {
            if (fence.m_FenceType != GraphicsFenceType.AsyncQueueSynchronisation)
                throw new ArgumentException("Attempting to call WaitOnAsyncGPUFence on a fence that is not of GraphicsFenceType.AsyncQueueSynchronization");

            fence.Validate();

            //Don't wait on a fence that's already known to have passed
            if (fence.IsFencePending())
                WaitOnGPUFence_Internal(fence.m_Ptr, stage);
        }

        // Set a float parameter.
        /// <summary>
        /// Adds a command to set a float parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="name">
        /// Name of the variable in shader code.
        /// </param>
        /// <param name="val">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeFloatParam(ComputeShader computeShader, string name, float val)
        {
            SetComputeFloatParam(computeShader, Shader.PropertyToID(name), val);
        }

        // Set an integer parameter.
        /// <summary>
        /// Adds a command to set an integer parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="name">
        /// Name of the variable in shader code.
        /// </param>
        /// <param name="val">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeIntParam(ComputeShader computeShader, string name, int val)
        {
            SetComputeIntParam(computeShader, Shader.PropertyToID(name), val);
        }

        // Set a vector parameter.
        /// <summary>
        /// Adds a command to set a vector parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="name">
        /// Name of the variable in shader code.
        /// </param>
        /// <param name="val">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeVectorParam(ComputeShader computeShader, string name, Vector4 val)
        {
            SetComputeVectorParam(computeShader, Shader.PropertyToID(name), val);
        }

        // Set a vector array parameter.
        /// <summary>
        /// Adds a command to set a vector array parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="name">
        /// Property name.
        /// </param>
        /// <param name="values">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.µ
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeVectorArrayParam(ComputeShader computeShader, string name, Vector4[] values)
        {
            SetComputeVectorArrayParam(computeShader, Shader.PropertyToID(name), values);
        }

        // Set a matrix parameter.
        /// <summary>
        /// Adds a command to set a matrix parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="name">
        /// Name of the variable in shader code.
        /// </param>
        /// <param name="val">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeMatrixParam(ComputeShader computeShader, string name, Matrix4x4 val)
        {
            SetComputeMatrixParam(computeShader, Shader.PropertyToID(name), val);
        }

        // Set a matrix array parameter.
        /// <summary>
        /// Adds a command to set a matrix array parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="name">
        /// Name of the variable in shader code.
        /// </param>
        /// <param name="values">
        /// Value to set.
        /// </param>
        /// <description>
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeMatrixArrayParam(ComputeShader computeShader, string name, Matrix4x4[] values)
        {
            SetComputeMatrixArrayParam(computeShader, Shader.PropertyToID(name), values);
        }

        // Set multiple consecutive float parameters at once.
        /// <summary>
        /// Adds a command to set multiple consecutive float parameters on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="name">
        /// Name of the variable in shader code.
        /// </param>
        /// <param name="values">
        /// Values to set.
        /// </param>
        /// <description>
        /// This function can be used to set float vector, float array or float vector array
        /// values. For example, @@float4 myArray[4]@@ in the compute shader
        /// can be filled by passing 16 floats. See [[wiki:Compute Shaders|Compute Shaders]] for information on data layout rules.
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam,  SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeFloatParams(ComputeShader computeShader, string name, params float[] values)
        {
            Internal_SetComputeFloats(computeShader, Shader.PropertyToID(name), values);
        }

        /// <summary>
        /// Adds a command to set multiple consecutive float parameters on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="values">
        /// Values to set.
        /// </param>
        /// <description>
        /// This function can be used to set float vector, float array or float vector array
        /// values. For example, @@float4 myArray[4]@@ in the compute shader
        /// can be filled by passing 16 floats. See [[wiki:Compute Shaders|Compute Shaders]] for information on data layout rules.
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam,  SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeFloatParams(ComputeShader computeShader, int nameID, params float[] values)
        {
            Internal_SetComputeFloats(computeShader, nameID, values);
        }

        // Set multiple consecutive integer parameters at once.
        /// <summary>
        /// Adds a command to set multiple consecutive integer parameters on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="name">
        /// Name of the variable in shader code.
        /// </param>
        /// <param name="values">
        /// Values to set.
        /// </param>
        /// <description>
        /// This function can be used to set an integer vector, integer array or integer vector array
        /// values. For example, @@int4 myArray[2]@@ in the compute shader
        /// can be filled by passing 8 integers. See [[wiki:Compute Shaders|Compute Shaders]] for information on data layout rules.
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeIntParams(ComputeShader computeShader, string name, params int[] values)
        {
            Internal_SetComputeInts(computeShader, Shader.PropertyToID(name), values);
        }

        /// <summary>
        /// Adds a command to set multiple consecutive integer parameters on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="values">
        /// Values to set.
        /// </param>
        /// <description>
        /// This function can be used to set an integer vector, integer array or integer vector array
        /// values. For example, @@int4 myArray[2]@@ in the compute shader
        /// can be filled by passing 8 integers. See [[wiki:Compute Shaders|Compute Shaders]] for information on data layout rules.
        /// Constant buffers are shared between all kernels in a single compute shader asset. Therefore this function affects all kernels in the passed ComputeShader.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeIntParams(ComputeShader computeShader, int nameID, params int[] values)
        {
            Internal_SetComputeInts(computeShader, nameID, values);
        }

        // Set a texture parameter.
        /// <summary>
        /// Adds a command to set a texture parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="kernelIndex">
        /// Which kernel the texture is being set for. See [[ComputeShader.FindKernel]].
        /// </param>
        /// <param name="name">
        /// Name of the texture variable in shader code.
        /// </param>
        /// <param name="rt">
        /// Texture value or identifier to set, see [[RenderTargetIdentifier]].
        /// </param>
        /// <description>
        /// Textures and buffers are set per-kernel. Use [[ComputeShader.FindKernel]] to find kernel index by function name.
        /// Please note that the mipLevel parameter is ignored unless the shader specifies a read-write (unordered access) texture.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, string name, RenderTargetIdentifier rt)
        {
            Internal_SetComputeTextureParam(computeShader, kernelIndex, Shader.PropertyToID(name), ref rt, 0);
        }

        /// <summary>
        /// Adds a command to set a texture parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="kernelIndex">
        /// Which kernel the texture is being set for. See [[ComputeShader.FindKernel]].
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="rt">
        /// Texture value or identifier to set, see [[RenderTargetIdentifier]].
        /// </param>
        /// <description>
        /// Textures and buffers are set per-kernel. Use [[ComputeShader.FindKernel]] to find kernel index by function name.
        /// Please note that the mipLevel parameter is ignored unless the shader specifies a read-write (unordered access) texture.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, int nameID, RenderTargetIdentifier rt)
        {
            Internal_SetComputeTextureParam(computeShader, kernelIndex, nameID, ref rt, 0);
        }

        /// <summary>
        /// Adds a command to set a texture parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="kernelIndex">
        /// Which kernel the texture is being set for. See [[ComputeShader.FindKernel]].
        /// </param>
        /// <param name="name">
        /// Name of the texture variable in shader code.
        /// </param>
        /// <param name="rt">
        /// Texture value or identifier to set, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="mipLevel">
        /// Optional mipmap level of the read-write texture.
        /// </param>
        /// <description>
        /// Textures and buffers are set per-kernel. Use [[ComputeShader.FindKernel]] to find kernel index by function name.
        /// Please note that the mipLevel parameter is ignored unless the shader specifies a read-write (unordered access) texture.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, string name, RenderTargetIdentifier rt, int mipLevel)
        {
            Internal_SetComputeTextureParam(computeShader, kernelIndex, Shader.PropertyToID(name), ref rt, mipLevel);
        }

        /// <summary>
        /// Adds a command to set a texture parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="kernelIndex">
        /// Which kernel the texture is being set for. See [[ComputeShader.FindKernel]].
        /// </param>
        /// <param name="nameID">
        /// Property name ID. Use [[Shader.PropertyToID]] to get this ID.
        /// </param>
        /// <param name="rt">
        /// Texture value or identifier to set, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="mipLevel">
        /// Optional mipmap level of the read-write texture.
        /// </param>
        /// <description>
        /// Textures and buffers are set per-kernel. Use [[ComputeShader.FindKernel]] to find kernel index by function name.
        /// Please note that the mipLevel parameter is ignored unless the shader specifies a read-write (unordered access) texture.
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeBufferParam.
        /// </description>
        public void SetComputeTextureParam(ComputeShader computeShader, int kernelIndex, int nameID, RenderTargetIdentifier rt, int mipLevel)
        {
            Internal_SetComputeTextureParam(computeShader, kernelIndex, nameID, ref rt, mipLevel);
        }

        /// <summary>
        /// Adds a command to set an input or output buffer parameter on a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to set parameter for.
        /// </param>
        /// <param name="kernelIndex">
        /// Which kernel the buffer is being set for. See [[ComputeShader.FindKernel]].
        /// </param>
        /// <param name="name">
        /// Name of the buffer variable in shader code.
        /// </param>
        /// <param name="buffer">
        /// Buffer to set.
        /// </param>
        /// <description>
        /// Buffers and textures are set per-kernel. Use [[ComputeShader.FindKernel]] to find kernel index by function name.
        /// Setting a compute buffer to a kernel will leave the append/consume counter value unchanged. To set or reset the value, use [[ComputeBuffer.SetCounterValue]].
        /// SA: DispatchCompute, SetComputeFloatParam, SetComputeFloatParams, SetComputeIntParam, SetComputeIntParams, SetComputeMatrixParam, SetComputeMatrixArrayParam, SetComputeVectorParam, SetComputeVectorArrayParam, SetComputeTextureParam.
        /// </description>
        public void SetComputeBufferParam(ComputeShader computeShader, int kernelIndex, string name, ComputeBuffer buffer)
        {
            SetComputeBufferParam(computeShader, kernelIndex, Shader.PropertyToID(name), buffer);
        }

        // Execute a compute shader.
        /// <summary>
        /// Add a command to execute a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to execute.
        /// </param>
        /// <param name="kernelIndex">
        /// Kernel index to execute, see [[ComputeShader.FindKernel]].
        /// </param>
        /// <param name="threadGroupsX">
        /// Number of work groups in the X dimension.
        /// </param>
        /// <param name="threadGroupsY">
        /// Number of work groups in the Y dimension.
        /// </param>
        /// <param name="threadGroupsZ">
        /// Number of work groups in the Z dimension.
        /// </param>
        /// <description>
        /// When the command buffer executes, a compute shader kernel is dispatched, with work group size either specified directly (see [[ComputeShader.Dispatch]])
        /// or read from the GPU buffer (see [[ComputeShader.DispatchIndirect]]).
        /// </description>
        public void DispatchCompute(ComputeShader computeShader, int kernelIndex, int threadGroupsX, int threadGroupsY, int threadGroupsZ)
        {
            Internal_DispatchCompute(computeShader, kernelIndex, threadGroupsX, threadGroupsY, threadGroupsZ);
        }

        /// <summary>
        /// Add a command to execute a [[ComputeShader]].
        /// </summary>
        /// <param name="computeShader">
        /// [[ComputeShader]] to execute.
        /// </param>
        /// <param name="kernelIndex">
        /// Kernel index to execute, see [[ComputeShader.FindKernel]].
        /// </param>
        /// <param name="indirectBuffer">
        /// [[ComputeBuffer]] with dispatch arguments.
        /// </param>
        /// <param name="argsOffset">
        /// Byte offset indicating the location of the dispatch arguments in the buffer.
        /// </param>
        /// <description>
        /// When the command buffer executes, a compute shader kernel is dispatched, with work group size either specified directly (see [[ComputeShader.Dispatch]])
        /// or read from the GPU buffer (see [[ComputeShader.DispatchIndirect]]).
        /// </description>
        public void DispatchCompute(ComputeShader computeShader, int kernelIndex, ComputeBuffer indirectBuffer, uint argsOffset)
        {
            Internal_DispatchComputeIndirect(computeShader, kernelIndex, indirectBuffer, argsOffset);
        }

        /// <summary>
        /// Generate mipmap levels of a render texture.
        /// </summary>
        /// <param name="rt">
        /// The render texture requiring mipmaps generation.
        /// </param>
        /// <description>
        /// Use this function to manually re-generate mipmap levels of a render texture. The render texture has to have mipmaps ([[RenderTexture-useMipMap|useMipMap]] set to true),
        /// and automatic mip generation turned off ([[RenderTexture-autoGenerateMips|autoGenerateMips]] set to false).
        /// On some platforms (most notably, D3D9), there is no way to manually generate render texture mip levels; in these cases this function does nothing.
        /// SA: [[RenderTexture-useMipMap|useMipMap]], [[RenderTexture-autoGenerateMips|autoGenerateMips]].
        /// </description>
        public void GenerateMips(RenderTexture rt)
        {
            if (rt == null)
                throw new ArgumentNullException("rt");

            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Internal_GenerateMips(rt);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void ResolveAntiAliasedSurface(RenderTexture rt, RenderTexture target = null)
        {
            if (rt == null)
                throw new ArgumentNullException("rt");

            Internal_ResolveAntiAliasedSurface(rt, target);
        }

        /// <summary>
        /// Add a "draw mesh" command.
        /// </summary>
        /// <param name="mesh">
        /// Mesh to draw.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to render.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (default is -1, which renders all passes).
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply onto material just before this mesh will be drawn. See [[MaterialPropertyBlock]].
        /// </param>
        /// <description>
        /// SA: DrawRenderer, [[MaterialPropertyBlock]].
        /// </description>
        public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass, MaterialPropertyBlock properties)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");

            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            if (submeshIndex < 0 || submeshIndex >= mesh.subMeshCount)
            {
                submeshIndex = Mathf.Clamp(submeshIndex, 0, mesh.subMeshCount - 1);
                Debug.LogWarning(String.Format("submeshIndex out of range. Clampped to {0}.", submeshIndex));
            }
            if (material == null)
                throw new ArgumentNullException("material");
            Internal_DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, properties);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex, int shaderPass)
        {
            DrawMesh(mesh, matrix, material, submeshIndex, shaderPass, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int submeshIndex)
        {
            DrawMesh(mesh, matrix, material, submeshIndex, -1);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material)
        {
            DrawMesh(mesh, matrix, material, 0);
        }

        /// <summary>
        /// Add a "draw renderer" command.
        /// </summary>
        /// <param name="renderer">
        /// Renderer to draw.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to render.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (default is -1, which renders all passes).
        /// </param>
        /// <description>
        /// SA: DrawMesh.
        /// </description>
        public void DrawRenderer(Renderer renderer, Material material, int submeshIndex, int shaderPass)
        {
            if (renderer == null)
                throw new ArgumentNullException("renderer");

            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            if (submeshIndex < 0)
            {
                submeshIndex = Mathf.Max(submeshIndex, 0);
                Debug.LogWarning(String.Format("submeshIndex out of range. Clampped to {0}.", submeshIndex));
            }
            if (material == null)
                throw new ArgumentNullException("material");
            Internal_DrawRenderer(renderer, material, submeshIndex, shaderPass);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void DrawRenderer(Renderer renderer, Material material, int submeshIndex)
        {
            DrawRenderer(renderer, material, submeshIndex, -1);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void DrawRenderer(Renderer renderer, Material material)
        {
            DrawRenderer(renderer, material, 0);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="vertexCount">
        /// Vertex count to render.
        /// </param>
        /// <param name="instanceCount">
        /// Instance count to render.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply just before rendering. See [[MaterialPropertyBlock]].
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without any vertex or index buffers. This is mainly useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// In the vertex shader, you'd typically use the SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProceduralIndirect, [[MaterialPropertyBlock]], [[Graphics.DrawProcedural]].
        /// </description>
        public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount, MaterialPropertyBlock properties)
        {
            if (material == null)
                throw new ArgumentNullException("material");

            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Internal_DrawProcedural(matrix, material, shaderPass, topology, vertexCount, instanceCount, properties);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="vertexCount">
        /// Vertex count to render.
        /// </param>
        /// <param name="instanceCount">
        /// Instance count to render.
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without any vertex or index buffers. This is mainly useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// In the vertex shader, you'd typically use the SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProceduralIndirect, [[MaterialPropertyBlock]], [[Graphics.DrawProcedural]].
        /// </description>
        public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount, int instanceCount)
        {
            DrawProcedural(matrix, material, shaderPass, topology, vertexCount, instanceCount, null);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="vertexCount">
        /// Vertex count to render.
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without any vertex or index buffers. This is mainly useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// In the vertex shader, you'd typically use the SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProceduralIndirect, [[MaterialPropertyBlock]], [[Graphics.DrawProcedural]].
        /// </description>
        public void DrawProcedural(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int vertexCount)
        {
            DrawProcedural(matrix, material, shaderPass, topology, vertexCount, 1);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="indexBuffer">
        /// The index buffer used to submit vertices to the GPU.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="indexCount">
        /// Index count to render.
        /// </param>
        /// <param name="instanceCount">
        /// Instance count to render.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply just before rendering. See [[MaterialPropertyBlock]].
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without a vertex buffer. This is mainly useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// In the vertex shader, you'd typically use the SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProceduralIndirect, [[MaterialPropertyBlock]], [[Graphics.DrawProcedural]].
        /// </description>
        public void DrawProcedural(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount, int instanceCount, MaterialPropertyBlock properties)
        {
            if (indexBuffer == null)
                throw new ArgumentNullException("indexBuffer");
            if (material == null)
                throw new ArgumentNullException("material");
            Internal_DrawProceduralIndexed(indexBuffer, matrix, material, shaderPass, topology, indexCount, instanceCount, properties);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="indexBuffer">
        /// The index buffer used to submit vertices to the GPU.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="indexCount">
        /// Index count to render.
        /// </param>
        /// <param name="instanceCount">
        /// Instance count to render.
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without a vertex buffer. This is mainly useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// In the vertex shader, you'd typically use the SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProceduralIndirect, [[MaterialPropertyBlock]], [[Graphics.DrawProcedural]].
        /// </description>
        public void DrawProcedural(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount, int instanceCount)
        {
            DrawProcedural(indexBuffer, matrix, material, shaderPass, topology, indexCount, instanceCount, null);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="indexBuffer">
        /// The index buffer used to submit vertices to the GPU.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="indexCount">
        /// Index count to render.
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without a vertex buffer. This is mainly useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// In the vertex shader, you'd typically use the SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProceduralIndirect, [[MaterialPropertyBlock]], [[Graphics.DrawProcedural]].
        /// </description>
        public void DrawProcedural(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, int indexCount)
        {
            DrawProcedural(indexBuffer, matrix, material, shaderPass, topology, indexCount, 1);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="bufferWithArgs">
        /// Buffer with draw arguments.
        /// </param>
        /// <param name="argsOffset">
        /// Byte offset where in the buffer the draw arguments are.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply just before rendering. See [[MaterialPropertyBlock]].
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without any vertex or index buffers.
        /// The amount of geometry to draw is read from a [[ComputeBuffer]]. Typical use case is generating an arbitrary amount of data from a [[ComputeShader]] and then rendering that, without requiring a readback to the CPU.
        /// This is only useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// Buffer with arguments, /bufferWithArgs/, has to have four integer numbers at given /argsOffset/ offset:
        /// vertex count per instance, instance count, start vertex location, and start instance location.
        /// This maps to Direct3D11 DrawInstancedIndirect and equivalent functions on other graphics APIs. On OpenGL versions before 4.2 and all OpenGL ES versions that support indirect draw, the last argument is reserved and therefore must be zero.
        /// In the vertex shader, you'd typically use SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProcedural, [[MaterialPropertyBlock]], [[Graphics.DrawProceduralIndirect]], [[ComputeBuffer.CopyCount]], [[SystemInfo.supportsComputeShaders]].
        /// </description>
        public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
        {
            if (material == null)
                throw new ArgumentNullException("material");
            if (bufferWithArgs == null)
                throw new ArgumentNullException("bufferWithArgs");

            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Internal_DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="bufferWithArgs">
        /// Buffer with draw arguments.
        /// </param>
        /// <param name="argsOffset">
        /// Byte offset where in the buffer the draw arguments are.
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without any vertex or index buffers.
        /// The amount of geometry to draw is read from a [[ComputeBuffer]]. Typical use case is generating an arbitrary amount of data from a [[ComputeShader]] and then rendering that, without requiring a readback to the CPU.
        /// This is only useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// Buffer with arguments, /bufferWithArgs/, has to have four integer numbers at given /argsOffset/ offset:
        /// vertex count per instance, instance count, start vertex location, and start instance location.
        /// This maps to Direct3D11 DrawInstancedIndirect and equivalent functions on other graphics APIs. On OpenGL versions before 4.2 and all OpenGL ES versions that support indirect draw, the last argument is reserved and therefore must be zero.
        /// In the vertex shader, you'd typically use SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProcedural, [[MaterialPropertyBlock]], [[Graphics.DrawProceduralIndirect]], [[ComputeBuffer.CopyCount]], [[SystemInfo.supportsComputeShaders]].
        /// </description>
        public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset)
        {
            DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, null);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="bufferWithArgs">
        /// Buffer with draw arguments.
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without any vertex or index buffers.
        /// The amount of geometry to draw is read from a [[ComputeBuffer]]. Typical use case is generating an arbitrary amount of data from a [[ComputeShader]] and then rendering that, without requiring a readback to the CPU.
        /// This is only useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// Buffer with arguments, /bufferWithArgs/, has to have four integer numbers at given /argsOffset/ offset:
        /// vertex count per instance, instance count, start vertex location, and start instance location.
        /// This maps to Direct3D11 DrawInstancedIndirect and equivalent functions on other graphics APIs. On OpenGL versions before 4.2 and all OpenGL ES versions that support indirect draw, the last argument is reserved and therefore must be zero.
        /// In the vertex shader, you'd typically use SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProcedural, [[MaterialPropertyBlock]], [[Graphics.DrawProceduralIndirect]], [[ComputeBuffer.CopyCount]], [[SystemInfo.supportsComputeShaders]].
        /// </description>
        public void DrawProceduralIndirect(Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs)
        {
            DrawProceduralIndirect(matrix, material, shaderPass, topology, bufferWithArgs, 0);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="indexBuffer">
        /// Index buffer used to submit vertices to the GPU.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="bufferWithArgs">
        /// Buffer with draw arguments.
        /// </param>
        /// <param name="argsOffset">
        /// Byte offset where in the buffer the draw arguments are.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply just before rendering. See [[MaterialPropertyBlock]].
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without a vertex buffer.
        /// The amount of geometry to draw is read from a [[ComputeBuffer]]. Typical use case is generating an arbitrary amount of data from a [[ComputeShader]] and then rendering that, without requiring a readback to the CPU.
        /// This is only useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// Buffer with arguments, /bufferWithArgs/, has to have five integer numbers at given /argsOffset/ offset:
        /// index count per instance, instance count, start index location, base vertex location, and start instance location.
        /// This maps to Direct3D11 DrawIndexedInstancedIndirect and equivalent functions on other graphics APIs. On OpenGL versions before 4.2 and all OpenGL ES versions that support indirect draw, the last argument is reserved and therefore must be zero.
        /// In the vertex shader, you'd typically use SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProcedural, [[MaterialPropertyBlock]], [[Graphics.DrawProceduralIndirect]], [[ComputeBuffer.CopyCount]], [[SystemInfo.supportsComputeShaders]].
        /// </description>
        public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
        {
            if (indexBuffer == null)
                throw new ArgumentNullException("indexBuffer");
            if (material == null)
                throw new ArgumentNullException("material");
            if (bufferWithArgs == null)
                throw new ArgumentNullException("bufferWithArgs");
            Internal_DrawProceduralIndexedIndirect(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, properties);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="indexBuffer">
        /// Index buffer used to submit vertices to the GPU.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="bufferWithArgs">
        /// Buffer with draw arguments.
        /// </param>
        /// <param name="argsOffset">
        /// Byte offset where in the buffer the draw arguments are.
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without a vertex buffer.
        /// The amount of geometry to draw is read from a [[ComputeBuffer]]. Typical use case is generating an arbitrary amount of data from a [[ComputeShader]] and then rendering that, without requiring a readback to the CPU.
        /// This is only useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// Buffer with arguments, /bufferWithArgs/, has to have five integer numbers at given /argsOffset/ offset:
        /// index count per instance, instance count, start index location, base vertex location, and start instance location.
        /// This maps to Direct3D11 DrawIndexedInstancedIndirect and equivalent functions on other graphics APIs. On OpenGL versions before 4.2 and all OpenGL ES versions that support indirect draw, the last argument is reserved and therefore must be zero.
        /// In the vertex shader, you'd typically use SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProcedural, [[MaterialPropertyBlock]], [[Graphics.DrawProceduralIndirect]], [[ComputeBuffer.CopyCount]], [[SystemInfo.supportsComputeShaders]].
        /// </description>
        public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset)
        {
            DrawProceduralIndirect(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, argsOffset, null);
        }

        /// <summary>
        /// Add a "draw procedural geometry" command.
        /// </summary>
        /// <param name="indexBuffer">
        /// Index buffer used to submit vertices to the GPU.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix to use.
        /// </param>
        /// <param name="material">
        /// Material to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use (or -1 for all passes).
        /// </param>
        /// <param name="topology">
        /// Topology of the procedural geometry.
        /// </param>
        /// <param name="bufferWithArgs">
        /// Buffer with draw arguments.
        /// </param>
        /// <description>
        /// When the command buffer executes, this will do a draw call on the GPU, without a vertex buffer.
        /// The amount of geometry to draw is read from a [[ComputeBuffer]]. Typical use case is generating an arbitrary amount of data from a [[ComputeShader]] and then rendering that, without requiring a readback to the CPU.
        /// This is only useful on [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level hardware where shaders can read arbitrary data from [[ComputeBuffer]] buffers.
        /// Buffer with arguments, /bufferWithArgs/, has to have five integer numbers at given /argsOffset/ offset:
        /// index count per instance, instance count, start index location, base vertex location, and start instance location.
        /// This maps to Direct3D11 DrawIndexedInstancedIndirect and equivalent functions on other graphics APIs. On OpenGL versions before 4.2 and all OpenGL ES versions that support indirect draw, the last argument is reserved and therefore must be zero.
        /// In the vertex shader, you'd typically use SV_VertexID and SV_InstanceID input variables to fetch data from some buffers.
        /// SA: DrawProcedural, [[MaterialPropertyBlock]], [[Graphics.DrawProceduralIndirect]], [[ComputeBuffer.CopyCount]], [[SystemInfo.supportsComputeShaders]].
        /// </description>
        public void DrawProceduralIndirect(GraphicsBuffer indexBuffer, Matrix4x4 matrix, Material material, int shaderPass, MeshTopology topology, ComputeBuffer bufferWithArgs)
        {
            DrawProceduralIndirect(indexBuffer, matrix, material, shaderPass, topology, bufferWithArgs, 0);
        }

        /// <summary>
        /// Add a "draw mesh with instancing" command.
        /// The command will not immediately fail and throw an exception if [[Material.enableInstancing]] is false, but it will log an error and skips rendering each time the command is being executed if such a condition is detected.
        /// InvalidOperationException will be thrown if the current platform doesn't support this API (i.e. if GPU instancing is not available). See [[SystemInfo.supportsInstancing]].
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use, or -1 which renders all passes.
        /// </param>
        /// <param name="matrices">
        /// The array of object transformation matrices.
        /// </param>
        /// <param name="count">
        /// The number of instances to be drawn.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply onto material just before this mesh will be drawn. See [[MaterialPropertyBlock]].
        /// </param>
        /// <description>
        /// SA: DrawMesh, [[Graphics.DrawMeshInstanced]], [[MaterialPropertyBlock]].
        /// </description>
        public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties)
        {
            if (!SystemInfo.supportsInstancing)
                throw new InvalidOperationException("DrawMeshInstanced is not supported.");
            if (mesh == null)
                throw new ArgumentNullException("mesh");
            if (submeshIndex < 0 || submeshIndex >= mesh.subMeshCount)
                throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
            if (material == null)
                throw new ArgumentNullException("material");
            if (matrices == null)
                throw new ArgumentNullException("matrices");
            if (count < 0 || count > Mathf.Min(Graphics.kMaxDrawMeshInstanceCount, matrices.Length))
                throw new ArgumentOutOfRangeException("count", String.Format("Count must be in the range of 0 to {0}.", Mathf.Min(Graphics.kMaxDrawMeshInstanceCount, matrices.Length)));

            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            if (count > 0)
                Internal_DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, count, properties);
        }

        /// <summary>
        /// Add a "draw mesh with instancing" command.
        /// The command will not immediately fail and throw an exception if [[Material.enableInstancing]] is false, but it will log an error and skips rendering each time the command is being executed if such a condition is detected.
        /// InvalidOperationException will be thrown if the current platform doesn't support this API (i.e. if GPU instancing is not available). See [[SystemInfo.supportsInstancing]].
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use, or -1 which renders all passes.
        /// </param>
        /// <param name="matrices">
        /// The array of object transformation matrices.
        /// </param>
        /// <param name="count">
        /// The number of instances to be drawn.
        /// </param>
        /// <description>
        /// SA: DrawMesh, [[Graphics.DrawMeshInstanced]], [[MaterialPropertyBlock]].
        /// </description>
        public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices, int count)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, count, null);
        }

        /// <summary>
        /// Add a "draw mesh with instancing" command.
        /// The command will not immediately fail and throw an exception if [[Material.enableInstancing]] is false, but it will log an error and skips rendering each time the command is being executed if such a condition is detected.
        /// InvalidOperationException will be thrown if the current platform doesn't support this API (i.e. if GPU instancing is not available). See [[SystemInfo.supportsInstancing]].
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use, or -1 which renders all passes.
        /// </param>
        /// <param name="matrices">
        /// The array of object transformation matrices.
        /// </param>
        /// <description>
        /// SA: DrawMesh, [[Graphics.DrawMeshInstanced]], [[MaterialPropertyBlock]].
        /// </description>
        public void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, int shaderPass, Matrix4x4[] matrices)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, shaderPass, matrices, matrices.Length);
        }

        /// <summary>
        /// Add a "draw mesh with indirect instancing" command.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use, or -1 which renders all passes.
        /// </param>
        /// <param name="bufferWithArgs">
        /// The GPU buffer containing the arguments for how many instances of this mesh to draw.
        /// </param>
        /// <param name="argsOffset">
        /// The byte offset into the buffer, where the draw arguments start.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply onto material just before this mesh will be drawn. See [[MaterialPropertyBlock]].
        /// </param>
        /// <description>
        /// SA: DrawMesh, [[Graphics.DrawMeshInstancedIndirect]], [[MaterialPropertyBlock]].
        /// </description>
        public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
        {
            if (!SystemInfo.supportsInstancing)
                throw new InvalidOperationException("Instancing is not supported.");
            if (mesh == null)
                throw new ArgumentNullException("mesh");
            if (submeshIndex < 0 || submeshIndex >= mesh.subMeshCount)
                throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
            if (material == null)
                throw new ArgumentNullException("material");
            if (bufferWithArgs == null)
                throw new ArgumentNullException("bufferWithArgs");
            Internal_DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, properties);
        }

        /// <summary>
        /// Add a "draw mesh with indirect instancing" command.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use, or -1 which renders all passes.
        /// </param>
        /// <param name="bufferWithArgs">
        /// The GPU buffer containing the arguments for how many instances of this mesh to draw.
        /// </param>
        /// <param name="argsOffset">
        /// The byte offset into the buffer, where the draw arguments start.
        /// </param>
        /// <description>
        /// SA: DrawMesh, [[Graphics.DrawMeshInstancedIndirect]], [[MaterialPropertyBlock]].
        /// </description>
        public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs, int argsOffset)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, argsOffset, null);
        }

        /// <summary>
        /// Add a "draw mesh with indirect instancing" command.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="shaderPass">
        /// Which pass of the shader to use, or -1 which renders all passes.
        /// </param>
        /// <param name="bufferWithArgs">
        /// The GPU buffer containing the arguments for how many instances of this mesh to draw.
        /// </param>
        /// <description>
        /// SA: DrawMesh, [[Graphics.DrawMeshInstancedIndirect]], [[MaterialPropertyBlock]].
        /// </description>
        public void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, int shaderPass, ComputeBuffer bufferWithArgs)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, shaderPass, bufferWithArgs, 0, null);
        }

        /// <summary>
        /// Adds a command onto the commandbuffer to draw the VR Device's occlusion mesh to the current render target.
        /// </summary>
        /// <param name="normalizedCamViewport">
        /// The viewport of the camera currently being rendered.
        /// </param>
        /// <description>
        /// Commands in the rendering command buffer are lower-level graphics operations that can be sequenced in scripting. This command in particular is used to render an occlusion mesh provided by the active VR Device.
        /// Call this method before other rendering methods to prevent rendering of objects that are outside the VR device's visible regions.
        /// SA: [[XRSettings.useOcclusionMesh]] and [[XRSettings.occlusionMaskScale]]
        /// </description>
        public void DrawOcclusionMesh(RectInt normalizedCamViewport)
        {
            Internal_DrawOcclusionMesh(normalizedCamViewport);
        }

        /// <summary>
        /// Set random write target for [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level pixel shaders.
        /// </summary>
        /// <param name="index">
        /// Index of the random write target in the shader.
        /// </param>
        /// <param name="rt">
        /// RenderTargetIdentifier to set as write target.
        /// </param>
        /// <description>
        /// This is the CommandBuffer equivalent of [[Graphics.SetRandomWriteTarget]]. The same limitations nad exceptions applies to this call.
        /// </description>
        public void SetRandomWriteTarget(int index, RenderTargetIdentifier rt)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            SetRandomWriteTarget_Texture(index, ref rt);
        }

        /// <summary>
        /// Set random write target for [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level pixel shaders.
        /// </summary>
        /// <param name="index">
        /// Index of the random write target in the shader.
        /// </param>
        /// <param name="buffer">
        /// ComputeBuffer to set as write targe.
        /// </param>
        /// <param name="preserveCounterValue">
        /// Whether to leave the append/consume counter value unchanged.
        /// </param>
        /// <description>
        /// This is the CommandBuffer equivalent of [[Graphics.SetRandomWriteTarget]]. The same limitations nad exceptions applies to this call.
        /// </description>
        public void SetRandomWriteTarget(int index, ComputeBuffer buffer, bool preserveCounterValue)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            SetRandomWriteTarget_Buffer(index, buffer, preserveCounterValue);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetRandomWriteTarget(int index, ComputeBuffer buffer)
        {
            SetRandomWriteTarget(index, buffer, false);
        }

        /// <summary>
        /// Adds a command to copy a texture into another texture.
        /// </summary>
        /// <param name="src">
        /// Source texture or identifier, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="dst">
        /// Destination texture or identifier, see [[RenderTargetIdentifier]].
        /// </param>
        /// <description>
        /// This function efficiently copies pixel data from one Texture to another.
        /// Source and destination elements can be Textures, cube maps, texture array layers or 3D texture depth slices. Mipmap levels and source and destination sub-regions can be specified.
        /// Source and destination pixel dimensions must be the same, as copying does not do any scaling.
        /// Texture formats should be compatible (for example, [[TextureFormat.ARGB32]] and [[RenderTextureFormat.ARGB32]] are compatible).
        /// Exact rules for which formats are compatible vary between graphics APIs. Formats that are exactly the same can always be copied.
        /// On some platforms (for instance, D3D11) you can also copy between formats that are of the same bit width.
        /// Compressed texture formats add some restrictions to the CopyTexture with a region variant. For example, PVRTC formats
        /// are not supported since they are not block-based (for these formats you can only copy whole texture or whole mipmap level).
        /// For block-based formats (for instance, DXT, ETC), the region size and coordinates must be a multiple of compression block size (4 pixels for DXT).
        /// If both source and destination textures are marked as "readable" (that is, a copy of the data exists in system memory
        /// for reading/writing on the CPU), the data is copied in system memory as well as on the GPU.
        /// Some platforms might not have functionality of all sorts of texture copying (for instance, copy from a render texture
        /// into a regular texture). See [[Rendering.CopyTextureSupport]], and use [[SystemInfo.copyTextureSupport]] to check.
        /// SA: [[Graphics.CopyTexture]], [[Rendering.CopyTextureSupport]].
        /// </description>
        public void CopyTexture(RenderTargetIdentifier src, RenderTargetIdentifier dst)
        {
            CopyTexture_Internal(ref src, -1, -1, -1, -1, -1, -1,
                ref dst, -1, -1, -1, -1, 0x1);
        }

        /// <summary>
        /// Adds a command to copy a texture into another texture.
        /// </summary>
        /// <param name="src">
        /// Source texture or identifier, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="srcElement">
        /// Source texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="dst">
        /// Destination texture or identifier, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="dstElement">
        /// Destination texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <description>
        /// This function efficiently copies pixel data from one Texture to another.
        /// Source and destination elements can be Textures, cube maps, texture array layers or 3D texture depth slices. Mipmap levels and source and destination sub-regions can be specified.
        /// Source and destination pixel dimensions must be the same, as copying does not do any scaling.
        /// Texture formats should be compatible (for example, [[TextureFormat.ARGB32]] and [[RenderTextureFormat.ARGB32]] are compatible).
        /// Exact rules for which formats are compatible vary between graphics APIs. Formats that are exactly the same can always be copied.
        /// On some platforms (for instance, D3D11) you can also copy between formats that are of the same bit width.
        /// Compressed texture formats add some restrictions to the CopyTexture with a region variant. For example, PVRTC formats
        /// are not supported since they are not block-based (for these formats you can only copy whole texture or whole mipmap level).
        /// For block-based formats (for instance, DXT, ETC), the region size and coordinates must be a multiple of compression block size (4 pixels for DXT).
        /// If both source and destination textures are marked as "readable" (that is, a copy of the data exists in system memory
        /// for reading/writing on the CPU), the data is copied in system memory as well as on the GPU.
        /// Some platforms might not have functionality of all sorts of texture copying (for instance, copy from a render texture
        /// into a regular texture). See [[Rendering.CopyTextureSupport]], and use [[SystemInfo.copyTextureSupport]] to check.
        /// SA: [[Graphics.CopyTexture]], [[Rendering.CopyTextureSupport]].
        /// </description>
        public void CopyTexture(RenderTargetIdentifier src, int srcElement,
            RenderTargetIdentifier dst, int dstElement)
        {
            CopyTexture_Internal(ref src, srcElement, -1, -1, -1, -1, -1,
                ref dst, dstElement, -1, -1, -1, 0x2);
        }

        /// <summary>
        /// Adds a command to copy a texture into another texture.
        /// </summary>
        /// <param name="src">
        /// Source texture or identifier, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="srcElement">
        /// Source texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="srcMip">
        /// Source texture mipmap level.
        /// </param>
        /// <param name="dst">
        /// Destination texture or identifier, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="dstElement">
        /// Destination texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="dstMip">
        /// Destination texture mipmap level.
        /// </param>
        /// <description>
        /// This function efficiently copies pixel data from one Texture to another.
        /// Source and destination elements can be Textures, cube maps, texture array layers or 3D texture depth slices. Mipmap levels and source and destination sub-regions can be specified.
        /// Source and destination pixel dimensions must be the same, as copying does not do any scaling.
        /// Texture formats should be compatible (for example, [[TextureFormat.ARGB32]] and [[RenderTextureFormat.ARGB32]] are compatible).
        /// Exact rules for which formats are compatible vary between graphics APIs. Formats that are exactly the same can always be copied.
        /// On some platforms (for instance, D3D11) you can also copy between formats that are of the same bit width.
        /// Compressed texture formats add some restrictions to the CopyTexture with a region variant. For example, PVRTC formats
        /// are not supported since they are not block-based (for these formats you can only copy whole texture or whole mipmap level).
        /// For block-based formats (for instance, DXT, ETC), the region size and coordinates must be a multiple of compression block size (4 pixels for DXT).
        /// If both source and destination textures are marked as "readable" (that is, a copy of the data exists in system memory
        /// for reading/writing on the CPU), the data is copied in system memory as well as on the GPU.
        /// Some platforms might not have functionality of all sorts of texture copying (for instance, copy from a render texture
        /// into a regular texture). See [[Rendering.CopyTextureSupport]], and use [[SystemInfo.copyTextureSupport]] to check.
        /// SA: [[Graphics.CopyTexture]], [[Rendering.CopyTextureSupport]].
        /// </description>
        public void CopyTexture(RenderTargetIdentifier src, int srcElement, int srcMip,
            RenderTargetIdentifier dst, int dstElement, int dstMip)
        {
            CopyTexture_Internal(ref src, srcElement, srcMip, -1, -1, -1, -1,
                ref dst, dstElement, dstMip, -1, -1, 0x3);
        }

        /// <summary>
        /// Adds a command to copy a texture into another texture.
        /// </summary>
        /// <param name="src">
        /// Source texture or identifier, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="srcElement">
        /// Source texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="srcMip">
        /// Source texture mipmap level.
        /// </param>
        /// <param name="srcX">
        /// X coordinate of source texture region to copy (left side is zero).
        /// </param>
        /// <param name="srcY">
        /// Y coordinate of source texture region to copy (bottom is zero).
        /// </param>
        /// <param name="srcWidth">
        /// Width of source texture region to copy.
        /// </param>
        /// <param name="srcHeight">
        /// Height of source texture region to copy.
        /// </param>
        /// <param name="dst">
        /// Destination texture or identifier, see [[RenderTargetIdentifier]].
        /// </param>
        /// <param name="dstElement">
        /// Destination texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="dstMip">
        /// Destination texture mipmap level.
        /// </param>
        /// <param name="dstX">
        /// X coordinate of where to copy region in destination texture (left side is zero).
        /// </param>
        /// <param name="dstY">
        /// Y coordinate of where to copy region in destination texture (bottom is zero).
        /// </param>
        /// <description>
        /// This function efficiently copies pixel data from one Texture to another.
        /// Source and destination elements can be Textures, cube maps, texture array layers or 3D texture depth slices. Mipmap levels and source and destination sub-regions can be specified.
        /// Source and destination pixel dimensions must be the same, as copying does not do any scaling.
        /// Texture formats should be compatible (for example, [[TextureFormat.ARGB32]] and [[RenderTextureFormat.ARGB32]] are compatible).
        /// Exact rules for which formats are compatible vary between graphics APIs. Formats that are exactly the same can always be copied.
        /// On some platforms (for instance, D3D11) you can also copy between formats that are of the same bit width.
        /// Compressed texture formats add some restrictions to the CopyTexture with a region variant. For example, PVRTC formats
        /// are not supported since they are not block-based (for these formats you can only copy whole texture or whole mipmap level).
        /// For block-based formats (for instance, DXT, ETC), the region size and coordinates must be a multiple of compression block size (4 pixels for DXT).
        /// If both source and destination textures are marked as "readable" (that is, a copy of the data exists in system memory
        /// for reading/writing on the CPU), the data is copied in system memory as well as on the GPU.
        /// Some platforms might not have functionality of all sorts of texture copying (for instance, copy from a render texture
        /// into a regular texture). See [[Rendering.CopyTextureSupport]], and use [[SystemInfo.copyTextureSupport]] to check.
        /// SA: [[Graphics.CopyTexture]], [[Rendering.CopyTextureSupport]].
        /// </description>
        public void CopyTexture(RenderTargetIdentifier src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight,
            RenderTargetIdentifier dst, int dstElement, int dstMip, int dstX, int dstY)
        {
            CopyTexture_Internal(ref src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight,
                ref dst, dstElement, dstMip, dstX, dstY, 0x4);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(Texture source, RenderTargetIdentifier dest)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Texture(source, ref dest, null, -1, new Vector2(1.0f, 1.0f), new Vector2(0.0f, 0.0f), Texture2DArray.allSlices, 0);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="scale">
        /// Scale applied to the source texture coordinate.
        /// </param>
        /// <param name="offset">
        /// Offset applied to the source texture coordinate.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(Texture source, RenderTargetIdentifier dest, Vector2 scale, Vector2 offset)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Texture(source, ref dest, null, -1, scale, offset, Texture2DArray.allSlices, 0);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="mat">
        /// Material to use.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(Texture source, RenderTargetIdentifier dest, Material mat)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Texture(source, ref dest, mat, -1, new Vector2(1.0f, 1.0f), new Vector2(0.0f, 0.0f), Texture2DArray.allSlices, 0);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="mat">
        /// Material to use.
        /// </param>
        /// <param name="pass">
        /// Shader pass to use (default is -1, meaning "all passes").
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(Texture source, RenderTargetIdentifier dest, Material mat, int pass)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Texture(source, ref dest, mat, pass, new Vector2(1.0f, 1.0f), new Vector2(0.0f, 0.0f), Texture2DArray.allSlices, 0);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Identifier(ref source, ref dest, null, -1, new Vector2(1.0f, 1.0f), new Vector2(0.0f, 0.0f), Texture2DArray.allSlices, 0);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="scale">
        /// Scale applied to the source texture coordinate.
        /// </param>
        /// <param name="offset">
        /// Offset applied to the source texture coordinate.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Vector2 scale, Vector2 offset)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Identifier(ref source, ref dest, null, -1, scale, offset, Texture2DArray.allSlices, 0);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="mat">
        /// Material to use.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Identifier(ref source, ref dest, mat, -1, new Vector2(1.0f, 1.0f), new Vector2(0.0f, 0.0f), Texture2DArray.allSlices, 0);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="mat">
        /// Material to use.
        /// </param>
        /// <param name="pass">
        /// Shader pass to use (default is -1, meaning "all passes").
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int pass)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Identifier(ref source, ref dest, mat, pass, new Vector2(1.0f, 1.0f), new Vector2(0.0f, 0.0f), Texture2DArray.allSlices, 0);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="sourceDepthSlice">
        /// The texture array source slice to perform the blit from.
        /// </param>
        /// <param name="destDepthSlice">
        /// The texture array destination slice to perform the blit to.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, int sourceDepthSlice, int destDepthSlice)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Identifier(ref source, ref dest, null, -1, new Vector2(1.0f, 1.0f), new Vector2(0.0f, 0.0f), sourceDepthSlice, destDepthSlice);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="scale">
        /// Scale applied to the source texture coordinate.
        /// </param>
        /// <param name="offset">
        /// Offset applied to the source texture coordinate.
        /// </param>
        /// <param name="sourceDepthSlice">
        /// The texture array source slice to perform the blit from.
        /// </param>
        /// <param name="destDepthSlice">
        /// The texture array destination slice to perform the blit to.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Identifier(ref source, ref dest, null, -1, scale, offset, sourceDepthSlice, destDepthSlice);
        }

        /// <summary>
        /// Add a "blit into a render texture" command.
        /// </summary>
        /// <param name="source">
        /// Source texture or render target to blit from.
        /// </param>
        /// <param name="dest">
        /// Destination to blit into.
        /// </param>
        /// <param name="mat">
        /// Material to use.
        /// </param>
        /// <param name="pass">
        /// Shader pass to use (default is -1, meaning "all passes").
        /// </param>
        /// <param name="destDepthSlice">
        /// The texture array destination slice to perform the blit to.
        /// </param>
        /// <description>
        /// This is similar to [[Graphics.Blit]] - it is mostly for copying from one (render)texture into another, potentially using a custom shader.
        /// Source texture or render target will be passed to the material as "_MainTex" property.
        /// Render texture to use can be indicated in several ways: a RenderTexture object, a temporary render texture created with GetTemporaryRT, or one of built-in temporary textures ([[Rendering.BuiltinRenderTextureType]]). All that is expressed by a [[Rendering.RenderTargetIdentifier]] struct, which has implicit conversion operators to save on typing.
        /// Note that Blit changes the currently active render target. After Blit executes, /dest/ becomes the active render target.
        /// Often the previous content of the Blit /dest/ does not need to be preserved. In this case, it is recommended to activate the /dest/ render target explicitly with the appropriate load and store actions using SetRenderTarget. The Blit /dest/ should then be set to [[Rendering.BuiltinRenderTextureType.CurrentActive]].
        /// SA: GetTemporaryRT, [[Rendering.RenderTargetIdentifier]].
        /// </description>
        public void Blit(RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int pass, int destDepthSlice)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            Blit_Identifier(ref source, ref dest, mat, pass, new Vector2(1.0f, 1.0f), new Vector2(0.0f, 0.0f), Texture2DArray.allSlices, destDepthSlice);
        }

        /// <summary>
        /// Add a "set global shader float property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader float property will be set at this point. The effect is as if [[Shader.SetGlobalFloat]] was called.
        /// </description>
        public void SetGlobalFloat(string name, float value)
        {
            SetGlobalFloat(Shader.PropertyToID(name), value);
        }

        /// <summary>
        /// Sets the given global integer property for all shaders.
        /// </summary>
        /// <description>
        /// Internally, float and integer shader properties are treated exactly the same, so this function is just an alias to SetGlobalFloat.
        /// </description>
        public void SetGlobalInt(string name, int value)
        {
            SetGlobalInt(Shader.PropertyToID(name), value);
        }

        /// <summary>
        /// Add a "set global shader vector property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader vector property will be set at this point. The effect is as if [[Shader.SetGlobalVector]] was called.
        /// </description>
        public void SetGlobalVector(string name, Vector4 value)
        {
            SetGlobalVector(Shader.PropertyToID(name), value);
        }

        /// <summary>
        /// Add a "set global shader color property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader color property will be set at this point. The effect is as if [[Shader.SetGlobalColor]] was called.
        /// </description>
        public void SetGlobalColor(string name, Color value)
        {
            SetGlobalColor(Shader.PropertyToID(name), value);
        }

        /// <summary>
        /// Add a "set global shader matrix property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader matrix property will be set at this point. The effect is as if [[Shader.SetGlobalMatrix]] was called.
        /// </description>
        public void SetGlobalMatrix(string name, Matrix4x4 value)
        {
            SetGlobalMatrix(Shader.PropertyToID(name), value);
        }

        // List<T> version
        /// <summary>
        /// Add a "set global shader float array property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader float array property will be set at this point. The effect is as if [[Shader.SetGlobalFloatArray]] was called.
        /// </description>
        public void SetGlobalFloatArray(string propertyName, List<float> values)
        {
            SetGlobalFloatArray(Shader.PropertyToID(propertyName), values);
        }

        /// <summary>
        /// Add a "set global shader float array property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader float array property will be set at this point. The effect is as if [[Shader.SetGlobalFloatArray]] was called.
        /// </description>
        public void SetGlobalFloatArray(int nameID, List<float> values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (values.Count == 0) throw new ArgumentException("Zero-sized array is not allowed.");
            SetGlobalFloatArrayListImpl(nameID, values);
        }

        // T[] version
        /// <summary>
        /// Add a "set global shader float array property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader float array property will be set at this point. The effect is as if [[Shader.SetGlobalFloatArray]] was called.
        /// </description>
        public void SetGlobalFloatArray(string propertyName, float[] values)
        {
            SetGlobalFloatArray(Shader.PropertyToID(propertyName), values);
        }

        // List<T> version
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetGlobalVectorArray(string propertyName, List<Vector4> values)
        {
            SetGlobalVectorArray(Shader.PropertyToID(propertyName), values);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetGlobalVectorArray(int nameID, List<Vector4> values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (values.Count == 0) throw new ArgumentException("Zero-sized array is not allowed.");
            SetGlobalVectorArrayListImpl(nameID, values);
        }

        // T[] version
        /// <summary>
        /// Add a "set global shader vector array property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader vector array property will be set at this point. The effect is as if [[Shader.SetGlobalVectorArray]] was called.
        /// </description>
        public void SetGlobalVectorArray(string propertyName, Vector4[] values)
        {
            SetGlobalVectorArray(Shader.PropertyToID(propertyName), values);
        }

        // List<T> version
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetGlobalMatrixArray(string propertyName, List<Matrix4x4> values)
        {
            SetGlobalMatrixArray(Shader.PropertyToID(propertyName), values);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetGlobalMatrixArray(int nameID, List<Matrix4x4> values)
        {
            if (values == null) throw new ArgumentNullException("values");
            if (values.Count == 0) throw new ArgumentException("Zero-sized array is not allowed.");
            SetGlobalMatrixArrayListImpl(nameID, values);
        }

        // T[] version
        /// <summary>
        /// Add a "set global shader matrix array property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader matrix array property will be set at this point. The effect is as if [[Shader.SetGlobalMatrixArray]] was called.
        /// </description>
        public void SetGlobalMatrixArray(string propertyName, Matrix4x4[] values)
        {
            SetGlobalMatrixArray(Shader.PropertyToID(propertyName), values);
        }

        /// <summary>
        /// Add a "set global shader texture property" command, referencing a RenderTexture.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader texture property will be set at this point. The effect is as if [[Shader.SetGlobalTexture]] was called, but with a RenderTexture instead of a Texture.
        ///         SA [[RenderTargetIdentifier]].
        /// </description>
        public void SetGlobalTexture(string name, RenderTargetIdentifier value)
        {
            SetGlobalTexture(Shader.PropertyToID(name), value);
        }

        /// <summary>
        /// Add a "set global shader texture property" command, referencing a RenderTexture.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader texture property will be set at this point. The effect is as if [[Shader.SetGlobalTexture]] was called, but with a RenderTexture instead of a Texture.
        ///         SA [[RenderTargetIdentifier]].
        /// </description>
        public void SetGlobalTexture(int nameID, RenderTargetIdentifier value)
        {
            SetGlobalTexture_Impl(nameID, ref value);
        }

        /// <summary>
        /// Add a "set global shader buffer property" command.
        /// </summary>
        /// <description>
        /// When the command buffer will be executed, a global shader buffer property will be set at this point. The effect is as if [[Shader.SetGlobalBuffer]] was called.
        /// </description>
        public void SetGlobalBuffer(string name, ComputeBuffer value)
        {
            SetGlobalBuffer(Shader.PropertyToID(name), value);
        }

        /// <summary>
        /// Add a "set shadow sampling mode" command.
        /// </summary>
        /// <param name="shadowmap">
        /// Shadowmap render target to change the sampling mode on.
        /// </param>
        /// <param name="mode">
        /// New sampling mode.
        /// </param>
        /// <description>
        /// Shadowmap render textures are normally set up to be sampled with a comparison filter - the sampler gets a shadow-space depth value of the screen pixel and returns 0 or 1, depending on if the depth value in the shadowmap is smaller or larger. That's the default [[ShadowSamplingMode.CompareDepths]] mode and is used for rendering shadows.
        /// If you'd like to access the shadowmap values as in a regular texture, set the sampling mode to [[ShadowSamplingMode.RawDepth]].
        /// Shadowmap's sampling mode will be reverted to default after the the last command in the current CommandBuffer.
        /// Check [[SystemInfo.supportsRawShadowDepthSampling]] to verify if current runtime platform supports sampling shadows this way. Notably, DirectX9 does not.
        /// </description>
        public void SetShadowSamplingMode(UnityEngine.Rendering.RenderTargetIdentifier shadowmap, ShadowSamplingMode mode)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            SetShadowSamplingMode_Impl(ref shadowmap, mode);
        }

        /// <summary>
        /// Add a command to set single-pass stereo mode for the camera.
        /// </summary>
        /// <param name="mode">
        /// Single-pass stereo mode for the camera.
        /// </param>
        /// <description>
        /// This property is only used when Virtual Reality is enabled. The values passed to mode are found in the [[SinglePassStereoMode]] enum. This can be paired with set shader keyword (i.e. UNITY_SINGLE_PASS_STEREO) to temporarily disable stereo rendering for fullscreen effects. Note: changing single-pass stereo mode can cause rendering artifacts when stereo is enabled.
        /// </description>
        public void SetSinglePassStereo(SinglePassStereoMode mode)
        {
            Internal_SetSinglePassStereo(mode);
        }

        /// <summary>
        /// Send a user-defined event to a native code plugin.
        /// </summary>
        /// <param name="callback">
        /// Native code callback to queue for Unity's renderer to invoke.
        /// </param>
        /// <param name="eventID">
        /// User defined id to send to the callback.
        /// </param>
        /// <description>
        /// See [[GL.IssuePluginEvent]] for more details and an example.
        /// </description>
        public void IssuePluginEvent(IntPtr callback, int eventID)
        {
            if (callback == IntPtr.Zero)
                throw new ArgumentException("Null callback specified.");

            IssuePluginEventInternal(callback, eventID);
        }

        /// <summary>
        /// Send a user-defined event to a native code plugin with custom data.
        /// </summary>
        /// <param name="callback">
        /// Native code callback to queue for Unity's renderer to invoke.
        /// </param>
        /// <param name="eventID">
        /// Built in or user defined id to send to the callback.
        /// </param>
        /// <param name="data">
        /// Custom data to pass to the native plugin callback.
        /// </param>
        public void IssuePluginEventAndData(IntPtr callback, int eventID, IntPtr data)
        {
            if (callback == IntPtr.Zero)
                throw new ArgumentException("Null callback specified.");

            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            IssuePluginEventAndDataInternal(callback, eventID, data);
        }

        /// <summary>
        /// Send a user-defined blit event to a native code plugin.
        /// </summary>
        /// <param name="callback">
        /// Native code callback to queue for Unity's renderer to invoke.
        /// </param>
        /// <param name="command">
        /// User defined command id to send to the callback.
        /// </param>
        /// <param name="source">
        /// Source render target.
        /// </param>
        /// <param name="dest">
        /// Destination render target.
        /// </param>
        /// <param name="commandParam">
        /// User data command parameters.
        /// </param>
        /// <param name="commandFlags">
        /// User data command flags.
        /// </param>
        public void IssuePluginCustomBlit(IntPtr callback, uint command, UnityEngine.Rendering.RenderTargetIdentifier source, UnityEngine.Rendering.RenderTargetIdentifier dest, uint commandParam, uint commandFlags)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            IssuePluginCustomBlitInternal(callback, command, ref source, ref dest, commandParam, commandFlags);
        }

        /// <summary>
        /// Deprecated. Use CommandBuffer.IssuePluginCustomTextureUpdateV2 instead.
        /// </summary>
        /// <param name="callback">
        /// Native code callback to queue for Unity's renderer to invoke.
        /// </param>
        /// <param name="targetTexture">
        /// Texture resource to be updated.
        /// </param>
        /// <param name="userData">
        /// User data to send to the native plugin.
        /// </param>
        /// <description>
        /// Send a texture update event to a native code plugin.
        /// </description>
        [Obsolete("Use IssuePluginCustomTextureUpdateV2 to register TextureUpdate callbacks instead. Callbacks will be passed event IDs kUnityRenderingExtEventUpdateTextureBeginV2 or kUnityRenderingExtEventUpdateTextureEndV2, and data parameter of type UnityRenderingExtTextureUpdateParamsV2.", false)]
        public void IssuePluginCustomTextureUpdate(IntPtr callback, Texture targetTexture, uint userData)
        {
            IssuePluginCustomTextureUpdateInternal(callback, targetTexture, userData, false);
        }

        /// <summary>
        /// Deprecated. Use CommandBuffer.IssuePluginCustomTextureUpdateV2 instead.
        /// </summary>
        /// <param name="callback">
        /// Native code callback to queue for Unity's renderer to invoke.
        /// </param>
        /// <param name="targetTexture">
        /// Texture resource to be updated.
        /// </param>
        /// <param name="userData">
        /// User data to send to the native plugin.
        /// </param>
        /// <description>
        /// Send a texture update event to a native code plugin.
        /// </description>
        [Obsolete("Use IssuePluginCustomTextureUpdateV2 to register TextureUpdate callbacks instead. Callbacks will be passed event IDs kUnityRenderingExtEventUpdateTextureBeginV2 or kUnityRenderingExtEventUpdateTextureEndV2, and data parameter of type UnityRenderingExtTextureUpdateParamsV2.", false)]
        public void IssuePluginCustomTextureUpdateV1(IntPtr callback, Texture targetTexture, uint userData)
        {
            IssuePluginCustomTextureUpdateInternal(callback, targetTexture, userData, false);
        }

        /// <summary>
        /// Send a texture update event to a native code plugin.
        /// </summary>
        /// <param name="callback">
        /// Native code callback to queue for Unity's renderer to invoke.
        /// </param>
        /// <param name="targetTexture">
        /// Texture resource to be updated.
        /// </param>
        /// <param name="userData">
        /// User data to send to the native plugin.
        /// </param>
        public void IssuePluginCustomTextureUpdateV2(IntPtr callback, Texture targetTexture, uint userData)
        {
            ValidateAgainstExecutionFlags(CommandBufferExecutionFlags.None, CommandBufferExecutionFlags.AsyncCompute);

            IssuePluginCustomTextureUpdateInternal(callback, targetTexture, userData, true);
        }
    }
}
