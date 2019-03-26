using System;
using ShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode;
using LightProbeUsage   = UnityEngine.Rendering.LightProbeUsage;
using UnityEngine.Rendering;

namespace UnityEngine
{
#if UNITY_EDITOR
    partial class Mesh
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property Mesh.uv1 has been deprecated. Use Mesh.uv2 instead (UnityUpgradable) -> uv2", true)]
        public Vector2[] uv1 { get { return null; } set {} }
    }

    partial class Renderer
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property lightmapTilingOffset has been deprecated. Use lightmapScaleOffset (UnityUpgradable) -> lightmapScaleOffset", true)]
        public Vector4 lightmapTilingOffset { get { return Vector4.zero; } set {} }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Use probeAnchor instead (UnityUpgradable) -> probeAnchor", true)]
        public Transform lightProbeAnchor { get { return probeAnchor; } set { probeAnchor = value; } }
    }

    partial class Projector
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property isOrthoGraphic has been deprecated. Use orthographic instead (UnityUpgradable) -> orthographic", true)]
        public bool isOrthoGraphic { get { return false; } set {} }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property orthoGraphicSize has been deprecated. Use orthographicSize instead (UnityUpgradable) -> orthographicSize", true)]
        public float orthoGraphicSize { get { return -1f; } set {} }
    }

    partial class Graphics
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Method DrawMesh has been deprecated. Use Graphics.DrawMeshNow instead (UnityUpgradable) -> DrawMeshNow(*)", true)]
        [UnityEngine.Internal.ExcludeFromDocs]
        static public void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation) {}

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Method DrawMesh has been deprecated. Use Graphics.DrawMeshNow instead (UnityUpgradable) -> DrawMeshNow(*)", true)]
        [UnityEngine.Internal.ExcludeFromDocs]
        static public void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex) {}

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Method DrawMesh has been deprecated. Use Graphics.DrawMeshNow instead (UnityUpgradable) -> DrawMeshNow(*)", true)]
        [UnityEngine.Internal.ExcludeFromDocs]
        static public void DrawMesh(Mesh mesh, Matrix4x4 matrix) {}

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Method DrawMesh has been deprecated. Use Graphics.DrawMeshNow instead (UnityUpgradable) -> DrawMeshNow(*)", true)]
        [UnityEngine.Internal.ExcludeFromDocs]
        static public void DrawMesh(Mesh mesh, Matrix4x4 matrix, int materialIndex) {}

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Method DrawProcedural has been deprecated. Use Graphics.DrawProceduralNow instead. (UnityUpgradable) -> DrawProceduralNow(*)", true)]
        [UnityEngine.Internal.ExcludeFromDocs]
        public static void DrawProcedural(MeshTopology topology, int vertexCount, int instanceCount = 1) { DrawProceduralNow(topology, vertexCount, instanceCount); }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Method DrawProceduralIndirect has been deprecated. Use Graphics.DrawProceduralIndirectNow instead. (UnityUpgradable) -> DrawProceduralIndirectNow(*)", true)]
        [UnityEngine.Internal.ExcludeFromDocs]
        public static void DrawProceduralIndirect(MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset = 0) { DrawProceduralIndirectNow(topology, bufferWithArgs, argsOffset); }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property deviceName has been deprecated. Use SystemInfo.graphicsDeviceName instead (UnityUpgradable) -> UnityEngine.SystemInfo.graphicsDeviceName", true)]
        static public string deviceName { get { return SystemInfo.graphicsDeviceName; } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property deviceVendor has been deprecated. Use SystemInfo.graphicsDeviceVendor instead (UnityUpgradable) -> UnityEngine.SystemInfo.graphicsDeviceVendor", true)]
        static public string deviceVendor { get { return SystemInfo.graphicsDeviceVendor; } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property deviceVersion has been deprecated. Use SystemInfo.graphicsDeviceVersion instead (UnityUpgradable) -> UnityEngine.SystemInfo.graphicsDeviceVersion", true)]
        static public string deviceVersion { get { return SystemInfo.graphicsDeviceVersion; } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("CreateGPUFence has been deprecated. Use CreateGraphicsFence instead (UnityUpgradable) -> CreateAsyncGraphicsFence(*)", true)]
        public static GPUFence CreateGPUFence([UnityEngine.Internal.DefaultValue("UnityEngine.Rendering.SynchronisationStage.PixelProcessing")] SynchronisationStage stage) { return new GPUFence(); }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("WaitOnGPUFence has been deprecated. Use WaitOnAsyncGraphicsFence instead (UnityUpgradable) -> WaitOnAsyncGraphicsFence(*)", true)]
        public static void WaitOnGPUFence(GPUFence fence, [UnityEngine.Internal.DefaultValue("UnityEngine.Rendering.SynchronisationStage.PixelProcessing")] SynchronisationStage stage) {}


        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("CreateGPUFence has been deprecated. Use CreateGraphicsFence instead (UnityUpgradable) -> CreateAsyncGraphicsFence(*)", true)]
        [UnityEngine.Internal.ExcludeFromDocs] public static GPUFence CreateGPUFence() { return new GPUFence(); }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("WaitOnGPUFence has been deprecated. Use WaitOnAsyncGraphicsFence instead (UnityUpgradable) -> WaitOnAsyncGraphicsFence(*)", true)]
        [UnityEngine.Internal.ExcludeFromDocs] public static void WaitOnGPUFence(GPUFence fence) {}
    }

    partial class Screen
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property GetResolution has been deprecated. Use resolutions instead (UnityUpgradable) -> resolutions", true)]
        static public Resolution[] GetResolution { get { return null; } }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property showCursor has been deprecated. Use Cursor.visible instead (UnityUpgradable) -> UnityEngine.Cursor.visible", true)]
        static public bool showCursor { get; set; }
    }

    partial class LightmapData
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property LightmapData.lightmap has been deprecated. Use LightmapData.lightmapColor instead (UnityUpgradable) -> lightmapColor", true)]
        public Texture2D lightmap { get { return default(Texture2D); } set {} }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property LightmapData.lightmapFar has been deprecated. Use LightmapData.lightmapColor instead (UnityUpgradable) -> lightmapColor", true)]
        public Texture2D lightmapFar { get { return null; } set {} }

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Property LightmapData.lightmapNear has been deprecated. Use LightmapData.lightmapDir instead (UnityUpgradable) -> lightmapDir", true)]
        public Texture2D lightmapNear { get { return null; } set {} }
    }

    partial class Shader
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("SetGlobalTexGenMode is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
        public static void SetGlobalTexGenMode(string propertyName, TexGenMode mode)            {}

        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("SetGlobalTextureMatrixName is not supported anymore. Use programmable shaders to achieve the same effect.", true)]
        public static void SetGlobalTextureMatrixName(string propertyName, string matrixName)   {}
    }
#endif

    /// <summary>
    /// Single, dual, or directional lightmaps rendering mode, used only in GIWorkflowMode.Legacy
    /// </summary>
    /// <description>
    /// SA: [[LightmapSettings.lightmapsModeLegacy]].
    /// </description>
    public enum LightmapsModeLegacy
    {
        /// <summary>
        /// Single, traditional lightmap rendering mode.
        /// </summary>
        /// <description>
        /// SA: [[LightmapSettings.lightmapsModeLegacy]].
        /// </description>
        Single = 0,
        /// <summary>
        /// Dual lightmap rendering mode.
        /// </summary>
        /// <description>
        /// SA: [[LightmapSettings.lightmapsModeLegacy]].
        /// </description>
        Dual = 1,
        /// <summary>
        /// Directional rendering mode.
        /// </summary>
        /// <description>
        /// SA: [[LightmapSettings.lightmapsModeLegacy]].
        /// </description>
        Directional = 2,
    }

    partial class LightmapSettings
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use lightmapsMode instead.", false)]
        public static LightmapsModeLegacy lightmapsModeLegacy { get { return LightmapsModeLegacy.Single; } set {} }
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use QualitySettings.desiredColorSpace instead.", false)]
        public static ColorSpace bakedColorSpace { get { return QualitySettings.desiredColorSpace; } set {} }
    }

    partial class LightProbes
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Use GetInterpolatedProbe instead.", true)]
        public void GetInterpolatedLightProbe(Vector3 position, Renderer renderer, float[] coefficients) {}
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Use bakedProbes instead.", true)]
        public float[] coefficients { get { return new float[0]; } set {} }
    }

    /// <summary>
    /// The trail renderer is used to make trails behind objects in the Scene as they move about.
    /// </summary>
    /// <description>
    /// This class is a script interface for a [[wiki:class-TrailRenderer|trail renderer]] component.
    /// </description>
    partial class TrailRenderer
    {
        /// <summary>
        /// Get the number of line segments in the trail.
        /// </summary>
        [Obsolete("Use positionCount instead (UnityUpgradable) -> positionCount", false)]
        public int numPositions { get { return positionCount; } }
    }

    /// <summary>
    /// The line renderer is used to draw free-floating lines in 3D space.
    /// </summary>
    /// <description>
    /// This class is a script interface for a [[wiki:class-LineRenderer|line renderer]] component.
    /// </description>
    partial class LineRenderer
    {
        /// <summary>
        /// Set the line width at the start and at the end.
        /// </summary>
        [Obsolete("Use startWidth, endWidth or widthCurve instead.", false)]
        public void SetWidth(float start, float end)
        {
            startWidth = start;
            endWidth = end;
        }

        /// <summary>
        /// Set the line color at the start and at the end.
        /// </summary>
        [Obsolete("Use startColor, endColor or colorGradient instead.", false)]
        public void SetColors(Color start, Color end)
        {
            startColor = start;
            endColor = end;
        }

        /// <summary>
        /// Set the number of line segments.
        /// </summary>
        /// <description>
        /// SA: SetPosition function.
        /// SA: SetPositions function.
        /// </description>
        [Obsolete("Use positionCount instead.", false)]
        public void SetVertexCount(int count)
        {
            positionCount = count;
        }

        /// <summary>
        /// Set the number of line segments.
        /// </summary>
        [Obsolete("Use positionCount instead (UnityUpgradable) -> positionCount", false)]
        public int numPositions { get { return positionCount; } set { positionCount = value; } }
    }

    /// <summary>
    /// A block of material values to apply.
    /// </summary>
    /// <description>
    /// MaterialPropertyBlock is used by [[Graphics.DrawMesh]] and Renderer.SetPropertyBlock. Use
    /// it in situations where you want to draw
    /// multiple objects with the same material, but slightly different properties. For example, if you
    /// want to slightly change the color of each mesh drawn. Changing the render state is not supported.
    /// Unity's terrain engine uses MaterialPropertyBlock to draw trees; all of them use the
    /// same material, but each tree has different color, scale & wind factor.
    /// The block passed to [[Graphics.DrawMesh]] or Renderer.SetPropertyBlock is copied, so the most efficient way of using it is
    /// to create one block and reuse it for all DrawMesh calls. Use SetFloat, SetVector, SetColor, SetMatrix, SetTexture, SetBuffer to add or replace values.
    /// SA: [[Graphics.DrawMesh]], [[Material]].
    /// </description>
    partial class MaterialPropertyBlock
    {
        // TODO: effectively adding a property or setting a property should be the same, but SetFloat will be a bit slower due to an extra lookup...

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetFloat instead (UnityUpgradable) -> SetFloat(*)", false)]
        public void AddFloat(string name, float value) { SetFloat(Shader.PropertyToID(name), value); }
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetFloat instead (UnityUpgradable) -> SetFloat(*)", false)]
        public void AddFloat(int nameID, float value)  { SetFloat(nameID, value); }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetVector instead (UnityUpgradable) -> SetVector(*)", false)]
        public void AddVector(string name, Vector4 value)   { SetVector(Shader.PropertyToID(name), value); }
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetVector instead (UnityUpgradable) -> SetVector(*)", false)]
        public void AddVector(int nameID, Vector4 value)    { SetVector(nameID, value); }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetColor instead (UnityUpgradable) -> SetColor(*)", false)]
        public void AddColor(string name, Color value)  { SetColor(Shader.PropertyToID(name), value); }
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetColor instead (UnityUpgradable) -> SetColor(*)", false)]
        public void AddColor(int nameID, Color value)   { SetColor(nameID, value); }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetMatrix instead (UnityUpgradable) -> SetMatrix(*)", false)]
        public void AddMatrix(string name, Matrix4x4 value) { SetMatrix(Shader.PropertyToID(name), value); }
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetMatrix instead (UnityUpgradable) -> SetMatrix(*)", false)]
        public void AddMatrix(int nameID, Matrix4x4 value)  { SetMatrix(nameID, value); }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetTexture instead (UnityUpgradable) -> SetTexture(*)", false)]
        public void AddTexture(string name, Texture value)  { SetTexture(Shader.PropertyToID(name), value); }
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use SetTexture instead (UnityUpgradable) -> SetTexture(*)", false)]
        public void AddTexture(int nameID, Texture value)   { SetTexture(nameID, value); }
    }

    partial class QualitySettings
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use GetQualityLevel and SetQualityLevel", false)]
        public static QualityLevel currentLevel { get { return (QualityLevel)GetQualityLevel(); } set { SetQualityLevel((int)value, true); } }
    }

    partial class Renderer
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Use shadowCastingMode instead.", false)]
        public bool castShadows
        {
            get { return shadowCastingMode != ShadowCastingMode.Off; }
            set { shadowCastingMode = value ? ShadowCastingMode.On : ShadowCastingMode.Off; }
        }

        [Obsolete("Use motionVectorGenerationMode instead.", false)]
        public bool motionVectors
        {
            get { return motionVectorGenerationMode == MotionVectorGenerationMode.Object; }
            set { motionVectorGenerationMode = value ? MotionVectorGenerationMode.Object : MotionVectorGenerationMode.Camera; }
        }

        [Obsolete("Use lightProbeUsage instead.", false)]
        public bool useLightProbes
        {
            get { return lightProbeUsage != LightProbeUsage.Off; }
            set { lightProbeUsage = value ? LightProbeUsage.BlendProbes : LightProbeUsage.Off; }
        }
    }

    /// <summary>
    /// The Render Settings contain values for a range of visual elements in your Scene, like fog and ambient light.
    /// </summary>
    /// <description>
    /// Note that render settings are per-scene.
    /// </description>
    partial class RenderSettings
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use RenderSettings.ambientIntensity instead (UnityUpgradable) -> ambientIntensity", false)]
        public static float ambientSkyboxAmount { get { return ambientIntensity; } set { ambientIntensity = value; } }
    }

    partial class Screen
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Use Cursor.lockState and Cursor.visible instead.", false)]
        public static bool lockCursor
        {
            get { return CursorLockMode.Locked == Cursor.lockState; }
            set
            {
                if (value) { Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked; }
                else        { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            }
        }
    }

    partial class Shader
    {
        [Obsolete("Use Graphics.activeTier instead (UnityUpgradable) -> UnityEngine.Graphics.activeTier", false)]
        public static UnityEngine.Rendering.ShaderHardwareTier globalShaderHardwareTier
        {
            get { return (UnityEngine.Rendering.ShaderHardwareTier)Graphics.activeTier; }
            set { Graphics.activeTier = (UnityEngine.Rendering.GraphicsTier)value; }
        }
    }

    partial class Material
    {
        [Obsolete("Creating materials from shader source string will be removed in the future. Use Shader assets instead.", false)]
        public static Material Create(string scriptContents)
        {
            return new Material(scriptContents);
        }
    }
}

namespace UnityEngine.Rendering
{
    // deprecated in 5.5
    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    [Obsolete("ShaderHardwareTier was renamed to GraphicsTier (UnityUpgradable) -> GraphicsTier", false)]
    public enum ShaderHardwareTier
    {
        /// <summary>
        /// The first shader hardware tier - corresponds to shader define UNITY_HARDWARE_TIER1.
        /// </summary>
        Tier1 = 0,
        /// <summary>
        /// The second shader hardware tier - corresponds to shader define UNITY_HARDWARE_TIER2.
        /// </summary>
        Tier2 = 1,
        /// <summary>
        /// The third shader hardware tier - corresponds to shader define UNITY_HARDWARE_TIER3.
        /// </summary>
        Tier3 = 2,
    }
}
