using System;
using System.Collections;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
    /// <summary>
    /// The LOD fade modes. Modes other than [[LODFadeMode.None]] will result in Unity calculating a blend factor for blending/interpolating between two neighbouring LODs and pass it to your shader.
    /// </summary>
    public enum LODFadeMode
    {
        /// <summary>
        /// Indicates the LOD fading is turned off.
        /// </summary>
        None = 0,
        /// <summary>
        /// Perform cross-fade style blending between the current LOD and the next LOD if the distance to camera falls in the range specified by the [[LOD.fadeTransitionWidth]] of each LOD.
        /// </summary>
        CrossFade = 1,
        /// <summary>
        /// By specifying this mode, your LODGroup will perform a SpeedTree-style LOD fading scheme:\\\\
        /// * For all the mesh LODs other than the last (most crude) mesh LOD, the fade factor is calculated as the percentage of the object's current screen height, compared to the whole range of the LOD. It is 1, if the camera is right at the position where the previous LOD switches out and 0, if the next LOD is just about to switch in.\\\\
        /// * For the last mesh LOD and the billboard LOD, the cross-fade mode is used.
        /// </summary>
        SpeedTree = 2
    }

    /// <summary>
    /// Structure for building a LOD for passing to the SetLODs function.
    /// </summary>
    [UsedByNativeCode]
    public struct LOD
    {
        // Construct a LOD
        /// <summary>
        /// Construct a LOD.
        /// </summary>
        /// <param name="screenRelativeTransitionHeight">
        /// The screen relative height to use for the transition [0-1].
        /// </param>
        /// <param name="renderers">
        /// An array of renderers to use for this LOD level.
        /// </param>
        public LOD(float screenRelativeTransitionHeight, Renderer[] renderers)
        {
            this.screenRelativeTransitionHeight = screenRelativeTransitionHeight;
            this.fadeTransitionWidth = 0;
            this.renderers = renderers;
        }

        // The screen relative height to use for the transition [0-1]
        /// <summary>
        /// The screen relative height to use for the transition [0-1].
        /// </summary>
        public float screenRelativeTransitionHeight;
        // Width of the transition (proportion to the current LOD's whole length).
        /// <summary>
        /// Width of the cross-fade transition zone (proportion to the current LOD's whole length) [0-1]. Only used if it's not animated.
        /// </summary>
        public float fadeTransitionWidth;
        // List of renderers for this LOD level
        /// <summary>
        /// List of renderers for this LOD level.
        /// </summary>
        public Renderer[] renderers;
    }

    // LODGroup lets you group multiple Renderers into LOD levels.
    /// <summary>
    /// LODGroup lets you group multiple Renderers into LOD levels.
    /// </summary>
    /// <description>
    /// This can be used to switch between different LOD levels at runtime based on size on screen.
    /// </description>
    [NativeHeader("Runtime/Graphics/LOD/LODGroup.h")]
    [NativeHeader("Runtime/Graphics/LOD/LODGroupManager.h")]
    [NativeHeader("Runtime/Graphics/LOD/LODUtility.h")]
    [StaticAccessor("GetLODGroupManager()", StaticAccessorType.Dot)]
    public class LODGroup : Component
    {
        // The local reference point against which the LOD distance is calculated.
        /// <summary>
        /// The local reference point against which the LOD distance is calculated.
        /// </summary>
        extern public Vector3 localReferencePoint { get; set; }

        // The size of LOD object in local space
        /// <summary>
        /// The size of the LOD object in local space.
        /// </summary>
        extern public float size { get; set; }

        // The number of LOD levels
        /// <summary>
        /// The number of LOD levels.
        /// </summary>
        extern public int lodCount  {[NativeMethod("GetLODCount")] get; }

        // The fade mode
        /// <summary>
        /// The LOD fade mode used.
        /// </summary>
        /// <description>
        /// SA: [[LODFadeMode]].
        /// </description>
        extern public LODFadeMode fadeMode  { get; set; }

        // Is cross-fading animated?
        /// <summary>
        /// Specify if the cross-fading should be animated by time. The animation duration is specified globally as crossFadeAnimationDuration.
        /// </summary>
        extern public bool animateCrossFading  { get; set; }

        // Enable / Disable the LODGroup - Disabling will turn off all renderers.
        /// <summary>
        /// Enable / Disable the LODGroup - Disabling will turn off all renderers.
        /// </summary>
        extern public bool enabled  { get; set; }

        // Recalculate the bounding region for the LODGroup (Relatively slow, do not call often)
        /// <summary>
        /// Recalculate the bounding region for the LODGroup (Relatively slow, do not call often).
        /// </summary>
        [FreeFunction("UpdateLODGroupBoundingBox", HasExplicitThis = true)]
        extern public void RecalculateBounds();

        /// <summary>
        /// Returns the array of LODs.
        /// </summary>
        /// <returns>
        /// The LOD array.
        /// </returns>
        [FreeFunction("GetLODs_Binding", HasExplicitThis = true)]
        extern public LOD[] GetLODs();

        /// <summary>
        /// Set the LODs for the LOD group. This will remove any existing LODs configured on the LODGroup.
        /// </summary>
        /// <param name="lods">
        /// The LODs to use for this group.
        /// </param>
        [Obsolete("Use SetLODs instead.")]
        public void SetLODS(LOD[] lods) { SetLODs(lods); }

        // Set the LODs for the LOD group. This will remove any existing LODs configured on the LODGroup
        /// <summary>
        /// Set the LODs for the LOD group. This will remove any existing LODs configured on the LODGroup.
        /// </summary>
        /// <param name="lods">
        /// The LODs to use for this group.
        /// </param>
        [FreeFunction("SetLODs_Binding", HasExplicitThis = true)]
        extern public void SetLODs(LOD[] lods);

        // Force a LOD level on this LOD group
        //
        // @param index The LOD level to use. Passing index < 0 will return to standard LOD processing
        /// <param name="index">
        /// The LOD level to use. Passing index < 0 will return to standard LOD processing.
        /// </param>
        [FreeFunction("ForceLODLevel", HasExplicitThis = true)]
        extern public void ForceLOD(int index);

        /// <summary>
        /// The cross-fading animation duration in seconds. ArgumentException will be thrown if it is set to zero or a negative value.
        /// </summary>
        /// <description>
        /// SA: animateCrossFading.
        /// </description>
        [StaticAccessor("GetLODGroupManager()")]
        extern public static float crossFadeAnimationDuration { get; set; }
    }
}
