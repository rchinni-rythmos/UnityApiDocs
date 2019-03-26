using System;
using UnityEngine.Bindings;
using UnityEngine.Scripting;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine
{
    /// <summary>
    /// Rendering path of a [[Camera]].
    /// </summary>
    public enum RenderingPath
    {
        /// <summary>
        /// Use Player Settings.
        /// </summary>
        UsePlayerSettings = -1,
        /// <summary>
        /// Vertex Lit.
        /// </summary>
        VertexLit = 0,
        /// <summary>
        /// Forward Rendering.
        /// </summary>
        Forward = 1,
        /// <summary>
        /// Deferred Lighting (Legacy).
        /// </summary>
        /// <description>
        /// This is a deferred lighting path (also known as "light pre-pass"), that
        /// renders Scene information into a very small G-Buffer twice, computing lighting in between.
        /// It does not require a GPU with multiple render targets (MRT) support, but is a lot
        /// less flexible than [[RenderingPath.DeferredShading]] is.
        /// Note: Deferred rendering is not compatible with orthographic camera projection.
        /// </description>
        DeferredLighting = 2,
        /// <summary>
        /// Deferred Shading.
        /// </summary>
        /// <description>
        /// This is a standard deferred shading path, that renders Scene information
        /// into G-Buffers using multiple render targets, and computes lighting afterwards.
        /// Due to use of multiple render targets, it requires GPU with MRT support.
        /// Note: Deferred rendering is not compatible with orthographic camera projection.
        /// </description>
        DeferredShading = 3
    }

    // Match Camera::TransparencySortMode on C++ side
    /// <summary>
    /// Transparent object sorting mode of a [[Camera]].
    /// </summary>
    /// <description>
    /// By default, perspective cameras sort objects based on distance from camera position
    /// to the object center; and orthographic cameras sort based on distance along the view direction.
    /// If you're making a 2D game with a perspective camera, you might want to use [[TransparencySortMode.Orthographic]]
    /// sort mode so that objects are sorted based on distance along the camera's view.
    /// SA: [[Camera.transparencySortMode]].
    /// </description>
    public enum TransparencySortMode
    {
        /// <summary>
        /// Default transparency sorting mode.
        /// </summary>
        /// <description>
        /// SA: [[Camera.transparencySortMode]].
        /// </description>
        Default = 0,
        /// <summary>
        /// Perspective transparency sorting mode.
        /// </summary>
        /// <description>
        /// Transparent objects will be sorted based on distance from camera position to the object center.
        /// SA: [[Camera.transparencySortMode]].
        /// </description>
        Perspective = 1,
        /// <summary>
        /// Orthographic transparency sorting mode.
        /// </summary>
        /// <description>
        /// Transparent objects will be sorted based on distance along the camera's view.
        /// SA: [[Camera.transparencySortMode]].
        /// </description>
        Orthographic = 2,
        /// <summary>
        /// Sort objects based on distance along a custom axis.
        /// </summary>
        /// <description>
        /// Transparent objects are sorted based on distance along a custom axis. For example, you could specify this mode and the axis to be (0.0f, 1.0f, 0.0f). This will effectively make renderers sorted to the back as they go up in Y. This is a common feature of 2.5D games.
        /// Note: This has a lower priority compared to other sorting criterias such as [[SortingLayer]].
        /// SA: [[Camera.transparencySortMode]], [[Camera.transparencySortAxis]], [[GraphicsSettings.transparencySortMode]].
        /// </description>
        CustomAxis = 3
    }

    // Match the TargetEyeMask enum in GfxDeviceTypes.h on C++ side
    // bitshifts must match the StereoscopicEye enum
    /// <summary>
    /// Enum values for the Camera's targetEye property.
    /// </summary>
    public enum StereoTargetEyeMask
    {
        /// <summary>
        /// Do not render either eye to the HMD.
        /// </summary>
        None = 0,
        /// <summary>
        /// Render only the Left eye to the HMD.
        /// </summary>
        Left = 1 << 0,
        /// <summary>
        /// Render only the right eye to the HMD.
        /// </summary>
        Right = 1 << 1,
        /// <summary>
        /// Render both eyes to the HMD.
        /// </summary>
        Both = Left | Right
    }

    /// <summary>
    /// Describes different types of camera.
    /// </summary>
    [Flags]
    public enum CameraType
    {
        /// <summary>
        /// Used to indicate a regular in-game camera.
        /// </summary>
        Game = 1,
        /// <summary>
        /// Used to indicate that a camera is used for rendering the Scene View in the Editor.
        /// </summary>
        SceneView = 2,
        /// <summary>
        /// Used to indicate a camera that is used for rendering previews in the Editor.
        /// </summary>
        Preview = 4,
        /// <summary>
        /// Used to indicate that a camera is used for rendering VR (in edit mode) in the Editor.
        /// </summary>
        VR = 8,
        /// <summary>
        /// Used to indicate a camera that is used for rendering reflection probes.
        /// </summary>
        Reflection = 16
    }

    /// <summary>
    /// [[ComputeBuffer]] type.
    /// </summary>
    /// <description>
    /// Different types of compute buffers map to different usage and declarations in HLSL shaders. Default type is "structured buffer" (@@StructuredBuffer<T>@@ or @@RWStructuredBuffer<T>@@).
    /// SA: [[ComputeBuffer]], [[ComputeShader]], [[Material.SetBuffer]].
    /// </description>
    [Flags]
    public enum ComputeBufferType
    {
        /// <summary>
        /// Default [[ComputeBuffer]] type (structured buffer).
        /// </summary>
        /// <description>
        /// In HLSL shaders, this maps to @@StructuredBuffer<T>@@ or @@RWStructuredBuffer<T>@@.
        /// The /stride/ passed when constructing the buffer must match structure size, be a multiple of 4 and less than 2048.
        /// See Microsoft's HLSL documentation on <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff471514.aspx">StructuredBuffer</a> and <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff471494.aspx">RWStructuredBuffer</a>.
        /// SA: [[ComputeBuffer]], [[ComputeShader]], [[Material.SetBuffer]].
        /// </description>
        Default = 0,
        /// <summary>
        /// Raw [[ComputeBuffer]] type (byte address buffer).
        /// </summary>
        /// <description>
        /// In HLSL shaders, this maps to @@ByteAddressBuffer@@ or @@RWByteAddressBuffer@@. Underlying DX11 format for shader access is typeless R32.
        /// See Microsoft's HLSL documentation on <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff471453.aspx">ByteAddressBuffer</a> and <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff471475.aspx">RWByteAddressBuffer</a>.
        /// SA: [[ComputeBuffer]], [[ComputeShader]], [[Material.SetBuffer]].
        /// </description>
        Raw = 1,
        /// <summary>
        /// Append-consume [[ComputeBuffer]] type.
        /// </summary>
        /// <description>
        /// Allows a buffer to be treated like a stack in compute shaders. Maps to @@AppendStructuredBuffer<T>@@ or @@ConsumeStructuredBuffer<T>@@ in HLSL.
        /// The /stride/ passed when constructing the buffer must match structure size, be a multiple of 4 and less than 2048.
        /// See Microsoft's HLSL documentation on <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff471448.aspx">AppendStructuredBuffer</a> and <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff471459.aspx">ConsumeStructuredBuffer</a>.
        /// The buffer size value can be copied into another buffer using [[ComputeBuffer.CopyCount]], or explicitly reset with [[ComputeBuffer.SetCounterValue]].
        /// SA: [[ComputeBuffer]], [[ComputeShader]], [[Material.SetBuffer]].
        /// </description>
        Append = 2,
        /// <summary>
        /// [[ComputeBuffer]] with a counter.
        /// </summary>
        /// <description>
        /// Adds a "counter" to a @@RWStructuredBuffer@@ and allows using @@IncrementCounter@@ / @@DecrementCounter@@ HLSL functions on it.
        /// The /stride/ passed when constructing the buffer must match structure size, be a multiple of 4 and less than 2048.
        /// See Microsoft's HLSL documentation on <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff471497.aspx">IncrementCounter</a> and <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/ff471495.aspx">DecrementCounter</a>.
        /// The buffer size value can be copied into another buffer using [[ComputeBuffer.CopyCount]], or explicitly reset with [[ComputeBuffer.SetCounterValue]].
        /// SA: [[ComputeBuffer]], [[ComputeShader]], [[Material.SetBuffer]].
        /// </description>
        Counter = 4,
        /// <summary>
        /// [[ComputeBuffer]] that you can use as a constant buffer (uniform buffer).
        /// </summary>
        /// <description>
        /// If you use this flag, you can use the [[ComputeBuffer]] as a parameter to [[Shader.SetConstantBuffer]] and [[Material.SetConstantBuffer]]. If you also need the buffer to be bound as a structured buffer, you must add the [[ComputeBufferType.StructuredBuffer]] flag. Some renderers (such as DX11) do not support binding buffers as both constant and structured buffers.
        /// </description>
        Constant = 8,
        /// <summary>
        /// [[ComputeBuffer]] that you can use as a structured buffer.
        /// </summary>
        /// <description>
        /// This is otherwise identical to [[ComputeBufferType.Default]] except that if any other [[ComputeBufferType]] flags are used, the resulting ComputeBuffer will not be able to be bound as a structured buffer unless [[ComputeBufferType.Structured]] is explicitly added.
        /// </description>
        Structured = 16,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.Obsolete("Enum member DrawIndirect has been deprecated. Use IndirectArguments instead (UnityUpgradable) -> IndirectArguments", false)]
        DrawIndirect = 256,
        /// <summary>
        /// [[ComputeBuffer]] used for [[Graphics.DrawProceduralIndirect]], [[ComputeShader.DispatchIndirect]] or [[Graphics.DrawMeshInstancedIndirect]] arguments.
        /// </summary>
        /// <description>
        /// Buffer size has to be at least 12 bytes. Underlying DX11 unordered access view format will be R32_UINT, and shader resource view format will be R32 typeless.
        /// SA: [[ComputeBuffer]], [[ComputeShader]], [[Material.SetBuffer]], [[ComputeBuffer.CopyCount]].
        /// </description>
        IndirectArguments = 256,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.Obsolete("Enum member GPUMemory has been deprecated. All compute buffers now follow the behavior previously defined by this member.", false)]
        GPUMemory = 512
    }

    /// <summary>
    /// The type of a [[Light]].
    /// </summary>
    /// <description>
    /// SA: [[Light.type]], [[wiki:class-Light|light component]].
    /// </description>
    public enum LightType
    {
        /// <summary>
        /// The light is a spot light.
        /// </summary>
        /// <description>
        /// SA: [[Light.type]], [[wiki:class-Light|light component]].
        /// </description>
        Spot = 0,
        /// <summary>
        /// The light is a directional light.
        /// </summary>
        /// <description>
        /// SA: [[Light.type]], [[wiki:class-Light|light component]].
        /// </description>
        Directional = 1,
        /// <summary>
        /// The light is a point light.
        /// </summary>
        /// <description>
        /// SA: [[Light.type]], [[wiki:class-Light|light component]].
        /// </description>
        Point = 2,

        //[System.Obsolete("Enum member LightType.Area has been deprecated. Use LightType.Rectangle instead (UnityUpgradable) -> Rectangle", true)]
        // For now, we will have both Area and Rectangle in the source, but this will be removed once the SRP package is in.
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        Area = 3,

        /// <summary>
        /// The light is a rectangle shaped area light. It affects only baked lightmaps and lightprobes.
        /// </summary>
        /// <description>
        /// SA: [[Light.type]], [[wiki:class-Light|light component]].
        /// </description>
        Rectangle = 3,
        /// <summary>
        /// The light is a disc shaped area light. It affects only baked lightmaps and lightprobes.
        /// </summary>
        /// <description>
        /// SA: [[Light.type]], [[wiki:class-Light|light component]].
        /// </description>
        Disc = 4
    }

    /// <summary>
    /// How the [[Light]] is rendered.
    /// </summary>
    /// <description>
    /// SA: [[wiki:class-Light|light component]].
    /// </description>
    public enum LightRenderMode
    {
        /// <summary>
        /// Automatically choose the render mode.
        /// </summary>
        /// <description>
        /// This chooses whether to render the [[Light]] as a pixel or vertex light (recommended and default).
        /// SA: [[wiki:class-Light|light component]].
        /// </description>
        Auto = 0,
        /// <summary>
        /// Force the [[Light]] to be a pixel light.
        /// </summary>
        /// <description>
        /// Use this only for really important lights, like a player flashlight.
        /// SA: [[wiki:class-Light|light component]].
        /// </description>
        ForcePixel = 1,
        /// <summary>
        /// Force the [[Light]] to be a vertex light.
        /// </summary>
        /// <description>
        /// This option is good for background or distant lighting.
        /// SA: [[wiki:class-Light|light component]].
        /// </description>
        ForceVertex = 2
    }

    /// <summary>
    /// Shadow casting options for a [[Light]].
    /// </summary>
    /// <description>
    /// SA: [[wiki:class-Light|light component]].
    /// </description>
    public enum LightShadows
    {
        /// <summary>
        /// Do not cast shadows (default).
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-Light|light component]].
        /// </description>
        None = 0,
        /// <summary>
        /// Cast "hard" shadows (with no shadow filtering).
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-Light|light component]].
        /// </description>
        Hard = 1,
        /// <summary>
        /// Cast "soft" shadows (with 4x PCF filtering).
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-Light|light component]].
        /// </description>
        Soft = 2
    }

    /// <summary>
    /// Fog mode to use.
    /// </summary>
    /// <description>
    /// SA: [[RenderSettings.fogMode]], [[wiki:GlobalIllumination|Lighting Window]].
    /// </description>
    public enum FogMode
    {
        /// <summary>
        /// Linear fog.
        /// </summary>
        /// <description>
        /// SA: [[RenderSettings.fogMode]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        Linear = 1,
        /// <summary>
        /// Exponential fog.
        /// </summary>
        /// <description>
        /// SA: [[RenderSettings.fogMode]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        Exponential = 2,
        /// <summary>
        /// Exponential squared fog (default).
        /// </summary>
        /// <description>
        /// SA: [[RenderSettings.fogMode]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        ExponentialSquared = 3
    }

    /// <summary>
    /// Enum describing what part of a light contribution can be baked.
    /// </summary>
    [Flags]
    public enum LightmapBakeType
    {
        /// <summary>
        /// Realtime lights cast run time light and shadows. They can change position, orientation, color, brightness, and many other properties at run time. No lighting gets baked into lightmaps or light probes..
        /// </summary>
        Realtime = 4,
        /// <summary>
        /// Baked lights cannot move or change in any way during run time. All lighting for static objects gets baked into lightmaps. Lighting and shadows for dynamic objects gets baked into Light Probes.
        /// </summary>
        Baked = 2,
        /// <summary>
        /// Mixed lights allow a mix of realtime and baked lighting, based on the Mixed Lighting Mode used. These lights cannot move, but can change color and intensity at run time. Changes to color and intensity only affect direct lighting as indirect lighting gets baked. If using Subtractive mode, changes to color or intensity are not calculated at run time on static objects.
        /// </summary>
        Mixed = 1
    }

    // make sure to add any new mode to LightmapMixedBakeMode for SRP so that it can be supported
    /// <summary>
    /// Enum describing what lighting mode to be used with Mixed lights.
    /// </summary>
    /// <description>
    /// Summary of the baked data associated with each mode:
    /// __IndirectOnly__\\
    /// Lightmaps\\
    /// - direct: no\\
    /// - occlusion: no\\
    /// Light probes\\
    /// - direct: no\\
    /// - occlusion: no
    /// __Shadowmask__\\
    /// Lightmaps\\
    /// - direct: no\\
    /// - occlusion: yes\\
    /// Light probes\\
    /// - direct: no\\
    /// - occlusion: yes
    /// __Subtractive__\\
    /// Lightmaps\\
    /// - direct: yes\\
    /// - occlusion: no\\
    /// Light probes\\
    /// - direct: no\\
    /// - occlusion: yes.
    /// </description>
    public enum MixedLightingMode
    {
        /// <summary>
        /// Mixed lights provide realtime direct lighting while indirect light is baked into lightmaps and light probes.
        /// </summary>
        IndirectOnly = 0,
        /// <summary>
        /// Mixed lights provide realtime direct lighting. Indirect lighting gets baked into lightmaps and light probes. Shadowmasks and light probe occlusion get generated for baked shadows. The Shadowmask Mode used at run time can be set in the Quality Settings panel.
        /// </summary>
        Shadowmask = 2,
        /// <summary>
        /// Mixed lights provide baked direct and indirect lighting for static objects. Dynamic objects receive realtime direct lighting and cast shadows on static objects using the main directional light in the Scene.
        /// </summary>
        Subtractive = 1
    };

    // Must match ReceiveGIEnum::ReceiveGI on the C++ side
    /// <summary>
    /// Determines if the object receives Global Illumination from its surroundings through either Lightmaps or LightProbes. Forced to LightProbes if Contribute GI is turned off.
    /// </summary>
    public enum ReceiveGI
    {
        // Off = 0,
        /// <summary>
        /// Makes the GameObject use lightmaps to receive Global Illumination.
        /// </summary>
        Lightmaps = 1,
        /// <summary>
        /// The object will have the option to use Light Probes to receive Global Illumination. See [[Rendering.LightProbeUsage]].
        /// </summary>
        LightProbes = 2
    };

    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    [Obsolete("See QualitySettings.names, QualitySettings.SetQualityLevel, and QualitySettings.GetQualityLevel")]
    public enum QualityLevel
    {
        /// <summary>
        /// The "fastest" quality level.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.currentLevel]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        Fastest = 0,
        /// <summary>
        /// The "fast" quality level.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.currentLevel]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        Fast = 1,
        /// <summary>
        /// The "simple" quality level.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.currentLevel]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        Simple = 2,
        /// <summary>
        /// The "good" quality level.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.currentLevel]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        Good = 3,
        /// <summary>
        /// The "beautiful" quality level.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.currentLevel]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        Beautiful = 4,
        /// <summary>
        /// The "fantastic" quality level.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.currentLevel]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        Fantastic = 5
    }

    /// <summary>
    /// Shadow projection type for [[wiki:class-QualitySettings|Quality Settings]].
    /// </summary>
    public enum ShadowProjection
    {
        /// <summary>
        /// Close fit shadow maps with linear fadeout.
        /// </summary>
        CloseFit = 0,
        /// <summary>
        /// Stable shadow maps with spherical fadeout.
        /// </summary>
        StableFit = 1
    }

    /// <summary>
    /// Determines which type of shadows should be used.
    /// </summary>
    /// <description>
    /// The available options are Hard and Soft Shadows, Hard Shadows Only and Disable Shadows.
    /// SA: [[QualitySettings.shadows]], [[Light.shadows]].
    /// </description>
    public enum ShadowQuality
    {
        /// <summary>
        /// Disable Shadows.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadows]], [[Light.shadows]].
        /// </description>
        Disable = 0,
        /// <summary>
        /// Hard Shadows Only.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadows]], [[Light.shadows]].
        /// </description>
        HardOnly = 1,
        /// <summary>
        /// Hard and Soft Shadows.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadows]], [[Light.shadows]].
        /// </description>
        All = 2
    }

    /// <summary>
    /// Default shadow resolution.
    /// </summary>
    /// <description>
    /// SA: [[QualitySettings.shadowResolution]], [[Light.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
    /// </description>
    public enum ShadowResolution
    {
        /// <summary>
        /// Low shadow map resolution.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        Low = UnityEngine.Rendering.LightShadowResolution.Low,
        /// <summary>
        /// Medium shadow map resolution.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        Medium = UnityEngine.Rendering.LightShadowResolution.Medium,
        /// <summary>
        /// High shadow map resolution.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        High = UnityEngine.Rendering.LightShadowResolution.High,
        /// <summary>
        /// Very high shadow map resolution.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        VeryHigh = UnityEngine.Rendering.LightShadowResolution.VeryHigh
    }

    /// <summary>
    /// The rendering mode of Shadowmask.
    /// </summary>
    /// <description>
    /// Set whether static shadow casters should be rendered into realtime shadow maps.
    ///           SA: [[QualitySettings.shadowmaskMode]], [[QualitySettings.shadowDistance]].
    /// </description>
    public enum ShadowmaskMode
    {
        /// <summary>
        /// Static shadow casters won't be rendered into realtime shadow maps. All shadows from static casters are handled via Shadowmasks and occlusion from Light Probes.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadowmaskMode]], [[QualitySettings.shadowDistance]].
        /// </description>
        Shadowmask = 0,
        /// <summary>
        /// Static shadow casters will be rendered into realtime shadow maps. Shadowmasks and occlusion from Light Probes will only be used past the realtime shadow distance.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.shadowmaskMode]], [[QualitySettings.shadowDistance]].
        /// </description>
        DistanceShadowmask = 1
    }

    /// <summary>
    /// Values for Camera.clearFlags, determining what to clear when rendering a [[Camera]].
    /// </summary>
    /// <description>
    /// SA: [[wiki:class-Camera|camera component]].
    /// </description>
    public enum CameraClearFlags
    {
        /// <summary>
        /// Clear with the skybox.
        /// </summary>
        /// <description>
        /// If a skybox is not set up, the Camera will clear with a Camera.backgroundColor.
        /// SA: [[Camera.clearFlags]] property, [[wiki:class-Camera|camera component]], [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        Skybox = 1,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        Color = 2,
        /// <summary>
        /// Clear with a background color.
        /// </summary>
        /// <description>
        /// SA: [[Camera.clearFlags]], [[wiki:class-Camera|camera component]], [[Camera.backgroundColor]]
        /// </description>
        SolidColor = 2,
        /// <summary>
        /// Clear only the depth buffer.
        /// </summary>
        /// <description>
        /// This will leave colors from the previous frame or whatever was displayed before.
        /// SA: [[Camera.clearFlags]] property, [[wiki:class-Camera|camera component]].
        /// </description>
        Depth = 3,
        /// <summary>
        /// Don't clear anything.
        /// </summary>
        /// <description>
        /// This will leave colors and depth buffer from the previous frame or whatever was displayed before.
        /// SA: [[Camera.clearFlags]] property, [[wiki:class-Camera|camera component]].
        /// </description>
        Nothing = 4
    }

    /// <summary>
    /// Depth texture generation mode for [[Camera]].
    /// </summary>
    /// <description>
    /// The flags can be combined, so you can set a Camera to generate any combination of: Depth, Depth+Normals, and MotionVector textures if needed.
    /// SA: [[wiki:SL-CameraDepthTexture|Using camera's depth textures]], Camera.depthTextureMode.
    /// </description>
    [Flags]
    public enum DepthTextureMode
    {
        /// <summary>
        /// Do not generate depth texture (Default).
        /// </summary>
        /// <description>
        /// SA: [[wiki:SL-CameraDepthTexture|Using camera's depth textures]], Camera.depthTextureMode.
        /// </description>
        None = 0,
        /// <summary>
        /// Generate a depth texture.
        /// </summary>
        /// <description>
        /// Will generate a screen-space depth texture as seen from this camera.
        /// Texture will be in [[RenderTextureFormat.Depth]] format and will be set as @@_CameraDepthTexture@@
        /// global shader property.
        /// SA: [[wiki:SL-CameraDepthTexture|Using camera's depth textures]], Camera.depthTextureMode.
        /// </description>
        Depth = 1,
        /// <summary>
        /// Generate a depth + normals texture.
        /// </summary>
        /// <description>
        /// Will generate a screen-space depth and view space normals texture as seen from this camera.
        /// Texture will be in [[RenderTextureFormat.ARGB32]] format and will be set as @@_CameraDepthNormalsTexture@@
        /// global shader property. Depth and normals will be specially encoded, see [[wiki:comp:SL-CameraDepthTexture|Camera Depth Texture]] page for details.
        /// SA: [[wiki:SL-CameraDepthTexture|Using camera's depth textures]], Camera.depthTextureMode.
        /// </description>
        DepthNormals = 2,
        /// <summary>
        /// Specifies whether motion vectors should be rendered (if possible).
        /// </summary>
        /// <description>
        /// When set, the camera renders another pass (after opaque but before Image Effects): First, a full screen pass is rendered to reconstruct screen-space motion from the camera movement, then, any moving objects have a custom pass to render their object-specific motion. The buffer uses the [[RenderTextureFormat.RGHalf]] format, so this feature only works on platforms where this format is supported.
        /// Motion vectors capture the per-pixel, screen-space motion of objects from one frame to the next. Use this velocity to reconstruct previous positions, calculate blur for motion blur, or implement temporal anti-aliasing.
        /// To access the generated motion vectors, you can simple read the texture sampler: sampler2D_half _CameraMotionVectorsTexture in any opaque Image Effect.
        /// SA: [[Renderer.motionVectorGenerationMode]], [[Camera.depthTextureMode]], [[SkinnedMeshRenderer.skinnedMotionVectors]], [[PassType.MotionVectors]], [[SystemInfo.supportsMotionVectors]].
        /// </description>
        MotionVectors = 4
    }

    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    public enum TexGenMode
    {
        /// <summary>
        /// The texture gets its coordinates from UV.
        /// </summary>
        None  = 0,
        /// <summary>
        /// The texture uses spherical reflection mappping.
        /// </summary>
        SphereMap = 1,
        /// <summary>
        /// The texture is applied in object space.
        /// </summary>
        Object = 2,
        /// <summary>
        /// Projected Eye space.
        /// </summary>
        EyeLinear = 3,
        /// <summary>
        /// Cubemap reflection calculation.
        /// </summary>
        CubeReflect = 4,
        /// <summary>
        /// Cubemap normal calculation.
        /// </summary>
        CubeNormal = 5
    }

    /// <summary>
    /// Anisotropic filtering mode.
    /// </summary>
    /// <description>
    /// SA: [[QualitySettings.anisotropicFiltering]].
    /// </description>
    public enum AnisotropicFiltering
    {
        /// <summary>
        /// Disable anisotropic filtering for all textures.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.anisotropicFiltering]].
        /// </description>
        Disable = 0,
        /// <summary>
        /// Enable anisotropic filtering, as set for each texture.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.anisotropicFiltering]].
        /// </description>
        Enable = 1,
        /// <summary>
        /// Enable anisotropic filtering for all textures.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.anisotropicFiltering]].
        /// </description>
        ForceEnable = 2
    }

    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    [Obsolete("BlendWeights is obsolete. Use SkinWeights instead (UnityUpgradable) -> SkinWeights", true)]
    public enum BlendWeights
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("BlendWeights.OneBone is obsolete. Use SkinWeights.OneBone instead (UnityUpgradable) -> SkinWeights.OneBone", true)]
        OneBone = 1,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("BlendWeights.TwoBones is obsolete. Use SkinWeights.TwoBones instead (UnityUpgradable) -> SkinWeights.TwoBones", true)]
        TwoBones = 2,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("BlendWeights.FourBones is obsolete. Use SkinWeights.FourBones instead (UnityUpgradable) -> SkinWeights.FourBones", true)]
        FourBones = 4,
    }
    /// <summary>
    /// Skin weights.
    /// </summary>
    /// <description>
    /// How many bones affect each vertex.
    /// SA: QualitySettings.skinWeights.
    /// </description>
    public enum SkinWeights
    {
        /// <summary>
        /// One bone affects each vertex.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.skinWeights]].
        /// </description>
        OneBone = 1,
        /// <summary>
        /// Two bones affect each vertex.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.skinWeights]].
        /// </description>
        TwoBones = 2,
        /// <summary>
        /// Four bones affect each vertex.
        /// </summary>
        /// <description>
        /// SA: [[QualitySettings.skinWeights]].
        /// </description>
        FourBones = 4,
        /// <summary>
        /// An unlimited number of bones affect each vertex.
        /// </summary>
        /// <description>
        /// The count is only limited by the number of bones per vertex in the Mesh, which can be adjusted in its Import Setttings.
        /// SA: [[QualitySettings.skinWeights]].
        /// </description>
        Unlimited = 255
    }

    /// <summary>
    /// Topology of [[Mesh]] faces.
    /// </summary>
    /// <description>
    /// Normally meshes are composed of triangles (three vertex indices per face), but
    /// in some cases you might want to render complex things that are made up from lines
    /// or points. Creating a [[Mesh]] with that topology and using it to render is usually the
    /// most efficient way.
    /// SA: [[Mesh.SetIndices]] function.
    /// </description>
    public enum MeshTopology
    {
        /// <summary>
        /// Mesh is made from triangles.
        /// </summary>
        /// <description>
        /// Each three indices in the mesh index buffer form a triangular face.
        /// SA: [[Mesh.SetIndices]] function.
        /// </description>
        Triangles = 0,
        /// <summary>
        /// Mesh is made from quads.
        /// </summary>
        /// <description>
        /// Each four indices in the mesh index buffer form a quadrangular face.
        /// Note that quad topology is emulated on many platforms, so it's more efficient
        /// to use a triangular mesh. Unless you really need quads, for example if using
        /// DirectX 11 tessellation shaders that operate on quad patches.
        /// SA: [[Mesh.SetIndices]] function.
        /// </description>
        Quads = 2,
        /// <summary>
        /// Mesh is made from lines.
        /// </summary>
        /// <description>
        /// Each two indices in the mesh index buffer form a line.
        /// SA: [[Mesh.SetIndices]] function.
        /// </description>
        Lines = 3,
        /// <summary>
        /// Mesh is a line strip.
        /// </summary>
        /// <description>
        /// First two indices form a line, and then each new index connects a new
        /// vertex to the existing line strip.
        /// SA: [[Mesh.SetIndices]] function.
        /// </description>
        LineStrip = 4,
        /// <summary>
        /// Mesh is made from points.
        /// </summary>
        /// <description>
        /// In most of use cases, mesh index buffer should be "identity":
        /// 0, 1, 2, 3, 4, 5, ...
        /// SA: [[Mesh.SetIndices]] function.
        /// </description>
        Points = 5
    }

    /// <summary>
    /// The maximum number of bones affecting a single vertex.
    /// </summary>
    /// <description>
    /// SA: [[SkinnedMeshRenderer.quality]].
    /// </description>
    public enum SkinQuality
    {
        /// <summary>
        /// Chooses the number of bones from the number current [[QualitySettings]]. (Default)
        /// </summary>
        Auto = 0,
        /// <summary>
        /// Use only 1 bone to deform a single vertex. (The most important bone will be used).
        /// </summary>
        Bone1 = 1,
        /// <summary>
        /// Use 2 bones to deform a single vertex. (The most important bones will be used).
        /// </summary>
        Bone2 = 2,
        /// <summary>
        /// Use 4 bones to deform a single vertex.
        /// </summary>
        Bone4 = 4
    }

    /// <summary>
    /// Color space for player settings.
    /// </summary>
    /// <description>
    /// This enum is used to indicate color space used in project ([[PlayerSettings.colorSpace]]) and sprite atlases ([[AtlasSettings.colorSpace]]).
    /// SA: [[wiki:LinearLighting|Linear and Gamma rendering]], [[RenderTextureReadWrite]].
    /// </description>
    public enum ColorSpace
    {
        /// <summary>
        /// Uninitialized color space.
        /// </summary>
        /// <description>
        /// SA: [[wiki:LinearLighting|Linear and Gamma rendering]], [[ColorSpace]], [[RenderTextureReadWrite]].
        /// </description>
        Uninitialized = -1,
        /// <summary>
        /// Gamma color space.
        /// </summary>
        /// <description>
        /// SA: [[wiki:LinearLighting|Linear and Gamma rendering]], [[ColorSpace]], [[RenderTextureReadWrite]].
        /// </description>
        Gamma = 0,
        /// <summary>
        /// Linear color space.
        /// </summary>
        /// <description>
        /// SA: [[wiki:LinearLighting|Linear and Gamma rendering]], [[ColorSpace]], [[RenderTextureReadWrite]].
        /// </description>
        Linear = 1
    }

    // Match ColorGamut on C++ side
    /// <summary>
    /// Represents a color gamut.
    /// </summary>
    [UsedByNativeCode]
    [NativeHeader("Runtime/Graphics/ColorGamut.h")]
    public enum ColorGamut
    {
        /// <summary>
        /// sRGB color gamut.
        /// </summary>
        /// <description>
        /// sRGB color gamut uses Rec709 primary colors and a non-linear transfer function.
        /// </description>
        sRGB = 0,
        /// <summary>
        /// Rec. 709 color gamut.
        /// </summary>
        /// <description>
        /// ITU-R Recommendation BT.709 color gamut.
        /// </description>
        Rec709 = 1,
        /// <summary>
        /// Rec. 2020 color gamut.
        /// </summary>
        /// <description>
        /// ITU-R Recommendation BT.2020 color gamut.
        /// </description>
        Rec2020 = 2,
        /// <summary>
        /// Display-P3 color gamut.
        /// </summary>
        /// <description>
        /// Display-P3 color gamut uses DCI P3 primary colors. The transfer function is specified the same way as for sRGB.
        /// </description>
        DisplayP3 = 3,
        /// <summary>
        /// HDR10 high dynamic range color gamut.
        /// </summary>
        /// <description>
        /// HDR10 Media Profile color gamut uses the same primary colors as Rec. 2020, but different transfer function and has high dynamic range.
        /// </description>
        HDR10 = 4,
        /// <summary>
        /// DolbyHDR high dynamic range color gamut.
        /// </summary>
        /// <description>
        /// DolbyHDR color gamut uses the same primary colors as Rec. 2020, but different transfer function and has high dynamic range.
        /// </description>
        DolbyHDR = 5
    };

    /// <summary>
    /// Describes screen orientation.
    /// </summary>
    /// <description>
    /// Currently this is only relevant on mobile devices.
    /// SA: [[Screen.orientation]].
    /// </description>
    public enum ScreenOrientation
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.Obsolete("Enum member Unknown has been deprecated.", false)]
        Unknown = 0,

        /// <summary>
        /// Portrait orientation.
        /// </summary>
        /// <description>
        /// SA: [[Screen.orientation]].
        /// </description>
        Portrait = 1,
        /// <summary>
        /// Portrait orientation, upside down.
        /// </summary>
        /// <description>
        /// Available on iOS and on Android 2.3+. On older Androids falls back to Portrait.
        /// SA: [[Screen.orientation]].
        /// </description>
        PortraitUpsideDown = 2,
        /// <summary>
        /// Landscape orientation, counter-clockwise from the portrait orientation.
        /// </summary>
        /// <description>
        /// SA: [[Screen.orientation]].
        /// </description>
        LandscapeLeft = 3,
        /// <summary>
        /// Landscape orientation, clockwise from the portrait orientation.
        /// </summary>
        /// <description>
        /// Available on iOS and on Android 2.3+. On older Androids falls
        /// back to LandscapeLeft.
        /// SA: [[Screen.orientation]].
        /// </description>
        LandscapeRight = 4,
        /// <summary>
        /// Auto-rotates the screen as necessary toward any of the enabled orientations.
        /// </summary>
        /// <description>
        /// When this option is assigned to the [[Screen.orientation]] property, the screen will auto-rotate so that the bottom of the screen image points downwards. The orientations that can be used are set by the Screen.autorotateToLandscapeLeft, Screen.autorotateToLandscapeRight, Screen.autorotateToPortrait and Screen::autorotateToPortraitUpsideDown properties. For example, if Screen.autorotateToPortrait and Screen::autorotateToPortraitUpsideDown are both true but the others are false then the auto-rotation will never choose either of the landscape options even when the device is held with the long side of the screen pointing downwards.
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// public class Example : MonoBehaviour
        /// {
        ///     void Start()
        ///     {
        ///         Screen.autorotateToPortrait = true;
        ///         Screen.autorotateToPortraitUpsideDown = true;
        ///         Screen.orientation = ScreenOrientation.AutoRotation;
        ///     }
        /// }
        /// </dw-legacy-code>
        /// <description>
        /// SA: [[Screen.orientation]].
        /// </description>
        AutoRotation = 5,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        Landscape = 3
    }

    /// <summary>
    /// Filtering mode for textures. Corresponds to the settings in a [[wiki:Textures|texture inspector]].
    /// </summary>
    /// <description>
    /// SA: [[Texture.filterMode]], [[wiki:Textures|texture assets]].
    /// </description>
    /// <dw-legacy-code>
    /// //This script changes the filter mode of your Texture you attach when you press the space key in Play Mode. It switches between Point, Bilinear and Trilinear filter modes.
    /// //Attach this script to a GameObject
    /// //Click on the GameObject and attach a Texture to the My Texture field in the Inspector.
    /// //Apply the Texture to GameObjects (click and drag the Texture onto a GameObject in Editor mode) in your Scene to see it change modes in-game.
    /// using UnityEngine;
    /// public class Example : MonoBehaviour
    /// {
    ///     //Remember to assign a Texture in the Inspector window to ensure this works
    ///     public Texture m_MyTexture;
    ///     void Update()
    ///     {
    ///         //Press the space key to switch between Filter Modes
    ///         if (Input.GetKeyDown(KeyCode.Space))
    ///         {
    ///             //Switch the Texture's Filter Mode
    ///             m_MyTexture.filterMode = SwitchFilterModes();
    ///             //Output the current Filter Mode to the console
    ///             Debug.Log("Filter mode : " + m_MyTexture.filterMode);
    ///         }
    ///     }
    ///     //Switch between Filter Modes when the user clicks the Button
    ///     FilterMode SwitchFilterModes()
    ///     {
    ///         //Switch the Filter Mode of the Texture when user clicks the Button
    ///         switch (m_MyTexture.filterMode)
    ///         {
    ///             //If the FilterMode is currently Bilinear, switch it to Point on the Button click
    ///             case FilterMode.Bilinear:
    ///                 m_MyTexture.filterMode = FilterMode.Point;
    ///                 break;
    ///             //If the FilterMode is currently Point, switch it to Trilinear on the Button click
    ///             case FilterMode.Point:
    ///                 m_MyTexture.filterMode = FilterMode.Trilinear;
    ///                 break;
    ///             //If the FilterMode is currently Trilinear, switch it to Bilinear on the Button click
    ///             case FilterMode.Trilinear:
    ///                 m_MyTexture.filterMode = FilterMode.Bilinear;
    ///                 break;
    ///         }
    ///         //Return the new Texture FilterMode
    ///         return m_MyTexture.filterMode;
    ///     }
    /// }
    /// </dw-legacy-code>
    public enum FilterMode
    {
        /// <summary>
        /// Point filtering - texture pixels become blocky up close.
        /// </summary>
        /// <description>
        /// SA: [[Texture.filterMode]], [[wiki:Textures|texture assets]].
        /// </description>
        /// <dw-legacy-code>
        /// //This script changes the filter mode of your Texture you attach when you press the space key in Play Mode. It switches between Point, Bilinear and Trilinear filter modes.
        /// //Attach this script to a GameObject
        /// //Click on the GameObject and attach a Texture to the My Texture field in the Inspector.
        /// //Apply the Texture to GameObjects (click and drag the Texture onto a GameObject in Editor mode) in your Scene to see it change modes in-game.
        /// using UnityEngine;
        /// public class Example : MonoBehaviour
        /// {
        ///     //Remember to assign a Texture in the Inspector window to ensure this works
        ///     public Texture m_MyTexture;
        ///     void Update()
        ///     {
        ///         //Press the space key to switch between Filter Modes
        ///         if (Input.GetKeyDown(KeyCode.Space))
        ///         {
        ///             //Switch the Texture's Filter Mode
        ///             m_MyTexture.filterMode = SwitchFilterModes();
        ///             //Output the current Filter Mode to the console
        ///             Debug.Log("Filter mode : " + m_MyTexture.filterMode);
        ///         }
        ///     }
        ///     //Switch between Filter Modes when the user clicks the Button
        ///     FilterMode SwitchFilterModes()
        ///     {
        ///         //Switch the Filter Mode of the Texture when user clicks the Button
        ///         switch (m_MyTexture.filterMode)
        ///         {
        ///             //If the FilterMode is currently Bilinear, switch it to Point on the Button click
        ///             case FilterMode.Bilinear:
        ///                 m_MyTexture.filterMode = FilterMode.Point;
        ///                 break;
        ///             //If the FilterMode is currently Point, switch it to Trilinear on the Button click
        ///             case FilterMode.Point:
        ///                 m_MyTexture.filterMode = FilterMode.Trilinear;
        ///                 break;
        ///             //If the FilterMode is currently Trilinear, switch it to Bilinear on the Button click
        ///             case FilterMode.Trilinear:
        ///                 m_MyTexture.filterMode = FilterMode.Bilinear;
        ///                 break;
        ///         }
        ///         //Return the new Texture FilterMode
        ///         return m_MyTexture.filterMode;
        ///     }
        /// }
        /// </dw-legacy-code>
        Point = 0,
        /// <summary>
        /// Bilinear filtering - texture samples are averaged.
        /// </summary>
        /// <description>
        /// SA: [[Texture.filterMode]], [[wiki:Textures|texture assets]].
        /// </description>
        /// <dw-legacy-code>
        /// //This script changes the filter mode of your Texture you attach when you press the space key in Play Mode. It switches between Point, Bilinear and Trilinear filter modes.
        /// //Attach this script to a GameObject
        /// //Click on the GameObject and attach a Texture to the My Texture field in the Inspector.
        /// //Apply the Texture to GameObjects (click and drag the Texture onto a GameObject in Editor mode) in your Scene to see it change modes in-game.
        /// using UnityEngine;
        /// public class Example : MonoBehaviour
        /// {
        ///     //Remember to assign a Texture in the Inspector window to ensure this works
        ///     public Texture m_MyTexture;
        ///     void Update()
        ///     {
        ///         //Press the space key to switch between Filter Modes
        ///         if (Input.GetKeyDown(KeyCode.Space))
        ///         {
        ///             //Switch the Texture's Filter Mode
        ///             m_MyTexture.filterMode = SwitchFilterModes();
        ///             //Output the current Filter Mode to the console
        ///             Debug.Log("Filter mode : " + m_MyTexture.filterMode);
        ///         }
        ///     }
        ///     //Switch between Filter Modes when the user clicks the Button
        ///     FilterMode SwitchFilterModes()
        ///     {
        ///         //Switch the Filter Mode of the Texture when user clicks the Button
        ///         switch (m_MyTexture.filterMode)
        ///         {
        ///             //If the FilterMode is currently Bilinear, switch it to Point on the Button click
        ///             case FilterMode.Bilinear:
        ///                 m_MyTexture.filterMode = FilterMode.Point;
        ///                 break;
        ///             //If the FilterMode is currently Point, switch it to Trilinear on the Button click
        ///             case FilterMode.Point:
        ///                 m_MyTexture.filterMode = FilterMode.Trilinear;
        ///                 break;
        ///             //If the FilterMode is currently Trilinear, switch it to Bilinear on the Button click
        ///             case FilterMode.Trilinear:
        ///                 m_MyTexture.filterMode = FilterMode.Bilinear;
        ///                 break;
        ///         }
        ///         //Return the new Texture FilterMode
        ///         return m_MyTexture.filterMode;
        ///     }
        /// }
        /// </dw-legacy-code>
        Bilinear = 1,
        /// <summary>
        /// Trilinear filtering - texture samples are averaged and also blended between mipmap levels.
        /// </summary>
        /// <description>
        /// For textures without mipmaps, this setting is the same as Bilinear.
        /// SA: [[Texture.filterMode]], [[wiki:Textures|texture assets]].
        /// </description>
        /// <dw-legacy-code>
        /// //This script changes the filter mode of your Texture you attach when you press the space key in Play Mode. It switches between Point, Bilinear and Trilinear filter modes.
        /// //Attach this script to a GameObject
        /// //Click on the GameObject and attach a Texture to the My Texture field in the Inspector.
        /// //Apply the Texture to GameObjects (click and drag the Texture onto a GameObject in Editor mode) in your Scene to see it change modes in-game.
        /// using UnityEngine;
        /// public class Example : MonoBehaviour
        /// {
        ///     //Remember to assign a Texture in the Inspector window to ensure this works
        ///     public Texture m_MyTexture;
        ///     void Update()
        ///     {
        ///         //Press the space key to switch between Filter Modes
        ///         if (Input.GetKeyDown(KeyCode.Space))
        ///         {
        ///             //Switch the Texture's Filter Mode
        ///             m_MyTexture.filterMode = SwitchFilterModes();
        ///             //Output the current Filter Mode to the console
        ///             Debug.Log("Filter mode : " + m_MyTexture.filterMode);
        ///         }
        ///     }
        ///     //Switch between Filter Modes when the user clicks the Button
        ///     FilterMode SwitchFilterModes()
        ///     {
        ///         //Switch the Filter Mode of the Texture when user clicks the Button
        ///         switch (m_MyTexture.filterMode)
        ///         {
        ///             //If the FilterMode is currently Bilinear, switch it to Point on the Button click
        ///             case FilterMode.Bilinear:
        ///                 m_MyTexture.filterMode = FilterMode.Point;
        ///                 break;
        ///             //If the FilterMode is currently Point, switch it to Trilinear on the Button click
        ///             case FilterMode.Point:
        ///                 m_MyTexture.filterMode = FilterMode.Trilinear;
        ///                 break;
        ///             //If the FilterMode is currently Trilinear, switch it to Bilinear on the Button click
        ///             case FilterMode.Trilinear:
        ///                 m_MyTexture.filterMode = FilterMode.Bilinear;
        ///                 break;
        ///         }
        ///         //Return the new Texture FilterMode
        ///         return m_MyTexture.filterMode;
        ///     }
        /// }
        /// </dw-legacy-code>
        Trilinear = 2,
    }

    /// <summary>
    /// Wrap mode for textures.
    /// </summary>
    /// <description>
    /// Corresponds to the settings in a [[wiki:class-TextureImporter|texture inspector]].
    /// Wrap mode determines how texture is sampled when texture coordinates are outside of
    /// the typical 0..1 range. For example, Repeat makes the texture tile, whereas
    public enum TextureWrapMode
    {
        /// <summary>
        /// Tiles the texture, creating a repeating pattern.
        /// </summary>
        /// <description>
        /// When UVs are outside of the 0...1 range, the integer part will be ignored, thus creating a repeating pattern.
        /// SA: [[Texture.wrapMode]], [[wiki:class-TextureImporter|texture assets]].
        /// </description>
        Repeat = 0,
        /// <summary>
        /// Clamps the texture to the last pixel at the edge.
        /// </summary>
        /// <description>
        /// This is useful for preventing wrapping artifacts when mapping an image onto an object and you don't want the texture to tile.
        /// UV coordinates will be clamped to the range 0...1. When UVs are larger than 1 or smaller than 0, the last pixel at the border will be used.
        /// This mode is called "clamp to edge" in graphics APIs like Vulkan, Metal and OpenGL.
        /// SA: [[Texture.wrapMode]], [[wiki:class-TextureImporter|texture assets]].
        /// </description>
        Clamp = 1,
        /// <summary>
        /// Tiles the texture, creating a repeating pattern by mirroring it at every integer boundary.
        /// </summary>
        /// <description>
        /// SA: [[Texture.wrapMode]], [[wiki:class-TextureImporter|texture assets]].
        /// </description>
        Mirror = 2,
        /// <summary>
        /// Mirrors the texture once, then clamps to edge pixels.
        /// </summary>
        /// <description>
        /// This effectively mirrors the texture around zero UV coordinates, and repeats edge pixel values when outside of [-1..1] range.
        /// This mode is called "mirror and clamp to edge" in graphics APIs like Vulkan, Metal and OpenGL. This feature is not always supported when using OpenGL ES and Vulkan graphics APIs, specifically on ARM and Qualcomm GPUs platforms. Check [[SystemInfo.supportsTextureWrapMirrorOnce]] to figure out whether the system is capable..
        /// SA: [[Texture.wrapMode]], [[wiki:class-TextureImporter|texture assets]], [[SystemInfo.supportsTextureWrapMirrorOnce]].
        /// </description>
        MirrorOnce = 3,
    }

    /// <summary>
    /// NPOT [[Texture2D|textures]] support.
    /// </summary>
    public enum NPOTSupport
    {
        /// <summary>
        /// NPOT textures are not supported. Will be upscaled/padded at loading time.
        /// </summary>
        [Obsolete("NPOTSupport.None does not happen on any platforms")]
        None = 0,
        /// <summary>
        /// Limited NPOT support: no mip-maps and clamp [[TextureWrapMode|wrap mode]] will be forced.
        /// </summary>
        /// <description>
        /// If NPOT Texture do have mip-maps it will be upscaled/padded at loading time.
        /// </description>
        Restricted = 1,
        /// <summary>
        /// Full NPOT support.
        /// </summary>
        Full = 2
    }

    /// <summary>
    /// Format used when creating textures from scripts.
    /// </summary>
    /// <dw-legacy-code>
    /// using UnityEngine;
    /// public class Example : MonoBehaviour
    /// {
    ///     void Start()
    ///     {
    ///         // Create a new alpha-only texture and assign it
    ///         // to the renderer's material
    ///         Texture2D texture = new Texture2D(128, 128, TextureFormat.Alpha8, false);
    ///         GetComponent<Renderer>().material.mainTexture = texture;
    ///     }
    /// }
    /// </dw-legacy-code>
    /// <description>
    /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
    /// SA: [[Texture2D]], [[wiki:Textures|texture assets]].
    /// </description>
    public enum TextureFormat
    {
        /// <summary>
        /// Alpha-only texture format.
        /// </summary>
        /// <description>
        /// SA: [[Texture2D]], [[wiki:Textures|texture assets]].
        /// </description>
        Alpha8      = 1,
        /// <summary>
        /// A 16 bits/pixel texture format. Texture stores color with an alpha channel.
        /// </summary>
        /// <description>
        /// SA: [[Texture2D]], [[wiki:Textures|texture assets]].
        /// </description>
        ARGB4444    = 2,
        /// <summary>
        /// Color texture format, 8-bits per channel.
        /// </summary>
        /// <description>
        /// Each of RGB color channels is stored as an 8-bit value in [0..1] range. In memory, the channel data is ordered this way: R, G, B bytes one after another.
        /// Note that there are almost no GPUs that support this format natively, so at texture load time it is converted into an RGBA32 format. RGB24 is thus only useful for some game build size savings.
        /// SA: [[Texture2D]], [[wiki:Textures|texture assets]].
        /// </description>
        RGB24       = 3,
        /// <summary>
        /// Color with alpha texture format, 8-bits per channel.
        /// </summary>
        /// <description>
        /// Each of RGBA color channels is stored as an 8-bit value in [0..1] range. In memory, the channel data is ordered this way: R, G, B, A bytes one after another.
        /// SA: [[Texture2D]], [[wiki:Textures|texture assets]].
        /// </description>
        RGBA32      = 4,
        /// <summary>
        /// Color with alpha texture format, 8-bits per channel.
        /// </summary>
        /// <description>
        /// Each of RGBA color channels is stored as an 8-bit value in [0..1] range. In memory, the channel data is ordered this way: A, R, G, B bytes one after another.
        /// Note that RGBA32 format might be slightly more efficient as the data layout in memory more
        /// closely matches what the graphics APIs expect.
        /// SA: [[Texture2D]], [[wiki:Textures|texture assets]].
        /// </description>
        ARGB32      = 5,
        /// <summary>
        /// A 16 bit color texture format.
        /// </summary>
        /// <description>
        /// SA: [[Texture2D]], [[wiki:Textures|texture assets]].
        /// </description>
        RGB565      = 7,
        /// <summary>
        /// Single channel (R) texture format, 16 bit integer.
        /// </summary>
        /// <description>
        /// Currently, this texture format is only useful for runtime or native code plugins as there is no support for texture importing for this format.
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        R16         = 9,
        /// <summary>
        /// Compressed color texture format.
        /// </summary>
        /// <description>
        /// DXT1 format compresses textures to 4 bits per pixel, and is widely supported on PC, console and Windows Phone platforms.
        /// It is a good format to compress most of RGB textures. For RGBA (with alpha) textures, use DXT5.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        DXT1        = 10,
        /// <summary>
        /// Compressed color with alpha channel texture format.
        /// </summary>
        /// <description>
        /// DXT5 format compresses textures to 8 bits per pixel, and is widely supported on PC, console and Windows Phone platforms.
        /// It is a good format to compress most of RGBA textures. For RGB (without alpha) textures, DXT1 is better. When targeting DX11-class hardware (modern PC, PS4, XboxOne), using BC7 might be useful, since compression quality is often better.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        DXT5        = 12,
        /// <summary>
        /// Color and alpha  texture format, 4 bit per channel.
        /// </summary>
        RGBA4444    = 13,
        /// <summary>
        /// Color with alpha texture format, 8-bits per channel.
        /// </summary>
        /// <description>
        /// BGRA32 format is used by [[WebCamTexture]] on some platforms. Each of RGBA color channels is stored as an 8-bit value in [0..1] range. In memory, the channel data is ordered this way: B, G, R, A bytes one after another.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        BGRA32      = 14,

        /// <summary>
        /// Scalar (R)  texture format, 16 bit floating point.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// </description>
        RHalf       = 15,
        /// <summary>
        /// Two color (RG)  texture format, 16 bit floating point per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// </description>
        RGHalf      = 16,
        /// <summary>
        /// RGB color and alpha texture format, 16 bit floating point per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// </description>
        RGBAHalf    = 17,
        /// <summary>
        /// Scalar (R) texture format, 32 bit floating point.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// </description>
        RFloat      = 18,
        /// <summary>
        /// Two color (RG)  texture format, 32 bit floating point per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// </description>
        RGFloat     = 19,
        /// <summary>
        /// RGB color and alpha texture format,  32-bit floats per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// </description>
        RGBAFloat   = 20,

        /// <summary>
        /// A format that uses the YUV color space and is often used for video encoding or playback.
        /// </summary>
        /// <description>
        /// Currently, this texture format is only useful for native code plugins as there is no support for texture importing or pixel access for this format.  YUY2 is implemented for Direct3D 9, Direct3D 11, and Xbox One.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        YUY2        = 21,
        /// <summary>
        /// RGB HDR format, with 9 bit mantissa per channel and a 5 bit shared exponent.
        /// </summary>
        /// <description>
        /// Three partial-precision floating-point numbers encoded into a single 32-bit value all sharing the same 5-bit exponent (variant of s10e5, which is sign bit, 10-bit mantissa, and 5-bit biased(15) exponent). There is no sign bit, and there is a shared 5-bit biased(15) exponent and a 9-bit mantissa for each channel. RGB9e5Float is implemented for Direct3D 11, Direct3D 12, Xbox One, Playstation 4, OpenGL 3.0+, metal and Vulkan. The format is used for Precomputed Realtime GI textures on supported platforms.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        RGB9e5Float = 22,

        /// <summary>
        /// Compressed one channel (R) texture format.
        /// </summary>
        /// <description>
        /// BC4 format compresses textures to 4 bits per pixel, keeping only the red color channel. It is widely supported on PC and console platforms.
        /// It is a good format to compress single-channel textures like heightfields or masks. For two channel textures, see BC5.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        BC4         = 26,
        /// <summary>
        /// Compressed two-channel (RG) texture format.
        /// </summary>
        /// <description>
        /// BC5 format compresses textures to 8 bits per pixel, keeping only the red and green color channels. It is widely supported on PC and console platforms.
        /// It is a good format to compress two-channel textures, e.g. as a compression format for tangent space normal maps or velocity fields. For one channel textures, see BC4.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        BC5         = 27,
        /// <summary>
        /// HDR compressed color texture format.
        /// </summary>
        /// <description>
        /// BC6H format compresses RGB HDR textures to 8 bits per pixel, and is supported on DX11-class PC hardware, as well as PS4 and XboxOne.
        /// It is a good format for compressing floating point texture data (skyboxes, reflection probes, lightmaps, emissive textures), e.g.
        /// textures that uncompressed would be in RGBAHalf or RGBAFloat formats. Note that BC6H does not retain the alpha channel; it only stores RGB color channels.
        /// When loading BC6H textures on a platform that does not support it, the texture will be decompressed into RGBAHalf format (64 bits per pixel) at load time. Note that BC7 is not available on Mac when using OpenGL.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        BC6H        = 24,
        /// <summary>
        /// High quality compressed color texture format.
        /// </summary>
        /// <description>
        /// BC7 format compresses textures to 8 bits per pixel, and is supported on DX11-class PC hardware, as well as PS4 and XboxOne.
        /// Generally it produces better quality than the more widely available DXT5 format, however it requires a modern GPU, and texture compression during import time is often slower too. Note that BC7 is not available on Mac when using OpenGL.
        /// When loading BC7 textures on a platform that does not support it, the texture will be decompressed into RGBA32 format (32 bits per pixel) at load time.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        BC7         = 25,

    #if ENABLE_CRUNCH_TEXTURE_COMPRESSION
        /// <summary>
    /// Compressed color texture format with Crunch compression for smaller storage sizes.
    /// </summary>
    /// <description>
    /// The DXT1Crunched format is similar to the DXT1 format but with additional JPEG-like lossy compression for storage size reduction. Textures are transcoded into the DXT1 format at load time.
    /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
    /// </description>
    DXT1Crunched = 28,
        /// <summary>
        /// Compressed color with alpha channel texture format with Crunch compression for smaller storage sizes.
        /// </summary>
        /// <description>
        /// The DXT5Crunched format is similar to the DXT5 format but with additional JPEG-like lossy compression for storage size reduction. Textures are transcoded into the DXT5 format at load time.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        DXT5Crunched = 29,
    #endif

        /// <summary>
        /// PowerVR (iOS) 2 bits/pixel compressed color texture format.
        /// </summary>
        /// <description>
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        PVRTC_RGB2  = 30,
        /// <summary>
        /// PowerVR (iOS) 2 bits/pixel compressed with alpha channel texture format.
        /// </summary>
        /// <description>
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        PVRTC_RGBA2 = 31,
        /// <summary>
        /// PowerVR (iOS) 4 bits/pixel compressed color texture format.
        /// </summary>
        /// <description>
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        PVRTC_RGB4  = 32,
        /// <summary>
        /// PowerVR (iOS) 4 bits/pixel compressed with alpha channel texture format.
        /// </summary>
        /// <description>
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        PVRTC_RGBA4 = 33,
        /// <summary>
        /// ETC (GLES2.0) 4 bits/pixel compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ETC_RGB4    = 34,

#if UNITY_EDITOR
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member TextureFormat.ATC_RGB4 has been deprecated. Use ETC_RGB4 instead (UnityUpgradable) -> ETC_RGB4", true)]
        ATC_RGB4 = -127,

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member TextureFormat.ATC_RGBA8 has been deprecated. Use ETC2_RGBA8 instead (UnityUpgradable) -> ETC2_RGBA8", true)]
        ATC_RGBA8 = -127,
#endif

        /// <summary>
        /// ETC2 / EAC (GL ES 3.0) 4 bits/pixel compressed unsigned single-channel texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        EAC_R = 41,
        /// <summary>
        /// ETC2 / EAC (GL ES 3.0) 4 bits/pixel compressed signed single-channel texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        EAC_R_SIGNED = 42,
        /// <summary>
        /// ETC2 / EAC (GL ES 3.0) 8 bits/pixel compressed unsigned dual-channel (RG) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        EAC_RG = 43,
        /// <summary>
        /// ETC2 / EAC (GL ES 3.0) 8 bits/pixel compressed signed dual-channel (RG) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        EAC_RG_SIGNED = 44,
        /// <summary>
        /// ETC2 (GL ES 3.0) 4 bits/pixel compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ETC2_RGB = 45,
        /// <summary>
        /// ETC2 (GL ES 3.0) 4 bits/pixel RGB+1-bit alpha texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ETC2_RGBA1 = 46,
        /// <summary>
        /// ETC2 (GL ES 3.0) 8 bits/pixel compressed RGBA texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ETC2_RGBA8 = 47,

        /// <summary>
        /// ASTC (4x4 pixel block in 128 bits) compressed RGB(A) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_4x4 = 48,
        /// <summary>
        /// ASTC (5x5 pixel block in 128 bits) compressed RGB(A) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_5x5 = 49,
        /// <summary>
        /// ASTC (6x6 pixel block in 128 bits) compressed RGB(A) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_6x6 = 50,
        /// <summary>
        /// ASTC (8x8 pixel block in 128 bits) compressed RGB(A) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_8x8 = 51,
        /// <summary>
        /// ASTC (10x10 pixel block in 128 bits) compressed RGB(A) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_10x10 = 52,
        /// <summary>
        /// ASTC (12x12 pixel block in 128 bits) compressed RGB(A) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_12x12 = 53,

        // Nintendo 3DS
        /// <summary>
        /// ETC 4 bits/pixel compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        [System.Obsolete("Nintendo 3DS is no longer supported.")]
        ETC_RGB4_3DS = 60,
        /// <summary>
        /// ETC 4 bits/pixel RGB + 4 bits/pixel Alpha compressed texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        [System.Obsolete("Nintendo 3DS is no longer supported.")]
        ETC_RGBA8_3DS = 61,

        /// <summary>
        /// Two color (RG) texture format, 8-bits per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// </description>
        RG16 = 62,
        /// <summary>
        /// Single channel (R) texture format, 8 bit integer.
        /// </summary>
        /// <description>
        /// Currently, this texture format is only useful for runtime or native code plugins as there is no support for texture importing for this format.
        /// Note that not all graphics cards support all texture formats, use [[SystemInfo.SupportsTextureFormat]] to check.
        /// </description>
        R8 = 63,

    #if ENABLE_CRUNCH_TEXTURE_COMPRESSION
        /// <summary>
    /// Compressed color texture format with Crunch compression for smaller storage sizes.
    /// </summary>
    /// <description>
    /// The ETC_RGB4Crunched format is similar to the ETC_RGB4 format but with additional JPEG-like lossy compression for storage size reduction. Textures are transcoded into the ETC_RGB4 format at load time.
    /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
    /// </description>
    ETC_RGB4Crunched = 64,
        /// <summary>
        /// Compressed color with alpha channel texture format using Crunch compression for smaller storage sizes.
        /// </summary>
        /// <description>
        /// The ETC2_RGBA8Crunched format is similar to the ETC2_RGBA8 format but with additional JPEG-like lossy compression for storage size reduction. Textures are transcoded into the ETC2_RGBA8 format at load time.
        /// SA: [[Texture2D.format]], [[wiki:Textures|texture assets]].
        /// </description>
        ETC2_RGBA8Crunched = 65,
    #endif

        /// <summary>
        /// ASTC (4x4 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_HDR_4x4 = 66,
        /// <summary>
        /// ASTC (5x5 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_HDR_5x5 = 67,
        /// <summary>
        /// ASTC (6x6 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_HDR_6x6 = 68,
        /// <summary>
        /// ASTC (8x8 pixel block in 128 bits) compressed RGB(A) texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_HDR_8x8 = 69,
        /// <summary>
        /// ASTC (10x10 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_HDR_10x10 = 70,
        /// <summary>
        /// ASTC (12x12 pixel block in 128 bits) compressed RGB(A) HDR texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_HDR_12x12 = 71,

        // please note that obsolete attrs are currently disabled because we have tests that checks for "no warnings"
        // yet at the same time there are packages that reference old ASTC enums.
        // hence the only way is to go to trunk -> fix packages -> obsolete

        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGB_4x4 has been deprecated. Use ASTC_4x4 instead (UnityUpgradable) -> ASTC_4x4")]
        /// <summary>
        /// ASTC (4x4 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGB_4x4 = 48,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGB_5x5 has been deprecated. Use ASTC_5x5 instead (UnityUpgradable) -> ASTC_5x5")]
        /// <summary>
        /// ASTC (5x5 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGB_5x5 = 49,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGB_6x6 has been deprecated. Use ASTC_6x6 instead (UnityUpgradable) -> ASTC_6x6")]
        /// <summary>
        /// ASTC (6x6 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGB_6x6 = 50,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGB_8x8 has been deprecated. Use ASTC_8x8 instead (UnityUpgradable) -> ASTC_8x8")]
        /// <summary>
        /// ASTC (8x8 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGB_8x8 = 51,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGB_10x10 has been deprecated. Use ASTC_10x10 instead (UnityUpgradable) -> ASTC_10x10")]
        /// <summary>
        /// ASTC (10x10 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGB_10x10 = 52,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGB_12x12 has been deprecated. Use ASTC_12x12 instead (UnityUpgradable) -> ASTC_12x12")]
        /// <summary>
        /// ASTC (12x12 pixel block in 128 bits) compressed RGB texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGB_12x12 = 53,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGBA_4x4 has been deprecated. Use ASTC_4x4 instead (UnityUpgradable) -> ASTC_4x4")]
        /// <summary>
        /// ASTC (4x4 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGBA_4x4 = 54,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGBA_5x5 has been deprecated. Use ASTC_5x5 instead (UnityUpgradable) -> ASTC_5x5")]
        /// <summary>
        /// ASTC (5x5 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGBA_5x5 = 55,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGBA_6x6 has been deprecated. Use ASTC_6x6 instead (UnityUpgradable) -> ASTC_6x6")]
        /// <summary>
        /// ASTC (6x6 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGBA_6x6 = 56,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGBA_8x8 has been deprecated. Use ASTC_8x8 instead (UnityUpgradable) -> ASTC_8x8")]
        /// <summary>
        /// ASTC (8x8 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGBA_8x8 = 57,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGBA_10x10 has been deprecated. Use ASTC_10x10 instead (UnityUpgradable) -> ASTC_10x10")]
        /// <summary>
        /// ASTC (10x10 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGBA_10x10 = 58,
        // [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        // [System.Obsolete("Enum member TextureFormat.ASTC_RGBA_12x12 has been deprecated. Use ASTC_12x12 instead (UnityUpgradable) -> ASTC_12x12")]
        /// <summary>
        /// ASTC (12x12 pixel block in 128 bits) compressed RGBA texture format.
        /// </summary>
        /// <description>
        /// SA: [[TextureImporter.textureFormat]].
        /// </description>
        ASTC_RGBA_12x12 = 59,

#if UNITY_EDITOR
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member TextureFormat.PVRTC_2BPP_RGB has been deprecated. Use PVRTC_RGB2 instead (UnityUpgradable) -> PVRTC_RGB2", true)]
        PVRTC_2BPP_RGB = -127,

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member TextureFormat.PVRTC_2BPP_RGBA has been deprecated. Use PVRTC_RGBA2 instead (UnityUpgradable) -> PVRTC_RGBA2", true)]
        PVRTC_2BPP_RGBA = -127,

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member TextureFormat.PVRTC_4BPP_RGB has been deprecated. Use PVRTC_RGB4 instead (UnityUpgradable) -> PVRTC_RGB4", true)]
        PVRTC_4BPP_RGB = -127,

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member TextureFormat.PVRTC_4BPP_RGBA has been deprecated. Use PVRTC_RGBA4 instead (UnityUpgradable) -> PVRTC_RGBA4", true)]
        PVRTC_4BPP_RGBA = -127,
#endif
    }

    /// <summary>
    /// [[Cubemap]] face.
    /// </summary>
    /// <description>
    /// Used by [[Cubemap.GetPixel]] and [[Cubemap.SetPixel]].
    /// </description>
    public enum CubemapFace
    {
        /// <summary>
        /// Cubemap face is unknown or unspecified.
        /// </summary>
        Unknown   = -1,
        /// <summary>
        /// Right facing side (+x).
        /// </summary>
        PositiveX = 0,
        /// <summary>
        /// Left facing side (-x).
        /// </summary>
        NegativeX = 1,
        /// <summary>
        /// Upwards facing side (+y).
        /// </summary>
        PositiveY = 2,
        /// <summary>
        /// Downward facing side (-y).
        /// </summary>
        NegativeY = 3,
        /// <summary>
        /// Forward facing side (+z).
        /// </summary>
        PositiveZ = 4,
        /// <summary>
        /// Backward facing side (-z).
        /// </summary>
        NegativeZ = 5
    }

    /// <summary>
    /// Format of a [[RenderTexture]].
    /// </summary>
    /// <description>
    /// Note that a particular render texture format might not be supported by the current platform or GPU. Use [[SystemInfo.SupportsRenderTextureFormat]] to check before using.
    /// SA: [[RenderTexture.format]], [[RenderTexture]] class.
    /// </description>
    public enum RenderTextureFormat
    {
        /// <summary>
        /// Color render texture format, 8 bits per channel.
        /// </summary>
        /// <description>
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class.
        /// </description>
        ARGB32 = 0,
        /// <summary>
        /// A depth render texture format.
        /// </summary>
        /// <description>
        /// Depth format is used to render high precision "depth" value into a render texture. Which format
        /// is actually used depends on the platform. On OpenGL it is the native "depth component" format
        /// (usually 24 or 16 bits), on Direct3D9 it is the 32 bit floating point ("R32F") format. When writing
        /// shaders that use or render into a depth texture, care must be taken to ensure that they work both
        /// on OpenGL and on Direct3D, see [[wiki:SL-DepthTextures|depth textures documentation]].
        /// Note that not all graphics cards support depth textures. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        Depth = 1,
        /// <summary>
        /// Color render texture format, 16 bit floating point per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support floating point render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        ARGBHalf = 2,
        /// <summary>
        /// A native shadowmap render texture format.
        /// </summary>
        /// <description>
        /// This represents a format for which the GPU can automatically do shadowmap comparisons
        /// for. Unity uses this format internally for shadows, when supported. Note that some
        /// platforms or GPUs do not support Shadowmap format, in which case shadows
        /// end up using [[RenderTextureFormat.Depth]] format.
        /// Note that not all graphics cards support shadowmaps. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        Shadowmap = 3,
        /// <summary>
        /// Color render texture format.
        /// </summary>
        /// <description>
        /// 5 bits for Red channel, 6 bits for Green channel, 5 bits for Blue channel
        /// Note that not all graphics cards support 16 bit textures. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RGB565 = 4,
        /// <summary>
        /// Color render texture format, 4 bit per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support 16 bit textures. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        ARGB4444 = 5,
        /// <summary>
        /// Color render texture format, 1 bit for Alpha channel, 5 bits for Red, Green and Blue channels.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support 16 bit textures. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        ARGB1555 = 6,
        /// <summary>
        /// Default color render texture format: will be chosen accordingly to Frame Buffer format and Platform.
        /// </summary>
        /// <description>
        /// Typically this is ARGB32 format.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class.
        /// </description>
        Default = 7,
        /// <summary>
        /// Color render texture format. 10 bits for colors, 2 bits for alpha.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support this format. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        ARGB2101010 = 8,
        /// <summary>
        /// Default HDR color render texture format: will be chosen accordingly to Frame Buffer format and Platform.
        /// </summary>
        /// <description>
        /// Typically this is ARGBHalf format.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        DefaultHDR = 9,
        /// <summary>
        /// Four color render texture format, 16 bits per channel, fixed point, unsigned normalized.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support this format. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        ARGB64 = 10,
        /// <summary>
        /// Color render texture format, 32 bit floating point per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support floating point render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        ARGBFloat = 11,
        /// <summary>
        /// Two color (RG) render texture format, 32 bit floating point per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support floating point render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RGFloat = 12,
        /// <summary>
        /// Two color (RG) render texture format, 16 bit floating point per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support floating point render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RGHalf = 13,
        /// <summary>
        /// Scalar (R) render texture format, 32 bit floating point.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support floating point render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RFloat = 14,
        /// <summary>
        /// Scalar (R) render texture format, 16 bit floating point.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support floating point render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RHalf = 15,
        /// <summary>
        /// Single channel (R) render texture format, 8 bit integer.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support integer render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        R8 = 16,
        /// <summary>
        /// Four channel (ARGB) render texture format, 32 bit signed integer per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support integer render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        ARGBInt = 17,
        /// <summary>
        /// Two channel (RG) render texture format, 32 bit signed integer per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support integer render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RGInt = 18,
        /// <summary>
        /// Scalar (R) render texture format, 32 bit signed integer.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support integer render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RInt = 19,
        /// <summary>
        /// Color render texture format, 8 bits per channel.
        /// </summary>
        BGRA32 = 20,
        // kRTFormatVideo = 21,
        /// <summary>
        /// Color render texture format. R and G channels are 11 bit floating point, B channel is 10 bit floating point.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support this format. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RGB111110Float = 22,
        /// <summary>
        /// Two color (RG) render texture format, 16 bits per channel, fixed point, unsigned normalized.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support this format. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class.
        /// </description>
        RG32 = 23,
        /// <summary>
        /// Four channel (RGBA) render texture format, 16 bit unsigned integer per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support integer render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RGBAUShort = 24,
        /// <summary>
        /// Two channel (RG) render texture format, 8 bits per channel.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support integer render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        RG16 = 25,
        /// <summary>
        /// Color render texture format, 10 bit per channel, extended range.
        /// </summary>
        /// <description>
        /// The components are linearly encoded and their values range from -0.752941 to 1.25098.
        /// Note that not all graphics cards support this format. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        BGRA10101010_XR = 26,
        /// <summary>
        /// Color render texture format, 10 bit per channel, extended range.
        /// </summary>
        /// <description>
        /// The components are linearly encoded and their values range from -0.752941 to 1.25098.
        /// Note that not all graphics cards support this format. Use [[SystemInfo.SupportsRenderTextureFormat]]
        /// to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        BGR101010_XR = 27,
        /// <summary>
        /// Single channel (R) render texture format, 16 bit integer.
        /// </summary>
        /// <description>
        /// Note that not all graphics cards support integer render textures.
        /// Use [[SystemInfo.SupportsRenderTextureFormat]] to check for support.
        /// SA: [[RenderTexture.format]], [[RenderTexture]] class, [[SystemInfo.SupportsRenderTextureFormat]].
        /// </description>
        R16 = 28,
    }

    /// <summary>
    /// This enum describes how the [[RenderTexture]] is used as a VR eye texture. Instead of using the values of this enum manually, use the value returned by [[XR.XRSettings.eyeTextureDesc|eyeTextureDesc]] or other VR functions returning a [[RenderTextureDescriptor]].
    /// </summary>
    public enum VRTextureUsage
    {
        /// <summary>
        /// The RenderTexture is not a VR eye texture. No special rendering behavior will occur.
        /// </summary>
        None,
        /// <summary>
        /// This texture corresponds to a single eye on a stereoscopic display.
        /// </summary>
        OneEye,
        /// <summary>
        /// This texture corresponds to two eyes on a stereoscopic display. This will be taken into account when using [[Graphics.Blit]] and other rendering functions.
        /// </summary>
        TwoEyes,
        /// <summary>
        /// The texture used by an external XR provider.  The provider is responsible for defining the texture's layout and use.
        /// </summary>
        DeviceSpecific
    }

    // keep this in sync with the RenderTextureFlags enum in RenderTexture.h
    /// <summary>
    /// Set of flags that control the state of a newly-created RenderTexture.
    /// </summary>
    [Flags]
    public enum RenderTextureCreationFlags
    {
        /// <summary>
        /// Set this flag to allocate mipmaps in the RenderTexture. See [[RenderTexture.useMipMap]] for more details.
        /// </summary>
        MipMap = 1 << 0,
        /// <summary>
        /// Determines whether or not mipmaps are automatically generated when the RenderTexture is modified.
        /// This flag is set by default, and has no effect if the [[RenderTextureCreationFlags.MipMap]] flag is not also set.
        /// See [[RenderTexture.autoGenerateMips]] for more details.
        /// </summary>
        AutoGenerateMips = 1 << 1,
        /// <summary>
        /// When this flag is set, reads and writes to this texture are converted to SRGB color space. See [[RenderTexture.sRGB]] for more details.
        /// </summary>
        SRGB = 1 << 2,
        /// <summary>
        /// Set this flag when the Texture is to be used as a VR eye texture. This flag is cleared by default. This flag is set on a RenderTextureDesc when it is returned from GetDefaultVREyeTextureDesc or other VR functions returning a RenderTextureDesc.
        /// </summary>
        EyeTexture = 1 << 3,
        /// <summary>
        /// Set this flag to enable random access writes to the RenderTexture from shaders.
        /// Normally, pixel shaders only operate on pixels they are given. Compute shaders cannot write to textures without this flag. Random write enables shaders to write to arbitrary locations on a RenderTexture.  See [[RenderTexture.enableRandomWrite]] for more details, including supported platforms.
        /// </summary>
        EnableRandomWrite = 1 << 4,
        /// <summary>
        /// This flag is always set internally when a RenderTexture is created from script. It has no effect when set manually from script code.
        /// </summary>
        CreatedFromScript = 1 << 5, // always set by script.
        // SampleOnlyDepth = 1 << 6, // this is only used internally.
        /// <summary>
        /// Clear this flag when a RenderTexture is a VR eye texture and the device does not automatically flip the texture when being displayed. This is platform specific and
        /// It is set by default. This flag is only cleared when part of a RenderTextureDesc that is returned from GetDefaultVREyeTextureDesc or other VR functions that return a RenderTextureDesc. Currently, only Hololens eye textures need to clear this flag.
        /// </summary>
        AllowVerticalFlip = 1 << 7,
        /// <summary>
        /// When this flag is set, the engine will not automatically resolve the color surface.
        /// </summary>
        NoResolvedColorSurface = 1 << 8,
        /// <summary>
        /// Set this flag to mark this RenderTexture for Dynamic Resolution should the target platform/graphics API support Dynamic Resolution. See [[ScalabeBufferManager]] for more details.
        /// </summary>
        DynamicallyScalable = 1 << 10,
        /// <summary>
        /// Setting this flag causes the RenderTexture to be bound as a multisampled texture in a shader. The flag prevents the RenderTexture from being resolved by default when [[RenderTexture.antiAliasing]] is greater than 1.
        /// </summary>
        BindMS = 1 << 11,
    }

    /// <summary>
    /// Color space conversion mode of a [[RenderTexture]].
    /// </summary>
    /// <description>
    /// When using Gamma [[wiki:LinearLighting|color space]], no conversions are done of any kind, and this setting is not used.
    /// When Linear color space is used, then by default non-HDR render textures are considered
    /// to contain sRGB data (i.e. "regular colors"), and fragment shaders are considered to output linear color values.
    /// So by default the fragment shader color value is converted into sRGB when rendering into a texture;
    /// and when sampling the texture in the shader the sRGB colors are converted into linear values.
    /// This is the sRGB read-write mode; and the Default mode matches that when linear color space is used.
    /// When this mode is set on a render texture, [[RenderTexture.sRGB]] will return true.
    /// However, if your render texture will contain non-color data (normals, velocities, other custom values)
    /// then you don't want Linear<->sRGB conversions to happen. This is the Linear read-write mode.
    /// When this mode is set on a render texture, [[RenderTexture.sRGB]] will return false.
    /// Note that some [[RenderTextureFormat|render texture formats]] are always considered to contain "linear" data
    /// and no sRGB conversions are ever performed on them, no matter what is the read-write setting. This is true
    /// for all "HDR" (floating point) formats, and other formats like Depth or Shadowmap.
    /// SA: [[wiki:LinearLighting|Linear Color Space]], [[RenderTexture.sRGB]], [[PlayerSettings.colorSpace]], [[GL.sRGBWrite]].
    /// </description>
    public enum RenderTextureReadWrite
    {
        /// <summary>
        /// Default color space conversion based on project settings.
        /// </summary>
        /// <description>
        /// This value picks sRGB (when Linear color space is used) or Linear
        /// (when gamma color space is used) based on [[PlayerSettings.colorSpace]].
        /// See [[RenderTextureReadWrite]] overview page for more details.
        /// </description>
        Default = 0,
        /// <summary>
        /// Render texture contains linear (non-color) data; don't perform color conversions on it.
        /// </summary>
        /// <description>
        /// See [[RenderTextureReadWrite]] overview page for more details.
        /// </description>
        Linear = 1,
        /// <summary>
        /// Render texture contains sRGB (color) data, perform Linear<->sRGB conversions on it.
        /// </summary>
        /// <description>
        /// See [[RenderTextureReadWrite]] overview page for more details.
        /// </description>
        sRGB = 2
    }

    /// <summary>
    /// Flags enumeration of the render texture memoryless modes.
    /// </summary>
    /// <description>
    /// SA. [[RenderTexture.memorylessMode]], [[RenderTexture]].
    /// </description>
    [Flags]
    public enum RenderTextureMemoryless
    {
        /// <summary>
        /// The render texture is not memoryless.
        /// </summary>
        /// <description>
        /// SA. [[RenderTexture.memorylessMode]], [[RenderTexture]].
        /// </description>
        None = 0,
        /// <summary>
        /// Render texture color pixels are memoryless when [[RenderTexture.antiAliasing]] is set to 1.
        /// </summary>
        /// <description>
        /// Note that memoryless render textures are only supported on iOS/tvOS 10.0+ Metal and Vulkan. Render textures are read/write protected and stored in CPU or GPU memory on other platforms.
        /// SA. [[RenderTexture.memorylessMode]], [[RenderTexture]].
        /// </description>
        Color = 1,
        /// <summary>
        /// Render texture depth pixels are memoryless.
        /// </summary>
        /// <description>
        /// Note that memoryless render textures are only supported on iOS/tvOS 10.0+ Metal and Vulkan. Render textures are read/write protected and stored in CPU or GPU memory on other platforms.
        /// SA. [[RenderTexture.memorylessMode]], [[RenderTexture]].
        /// </description>
        Depth = 2,
        /// <summary>
        /// Render texture color pixels are memoryless when [[RenderTexture.antiAliasing]] is set to 2, 4 or 8.
        /// </summary>
        /// <description>
        /// Note that memoryless render textures are only supported on iOS/tvOS 10.0+ Metal. Render textures are read/write protected and stored in CPU or GPU memory on other platforms.
        /// SA. [[RenderTexture.memorylessMode]], [[RenderTexture]].
        /// </description>
        MSAA = 4
    }

    namespace Experimental
    {
        namespace Rendering
        {
            // keep this in sync with the TextureCreationFlags enum in Texture.h
            /// <summary>
            /// There is currently no documentation for this api.
            /// </summary>
            [Flags]
            public enum TextureCreationFlags
            {
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                None = 0,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                MipChain = 1 << 0,
                //DontInitializePixels = 1 << 2, // this is only used internally.
                //DontDestroyTexture = 1 << 3, // this is only used internally.
                //DontCreateSharedTextureData = 1 << 4, // this is only used internally.
                //APIShareable = 1 << 5, // this is only used internally.
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                Crunch = 1 << 6,
            }

            // Keep in sync with FormatUsage in Runtime/Graphics/Format.h
            /// <summary>
            /// There is currently no documentation for this api.
            /// </summary>
            public enum FormatUsage
            {
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                Sample = 0,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                Linear = 1,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                Sparse = 2,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                Render = 4,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                Blend = 5,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                GetPixels = 6,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                SetPixels = 7,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                ReadPixels = 8,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                LoadStore = 9,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                MSAA2x = 10,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                MSAA4x = 11,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                MSAA8x = 12,
            }

            // Keep in sync with DefaultFormat in Runtime/Graphics/Format.h
            /// <summary>
            ///                 Use a default format to create either Textures or RenderTextures from scripts based on platform specific capability.
            /// </summary>
            /// <description>
            /// Each graphics card may not support all usages across formats. Use [[SystemInfo.IsFormatSupported]] to check which usages the graphics card supports.
            /// SA: [[Texture2D]], [[wiki:Textures|texture assets]].
            /// </description>
            public enum DefaultFormat
            {
                /// <summary>
                /// Represents the default platform-specific LDR format. If the project uses the linear rendering mode, the actual format is sRGB. If the project uses the gamma rendering mode, the actual format is UNorm.
                /// </summary>
                LDR,
                /// <summary>
                /// Represents the default platform specific HDR format.
                /// </summary>
                HDR,
            }

            // Keep in sync with GraphicsFormat in Runtime/Graphics/Format.h
            /// <summary>
            /// There is currently no documentation for this api.
            /// </summary>
            public enum GraphicsFormat
            {
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                None = 0,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8_SRGB = 1,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8_SRGB = 2,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8_SRGB = 3,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8A8_SRGB = 4,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8_UNorm = 5,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8_UNorm = 6,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8_UNorm = 7,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8A8_UNorm = 8,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8_SNorm = 9,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8_SNorm = 10,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8_SNorm = 11,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8A8_SNorm = 12,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8_UInt = 13,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8_UInt = 14,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8_UInt = 15,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8A8_UInt = 16,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8_SInt = 17,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8_SInt = 18,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8_SInt = 19,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R8G8B8A8_SInt = 20,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16_UNorm = 21,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16_UNorm = 22,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16_UNorm = 23,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16A16_UNorm = 24,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16_SNorm = 25,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16_SNorm = 26,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16_SNorm = 27,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16A16_SNorm = 28,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16_UInt = 29,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16_UInt = 30,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16_UInt = 31,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16A16_UInt = 32,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16_SInt = 33,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16_SInt = 34,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16_SInt = 35,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16A16_SInt = 36,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32_UInt = 37,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32_UInt = 38,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32B32_UInt = 39,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32B32A32_UInt = 40,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32_SInt = 41,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32_SInt = 42,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32B32_SInt = 43,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32B32A32_SInt = 44,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16_SFloat = 45,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16_SFloat = 46,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16_SFloat = 47,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R16G16B16A16_SFloat = 48,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32_SFloat = 49,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32_SFloat = 50,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32B32_SFloat = 51,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R32G32B32A32_SFloat = 52,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8_SRGB = 56,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8A8_SRGB = 57,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8_UNorm = 58,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8A8_UNorm = 59,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8_SNorm = 60,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8A8_SNorm = 61,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8_UInt = 62,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8A8_UInt = 63,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8_SInt = 64,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B8G8R8A8_SInt = 65,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R4G4B4A4_UNormPack16 = 66,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B4G4R4A4_UNormPack16 = 67,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R5G6B5_UNormPack16 = 68,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B5G6R5_UNormPack16 = 69,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R5G5B5A1_UNormPack16 = 70,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B5G5R5A1_UNormPack16 = 71,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A1R5G5B5_UNormPack16 = 72,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                E5B9G9R9_UFloatPack32 = 73,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                B10G11R11_UFloatPack32 = 74,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A2B10G10R10_UNormPack32 = 75,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A2B10G10R10_UIntPack32 = 76,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A2B10G10R10_SIntPack32 = 77,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A2R10G10B10_UNormPack32 = 78,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A2R10G10B10_UIntPack32 = 79,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A2R10G10B10_SIntPack32 = 80,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A2R10G10B10_XRSRGBPack32 = 81,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A2R10G10B10_XRUNormPack32 = 82,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R10G10B10_XRSRGBPack32 = 83,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R10G10B10_XRUNormPack32 = 84,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A10R10G10B10_XRSRGBPack32 = 85,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                A10R10G10B10_XRUNormPack32 = 86,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
                [Obsolete("Enum member GraphicsFormat.RGB_DXT1_SRGB has been deprecated. Use GraphicsFormat.RGBA_DXT1_SRGB instead (UnityUpgradable) -> RGBA_DXT1_SRGB", true)]
                RGB_DXT1_SRGB = 96,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_DXT1_SRGB = 96,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
                [Obsolete("Enum member GraphicsFormat.RGB_DXT1_UNorm has been deprecated. Use GraphicsFormat.RGBA_DXT1_UNorm instead (UnityUpgradable) -> RGBA_DXT1_UNorm", true)]
                RGB_DXT1_UNorm = 97,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_DXT1_UNorm = 97,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_DXT3_SRGB = 98,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_DXT3_UNorm = 99,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_DXT5_SRGB = 100,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_DXT5_UNorm = 101,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R_BC4_UNorm = 102,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R_BC4_SNorm = 103,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RG_BC5_UNorm = 104,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RG_BC5_SNorm = 105,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_BC6H_UFloat = 106,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_BC6H_SFloat = 107,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_BC7_SRGB = 108,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_BC7_UNorm = 109,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_PVRTC_2Bpp_SRGB = 110,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_PVRTC_2Bpp_UNorm = 111,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_PVRTC_4Bpp_SRGB = 112,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_PVRTC_4Bpp_UNorm = 113,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_PVRTC_2Bpp_SRGB = 114,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_PVRTC_2Bpp_UNorm = 115,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_PVRTC_4Bpp_SRGB = 116,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_PVRTC_4Bpp_UNorm = 117,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_ETC_UNorm = 118,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_ETC2_SRGB = 119,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_ETC2_UNorm = 120,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_A1_ETC2_SRGB = 121,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGB_A1_ETC2_UNorm = 122,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ETC2_SRGB = 123,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ETC2_UNorm = 124,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R_EAC_UNorm = 125,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                R_EAC_SNorm = 126,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RG_EAC_UNorm = 127,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RG_EAC_SNorm = 128,

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC4X4_SRGB = 129,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC4X4_UNorm = 130,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC5X5_SRGB = 131,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC5X5_UNorm = 132,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC6X6_SRGB = 133,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC6X6_UNorm = 134,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC8X8_SRGB = 135,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC8X8_UNorm = 136,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC10X10_SRGB = 137,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC10X10_UNorm = 138,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC12X12_SRGB = 139,
                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                RGBA_ASTC12X12_UNorm = 140
            }
        }//namespace Rendering
    }//namespace Experimental

    /// <summary>
    /// Lightmap (and lighting) configuration mode, controls how lightmaps interact with lighting and what kind of information they store.
    /// </summary>
    [Flags]
    public enum LightmapsMode
    {
        /// <summary>
        /// Light intensity (no directional information), encoded as 1 lightmap.
        /// </summary>
        NonDirectional = 0,
        /// <summary>
        /// Directional information for direct light is combined with directional information for indirect light, encoded as 2 lightmaps.
        /// </summary>
        CombinedDirectional = 1,
#if UNITY_EDITOR
        /// <summary>
        /// Directional information for direct light is stored separately from directional information for indirect light, encoded as 4 lightmaps.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member LightmapsMode.SeparateDirectional has been removed. Use CombinedDirectional instead (UnityUpgradable) -> CombinedDirectional", true)]
        SeparateDirectional = 2,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member LightmapsMode.Single has been removed. Use NonDirectional instead (UnityUpgradable) -> NonDirectional", true)]
        Single = 0,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member LightmapsMode.Dual has been removed. Use CombinedDirectional instead (UnityUpgradable) -> CombinedDirectional", true)]
        Dual = 1,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Obsolete("Enum member LightmapsMode.Directional has been removed. Use CombinedDirectional instead (UnityUpgradable) -> CombinedDirectional", true)]
        Directional = 2,
#endif
    }

    // Match MaterialGlobalIlluminationFlags on C++ side
    /// <summary>
    /// How the material interacts with lightmaps and lightprobes.
    /// </summary>
    /// <description>
    /// SA: [[Material.globalIlluminationFlags]].
    /// </description>
    [Flags]
    public enum MaterialGlobalIlluminationFlags
    {
        /// <summary>
        /// The emissive lighting does not affect Global Illumination at all.
        /// </summary>
        None = 0,
        /// <summary>
        /// The emissive lighting will affect realtime Global Illumination. It emits lighting into realtime lightmaps and realtime lightprobes.
        /// </summary>
        /// <description>
        /// The flags are mutually exclusive so if you are using RealtimeEmissive lighting, you must remove the EmissiveIsBlack flag from the material as shown in the example below.
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///     Material material;
        ///     void Start()
        ///     {
        ///         // Remove the EmissiveIsBlack flag from material:
        ///         MaterialGlobalIlluminationFlags flags = material.globalIlluminationFlags;
        ///         flags &= ~MaterialGlobalIlluminationFlags.EmissiveIsBlack;
        ///         material.globalIlluminationFlags = flags;
        ///     }
        /// }
        /// </dw-legacy-code>
        /// <description>
        /// SA: [[Material.globalIlluminationFlags.EmissiveIsBlack]].
        /// </description>
        RealtimeEmissive = 1 << 0,
        /// <summary>
        /// The emissive lighting affects baked Global Illumination. It emits lighting into baked lightmaps and baked lightprobes.
        /// </summary>
        BakedEmissive = 1 << 1,
        /// <summary>
        /// The emissive lighting is guaranteed to be black. This lets the lightmapping system know that it doesn't have to extract emissive lighting information from the material and can simply assume it is completely black.
        /// </summary>
        /// <description>
        /// SA: [[Material.globalIlluminationFlags.RealtimeEmissive]].
        /// </description>
        EmissiveIsBlack = 1 << 2,
        /// <summary>
        /// Helper Mask to be used to query the enum only based on whether realtime GI or baked GI is set, ignoring all other bits.
        /// </summary>
        AnyEmissive = RealtimeEmissive | BakedEmissive
    }

    // Match the enums from LightProbeProxyVolume class on C++ side
    /// <summary>
    /// The Light Probe Proxy Volume component offers the possibility to use higher resolution lighting for large non-static GameObjects.
    /// </summary>
    /// <description>
    /// By default, a probe-lit Renderer receives lighting from a single Light Probe that is interpolated from the surrounding Light Probes in the Scene. Because of this, GameObjects have constant ambient lighting regardless of their position on the surface. The light has have a rotational gradient because it's using spherical harmonics, but it lacks a spatial gradient. This is more noticeable on larger GameObjects and Particle Systems. The lighting across the GameObject matches the lighting at the anchor point, and if the GameObject straddles a lighting gradient, parts of the GameObject will look incorrect.
    /// This component will generate a 3D grid of interpolated Light Probes inside a bounding volume. The resolution of the grid can be user-specified. The spherical harmonics coefficients of the interpolated Light Probes are updated into 3D textures, which are sampled at render time to compute the contribution to the diffuse ambient lighting. This adds a spatial gradient to probe-lit GameObjects.
    /// SA: [[wiki:LightProbes|Light Probes]].
    /// </description>
    public partial class LightProbeProxyVolume
    {
        /// <summary>
        /// The resolution mode for generating a grid of interpolated Light Probes.
        /// </summary>
        /// <description>
        /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]].
        /// </description>
        public enum ResolutionMode
        {
            /// <summary>
            /// The automatic mode uses a number of interpolated Light Probes per unit area, and uses the bounding volume size to compute the resolution. The final resolution value is a power of 2.
            /// </summary>
            Automatic = 0,
            /// <summary>
            /// The custom mode allows you to specify the 3D grid resolution.
            /// </summary>
            Custom = 1
        }

        /// <summary>
        /// The bounding box mode for generating a grid of interpolated Light Probes.
        /// </summary>
        /// <description>
        /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]].
        /// </description>
        public enum BoundingBoxMode
        {
            /// <summary>
            /// The bounding box encloses the current Renderer and all the relevant Renderers down the hierarchy, in local space.
            /// </summary>
            /// <description>
            /// Only Renderers that have the __Light Probes__ property set to __Use Proxy Volume__ are taken into account. The interpolated Light Probe positions are generated in the local-space of the Renderer inside the resulting bounding box. If a Renderer component isn’t attached to the GameObject, a default bounding box is generated.
            /// </description>
            AutomaticLocal = 0,
            /// <summary>
            /// The bounding box encloses the current Renderer and all the relevant Renderers down the hierarchy, in world space.
            /// </summary>
            /// <description>
            /// Only Renderers that have the __Light Probes__ property set to __Use Proxy Volume__ are taken into account. The bounding box is world-space aligned.
            /// </description>
            AutomaticWorld = 1,
            /// <summary>
            /// A custom local-space bounding box is used. The user is able to edit the bounding box.
            /// </summary>
            Custom = 2
        }

        /// <summary>
        /// The mode in which the interpolated Light Probe positions are generated.
        /// </summary>
        /// <description>
        /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]].
        /// </description>
        public enum ProbePositionMode
        {
            /// <summary>
            /// Divide the volume in cells based on resolution, and generate interpolated Light Probes positions in the corner/edge of the cells.
            /// </summary>
            CellCorner = 0,
            /// <summary>
            /// Divide the volume in cells based on resolution, and generate interpolated Light Probe positions in the center of the cells.
            /// </summary>
            CellCenter = 1
        }

        /// <summary>
        /// An enum describing the way a Light Probe Proxy Volume refreshes in the Player.
        /// </summary>
        /// <description>
        /// SA: [[LightProbeProxyVolume|Light Probe Proxy Volume]].
        /// </description>
        public enum RefreshMode
        {
            /// <summary>
            /// Automatically detects updates in Light Probes and triggers an update of the Light Probe volume.
            /// </summary>
            /// <description>
            /// SA: [[LightProbeProxyVolume|Light Probe Proxy Volume]], [[LightProbes|Light Probes]].
            /// </description>
            Automatic = 0,
            /// <summary>
            /// Causes Unity to update the Light Probe Proxy Volume every frame.
            /// </summary>
            /// <description>
            /// Note that updating a Light Probe Proxy Volume every frame may be resource-intensive. The performance impact depends on the resolution of the interpolated Light Probe grid. The Light Probe interpolation is multi-threaded.
            /// SA: [[LightProbeProxyVolume|Light Probe Proxy Volume]].
            /// </description>
            EveryFrame = 1,
            /// <summary>
            /// Use this option to indicate that the Light Probe Proxy Volume is never to be automatically updated by Unity.
            /// </summary>
            /// <description>
            /// This is useful if you wish to completely control the Light Probe Proxy Volume refresh behavior via scripting.
            /// SA: [[LightProbeProxyVolume|Light Probe Proxy Volume]], [[LightProbeProxyVolume.Update]].
            /// </description>
            ViaScripting = 2,
        }

        /// <summary>
        /// An enum describing the Quality option used by the Light Probe Proxy Volume component.
        /// </summary>
        /// <description>
        /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]],  [[SphericalHarmonicsL2|Spherical Harmonics(SH)]].
        /// </description>
        public enum QualityMode
        {
            /// <summary>
            /// This option will use only two SH coefficients bands: L0 and L1. The coefficients are sampled from the Light Probe Proxy Volume 3D Texture. Using this option might increase the draw call batch sizes by not having to change the L2 coefficients per Renderer.
            /// </summary>
            /// <description>
            /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]],  [[SphericalHarmonicsL2|Spherical Harmonics(SH)]].
            /// </description>
            Low = 0,
            /// <summary>
            /// This option will use L0 and L1 SH coefficients from the Light Probe Proxy Volume 3D Texture. The L2 coefficients are constant per Renderer. By having to provide the L2 coefficients, draw call batches might be broken.
            /// </summary>
            /// <description>
            /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]],  [[SphericalHarmonicsL2|Spherical Harmonics(SH)]].
            /// </description>
            Normal = 1,
        }
    }

    /// <summary>
    /// Specify the source of a Custom Render Texture initialization.
    /// </summary>
    /// <description>
    /// SA: [[CustomRenderTexture]].
    /// </description>
    public enum CustomRenderTextureInitializationSource
    {
        /// <summary>
        /// Custom Render Texture is initialized by a Texture multiplied by a Color.
        /// </summary>
        /// <description>
        /// SA: [[CustomRenderTexture]].
        /// </description>
        TextureAndColor = 0,
        /// <summary>
        /// Custom Render Texture is initalized with a Material.
        /// </summary>
        /// <description>
        /// SA: [[CustomRenderTexture]].
        /// </description>
        Material = 1,
    }

    /// <summary>
    /// Frequency of update or initialization of a Custom Render Texture.
    /// </summary>
    /// <description>
    /// SA: [[CustomRenderTexture]].
    /// </description>
    public enum CustomRenderTextureUpdateMode
    {
        /// <summary>
        /// Initialization/Update will occur once at load time and then can be triggered again by script.
        /// </summary>
        /// <description>
        /// SA: [[CustomRenderTexture]].
        /// </description>
        OnLoad = 0,
        /// <summary>
        /// Initialization/Update will occur at every frame.
        /// </summary>
        /// <description>
        /// SA: [[CustomRenderTexture]].
        /// </description>
        Realtime = 1,
        /// <summary>
        /// Initialization/Update will only occur when triggered by the script.
        /// </summary>
        /// <description>
        /// SA: [[CustomRenderTexture]].
        /// </description>
        OnDemand = 2,
    }

    /// <summary>
    /// Space in which coordinates are provided for Update Zones.
    /// </summary>
    /// <description>
    /// SA: [[CustomRenderTexture]].
    /// </description>
    public enum CustomRenderTextureUpdateZoneSpace
    {
        /// <summary>
        /// Coordinates are normalized. (0, 0) is top left and (1, 1) is bottom right.
        /// </summary>
        /// <description>
        /// SA: [[CustomRenderTexture]].
        /// </description>
        Normalized = 0,
        /// <summary>
        /// Coordinates are expressed in pixels. (0, 0) is top left (width, height) is bottom right.
        /// </summary>
        /// <description>
        /// SA: [[CustomRenderTexture]].
        /// </description>
        Pixel = 1,
    }

    /// <summary>
    /// The type of motion vectors that should be generated.
    /// </summary>
    /// <description>
    /// SA: [[DepthTextureMode.MotionVectors]], [[SkinnedMeshRenderer.skinnedMotionVectors]], [[PassType.MotionVectors]], [[SystemInfo.supportsMotionVectors]].
    /// </description>
    public enum MotionVectorGenerationMode
    {
        /// <summary>
        /// Use only camera movement to track motion.
        /// </summary>
        Camera = 0,
        /// <summary>
        /// Use a specific pass (if required) to track motion.
        /// </summary>
        Object = 1,
        /// <summary>
        /// Do not track motion. Motion vectors will be 0.
        /// </summary>
        ForceNoMotion = 2,
    }

    /// <summary>
    /// Choose how textures are applied to Lines and Trails.
    /// </summary>
    public enum LineTextureMode
    {
        /// <summary>
        /// Map the texture once along the entire length of the line.
        /// </summary>
        Stretch = 0,
        /// <summary>
        /// Repeat the texture along the line, based on its length in world units. To set the tiling rate, use [[Material.SetTextureScale]].
        /// </summary>
        Tile = 1,
        /// <summary>
        /// Map the texture once along the entire length of the line, assuming all vertices are evenly spaced.
        /// </summary>
        DistributePerSegment = 2,
        /// <summary>
        /// Repeat the texture along the line, repeating at a rate of once per line segment. To adjust the tiling rate, use [[Material.SetTextureScale]].
        /// </summary>
        RepeatPerSegment = 3,
    }

    /// <summary>
    /// Control the direction lines face, when using the [[LineRenderer]] or [[TrailRenderer]].
    /// </summary>
    public enum LineAlignment
    {
        /// <summary>
        /// Lines face the camera.
        /// </summary>
        View = 0,
        /// <summary>
        /// Lines face the direction of the Transform Component.
        /// </summary>
        [System.Obsolete("Enum member Local has been deprecated. Use TransformZ instead (UnityUpgradable) -> TransformZ", false)]
        Local = 1,
        /// <summary>
        /// Lines face the Z axis of the Transform Component.
        /// </summary>
        TransformZ = 1,
    }
} // namespace UnityEngine


namespace UnityEngine.Rendering
{
    // Match IndexFormat on C++ side
    /// <summary>
    /// Format of the mesh index buffer data.
    /// </summary>
    /// <description>
    /// Index buffer can either be 16 bit (supports up to 65535 vertices in a mesh), or 32 bit (supports up to 4 billion vertices).
    /// Default index format is 16 bit, since that takes less memory and bandwidth.
    /// Note that GPU support for 32 bit indices is not guaranteed on all platforms; for example Android devices
    /// with Mali-400 GPU do not support them. When using 32 bit indices on such a platform, a warning message will be
    /// logged and mesh will not render.
    /// SA: [[Mesh.indexFormat]], [[ModelImporter.indexFormat]].
    /// </description>
    public enum IndexFormat
    {
        /// <summary>
        /// 16 bit mesh index buffer format.
        /// </summary>
        /// <description>
        /// This format supports meshes with up to 65535 vertices.
        /// SA: [[Mesh.indexFormat]], [[ModelImporter.indexFormat]].
        /// </description>
        UInt16 = 0,
        /// <summary>
        /// 32 bit mesh index buffer format.
        /// </summary>
        /// <description>
        /// This format supports meshes with up to 4 billion vertices.
        /// Note that GPU support for 32 bit indices is not guaranteed on all platforms; for example Android devices
        /// with Mali-400 GPU do not support them. When using 32 bit indices on such a platform, a warning message will be
        /// logged and mesh will not render.
        /// SA: [[Mesh.indexFormat]], [[ModelImporter.indexFormat]].
        /// </description>
        UInt32 = 1,
    }

    //Keep in sync with ShaderChannel in GfxDeviceTypes.h
    /// <summary>
    /// A list of data channels that describe a vertex in a mesh.
    /// </summary>
    [MovedFrom("UnityEngine.Experimental.Rendering")]
    public enum VertexAttribute
    {
        /// <summary>
        /// The position channel. The common format is [[Vector3]].
        /// </summary>
        Position = 0, // Vertex (vector3)
        /// <summary>
        /// The normal channel. The common format is [[Vector3]].
        /// </summary>
        Normal,       // Normal (vector3)
        /// <summary>
        /// The tangent channel. The common format is [[Vector4]].
        /// </summary>
        Tangent,      // Tangent (vector4)
        /// <summary>
        /// The color channel.
        /// </summary>
        Color,        // Vertex color
        /// <summary>
        /// The primary UV channel. The common format is [[Vector2]].
        /// </summary>
        TexCoord0,    // Texcoord 0
        /// <summary>
        /// Additional UV channel. The common format is [[Vector2]].
        /// </summary>
        TexCoord1,    // Texcoord 1
        /// <summary>
        /// Additional UV channel. The common format is [[Vector2]].
        /// </summary>
        TexCoord2,    // Texcoord 2
        /// <summary>
        /// Additional UV channel. The common format is [[Vector2]].
        /// </summary>
        TexCoord3,    // Texcoord 3
        /// <summary>
        /// Additional UV channel. The common format is [[Vector2]].
        /// </summary>
        TexCoord4,    // Texcoord 4
        /// <summary>
        /// Additional UV channel. The common format is [[Vector2]].
        /// </summary>
        TexCoord5,    // Texcoord 5
        /// <summary>
        /// Additional UV channel. The common format is [[Vector2]].
        /// </summary>
        TexCoord6,    // Texcoord 6
        /// <summary>
        /// Additional UV channel. The common format is [[Vector2]].
        /// </summary>
        TexCoord7,    // Texcoord 7
        /// <summary>
        /// Blend weights for skinned meshes. The common format is [[Float]].
        /// </summary>
        BlendWeight,
        /// <summary>
        /// Blend indices for skinned meshes. The common format is [[Int]].
        /// </summary>
        BlendIndices,
    }

    // Match Camera::OpaqueSortMode on C++ side
    /// <summary>
    /// Opaque object sorting mode of a [[Camera]].
    /// </summary>
    /// <description>
    /// Opaque objects are sorted by various criteria (sorting layers, shader queues,
    /// materials, distance, lightmaps etc.) to maximize both the CPU efficiency (reduce number
    /// of state changes and improve draw call batching), and to maximize GPU efficiency
    /// (many GPUs prefer rough front-to-back rendering order for faster rejection of invisible
    /// surfaces).
    /// By default, opaque objects are grouped in rough front-to-back buckets, on the GPUs
    /// where doing that is beneficial. There are GPUs where doing this distance based
    /// sorting is not really helpful (most notably, PowerVR/Apple GPUs), and so on these GPUs
    /// the distance based sorting is not done by default.
    /// The [[Camera.opaqueSortMode]] property lets you override this default behavior.
    /// For example, you might want to never do distance-based sorting for opaque objects,
    /// if you know you need much more CPU performance than GPU performance.
    /// SA: [[Camera.opaqueSortMode]].
    /// </description>
    public enum OpaqueSortMode
    {
        /// <summary>
        /// Default opaque sorting mode.
        /// </summary>
        /// <description>
        /// SA: [[Camera.opaqueSortMode]].
        /// </description>
        Default = 0,
        /// <summary>
        /// Do rough front-to-back sorting of opaque objects.
        /// </summary>
        /// <description>
        /// SA: [[Camera.opaqueSortMode]].
        /// </description>
        FrontToBack = 1,
        /// <summary>
        /// Do not sort opaque objects by distance.
        /// </summary>
        /// <description>
        /// SA: [[Camera.opaqueSortMode]].
        /// </description>
        NoDistanceSort = 2
    }

    // Match RenderLoopEnums.h on C++ side
    /// <summary>
    /// Determine in which order objects are renderered.
    /// </summary>
    /// <description>
    /// This way for example transparent objects are rendered after opaque objects, and so on.
    /// SA: [[Material.renderQueue]], [[Shader.renderQueue]], [[wiki:SL-SubShaderTags|subshader tags]].
    /// </description>
    public enum RenderQueue
    {
        /// <summary>
        /// This render queue is rendered before any others.
        /// </summary>
        /// <description>
        /// You would typically use this for things that really need to be in the background.
        /// SA: [[Material.renderQueue]], [[Shader.renderQueue]], [[wiki:SL-SubShaderTags|subshader tags]].
        /// </description>
        Background = 1000,
        /// <summary>
        /// Opaque geometry uses this queue.
        /// </summary>
        /// <description>
        /// This is used for most objects.
        /// SA: [[Material.renderQueue]], [[Shader.renderQueue]], [[wiki:SL-SubShaderTags|subshader tags]].
        /// </description>
        Geometry = 2000,
        /// <summary>
        /// Alpha tested geometry uses this queue.
        /// </summary>
        /// <description>
        /// It’s a separate queue from Geometry one since it’s more efficient to render alpha-tested objects after all solid ones are drawn.
        /// SA: [[Material.renderQueue]], [[Shader.renderQueue]], [[wiki:SL-SubShaderTags|subshader tags]].
        /// </description>
        AlphaTest = 2450, // we want it to be in the end of geometry queue
        /// <summary>
        /// Last render queue that is considered "opaque".
        /// </summary>
        /// <description>
        /// Render queues in [0, GeometryLast] range are treated as opaque objects (sorted to reduce render state changes), whereas queues in [GeometryLast+1, 5000] range are treated as semitransparent objects (sorted back-to-front).
        /// SA: [[Material.renderQueue]], [[Shader.renderQueue]], [[wiki:SL-SubShaderTags|subshader tags]].
        /// </description>
        GeometryLast = 2500, // last queue that is considered "opaque" by Unity
        /// <summary>
        /// This render queue is rendered after Geometry and AlphaTest, in back-to-front order.
        /// </summary>
        /// <description>
        /// Anything alpha-blended (i.e. shaders that don’t write to depth buffer) should go here (glass, particle effects).
        /// SA: [[Material.renderQueue]], [[Shader.renderQueue]], [[wiki:SL-SubShaderTags|subshader tags]].
        /// </description>
        Transparent = 3000,
        /// <summary>
        /// This render queue is meant for overlay effects.
        /// </summary>
        /// <description>
        /// Anything rendered last should go here (e.g. lens flares).
        /// SA: [[Material.renderQueue]], [[Shader.renderQueue]], [[wiki:SL-SubShaderTags|subshader tags]].
        /// </description>
        Overlay = 4000,
    }

    // Make sure the values are in sync with the native side!
    /// <summary>
    /// This enum describes what should be done on the render target when it is activated (loaded).
    /// </summary>
    /// <description>
    /// When the GPU starts rendering into a render target, this setting specifies the action that should be performed on the existing contents of the surface. Tile-based GPUs may get performance advantage if the load action is Clear or DontCare. The user should avoid using RenderBufferLoadAction.Load whenever possible.
    /// Please note that not all platforms have load/store actions, so this setting might be ignored at runtime. Generally mobile-oriented graphics APIs (OpenGL ES, Metal) take advantage of these settings.
    /// </description>
    public enum RenderBufferLoadAction
    {
        /// <summary>
        /// When this RenderBuffer is activated, preserve the existing contents of it. This setting is expensive on tile-based GPUs and should be avoided whenever possible.
        /// </summary>
        Load = 0,
        /// <summary>
        /// Upon activating the render buffer, clear its contents. Currently only works together with the [[RenderPass]] API.
        /// </summary>
        Clear = 1,
        /// <summary>
        /// When this RenderBuffer is activated, the GPU is instructed not to care about the existing contents of that RenderBuffer. On tile-based GPUs this means that the RenderBuffer contents do not need to be loaded into the tile memory, providing a performance boost.
        /// </summary>
        DontCare = 2,
    }

    // Make sure the values are in sync with the native side!
    /// <summary>
    /// This enum describes what should be done on the render target when the GPU is done rendering into it.
    /// </summary>
    /// <description>
    /// When the GPU is done rendering into a render target, this setting specifies the action that should be performed on the rendering results. Tile-based GPUs may get performance advantage if the store action is DontCare. For example, this setting can be useful if the depth buffer contents are not needed after rendering the frame.
    /// Please note that not all platforms have load/store actions, so this setting might be ignored at runtime. Generally mobile-oriented graphics APIs (OpenGL ES, Metal) take advantage of these settings.
    /// </description>
    public enum RenderBufferStoreAction
    {
        /// <summary>
        /// The RenderBuffer contents need to be stored to RAM. If the surface has MSAA enabled, this stores the non-resolved surface.
        /// </summary>
        Store = 0,
        /// <summary>
        /// Resolve the (MSAA'd) surface. Currently only used with the [[RenderPass]] API.
        /// </summary>
        Resolve = 1, // Resolve the MSAA surface (currently only works with RenderPassSetup)
        /// <summary>
        /// Resolve the (MSAA'd) surface, but also store the multisampled version. Currently only used with the [[RenderPass]] API.
        /// </summary>
        StoreAndResolve = 2, // Resolve the MSAA surface into the resolve target, but also store the MSAA version
        /// <summary>
        /// The contents of the RenderBuffer are not needed and can be discarded. Tile-based GPUs will skip writing out the surface contents altogether, providing performance boost.
        /// </summary>
        DontCare = 3,
    }

    /// <summary>
    /// Blend mode for controlling the blending.
    /// </summary>
    /// <description>
    /// The blend mode is set separately for source and destination, and it controls the blend factor of each component going into the blend equation. It is also possible to set the blend mode for color and alpha components separately. Note: the blend modes are ignored if logical blend operations or advanced OpenGL blend operations are in use.
    /// </description>
    public enum BlendMode
    {
        /// <summary>
        /// Blend factor is  (0, 0, 0, 0).
        /// </summary>
        Zero = 0,
        /// <summary>
        /// Blend factor is (1, 1, 1, 1).
        /// </summary>
        One = 1,
        /// <summary>
        /// Blend factor is (Rd, Gd, Bd, Ad).
        /// </summary>
        DstColor = 2,
        /// <summary>
        /// Blend factor is (Rs, Gs, Bs, As).
        /// </summary>
        SrcColor = 3,
        /// <summary>
        /// Blend factor is (1 - Rd, 1 - Gd, 1 - Bd, 1 - Ad).
        /// </summary>
        OneMinusDstColor = 4,
        /// <summary>
        /// Blend factor is (As, As, As, As).
        /// </summary>
        SrcAlpha = 5,
        /// <summary>
        /// Blend factor is (1 - Rs, 1 - Gs, 1 - Bs, 1 - As).
        /// </summary>
        OneMinusSrcColor = 6,
        /// <summary>
        /// Blend factor is (Ad, Ad, Ad, Ad).
        /// </summary>
        DstAlpha = 7,
        /// <summary>
        /// Blend factor is (1 - Ad, 1 - Ad, 1 - Ad, 1 - Ad).
        /// </summary>
        OneMinusDstAlpha = 8,
        /// <summary>
        /// Blend factor is (f, f, f, 1); where f = min(As, 1 - Ad).
        /// </summary>
        SrcAlphaSaturate = 9,
        /// <summary>
        /// Blend factor is (1 - As, 1 - As, 1 - As, 1 - As).
        /// </summary>
        OneMinusSrcAlpha = 10
    }

    /// <summary>
    /// Blend operation.
    /// </summary>
    /// <description>
    /// The blend operation that is used to combine the pixel shader output with the render target. This can be passed through Material.SetInt() to change the blend operation during runtime.
    /// Note that the logical operations are only supported in Gamma (non-sRGB) colorspace, on DX11.1 hardware running on DirectX 11.1 runtime.
    /// Advanced OpenGL blend operations are supported only on hardware supporting either GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced and may require use of [[GL.RenderTargetBarrier]]. In addition, the shaders that are used with the advanced blend operations must have a UNITY_REQUIRE_ADVANDED_BLEND(mode) declaration in the shader code where mode is one of the blend operations or "all_equations" for supporting all advanced blend operations (see the KHR_blend_equation_advanced spec for other values).
    /// </description>
    public enum BlendOp
    {
        /// <summary>
        /// Add (s + d).
        /// </summary>
        /// <description>
        /// Add source and destination together, with specified blend modes.
        /// </description>
        Add = 0,
        /// <summary>
        /// Subtract.
        /// </summary>
        /// <description>
        /// Subtract the destination from the source.
        /// </description>
        Subtract = 1,
        /// <summary>
        /// Reverse subtract.
        /// </summary>
        /// <description>
        /// Subtract the source from the destination.
        /// </description>
        ReverseSubtract = 2,
        /// <summary>
        /// Min.
        /// </summary>
        /// <description>
        /// Select the smaller value from source and destination.
        /// </description>
        Min = 3,
        /// <summary>
        /// Max.
        /// </summary>
        /// <description>
        /// Select the larger value of in source and destination.
        /// </description>
        Max = 4,
        /// <summary>
        /// Logical Clear (0).
        /// </summary>
        /// <description>
        /// Clears all bits in the target to 0. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalClear = 5,
        /// <summary>
        /// Logical SET (1) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical SET (1) operation, effectively setting all bits in the render target. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalSet = 6,
        /// <summary>
        /// Logical Copy (s) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// This operation copies the source bits to target, effectively disabling blending. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalCopy = 7,
        /// <summary>
        /// Logical inverted Copy (!s) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// This operation inverts the source bits before blitting to target.This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalCopyInverted = 8,
        /// <summary>
        /// Logical No-op (d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical no-op (dest) operation, effectively leaving the render target unchanged. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalNoop = 9,
        /// <summary>
        /// Logical Inverse (!d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// This operation inverts the bits in the destination, ignoring source. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalInvert = 10,
        /// <summary>
        /// Logical AND (s & d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical AND (src & dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalAnd = 11,
        /// <summary>
        /// Logical NAND !(s & d). D3D11.1 only.
        /// </summary>
        /// <description>
        /// Performs logical NAND !(src & dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalNand = 12,
        /// <summary>
        /// Logical OR (s | d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical OR (src | dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalOr = 13,
        /// <summary>
        /// Logical NOR !(s | d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical NOR !(src | dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalNor = 14,
        /// <summary>
        /// Logical XOR (s XOR d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical XOR (src XOR dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalXor = 15,
        /// <summary>
        /// Logical Equivalence !(s XOR d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// This operation performs !(s XOR d). This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalEquivalence = 16,
        /// <summary>
        /// Logical reverse AND (s & !d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical reverse AND (src & !dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalAndReverse = 17,
        /// <summary>
        /// Logical inverted AND (!s & d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical inverted AND (!src & dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalAndInverted = 18,
        /// <summary>
        /// Logical reverse OR (s | !d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical reverse OR (src | !dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalOrReverse = 19,
        /// <summary>
        /// Logical inverted OR (!s | d) (D3D11.1 only).
        /// </summary>
        /// <description>
        /// Performs logical inverted OR (!src | dest) operation. This mode is currently only available with D3D11 renderer on DX11.1 class hardware and DX runtime.
        /// </description>
        LogicalOrInverted = 20,
        /// <summary>
        /// Multiply (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        Multiply = 21,
        /// <summary>
        /// Screen (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        Screen = 22,
        /// <summary>
        /// Overlay (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        Overlay = 23,
        /// <summary>
        /// Darken (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        Darken = 24,
        /// <summary>
        /// Lighten (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        Lighten = 25,
        /// <summary>
        /// Color dodge (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        ColorDodge = 26,
        /// <summary>
        /// Color burn (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        ColorBurn = 27,
        /// <summary>
        /// Hard light (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        HardLight = 28,
        /// <summary>
        /// Soft light (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        SoftLight = 29,
        /// <summary>
        /// Difference (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        Difference = 30,
        /// <summary>
        /// Exclusion (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        Exclusion = 31,
        /// <summary>
        /// HSL Hue (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        HSLHue = 32,
        /// <summary>
        /// HSL saturation (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        HSLSaturation = 33,
        /// <summary>
        /// HSL color (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        HSLColor = 34,
        /// <summary>
        /// HSL luminosity (Advanced OpenGL blending).
        /// </summary>
        /// <description>
        /// As specified in GL_KHR_blend_equation_advanced. This mode is currently available only on OpenGL hardware with GL_KHR_blend_equation_advanced or GL_NV_blend_equation_advanced extension support.
        /// </description>
        HSLLuminosity = 35,
    }

    /// <summary>
    /// Depth or stencil comparison function.
    /// </summary>
    public enum CompareFunction
    {
        /// <summary>
        /// Depth or stencil test is disabled.
        /// </summary>
        Disabled = 0,
        /// <summary>
        /// Never pass depth or stencil test.
        /// </summary>
        Never = 1,
        /// <summary>
        /// Pass depth or stencil test when new value is less than old one.
        /// </summary>
        Less = 2,
        /// <summary>
        /// Pass depth or stencil test when values are equal.
        /// </summary>
        Equal = 3,
        /// <summary>
        /// Pass depth or stencil test when new value is less or equal than old one.
        /// </summary>
        LessEqual = 4,
        /// <summary>
        /// Pass depth or stencil test when new value is greater than old one.
        /// </summary>
        Greater = 5,
        /// <summary>
        /// Pass depth or stencil test when values are different.
        /// </summary>
        NotEqual = 6,
        /// <summary>
        /// Pass depth or stencil test when new value is greater or equal than old one.
        /// </summary>
        GreaterEqual = 7,
        /// <summary>
        /// Always pass depth or stencil test.
        /// </summary>
        Always = 8
    }

    /// <summary>
    /// Backface culling mode.
    /// </summary>
    public enum CullMode
    {
        /// <summary>
        /// Disable culling.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Cull front-facing geometry.
        /// </summary>
        Front = 1,
        /// <summary>
        /// Cull back-facing geometry.
        /// </summary>
        Back = 2
    }

    /// <summary>
    /// Specifies which color components will get written into the target framebuffer.
    /// </summary>
    [Flags]
    public enum ColorWriteMask
    {
        /// <summary>
        /// Write alpha component.
        /// </summary>
        Alpha = 1,
        /// <summary>
        /// Write blue component.
        /// </summary>
        Blue = 2,
        /// <summary>
        /// Write green component.
        /// </summary>
        Green = 4,
        /// <summary>
        /// Write red component.
        /// </summary>
        Red = 8,
        /// <summary>
        /// Write all components (R, G, B and Alpha).
        /// </summary>
        All = 15
    }

    /// <summary>
    /// Specifies the operation that's performed on the stencil buffer when rendering.
    /// </summary>
    public enum StencilOp
    {
        /// <summary>
        /// Keeps the current stencil value.
        /// </summary>
        Keep = 0,
        /// <summary>
        /// Sets the stencil buffer value to zero.
        /// </summary>
        Zero = 1,
        /// <summary>
        /// Replace the stencil buffer value with reference value (specified in the shader).
        /// </summary>
        Replace = 2,
        /// <summary>
        /// Increments the current stencil buffer value. Clamps to the maximum representable unsigned value.
        /// </summary>
        IncrementSaturate = 3,
        /// <summary>
        /// Decrements the current stencil buffer value. Clamps to 0.
        /// </summary>
        DecrementSaturate = 4,
        /// <summary>
        /// Bitwise inverts the current stencil buffer value.
        /// </summary>
        Invert = 5,
        /// <summary>
        /// Increments the current stencil buffer value. Wraps stencil buffer value to zero when incrementing the maximum representable unsigned value.
        /// </summary>
        IncrementWrap = 6,
        /// <summary>
        /// Decrements the current stencil buffer value. Wraps stencil buffer value to the maximum representable unsigned value when decrementing a stencil buffer value of zero.
        /// </summary>
        DecrementWrap = 7
    }

    /// <summary>
    /// Ambient lighting mode.
    /// </summary>
    /// <description>
    /// Unity can provide ambient lighting in several modes, for example directional ambient with separate sky, equator and ground colors, or flat ambient with a single color.
    /// SA: [[RenderSettings.ambientMode]], [[wiki:GlobalIllumination|Lighting Window]].
    /// </description>
    public enum AmbientMode
    {
        /// <summary>
        /// Skybox-based or custom ambient lighting.
        /// </summary>
        /// <description>
        /// Ambient color is calculated from the current skybox, or set manually.
        /// SA: [[RenderSettings.ambientMode]], [[RenderSettings.ambientProbe]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        Skybox = 0,
        /// <summary>
        /// Trilight ambient lighting.
        /// </summary>
        /// <description>
        /// Ambient is defined by three colors: "sky", "equator" and "ground".
        /// SA: [[RenderSettings.ambientSkyColor]], [[RenderSettings.ambientEquatorColor]], [[RenderSettings.ambientGroundColor]], [[RenderSettings.ambientMode]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        Trilight = 1,
        /// <summary>
        /// Flat ambient lighting.
        /// </summary>
        /// <description>
        /// Ambient is defined by a single color.
        /// SA: [[RenderSettings.ambientEquatorColor]], [[RenderSettings.ambientMode]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        Flat = 3,
        /// <summary>
        /// Ambient lighting is defined by a custom cubemap.
        /// </summary>
        Custom = 4
    }

    /// <summary>
    /// Default reflection mode.
    /// </summary>
    /// <description>
    /// Unity can use a custom texture or generate a specular reflection texture from the skybox.
    /// SA: [[RenderSettings.defaultReflectionMode]], [[wiki:GlobalIllumination|Lighting Window]].
    /// </description>
    public enum DefaultReflectionMode
    {
        /// <summary>
        /// Skybox-based default reflection.
        /// </summary>
        /// <description>
        /// Default specular reflection cubemap is calculated from the current skybox.
        /// SA: [[RenderSettings.defaultReflectionMode]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        Skybox = 0,
        /// <summary>
        /// Custom default reflection.
        /// </summary>
        /// <description>
        /// You can specify cubemap that will be used as a default specular reflection.
        /// SA: [[RenderSettings.customReflection]], [[RenderSettings.defaultReflectionMode]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        Custom = 1
    }

    // Keep in sync with LightmapEditorSettings::ReflectionCompression in Editor/Src/LightmapEditorSettings.h
    /// <summary>
    /// Determines how Unity will compress baked reflection cubemap.
    /// </summary>
    public enum ReflectionCubemapCompression
    {
        /// <summary>
        /// Baked Reflection cubemap will be left uncompressed.
        /// </summary>
        Uncompressed = 0,
        /// <summary>
        /// Baked Reflection cubemap will be compressed.
        /// </summary>
        Compressed = 1,
        /// <summary>
        /// Baked Reflection cubemap will be compressed if compression format is suitable.
        /// </summary>
        /// <description>
        /// Some texture compression formats produce bad wrapping artifacts when used on cubemaps, for example PVRTC. On platforms that use these formats, baked reflection cubemaps will be left uncompressed.
        /// </description>
        Auto = 2,
    }

    // Keep in sync with RenderCameraEventType in Runtime/Graphics/CommandBuffer/RenderingEvents.h
    /// <summary>
    /// Defines a place in camera's rendering to attach [[Rendering.CommandBuffer]] objects to.
    /// </summary>
    /// <description>
    /// Unity's rendering loop can be extended by adding so called "command buffers" at various points in camera rendering. For example, you could add some custom geometry to be drawn right after the skybox is drawn.
    /// SA: [[Rendering.CommandBuffer]], [[Rendering.LightEvent]], [[wiki:GraphicsCommandBuffers|command buffers overview]].
    /// </description>
    public enum CameraEvent
    {
        /// <summary>
        /// Before camera's depth texture is generated.
        /// </summary>
        /// <description>
        /// SA: [[wiki:SL-CameraDepthTexture|Using camera's depth textures]].
        /// </description>
        BeforeDepthTexture = 0,
        /// <summary>
        /// After camera's depth texture is generated.
        /// </summary>
        /// <description>
        /// SA: [[wiki:SL-CameraDepthTexture|Using camera's depth textures]].
        /// </description>
        AfterDepthTexture,
        /// <summary>
        /// Before camera's depth+normals texture is generated.
        /// </summary>
        BeforeDepthNormalsTexture,
        /// <summary>
        /// After camera's depth+normals texture is generated.
        /// </summary>
        AfterDepthNormalsTexture,
        /// <summary>
        /// Before deferred rendering G-buffer is rendered.
        /// </summary>
        /// <description>
        /// The G-buffer render target(s) will be set and cleared already, but nothing rendered into them yet.
        /// </description>
        BeforeGBuffer,
        /// <summary>
        /// After deferred rendering G-buffer is rendered.
        /// </summary>
        /// <description>
        /// Will be called immediately after all objects are rendered into G-buffer. The G-buffer render target(s) will be active, however they will not be set up as shader properties yet.
        /// Generally the BeforeLighting event is probably a better place to start doing custom G-buffer modifications.
        /// </description>
        AfterGBuffer,
        /// <summary>
        /// Before lighting pass in deferred rendering.
        /// </summary>
        /// <description>
        /// G-buffer will already be rendered and setup for access as shader parameters.
        /// </description>
        BeforeLighting,
        /// <summary>
        /// After lighting pass in deferred rendering.
        /// </summary>
        /// <description>
        /// Light buffer will be the active render target at this point.
        /// </description>
        AfterLighting,
        /// <summary>
        /// Before final geometry pass in deferred lighting.
        /// </summary>
        BeforeFinalPass,
        /// <summary>
        /// After final geometry pass in deferred lighting.
        /// </summary>
        AfterFinalPass,
        /// <summary>
        /// Before opaque objects in forward rendering.
        /// </summary>
        BeforeForwardOpaque,
        /// <summary>
        /// After opaque objects in forward rendering.
        /// </summary>
        AfterForwardOpaque,
        /// <summary>
        /// Before image effects that happen between opaque & transparent objects.
        /// </summary>
        BeforeImageEffectsOpaque,
        /// <summary>
        /// After image effects that happen between opaque & transparent objects.
        /// </summary>
        AfterImageEffectsOpaque,
        /// <summary>
        /// Before skybox is drawn.
        /// </summary>
        BeforeSkybox,
        /// <summary>
        /// After skybox is drawn.
        /// </summary>
        AfterSkybox,
        /// <summary>
        /// Before transparent objects in forward rendering.
        /// </summary>
        BeforeForwardAlpha,
        /// <summary>
        /// After transparent objects in forward rendering.
        /// </summary>
        AfterForwardAlpha,
        /// <summary>
        /// Before image effects.
        /// </summary>
        BeforeImageEffects,
        /// <summary>
        /// After image effects.
        /// </summary>
        AfterImageEffects,
        /// <summary>
        /// After camera has done rendering everything.
        /// </summary>
        AfterEverything,
        /// <summary>
        /// Before reflections pass in deferred rendering.
        /// </summary>
        BeforeReflections,
        /// <summary>
        /// After reflections pass in deferred rendering.
        /// </summary>
        AfterReflections,
        /// <summary>
        /// Before halo and lens flares.
        /// </summary>
        BeforeHaloAndLensFlares,
        /// <summary>
        /// After halo and lens flares.
        /// </summary>
        AfterHaloAndLensFlares
    }

    internal static class CameraEventUtils
    {
        const CameraEvent k_MinimumValue = CameraEvent.BeforeDepthTexture;
        const CameraEvent k_MaximumValue = CameraEvent.AfterHaloAndLensFlares;

        public static bool IsValid(CameraEvent value)
        {
            return value >= k_MinimumValue && value <= k_MaximumValue;
        }
    }

    // Keep in sync with RenderLightEventType in Runtime/Graphics/CommandBuffer/RenderingEvents.h
    /// <summary>
    /// Defines a place in light's rendering to attach [[Rendering.CommandBuffer]] objects to.
    /// </summary>
    /// <description>
    /// Unity's rendering loop can be extended by adding so called "command buffers" at various points in light rendering; mostly related to shadows. For example, you could do custom processing of the shadow map after it is rendered.
    /// SA: [[Rendering.CommandBuffer]], [[Rendering.CameraEvent]], [[wiki:GraphicsCommandBuffers|command buffers overview]].
    /// </description>
    public enum LightEvent
    {
        /// <summary>
        /// Before shadowmap is rendered.
        /// </summary>
        /// <description>
        /// Shadowmap render target will be set and cleared, but shadow casters not rendered yet.
        /// </description>
        BeforeShadowMap = 0,
        /// <summary>
        /// After shadowmap is rendered.
        /// </summary>
        /// <description>
        /// This value indicates all shadow casters are rendered and the current render target is still the shadow map. Note that shadow cascade parameters are not yet set. See [[Rendering.LightEvent.BeforeScreenspaceMask]] for more information.
        /// </description>
        AfterShadowMap,
        /// <summary>
        /// Before directional light screenspace shadow mask is computed.
        /// </summary>
        /// <description>
        /// Directional lights when using non-mobile shadows "gather" shadowmap into a screenspace buffer and do PCF filtering
        /// during this step. Later on actual object rendering just samples this screenspace buffer.
        /// This light event executes command buffers when the screen-space mask render target is set and cleared, and the shadow cascade parameters are set. Note that the shadow mask is not yet computed.
        /// </description>
        BeforeScreenspaceMask,
        /// <summary>
        /// After directional light screenspace shadow mask is computed.
        /// </summary>
        /// <description>
        /// Directional lights when using non-mobile shadows "gather" shadowmap into a screenspace buffer and do PCF filtering
        /// during this step. Later on actual object rendering just samples this screenspace buffer.
        /// This light event will execute command buffers when the screenspace mask is computed, and the active render target is still the screenspace mask.
        /// </description>
        AfterScreenspaceMask,
        /// <summary>
        /// Before shadowmap pass is rendered.
        /// </summary>
        /// <description>
        /// When this event is triggered, the shadowmap render target has been set and cleared, but shadow casters in the pass have not yet been rendered.
        /// This event differs from [[Rendering.LightEvent.BeforeShadowMap]] in that for light types that render shadows using multiple passes, the event triggers before each pass. Additional control over when this event triggers can be achieved by passing a [[Rendering.ShadowMapPass]] mask to [[Light.AddCommandBuffer]].
        /// </description>
        BeforeShadowMapPass,
        /// <summary>
        /// After shadowmap pass is rendered.
        /// </summary>
        /// <description>
        /// When this event is triggered, all shadow casters in pass have already been rendered, and current render target is still the shadowmap.
        /// This event differs from [[Rendering.LightEvent.AfterShadowMap]] in that for light types that render shadows using multiple passes, the event triggers after each pass. Additional control over when this event triggers can be achieved by passing a [[Rendering.ShadowMapPass]] mask to [[Light.AddCommandBuffer]].
        /// </description>
        AfterShadowMapPass,
    }

    // Keep in sync with RenderShadowMapPassType in Runtime/Graphics/CommandBuffer/RenderingEvents.h
    /// <summary>
    /// Allows precise control over which shadow map passes to execute [[Rendering.CommandBuffer]] objects attached using [[Light.AddCommandBuffer]].
    /// </summary>
    /// <description>
    /// These flags only take effect when used with [[Rendering.LightEvent/BeforeShadowMapPass]] or [[Rendering.LightEvent/AfterShadowMapPass]].
    /// </description>
    [Flags]
    public enum ShadowMapPass
    {
        /// <summary>
        /// +X point light shadow cubemap face.
        /// </summary>
        PointlightPositiveX     = 1 << 0,
        /// <summary>
        /// -X point light shadow cubemap face.
        /// </summary>
        PointlightNegativeX     = 1 << 1,
        /// <summary>
        /// +Y point light shadow cubemap face.
        /// </summary>
        PointlightPositiveY     = 1 << 2,
        /// <summary>
        /// -Y point light shadow cubemap face.
        /// </summary>
        PointlightNegativeY     = 1 << 3,
        /// <summary>
        /// +Z point light shadow cubemap face.
        /// </summary>
        PointlightPositiveZ     = 1 << 4,
        /// <summary>
        /// -Z point light shadow cubemap face.
        /// </summary>
        PointlightNegativeZ     = 1 << 5,

        /// <summary>
        /// First directional shadow map cascade.
        /// </summary>
        DirectionalCascade0     = 1 << 6,
        /// <summary>
        /// Second directional shadow map cascade.
        /// </summary>
        DirectionalCascade1     = 1 << 7,
        /// <summary>
        /// Third directional shadow map cascade.
        /// </summary>
        DirectionalCascade2     = 1 << 8,
        /// <summary>
        /// Fourth directional shadow map cascade.
        /// </summary>
        DirectionalCascade3     = 1 << 9,

        /// <summary>
        /// Spotlight shadow pass.
        /// </summary>
        Spotlight               = 1 << 10,
        /// <summary>
        /// All point light shadow passes.
        /// </summary>
        Pointlight              = PointlightPositiveX | PointlightNegativeX | PointlightPositiveY | PointlightNegativeY | PointlightPositiveZ | PointlightNegativeZ,
        /// <summary>
        /// All directional shadow map passes.
        /// </summary>
        Directional             = DirectionalCascade0 | DirectionalCascade1 | DirectionalCascade2 | DirectionalCascade3,
        /// <summary>
        /// All shadow map passes.
        /// </summary>
        All                     = Pointlight | Spotlight | Directional,
    }

    /// <summary>
    /// Built-in temporary render textures produced during camera's rendering.
    /// </summary>
    /// <description>
    /// When camera is rendering the Scene, in some cases it can produce temporary render textures in the process (e.g. depth textures, deferred G-buffer etc.). This enum indicates these temporary render textures.
    /// BuiltinRenderTextureType can be used as a [[Rendering.RenderTargetIdentifier]] in some functions of [[Rendering.CommandBuffer]].
    /// SA: [[Rendering.CommandBuffer]], [[Rendering.RenderTargetIdentifier]].
    /// </description>
    public enum BuiltinRenderTextureType
    {
        /// <summary>
        /// A globally set property name.
        /// </summary>
        PropertyName = -4, // Property id
        /// <summary>
        /// The raw RenderBuffer pointer to be used.
        /// </summary>
        BufferPtr = -3, // Raw buffer pointer
        /// <summary>
        /// The given RenderTexture.
        /// </summary>
        RenderTexture = -2, // Render texture. We cannot just use BufferPtr because we need lazy resolve of the buffer pointer here (the RT might not be created yet)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        BindableTexture = -1, // a bindable texture, of any dimension, that is not a render texture
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        None = 0, // "nothing", just need zero default value for RenderTargetIdentifier
        /// <summary>
        /// Currently active render target.
        /// </summary>
        /// <description>
        /// During command buffer execution, this identifies the render target that is active "right now". During command buffer execution, the active render target might be changed by [[Rendering.CommandBuffer.SetRenderTarget]] or
        /// [[Rendering.CommandBuffer.Blit]] commands.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        CurrentActive = 1,  // currently active RT
        /// <summary>
        /// Target texture of currently rendering camera.
        /// </summary>
        /// <description>
        /// This is the render target where the current camera would be ultimately rendering into. The render target that is active
        /// right now might be different (e.g. during light shadow map rendering, or right after another [[Rendering.CommandBuffer.Blit]] etc.).
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        CameraTarget = 2, // current camera target
        /// <summary>
        /// Camera's depth texture.
        /// </summary>
        /// <description>
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        Depth = 3,      // camera's depth texture
        /// <summary>
        /// Camera's depth+normals texture.
        /// </summary>
        /// <description>
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        DepthNormals = 4,   // camera's depth+normals texture
        /// <summary>
        /// Resolved depth buffer from deferred.
        /// </summary>
        /// <description>
        /// The resolved depth buffer contains depth written when filling G-buffers as well as depth from forward rendered objects if there's an active shadowed directional light, or if the camera has requested a depth texture.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        ResolvedDepth = 5, // resolved depth buffer from deferred
        //SeparatePassDepth = 6, // "separate pass workaround" depth buffer from deferred
        /// <summary>
        /// Deferred lighting (normals+specular) G-buffer.
        /// </summary>
        /// <description>
        /// World space normals in RGB channels; specular exponent in A channel.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        PrepassNormalsSpec = 7,
        /// <summary>
        /// Deferred lighting light buffer.
        /// </summary>
        /// <description>
        /// Contains lighting information in legacy (prepass) deferred lighting.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        PrepassLight = 8,
        /// <summary>
        /// Deferred lighting HDR specular light buffer (Xbox 360 only).
        /// </summary>
        /// <description>
        /// Contains specular lighting information in legacy (prepass) deferred lighting. This is only used on Xbox 360, and only when camera is HDR.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        PrepassLightSpec = 9,
        /// <summary>
        /// Deferred shading G-buffer #0 (typically diffuse color).
        /// </summary>
        /// <description>
        /// Built-in deferred shaders put diffuse albedo color into RGB channels of this texture. But your own custom shaders could be outputting anything there of course.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        GBuffer0 = 10,
        /// <summary>
        /// Deferred shading G-buffer #1 (typically specular + roughness).
        /// </summary>
        /// <description>
        /// Built-in deferred shaders put specular color into RGB channels, and roughness into A channel of this texture. But your own custom shaders could be outputting anything there of course.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        GBuffer1 = 11,
        /// <summary>
        /// Deferred shading G-buffer #2 (typically normals).
        /// </summary>
        /// <description>
        /// Built-in deferred shaders put world-space normals into RGB channels of this texture. But your own custom shaders could be outputting anything there of course.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        GBuffer2 = 12,
        /// <summary>
        /// Deferred shading G-buffer #3 (typically emission/lighting).
        /// </summary>
        /// <description>
        /// Built-in deferred shaders put ambient & emission into RGB channels of this texture. And then the lights are also added there during lighting pass. But your own custom shaders could be outputting anything there of course.
        /// Note that GBuffer3 render texture is not created when the current camera is using HDR; instead emission/lighting is rendered directly into
        /// a camera's target texture. You'll need to use CameraTarget render texture type to handle the HDR camera case.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        GBuffer3 = 13,
        /// <summary>
        /// Reflections gathered from default reflection and reflections probes.
        /// </summary>
        /// <description>
        /// Used by screen space reflections as a fallback, when it's not possible to get reflections from the screen.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        Reflections = 14,
        /// <summary>
        /// Motion Vectors generated when the camera has motion vectors enabled.
        /// </summary>
        /// <description>
        /// Used by various post effects that require per pixel motion information.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        MotionVectors = 15,
        /// <summary>
        /// Deferred shading G-buffer #4 (typically occlusion mask for static lights if any).
        /// </summary>
        /// <description>
        /// Built-in deferred shaders put baked direct light occlusion into RGBA channels of this texture on platform that support at least 8 render targets.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        GBuffer4 = 16,
        /// <summary>
        /// G-buffer #5 Available.
        /// </summary>
        /// <description>
        /// Available for custom effects on platforms that support at least 8 render targets.
        /// </description>
        GBuffer5 = 17,
        /// <summary>
        /// G-buffer #6 Available.
        /// </summary>
        /// <description>
        /// Available for custom effects on platforms that support at least 8 render targets.
        /// </description>
        GBuffer6 = 18,
        /// <summary>
        /// G-buffer #7 Available.
        /// </summary>
        /// <description>
        /// Available for custom effects on platforms that support at least 8 render targets.
        /// </description>
        GBuffer7 = 19,
    };

    // Match ShaderPassType on C++ side
    /// <summary>
    /// Shader pass type for Unity's lighting pipeline.
    /// </summary>
    /// <description>
    /// This corresponds to "LightMode" tag in the shader pass, see [[wiki:man:SL-PassTags|Pass tags]].
    /// </description>
    public enum PassType
    {
        /// <summary>
        /// Regular shader pass that does not interact with lighting.
        /// </summary>
        Normal = 0,
        /// <summary>
        /// Legacy vertex-lit shader pass.
        /// </summary>
        Vertex = 1,
        /// <summary>
        /// Legacy vertex-lit shader pass, with mobile lightmaps.
        /// </summary>
        VertexLM = 2,

        /// <summary>
        /// Legacy vertex-lit shader pass, with desktop (RGBM) lightmaps.
        /// </summary>
        [System.Obsolete("VertexLMRGBM PassType is obsolete. Please use VertexLM PassType together with DecodeLightmap shader function.")]
        VertexLMRGBM = 3,

        /// <summary>
        /// Forward rendering base pass.
        /// </summary>
        ForwardBase = 4,
        /// <summary>
        /// Forward rendering additive pixel light pass.
        /// </summary>
        ForwardAdd = 5,
        /// <summary>
        /// Legacy deferred lighting (light pre-pass) base pass.
        /// </summary>
        LightPrePassBase = 6,
        /// <summary>
        /// Legacy deferred lighting (light pre-pass) final pass.
        /// </summary>
        LightPrePassFinal = 7,
        /// <summary>
        /// Shadow caster & depth texure shader pass.
        /// </summary>
        ShadowCaster = 8,
        // ShadowCollector = 9 -- not needed starting with 5.0
        /// <summary>
        /// Deferred Shading shader pass.
        /// </summary>
        Deferred = 10,
        /// <summary>
        /// Shader pass used to generate the albedo and emissive values used as input to lightmapping.
        /// </summary>
        /// <description>
        /// Baked and realtime GI use this pass to get information about the shader emission and albedo properties.
        /// Surface shaders generate this pass automatically, in a similar way to other lighting related passes.
        /// </description>
        Meta = 11,
        /// <summary>
        /// Motion vector render pass.
        /// </summary>
        /// <description>
        /// Used to generate motion vectors that can be used in Image Effects. This pass is rendered after opaque objects, but before opaque Image Effects..
        /// SA: [[Renderer.motionVectorGenerationMode]], [[DepthTextureMode.MotionVectors]], [[SkinnedMeshRenderer.skinnedMotionVectors]], [[SystemInfo.supportsMotionVectors]].
        /// </description>
        MotionVectors = 12,
        /// <summary>
        /// Custom scriptable pipeline.
        /// </summary>
        ScriptableRenderPipeline = 13,
        /// <summary>
        /// Custom scriptable pipeline when lightmode is set to default unlit or no light mode is set.
        /// </summary>
        ScriptableRenderPipelineDefaultUnlit = 14
    }

    // Match ShadowCastingMode enum on C++ side
    /// <summary>
    /// How shadows are cast from this object.
    /// </summary>
    /// <description>
    /// SA: [[Renderer.shadowCastingMode]].
    /// </description>
    public enum ShadowCastingMode
    {
        /// <summary>
        /// No shadows are cast from this object.
        /// </summary>
        /// <description>
        /// SA: [[Renderer.shadowCastingMode]].
        /// </description>
        Off = 0,
        /// <summary>
        /// Shadows are cast from this object.
        /// </summary>
        /// <description>
        /// Shadow rendering will use the same culling mode as specified in the object's shader. Typically this means that single-sided objects (like a Plane or a Quad) do not cast shadows if the light is behind them. Use [[Rendering.ShadowCastingMode.TwoSided]] to treat objects as two-sided for shadow rendering.
        /// SA: [[Renderer.shadowCastingMode]].
        /// </description>
        On = 1,
        /// <summary>
        /// Shadows are cast from this object, treating it as two-sided.
        /// </summary>
        /// <description>
        /// Shadow rendering will turn off backface culling, even if object's shader has backface culling on. This means that single-sided objects (like a Plane or a Quad) will cast shadows, even if the light is behind them.
        /// SA: [[Renderer.shadowCastingMode]].
        /// </description>
        TwoSided = 2,
        /// <summary>
        /// Object casts shadows, but is otherwise invisible in the Scene.
        /// </summary>
        /// <description>
        /// This is useful for certain effects or optimization purposes; essentially this makes an object that only casts shadows, but is otherwise invisible.
        /// SA: [[Renderer.shadowCastingMode]].
        /// </description>
        ShadowsOnly = 3
    }

    // Match LightShadowResolution on C++ side
    /// <summary>
    /// Shadow resolution options for a [[Light]].
    /// </summary>
    /// <description>
    /// SA: [[Light.shadowResolution]], [[QualitySettings.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
    /// </description>
    public enum LightShadowResolution
    {
        /// <summary>
        /// Use resolution from QualitySettings (default).
        /// </summary>
        /// <description>
        /// SA: [[Light.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        FromQualitySettings = -1,
        /// <summary>
        /// Low shadow map resolution.
        /// </summary>
        /// <description>
        /// SA: [[Light.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        Low = 0,
        /// <summary>
        /// Medium shadow map resolution.
        /// </summary>
        /// <description>
        /// SA: [[Light.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        Medium = 1,
        /// <summary>
        /// High shadow map resolution.
        /// </summary>
        /// <description>
        /// SA: [[Light.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        High = 2,
        /// <summary>
        /// Very high shadow map resolution.
        /// </summary>
        /// <description>
        /// SA: [[Light.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        VeryHigh = 3
    }

    // Match GfxDeviceRenderer enum on C++ side
    /// <summary>
    /// Graphics device API type.
    /// </summary>
    /// <description>
    /// Many different low-level graphics APIs can be used by Unity. If for some reason you need to know
    /// whether Direct3D 9 or 11 is being used, or OpenGL ES 2 or 3, you can use [[SystemInfo.graphicsDeviceType]]
    /// to check for that.
    /// SA: [[SystemInfo.graphicsDeviceType]].
    /// </description>
    [UsedByNativeCode]
    public enum GraphicsDeviceType
    {
        /// <summary>
        /// OpenGL 2.x graphics API. (deprecated, only available on Linux and MacOSX)
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        [System.Obsolete("OpenGL2 is no longer supported in Unity 5.5+")]
        OpenGL2 = 0,
        /// <summary>
        /// Direct3D 9 graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        [System.Obsolete("Direct3D 9 is no longer supported in Unity 2017.2+")]
        Direct3D9 = 1,
        /// <summary>
        /// Direct3D 11 graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        Direct3D11 = 2,
        /// <summary>
        /// PlayStation 3 graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        [System.Obsolete("PS3 is no longer supported in Unity 5.5+")]
        PlayStation3 = 3,
        /// <summary>
        /// No graphics API.
        /// </summary>
        /// <description>
        /// This typically happens when a "null" graphics API is explicitly requested from command line arguments,
        /// for example when running game servers or editor in batch mode.
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        Null = 4,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.Obsolete("Xbox360 is no longer supported in Unity 5.5+")]
        Xbox360 = 6,
        /// <summary>
        /// OpenGL ES 2.0 graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        OpenGLES2 = 8,
        /// <summary>
        /// OpenGL ES 3.0 graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        OpenGLES3 = 11,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.Obsolete("PVita is no longer supported as of Unity 2018")]
        PlayStationVita = 12,
        /// <summary>
        /// PlayStation 4 graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        PlayStation4 = 13,
        /// <summary>
        /// Xbox One graphics API using Direct3D 11.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        XboxOne = 14,
        /// <summary>
        /// PlayStation Mobile (PSM) graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        [System.Obsolete("PlayStationMobile is no longer supported in Unity 5.3+")]
        PlayStationMobile = 15,
        /// <summary>
        /// iOS Metal graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        Metal = 16,
        /// <summary>
        /// OpenGL (Core profile - GL3 or later) graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        OpenGLCore = 17,
        /// <summary>
        /// Direct3D 12 graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        Direct3D12 = 18,
        /// <summary>
        /// Nintendo 3DS graphics API.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        [System.Obsolete("Nintendo 3DS support is unavailable since 2018.1")]
        N3DS = 19,
        /// <summary>
        /// Vulkan (EXPERIMENTAL).
        /// </summary>
        Vulkan = 21,
        /// <summary>
        /// Nintendo Switch graphics API.
        /// </summary>
        Switch = 22,
        /// <summary>
        /// Xbox One graphics API using Direct3D 12.
        /// </summary>
        /// <description>
        /// SA: [[SystemInfo.graphicsDeviceType]].
        /// </description>
        XboxOneD3D12 = 23
    }

    /// <summary>
    /// Graphics Tier.
    /// SA: [[Graphics.activeTier]].
    /// </summary>
    public enum GraphicsTier
    {
        /// <summary>
        /// The first graphics tier (Low) - corresponds to shader define UNITY_HARDWARE_TIER1.
        /// </summary>
        /// <description>
        ///                 This tier is selected for:
        ///                 - Android devices that don't have OpenGL ES 3 support
        ///                 - iPhone 5, 5C and earlier
        ///                 - iPod Touch 5th generation and earlier
        ///                 - iPad 4th generation and earlier
        ///                 - iPad Mini 1st generation
        ///                 - DirectX 9 class hardware on desktops
        ///                 - HoloLens
        /// </description>
        Tier1 = 0,
        /// <summary>
        /// The second graphics tier (Medium) - corresponds to shader define UNITY_HARDWARE_TIER2.
        /// </summary>
        /// <description>
        ///                 This tier is selected for:
        ///                 - Android devices with OpenGL ES 3.0+ support
        ///                 - iPhone 5S and later
        ///                 - iPod Touch 6th generation
        ///                 - iPad Air and later
        ///                 - iPad Mini 2nd generation and later
        ///                 - Apple TV
        /// </description>
        Tier2 = 1,
        /// <summary>
        /// The third graphics tier (High) - corresponds to shader define UNITY_HARDWARE_TIER3.
        /// </summary>
        /// <description>
        ///                 This tier is selected for:
        ///                 - OpenGL or DirectX 11+ class hardware on desktops
        ///                 - Metal on Macs
        /// </description>
        Tier3 = 2,
    }

    // Note: match layout of C++ MonoRenderTargetIdentifier!
    /// <summary>
    /// Identifies a [[RenderTexture]] for a [[Rendering.CommandBuffer]].
    /// </summary>
    /// <description>
    /// Render textures can be identified in a number of ways, for example a [[RenderTexture]] object, or one of built-in render textures ([[Rendering.BuiltinRenderTextureType]]), or a temporary render texture with a name (that was created using [[Rendering.CommandBuffer.GetTemporaryRT]]).
    /// This struct serves as a way to identify them, and has implcit conversion operators so that in most cases you can save some typing.
    /// SA: [[Rendering.CommandBuffer]].
    /// </description>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct RenderTargetIdentifier : IEquatable<RenderTargetIdentifier>
    {
        // constructors
        /// <summary>
        /// Creates a render target identifier.
        /// </summary>
        /// <param name="type">
        /// Built-in temporary render texture type.
        /// </param>
        /// <description>
        /// Textures can be identified in a number of ways, for example a [[RenderTexture]] object, or a [[Texture]] object, or one of built-in render textures ([[Rendering.BuiltinRenderTextureType]]), or a temporary render texture with a name (that was created using [[Rendering.CommandBuffer.GetTemporaryRT]]).
        /// RenderTargetIdentifier can be implicitly created from a RenderTexture reference, or a Texture reference, or a BuiltinRenderTextureType, or a name.
        /// A RenderTargetIdentifier created from Texture reference is only valid when passed to [[Rendering.CommandBuffer.SetGlobalTexture]]
        /// See Also: [[Rendering.CommandBuffer.SetRenderTarget]], [[Rendering.CommandBuffer.SetGlobalTexture]].
        /// </description>
        public RenderTargetIdentifier(BuiltinRenderTextureType type)
        {
            m_Type = type;
            m_NameID = -1; // FastPropertyName kInvalidIndex
            m_InstanceID = 0;
            m_BufferPointer = IntPtr.Zero;
            m_MipLevel = 0;
            m_CubeFace = CubemapFace.Unknown;
            m_DepthSlice = 0;
        }

        /// <summary>
        /// Creates a render target identifier.
        /// </summary>
        /// <param name="name">
        /// Temporary render texture name.
        /// </param>
        /// <description>
        /// Textures can be identified in a number of ways, for example a [[RenderTexture]] object, or a [[Texture]] object, or one of built-in render textures ([[Rendering.BuiltinRenderTextureType]]), or a temporary render texture with a name (that was created using [[Rendering.CommandBuffer.GetTemporaryRT]]).
        /// RenderTargetIdentifier can be implicitly created from a RenderTexture reference, or a Texture reference, or a BuiltinRenderTextureType, or a name.
        /// A RenderTargetIdentifier created from Texture reference is only valid when passed to [[Rendering.CommandBuffer.SetGlobalTexture]]
        /// See Also: [[Rendering.CommandBuffer.SetRenderTarget]], [[Rendering.CommandBuffer.SetGlobalTexture]].
        /// </description>
        public RenderTargetIdentifier(string name)
        {
            m_Type = BuiltinRenderTextureType.PropertyName;
            m_NameID = Shader.PropertyToID(name);
            m_InstanceID = 0;
            m_BufferPointer = IntPtr.Zero;
            m_MipLevel = 0;
            m_CubeFace = CubemapFace.Unknown;
            m_DepthSlice = 0;
        }

        /// <summary>
        /// Creates a render target identifier.
        /// </summary>
        /// <param name="name">
        /// Temporary render texture name.
        /// </param>
        /// <description>
        /// Textures can be identified in a number of ways, for example a [[RenderTexture]] object, or a [[Texture]] object, or one of built-in render textures ([[Rendering.BuiltinRenderTextureType]]), or a temporary render texture with a name (that was created using [[Rendering.CommandBuffer.GetTemporaryRT]]).
        /// RenderTargetIdentifier can be implicitly created from a RenderTexture reference, or a Texture reference, or a BuiltinRenderTextureType, or a name.
        /// A RenderTargetIdentifier created from Texture reference is only valid when passed to [[Rendering.CommandBuffer.SetGlobalTexture]]
        /// See Also: [[Rendering.CommandBuffer.SetRenderTarget]], [[Rendering.CommandBuffer.SetGlobalTexture]].
        /// </description>
        public RenderTargetIdentifier(string name, int mipLevel = 0, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
        {
            m_Type = BuiltinRenderTextureType.PropertyName;
            m_NameID = Shader.PropertyToID(name);
            m_InstanceID = 0;
            m_BufferPointer = IntPtr.Zero;
            m_MipLevel = mipLevel;
            m_CubeFace = cubeFace;
            m_DepthSlice = depthSlice;
        }

        /// <summary>
        /// Creates a render target identifier.
        /// </summary>
        /// <param name="nameID">
        /// Temporary render texture name (as integer, see [[Shader.PropertyToID]]).
        /// </param>
        /// <description>
        /// Textures can be identified in a number of ways, for example a [[RenderTexture]] object, or a [[Texture]] object, or one of built-in render textures ([[Rendering.BuiltinRenderTextureType]]), or a temporary render texture with a name (that was created using [[Rendering.CommandBuffer.GetTemporaryRT]]).
        /// RenderTargetIdentifier can be implicitly created from a RenderTexture reference, or a Texture reference, or a BuiltinRenderTextureType, or a name.
        /// A RenderTargetIdentifier created from Texture reference is only valid when passed to [[Rendering.CommandBuffer.SetGlobalTexture]]
        /// See Also: [[Rendering.CommandBuffer.SetRenderTarget]], [[Rendering.CommandBuffer.SetGlobalTexture]].
        /// </description>
        public RenderTargetIdentifier(int nameID)
        {
            m_Type = BuiltinRenderTextureType.PropertyName;
            m_NameID = nameID;
            m_InstanceID = 0;
            m_BufferPointer = IntPtr.Zero;
            m_MipLevel = 0;
            m_CubeFace = CubemapFace.Unknown;
            m_DepthSlice = 0;
        }

        /// <summary>
        /// Creates a render target identifier.
        /// </summary>
        /// <param name="nameID">
        /// Temporary render texture name (as integer, see [[Shader.PropertyToID]]).
        /// </param>
        /// <description>
        /// Textures can be identified in a number of ways, for example a [[RenderTexture]] object, or a [[Texture]] object, or one of built-in render textures ([[Rendering.BuiltinRenderTextureType]]), or a temporary render texture with a name (that was created using [[Rendering.CommandBuffer.GetTemporaryRT]]).
        /// RenderTargetIdentifier can be implicitly created from a RenderTexture reference, or a Texture reference, or a BuiltinRenderTextureType, or a name.
        /// A RenderTargetIdentifier created from Texture reference is only valid when passed to [[Rendering.CommandBuffer.SetGlobalTexture]]
        /// See Also: [[Rendering.CommandBuffer.SetRenderTarget]], [[Rendering.CommandBuffer.SetGlobalTexture]].
        /// </description>
        public RenderTargetIdentifier(int nameID, int mipLevel = 0, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
        {
            m_Type = BuiltinRenderTextureType.PropertyName;
            m_NameID = nameID;
            m_InstanceID = 0;
            m_BufferPointer = IntPtr.Zero;
            m_MipLevel = mipLevel;
            m_CubeFace = cubeFace;
            m_DepthSlice = depthSlice;
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTargetIdentifier(RenderTargetIdentifier renderTargetIdentifier, int mipLevel, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
        {
            m_Type = renderTargetIdentifier.m_Type;
            m_NameID = renderTargetIdentifier.m_NameID;
            m_InstanceID = renderTargetIdentifier.m_InstanceID;
            m_BufferPointer = renderTargetIdentifier.m_BufferPointer;
            m_MipLevel = mipLevel;
            m_CubeFace = cubeFace;
            m_DepthSlice = depthSlice;
        }

        /// <summary>
        /// Creates a render target identifier.
        /// </summary>
        /// <param name="tex">
        /// RenderTexture or Texture object to use.
        /// </param>
        /// <description>
        /// Textures can be identified in a number of ways, for example a [[RenderTexture]] object, or a [[Texture]] object, or one of built-in render textures ([[Rendering.BuiltinRenderTextureType]]), or a temporary render texture with a name (that was created using [[Rendering.CommandBuffer.GetTemporaryRT]]).
        /// RenderTargetIdentifier can be implicitly created from a RenderTexture reference, or a Texture reference, or a BuiltinRenderTextureType, or a name.
        /// A RenderTargetIdentifier created from Texture reference is only valid when passed to [[Rendering.CommandBuffer.SetGlobalTexture]]
        /// See Also: [[Rendering.CommandBuffer.SetRenderTarget]], [[Rendering.CommandBuffer.SetGlobalTexture]].
        /// </description>
        public RenderTargetIdentifier(Texture tex)
        {
            if (tex == null)
            {
                m_Type = BuiltinRenderTextureType.None;
            }
            else if (tex is RenderTexture)
            {
                m_Type = BuiltinRenderTextureType.RenderTexture;
            }
            else
            {
                m_Type = BuiltinRenderTextureType.BindableTexture;
            }
            m_BufferPointer = IntPtr.Zero;
            m_NameID = -1; // FastPropertyName kInvalidIndex
            m_InstanceID = tex ? tex.GetInstanceID() : 0;
            m_MipLevel = 0;
            m_CubeFace = CubemapFace.Unknown;
            m_DepthSlice = 0;
        }

        /// <summary>
        /// Creates a render target identifier.
        /// </summary>
        /// <param name="tex">
        /// RenderTexture or Texture object to use.
        /// </param>
        /// <description>
        /// Textures can be identified in a number of ways, for example a [[RenderTexture]] object, or a [[Texture]] object, or one of built-in render textures ([[Rendering.BuiltinRenderTextureType]]), or a temporary render texture with a name (that was created using [[Rendering.CommandBuffer.GetTemporaryRT]]).
        /// RenderTargetIdentifier can be implicitly created from a RenderTexture reference, or a Texture reference, or a BuiltinRenderTextureType, or a name.
        /// A RenderTargetIdentifier created from Texture reference is only valid when passed to [[Rendering.CommandBuffer.SetGlobalTexture]]
        /// See Also: [[Rendering.CommandBuffer.SetRenderTarget]], [[Rendering.CommandBuffer.SetGlobalTexture]].
        /// </description>
        public RenderTargetIdentifier(Texture tex, int mipLevel = 0, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
        {
            if (tex == null)
            {
                m_Type = BuiltinRenderTextureType.None;
            }
            else if (tex is RenderTexture)
            {
                m_Type = BuiltinRenderTextureType.RenderTexture;
            }
            else
            {
                m_Type = BuiltinRenderTextureType.BindableTexture;
            }
            m_BufferPointer = IntPtr.Zero;
            m_NameID = -1; // FastPropertyName kInvalidIndex
            m_InstanceID = tex ? tex.GetInstanceID() : 0;
            m_MipLevel = mipLevel;
            m_CubeFace = cubeFace;
            m_DepthSlice = depthSlice;
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTargetIdentifier(RenderBuffer buf, int mipLevel = 0, CubemapFace cubeFace = CubemapFace.Unknown, int depthSlice = 0)
        {
            m_Type = BuiltinRenderTextureType.BufferPtr;
            m_NameID = -1;
            m_InstanceID = buf.m_RenderTextureInstanceID;
            m_BufferPointer = buf.m_BufferPtr;
            m_MipLevel = mipLevel;
            m_CubeFace = cubeFace;
            m_DepthSlice = depthSlice;
        }

        // implicit conversion operators
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static implicit operator RenderTargetIdentifier(BuiltinRenderTextureType type)
        {
            return new RenderTargetIdentifier(type);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static implicit operator RenderTargetIdentifier(string name)
        {
            return new RenderTargetIdentifier(name);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static implicit operator RenderTargetIdentifier(int nameID)
        {
            return new RenderTargetIdentifier(nameID);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static implicit operator RenderTargetIdentifier(Texture tex)
        {
            return new RenderTargetIdentifier(tex);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static implicit operator RenderTargetIdentifier(RenderBuffer buf)
        {
            return new RenderTargetIdentifier(buf);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public override string ToString()
        {
            return UnityString.Format("Type {0} NameID {1} InstanceID {2}", m_Type, m_NameID, m_InstanceID);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public override int GetHashCode()
        {
            return (m_Type.GetHashCode() * 23 + m_NameID.GetHashCode()) * 23 + m_InstanceID.GetHashCode();
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public bool Equals(RenderTargetIdentifier rhs)
        {
            return m_Type == rhs.m_Type &&
                m_NameID == rhs.m_NameID &&
                m_InstanceID == rhs.m_InstanceID &&
                m_BufferPointer == rhs.m_BufferPointer &&
                m_MipLevel == rhs.m_MipLevel &&
                m_CubeFace == rhs.m_CubeFace &&
                m_DepthSlice == rhs.m_DepthSlice;
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (!(obj is RenderTargetIdentifier))
                return false;
            RenderTargetIdentifier rhs = (RenderTargetIdentifier)obj;
            return Equals(rhs);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static bool operator==(RenderTargetIdentifier lhs, RenderTargetIdentifier rhs)
        {
            return lhs.Equals(rhs);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static bool operator!=(RenderTargetIdentifier lhs, RenderTargetIdentifier rhs)
        {
            return !lhs.Equals(rhs);
        }

        // private variable never read: we match struct in native side
        #pragma warning disable 0414
        private BuiltinRenderTextureType m_Type;
        private int m_NameID;
        private int m_InstanceID;
        private IntPtr m_BufferPointer;
        private int m_MipLevel;
        private CubemapFace m_CubeFace;
        private int m_DepthSlice;
        #pragma warning restore 0414
    }

    /// <summary>
    /// Describes a render target with one or more color buffers, a depth/stencil buffer and the associated load/store-actions that are applied when the render target is active.
    /// </summary>
    /// <description>
    /// This data structure is similiar to [[RenderTargetSetup]], but relies on a [[RenderTargetIdentifier]] to ensure compatability with [[CommandBuffer.SetRenderTarget]].
    /// To render to a specific mip-level, cubemap face or depth slice the [[RenderTargetIdentifier]] should be created accordingly.
    /// Note: the number of load- and store-actions specified for color buffers must be equal to the number of color buffers.
    /// SA: [[Rendering.CommandBuffer]].
    /// </description>
    public struct RenderTargetBinding
    {
        RenderTargetIdentifier[] m_ColorRenderTargets;
        RenderTargetIdentifier m_DepthRenderTarget;

        RenderBufferLoadAction[] m_ColorLoadActions;
        RenderBufferStoreAction[] m_ColorStoreActions;

        RenderBufferLoadAction m_DepthLoadAction;
        RenderBufferStoreAction m_DepthStoreAction;

        /// <summary>
        /// Color buffers to use as render targets.
        /// </summary>
        public RenderTargetIdentifier[] colorRenderTargets { get { return m_ColorRenderTargets; } set { m_ColorRenderTargets = value; } }
        /// <summary>
        /// Depth/stencil buffer to use as render target.
        /// </summary>
        public RenderTargetIdentifier depthRenderTarget { get { return m_DepthRenderTarget; } set { m_DepthRenderTarget = value; } }
        /// <summary>
        /// Load actions for color buffers.
        /// </summary>
        /// <description>
        /// Not all platforms support load actions. If the platform you are using does not support load actions, [[RenderBufferLoadAction.Load]] will be used instead.
        /// </description>
        public RenderBufferLoadAction[] colorLoadActions { get { return m_ColorLoadActions; } set { m_ColorLoadActions = value; } }
        /// <summary>
        /// Store actions for color buffers.
        /// </summary>
        /// <description>
        /// Not all platforms support store actions. If the platform you are using does not support store actions, [[RenderBufferStoreAction.Store]] will be used instead.
        /// </description>
        public RenderBufferStoreAction[] colorStoreActions { get { return m_ColorStoreActions; } set { m_ColorStoreActions = value; } }
        /// <summary>
        /// Load action for the depth/stencil buffer.
        /// </summary>
        /// <description>
        /// Not all platforms support load actions. If the platform you are using does not support load actions, [[RenderBufferLoadAction.Load]] will be used instead.
        /// </description>
        public RenderBufferLoadAction depthLoadAction { get { return m_DepthLoadAction; } set { m_DepthLoadAction = value; } }
        /// <summary>
        /// Store action for the depth/stencil buffer.
        /// </summary>
        /// <description>
        /// Not all platforms support store actions. If the platform you are using does not support store actions, [[RenderBufferStoreAction.Store]] will be used instead.
        /// </description>
        public RenderBufferStoreAction depthStoreAction { get { return m_DepthStoreAction; } set { m_DepthStoreAction = value; } }

        /// <summary>
        /// Constructs RenderTargetBinding.
        /// </summary>
        /// <param name="depthLoadAction">
        /// Load action for the depth/stencil buffer.
        /// </param>
        /// <param name="depthStoreAction">
        /// Store action for the depth/stencil buffer.
        /// </param>
        public RenderTargetBinding(RenderTargetIdentifier[] colorRenderTargets, RenderBufferLoadAction[] colorLoadActions, RenderBufferStoreAction[] colorStoreActions,
                                   RenderTargetIdentifier depthRenderTarget, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
        {
            m_ColorRenderTargets = colorRenderTargets;
            m_DepthRenderTarget = depthRenderTarget;

            m_ColorLoadActions = colorLoadActions;
            m_ColorStoreActions = colorStoreActions;

            m_DepthLoadAction = depthLoadAction;
            m_DepthStoreAction = depthStoreAction;
        }

        /// <summary>
        /// Constructs RenderTargetBinding.
        /// </summary>
        /// <param name="colorLoadAction">
        /// Load actions for color buffers.
        /// </param>
        /// <param name="colorStoreAction">
        /// Store actions for color buffers.
        /// </param>
        /// <param name="depthLoadAction">
        /// Load action for the depth/stencil buffer.
        /// </param>
        /// <param name="depthStoreAction">
        /// Store action for the depth/stencil buffer.
        /// </param>
        public RenderTargetBinding(RenderTargetIdentifier colorRenderTarget, RenderBufferLoadAction colorLoadAction, RenderBufferStoreAction colorStoreAction,
                                   RenderTargetIdentifier depthRenderTarget, RenderBufferLoadAction depthLoadAction, RenderBufferStoreAction depthStoreAction)
            : this(new RenderTargetIdentifier[] { colorRenderTarget }, new RenderBufferLoadAction[] { colorLoadAction }, new RenderBufferStoreAction[] { colorStoreAction }, depthRenderTarget, depthLoadAction, depthStoreAction)
        {
        }

        /// <summary>
        /// Constructs RenderTargetBinding.
        /// </summary>
        public RenderTargetBinding(RenderTargetSetup setup)
        {
            m_ColorRenderTargets = new RenderTargetIdentifier[setup.color.Length];
            for (int i = 0; i < m_ColorRenderTargets.Length; ++i)
                m_ColorRenderTargets[i] = new RenderTargetIdentifier(setup.color[i], setup.mipLevel, setup.cubemapFace, setup.depthSlice);

            m_DepthRenderTarget = setup.depth;

            m_ColorLoadActions = (RenderBufferLoadAction[])setup.colorLoad.Clone();
            m_ColorStoreActions = (RenderBufferStoreAction[])setup.colorStore.Clone();

            m_DepthLoadAction = setup.depthLoad;
            m_DepthStoreAction = setup.depthStore;
        }
    }


// Keep in sync with ReflectionProbeUsage in Runtime\Camera\ReflectionProbeTypes.h
    /// <summary>
    /// Reflection Probe usage.
    /// </summary>
    public enum ReflectionProbeUsage
    {
        /// <summary>
        /// Reflection probes are disabled, skybox will be used for reflection.
        /// </summary>
        Off,
        /// <summary>
        /// Reflection probes are enabled. Blending occurs only between probes, useful in indoor environments. The renderer will use default reflection if there are no reflection probes nearby, but no blending between default reflection and probe will occur.
        /// </summary>
        BlendProbes,
        /// <summary>
        /// Reflection probes are enabled. Blending occurs between probes or probes and default reflection, useful for outdoor environments.
        /// </summary>
        BlendProbesAndSkybox,
        /// <summary>
        /// Reflection probes are enabled, but no blending will occur between probes when there are two overlapping volumes.
        /// </summary>
        Simple
    }

    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    public enum ReflectionProbeType
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        Cube = 0,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        Card = 1,
    }

    /// <summary>
    /// Values for ReflectionProbe.clearFlags, determining what to clear when rendering a [[ReflectionProbe]].
    /// </summary>
    /// <description>
    /// SA: [[wiki:class-ReflectionProbe|reflection probe component]].
    /// </description>
    public enum ReflectionProbeClearFlags
    {
        /// <summary>
        /// Clear with the skybox.
        /// </summary>
        /// <description>
        /// If a skybox is not set up, the Reflection Probe will clear with a ReflectionProbe.backgroundColor.
        /// SA: [[ReflectionProbe.clearFlags]] property.
        /// </description>
        Skybox = 1,
        /// <summary>
        /// Clear with a background color.
        /// </summary>
        /// <description>
        /// SA: [[ReflectionProbe.clearFlags]] property.
        /// </description>
        SolidColor = 2
    }

    /// <summary>
    /// Reflection probe's update mode.
    /// </summary>
    public enum ReflectionProbeMode
    {
        /// <summary>
        /// Reflection probe is baked in the Editor.
        /// </summary>
        Baked = 0,
        /// <summary>
        /// Reflection probe is updating in realtime.
        /// </summary>
        Realtime = 1,
        /// <summary>
        /// Reflection probe uses a custom texture specified by the user.
        /// </summary>
        Custom = 2
    }

    /// <summary>
    /// ReflectionProbeBlendInfo contains information required for blending probes.
    /// </summary>
    /// <description>
    /// SA: [[Renderer.reflectionProbeUsage]].
    /// </description>
    [UsedByNativeCode]
    public struct ReflectionProbeBlendInfo
    {
        /// <summary>
        /// Reflection Probe used in blending.
        /// </summary>
        public ReflectionProbe probe;
        /// <summary>
        /// Specifies the weight used in the interpolation between two probes, value varies from 0.0 to 1.0.
        /// </summary>
        public float weight;
    }

    /// <summary>
    /// An enum describing the way a realtime reflection probe refreshes in the Player.
    /// </summary>
    public enum ReflectionProbeRefreshMode
    {
        /// <summary>
        /// Causes the probe to update only on the first frame it becomes visible. The probe will no longer update automatically, however you may subsequently use [[RenderProbe]] to refresh the probe
        /// SA: [[ReflectionProbe.RenderProbe]].
        /// </summary>
        OnAwake = 0,
        /// <summary>
        /// Causes Unity to update the probe's cubemap every frame.
        /// Note that updating a probe is very costly. Setting this option on too many probes could have a significant negative effect on frame rate. Use time-slicing to help improve performance.
        /// SA: [[ReflectionProbeTimeSlicingMode]].
        /// </summary>
        EveryFrame = 1,
        /// <summary>
        /// Sets the probe to never be automatically updated by Unity while your game is running. Use this to completely control the probe refresh behavior by script.
        /// SA: [[ReflectionProbe.RenderProbe]].
        /// </summary>
        ViaScripting = 2,
    }

    /// <summary>
    /// When a probe's [[ReflectionProbe.refreshMode]] is set to [[ReflectionProbeRefreshMode.EveryFrame]], this enum specify whether or not Unity should update the probe's cubemap over several frames or update the whole cubemap in one frame.
    /// Updating a probe's cubemap is a costly operation. Unity needs to render the entire Scene for each face of the cubemap, as well as perform special blurring in order to get glossy reflections. The impact on frame rate can be significant. Time-slicing helps maintaning a more constant frame rate during these updates by performing the rendering over several frames.
    /// </summary>
    public enum ReflectionProbeTimeSlicingMode
    {
        /// <summary>
        /// Instructs Unity to use time-slicing by first rendering all faces at once, then spreading the remaining work over the next 8 frames. Using this option, updating the probe will take 9 frames.
        /// </summary>
        AllFacesAtOnce = 0,
        /// <summary>
        /// Instructs Unity to spread the rendering of each face over several frames. Using this option, updating the cubemap will take 14 frames. This option greatly reduces the impact on frame rate, however it may produce incorrect results, especially in Scenes where lighting conditions change over these 14 frames.
        /// </summary>
        IndividualFaces = 1,
        /// <summary>
        /// Unity will render the probe entirely in one frame.
        /// </summary>
        NoTimeSlicing = 2,
    }

    // Match ShadowSamplingMode on C++ side
    /// <summary>
    /// Used by [[CommandBuffer.SetShadowSamplingMode]].
    /// </summary>
    public enum ShadowSamplingMode
    {
        /// <summary>
        /// Default shadow sampling mode: sampling with a comparison filter.
        /// </summary>
        /// <description>
        /// The texture and sampler should be declared with:
        /// @@UNITY_DECLARE_SHADOWMAP(_Shadowmap);@@
        /// and sampled with:
        /// @@UNITY_SAMPLE_SHADOW(_Shadowmap, half3(uv, depth_for_comparison));@@.
        /// </description>
        CompareDepths = 0,
        /// <summary>
        /// Shadow sampling mode for sampling the depth value.
        /// </summary>
        /// <description>
        /// The texture and sampler can be declared with:
        /// @@sampler2D _Shadowmap;@@
        /// and sampled with:
        /// @@tex2D(_Shadowmap, uv).r;@@.
        /// </description>
        RawDepth = 1,
        /// <summary>
        /// In ShadowSamplingMode.None, depths are not compared. Use this value if a Texture is not a shadowmap.
        /// </summary>
        None = 2
    }

    // Match BaseRenderer::LightProbeUsage on C++ side
    /// <summary>
    /// Light probe interpolation type.
    /// </summary>
    /// <description>
    /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]].
    /// </description>
    public enum LightProbeUsage
    {
        /// <summary>
        /// Light Probes are not used. The Scene's ambient probe is provided to the shader.
        /// </summary>
        Off = 0,
        /// <summary>
        /// Simple light probe interpolation is used.
        /// </summary>
        /// <description>
        /// If baked light probes are present in the Scene, an interpolated light probe will be calculated for this object and set as built-in shader uniform variables. Surface shaders use this information automatically. To add light probe contribution to your custom non-surface shaders, use ShadeSH9(worldSpaceNormal) in your vertex or pixel shader.
        /// SA: [[wiki:LightProbes|Light Probes]].
        /// </description>
        BlendProbes = 1,
        /// <summary>
        /// Uses a 3D grid of interpolated light probes.
        /// </summary>
        /// <description>
        /// A __Light Probe Proxy Volume__ component which may reside on the same game object or on another game object will be required. In order to use a __Light Probe Proxy Volume__ component which resides on another game object, you must use the __Proxy Volume Override__ property where you can specify the source game object.
        /// Surface shaders use the information associated with the proxy volume automatically. To use the proxy volume information in your custom shaders, you can use ShadeSHPerPixel function in your pixel shader.
        /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]].
        /// </description>
        UseProxyVolume = 2,
        //ExplicitIndex = 3, // This is internal only so we don't expose it to C#
        /// <summary>
        /// The light probe shader uniform values are extracted from the material property block set on the renderer.
        /// </summary>
        /// <description>
        /// Property /unity_SHAr/, /unity_SHAg/, /unity_SHAb/, /unity_SHBr/, /unity_SHBg/, /unity_SHBb/ and /unity_SHC/ will be set to zero if they are not part of the MaterialPropertyBlock.\\
        /// Property /unity_ProbesOcclusion/ will be calculated as in normal lighting if it is not part of the MaterialPropertyBlock.
        /// Note that using the light probe values baked at a different place may lead to incorrect rendering, especially when local lights (i.e. point lights and spot lights) are used. This mode is more useful when drawing instanced objects with [[Graphics.DrawMeshInstanced]], where the light probe data is pre-calculated and provided as arrays.
        /// SA: [[MaterialPropertyBlock]], [[MaterialPropertyBlock.CopySHCoefficientArraysFrom]], [[MaterialPropertyBlock.CopyProbeOcclusionArrayFrom]].
        /// </description>
        CustomProvided = 4,
    }

    // Match GraphicsSettings::BuiltinShaderType on C++ side
    /// <summary>
    /// Built-in shader types used by [[Rendering.GraphicsSettings]].
    /// </summary>
    /// <description>
    /// SA: [[GraphicsSettings.SetShaderMode]], [[Rendering.BuiltinShaderMode]], [[wiki:class-GraphicsSettings|Graphics Settings]].
    /// </description>
    public enum BuiltinShaderType
    {
        /// <summary>
        /// Shader used for deferred shading calculations.
        /// </summary>
        /// <description>
        /// SA: [[GraphicsSettings.SetShaderMode]], [[Rendering.BuiltinShaderMode]], [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        DeferredShading = 0,
        /// <summary>
        /// Shader used for deferred reflection probes.
        /// </summary>
        /// <description>
        /// When using deferred shading, [[ReflectionProbe|reflection probes]] are rendered in "deferred" way by default.
        /// All deferred objects in the Scene get per-pixel reflection probes that are calculated using this shader.
        /// When setting deferred reflections shader to "disabled" ([[BuiltinShaderMode.Disabled]]), reflection probes are done in
        /// per-object way, similar to how forward rendering computes them.
        /// SA: [[GraphicsSettings.SetShaderMode]], [[Rendering.BuiltinShaderMode]], [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        DeferredReflections = 1,
        /// <summary>
        /// Shader used for legacy deferred lighting calculations.
        /// </summary>
        /// <description>
        /// SA: [[GraphicsSettings.SetShaderMode]], [[Rendering.BuiltinShaderMode]], [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        LegacyDeferredLighting = 2,
        /// <summary>
        /// Shader used for screen-space cascaded shadows.
        /// </summary>
        /// <description>
        /// Cascaded shadow maps for directional lights on PC/console platforms compute a
        /// screenspace shadow mask using this shader.
        /// SA: [[GraphicsSettings.SetShaderMode]], [[Rendering.BuiltinShaderMode]], [[wiki:class-GraphicsSettings|Graphics Settings]], [[wiki:DirLightShadows|Directional Light Shadows]].
        /// </description>
        ScreenSpaceShadows = 3,
        /// <summary>
        /// Shader used for depth and normals texture when enabled on a Camera.
        /// </summary>
        /// <description>
        /// SA: [[Camera.depthTextureMode]], [[DepthTextureMode.DepthNormals]].
        /// </description>
        DepthNormals = 4,
        /// <summary>
        /// Shader used for Motion Vectors when enabled on a Camera.
        /// </summary>
        /// <description>
        /// SA: [[Renderer.motionVectorGenerationMode]], [[Camera.depthTextureMode]], [[SkinnedMeshRenderer.skinnedMotionVectors]], [[PassType.MotionVectors]], [[DepthTextureMode.MotionVectors]], [[SystemInfo.supportsMotionVectors]].
        /// </description>
        MotionVectors = 5,
        /// <summary>
        /// Default shader used for light halos.
        /// </summary>
        /// <description>
        /// This is default shader used for light halos.
        /// </description>
        LightHalo = 6,
        /// <summary>
        /// Default shader used for lens flares.
        /// </summary>
        /// <description>
        /// This is default shader used for lens flares.
        /// </description>
        LensFlare = 7,
    }

    // Match BuiltinShaderSettings::BuiltinShaderMode on C++ side
    /// <summary>
    /// Built-in shader modes used by [[Rendering.GraphicsSettings]].
    /// </summary>
    /// <description>
    /// SA: [[GraphicsSettings.SetShaderMode]], [[Rendering.BuiltinShaderType]], [[wiki:class-GraphicsSettings|Graphics Settings]].
    /// </description>
    public enum BuiltinShaderMode
    {
        /// <summary>
        /// Don't use any shader, effectively disabling the functionality.
        /// </summary>
        /// <description>
        /// This is primarily used as a build size optimization, for example if you know the project never uses deferred shading,
        /// you could disable support for it in [[wiki:class-GraphicsSettings|Graphics Settings]] and save some build data size.
        /// When [[BuiltinShaderType.DeferredReflections]] is disabled, then in deferred shading the reflection probes are done in
        /// per-object way, instead of a separate deferred per-pixel reflections pass.
        /// SA: [[GraphicsSettings.SetShaderMode]], [[Rendering.BuiltinShaderType]], [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        Disabled = 0,
        /// <summary>
        /// Use built-in shader (default).
        /// </summary>
        /// <description>
        /// SA: [[GraphicsSettings.SetShaderMode]], [[Rendering.BuiltinShaderType]], [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        UseBuiltin = 1,
        /// <summary>
        /// Use custom shader instead of built-in one.
        /// </summary>
        /// <description>
        /// This is useful for implementing custom functionality. For example, by default deferred shading does shading calculations
        /// that match the Standard shader lighting model. But if you'd want to use a different BRDF, or use a custom deferred G-buffer
        /// layout, then you'd need to override [[BuiltinShaderType.DeferredShading]] with your own custom shader.
        /// SA: [[GraphicsSettings.SetShaderMode]], [[GraphicsSettings.SetCustomMode]], [[Rendering.BuiltinShaderType]], [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        UseCustom = 2,
    }

    // Match platform_caps_keywords enum on C++ side (Runtime/Graphics/PlatformCapsKeywords.h)
    // with enum names being actual shader defines (from platform_caps_keywords::KeywordDefine)
    /// <summary>
    /// Defines set by editor when compiling shaders, depending on target platform and tier.
    /// </summary>
    /// <description>
    /// SA: [[GraphicsSettings.HasShaderDefine]].
    /// </description>
    public enum BuiltinShaderDefine
    {
        /// <summary>
        /// UNITY_NO_DXT5nm is set when compiling shader for platform that do not support DXT5NM, meaning that normal maps will be encoded in RGB instead.
        /// </summary>
        UNITY_NO_DXT5nm,
        /// <summary>
        /// UNITY_NO_RGBM is set when compiling shader for platform that do not support RGBM, so dLDR will be used instead.
        /// </summary>
        UNITY_NO_RGBM,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        UNITY_USE_NATIVE_HDR,
        /// <summary>
        /// UNITY_ENABLE_REFLECTION_BUFFERS is set when deferred shading renders reflection probes in deferred mode. With this option set reflections are rendered into a per-pixel buffer. This is similar to the way lights are rendered into a per-pixel buffer. UNITY_ENABLE_REFLECTION_BUFFERS is on by default when using deferred shading, but you can turn it off by setting “No support” for the Deferred Reflections shader option in Graphics Settings. When the setting is off, reflection probes are rendered per-object, similar to the way forward rendering works.
        /// </summary>
        /// <description>
        /// SA: [[BuiltinShaderType.DeferredReflections]].
        /// </description>
        UNITY_ENABLE_REFLECTION_BUFFERS,
        /// <summary>
        /// UNITY_FRAMEBUFFER_FETCH_AVAILABLE is set when compiling shaders for platforms where framebuffer fetch is potentially available.
        /// </summary>
        UNITY_FRAMEBUFFER_FETCH_AVAILABLE,
        /// <summary>
        /// UNITY_ENABLE_NATIVE_SHADOW_LOOKUPS enables use of built-in shadow comparison samplers on OpenGL ES 2.0.
        /// </summary>
        UNITY_ENABLE_NATIVE_SHADOW_LOOKUPS,
        /// <summary>
        /// UNITY_METAL_SHADOWS_USE_POINT_FILTERING is set if shadow sampler should use point filtering on iOS Metal.
        /// </summary>
        /// <description>
        /// SA: [[PlayerSettings.iOS.forceHardShadowsOnMetal]].
        /// </description>
        UNITY_METAL_SHADOWS_USE_POINT_FILTERING,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        UNITY_NO_CUBEMAP_ARRAY,
        /// <summary>
        /// UNITY_NO_SCREENSPACE_SHADOWS is set when screenspace cascaded shadow maps are disabled.
        /// </summary>
        /// <description>
        /// SA: [[TierSettings.cascadedShadowMaps]].
        /// </description>
        UNITY_NO_SCREENSPACE_SHADOWS,
        /// <summary>
        /// UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS is set when Semitransparent Shadows are enabled.
        /// </summary>
        /// <description>
        /// SA: [[TierSettings.semitransparentShadows]].
        /// </description>
        UNITY_USE_DITHER_MASK_FOR_ALPHABLENDED_SHADOWS,
        /// <summary>
        /// UNITY_PBS_USE_BRDF1 is set if Standard Shader BRDF1 should be used.
        /// </summary>
        /// <description>
        /// SA: [[TierSettings.standardShaderQuality]].
        /// </description>
        UNITY_PBS_USE_BRDF1,
        /// <summary>
        /// UNITY_PBS_USE_BRDF2 is set if Standard Shader BRDF2 should be used.
        /// </summary>
        /// <description>
        /// SA: [[TierSettings.standardShaderQuality]].
        /// </description>
        UNITY_PBS_USE_BRDF2,
        /// <summary>
        /// UNITY_PBS_USE_BRDF3 is set if Standard Shader BRDF3 should be used.
        /// </summary>
        /// <description>
        /// SA: [[TierSettings.standardShaderQuality]].
        /// </description>
        UNITY_PBS_USE_BRDF3,
        /// <summary>
        /// UNITY_NO_FULL_STANDARD_SHADER is set if Standard shader BRDF3 with extra simplifications should be used.
        /// </summary>
        UNITY_NO_FULL_STANDARD_SHADER,
        /// <summary>
        /// UNITY_SPECCUBE_BLENDING is set if Reflection Probes Box Projection is enabled.
        /// </summary>
        /// <description>
        /// SA: [[TierSettings.reflectionProbeBoxProjection]].
        /// </description>
        UNITY_SPECCUBE_BOX_PROJECTION,
        /// <summary>
        /// UNITY_SPECCUBE_BLENDING is set if Reflection Probes Blending is enabled.
        /// </summary>
        /// <description>
        /// SA: [[TierSettings.reflectionProbeBlending]].
        /// </description>
        UNITY_SPECCUBE_BLENDING,
        /// <summary>
        /// UNITY_ENABLE_DETAIL_NORMALMAP is set if Detail Normal Map should be sampled if assigned.
        /// </summary>
        /// <description>
        /// SA: [[TierSettings.detailNormalMap]].
        /// </description>
        UNITY_ENABLE_DETAIL_NORMALMAP,
        /// <summary>
        /// SHADER_API_MOBILE is set when compiling shader for mobile platforms.
        /// </summary>
        SHADER_API_MOBILE,
        /// <summary>
        /// SHADER_API_DESKTOP is set when compiling shader for "desktop" platforms.
        /// </summary>
        SHADER_API_DESKTOP,
        /// <summary>
        /// UNITY_HARDWARE_TIER1 is set when compiling shaders for [[GraphicsTier.Tier1]].
        /// </summary>
        /// <description>
        /// Shader will have special per-tier variants only if #pragma hardware_tier_variants was used or if [[TierSettings]] for current platform are different (as they impact defines passed to shader compiler).
        /// </description>
        UNITY_HARDWARE_TIER1,
        /// <summary>
        /// UNITY_HARDWARE_TIER2 is set when compiling shaders for [[GraphicsTier.Tier2]].
        /// </summary>
        /// <description>
        /// Shader will have special per-tier variants only if #pragma hardware_tier_variants was used or if [[TierSettings]] for current platform are different (as they impact defines passed to shader compiler).
        /// </description>
        UNITY_HARDWARE_TIER2,
        /// <summary>
        /// UNITY_HARDWARE_TIER3 is set when compiling shaders for [[GraphicsTier.Tier3]].
        /// </summary>
        /// <description>
        /// Shader will have special per-tier variants only if #pragma hardware_tier_variants was used or if [[TierSettings]] for current platform are different (as they impact defines passed to shader compiler).
        /// </description>
        UNITY_HARDWARE_TIER3,
        /// <summary>
        /// UNITY_COLORSPACE_GAMMA is set when compiling shaders for Gamma Color Space.
        /// </summary>
        /// <description>
        /// SA: [[PlayerSettings.colorSpace]].
        /// </description>
        UNITY_COLORSPACE_GAMMA,
        /// <summary>
        /// UNITY_LIGHT_PROBE_PROXY_VOLUME is set when __Light Probe Proxy Volume__ feature is supported by the current graphics API and is enabled in the current Tier Settings(Graphics Settings).
        /// </summary>
        /// <description>
        /// SA: [[LightProbeProxyVolume]], [[TierSettings.enableLPPV]].
        /// </description>
        UNITY_LIGHT_PROBE_PROXY_VOLUME,
        /// <summary>
        /// UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS is set automatically for platforms that don't require full floating-point precision support in fragment shaders.
        /// </summary>
        UNITY_HALF_PRECISION_FRAGMENT_SHADER_REGISTERS,
        /// <summary>
        /// UNITY_LIGHTMAP_DLDR_ENCODING is set when lightmap textures are using double LDR encoding to store the values in the texture.
        /// </summary>
        UNITY_LIGHTMAP_DLDR_ENCODING,
        /// <summary>
        /// UNITY_LIGHTMAP_RGBM_ENCODING is set when lightmap textures are using RGBM encoding to store the values in the texture.
        /// </summary>
        UNITY_LIGHTMAP_RGBM_ENCODING,
        /// <summary>
        /// UNITY_LIGHTMAP_FULL_HDR is set when lightmap textures are not using any encoding to store the values in the texture.
        /// </summary>
        UNITY_LIGHTMAP_FULL_HDR,
    }

    // Match TextureDimension on C++ side
    /// <summary>
    /// Texture "dimension" (type).
    /// </summary>
    /// <description>
    /// Indicates type of a texture (2D texture, cubemap, 3D volume texture etc.).
    /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
    /// </description>
    public enum TextureDimension
    {
        /// <summary>
        /// Texture type is not initialized or unknown.
        /// </summary>
        /// <description>
        /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
        /// </description>
        Unknown = -1,
        /// <summary>
        /// No texture is assigned.
        /// </summary>
        /// <description>
        /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
        /// </description>
        None = 0,
        /// <summary>
        /// Any texture type.
        /// </summary>
        /// <description>
        /// This is a special case value for material properties that can accept any texture type.
        /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
        /// </description>
        Any = 1,
        /// <summary>
        /// 2D texture ([[Texture2D]]).
        /// </summary>
        /// <description>
        /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
        /// </description>
        Tex2D = 2,
        /// <summary>
        /// 3D volume texture ([[Texture3D]]).
        /// </summary>
        /// <description>
        /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
        /// </description>
        Tex3D = 3,
        /// <summary>
        /// [[Cubemap]] texture.
        /// </summary>
        /// <description>
        /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
        /// </description>
        Cube = 4,
        /// <summary>
        /// 2D array texture ([[Texture2DArray]]).
        /// </summary>
        /// <description>
        /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
        /// </description>
        Tex2DArray = 5,
        /// <summary>
        /// Cubemap array texture ([[CubemapArray]]).
        /// </summary>
        /// <description>
        /// SA: [[Texture.dimension]], [[MaterialProperty.textureDimension]].
        /// </description>
        CubeArray = 6,
    }

    // Match CopyTextureSupport on C++ side
    /// <summary>
    /// Support for various [[Graphics.CopyTexture]] cases.
    /// </summary>
    /// <description>
    /// Most modern platforms and graphics APIs support quite flexible texture copy (e.g. copy from a [[RenderTexture]]
    /// into a [[Cubemap]] face). However some older systems might not support certain parts of texture copy functionality.
    /// This enum indicates support for this. Use [[SystemInfo.copyTextureSupport]] to check for support before
    /// calling [[Graphics.CopyTexture]].
    /// Direct3D11, DirectD12 and PS4 platforms generally support flexible texture copy (all CopyTextureSupport flags are set).
    /// OpenGL supports flexible texture copy since OpenGL 4.3; OpenGL ES supports flexible texture copy since OpenGL ES 3.1 aep; on earlier versions there's no copy support right now ([[Rendering.CopyTextureSupport.None]]).
    /// Direct3D9 systems have somewhat limited texture copy support (can't copy 3D textures, and can't copy between
    /// textures and render textures).
    /// Metal and WebGL currently do not have texture copy support ([[Rendering.CopyTextureSupport.None]]).
    /// SA: [[Graphics.CopyTexture]], [[SystemInfo.copyTextureSupport]].
    /// </description>
    [Flags]
    public enum CopyTextureSupport
    {
        /// <summary>
        /// No support for [[Graphics.CopyTexture]].
        /// </summary>
        /// <description>
        /// See [[Rendering.CopyTextureSupport]] for an overview.
        /// </description>
        None = 0,
        /// <summary>
        /// Basic [[Graphics.CopyTexture]] support.
        /// </summary>
        /// <description>
        /// Generally this means that texture copies work between same texture types (e.g. 2D texture to 2D texture, or
        /// cubemap to cubemap). Additional cases that might not necessarily be present are other flags in
        /// [[Rendering.CopyTextureSupport]].
        /// </description>
        Basic = (1 << 0),
        /// <summary>
        /// Support for [[Texture3D]] in [[Graphics.CopyTexture]].
        /// </summary>
        /// <description>
        /// Not all platforms can do 3D texture copies, e.g. currently Direct3D9 systems can not do that. See
        /// [[Rendering.CopyTextureSupport]] for an overview.
        /// </description>
        Copy3D = (1 << 1),
        /// <summary>
        /// Support for [[Graphics.CopyTexture]] between different texture types.
        /// </summary>
        /// <description>
        /// When this flag is set, [[Graphics.CopyTexture]] can do copies between different texture "dimensions",
        /// for example copy a single cubemap face into a regular 2D texture; or copy from a 2D texture into
        /// a slice of 2D Array texture. See [[Graphics.CopyTextureSupport]] for an overview.
        /// </description>
        DifferentTypes = (1 << 2),
        /// <summary>
        /// Support for Texture to RenderTexture copies in [[Graphics.CopyTexture]].
        /// </summary>
        /// <description>
        /// Not all platforms can copy from a [[Texture2D]] into a [[RenderTexture]] directly. For example, Direct3D9
        /// currently can not do this. See [[Rendering.CopyTextureSupport]] for an overview.
        /// </description>
        TextureToRT = (1 << 3),
        /// <summary>
        /// Support for RenderTexture to Texture copies in [[Graphics.CopyTexture]].
        /// </summary>
        /// <description>
        /// Not all platforms can copy from a [[RenderTexture]] into a regular [[Texture2D]] directly. For example, Direct3D9
        /// currently can not do this. See [[Rendering.CopyTextureSupport]] for an overview.
        /// </description>
        RTToTexture = (1 << 4),
    }

    /// <summary>
    /// The HDR mode to use for rendering.
    /// </summary>
    /// <description>
    /// When HDR is enabled for the current Graphics Tier this selects the format to use for the HDR buffer.
    /// SA: [[Rendering.TierSettings.hdr]], [[Camera.allowHDR]].
    /// </description>
    public enum CameraHDRMode
    {
        /// <summary>
        /// Uses [[RenderTextureFormat.ARGBHalf]].
        /// </summary>
        /// <description>
        /// SA: [[Rendering.TierSettings.hdr]], [[Camera.allowHDR]].
        /// </description>
        FP16 = 1,
        /// <summary>
        /// Uses [[RenderTextureFormat.RGB111110Float]].
        /// </summary>
        /// <description>
        /// SA: [[Rendering.TierSettings.hdr]], [[Camera.allowHDR]].
        /// </description>
        R11G11B10 = 2
    }

    /// <summary>
    /// How much CPU usage to assign to the final lighting calculations at runtime.
    /// </summary>
    /// <description>
    /// How many CPU worker threads to create for Realtime Global Illumination lighting calculations in the Player. Increasing this makes the system react faster to changes in lighting at a cost of using more CPU time. The higher the CPU Usage value, the more worker threads are created for solving Realtime GI.\\
    /// \\
    /// __Please note__ that some platforms will allow all CPUs to be occupied by worker threads whilst some have a max limit:\\
    /// __Xbox One:__ 4 CPU cores.\\
    /// __PS4:__ 4 CPU cores.\\
    /// __Android:__ If the device is a bigLittle architecture, only the little CPUs will be used, otherwise it is CPUs - 1.\\
    /// </description>
    public enum RealtimeGICPUUsage
    {
        /// <summary>
        /// 25% of the allowed CPU threads are used as worker threads.
        /// </summary>
        /// <description>
        /// 25% of the total number of allowed logical CPU cores are populated with Enlighten worker threads.
        /// SA: [[Rendering.TierSettings.realtimeGICPUUsage]].
        /// </description>
        Low = 25,
        /// <summary>
        /// 50% of the allowed CPU threads are used as worker threads.
        /// </summary>
        /// <description>
        /// 50% of the total number of allowed logical CPU cores are populated with Enlighten worker threads.
        /// SA: [[Rendering.TierSettings.realtimeGICPUUsage]].
        /// </description>
        Medium = 50,
        /// <summary>
        /// 75% of the allowed CPU threads are used as worker threads.
        /// </summary>
        /// <description>
        /// 75% of the total number of allowed logical CPU cores are populated with Enlighten worker threads.
        /// SA: [[Rendering.TierSettings.realtimeGICPUUsage]].
        /// </description>
        High = 75,
        /// <summary>
        /// 100% of the allowed CPU threads are used as worker threads.
        /// </summary>
        /// <description>
        /// All of the total number of allowed logical CPU cores are populated with Enlighten worker threads.
        /// SA: [[Rendering.TierSettings.realtimeGICPUUsage]].
        /// </description>
        Unlimited = 100
    }

    //Needs to line up with the common elements of the c++ version of this enum found GfxDeviceTypes.h
    /// <summary>
    /// Describes the desired characteristics with respect to prioritisation and load balancing of the queue that a command buffer being submitted via [[Graphics.ExecuteCommandBufferAsync]] or [[ScriptableRenderContext.ExecuteCommandBufferAsync] should be sent to.
    /// </summary>
    public enum ComputeQueueType
    {
        /// <summary>
        /// This queue type would be the choice for compute tasks supporting or as optimisations to graphics processing. [[CommandBuffers]] sent to this queue would be expected to complete within the scope of a single frame and likely be synchronised with the graphics queue via [[GPUFence]]’s. Dispatches on default queue types would execute at a lower priority than graphics queue tasks.
        /// </summary>
        Default = 0,
        /// <summary>
        /// Background queue types would be the choice for tasks intended to run for an extended period of time, e.g for most of a frame or for several frames. Dispatches on background queues would execute at a lower priority than gfx queue tasks.
        /// </summary>
        Background = 1,
        /// <summary>
        /// This queue type would be the choice for compute tasks requiring processing as soon as possible and would be prioritised over the graphics queue.
        /// </summary>
        /// <description>
        /// Note due to the way that Unity internally deferrs it's submission of command buffers to the GPU users should not expect compute shader dispatches sent to Urgent async compute queues to complete and be available on the CPU immediately. On some platforms it is possible for the OS to schedule GPU work that would take priority over urgent async compute tasks.
        /// </description>
        Urgent = 2
    }

    /// <summary>
    /// Enum type defines the different stereo rendering modes available.
    /// </summary>
    public enum SinglePassStereoMode
    {
        /// <summary>
        /// Render stereo using multiple passes.
        /// </summary>
        /// <description>
        /// In multi-pass stereo rendering, the rendering pipeline traverses the scene graph twice, rendering one eye at a time. Scene culling and shadow map rendering can be shared between both eyes.
        /// </description>
        None = 0,
        /// <summary>
        /// Render stereo to the left and right halves of a single, double-width render target.
        /// </summary>
        /// <description>
        /// In side-by-side single-pass stereo rendering, the rendering pipeline traverses the scene graph only once while issuing two draw calls for each render node. Each eye is rendered to one side of the render target. The main render target must be a twice as wide as a single-eye target. Scene culling and shadow map rendering is shared between both eyes. Side-by-side rendering is significantly faster than multi-pass rendering for VR, but is a little slower than instancing or multiview modes.
        /// </description>
        SideBySide,
        /// <summary>
        /// Render stereo using GPU instancing.
        /// </summary>
        /// <description>
        ///                     In instanced single-pass stereo rendering, the rendering pipeline traverses the scene graph only once and issues a single, instanced draw call for each render node, thus reducing the bandwidth required to render the scene. Scene culling and shadow map rendering is shared between both eyes. The main render target must be an array of render targets.
        ///                     Special GPU hardware support is required for this mode to run. Depending on their graphics capabilities, certain GPUs can run this stereo rendering mode and others can run [[Rendering.SinglePassStereoMode.Multiview]]. The two modes are otherwise very similar.
        /// </description>
        Instancing,
        /// <summary>
        /// Render stereo using OpenGL multiview.
        /// </summary>
        /// <description>
        ///                     In multiview single-pass stereo rendering, the rendering pipeline traverses the scene graph only once and issues a single, instanced draw call for each render node, thus reducing the bandwidth required to render the scene. Scene culling and shadow map rendering is shared between both eyes. The main render target must be an array of render targets.
        ///                     Special GPU hardware support is required for this mode to run. Depending on their graphics capabilities, certain GPUs can run this stereo rendering mode and others can run [[Rendering.SinglePassStereoMode.Instancing]]. The two modes are otherwise very similar.
        /// </description>
        Multiview
    }

    //Needs to line up with the common elements of the c++ version of this enum found GfxDeviceTypes.h
    /// <summary>
    /// Flags describing the intention for how the command buffer will be executed. Set these via [[CommandBuffer.SetExecutionFlags]].
    /// </summary>
    public enum CommandBufferExecutionFlags
    {
        /// <summary>
        /// When no flags are specified, the command buffer is considered valid for all means of execution. This is the default for new command buffers.
        /// </summary>
        None = 0,
        /// <summary>
        /// Command buffers flagged for async compute execution will throw exceptions if non-compatible commands are added to them. See [[ScriptableRenderContext.ExecuteCommandBufferAsync]] and [[Graphics.ExecuteCommandBufferAsync]].
        /// </summary>
        AsyncCompute = 1 << 1
    }
} // namespace UnityEngine.Rendering

namespace UnityEngineInternal
{
    internal enum LightmapType
    {
        NoLightmap      = -1,
        StaticLightmap  = 0,
        DynamicLightmap = 1,
    }
}
