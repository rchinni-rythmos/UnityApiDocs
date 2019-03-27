using System;
using TextureDimension = UnityEngine.Rendering.TextureDimension;

namespace UnityEngine
{
    partial class RenderTexture
    {
#if UNITY_EDITOR
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Use RenderTexture.autoGenerateMips instead (UnityUpgradable) -> autoGenerateMips", false)]
        public bool generateMips
        {
            get { return autoGenerateMips; }
            set { autoGenerateMips = value; }
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("UsSetBorderColor is no longer supported.", true)]
        public void SetBorderColor(Color color) {}
#endif

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("Use RenderTexture.dimension instead.", false)]
        public bool isCubemap
        {
            get { return dimension == TextureDimension.Cube; }
            set { dimension = value ? TextureDimension.Cube : TextureDimension.Tex2D; }
        }

        /// <summary>
        /// If enabled, this Render Texture will be used as a [[Texture3D]].
        /// </summary>
        /// <description>
        /// Volumetric render textures currently only work on [[wiki:UsingDX11GL3Features]]. You can render into them using "random access writes" from
        /// a pixel shader or a compute shader.
        /// SA: volumeDepth, enableRandomWrite.
        /// </description>
        [Obsolete("Use RenderTexture.dimension instead.", false)]
        public bool isVolume
        {
            get { return dimension == TextureDimension.Tex3D; }
            set { dimension = value ? TextureDimension.Tex3D : TextureDimension.Tex2D; }
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("RenderTexture.enabled is always now, no need to use it.", false)]
        // for some reason we are providing enabled setter which is empty (i dont know what the intent is/was)
        public static bool enabled { get { return true; } set {} }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("GetTexelOffset always returns zero now, no point in using it.", false)]
        public Vector2 GetTexelOffset() { return Vector2.zero; }
    }
}
