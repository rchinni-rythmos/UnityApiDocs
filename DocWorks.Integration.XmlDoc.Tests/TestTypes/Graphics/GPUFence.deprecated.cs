using System;
using System.Collections.Generic;
using ShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode;
using UnityEngine.Scripting;
using uei = UnityEngine.Internal;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace UnityEngine.Rendering
{
    // Old SynchronisationStage enum, stored here for backwards compatibility
    /// <summary>
    /// Broadly describes the stages of processing a draw call on the GPU.
    /// </summary>
    /// <description>
    /// Used to insert a GPU Fence after or wait for a particular phase of the draw call processing to complete.
    /// </description>
    public enum SynchronisationStage
    {
        /// <summary>
        /// All aspects of vertex processing.
        /// </summary>
        VertexProcessing = 0,
        /// <summary>
        /// The process of creating and shading the fragments.
        /// </summary>
        PixelProcessing = 1,
    }

    /// <summary>
    /// This functionality is deprecated, and should no longer be used. Please use [[GraphicsFence]].
    /// </summary>
    [Obsolete("GPUFence has been deprecated. Use GraphicsFence instead (UnityUpgradable) -> GraphicsFence", false)]
    public struct GPUFence
    {
        /// <summary>
        /// This functionality is deprecated, and should no longer be used. Please use [[GraphicsFence.passed]].
        /// </summary>
        public bool passed
        {
            get
            {
                return true;
            }
        }
    }
}
