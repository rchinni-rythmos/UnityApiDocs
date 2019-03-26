using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
    /// <summary>
    /// The portal for dynamically changing occlusion at runtime.
    /// </summary>
    [NativeHeader("Runtime/Camera/OcclusionPortal.h")]
    public sealed partial class OcclusionPortal : Component
    {
        /// <summary>
        /// Gets / sets the portal's open state.
        /// </summary>
        [NativeProperty("IsOpen")] public extern bool open { get; set; }
    }

    /// <summary>
    /// OcclusionArea is an area in which occlusion culling is performed.
    /// </summary>
    [NativeHeader("Runtime/Camera/OcclusionArea.h")]
    public sealed partial class OcclusionArea : Component
    {
        /// <summary>
        /// Center of the occlusion area relative to the transform.
        /// </summary>
        public extern Vector3 center { get; set; }
        /// <summary>
        /// Size that the occlusion area will have.
        /// </summary>
        public extern Vector3 size { get; set; }
    }

    /// <summary>
    /// A flare asset. Read more about flares in the [[wiki:class-Flare|components reference]].
    /// </summary>
    /// <description>
    /// The flare class has no properties. It needs to be [[wiki:class-Flare|setup]] up in the inspector.
    /// You can reference flares and assign them to a [[Light]] at runtime.
    /// SA: [[wiki:class-Flare|Flare assets]], [[LensFlare]] class.
    /// </description>
    [NativeHeader("Runtime/Camera/Flare.h")]
    public sealed partial class Flare : Object
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Flare()
        {
            Internal_Create(this);
        }

        extern static void Internal_Create([Writable] Flare self);
    }

    /// <summary>
    /// Script interface for a [[wiki:class-LensFlare|Lens flare component]].
    /// </summary>
    /// <description>
    /// This allows you to change the brightness and color of lens flares at runtime.
    /// </description>
    [NativeHeader("Runtime/Camera/Flare.h")]
    public sealed partial class LensFlare : Behaviour
    {
        /// <summary>
        /// The strength of the flare.
        /// </summary>
        /// <description>
        /// This controls the size and brightness of the flare elements.
        /// SA: [[wiki:class-LensFlare|Lens flare component]], [[wiki:class-Flare|flare assets]].
        /// </description>
        extern public float brightness { get; set; }
        /// <summary>
        /// The fade speed of the flare.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-LensFlare|Lens flare component]], [[wiki:class-Flare|flare assets]].
        /// </description>
        extern public float fadeSpeed  { get; set; }
        /// <summary>
        /// The color of the flare.
        /// </summary>
        /// <description>
        /// This controls the color of some flare elements (the ones that have ''use light color'' enabled).
        /// SA: [[wiki:class-LensFlare|Lens flare component]], [[wiki:class-Flare|flare assets]].
        /// </description>
        extern public Color color      { get; set; }
        /// <summary>
        /// The [[wiki:class-Flare|flare asset]] to use.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-LensFlare|Lens flare component]], [[wiki:class-Flare|flare assets]].
        /// </description>
        extern public Flare flare      { get; set; }
    }

    /// <summary>
    /// A script interface for a [[wiki:class-Projector|projector component]].
    /// </summary>
    /// <description>
    /// The Projector can be used to project any material onto the Scene - just like a real world projector.
    /// The properties exposed by this class are an exact match for the values in the Projector's inspector.
    /// It can be used to implement blob or projected shadows. You could also project an animated texture or
    /// a render texture that films another part of the Scene. The projector will render all objects in
    /// its view frustum with the provided material.
    /// There is no shortcut property in [[GameObject]] or [[Component]] to access the Projector, so you must
    /// use Component.GetComponent to do it:
    /// </description>
    /// <description>
    /// SA: [[wiki:class-Projector|projector component]].
    /// </description>
    [NativeHeader("Runtime/Camera/Projector.h")]
    public sealed partial class Projector : Behaviour
    {
        /// <summary>
        /// The near clipping plane distance.
        /// </summary>
        /// <description>
        /// The projector will not affect anything that is nearer than this distance.
        /// SA: [[wiki:class-Projector|projector component]].
        /// </description>
        extern public float nearClipPlane    { get; set; }
        /// <summary>
        /// The far clipping plane distance.
        /// </summary>
        /// <description>
        /// The projector will not affect anything that is further than this distance.
        /// SA: [[wiki:class-Projector|projector component]].
        /// </description>
        extern public float farClipPlane     { get; set; }
        /// <summary>
        /// The field of view of the projection in degrees.
        /// </summary>
        /// <description>
        /// This is the vertical field of view; horizontal FOV varies depending on the aspectRatio.
        /// Field of view is ignored when projector is orthographic (see orthographic).
        /// SA: [[wiki:class-Projector|projector component]].
        /// </description>
        extern public float fieldOfView      { get; set; }
        /// <summary>
        /// The aspect ratio of the projection.
        /// </summary>
        /// <description>
        /// This is projection width divided by height. An aspect ratio of 1.0
        /// makes the projection square; a ratio of 2.0 makes it twice as wide
        /// than high.
        /// SA: [[wiki:class-Projector|projector component]].
        /// </description>
        extern public float aspectRatio      { get; set; }
        /// <summary>
        /// Is the projection orthographic (''true'') or perspective (''false'')?
        /// </summary>
        /// <description>
        /// When orthographic is ''true'', projection is defined by orthographicSize.\\
        /// When orthographic is ''false'', projection is defined by fieldOfView.
        /// SA: [[wiki:class-Projector|projector component]].
        /// </description>
        extern public bool  orthographic     { get; set; }
        /// <summary>
        /// Projection's half-size when in orthographic mode.
        /// </summary>
        /// <description>
        /// This is half of the vertical size of the projection volume. Horizontal projection
        /// size varies depending on aspectRatio.
        /// Orthographic size is ignored when projection is not orthographic (see orthographic).
        /// SA: [[wiki:class-Projector|projector component]].
        /// </description>
        extern public float orthographicSize { get; set; }
        /// <summary>
        /// Which object layers are ignored by the projector.
        /// </summary>
        /// <description>
        /// See [[wiki:Layers|layer mask]].
        /// By default this is zero - i.e. no layers are ignored. Each set bit
        /// in /ignoreLayers/ will make this layer not affected by the projector.
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// public class Example : MonoBehaviour
        /// {
        ///     void Start()
        ///     {
        ///         Projector proj = GetComponent<Projector>();
        ///         // Make the projector ignore Default (0) layer
        ///         proj.ignoreLayers = (1 << 0);
        ///     }
        /// }
        /// </dw-legacy-code>
        /// <description>
        /// SA: [[wiki:class-Projector|projector component]], [[wiki:Layers|Layers]].
        /// </description>
        extern public int   ignoreLayers     { get; set; }

        /// <summary>
        /// The material that will be projected onto every object.
        /// </summary>
        /// <description>
        /// Note that unlike [[Renderer.material]], this returns a shared material reference and not a unique duplicate.
        /// Projector does nothing if it has no material set up.
        /// The <a href="https://assetstore.unity.com/packages/essentials/asset-packs/standard-assets-32351">Standard Assets </a> package
        /// contains an example of Projector's material in ''Blob-Shadow'' folder.
        /// SA: [[wiki:class-Projector|projector component]], [[Material]] class.
        /// </description>
        extern public Material material { get; set; }
    }

    /// <summary>
    /// Struct describing the result of a Global Illumination bake for a given light.
    /// </summary>
    /// <description>
    /// The example below demonstrates how you can check the baked status of a light and change its active state.
    /// </description>
    /// <dw-legacy-code>
    /// using UnityEngine;
    /// using System.Collections;
    /// public class LightBakingOutputExample : MonoBehaviour
    /// {
    ///     void TurnOffLight(Light light)
    ///     {
    ///         if (light.bakingOutput.isBaked && light.bakingOutput.lightmapBakeType != LightmapBakeType.Realtime)
    ///         {
    ///             Debug.Log("Light got some contribution statically baked, it cannot be turned off at runtime.");
    ///         }
    ///         else
    ///         {
    ///             light.enabled = false;
    ///         }
    ///     }
    /// }
    /// </dw-legacy-code>
    [NativeHeader("Runtime/Camera/SharedLightData.h")]
    public struct LightBakingOutput
    {
        /// <summary>
        /// In case of a [[LightmapBakeType.Mixed]] light, contains the index of the light as seen from the occlusion probes point of view if any, otherwise -1.
        /// </summary>
        public int probeOcclusionLightIndex;
        /// <summary>
        /// In case of a [[LightmapBakeType.Mixed]] light, contains the index of the occlusion mask channel to use if any, otherwise -1.
        /// </summary>
        public int occlusionMaskChannel;
        /// <summary>
        /// This property describes what part of a light's contribution was baked.
        /// </summary>
        [NativeName("lightmapBakeMode.lightmapBakeType")]
        public LightmapBakeType lightmapBakeType;
        /// <summary>
        /// In case of a [[LightmapBakeType.Mixed]] light, describes what Mixed mode was used to bake the light, irrelevant otherwise.
        /// </summary>
        [NativeName("lightmapBakeMode.mixedLightingMode")]
        public MixedLightingMode mixedLightingMode;
        /// <summary>
        /// Is the light contribution already stored in lightmaps and/or lightprobes?
        /// </summary>
        /// <description>
        /// SA: [[Light.bakingOutput]], [[LightBakingOutput]].
        /// </description>
        public bool isBaked;
    }

    /// <summary>
    /// Allows mixed lights to control shadow caster culling when Shadowmasks are present.
    /// </summary>
    [NativeHeader("Runtime/Camera/SharedLightData.h")]
    public enum LightShadowCasterMode
    {
        /// <summary>
        /// Use the global Shadowmask Mode from the quality settings.
        /// </summary>
        Default,
        /// <summary>
        /// Render only non-lightmapped objects into the shadow map. This corresponds with the Shadowmask mode.
        /// </summary>
        NonLightmappedOnly,
        /// <summary>
        /// Render all shadow casters into the shadow map. This corresponds with the distance Shadowmask mode.
        /// </summary>
        Everything
    }

    /// <summary>
    /// Script interface for [[wiki:class-Light|light components]].
    /// </summary>
    /// <description>
    /// Use this to control all aspects of Unity's lights. The properties are an exact match for the
    /// values shown in the Inspector.
    /// Usually lights are just created in the editor but sometimes you want to create a light from a script:
    /// </description>
    [RequireComponent(typeof(Transform))]
    [NativeHeader("Runtime/Camera/Light.h")]
    public sealed partial class Light : Behaviour
    {
        /// <summary>
        /// The type of the light.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-Light|Light component]].
        /// </description>
        [NativeProperty("LightType")] extern public LightType type { get; set; }

        /// <summary>
        /// The angle of the light's spotlight cone in degrees.
        /// </summary>
        /// <description>
        /// This is used primarily for LightType.Spot lights and has no effect for LightType.Point lights
        /// SA: [[wiki:class-Light|Light component]].
        /// </description>
        extern public float spotAngle        { get; set; }
        /// <summary>
        /// The angle of the light's spotlight inner cone in degrees.
        /// </summary>
        /// <description>
        /// This is only used for LightType.Spot lights. This only works with a Scriptable Render Pipeline.
        /// </description>
        extern public float innerSpotAngle   { get; set; }
        /// <summary>
        /// The color of the light.
        /// </summary>
        /// <description>
        /// To modify the light intensity you change light's color luminance.
        /// Lights always add illumination, so a light with a black color is the same
        /// as no light at all.
        /// SA: [[wiki:class-Light|Light component]].
        /// </description>
        /// <description>
        /// Another example:
        /// </description>
        extern public Color color            { get; set; }
        /// <summary>
        ///           The color temperature of the light.
        ///           Correlated Color Temperature (abbreviated as CCT) is multiplied with the color filter when calculating the final color of a light source. The color temperature of the electromagnetic radiation emitted from an ideal black body is defined as its surface temperature in Kelvin. White is 6500K according to the D65 standard. Candle light is 1800K.
        ///           If you want to use lightsUseCCT, lightsUseLinearIntensity has to be enabled to ensure physically correct output.
        ///           SA: [[GraphicsSettings.lightsUseLinearIntensity]], [[GraphicsSettings.lightsUseCCT]].
        /// </summary>
        extern public float colorTemperature { get; set; }
        /// <summary>
        /// The Intensity of a light is multiplied with the Light color.
        /// </summary>
        /// <description>
        /// The value can be between 0 and 8. This allows you to create over bright lights.
        /// </description>
        extern public float intensity        { get; set; }
        /// <summary>
        /// The multiplier that defines the strength of the bounce lighting.
        /// </summary>
        /// <description>
        /// 0 means no bounce light (only direct lighting) will be produced.
        /// 1 is physically correct behaviour and the default. The intensity of indirect lighting scales the same with the intensity of the light.
        /// A value larger than 1 means an artificially high amount of bounce light will be emitted.
        /// </description>
        extern public float bounceIntensity  { get; set; }

        /// <summary>
        /// Set to true to override light bounding sphere for culling.
        /// </summary>
        /// <description>
        /// SA: [[Light.boundingSphereOverride]].
        /// </description>
        extern public bool useBoundingSphereOverride { get; set; }
        /// <summary>
        /// Bounding sphere used to override the regular light bounding sphere during culling.
        /// </summary>
        /// <description>
        /// SA: [[Light.useBoundingSphereOverride]].
        /// </description>
        extern public Vector4 boundingSphereOverride { get; set; }

        /// <summary>
        /// The custom resolution of the shadow map.
        /// </summary>
        /// <description>
        /// By default, shadow map resolution is computed from its importance on screen. Setting this property to a value greater than zero will override that behavior. Please note that the shadow map resolution will still be rounded to the nearest power of two and capped by memory and hardware limits.
        /// SA: shadows property, shadowResolution property, [[wiki:LightPerformance|Shadow map size calculation]].
        /// </description>
        extern public int   shadowCustomResolution { get; set; }
        /// <summary>
        /// Shadow mapping constant bias.
        /// </summary>
        /// <description>
        /// Shadow caster surfaces are pushed by this world-space amount away from the light, to help prevent self-shadowing ("shadow acne") artifacts.
        /// SA: shadows, shadowNormalBias properties.
        /// </description>
        extern public float shadowBias             { get; set; }
        /// <summary>
        /// Shadow mapping normal-based bias.
        /// </summary>
        /// <description>
        /// Shadow caster surfaces are pushed inwards along their normals by this amount, to help prevent self-shadowing ("shadow acne") artifacts. Units of normal-based bias are expressed in terms of shadowmap texel size; typically values between 0.3-0.7 work well.
        /// Larger values prevent shadow acne better, at expense of making shadow shape smaller than the object actually is.
        /// Currently normal-based bias is only implemented for directional lights; it has no effect for other light types.
        /// SA: shadows, shadowBias properties.
        /// </description>
        extern public float shadowNormalBias       { get; set; }
        /// <summary>
        /// Near plane value to use for shadow frustums.
        /// </summary>
        /// <description>
        /// This determines how close to the light shadows will stop being rendered from an object.
        /// </description>
        extern public float shadowNearPlane        { get; set; }
        /// <summary>
        /// Set to true to enable custom matrix for culling during shadows.
        /// </summary>
        /// <description>
        /// SA: [[Light.shadowMatrixOverride]].
        /// </description>
        extern public bool useShadowMatrixOverride { get; set; }
        /// <summary>
        /// Projection matrix used to override the regular light matrix during shadow culling.
        /// </summary>
        /// <description>
        /// SA: [[Light.useShadowMatrixOverride]].
        /// </description>
        extern public Matrix4x4 shadowMatrixOverride { get; set; }

        /// <summary>
        /// The range of the light.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-Light|Light component]].
        /// </description>
        extern public float range { get; set; }
        /// <summary>
        /// The [[wiki:class-Flare|flare asset]] to use for this light.
        /// </summary>
        /// <description>
        /// SA: [[LensFlare.flare]], [[wiki:class-Light|Light component]] and [[wiki:class-Flare|flare asset]].
        /// </description>
        extern public Flare flare { get; set; }

        /// <summary>
        /// This property describes the output of the last Global Illumination bake.
        /// </summary>
        extern public LightBakingOutput bakingOutput { get; set; }
        /// <summary>
        /// This is used to light certain objects in the Scene selectively.
        /// </summary>
        /// <description>
        /// A [[GameObject]] will only be illuminated by a light if that light's /cullingMask/
        /// includes the layer chosen for the GameObject (ie, the mask bit for the layer must be set to 1
        /// for the object to receive any light).
        /// See [[wiki:Layers|Layers]] for more information about layer masking.
        /// SA: [[wiki:class-Light|Light component]].
        /// </description>
        extern public int  cullingMask        { get; set; }
        /// <summary>
        /// Determines which rendering LayerMask this Light affects.
        /// </summary>
        /// <description>
        /// When using a Scriptable Render Pipeline, you can specify an additional rendering-specific LayerMask. During shadow passes, this filters Renderers based on the Light and the Renderers rendering LayerMask.
        /// </description>
        extern public int  renderingLayerMask { get; set; }
        /// <summary>
        /// Allows you to override the global Shadowmask Mode per light. Only use this with render pipelines that can handle per light Shadowmask modes. Incompatible with the legacy renderers.
        /// </summary>
        extern public LightShadowCasterMode lightShadowCasterMode { get; set; }
#if UNITY_EDITOR
        /// <summary>
        /// Controls the amount of artificial softening applied to the edges of shadows cast by the Point or Spot light.
        /// </summary>
        extern public float shadowRadius { get; set; }
        /// <summary>
        /// Controls the amount of artificial softening applied to the edges of shadows cast by directional lights.
        /// </summary>
        extern public float shadowAngle { get; set; }
#endif
    }

    /// <summary>
    /// A script interface for the [[wiki:class-Skybox|skybox component]].
    /// </summary>
    /// <description>
    /// The skybox class has only the material property.
    /// SA: [[wiki:class-Skybox|skybox component]].
    /// </description>
    [NativeHeader("Runtime/Camera/Skybox.h")]
    public sealed partial class Skybox : Behaviour
    {
        /// <summary>
        /// The material used by the skybox.
        /// </summary>
        /// <description>
        /// Note that unlike [[Renderer.material]], this returns a shared material reference and not a unique duplicate.
        /// SA: [[wiki:class-Skybox|skybox component]].
        /// </description>
        extern public Material material { get; set; }
    }

    /// <summary>
    /// A class to access the [[Mesh]] of the [[wiki:class-MeshFilter|mesh filter]].
    /// </summary>
    /// <description>
    /// Use this with a procedural mesh interface. SA: [[Mesh]] class.
    /// </description>
    [RequireComponent(typeof(Transform))]
    [NativeHeader("Runtime/Graphics/Mesh/MeshFilter.h")]
    public sealed partial class MeshFilter : Component
    {
        [RequiredByNativeCode]  // MeshFilter is used in the VR Splash screen.
        private void DontStripMeshFilter() {}

        /// <summary>
        /// Returns the shared mesh of the mesh filter.
        /// </summary>
        /// <description>
        /// It is recommended to use this function only for __reading mesh data__
        /// and not for writing, since you might modify imported assets and all objects
        /// that use this mesh will be affected.
        /// Also, be aware that is not possible to undo the changes done to this mesh.
        /// </description>
        extern public Mesh sharedMesh { get; set; }
        /// <summary>
        /// Returns the instantiated [[Mesh]] assigned to the mesh filter.
        /// </summary>
        /// <description>
        /// If no mesh is assigned to the mesh filter a new mesh will be created and assigned.
        /// If a mesh is assigned to the mesh filter already, then first query of /mesh/ property will create a duplicate of it, and this copy
        /// will be returned. Further queries of /mesh/ property will return this duplicated mesh instance.
        /// Once /mesh/ property is queried, link to the original shared mesh is lost and [[MeshFilter.sharedMesh]] property becomes an alias to /mesh/.
        /// If you want to avoid this automatic mesh duplication, use [[MeshFilter.sharedMesh]] instead.
        /// By using /mesh/ property you can modify the mesh for a single object only. The
        /// other objects that used the same mesh will not be modified.
        /// It is your responsibility to destroy the automatically instantiated mesh when the game object is being destroyed. [[Resources.UnloadUnusedAssets]]
        /// also destroys the mesh but it is usually only called when loading a new level.
        /// Consider /mesh/ property as a shortcut for the following code:
        /// </description>
        /// <description>
        /// Which is called on first query of /mesh/ property.
        /// __Note:__\\
        /// If [[MeshFilter]] is a part of an asset object, quering /mesh/ property is not allowed and only asset mesh can be assigned.
        /// </description>
        /// <description>
        /// SA: [[Mesh]] class.
        /// </description>
        extern public Mesh mesh {[NativeName("GetInstantiatedMeshFromScript")] get; [NativeName("SetInstantiatedMesh")] set; }
    }

    [RequireComponent(typeof(Transform))]
    [NativeHeader("Runtime/Camera/HaloManager.h")]
    internal sealed partial class Halo : Behaviour
    {
    }
}
