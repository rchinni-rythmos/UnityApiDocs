using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

using LT = UnityEngineInternal.LightmapType;

namespace UnityEngine
{
    [RequireComponent(typeof(Transform))]
    [UsedByNativeCode]
    public partial class Renderer : Component
    {
        // called when the object became visible by any camera.
        // void OnBecameVisible();

        // called when the object is no longer visible by any camera.
        // void OnBecameInvisible();
    }

    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    public partial class Renderer : Component
    {
        extern public Bounds bounds {[FreeFunction(Name = "RendererScripting::GetBounds", HasExplicitThis = true)] get; }

        [FreeFunction(Name = "RendererScripting::SetStaticLightmapST", HasExplicitThis = true)] extern private void SetStaticLightmapST(Vector4 st);

        [FreeFunction(Name = "RendererScripting::GetMaterial", HasExplicitThis = true)] extern private Material GetMaterial();
        [FreeFunction(Name = "RendererScripting::GetSharedMaterial", HasExplicitThis = true)] extern private Material GetSharedMaterial();
        [FreeFunction(Name = "RendererScripting::SetMaterial", HasExplicitThis = true)] extern private void SetMaterial(Material m);

        [FreeFunction(Name = "RendererScripting::GetMaterialArray", HasExplicitThis = true)] extern private Material[] GetMaterialArray();
        [FreeFunction(Name = "RendererScripting::GetMaterialArray", HasExplicitThis = true)] extern private void CopyMaterialArray([Out] Material[] m);
        [FreeFunction(Name = "RendererScripting::GetSharedMaterialArray", HasExplicitThis = true)] extern private void CopySharedMaterialArray([Out] Material[] m);
        [FreeFunction(Name = "RendererScripting::SetMaterialArray", HasExplicitThis = true)] extern private void SetMaterialArray([NotNull] Material[] m);

        [FreeFunction(Name = "RendererScripting::SetPropertyBlock", HasExplicitThis = true)] extern internal void Internal_SetPropertyBlock(MaterialPropertyBlock properties);
        [FreeFunction(Name = "RendererScripting::GetPropertyBlock", HasExplicitThis = true)] extern internal void Internal_GetPropertyBlock([NotNull] MaterialPropertyBlock dest);
        [FreeFunction(Name = "RendererScripting::SetPropertyBlockMaterialIndex", HasExplicitThis = true)] extern internal void Internal_SetPropertyBlockMaterialIndex(MaterialPropertyBlock properties, int materialIndex);
        [FreeFunction(Name = "RendererScripting::GetPropertyBlockMaterialIndex", HasExplicitThis = true)] extern internal void Internal_GetPropertyBlockMaterialIndex([NotNull] MaterialPropertyBlock dest, int materialIndex);
        [FreeFunction(Name = "RendererScripting::HasPropertyBlock", HasExplicitThis = true)] extern public bool HasPropertyBlock();

        public void SetPropertyBlock(MaterialPropertyBlock properties) { Internal_SetPropertyBlock(properties); }
        public void SetPropertyBlock(MaterialPropertyBlock properties, int materialIndex) { Internal_SetPropertyBlockMaterialIndex(properties, materialIndex); }
        public void GetPropertyBlock(MaterialPropertyBlock properties) { Internal_GetPropertyBlock(properties); }
        public void GetPropertyBlock(MaterialPropertyBlock properties, int materialIndex) { Internal_GetPropertyBlockMaterialIndex(properties, materialIndex); }

        [FreeFunction(Name = "RendererScripting::GetClosestReflectionProbes", HasExplicitThis = true)] extern private void GetClosestReflectionProbesInternal(object result);
    }

    [NativeHeader("Runtime/Graphics/Renderer.h")]
    public partial class Renderer : Component
    {
        extern public bool enabled   { get; set; }
        extern public bool isVisible {[NativeName("IsVisibleInScene")] get; }

        extern public ShadowCastingMode shadowCastingMode { get; set; }
        extern public bool              receiveShadows { get; set; }

        extern public MotionVectorGenerationMode motionVectorGenerationMode { get; set; }
        extern public LightProbeUsage            lightProbeUsage { get; set; }
        extern public ReflectionProbeUsage       reflectionProbeUsage { get; set; }
        extern public UInt32                     renderingLayerMask { get; set; }
        extern public int                        rendererPriority { get; set; }

        extern public   string sortingLayerName  { get; set; }
        extern public   int    sortingLayerID    { get; set; }
        extern public   int    sortingOrder      { get; set; }
        extern internal int    sortingGroupID    { get; set; }
        extern internal int    sortingGroupOrder { get; set; }

        [NativeProperty("IsDynamicOccludee")] extern public bool allowOcclusionWhenDynamic { get; set; }


        [NativeProperty("StaticBatchRoot")] extern internal Transform staticBatchRootTransform { get; set; }
        extern internal int staticBatchIndex { get; }
        extern internal void SetStaticBatchInfo(int firstSubMesh, int subMeshCount);
        extern public bool isPartOfStaticBatch {[NativeName("IsPartOfStaticBatch")] get; }

        extern public Matrix4x4 worldToLocalMatrix { get; }
        extern public Matrix4x4 localToWorldMatrix { get; }


        extern public GameObject lightProbeProxyVolumeOverride { get; set; }
        extern public Transform  probeAnchor { get; set; }

        [NativeName("GetLightmapIndexInt")] extern private int  GetLightmapIndex(LT lt);
        [NativeName("SetLightmapIndexInt")] extern private void SetLightmapIndex(int index, LT lt);
        [NativeName("GetLightmapST")] extern private Vector4 GetLightmapST(LT lt);
        [NativeName("SetLightmapST")] extern private void    SetLightmapST(Vector4 st, LT lt);

        public int lightmapIndex         { get { return GetLightmapIndex(LT.StaticLightmap); }  set { SetLightmapIndex(value, LT.StaticLightmap); } }
        public int realtimeLightmapIndex { get { return GetLightmapIndex(LT.DynamicLightmap); } set { SetLightmapIndex(value, LT.DynamicLightmap); } }

        public Vector4 lightmapScaleOffset         { get { return GetLightmapST(LT.StaticLightmap); }  set { SetStaticLightmapST(value); } }
        public Vector4 realtimeLightmapScaleOffset { get { return GetLightmapST(LT.DynamicLightmap); } set { SetLightmapST(value, LT.DynamicLightmap); } }

        extern private int GetMaterialCount();
        [NativeName("GetMaterialArray")] extern private Material[] GetSharedMaterialArray();

        // this is needed to extract check for persistent from cpp to cs
    #if UNITY_EDITOR
        extern internal bool IsPersistent();
    #endif

        public Material[] materials
        {
            get
            {
            #if UNITY_EDITOR
                if (IsPersistent())
                {
                    Debug.LogError("Not allowed to access Renderer.materials on prefab object. Use Renderer.sharedMaterials instead", this);
                    return null;
                }
            #endif
                return GetMaterialArray();
            }
            set { SetMaterialArray(value); }
        }

        public Material material
        {
            get
            {
            #if UNITY_EDITOR
                if (IsPersistent())
                {
                    Debug.LogError("Not allowed to access Renderer.material on prefab object. Use Renderer.sharedMaterial instead", this);
                    return null;
                }
            #endif
                return GetMaterial();
            }
            set { SetMaterial(value); }
        }

        public Material sharedMaterial { get { return GetSharedMaterial(); } set { SetMaterial(value); } }
        public Material[] sharedMaterials { get { return GetSharedMaterialArray(); } set { SetMaterialArray(value); } }

        public void GetMaterials(List<Material> m)
        {
            if (m == null)
                throw new ArgumentNullException("The result material list cannot be null.", "m");
        #if UNITY_EDITOR
            if (IsPersistent())
                throw new InvalidOperationException("Not allowed to access Renderer.materials on prefab object. Use Renderer.sharedMaterials instead");
        #endif

            NoAllocHelpers.EnsureListElemCount(m, GetMaterialCount());
            CopyMaterialArray(NoAllocHelpers.ExtractArrayFromListT(m));
        }

        public void GetSharedMaterials(List<Material> m)
        {
            if (m == null)
                throw new ArgumentNullException("The result material list cannot be null.", "m");

            NoAllocHelpers.EnsureListElemCount(m, GetMaterialCount());
            CopySharedMaterialArray(NoAllocHelpers.ExtractArrayFromListT(m));
        }

        public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
        {
            GetClosestReflectionProbesInternal(result);
        }
    }

    [NativeHeader("Runtime/Graphics/TrailRenderer.h")]
    public sealed partial class TrailRenderer : Renderer
    {
        /// <summary>
        /// How long does the trail take to fade out.
        /// </summary>
        extern public float time                { get; set; }
        /// <summary>
        /// The width of the trail at the spawning point.
        /// </summary>
        /// <description>
        /// A width of 1 corresponds to 1 unit in the game world.
        ///                 SA: endWidth variable.
        /// </description>
        extern public float startWidth          { get; set; }
        /// <summary>
        /// The width of the trail at the end of the trail.
        /// </summary>
        /// <description>
        /// A width of 1 corresponds to 1 unit in the game world.
        ///                 SA: startWidth variable.
        /// </description>
        extern public float endWidth            { get; set; }
        /// <summary>
        /// Set an overall multiplier that is applied to the [[TrailRenderer.widthCurve]] to get the final width of the trail.
        /// </summary>
        extern public float widthMultiplier     { get; set; }
        /// <summary>
        /// Does the [[GameObject]] of this Trail Renderer auto destruct?
        /// </summary>
        /// <description>
        /// When set to /true/, the [[GameObject]] will be destroyed when all its points have been removed.
        /// </description>
        extern public bool  autodestruct        { get; set; }
        /// <summary>
        /// Creates trails when the [[GameObject]] moves.
        /// </summary>
        /// <description>
        /// When set to /true/, trail geometry will be created while the [[GameObject]] moves.
        /// </description>
        extern public bool  emitting            { get; set; }
        /// <summary>
        /// Set this to a value greater than 0, to get rounded corners between each segment of the trail.
        /// </summary>
        /// <description>
        /// The value controls how many vertices are added to each joint, where a higher value will give a smoother result.
        /// </description>
        extern public int   numCornerVertices   { get; set; }
        /// <summary>
        /// Set this to a value greater than 0, to get rounded corners on each end of the trail.
        /// </summary>
        /// <description>
        /// The value controls how many vertices are added to each end, where a higher value will give a smoother result.
        /// </description>
        extern public int   numCapVertices      { get; set; }
        /// <summary>
        /// Set the minimum distance the trail can travel before a new vertex is added to it.
        /// </summary>
        /// <description>
        /// Smaller values with give smoother trails, consisting of more vertices, but costing more performance.
        /// </description>
        extern public float minVertexDistance   { get; set; }

        /// <summary>
        /// Set the color at the start of the trail.
        /// </summary>
        extern public Color startColor          { get; set; }
        /// <summary>
        /// Set the color at the end of the trail.
        /// </summary>
        extern public Color endColor            { get; set; }

        /// <summary>
        /// Get the number of line segments in the trail.
        /// </summary>
        [NativeProperty("PositionsCount")] extern public int positionCount { get; }
        /// <summary>
        /// Set the position of a vertex in the trail.
        /// </summary>
        /// <param name="index">
        /// Which position to set.
        /// </param>
        /// <param name="position">
        /// The new position.
        /// </param>
        /// <description>
        /// You can only use this method to modify existing positions in the Trail. You cannot use it to add new positions.
        /// When setting multiple positions, consider using SetPositions instead because it is much faster than making individual function calls for each position.
        /// SA: positionCount property.
        /// </description>
        extern public void SetPosition(int index, Vector3 position);
        /// <summary>
        /// Get the position of a vertex in the trail.
        /// </summary>
        /// <param name="index">
        /// The index of the position to retrieve.
        /// </param>
        /// <returns>
        /// The position at the specified index in the array.
        /// </returns>
        /// <description>
        /// SA: numPositions property.
        /// SA: GetPositions function.
        /// </description>
        extern public Vector3 GetPosition(int index);

        /// <summary>
        /// Apply a shadow bias to prevent self-shadowing artifacts. The specified value is the proportion of the trail width at each segment.
        /// </summary>
        extern public float shadowBias { get; set; }

        /// <summary>
        /// Configures a trail to generate Normals and Tangents. With this data, Scene lighting can affect the trail via Normal Maps and the Unity Standard Shader, or your own custom-built Shaders.
        /// </summary>
        extern public bool generateLightingData { get; set; }

        /// <summary>
        /// Choose whether the U coordinate of the trail texture is tiled or stretched.
        /// </summary>
        /// <description>
        /// Stretching will cause the texture to be mapped once along the entire length of the trail, whereas Tiling will cause the texture to be repeated at a rate of once per world unit. To set the tiling rate, use [[Material.SetTextureScale]].
        /// </description>
        extern public LineTextureMode textureMode { get; set; }
        /// <summary>
        /// Select whether the trail will face the camera, or the orientation of the Transform Component.
        /// </summary>
        extern public LineAlignment   alignment   { get; set; }

        /// <summary>
        /// Removes all points from the TrailRenderer.
        /// Useful for restarting a trail from a new position.
        /// </summary>
        extern public void Clear();

        /// <summary>
        /// Creates a snapshot of TrailRenderer and stores it in /mesh/.
        /// </summary>
        /// <param name="mesh">
        /// A static mesh that will receive the snapshot of the trail.
        /// </param>
        public void BakeMesh(Mesh mesh, bool useTransform = false) { BakeMesh(mesh, Camera.main, useTransform); }
        /// <summary>
        /// Creates a snapshot of TrailRenderer and stores it in /mesh/.
        /// </summary>
        /// <param name="mesh">
        /// A static mesh that will receive the snapshot of the trail.
        /// </param>
        /// <param name="camera">
        /// The camera used for determining which way camera-space trails will face.
        /// </param>
        extern public void BakeMesh([NotNull] Mesh mesh, [NotNull] Camera camera, bool useTransform = false);

        /// <summary>
        /// Set the curve describing the width of the trail at various points along its length.
        /// </summary>
        /// <description>
        /// This property is multiplied by [[TrailRenderer.widthMultiplier]] to get the final width of the trail.
        /// </description>
        public AnimationCurve widthCurve    { get { return GetWidthCurveCopy(); }    set { SetWidthCurve(value); } }
        /// <summary>
        /// Set the color gradient describing the color of the trail at various points along its length.
        /// </summary>
        /// <description>
        /// SA: [[Gradient]].
        /// </description>
        public Gradient       colorGradient { get { return GetColorGradientCopy(); } set { SetColorGradient(value); } }

        // these are direct glue to TrailRenderer methods to simplify properties code (and have null checks generated)

        extern private AnimationCurve GetWidthCurveCopy();
        extern private void SetWidthCurve([NotNull] AnimationCurve curve);

        extern private Gradient GetColorGradientCopy();
        extern private void SetColorGradient([NotNull] Gradient curve);
    }

    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    public sealed partial class TrailRenderer : Renderer
    {
        /// <summary>
        /// Get the positions of all vertices in the trail.
        /// </summary>
        /// <param name="positions">
        /// The array of positions to retrieve.
        /// </param>
        /// <returns>
        /// How many positions were actually stored in the output array.
        /// </returns>
        /// <description>
        /// This method is preferred to GetPosition when retrieving all positions, as it is more efficient to get all positions using a single command than to get each position individually.
        /// SA: numPositions property.
        /// SA: GetPosition function.
        /// </description>
        [FreeFunction(Name = "TrailRendererScripting::GetPositions", HasExplicitThis = true)]
        extern public int GetPositions([NotNull][Out] Vector3[] positions);

        /// <summary>
        /// Sets the positions of all vertices in the trail.
        /// </summary>
        /// <param name="positions">
        /// The array of positions to set.
        /// </param>
        /// <description>
        /// You can only use this method to modify existing positions in the Trail. You cannot use it to add new positions.
        /// When setting all positions, use this method instead of SetPosition because it is more efficient to set all positions using a single command than to set each position individually.
        /// SA: positionCount property.
        /// SA: SetPosition function.
        /// </description>
        [FreeFunction(Name = "TrailRendererScripting::SetPositions", HasExplicitThis = true)]
        extern public void SetPositions([NotNull] Vector3[] positions);

        /// <summary>
        /// Adds a position to the trail.
        /// </summary>
        /// <param name="position">
        /// The position to add to the trail.
        /// </param>
        /// <description>
        /// SA: AddPositions function.
        /// </description>
        [FreeFunction(Name = "TrailRendererScripting::AddPosition", HasExplicitThis = true)]
        extern public void AddPosition(Vector3 position);

        /// <summary>
        /// Add an array of positions to the trail.
        /// </summary>
        /// <param name="positions">
        /// The positions to add to the trail.
        /// </param>
        /// <description>
        /// All points inside a TrailRenderer store a timestamp when they are born. This, together with the [[time|TrailRenderer.time]] property, is used to determine when they will be removed. For trails to disappear smoothly, each position must have a unique, increasing timestamp. When positions are supplied from script and the current time is identical for multiple points, position timestamps are adjusted to interpolate smoothly between the timestamp of the newest existing point in the trail and the current time.
        /// </description>
        [FreeFunction(Name = "TrailRendererScripting::AddPositions", HasExplicitThis = true)]
        extern public void AddPositions([NotNull] Vector3[] positions);
    }

    [NativeHeader("Runtime/Graphics/LineRenderer.h")]
    public sealed partial class LineRenderer : Renderer
    {
        /// <summary>
        /// Set the width at the start of the line.
        /// </summary>
        extern public float startWidth          { get; set; }
        /// <summary>
        /// Set the width at the end of the line.
        /// </summary>
        extern public float endWidth            { get; set; }
        /// <summary>
        /// Set an overall multiplier that is applied to the [[LineRenderer.widthCurve]] to get the final width of the line.
        /// </summary>
        extern public float widthMultiplier     { get; set; }
        /// <summary>
        /// Set this to a value greater than 0, to get rounded corners between each segment of the line.
        /// </summary>
        /// <description>
        /// The value controls how many vertices are added to each joint, where a higher value will give a smoother result.
        /// </description>
        extern public int   numCornerVertices   { get; set; }
        /// <summary>
        /// Set this to a value greater than 0, to get rounded corners on each end of the line.
        /// </summary>
        /// <description>
        /// The value controls how many vertices are added to each end, where a higher value will give a smoother result.
        /// </description>
        extern public int   numCapVertices      { get; set; }
        /// <summary>
        /// If enabled, the lines are defined in world space.
        /// </summary>
        /// <description>
        /// This means the object's position is ignored, and the lines are rendered around world origin.
        /// </description>
        extern public bool  useWorldSpace       { get; set; }
        /// <summary>
        /// Connect the start and end positions of the line together to form a continuous loop.
        /// </summary>
        extern public bool  loop                { get; set; }

        /// <summary>
        /// Set the color at the start of the line.
        /// </summary>
        extern public Color startColor          { get; set; }
        /// <summary>
        /// Set the color at the end of the line.
        /// </summary>
        extern public Color endColor            { get; set; }

        /// <summary>
        /// Set/get the number of vertices.
        /// </summary>
        /// <description>
        /// positionCount returns the number of vertices in the line. The example below shows a line with 3 vertices.
        /// </description>
        [NativeProperty("PositionsCount")] extern public int positionCount { get; set; }
        /// <summary>
        /// Set the position of a vertex in the line.
        /// </summary>
        /// <param name="index">
        /// Which position to set.
        /// </param>
        /// <param name="position">
        /// The new position.
        /// </param>
        /// <description>
        /// Consider using SetPositions instead, if setting multiple positions, as it is much faster than making individual function calls for each position.
        /// SA: positionCount property.
        /// </description>
        extern public void SetPosition(int index, Vector3 position);
        /// <summary>
        /// Get the position of a vertex in the line.
        /// </summary>
        /// <param name="index">
        /// The index of the position to retrieve.
        /// </param>
        /// <returns>
        /// The position at the specified index in the array.
        /// </returns>
        /// <description>
        /// SA: positionCount property, SetPositions function, GetPositions function.
        /// </description>
        extern public Vector3 GetPosition(int index);

        /// <summary>
        /// Apply a shadow bias to prevent self-shadowing artifacts. The specified value is the proportion of the line width at each segment.
        /// </summary>
        extern public float shadowBias          { get; set; }

        /// <summary>
        /// Configures a line to generate Normals and Tangents. With this data, Scene lighting can affect the line via Normal Maps and the Unity Standard Shader, or your own custom-built Shaders.
        /// </summary>
        extern public bool generateLightingData { get; set; }

        /// <summary>
        /// Choose whether the U coordinate of the line texture is tiled or stretched.
        /// </summary>
        /// <description>
        /// Stretching will cause the texture to be mapped once along the entire length of the line, whereas Tiling will cause the texture to be repeated at a rate of once per world unit. To set the tiling rate, use [[Material.SetTextureScale]].
        /// </description>
        extern public LineTextureMode textureMode { get; set; }
        /// <summary>
        /// Select whether the line will face the camera, or the orientation of the Transform Component.
        /// </summary>
        extern public LineAlignment   alignment   { get; set; }

        /// <summary>
        /// Generates a simplified version of the original line by removing points that fall within the specified tolerance.
        /// </summary>
        /// <param name="tolerance">
        /// This value is used to evaluate which points should be removed from the line. A higher value results in a simpler line (less points). A positive value close to zero results in a line with little to no reduction. A value of zero or less has no effect.
        /// </param>
        /// <description>
        /// Uses [[LineUtility.Simplify]] to perform the line simplification.
        /// This example shows how an existing line can be simplified:
        /// </description>
        /// <description>
        /// __This example generates a line in the shape of a sine wave and provides a GUI for customizing the line generation and simplification parameters.__
        /// </description>
        extern public void Simplify(float tolerance);

        /// <summary>
        /// Creates a snapshot of LineRenderer and stores it in /mesh/.
        /// </summary>
        /// <param name="mesh">
        /// A static mesh that will receive the snapshot of the line.
        /// </param>
        public void BakeMesh(Mesh mesh, bool useTransform = false) { BakeMesh(mesh, Camera.main, useTransform); }
        /// <summary>
        /// Creates a snapshot of LineRenderer and stores it in /mesh/.
        /// </summary>
        /// <param name="mesh">
        /// A static mesh that will receive the snapshot of the line.
        /// </param>
        /// <param name="camera">
        /// The camera used for determining which way camera-space lines will face.
        /// </param>
        extern public void BakeMesh([NotNull] Mesh mesh, [NotNull] Camera camera, bool useTransform = false);

        /// <summary>
        /// Set the curve describing the width of the line at various points along its length.
        /// </summary>
        /// <description>
        /// This property is multiplied by [[LineRenderer.widthMultiplier]] to get the final width of the line.
        /// </description>
        public AnimationCurve widthCurve    { get { return GetWidthCurveCopy(); }    set { SetWidthCurve(value); } }
        /// <summary>
        /// Set the color gradient describing the color of the line at various points along its length.
        /// </summary>
        /// <description>
        /// SA: [[Gradient]].
        /// </description>
        public Gradient       colorGradient { get { return GetColorGradientCopy(); } set { SetColorGradient(value); } }

        // these are direct glue to TrailRenderer methods to simplify properties code (and have null checks generated)

        extern private AnimationCurve GetWidthCurveCopy();
        extern private void SetWidthCurve([NotNull] AnimationCurve curve);

        extern private Gradient GetColorGradientCopy();
        extern private void SetColorGradient([NotNull] Gradient curve);
    }

    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    public sealed partial class LineRenderer : Renderer
    {
        /// <summary>
        /// Get the positions of all vertices in the line.
        /// </summary>
        /// <param name="positions">
        /// The array of positions to retrieve. The array passed should be of at least positionCount in size.
        /// </param>
        /// <returns>
        /// How many positions were actually stored in the output array.
        /// </returns>
        /// <description>
        /// This method is preferred to GetPosition when retrieving all positions, as it is more efficient to get all positions using a single command than to get each position individually.
        /// SA: positionCount property, GetPosition function.
        /// </description>
        [FreeFunction(Name = "LineRendererScripting::GetPositions", HasExplicitThis = true)]
        extern public int GetPositions([NotNull][Out] Vector3[] positions);

        /// <summary>
        /// Set the positions of all vertices in the line.
        /// </summary>
        /// <param name="positions">
        /// The array of positions to set.
        /// </param>
        /// <description>
        /// This method is preferred to SetPosition when setting all positions, as it is more efficient to set all positions using a single command than to set each position individually.  Note that positionCount must be called before SetPositions. Also SetPositions ignores points with indices beyond positionCount.
        /// SA: positionCount property, SetPosition function.
        /// </description>
        [FreeFunction(Name = "LineRendererScripting::SetPositions", HasExplicitThis = true)]
        extern public void SetPositions([NotNull] Vector3[] positions);
    }

    /// <summary>
    /// The Skinned Mesh filter.
    /// </summary>
    [NativeHeader("Runtime/Graphics/Mesh/SkinnedMeshRenderer.h")]
    public partial class SkinnedMeshRenderer : Renderer
    {
        /// <summary>
        /// The maximum number of bones affecting a single vertex.
        /// </summary>
        extern public SkinQuality quality { get; set; }
        /// <summary>
        /// If enabled, the Skinned Mesh will be updated when offscreen. If disabled, this also disables updating animations.
        /// </summary>
        /// <description>
        /// SA: [[wiki:class-SkinnedMeshRenderer|Skinned Mesh Renderer component]].
        /// </description>
        extern public bool updateWhenOffscreen  { get; set; }
        /// <summary>
        /// Forces the Skinned Mesh to recalculate its matricies when rendered
        /// </summary>
        /// <description>
        /// This property must be set in cases where the user would like to manually render a skinned mesh multiple times within a single update, an example of this would be rendering out the results of an animation to a texture.
        /// </description>
        extern public bool forceMatrixRecalculationPerRender  { get; set; }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        extern public Transform rootBone { get; set; }
    #if UNITY_EDITOR
        extern internal Transform actualRootBone { get; }
    #endif
        /// <summary>
    /// The bones used to skin the mesh.
    /// </summary>
    /// <description>
    /// See the code example for [[Mesh.bindposes]] for further details.
    /// </description>
    extern public Transform[] bones { get; set; }

        /// <summary>
        /// The mesh used for skinning.
        /// </summary>
        [NativeProperty("Mesh")] extern public Mesh sharedMesh { get; set; }
        /// <summary>
        /// Specifies whether skinned motion vectors should be used for this renderer.
        /// </summary>
        /// <description>
        /// If set to true, the SkinnedMeshRenderer generates vectors using skinning data from the current and last frame to calculate the per-pixel object movement. This means that the motion vector buffer captures small object movements. (For example; a character moving an arm.)
        /// Skinned motion vectors are important for characters with animation. There is a cost to skinned motion vectors, though;  they require twice as much memory per skinned mesh because the graphics memory on the GPU becomes double buffered (one buffer for the current frame and one buffer for the previous frame). The buffers track motion between frames; the velocity is the current frame's position minus the last frame's position.
        /// SA: [[DepthTextureMode.MotionVectors]], [[Renderer.motionVectorGenerationMode]], [[PassType.MotionVectors]], [[SystemInfo.supportsMotionVectors]].
        /// </description>
        [NativeProperty("SkinnedMeshMotionVectors")]  extern public bool skinnedMotionVectors { get; set; }

        /// <summary>
        /// Returns the weight of a BlendShape for this Renderer.
        /// </summary>
        /// <param name="index">
        /// The index of the BlendShape whose weight you want to retrieve. Index must be smaller than the Mesh.blendShapeCount of the Mesh attached to this Renderer.
        /// </param>
        /// <returns>
        /// The weight of the BlendShape.
        /// </returns>
        /// <description>
        /// The weight of a BlendShape represents how much the Mesh has been blended (or morphed) from its original shape to a target BlendShape (another Mesh containing the same topology, but with different vertex positions than the original). The BlendShape weight range includes values between the minimum and the maximum weights defined in the model.
        /// SA: SetBlendShapeWeight.
        /// </description>
        extern public float GetBlendShapeWeight(int index);
        /// <summary>
        /// Sets the weight of a BlendShape for this Renderer.
        /// </summary>
        /// <param name="index">
        /// The index of the BlendShape to modify. Index must be smaller than the Mesh.blendShapeCount of the Mesh attached to this Renderer.
        /// </param>
        /// <param name="value">
        /// The weight for this BlendShape.
        /// </param>
        /// <description>
        /// The weight of a BlendShape represents how much the Mesh has been blended (or morphed) from its original shape to a target BlendShape (another Mesh containing the same topology, but with different vertex positions than the original). The BlendShape weight range includes values between the minimum and the maximum weights defined in the model.
        /// SA: GetBlendShapeWeight.
        /// </description>
        extern public void  SetBlendShapeWeight(int index, float value);
        /// <summary>
        /// Creates a snapshot of SkinnedMeshRenderer and stores it in /mesh/.
        /// </summary>
        /// <param name="mesh">
        /// A static mesh that will receive the snapshot of the skinned mesh.
        /// </param>
        /// <description>
        /// The vertices are relative to the SkinnedMeshRenderer Transform component.
        /// __Notes__:\\
        /// The snapshot is still computed even when updateWhenOffscreen is set to false and the skinned mesh object is currently offscreen.\\
        /// When this function is called the skinning process will always take place on the CPU, regardless of the [[PlayerSettings.gpuSkinning|GPU Skinning]] setting
        /// </description>
        extern public void  BakeMesh(Mesh mesh);

        [FreeFunction(Name = "SkinnedMeshRendererScripting::GetLocalAABB", HasExplicitThis = true)]
        extern private Bounds GetLocalAABB();
        extern private void   SetLocalAABB(Bounds b);

        /// <summary>
        /// AABB of this Skinned Mesh in its local space.
        /// </summary>
        /// <description>
        /// It is precomputed on import for imported models based on animations associated with that model, which means that
        /// the bounding box might be much bigger than the mesh itself. It is recomputed every time whenupdateWhenOffscreen is enabled, but in 
        public Bounds localBounds { get { return GetLocalAABB(); } set { SetLocalAABB(value); } }
    }

    [NativeHeader("Runtime/Graphics/Mesh/MeshRenderer.h")]
    public partial class MeshRenderer : Renderer
    {
        [RequiredByNativeCode]  // MeshRenderer is used in the VR Splash screen.
        private void DontStripMeshRenderer() {}

        extern public Mesh additionalVertexStreams { get; set; }
        extern public int subMeshStartIndex {[NativeName("GetSubMeshStartIndex")] get; }
#if UNITY_EDITOR
        extern public ReceiveGI receiveGI { get; set; }
#endif
    }

    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    public static partial class RendererExtensions
    {
        [FreeFunction("RendererScripting::UpdateGIMaterialsForRenderer")] extern static internal void UpdateGIMaterialsForRenderer(Renderer renderer);
    }
}
