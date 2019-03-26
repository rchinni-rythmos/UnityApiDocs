using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Collections;
using UnityEngine.Scripting;
using UnityEngine.Bindings;

namespace UnityEngine
{
    // Script interface for [[wiki:class-Light|light components]].
    [RequireComponent(typeof(Transform))]
    [NativeHeader("Runtime/Export/Graphics/Light.bindings.h")]
    public sealed partial class Light : Behaviour
    {
        /// <summary>
        /// Revert all light parameters to default.
        /// </summary>
        extern public void Reset();

        // How this light casts shadows?
        /// <summary>
        /// How this light casts shadows
        /// </summary>
        /// <description>
        /// SA: [[LightShadows]], shadowStrength property, [[Renderer.shadowCastingMode]], [[Renderer.receiveShadows]].
        /// </description>
        extern public LightShadows shadows
        {
            [NativeMethod("GetShadowType")] get;
            [FreeFunction("Light_Bindings::SetShadowType", HasExplicitThis = true, ThrowsException = true)] set;
        }

        // Strength of light's shadows
        /// <summary>
        /// Strength of light's shadows.
        /// </summary>
        /// <description>
        /// SA: shadows, [[Renderer.shadowCastingMode]], [[Renderer.receiveShadows]].
        /// </description>
        extern public float shadowStrength
        {
            get;
            [FreeFunction("Light_Bindings::SetShadowStrength", HasExplicitThis = true)] set;
        }

        // Shadow resolution
        /// <summary>
        /// The resolution of the shadow map.
        /// </summary>
        /// <description>
        /// SA: shadows property, shadowCustomResolution property, [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        extern public UnityEngine.Rendering.LightShadowResolution shadowResolution
        {
            get;
            [FreeFunction("Light_Bindings::SetShadowResolution", HasExplicitThis = true, ThrowsException = true)] set;
        }

        // Note: do not remove (so that projects with assembly-only scritps using this will continue working),
        // just make it do nothing.
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Shadow softness is removed in Unity 5.0+", true)]
        public float shadowSoftness
        {
            get { return 4.0f; }
            set {}
        }

        // Note: do not remove (so that projects with assembly-only scritps using this will continue working),
        // just make it do nothing.
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Shadow softness is removed in Unity 5.0+", true)]
        public float shadowSoftnessFade
        {
            get { return 1.0f; }
            set {}
        }

        /// <summary>
        /// Per-light, per-layer shadow culling distances.
        /// </summary>
        /// <description>
        /// Dynamic shadows can be cast into view from shadow casters that are very far away from the camera. At low incident light angles, this can lead to a lot of objects needing to cast dynamic shadows, which in turn can result in high rendering costs during shadow maps generation.
        /// Using [[Light.layerShadowCullDistances]] lets you limit, on a per-layer basis, how far from the camera shadows casters are allowed to be before they get culled from shadow maps generation. The feature complements [[Camera.layerCullDistances]], but only affects shadow casting, not regular object rendering.
        /// Just like [[Camera.layerCullDistances]], [[Light.layerShadowCullDistances]] requires that you assign a float array of exactly 32 values. A distance of 0 in a layer's index means keep current behaviour for that layer. Assigning null completely disables shadow distance culling, and is effectively the same as passing an array of 32 zeros.
        /// By default, per-layer shadow culling will use a plane aligned with the camera. You can change this to a sphere by setting [[Camera.layerCullSpherical]] to true. The effect of this flag is shared by both [[Camera.layerCullDistances]] and [[Light.layerShadowCullDistances]].
        /// Please be aware that when you restrict camera culling distances using [[Camera.layerCullDistances]], this also restricts shadow casting to those same culling distances. As a result, if you use [[Camera.layerCullDistances]] and [[Light.layerShadowCullDistances]] at the same time *for the same layer index*, the effective shadow culling distance for that layer will be the smallest of those two distances. For layer indices where one of the values are zero, the other value gets used directly, and for layer indices where both values are zero, no special culling behaviour gets applied for that layer.
        /// See Also: [[Camera.layerCullDistances]], [[Camera.layerCullSpherical]]
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// [RequireComponent(typeof(Light))]
        /// public class LayerShadowCullDistancesExample : MonoBehaviour
        /// {
        ///     void OnEnable()
        ///     {
        ///         // Setup shadow cull distances
        ///         var shadowCullDistances = new float[32];
        ///         shadowCullDistances[10] = 5f;   // Let's imagine this is our 'Tiny Objects' layer
        ///         shadowCullDistances[11] = 15f;  // Let's imagine this is our 'Small Things' layer
        ///         shadowCullDistances[12] = 100f; // Let's imagine this is our 'Trees' layer
        ///         // Assign shadow cull distances. This will only affect layers 10, 11 and 12.
        ///         GetComponent<Light>().layerShadowCullDistances = shadowCullDistances;
        ///     }
        ///     void OnDisable()
        ///     {
        ///         // Completely disable shadow cull distances
        ///         GetComponent<Light>().layerShadowCullDistances = null;
        ///     }
        /// }
        /// </dw-legacy-code>
        extern public float[] layerShadowCullDistances
        {
            [FreeFunction("Light_Bindings::GetLayerShadowCullDistances", HasExplicitThis = true, ThrowsException = false)]
            get;
            [FreeFunction("Light_Bindings::SetLayerShadowCullDistances", HasExplicitThis = true, ThrowsException = true)]
            set;
        }

        /// <summary>
        /// The size of a directional light's cookie.
        /// </summary>
        /// <description>
        /// SA: cookie.
        /// </description>
        extern public float cookieSize { get; set; }

        // The cookie texture projected by the light.
        /// <summary>
        /// The cookie texture projected by the light.
        /// </summary>
        /// <description>
        /// If the cookie is a cube map, the light will become a LightType.Point light.
        /// Note that cookies are only displayed for pixel lights.
        /// SA: [[wiki:class-Light|Light component]].
        /// </description>
        extern public Texture cookie { get; set; }

        // How to render the light.
        /// <summary>
        /// How to render the light.
        /// </summary>
        /// <description>
        /// This can be [[LightRenderMode.Auto]], [[LightRenderMode.ForceVertex]] or [[LightRenderMode.ForcePixel]].
        /// Pixel lights render slower but look better, especially on not very highly tesselated geometry.
        /// Some effects (e.g. bumpmapping) are only displayed for pixel lights.
        /// SA: [[wiki:class-Light|Light component]].
        /// </description>
        extern public LightRenderMode renderMode
        {
            get;
            [FreeFunction("Light_Bindings::SetRenderMode", HasExplicitThis = true, ThrowsException = true)] set;
        }

        // This index was used to denote lights which contribution was baked in lightmaps and/or lightprobes.
        private int m_BakedIndex;
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("warning bakedIndex has been removed please use bakingOutput.isBaked instead.", true)]
        public int bakedIndex { get { return m_BakedIndex; } set { m_BakedIndex = value; } }

        // The size of the area light. Editor only.
        #if UNITY_EDITOR
        /// <summary>
        /// The size of the area light.
        /// </summary>
        /// <description>
        /// This property is only exposed to Editor scripts. It is not exposed during Play mode. SA: [[wiki:class-Light|Light component]].
        /// </description>
        extern public Vector2 areaSize { get; set; }
        #endif

        // Table defining the falloff curve for baked light sources.
        [FreeFunction("Light_Bindings::SetFalloffTable", HasExplicitThis = true, ThrowsException = true)]
        extern private void SetFalloffTable([NotNull] float[] input);

        [FreeFunction("Light_Bindings::SetAllLightsFalloffToInverseSquared")]
        extern private static void SetAllLightsFalloffToInverseSquared();

        [FreeFunction("Light_Bindings::SetAllLightsFalloffToUnityLegacy")]
        extern private static void SetAllLightsFalloffToUnityLegacy();

        // Lightmapping mode. Editor only.
        #if UNITY_EDITOR
        /// <summary>
        /// This property describes what part of a light's contribution can be baked.
        /// </summary>
        /// <description>
        /// If this setting is [[LightmapBakeType.Realtime]], realtime indirect GI can be precomputed, and then updated at runtime. If this setting is [[LightmapBakeType.Baked]], this light will be baked and won't be evaluated at runtime. If this setting is [[LightmapBakeType.Mixed]], this light will be a composition of baked and run time evaluation based on the selected Mixed Light mode in the lighting window's Settings tab.
        /// This property is only exposed to Editor scripts. It is not exposed during Play mode.
        /// </description>
        extern public LightmapBakeType lightmapBakeType
        {
            [NativeMethod("GetBakeType")] get;
            [NativeMethod("SetBakeType")] set;
        }

        /// <summary>
        /// Sets a light dirty to notify the light baking backends to update their internal light representation.
        /// </summary>
        extern public void SetLightDirty();
        #endif

        /// <summary>
        /// Add a command buffer to be executed at a specified place.
        /// </summary>
        /// <param name="evt">
        /// When to execute the command buffer during rendering.
        /// </param>
        /// <param name="buffer">
        /// The buffer to execute.
        /// </param>
        /// <description>
        /// Multiple command buffers can be set to execute at the same light event (or even the same buffer can be added multiple times). To remove command buffer from execution, use RemoveCommandBuffer.
        /// Passing a shadow pass mask allows detailed control over which shadow passes will execute the buffer.
        /// SA: [[Rendering.CommandBuffer]], [[Rendering.ShadowMapPass]], RemoveCommandBuffer, GetCommandBuffers.
        /// </description>
        public void AddCommandBuffer(UnityEngine.Rendering.LightEvent evt, UnityEngine.Rendering.CommandBuffer buffer)
        {
            AddCommandBuffer(evt, buffer, UnityEngine.Rendering.ShadowMapPass.All);
        }

        /// <summary>
        /// Add a command buffer to be executed at a specified place.
        /// </summary>
        /// <param name="evt">
        /// When to execute the command buffer during rendering.
        /// </param>
        /// <param name="buffer">
        /// The buffer to execute.
        /// </param>
        /// <param name="shadowPassMask">
        /// A mask specifying which shadow passes to execute the buffer for.
        /// </param>
        /// <description>
        /// Multiple command buffers can be set to execute at the same light event (or even the same buffer can be added multiple times). To remove command buffer from execution, use RemoveCommandBuffer.
        /// Passing a shadow pass mask allows detailed control over which shadow passes will execute the buffer.
        /// SA: [[Rendering.CommandBuffer]], [[Rendering.ShadowMapPass]], RemoveCommandBuffer, GetCommandBuffers.
        /// </description>
        [FreeFunction("Light_Bindings::AddCommandBuffer", HasExplicitThis = true)]
        public extern void AddCommandBuffer(UnityEngine.Rendering.LightEvent evt, UnityEngine.Rendering.CommandBuffer buffer, UnityEngine.Rendering.ShadowMapPass shadowPassMask);

        /// <summary>
        /// Adds a command buffer to the GPU's async compute queues and executes that command buffer when graphics processing reaches a given point.
        /// </summary>
        /// <param name="evt">
        /// The point during the graphics processing at which this command buffer should commence on the GPU.
        /// </param>
        /// <param name="buffer">
        /// The buffer to execute.
        /// </param>
        /// <param name="queueType">
        /// The desired async compute queue type to execute the buffer on.
        /// </param>
        /// <description>
        /// Execute an async compute command buffer on the GPU when the graphics queues processing reaches a point described by the evt parameter.
        /// Multiple command buffers can be set to execute at the same light event (or even the same buffer can be added multiple times). To remove a command buffer from execution, use RemoveCommandBuffer.
        /// Passing a shadow pass mask allows detailed control over which shadow passes will execute the buffer.
        /// The command buffer can only call the following commands for execution on the async compute queues, otherwise an error is logged and displayed in the Editor window:
        /// [[CommandBuffer.BeginSample]]
        /// [[CommandBuffer.CopyCounterValue]]
        /// [[CommandBuffer.CopyTexture]]
        /// [[CommandBuffer.CreateGPUFence]]
        /// [[CommandBuffer.DispatchCompute]]
        /// [[CommandBuffer.EndSample]]
        /// [[CommandBuffer.IssuePluginEvent]]
        /// [[CommandBuffer.SetComputeBufferParam]]
        /// [[CommandBuffer.SetComputeFloatParam]]
        /// [[CommandBuffer.SetComputeFloatParams]]
        /// [[CommandBuffer.SetComputeTextureParam]]
        /// [[CommandBuffer.SetComputeVectorParam]]
        /// [[CommandBuffer.WaitOnGPUFence]]
        /// All of the commands within the buffer are guaranteed to be executed on the same queue. If the target platform does not support async compute queues then the work is dispatched on the graphics queue.
        /// SA:[[GPUFence]], [[SystemInfo.supportsAsyncCompute]], [[Rendering.CommandBuffer]], RemoveCommandBuffer, GetCommandBuffers.
        /// </description>
        public void AddCommandBufferAsync(UnityEngine.Rendering.LightEvent evt, UnityEngine.Rendering.CommandBuffer buffer, UnityEngine.Rendering.ComputeQueueType queueType)
        {
            AddCommandBufferAsync(evt, buffer, UnityEngine.Rendering.ShadowMapPass.All, queueType);
        }

        /// <summary>
        /// Adds a command buffer to the GPU's async compute queues and executes that command buffer when graphics processing reaches a given point.
        /// </summary>
        /// <param name="evt">
        /// The point during the graphics processing at which this command buffer should commence on the GPU.
        /// </param>
        /// <param name="buffer">
        /// The buffer to execute.
        /// </param>
        /// <param name="shadowPassMask">
        /// A mask specifying which shadow passes to execute the buffer for.
        /// </param>
        /// <param name="queueType">
        /// The desired async compute queue type to execute the buffer on.
        /// </param>
        /// <description>
        /// Execute an async compute command buffer on the GPU when the graphics queues processing reaches a point described by the evt parameter.
        /// Multiple command buffers can be set to execute at the same light event (or even the same buffer can be added multiple times). To remove a command buffer from execution, use RemoveCommandBuffer.
        /// Passing a shadow pass mask allows detailed control over which shadow passes will execute the buffer.
        /// The command buffer can only call the following commands for execution on the async compute queues, otherwise an error is logged and displayed in the Editor window:
        /// [[CommandBuffer.BeginSample]]
        /// [[CommandBuffer.CopyCounterValue]]
        /// [[CommandBuffer.CopyTexture]]
        /// [[CommandBuffer.CreateGPUFence]]
        /// [[CommandBuffer.DispatchCompute]]
        /// [[CommandBuffer.EndSample]]
        /// [[CommandBuffer.IssuePluginEvent]]
        /// [[CommandBuffer.SetComputeBufferParam]]
        /// [[CommandBuffer.SetComputeFloatParam]]
        /// [[CommandBuffer.SetComputeFloatParams]]
        /// [[CommandBuffer.SetComputeTextureParam]]
        /// [[CommandBuffer.SetComputeVectorParam]]
        /// [[CommandBuffer.WaitOnGPUFence]]
        /// All of the commands within the buffer are guaranteed to be executed on the same queue. If the target platform does not support async compute queues then the work is dispatched on the graphics queue.
        /// SA:[[GPUFence]], [[SystemInfo.supportsAsyncCompute]], [[Rendering.CommandBuffer]], RemoveCommandBuffer, GetCommandBuffers.
        /// </description>
        [FreeFunction("Light_Bindings::AddCommandBufferAsync", HasExplicitThis = true)]
        public extern void AddCommandBufferAsync(UnityEngine.Rendering.LightEvent evt, UnityEngine.Rendering.CommandBuffer buffer, UnityEngine.Rendering.ShadowMapPass shadowPassMask, UnityEngine.Rendering.ComputeQueueType queueType);

        /// <summary>
        /// Remove command buffer from execution at a specified place.
        /// </summary>
        /// <param name="evt">
        /// When to execute the command buffer during rendering.
        /// </param>
        /// <param name="buffer">
        /// The buffer to execute.
        /// </param>
        /// <description>
        /// If the same buffer is added multiple times on this light event, all occurrences of it will be removed.
        /// SA: [[Rendering.CommandBuffer]], RemoveCommandBuffers, RemoveAllCommandBuffers, AddCommandBuffer, GetCommandBuffers.
        /// </description>
        extern public void RemoveCommandBuffer(UnityEngine.Rendering.LightEvent evt, UnityEngine.Rendering.CommandBuffer buffer);

        /// <summary>
        /// Remove command buffers from execution at a specified place.
        /// </summary>
        /// <param name="evt">
        /// When to execute the command buffer during rendering.
        /// </param>
        /// <description>
        /// This function removes all command buffers set on the specified light event.
        /// SA: [[Rendering.CommandBuffer]], RemoveCommandBuffer, RemoveAllCommandBuffers.
        /// </description>
        extern public void RemoveCommandBuffers(UnityEngine.Rendering.LightEvent evt);

        /// <summary>
        /// Remove all command buffers set on this light.
        /// </summary>
        /// <description>
        /// SA: [[Rendering.CommandBuffer]], RemoveCommandBuffer, RemoveCommandBuffers.
        /// </description>
        extern public void RemoveAllCommandBuffers();

        /// <summary>
        /// Get command buffers to be executed at a specified place.
        /// </summary>
        /// <param name="evt">
        /// When to execute the command buffer during rendering.
        /// </param>
        /// <returns>
        /// Array of command buffers.
        /// </returns>
        /// <description>
        /// SA: [[Rendering.CommandBuffer]], AddCommandBuffer, RemoveCommandBuffer.
        /// </description>
        [FreeFunction("Light_Bindings::GetCommandBuffers", HasExplicitThis = true)]
        extern public UnityEngine.Rendering.CommandBuffer[] GetCommandBuffers(UnityEngine.Rendering.LightEvent evt);

        /// <summary>
        /// Number of command buffers set up on this light (RO).
        /// </summary>
        /// <description>
        /// SA: [[Rendering.CommandBuffer]], AddCommandBuffer, RemoveCommandBuffer, GetCommandBuffers.
        /// </description>
        extern public int commandBufferCount { get; }


        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.Obsolete("Use QualitySettings.pixelLightCount instead.")]
        public static int pixelLightCount
        {
            get { return QualitySettings.pixelLightCount; }
            set { QualitySettings.pixelLightCount = value; }
        }

        //*undocumented For terrain engine only
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [FreeFunction("Light_Bindings::GetLights")]
        extern public static Light[] GetLights(LightType type, int layer);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("light.shadowConstantBias was removed, use light.shadowBias", true)]
        public float shadowConstantBias { get { return 0.0f; } set {} }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("light.shadowObjectSizeBias was removed, use light.shadowBias", true)]
        public float shadowObjectSizeBias { get { return 0.0f; } set {} }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("light.attenuate was removed; all lights always attenuate now", true)]
        public bool attenuate { get { return true; } set {} }
    }
}
