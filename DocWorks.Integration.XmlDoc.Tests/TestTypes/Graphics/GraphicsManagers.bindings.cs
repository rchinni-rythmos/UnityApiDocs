using System;
using UnityEngine.Scripting;
using UnityEngine.Bindings;
using uei = UnityEngine.Internal;

using AmbientMode = UnityEngine.Rendering.AmbientMode;
using ReflectionMode = UnityEngine.Rendering.DefaultReflectionMode;

namespace UnityEngine
{
    [NativeHeader("Runtime/Camera/RenderSettings.h")]
    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    [NativeHeader("Runtime/Graphics/QualitySettingsTypes.h")]
    [StaticAccessor("GetRenderSettings()", StaticAccessorType.Dot)]
    public sealed partial class RenderSettings : Object
    {
        private RenderSettings() {}

        /// <summary>
        /// Is fog enabled?
        /// </summary>
        /// <description>
        /// SA: [[RenderSettings.fogMode]].
        /// </description>
        [NativeProperty("UseFog")]         extern public static bool  fog              { get; set; }
        /// <summary>
        /// The starting distance of linear fog.
        /// </summary>
        /// <description>
        /// Fog start and end distances are used by FogMode.Linear fog mode.
        /// SA: [[RenderSettings.fogEndDistance]], [[RenderSettings.fogMode]].
        /// </description>
        [NativeProperty("LinearFogStart")] extern public static float fogStartDistance { get; set; }
        /// <summary>
        /// The ending distance of linear fog.
        /// </summary>
        /// <description>
        /// Fog start and end distances are used by FogMode.Linear fog mode.
        /// SA: [[RenderSettings.fogStartDistance]], [[RenderSettings.fogMode]].
        /// </description>
        [NativeProperty("LinearFogEnd")]   extern public static float fogEndDistance   { get; set; }
        /// <summary>
        /// Fog mode to use.
        /// </summary>
        /// <description>
        /// SA: [[RenderSettings.fog]].
        /// </description>
        extern public static FogMode fogMode    { get; set; }
        /// <summary>
        /// The color of the fog.
        /// </summary>
        extern public static Color   fogColor   { get; set; }
        /// <summary>
        /// The density of the exponential fog.
        /// </summary>
        /// <description>
        /// Fog density is used by FogMode.Exponential and FogMode.ExponentialSquared
        /// modes.
        /// </description>
        /// <description>
        /// SA: [[RenderSettings.fogMode]].
        /// </description>
        extern public static float   fogDensity { get; set; }

        /// <summary>
        /// Ambient lighting mode.
        /// </summary>
        /// <description>
        /// Unity can provide ambient lighting in several modes, for example directional ambient with separate sky, equator and ground colors, or flat ambient with a single color.
        /// SA: [[Rendering.AmbientMode]].
        /// </description>
        extern public static AmbientMode ambientMode   { get; set; }
        /// <summary>
        /// Ambient lighting coming from above.
        /// </summary>
        /// <description>
        /// Trilight ambient lighing mode uses this color to affect upwards-facing object parts.
        /// SA: ambientMode, ambientGroundColor, ambientEquatorColor.
        /// </description>
        extern public static Color ambientSkyColor     { get; set; }
        /// <summary>
        /// Ambient lighting coming from the sides.
        /// </summary>
        /// <description>
        /// Trilight ambient lighing mode uses this color to affect sideways-facing object parts.
        /// In Flat ambient lighting mode, equator color is just the single ambient color, and has the same value as ambientLight.
        /// SA: ambientMode, ambientSkyColor, ambientGroundColor.
        /// </description>
        extern public static Color ambientEquatorColor { get; set; }
        /// <summary>
        /// Ambient lighting coming from below.
        /// </summary>
        /// <description>
        /// Trilight ambient lighing mode uses this color to affect downwards-facing object parts.
        /// SA: ambientMode, ambientSkyColor, ambientEquatorColor.
        /// </description>
        extern public static Color ambientGroundColor  { get; set; }
        /// <summary>
        /// How much the light from the Ambient Source affects the Scene.
        /// </summary>
        extern public static float ambientIntensity    { get; set; }
        /// <summary>
        /// Flat ambient lighting color.
        /// </summary>
        /// <description>
        /// Flat ambient lighting mode uses color. It has the same value as ambientSkyColor.
        /// </description>
        /// <description>
        /// SA: ambientMode, [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        [NativeProperty("AmbientSkyColor")] extern public static Color ambientLight { get; set; }

        /// <summary>
        /// The color used for the sun shadows in the Subtractive lightmode.
        /// </summary>
        extern public static Color subtractiveShadowColor { get; set; }

        /// <summary>
        /// The global skybox to use.
        /// </summary>
        /// <description>
        /// If you change the skybox in playmode, you have to use the  [[DynamicGI.UpdateEnvironment]]  function call to update the ambient probe.
        /// </description>
        [NativeProperty("SkyboxMaterial")] extern static public Material skybox { get; set; }
        /// <summary>
        /// The light used by the procedural skybox.
        /// </summary>
        /// <description>
        /// If none, the brightest directional light is used.
        /// </description>
        extern public static Light sun { get; set; }
        /// <summary>
        /// Custom or skybox ambient lighting data.
        /// </summary>
        /// <description>
        /// Skybox ambient lighting mode uses this Spherical Harmonics (SH) probe to calculate ambient. You can also assign a completely custom SH probe this way.
        /// The GI system will bake the ambient probe, but it actually won't be used on geometry that uses light probes or lightmaps, as the environment lighting is already in the light probes and the lightmaps. It is used as the last fallback if light probes or lightmaps are not present or enabled for an object.
        /// Adjusting the ambient probe will not affect the input to realtime and baked Global Illumination. If you want to adjust ambient in a way that affects GI, adjust ambient through [[RenderSettings.ambientMode]], for instance by using [[AmbientMode.Trilight]]. The GI system will output the resulting ambient values into the ambient probe, which means that a custom ambient probe can be overwritten by the GI system.
        /// SA: ambientMode, [[Rendering.SphericalHarmonicsL2]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        extern public static Rendering.SphericalHarmonicsL2 ambientProbe { get; set; }

        /// <summary>
        /// Custom specular reflection cubemap.
        /// </summary>
        /// <description>
        /// Specifies a cubemap for use as a default specular reflection.
        /// SA: defaultReflectionMode.
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// // This example creates and uses a real-time Reflection Probe to update a Cubemap. The Cubemap is then used as a default specular reflection.
        /// public class UpdateDefaultReflection : MonoBehaviour
        /// {
        ///     private ReflectionProbe probeComponent = null;
        ///     private Cubemap cubemap = null;
        ///     private int renderId = -1;
        ///     void Start()
        ///     {
        ///         GameObject probeGameObject = new GameObject("Default Reflection Probe");
        ///         // Use a location such that the new Reflection Probe will not interfere with other Reflection Probes in the Scene.
        ///         probeGameObject.transform.position = new Vector3(0, -1000, 0);
        ///         // Create a Reflection Probe that only contains the Skybox. The Update function controls the Reflection Probe refresh.
        ///         probeComponent = probeGameObject.AddComponent<ReflectionProbe>() as ReflectionProbe;
        ///         probeComponent.resolution = 256;
        ///         probeComponent.size = new Vector3(1, 1, 1);
        ///         probeComponent.cullingMask = 0;
        ///         probeComponent.clearFlags = UnityEngine.Rendering.ReflectionProbeClearFlags.Skybox;
        ///         probeComponent.mode = UnityEngine.Rendering.ReflectionProbeMode.Realtime;
        ///         probeComponent.refreshMode = UnityEngine.Rendering.ReflectionProbeRefreshMode.ViaScripting;
        ///         probeComponent.timeSlicingMode = UnityEngine.Rendering.ReflectionProbeTimeSlicingMode.NoTimeSlicing;
        ///         // A cubemap is used as a default specular reflection.
        ///         cubemap = new Cubemap(probeComponent.resolution, probeComponent.hdr ? TextureFormat.RGBAHalf : TextureFormat.RGBA32, true);
        ///     }
        ///     // The Update function refreshes the Reflection Probe and copies the result to the default specular reflection Cubemap.
        ///     void Update()
        ///     {
        ///         // The texture associated with the real-time Reflection Probe is a render target and RenderSettings.customReflection is a Cubemap. We have to check the support if copying from render targets to Textures is supported.
        ///         if ((SystemInfo.copyTextureSupport & UnityEngine.Rendering.CopyTextureSupport.RTToTexture) != 0)
        ///         {
        ///             // Wait until previous RenderProbe is finished before we refresh the Reflection Probe again.
        ///             // renderId is a token used to figure out when the refresh of a Reflection Probe is finished. The refresh of a Reflection Probe can take mutiple frames when time-slicing is used.
        ///             if (renderId == -1 || probeComponent.IsFinishedRendering(renderId))
        ///             {
        ///                 if (probeComponent.IsFinishedRendering(renderId))
        ///                 {
        ///                     // After the previous RenderProbe is finished, we copy the probe's texture to the cubemap and set it as a custom reflection in RenderSettings.
        ///                     Graphics.CopyTexture(probeComponent.texture, cubemap as Texture);
        ///                     RenderSettings.customReflection = cubemap;
        ///                     RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
        ///                 }
        ///                 renderId = probeComponent.RenderProbe();
        ///             }
        ///         }
        ///     }
        /// }
        /// </dw-legacy-code>
        extern public static Cubemap        customReflection            { get; set; }
        /// <summary>
        /// How much the skybox / custom cubemap reflection affects the Scene.
        /// </summary>
        extern public static float          reflectionIntensity         { get; set; }
        /// <summary>
        /// The number of times a reflection includes other reflections.
        /// </summary>
        /// <description>
        /// Defines in how many passes reflections are calculated. In a given pass, the Scene is rendered into a cubemap with the reflections calculated in the previous pass applied to reflective objects.
        /// If set to 1, the Scene will be rendered once, which means that a reflection will not be able to reflect another reflection and reflective objects will show up black, when seen in other reflective surfaces.
        /// If set to 2, the Scene will be rendered twice and reflective objects will show reflections from the first pass, when seen in other reflective surfaces.
        /// </description>
        extern public static int            reflectionBounces           { get; set; }
        /// <summary>
        /// Default reflection mode.
        /// </summary>
        /// <description>
        /// Unity can use a custom texture or generate a specular reflection texture from skybox.
        /// SA: [[RenderSettings.defaultReflectionMode]], [[wiki:GlobalIllumination|Lighting Window]].
        /// </description>
        extern public static ReflectionMode defaultReflectionMode       { get; set; }
        /// <summary>
        /// Cubemap resolution for default reflection.
        /// </summary>
        extern public static int            defaultReflectionResolution { get; set; }

        /// <summary>
        /// Size of the [[Light]] halos.
        /// </summary>
        /// <description>
        /// For any light, the size of the halo is this value multiplied by [[Light.range]].
        /// </description>
        extern public static float haloStrength   { get; set; }
        /// <summary>
        /// The intensity of all flares in the Scene.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-LensFlare|LensFlare component]].
        /// </description>
        extern public static float flareStrength  { get; set; }
        /// <summary>
        /// The fade speed of all flares in the Scene.
        /// </summary>
        /// <description>
        /// SA: [[wiki:GlobalIllumination|Lighting Window]], [[wiki:class-LensFlare|LensFlare component]].
        /// </description>
        extern public static float flareFadeSpeed { get; set; }

        [FreeFunction("GetRenderSettings")] extern internal static Object GetRenderSettings();
        [StaticAccessor("RenderSettingsScripting", StaticAccessorType.DoubleColon)] extern internal static void Reset();
    }

    [NativeHeader("Runtime/Graphics/QualitySettings.h")]
    [StaticAccessor("GetQualitySettings()", StaticAccessorType.Dot)]
    public sealed partial class QualitySettings : Object
    {
        private QualitySettings() {}

        /// <summary>
        /// The maximum number of pixel lights that should affect any object.
        /// </summary>
        /// <description>
        /// If there are more lights illuminating an object, the dimmest ones will be rendered
        /// as vertex lights.
        /// Use this from scripting if you want to have finer control than offered by quality settings
        /// levels.
        /// </description>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static int pixelLightCount { get; set; }

        /// <summary>
        /// Realtime Shadows type to be used.
        /// </summary>
        /// <description>
        /// This determines which type of shadows should be used. The available options are Hard and Soft Shadows, Hard Shadows Only and Disable Shadows.
        /// SA: [[QualitySettings.shadows]], [[Light.shadows]].
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// // Let the shadow quality be selected
        /// public class ExampleScript : MonoBehaviour
        /// {
        ///     private int selection = 0;
        ///     private string[] itemName = {"   Disable   ", "   HardOnly   ", "   Hard and Soft   "};
        ///     void Start()
        ///     {
        ///         QualitySettings.shadows = ShadowQuality.All;
        ///     }
        ///     void OnGUI()
        ///     {
        ///         GUILayout.BeginVertical("Box");
        ///         selection = GUILayout.SelectionGrid(selection, itemName, 1, GUILayout.MinWidth(200), GUILayout.MinHeight(100));
        ///         GUILayout.EndVertical();
        ///         switch (selection)
        ///         {
        ///             case 0: QualitySettings.shadows = ShadowQuality.Disable; break;
        ///             case 1: QualitySettings.shadows = ShadowQuality.HardOnly; break;
        ///             default: QualitySettings.shadows = ShadowQuality.All; break;
        ///         }
        ///     }
        /// }
        /// </dw-legacy-code>
        [NativeProperty("ShadowQuality")] extern public static ShadowQuality shadows { get; set; }
        /// <summary>
        /// Directional light shadow projection.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static ShadowProjection shadowProjection      { get; set; }
        /// <summary>
        /// Number of cascades to use for directional light shadows.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static int              shadowCascades        { get; set; }
        /// <summary>
        /// Shadow drawing distance.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static float            shadowDistance        { get; set; }
        /// <summary>
        /// The default resolution of the shadow maps.
        /// </summary>
        /// <description>
        /// SA: [[Light.shadowResolution]], [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        [NativeProperty("ShadowResolution")] extern public static ShadowResolution shadowResolution      { get; set; }
        /// <summary>
        /// The rendering mode of Shadowmask.
        /// </summary>
        /// <description>
        /// Set whether static shadow casters should be rendered into realtime shadow maps.
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        [NativeProperty("ShadowmaskMode")] extern public static ShadowmaskMode   shadowmaskMode        { get; set; }
        /// <summary>
        /// Offset shadow frustum near plane.
        /// </summary>
        /// <description>
        /// Offset shadow near plane to account for large triangles being distorted by shadow pancaking.
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static float            shadowNearPlaneOffset { get; set; }
        /// <summary>
        /// The normalized cascade distribution for a 2 cascade setup. The value defines the position of the cascade with respect to Zero.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static float            shadowCascade2Split   { get; set; }
        /// <summary>
        /// The normalized cascade start position for a 4 cascade setup. Each member of the vector defines the normalized position of the coresponding cascade with respect to Zero.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static Vector3          shadowCascade4Split   { get; set; }

        /// <summary>
        /// Global multiplier for the LOD's switching distance.
        /// </summary>
        /// <description>
        /// A larger value leads to a longer view distance before a lower resolution LOD is picked.
        /// </description>
        [NativeProperty("LODBias")] extern public static float lodBias { get; set; }
        /// <summary>
        /// Global anisotropic filtering mode.
        /// </summary>
        /// <description>
        /// SA: [[AnisotropicFiltering]].
        /// </description>
        [NativeProperty("AnisotropicTextures")] extern public static AnisotropicFiltering anisotropicFiltering { get; set; }

        /// <summary>
        /// A texture size limit applied to all textures.
        /// </summary>
        /// <description>
        /// Setting this to one uses the first mipmap of each texture (so all textures are half size),
        /// setting this to two uses the second mipmap of each texture (so all textures are quarter size), etc..
        /// This can be used to decrease video memory requirements on low-end computers.
        /// The default value is zero.
        /// </description>
        extern public static int   masterTextureLimit    { get; set; }
        /// <summary>
        /// A maximum LOD level. All LOD groups.
        /// </summary>
        extern public static int   maximumLODLevel       { get; set; }
        /// <summary>
        /// Budget for how many ray casts can be performed per frame for approximate collision testing.
        /// </summary>
        extern public static int   particleRaycastBudget { get; set; }
        /// <summary>
        /// Should soft blending be used for particles?
        /// </summary>
        extern public static bool  softParticles         { get; set; }
        /// <summary>
        /// Use a two-pass shader for the vegetation in the terrain engine.
        /// </summary>
        /// <description>
        /// If enabled, vegetation will have smoothed edges, if disabled all plants
        /// will have hard edges but are rendered roughly twice as fast.
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static bool  softVegetation        { get; set; }
        /// <summary>
        /// The VSync Count.
        /// </summary>
        /// <description>
        /// The number of VSyncs that should pass between each frame. Use 'Don't Sync' (0) to not wait for VSync.
        /// Value must be 0, 1, 2, 3, or 4.
        /// </description>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static int   vSyncCount            { get; set; }
        /// <summary>
        /// Set The AA Filtering option.
        /// </summary>
        /// <description>
        /// Anti-aliasing value indicates the number of samples per pixel. If unsupported by the hardware or rendering API, the greatest supported number of samples less than the indicated number is used.
        /// </description>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static int   antiAliasing          { get; set; }
        /// <summary>
        /// Async texture upload provides timesliced async texture upload on the render thread with tight control over memory and timeslicing. There are no allocations except for the ones which driver has to do. To read data and upload texture data a ringbuffer whose size can be controlled is re-used.
        /// Use asyncUploadTimeSlice to set the time-slice in milliseconds for asynchronous texture uploads per
        /// frame. Minimum value is 1 and maximum is 33.
        /// </summary>
        /// <description>
        /// SA: [[wiki:AsyncTextureUpload|Asynchronous Texture Upload]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// public class StartupExampleScript : MonoBehaviour
        /// {
        ///     void Start()
        ///     {
        ///         // Set Time Slice to 2 ms
        ///         QualitySettings.asyncUploadTimeSlice = 2;
        ///     }
        /// }
        /// </dw-legacy-code>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static int   asyncUploadTimeSlice  { get; set; }
        /// <summary>
        /// Async texture upload provides timesliced async texture upload on the render thread with tight control over memory and timeslicing. There are no allocations except for the ones which driver has to do. To read data and upload texture data a ringbuffer whose size can be controlled is re-used.
        /// Use asyncUploadBufferSize to set the buffer size for asynchronous texture uploads. The size is in megabytes. Minimum value is 2 and maximum is 512. Although the buffer will resize automatically to fit the largest texture currently loading, it is recommended to set the value approximately to the size of biggest texture used in the Scene to avoid re-sizing of the buffer which can incur performance cost.
        /// </summary>
        /// <description>
        /// SA: [[wiki:AsyncTextureUpload|Asynchronous Texture Upload]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// public class StartupExampleScript : MonoBehaviour
        /// {
        ///     void Start()
        ///     {
        ///         // Set Ring Buffer Size to 16 MB.
        ///         QualitySettings.asyncUploadBufferSize = 16;
        ///     }
        /// }
        /// </dw-legacy-code>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static int   asyncUploadBufferSize { get; set; }
        /// <summary>
        /// This flag controls if the async upload pipeline's ring buffer remains allocated when there are no active loading operations.
        /// Set this to true, to make the ring buffer allocation persist after all upload operations have completed.
        /// If you have issues with excessive memory usage, you can set this to false. This means you reduce the runtime memory footprint, but memory fragmentation can occur.
        /// The default value is true.
        /// </summary>
        /// <description>
        /// SA: [[wiki:AsyncTextureUpload|Asynchronous Texture Upload]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// public class StartupExampleScript : MonoBehaviour
        /// {
        ///     void Start()
        ///     {
        ///         // The upload buffer will be deallocated when all uploads are complete.
        ///         QualitySettings.asyncUploadPersistentBuffer = false;
        ///     }
        /// }
        /// </dw-legacy-code>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static bool  asyncUploadPersistentBuffer { get; set; }


        /// <summary>
        /// Enables realtime reflection probes.
        /// </summary>
        /// <description>
        /// If disabled, realtime reflection probes will not be baked.
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static bool  realtimeReflectionProbes         { get; set; }
        /// <summary>
        /// If enabled, billboards will face towards camera position rather than camera orientation.
        /// </summary>
        extern public static bool  billboardsFaceCameraPosition     { get; set; }
        /// <summary>
        /// In resolution scaling mode, this factor is used to multiply with the target Fixed DPI specified to get the actual Fixed DPI to use for this quality setting.
        /// </summary>
        /// <description>
        /// SA: [[wiki:HOWTO-UIMultiResolution|Multi-Resolution UI]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// public class StartupExampleScript : MonoBehaviour
        /// {
        ///     void Start()
        ///     {
        ///         // Set the target Fixed DPI for this quality setting to be half of the default.
        ///         QualitySettings.resolutionScalingFixedDPIFactor = 0.5f;
        ///     }
        /// }
        /// </dw-legacy-code>
        /// <description>
        /// SA: [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static float resolutionScalingFixedDPIFactor  { get; set; }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("blendWeights is obsolete. Use skinWeights instead (UnityUpgradable) -> skinWeights", true)]
        extern public static BlendWeights blendWeights   { [NativeName("GetSkinWeights")] get; [NativeName("SetSkinWeights")] set; }

        /// <summary>
        /// Skin weights.
        /// </summary>
        /// <description>
        /// Maximum number of bone weights that can affect one vertex. The value can be either One Bone, Two Bones, Four Bones or Unlimited.
        /// Meshes with more than 4 bones per vertex require Compute Shader support for GPU Skinning. This may impact performance on platforms without Compute Shader support, which have to process the vertices on the CPU.
        /// </description>
        /// <description>
        /// SA: [[ModelImporter.maxBonesPerVertex]], [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        extern public static SkinWeights skinWeights { get; set; }

    #if ENABLE_TEXTURE_STREAMING
        /// <summary>
    /// Enable automatic streaming of texture mipmap levels based on their distance from all active cameras.
    /// </summary>
    /// <description>
    /// SA: [[SystemInfo.supportsMipStreaming]], [[StreamingController]], [[TextureImporter.streamingMipmaps]], [[QualitySettings.streamingMipmapsFeatureEnabled]].
    /// </description>
    extern public static bool streamingMipmapsActive { get; set; }
        /// <summary>
        /// The total amount of memory to be used by streaming and non-streaming textures.
        /// </summary>
        /// <description>
        /// Non-streaming textures will always be loaded at the largest mipmap level (even if that exceeds the budget). Streaming textures will pick the smallest mipmap level possible to try to hit the memory budget.
        /// </description>
        extern public static float streamingMipmapsMemoryBudget { get; set; }
        #if UNITY_EDITOR
        /// <summary>
        /// Number of renderers used to process each frame during the calculation of desired mipmap levels for the associated textures.
        /// </summary>
        /// <description>
        /// Lower this value to reduce the CPU cost when using this feature, at the expense of reducing the rate of texture mipmap computation and loading of those desired mipmaps.
        /// </description>
        extern public static int streamingMipmapsRenderersPerFrame { get; set; }
        #else
        extern public static int streamingMipmapsRenderersPerFrame { get; }
        #endif
        /// <summary>
        /// The maximum number of mipmap levels to discard for each texture.
        /// </summary>
        /// <description>
        /// This is also used as the number of mipmaps to discard when first loaded before texture streaming begins to bring in the desired mipmap levels based on camera calculations.
        /// </description>
        extern public static int streamingMipmapsMaxLevelReduction { get; set; }
        /// <summary>
        /// Process all enabled Cameras for texture streaming (rather than just those with StreamingController components).
        /// </summary>
        /// <description>
        /// Set to True to process all enabled Cameras for texture streaming. This is a quick way to set up an existing project.
        /// Set to False to process only those Cameras that have active StreamingController components. This allows more fine grain control.
        /// StreamingController components are always considered in the streaming system. These are considered active locations in the following cases:\\
        /// - Camera and StreamingController are enabled.\\
        /// - Camera is disabled but StreamingController is in a preloading state.\\
        /// SA: [[StreamingController]].
        /// </description>
        extern public static bool streamingMipmapsAddAllCameras { get; set; }
        /// <summary>
        /// The maximum number of active texture file IO requests from the texture streaming system.
        /// </summary>
        /// <description>
        /// This value limits the maximum number of texture file IO requests, from the texture streaming system, that are active at any one time. If the Scene texture content changes significantly and rapidly, the system may attempt to load more texture mipmaps than the file IO can keep up with. Lowering this value will reduce the IO bandwidth generated by the streaming system, resulting in a more rapid response than changing mipmap requirements.
        /// </description>
        extern public static int streamingMipmapsMaxFileIORequests { get; set; }
    #endif

        /// <summary>
        /// Maximum number of frames queued up by graphics driver.
        /// </summary>
        /// <description>
        /// Graphics drivers can queue up frames to be rendered. When CPU has much less work to do than the graphics card,
        /// is it possible for this queue to become quite large. In those cases, user's input will "lag behind"
        /// what is on the screen.
        /// Use [[QualitySettings.maxQueuedFrames]] to limit maximum number of frames that are queued. On PC, the default value is 2,
        /// which strikes the best balance between frame latency and framerate.
        /// Note that you can reduce input latency by using a smaller maxQueuedFrames because the CPU will be waiting until the graphics card finishes rendering previous frames. The downside of this however, is that it can result in a lower framerate.
        /// Currently maxQueuedFrames is implemented in Direct3D 9, Direct3D 11, and NVN graphics APIs; it will be ignored by other graphics API.
        /// </description>
        [StaticAccessor("QualitySettingsScripting", StaticAccessorType.DoubleColon)] extern public static int maxQueuedFrames { get; set; }

        /// <summary>
        /// Returns the current graphics quality level.
        /// </summary>
        /// <description>
        /// SA: SetQualityLevel.
        /// </description>
        [NativeName("GetCurrentIndex")] extern public static int  GetQualityLevel();
        /// <summary>
        /// Sets a new graphics quality level.
        /// </summary>
        /// <param name="index">
        /// Quality index to set.
        /// </param>
        /// <param name="applyExpensiveChanges">
        /// Should expensive changes be applied (Anti-aliasing etc).
        /// </param>
        /// <description>
        /// The list of quality levels can be found by going to __Edit__ > __Project Settings__ > __Quality__. You can add, remove or edit these.
        /// </description>
        /// <description>
        /// Note that changing the quality level can be an expensive operation if the new level
        /// has different anti-aliasing setting. It's fine to change the level when applying in-game
        /// quality options, but if you want to dynamically adjustquality level at runtime, pass
        /// false to applyExpensiveChanges so that expensive changes are not always applied.
        /// When building a player quality levels that are not used for that platform are stripped.
        /// You should not expect a given quality setting to be at a given index. It's best to query
        /// the available quality settings and use the returned index.
        /// SA: GetQualityLevel.
        /// </description>
        [NativeName("SetCurrentIndex")] extern public static void SetQualityLevel(int index, [uei.DefaultValue("true")] bool applyExpensiveChanges);

        /// <summary>
        /// The indexed list of available Quality Settings.
        /// </summary>
        [NativeProperty("QualitySettingsNames")] extern public static string[] names { get; }
    }

    // both desiredColorSpace/activeColorSpace should be deprecated
    [NativeHeader("Runtime/Misc/PlayerSettings.h")]
    public sealed partial class QualitySettings : Object
    {
        /// <summary>
        /// Desired color space (RO).
        /// </summary>
        /// <description>
        /// This is the desired color space based on player settings ([[PlayerSettings.colorSpace]]). Note that if the platform
        /// or hardware does not support the given color space, the actually used color space might end up being
        /// different. Use activeColorSpace to check for that.
        /// SA: [[wiki:LinearLighting|Linear and Gamma rendering]], [[ColorSpace]], activeColorSpace.
        /// </description>
        extern public static ColorSpace desiredColorSpace
        {
            [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)][NativeName("GetColorSpace")] get;
        }
        /// <summary>
        /// Active color space (RO).
        /// </summary>
        /// <description>
        /// This is the active color space based on player settings ([[PlayerSettings.colorSpace]]) and hardware support.
        /// Note that the requested color space might have been different.
        /// SA: [[wiki:LinearLighting|Linear and Gamma rendering]], [[ColorSpace]], desiredColorSpace.
        /// </description>
        extern public static ColorSpace activeColorSpace
        {
            [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)][NativeName("GetColorSpace")] get;
        }
    }
}

namespace UnityEditor.Experimental
{
    /// <summary>
    /// Experimental render settings features.
    /// </summary>
    /// <description>
    /// SA: [[RenderSettings]].
    /// </description>
    [NativeHeader("Runtime/Camera/RenderSettings.h")]
    [StaticAccessor("GetRenderSettings()", StaticAccessorType.Dot)]
    public sealed partial class RenderSettings : Object
    {
        /// <summary>
        /// If enabled, ambient trilight will be sampled using the old radiance sampling method.
        /// </summary>
        /// <description>
        /// This feature is here to maintain backwards compatibility for realtime ambient light when using the trilight mode. The new default behavior now uses the correct sampling behavior.
        /// </description>
        extern public static bool useRadianceAmbientProbe { get; set; }
    }
}
