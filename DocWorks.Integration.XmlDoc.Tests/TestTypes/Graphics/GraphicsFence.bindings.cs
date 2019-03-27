using System;
using System.Collections.Generic;
using ShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode;
using UnityEngine.Scripting;
using UnityEngine.Bindings;
using uei = UnityEngine.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace UnityEngine.Rendering
{
    /// <summary>
    /// Describes the various stages of GPU processing against which the GraphicsFence can be set and waited against.
    /// </summary>
    /// <description>
    /// The enum values can be combined; for example, a GraphicsFence created with SynchronisationStageFlags.VertexProcessing | SynchronisationStageFlags.ComputeProcessing flags would complete once all previous draw calls have completed their vertex shaders and all previous compute shader dispatches have completed.
    /// </description>
    public enum SynchronisationStageFlags
    {
        /// <summary>
        /// All aspects of vertex processing in the GPU.
        /// </summary>
        VertexProcessing = 1,
        /// <summary>
        /// All aspects of pixel processing in the GPU.
        /// </summary>
        PixelProcessing = 2,
        /// <summary>
        /// All compute shader dispatch operations.
        /// </summary>
        ComputeProcessing = 4,
        /// <summary>
        /// All previous GPU operations (vertex, pixel and compute).
        /// </summary>
        AllGPUOperations = VertexProcessing | PixelProcessing | ComputeProcessing,
    }

    // The type of GraphicsFence to create. CPUSynchronization one can only be used to check whether the GPU has passed the fence.
    // AsyncQueueSynchronisation can be used to synchronise between the main thread and the async queue
    /// <summary>
    /// The type of the GraphicsFence. Currently the only supported fence type is AsyncQueueSynchronization.
    /// </summary>
    public enum GraphicsFenceType
    {
        /// <summary>
        /// The GraphicsFence can be used to synchronise between different GPU queues, as well as to synchronise between GPU and the CPU.
        /// </summary>
        AsyncQueueSynchronisation = 0,
        /// <summary>
        /// The GraphicsFence can only be used to synchronize between the GPU and the CPU.
        /// </summary>
        CPUSynchronisation = 1,
    }


    /// <summary>
    /// Used to manage synchronisation between tasks on async compute queues and the graphics queue.
    /// </summary>
    /// <description>
    /// Not all platforms support Graphics fences. See [[SystemInfo.supportsGraphicsFence]].
    /// A [[GraphicsFence]] represents a point during GPU processing after a specific compute shader dispatch or draw call has completed. It can be used to achieve synchronisation between tasks running on the async compute queues or the graphics queue by having one or more queues wait until a given fence has passed. This is an important consideration when working with async compute because the various tasks running at the same time on the graphics queue and the async compute queues are key to improving GPU performance.
    /// GPUFences do not need to be used to synchronise a GPU task writing to a resource that will be read as an input by another. These resource dependencies are automatically handled by Unity.
    /// GPUFences should be created via [[Graphics.CreateGraphicsFence]] or [[CommandBuffer.CreateGraphicsFence]]. Attempting to use a GraphicsFence that has not been created via one of these functions will result in an exception.
    /// It is possible to create circular dependencies using GraphicsFences that, if executed, would deadlock the GPU. Unity will detect such circular dependencies in the Editor and raise exceptions if any exist after calls to [[Graphics.CreateGraphicsFence]], [[Graphics.WaitOnGraphicsFence]], [[Graphics.ExecuteCommandBuffer]], [[Graphics.ExecuteCommandBufferAsync]], [[ScriptableRenderContext.ExecuteCommandBuffer]], [[ScriptableRenderContext.ExecuteCommandBufferAsync]].
    /// SA: [[SystemInfo.supportsGraphicsFence]], [[Graphics.CreateGraphicsFence]], [[Graphics.WaitOnGraphicsFence]], [[CommandBuffer.CreateGraphicsFence]], [[CommandBuffer.WaitOnAsyncGraphicsFence]], [[Graphics.ExecuteCommandBuffer]], [[Graphics.ExecuteCommandBufferAsync]], [[ScriptableRenderContext.ExecuteCommandBuffer]], [[ScriptableRenderContext.ExecuteCommandBufferAsync]].
    /// </description>
    [NativeHeader("Runtime/Graphics/GPUFence.h")]
    [UsedByNativeCode]
    public struct GraphicsFence
    {
        internal IntPtr m_Ptr;
        internal int m_Version;
        internal GraphicsFenceType m_FenceType;

        internal static SynchronisationStageFlags TranslateSynchronizationStageToFlags(SynchronisationStage s)
        {
            return s == SynchronisationStage.VertexProcessing ? SynchronisationStageFlags.VertexProcessing : SynchronisationStageFlags.PixelProcessing;
        }

        /// <summary>
        /// Determines whether the [[GraphicsFence]] has passed.
        /// Allows the CPU to determine whether the GPU has passed the point in its processing represented by the [[GraphicsFence]].
        /// </summary>
        public bool passed
        {
            get
            {
                Validate();

                if (!SystemInfo.supportsGraphicsFence || (m_FenceType == GraphicsFenceType.AsyncQueueSynchronisation && !SystemInfo.supportsAsyncCompute))
                    throw new System.NotSupportedException("Cannot determine if this GraphicsFence has passed as this platform has not implemented GraphicsFences.");

                if (!IsFencePending())
                    return true;

                return HasFencePassed_Internal(m_Ptr);
            }
        }

        [FreeFunction("GPUFenceInternals::HasFencePassed_Internal")]
        extern private static bool HasFencePassed_Internal(IntPtr fencePtr);

        internal void InitPostAllocation()
        {
            if (m_Ptr == IntPtr.Zero)
            {
                if (SystemInfo.supportsGraphicsFence)
                {
                    throw new System.NullReferenceException("The internal fence ptr is null, this should not be possible for fences that have been correctly constructed using Graphics.CreateGraphicsFence() or CommandBuffer.CreateGraphicsFence()");
                }
                m_Version = GetPlatformNotSupportedVersion();
                return;
            }

            m_Version = GetVersionNumber(m_Ptr);
        }

        internal bool IsFencePending()
        {
            if (m_Ptr == IntPtr.Zero)
                return false;

            return m_Version == GetVersionNumber(m_Ptr);
        }

        internal void Validate()
        {
            if (m_Version == 0 || (SystemInfo.supportsGraphicsFence && m_Version == GetPlatformNotSupportedVersion()))
                throw new System.InvalidOperationException("This GraphicsFence object has not been correctly constructed see Graphics.CreateGraphicsFence() or CommandBuffer.CreateGraphicsFence()");
        }

        private int GetPlatformNotSupportedVersion()
        {
            return -1;
        }

        [NativeThrows]
        [FreeFunction("GPUFenceInternals::GetVersionNumber")]
        extern private static int GetVersionNumber(IntPtr fencePtr);
    }
}
