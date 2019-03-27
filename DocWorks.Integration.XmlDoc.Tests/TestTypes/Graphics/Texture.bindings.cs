using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.Bindings;
using uei = UnityEngine.Internal;
using UnityEngine.Experimental.Rendering;
using Unity.Collections.LowLevel.Unsafe;

using TextureDimension = UnityEngine.Rendering.TextureDimension;

namespace UnityEngine
{
    [NativeHeader("Runtime/Graphics/Texture.h")]
    [NativeHeader("Runtime/Streaming/TextureStreamingManager.h")]
    [UsedByNativeCode]
    public partial class Texture : Object
    {
        protected Texture() {}

        extern public static int masterTextureLimit { get; set; }

        [NativeProperty("AnisoLimit")] extern public static AnisotropicFiltering anisotropicFiltering { get; set; }
        [NativeName("SetGlobalAnisoLimits")] extern public static void SetGlobalAnisotropicFilteringLimits(int forcedMin, int globalMax);

        public virtual GraphicsFormat graphicsFormat
        {
            get { return GraphicsFormatUtility.GetFormat(this); }
        }

        extern private int GetDataWidth();
        extern private int GetDataHeight();
        extern private TextureDimension GetDimension();

        // Note: not implemented setters in base class since some classes do need to actually implement them (e.g. RenderTexture)
        virtual public int width { get { return GetDataWidth(); } set { throw new NotImplementedException(); } }
        virtual public int height { get { return GetDataHeight(); } set { throw new NotImplementedException(); } }
        virtual public TextureDimension dimension { get { return GetDimension(); } set { throw new NotImplementedException(); } }

        extern virtual public bool isReadable { get; }

        // Note: getter for "wrapMode" returns the U mode on purpose
        extern public TextureWrapMode wrapMode { [NativeName("GetWrapModeU")] get; set; }

        extern public TextureWrapMode wrapModeU { get; set; }
        extern public TextureWrapMode wrapModeV { get; set; }
        extern public TextureWrapMode wrapModeW { get; set; }
        extern public FilterMode filterMode { get; set; }
        extern public int anisoLevel { get; set; }
        extern public float mipMapBias { get; set; }
        extern public Vector2 texelSize { [NativeName("GetNpotTexelSize")] get; }
        extern public IntPtr GetNativeTexturePtr();
        [Obsolete("Use GetNativeTexturePtr instead.", false)]
        public int GetNativeTextureID() { return (int)GetNativeTexturePtr(); }

        extern public uint updateCount { get; }
        extern public void IncrementUpdateCount();

        [NativeMethod("GetActiveTextureColorSpace")]
        extern private int Internal_GetActiveTextureColorSpace();

        internal ColorSpace activeTextureColorSpace
        {
            [VisibleToOtherModules("UnityEngine.UIElementsModule")]
            get { return Internal_GetActiveTextureColorSpace() == 0 ? ColorSpace.Linear : ColorSpace.Gamma; }
        }

#if UNITY_EDITOR
        extern public Hash128 imageContentsHash { get; set; }
#endif

#if ENABLE_TEXTURE_STREAMING
        extern public static ulong totalTextureMemory
        {
            [FreeFunction("GetTextureStreamingManager().GetTotalTextureMemory")]
            get;
        }

        extern public static ulong desiredTextureMemory
        {
            [FreeFunction("GetTextureStreamingManager().GetDesiredTextureMemory")]
            get;
        }

        extern public static ulong targetTextureMemory
        {
            [FreeFunction("GetTextureStreamingManager().GetTargetTextureMemory")]
            get;
        }

        extern public static ulong currentTextureMemory
        {
            [FreeFunction("GetTextureStreamingManager().GetCurrentTextureMemory")]
            get;
        }

        extern public static ulong nonStreamingTextureMemory
        {
            [FreeFunction("GetTextureStreamingManager().GetNonStreamingTextureMemory")]
            get;
        }

        extern public static ulong streamingMipmapUploadCount
        {
            [FreeFunction("GetTextureStreamingManager().GetStreamingMipmapUploadCount")]
            get;
        }

        extern public static ulong streamingRendererCount
        {
            [FreeFunction("GetTextureStreamingManager().GetStreamingRendererCount")]
            get;
        }

        extern public static ulong streamingTextureCount
        {
            [FreeFunction("GetTextureStreamingManager().GetStreamingTextureCount")]
            get;
        }

        extern public static ulong nonStreamingTextureCount
        {
            [FreeFunction("GetTextureStreamingManager().GetNonStreamingTextureCount")]
            get;
        }

        extern public static ulong streamingTexturePendingLoadCount
        {
            [FreeFunction("GetTextureStreamingManager().GetStreamingTexturePendingLoadCount")]
            get;
        }

        extern public static ulong streamingTextureLoadingCount
        {
            [FreeFunction("GetTextureStreamingManager().GetStreamingTextureLoadingCount")]
            get;
        }

        [FreeFunction("GetTextureStreamingManager().SetStreamingTextureMaterialDebugProperties")]
        extern public static void SetStreamingTextureMaterialDebugProperties();

        extern public static bool streamingTextureForceLoadAll
        {
            [FreeFunction(Name = "GetTextureStreamingManager().GetForceLoadAll")]
            get;
            [FreeFunction(Name = "GetTextureStreamingManager().SetForceLoadAll")]
            set;
        }
        extern public static bool streamingTextureDiscardUnusedMips
        {
            [FreeFunction(Name = "GetTextureStreamingManager().GetDiscardUnusedMips")]
            get;
            [FreeFunction(Name = "GetTextureStreamingManager().SetDiscardUnusedMips")]
            set;
        }
#endif
        extern public static bool allowThreadedTextureCreation
        {
            [FreeFunction(Name = "Texture2DScripting::IsCreateTextureThreadedEnabled")]
            get;
            [FreeFunction(Name = "Texture2DScripting::EnableCreateTextureThreaded")]
            set;
        }
    }

    [NativeHeader("Runtime/Graphics/Texture2D.h")]
    [NativeHeader("Runtime/Graphics/GeneratedTextures.h")]
    [UsedByNativeCode]
    public sealed partial class Texture2D : Texture
    {
        extern public int mipmapCount { [NativeName("CountDataMipmaps")] get; }
        extern public TextureFormat format { [NativeName("GetTextureFormat")] get; }

        [StaticAccessor("builtintex", StaticAccessorType.DoubleColon)] extern public static Texture2D whiteTexture { get; }
        [StaticAccessor("builtintex", StaticAccessorType.DoubleColon)] extern public static Texture2D blackTexture { get; }
        [StaticAccessor("builtintex", StaticAccessorType.DoubleColon)] extern public static Texture2D normalTexture { get; }

        extern public void Compress(bool highQuality);

        [FreeFunction("Texture2DScripting::Create")]
        extern private static bool Internal_CreateImpl([Writable] Texture2D mono, int w, int h, GraphicsFormat format, TextureCreationFlags flags, IntPtr nativeTex);
        private static void Internal_Create([Writable] Texture2D mono, int w, int h, GraphicsFormat format, TextureCreationFlags flags, IntPtr nativeTex)
        {
            if (!Internal_CreateImpl(mono, w, h, format, flags, nativeTex))
                throw new UnityException("Failed to create texture because of invalid parameters.");
        }

        extern override public bool isReadable { get; }
        [NativeName("Apply")] extern private void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);
        [NativeName("Resize")] extern private bool ResizeImpl(int width, int height);
        [NativeName("SetPixel")] extern private void SetPixelImpl(int image, int x, int y, Color color);
        [NativeName("GetPixel")] extern private Color GetPixelImpl(int image, int x, int y);
        [NativeName("GetPixelBilinear")] extern private Color GetPixelBilinearImpl(int image, float u, float v);

        [FreeFunction(Name = "Texture2DScripting::ResizeWithFormat", HasExplicitThis = true)]
        extern private bool ResizeWithFormatImpl(int width, int height, TextureFormat format, bool hasMipMap);

        [FreeFunction(Name = "Texture2DScripting::ReadPixels", HasExplicitThis = true)]
        extern private void ReadPixelsImpl(Rect source, int destX, int destY, bool recalculateMipMaps);


        [FreeFunction(Name = "Texture2DScripting::SetPixels", HasExplicitThis = true)]
        extern private void SetPixelsImpl(int x, int y, int w, int h, Color[] pixel, int miplevel, int frame);

        [FreeFunction(Name = "Texture2DScripting::LoadRawData", HasExplicitThis = true)]
        extern private bool LoadRawTextureDataImpl(IntPtr data, int size);

        [FreeFunction(Name = "Texture2DScripting::LoadRawData", HasExplicitThis = true)]
        extern private bool LoadRawTextureDataImplArray(byte[] data);

        extern private IntPtr GetWritableImageData(int frame);
        extern private long GetRawImageDataSize();

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        extern private static AtomicSafetyHandle GetSafetyHandle(Texture2D tex);
#endif

        [FreeFunction("Texture2DScripting::GenerateAtlas")]
        extern private static void GenerateAtlasImpl(Vector2[] sizes, int padding, int atlasSize, [Out] Rect[] rect);

#if ENABLE_TEXTURE_STREAMING
        extern public bool streamingMipmaps { get; }
        extern public int streamingMipmapsPriority { get; }

        extern public int requestedMipmapLevel
        {
            [FreeFunction(Name = "GetTextureStreamingManager().GetRequestedMipmapLevel", HasExplicitThis = true)]
            get;
            [FreeFunction(Name = "GetTextureStreamingManager().SetRequestedMipmapLevel", HasExplicitThis = true)]
            set;
        }

#if UNITY_EDITOR
        extern internal bool loadAllMips
        {
            [FreeFunction(Name = "GetTextureStreamingManager().GetLoadAllMips", HasExplicitThis = true)]
            get;
            [FreeFunction(Name = "GetTextureStreamingManager().SetLoadAllMips", HasExplicitThis = true)]
            set;
        }
#endif

        extern public int desiredMipmapLevel
        {
            [FreeFunction(Name = "GetTextureStreamingManager().GetDesiredMipmapLevel", HasExplicitThis = true)]
            get;
        }

        extern public int loadingMipmapLevel
        {
            [FreeFunction(Name = "GetTextureStreamingManager().GetLoadingMipmapLevel", HasExplicitThis = true)]
            get;
        }

        extern public int loadedMipmapLevel
        {
            [FreeFunction(Name = "GetTextureStreamingManager().GetLoadedMipmapLevel", HasExplicitThis = true)]
            get;
        }

        [FreeFunction(Name = "GetTextureStreamingManager().ClearRequestedMipmapLevel", HasExplicitThis = true)]
        extern public void ClearRequestedMipmapLevel();

        [FreeFunction(Name = "GetTextureStreamingManager().IsRequestedMipmapLevelLoaded", HasExplicitThis = true)]
        extern public bool IsRequestedMipmapLevelLoaded();

#endif

        [FreeFunction("Texture2DScripting::UpdateExternalTexture", HasExplicitThis = true)]
        extern public void UpdateExternalTexture(IntPtr nativeTex);

        [FreeFunction("Texture2DScripting::SetAllPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern private void SetAllPixels32(Color32[] colors, int miplevel);

        [FreeFunction("Texture2DScripting::SetBlockOfPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern private void SetBlockOfPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, int miplevel);

        [FreeFunction("Texture2DScripting::GetRawTextureData", HasExplicitThis = true)]
        extern public byte[] GetRawTextureData();

        [FreeFunction("Texture2DScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight, int miplevel);

        public Color[] GetPixels(int x, int y, int blockWidth, int blockHeight)
        {
            return GetPixels(x, y, blockWidth, blockHeight, 0);
        }

        [FreeFunction("Texture2DScripting::GetPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern public Color32[] GetPixels32(int miplevel);

        public Color32[] GetPixels32()
        {
            return GetPixels32(0);
        }

        [FreeFunction("Texture2DScripting::PackTextures", HasExplicitThis = true)]
        extern public Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize, bool makeNoLongerReadable);

        public Rect[] PackTextures(Texture2D[] textures, int padding, int maximumAtlasSize)
        {
            return PackTextures(textures, padding, maximumAtlasSize, false);
        }

        public Rect[] PackTextures(Texture2D[] textures, int padding)
        {
            return PackTextures(textures, padding, 2048);
        }

#if UNITY_EDITOR
        extern public bool alphaIsTransparency { get; set; }

        [VisibleToOtherModules("UnityEngine.UIElementsModule")]
        extern internal float pixelsPerPoint { get; set; }
#endif
    }

    /// <summary>
    /// Class for handling cube maps, Use this to create or modify existing [[wiki:class-Cubemap|cube map assets]].
    /// </summary>
    [NativeHeader("Runtime/Graphics/CubemapTexture.h")]
    [ExcludeFromPreset]
    public sealed partial class Cubemap : Texture
    {
        /// <summary>
        /// How many mipmap levels are in this texture (RO).
        /// </summary>
        /// <description>
        /// The returned value includes the base level as well, so it is always 1 or more.
        /// </description>
        extern public int mipmapCount { [NativeName("CountDataMipmaps")] get; }
        /// <summary>
        /// The format of the pixel data in the texture (RO).
        /// </summary>
        /// <description>
        /// Use this to determine the format of the texture.
        /// </description>
        extern public TextureFormat format { [NativeName("GetTextureFormat")] get; }

        [FreeFunction("CubemapScripting::Create")]
        extern private static bool Internal_CreateImpl([Writable] Cubemap mono, int ext, GraphicsFormat format, TextureCreationFlags flags, IntPtr nativeTex);
        private static void Internal_Create([Writable] Cubemap mono, int ext, GraphicsFormat format, TextureCreationFlags flags, IntPtr nativeTex)
        {
            if (!Internal_CreateImpl(mono, ext, format, flags, nativeTex))
                throw new UnityException("Failed to create texture because of invalid parameters.");
        }

        [FreeFunction(Name = "CubemapScripting::Apply", HasExplicitThis = true)]
        extern private void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

        /// <summary>
        /// Updates Unity cubemap to use different native cubemap texture object.
        /// </summary>
        /// <param name="nativeTexture">
        /// Native cubemap texture object.
        /// </param>
        /// <description>
        /// This method is mostly useful for [[wiki:NativePluginInterface|native code plugins]] that create platform specific cubemap texture
        /// objects outside of Unity, and need to use these cubemaps in Unity scenes. For a cubemap created with CreateExternalTexture, this method switches to another underlying cubemap texture object if/when it changes.
        /// The actual contents of the native texture object will vary based on the native graphics API in use.  For example, if DirectX is in use, the native texture object will need
        /// to be a pointer to an ID3D11ShaderResourceView.  If OpenGL/OpenGL ES is in use, the native texture object should be a GLuint.  If Metal, then the native texture object should be a
        /// MTLTexture.
        /// SA: CreateExternalTexture.
        /// </description>
        [FreeFunction("CubemapScripting::UpdateExternalTexture", HasExplicitThis = true)]
        extern public void UpdateExternalTexture(IntPtr nativeTexture);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        extern override public bool isReadable { get; }
        [NativeName("SetPixel")] extern private void SetPixelImpl(int image, int x, int y, Color color);
        [NativeName("GetPixel")] extern private Color GetPixelImpl(int image, int x, int y);

        /// <summary>
        /// Performs smoothing of near edge regions.
        /// </summary>
        /// <param name="smoothRegionWidthInPixels">
        /// Pixel distance at edges over which to apply smoothing.
        /// </param>
        /// <description>
        /// Helps to compensate lack of linear interpolation across the edges of cubemap in GPU.
        /// Usually you will want to call this method for the cubemap which is going to be used for glossy reflections.
        /// </description>
        [NativeName("FixupEdges")] extern public void SmoothEdges([uei.DefaultValue("1")] int smoothRegionWidthInPixels);
        /// <summary>
        /// Performs smoothing of near edge regions.
        /// </summary>
        /// <description>
        /// Helps to compensate lack of linear interpolation across the edges of cubemap in GPU.
        /// Usually you will want to call this method for the cubemap which is going to be used for glossy reflections.
        /// </description>
        public void SmoothEdges() { SmoothEdges(1); }

        /// <summary>
        /// Returns pixel colors of a cubemap face.
        /// </summary>
        /// <param name="face">
        /// The face from which pixel data is taken.
        /// </param>
        /// <param name="miplevel">
        /// Mipmap level for the chosen face.
        /// </param>
        /// <description>
        /// This method returns an array of pixel colors of the whole
        /// mip level of a cubemap face.
        /// The returned array is a flattened 2D array, where pixels are laid out right to left,
        /// top to bottom (i.e. row after row). Array size is width by height of the mip level used.
        /// The default mip level is zero (the base texture) in which case the size is just the size of the texture.
        /// In general case, mip level size is @@mipSize=max(1,width>>miplevel)@@.
        /// The texture must have the __Is Readable__ flag set in the import settings, otherwise this method will fail. GetPixels is not available on Textures using Crunch texture compression.
        /// Using /GetPixels/ can be faster than calling GetPixel repeatedly, especially
        /// for large textures. In addition, /GetPixels/ can access individual mipmap levels.
        /// __Note:__ It is assumed that the six sides of each Cubemap are visible from the outside.  This
        /// means pixels are laid out left to right, top to bottom (i.e. row after row).  If the Cubemap
        /// surrounds the world then pixels appear right to left.
        /// SA: SetPixels, Texture2D.mipmapCount.
        /// </description>
        [FreeFunction(Name = "CubemapScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public Color[] GetPixels(CubemapFace face, int miplevel);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Color[] GetPixels(CubemapFace face)
        {
            return GetPixels(face, 0);
        }

        /// <summary>
        /// Sets pixel colors of a cubemap face.
        /// </summary>
        /// <param name="colors">
        /// Pixel data for the Cubemap face.
        /// </param>
        /// <param name="face">
        /// The face to which the new data should be applied.
        /// </param>
        /// <param name="miplevel">
        /// The mipmap level for the face.
        /// </param>
        /// <description>
        /// This method takes a color array and changes the pixel colors of the whole
        /// cubemap face. Call Apply to actually upload the changed
        /// pixels to the graphics card.
        /// The /colors/ array is a flattened 2D array, where pixels are laid out right to left,
        /// top to bottom (i.e. row after row). Array size must be at least width by height of the mip level used.
        /// The default mip level is zero (the base texture) in which case the size is just the size of the texture.
        /// In general case, mip level size is @@mipSize=max(1,width>>miplevel)@@.
        /// This method works only on /RGBA32/, /ARGB32/, /RGB24/ and /Alpha8/ texture formats.
        /// For other formats /SetPixels/ is ignored.
        /// SA: GetPixels, Apply, Texture2D.mipmapCount.
        /// </description>
        [FreeFunction(Name = "CubemapScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetPixels(Color[] colors, CubemapFace face, int miplevel);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetPixels(Color[] colors, CubemapFace face)
        {
            SetPixels(colors, face, 0);
        }
    }

    /// <summary>
    /// Class for handling 3D Textures, Use this to create [[wiki:class-Texture3D|3D texture assets]].
    /// </summary>
    /// <description>
    /// 3D textures are commonly used as lookup tables by shaders, or to represent volumetric data.
    /// Typically you'd create a 3D texture, fill it up with data (.SetPixels or SetPixels32) and call
    [NativeHeader("Runtime/Graphics/Texture3D.h")]
    [ExcludeFromPreset]
    public sealed partial class Texture3D : Texture
    {
        /// <summary>
        /// The depth of the texture (RO).
        /// </summary>
        /// <description>
        /// SA: Texture.width, Texture.height.
        /// </description>
        extern public int depth { [NativeName("GetTextureLayerCount")] get; }
        /// <summary>
        /// The format of the pixel data in the texture (RO).
        /// </summary>
        /// <description>
        /// Use this to determine the format of the texture.
        /// </description>
        extern public TextureFormat format { [NativeName("GetTextureFormat")] get; }

        /// <summary>
        /// Returns true if this 3D texture is Read/Write Enabled; otherwise returns false. For dynamic textures created from script, always returns true.
        /// </summary>
        extern override public bool isReadable { get; }
        [NativeName("SetPixel")] extern private void SetPixelImpl(int image, int x, int y, int z, Color color);
        [NativeName("GetPixel")] extern private Color GetPixelImpl(int image, int x, int y, int z);
        [NativeName("GetPixelBilinear")] extern private Color GetPixelBilinearImpl(int image, float u, float v, float w);

        [FreeFunction("Texture3DScripting::Create")]
        extern private static bool Internal_CreateImpl([Writable] Texture3D mono, int w, int h, int d, GraphicsFormat format, TextureCreationFlags flags);
        private static void Internal_Create([Writable] Texture3D mono, int w, int h, int d, GraphicsFormat format, TextureCreationFlags flags)
        {
            if (!Internal_CreateImpl(mono, w, h, d, format, flags))
                throw new UnityException("Failed to create texture because of invalid parameters.");
        }

        [FreeFunction(Name = "Texture3DScripting::Apply", HasExplicitThis = true)]
        extern private void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

        /// <summary>
        /// Returns an array of pixel colors representing one mip level of the 3D texture.
        /// </summary>
        /// <param name="miplevel">
        /// The mipmap level to be accessed.
        /// </param>
        /// <returns>
        /// The colors to get the array of pixels.
        /// </returns>
        /// <description>
        /// Note that using [[Color32]] data and GetPixels32 can be faster and consume less memory.
        /// SA: SetPixel, SetPixels, SetPixels32, GetPixel, GetPixelBilinear, GetPixels32, Apply functions.
        /// </description>
        [FreeFunction(Name = "Texture3DScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public Color[] GetPixels(int miplevel);

        /// <summary>
        /// Returns an array of pixel colors representing one mip level of the 3D texture.
        /// </summary>
        /// <returns>
        /// The colors to get the array of pixels.
        /// </returns>
        /// <description>
        /// Note that using [[Color32]] data and GetPixels32 can be faster and consume less memory.
        /// SA: SetPixel, SetPixels, SetPixels32, GetPixel, GetPixelBilinear, GetPixels32, Apply functions.
        /// </description>
        public Color[] GetPixels()
        {
            return GetPixels(0);
        }

        /// <summary>
        /// Returns an array of pixel colors representing one mip level of the 3D texture.
        /// </summary>
        /// <param name="miplevel">
        /// The mipmap level to be accessed.
        /// </param>
        /// <returns>
        /// The colors to get the array of pixels.
        /// </returns>
        /// <description>
        /// SA: SetPixel, SetPixels, SetPixels32, GetPixel, GetPixelBilinear, GetPixels, Apply functions.
        /// </description>
        [FreeFunction(Name = "Texture3DScripting::GetPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern public Color32[] GetPixels32(int miplevel);

        /// <summary>
        /// Returns an array of pixel colors representing one mip level of the 3D texture.
        /// </summary>
        /// <returns>
        /// The colors to get the array of pixels.
        /// </returns>
        /// <description>
        /// SA: SetPixel, SetPixels, SetPixels32, GetPixel, GetPixelBilinear, GetPixels, Apply functions.
        /// </description>
        public Color32[] GetPixels32()
        {
            return GetPixels32(0);
        }

        /// <summary>
        /// Sets pixel colors of a 3D texture.
        /// </summary>
        /// <param name="colors">
        /// The colors to set the pixels to.
        /// </param>
        /// <param name="miplevel">
        /// The mipmap level to be affected by the new colors.
        /// </param>
        /// <description>
        /// This function takes a color array and changes the pixel colors of the 3D texture.
        /// Call Apply to actually upload the changed pixels to the GPU.
        /// Note that using [[Color32]] data with ::SetPixels32 function can be faster and consume less memory. The array of pixels is ordered x, y, z. With for-loops have x in the centre, y in the middle and z on the outside to create the array of pixels.
        /// SA: GetPixel, GetPixelBilinear, GetPixels, GetPixels32, SetPixel, SetPixels32, Apply functions.
        /// </description>
        [FreeFunction(Name = "Texture3DScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetPixels(Color[] colors, int miplevel);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetPixels(Color[] colors)
        {
            SetPixels(colors, 0);
        }

        /// <summary>
        /// Sets pixel colors of a 3D texture.
        /// </summary>
        /// <param name="colors">
        /// The colors to set the pixels to.
        /// </param>
        /// <param name="miplevel">
        /// The mipmap level to be affected by the new colors.
        /// </param>
        /// <description>
        /// This function takes a [[Color32]] array and changes the pixel colors of the 3D texture.
        /// Call Apply to actually upload the changed pixels to the GPU.
        /// SA: GetPixel, GetPixelBilinear, GetPixels32, SetPixel, SetPixels, SetPixels32, Apply functions.
        /// </description>
        [FreeFunction(Name = "Texture3DScripting::SetPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetPixels32(Color32[] colors, int miplevel);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetPixels32(Color32[] colors)
        {
            SetPixels32(colors, 0);
        }
    }

    /// <summary>
    /// Class for handling 2D texture arrays.
    /// </summary>
    /// <description>
    /// Modern graphics APIs (e.g. D3D10 and later, OpenGL ES 3.0 and later, Metal etc.) support "texture arrays", which is an array of same size & format textures.
    /// From the shader side, they are treated as a single resource, and sampling them needs an extra coordinate that indicates which array element to sample from.
    /// Typically texture arrays are useful as an alternative for texture atlases, or in other cases where objects use a set of same-sized textures (e.g. terrains).
    /// Currently in Unity texture arrays do not have an import pipeline for them, and must be created from code, either at runtime or in editor scripts.
    /// Using [[Graphics.CopyTexture]] is useful for fast copying of pixel data from regular 2D textures into elements of a texture array. From editor scripts,
    /// a common way of creating serialized texture array is to create it, fill with data (either via [[Graphics.CopyTexture]] from regular 2D textures, or via SetPixels or SetPixels32) 
    [NativeHeader("Runtime/Graphics/Texture2DArray.h")]
    public sealed partial class Texture2DArray : Texture
    {
        /// <summary>
        /// Read Only. This property is used as a parameter in some overloads of the CommandBuffer.Blit, Graphics.Blit, CommandBuffer.SetRenderTarget, and Graphics.SetRenderTarget methods to indicate that all texture array slices are bound. The value of this property is -1.
        /// </summary>
        extern static public int allSlices { [NativeName("GetAllTextureLayersIdentifier")] get; }
        /// <summary>
        /// Number of elements in a texture array (RO).
        /// </summary>
        /// <description>
        /// SA: [[Texture.width]], [[Texture.height]].
        /// </description>
        extern public int depth { [NativeName("GetTextureLayerCount")] get; }
        /// <summary>
        /// Texture format (RO).
        /// </summary>
        extern public TextureFormat format { [NativeName("GetTextureFormat")] get; }

        /// <summary>
        /// Returns true if this texture array is Read/Write Enabled; otherwise returns false. For dynamic textures created from script, always returns true.
        /// </summary>
        extern override public bool isReadable { get; }

        [FreeFunction("Texture2DArrayScripting::Create")]
        extern private static bool Internal_CreateImpl([Writable] Texture2DArray mono, int w, int h, int d, GraphicsFormat format, TextureCreationFlags flags);
        private static void Internal_Create([Writable] Texture2DArray mono, int w, int h, int d, GraphicsFormat format, TextureCreationFlags flags)
        {
            if (!Internal_CreateImpl(mono, w, h, d, format, flags))
                throw new UnityException("Failed to create 2D array texture because of invalid parameters.");
        }

        [FreeFunction(Name = "Texture2DArrayScripting::Apply", HasExplicitThis = true)]
        extern private void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

        /// <summary>
        /// Returns pixel colors of a single array slice.
        /// </summary>
        /// <param name="arrayElement">
        /// Array slice to read pixels from.
        /// </param>
        /// <param name="miplevel">
        /// Mipmap level to read pixels from.
        /// </param>
        /// <returns>
        /// Array of pixel colors.
        /// </returns>
        /// <description>
        /// Note that using [[Color32]] data and GetPixels32 can be faster and consume less memory.
        /// SA: GetPixels32, SetPixels, [[Graphics.CopyTexture]].
        /// </description>
        [FreeFunction(Name = "Texture2DArrayScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public Color[] GetPixels(int arrayElement, int miplevel);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Color[] GetPixels(int arrayElement)
        {
            return GetPixels(arrayElement, 0);
        }

        /// <summary>
        /// Returns pixel colors of a single array slice.
        /// </summary>
        /// <param name="arrayElement">
        /// Array slice to read pixels from.
        /// </param>
        /// <param name="miplevel">
        /// Mipmap level to read pixels from.
        /// </param>
        /// <returns>
        /// Array of pixel colors in low precision (8 bits/channel) format.
        /// </returns>
        /// <description>
        /// SA: GetPixels, SetPixels32, [[Graphics.CopyTexture]].
        /// </description>
        [FreeFunction(Name = "Texture2DArrayScripting::GetPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern public Color32[] GetPixels32(int arrayElement, int miplevel);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Color32[] GetPixels32(int arrayElement)
        {
            return GetPixels32(arrayElement, 0);
        }

        /// <summary>
        /// Set pixel colors for the whole mip level.
        /// </summary>
        /// <param name="colors">
        /// An array of pixel colors.
        /// </param>
        /// <param name="arrayElement">
        /// The texture array element index.
        /// </param>
        /// <param name="miplevel">
        /// The mip level.
        /// </param>
        /// <description>
        /// This function takes a color array and changes the pixel colors of the whole
        /// mip level of the texture. Call Apply to actually upload the changed
        /// pixels to the graphics card.
        /// SA: SetPixels32, GetPixels, [[Texture2D.SetPixels]], [[Graphics.CopyTexture]].
        /// </description>
        [FreeFunction(Name = "Texture2DArrayScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetPixels(Color[] colors, int arrayElement, int miplevel);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetPixels(Color[] colors, int arrayElement)
        {
            SetPixels(colors, arrayElement, 0);
        }

        /// <summary>
        /// Set pixel colors for the whole mip level.
        /// </summary>
        /// <param name="colors">
        /// An array of pixel colors.
        /// </param>
        /// <param name="arrayElement">
        /// The texture array element index.
        /// </param>
        /// <param name="miplevel">
        /// The mip level.
        /// </param>
        /// <description>
        /// This function takes a color array and changes the pixel colors of the whole
        /// mip level of the texture. Call Apply to actually upload the changed
        /// pixels to the graphics card.
        /// This is a variant of ::SetPixels that takes low precision [[Color32]] pixel values.
        /// SA: SetPixels, ::GetPixels32, [[Texture2D.SetPixels]], [[Graphics.CopyTexture]].
        /// </description>
        [FreeFunction(Name = "Texture2DArrayScripting::SetPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetPixels32(Color32[] colors, int arrayElement, int miplevel);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetPixels32(Color32[] colors, int arrayElement)
        {
            SetPixels32(colors, arrayElement, 0);
        }
    }

    [NativeHeader("Runtime/Graphics/CubemapArrayTexture.h")]
    public sealed partial class CubemapArray : Texture
    {
        extern public int cubemapCount { get; }
        extern public TextureFormat format { [NativeName("GetTextureFormat")] get; }

        extern override public bool isReadable { get; }

        [FreeFunction("CubemapArrayScripting::Create")]
        extern private static bool Internal_CreateImpl([Writable] CubemapArray mono, int ext, int count, GraphicsFormat format, TextureCreationFlags flags);
        private static void Internal_Create([Writable] CubemapArray mono, int ext, int count, GraphicsFormat format, TextureCreationFlags flags)
        {
            if (!Internal_CreateImpl(mono, ext, count, format, flags))
                throw new UnityException("Failed to create cubemap array texture because of invalid parameters.");
        }

        [FreeFunction(Name = "CubemapArrayScripting::Apply", HasExplicitThis = true)]
        extern private void ApplyImpl(bool updateMipmaps, bool makeNoLongerReadable);

        [FreeFunction(Name = "CubemapArrayScripting::GetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public Color[] GetPixels(CubemapFace face, int arrayElement, int miplevel);

        public Color[] GetPixels(CubemapFace face, int arrayElement)
        {
            return GetPixels(face, arrayElement, 0);
        }

        [FreeFunction(Name = "CubemapArrayScripting::GetPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern public Color32[] GetPixels32(CubemapFace face, int arrayElement, int miplevel);

        public Color32[] GetPixels32(CubemapFace face, int arrayElement)
        {
            return GetPixels32(face, arrayElement, 0);
        }

        [FreeFunction(Name = "CubemapArrayScripting::SetPixels", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetPixels(Color[] colors, CubemapFace face, int arrayElement, int miplevel);

        public void SetPixels(Color[] colors, CubemapFace face, int arrayElement)
        {
            SetPixels(colors, face, arrayElement, 0);
        }

        [FreeFunction(Name = "CubemapArrayScripting::SetPixels32", HasExplicitThis = true, ThrowsException = true)]
        extern public void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement, int miplevel);

        public void SetPixels32(Color32[] colors, CubemapFace face, int arrayElement)
        {
            SetPixels32(colors, face, arrayElement, 0);
        }
    }

    [NativeHeader("Runtime/Graphics/SparseTexture.h")]
    public sealed partial class SparseTexture : Texture
    {
        extern public int tileWidth { get; }
        extern public int tileHeight { get; }
        extern public bool isCreated { [NativeName("IsInitialized")] get; }

        [FreeFunction(Name = "SparseTextureScripting::Create", ThrowsException = true)]
        extern private static void Internal_Create([Writable] SparseTexture mono, int width, int height, GraphicsFormat format, int mipCount);

        [FreeFunction(Name = "SparseTextureScripting::UpdateTile", HasExplicitThis = true)]
        extern public void UpdateTile(int tileX, int tileY, int miplevel, Color32[] data);

        [FreeFunction(Name = "SparseTextureScripting::UpdateTileRaw", HasExplicitThis = true)]
        extern public void UpdateTileRaw(int tileX, int tileY, int miplevel, byte[] data);

        public void UnloadTile(int tileX, int tileY, int miplevel)
        {
            UpdateTileRaw(tileX, tileY, miplevel, null);
        }
    }

    [NativeHeader("Runtime/Graphics/RenderTexture.h")]
    [NativeHeader("Runtime/Graphics/RenderBufferManager.h")]
    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    [NativeHeader("Runtime/Camera/Camera.h")]
    [UsedByNativeCode]
    public partial class RenderTexture : Texture
    {
        /// <summary>
        /// The width of the render texture in pixels.
        /// </summary>
        /// <description>
        /// Note that unlike [[Texture.width]] property, this is both read and write -
        /// setting a value changes size.
        /// SA:: height, format, depth.
        /// </description>
        override extern public int width { get; set; }
        /// <summary>
        /// The height of the render texture in pixels.
        /// </summary>
        /// <description>
        /// Note that unlike [[Texture.height]] property, this is both read and write -
        /// setting a value changes size.
        /// SA:: width, format, depth.
        /// </description>
        override extern public int height { get; set; }
        /// <summary>
        /// Dimensionality (type) of the render texture.
        /// </summary>
        /// <description>
        /// By default render textures are "2D" type, but it is also possible to have
        /// Cubemap or 3D render textures by changing dimension before they are created.
        /// [[Cubemap]] render textures are most often used for dynamic cubemap reflections, see [[Camera.RenderToCubemap]].
        /// A cubemap render texture must have the same width and height, and must be power of two size.
        /// 3D (volumetric) render textures currently only work on compute shader capable platforms (like [[wiki:UsingDX11GL3Features]]). You can render into them using "random access writes" from a pixel shader or a compute shader. Use volumeDepth to set 3D depth, and enableRandomWrite to enable arbitrary writes into it.
        /// SA: [[TextureDimension]].
        /// </description>
        override extern public TextureDimension dimension { get; set; }

        /// <summary>
        /// The color format of the render texture.
        /// </summary>
        /// <description>
        /// SA: [[GraphicsFormat]].
        /// </description>
        [NativeProperty("ColorFormat")]             extern public new GraphicsFormat graphicsFormat { get; set; }
        /// <summary>
        /// Render texture has mipmaps when this flag is set.
        /// </summary>
        /// <description>
        /// When set to /true/, rendering into this render texture will create and generate mipmap levels. By default
        /// render textures don't have mipmaps. This flag can be used only on render textures
        /// that have power-of-two size.
        /// By default the mipmaps will be automatically generated. If you want to render into texture mip levels manually, set autoGenerateMips to false.
        /// SA: autoGenerateMips, GenerateMips.
        /// </description>
        [NativeProperty("MipMap")]                  extern public bool useMipMap { get; set; }
        /// <summary>
        /// Does this render texture use sRGB read/write conversions? (RO).
        /// </summary>
        /// <description>
        /// When [[wiki:LinearLighting|Linear color space]] is used, render textures can perform Linear to sRGB conversions when rendering into them and sRGB to Linear conversions when sampling them in the shaders.
        /// The value of this property is based on the "readWrite" parameter of the [[RenderTexture]] constructor.
        /// SA: [[RenderTextureReadWrite]] for more details.
        /// </description>
        [NativeProperty("SRGBReadWrite")]           extern public bool sRGB { get; }
        /// <summary>
        /// If this RenderTexture is a VR eye texture used in stereoscopic rendering, this property decides what special rendering occurs, if any.
        /// </summary>
        [NativeProperty("VRUsage")]                 extern public VRTextureUsage vrUsage { get; set; }
        /// <summary>
        /// The render texture memoryless mode property.
        /// </summary>
        /// <description>
        /// Use this property to set or return the render texture memoryless modes.
        /// Memoryless render textures are temporarily stored in the on-tile memory when it is rendered. It does not get stored in CPU or GPU memory. This reduces memory usage of your app but note that you cannot read or write to these render textures.
        /// On-tile memory is a high speed dedicated memory used by mobile GPUs when rendering.
        /// Note that memoryless render textures are only supported on iOS/tvOS 10.0+ Metal and Vulkan. Render textures are read/write protected and stored in CPU or GPU memory on other platforms.
        /// SA. [[RenderTextureMemoryless]].
        /// </description>
        [NativeProperty("Memoryless")]              extern public RenderTextureMemoryless memorylessMode { get; set; }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTextureFormat format
        {
            get { return GraphicsFormatUtility.GetRenderTextureFormat(graphicsFormat); }
            set { graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(value, sRGB); }
        }

        /// <summary>
        /// Mipmap levels are generated automatically when this flag is set.
        /// </summary>
        /// <description>
        /// When a render texture is with mipmaps (.useMipMap), then by default rendering into it generates all the mipmap levels. The mipmap
        /// generation happens automatically only after rendering into this render texture; when active render target is switched to some other render texture.
        /// If you want to render into mip levels manually, or control when mipmap generation happens (via GenerateMips), set this variable to false.
        /// Default value is true.
        /// SA: useMipMap, GenerateMips.
        /// </description>
        extern public bool autoGenerateMips { get; set; }
        /// <summary>
        /// Volume extent of a 3D render texture or number of slices of array texture.
        /// </summary>
        /// <description>
        /// For volumetric render textures (see dimension), this variable determines the volume extent.
        /// For array render texture (see dimension), this variable determines the number of slices.
        /// SA: dimension.
        /// </description>
        extern public int volumeDepth { get; set; }
        /// <summary>
        /// The antialiasing level for the RenderTexture.
        /// </summary>
        /// <description>
        /// Anti-aliasing value indicates the number of samples per pixel. If unsupported by the hardware or rendering API, the greatest supported number of samples less than the indicated number is used.
        /// When a RenderTexture is using anti-aliasing, then any rendering into it will happen into the multi-sampled
        /// texture, which will be "resolved" into a regular texture when switching to another render target. To the rest
        /// of the system only this "resolved" surface is visible.
        /// </description>
        extern public int antiAliasing { get; set; }
        /// <summary>
        /// If true and antiAliasing is greater than 1, the render texture will not be resolved by default.  Use this if the render texture needs to be bound as a multisampled texture in a shader.
        /// </summary>
        extern public bool bindTextureMS { get; set; }
        /// <summary>
        /// Enable random access write into this render texture on Shader Model 5.0 level shaders.
        /// </summary>
        /// <description>
        /// Shader Model 5.0 level pixel or compute shaders can write into arbitrary locations of some textures, called "unordered access views" in [[wiki:UsingDX11GL3Features]]. Set this flag before creating your render texture to enable this capability.
        /// When a texture has this flag set, it can be written into as one RWTexture* resources in HLSL or image resources in GLSL. It can also be set as random access write target for pixel shaders using [[Graphics.SetRandomWriteTarget]].
        /// SA: [[Graphics.SetRandomWriteTarget]], [[wiki:UsingDX11GL3Features]].
        /// </description>
        extern public bool enableRandomWrite { get; set; }
        /// <summary>
        /// Is the render texture marked to be scaled by the Dynamic Resolution system.
        /// </summary>
        extern public bool useDynamicScale { get; set; }


        // for some reason we are providing isPowerOfTwo setter which is empty (i dont know what the intent is/was)
        extern private bool GetIsPowerOfTwo();
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public bool isPowerOfTwo { get { return GetIsPowerOfTwo(); } set {} }


        [FreeFunction("RenderTexture::GetActive")] extern private static RenderTexture GetActive();
        [FreeFunction("RenderTextureScripting::SetActive")] extern private static void SetActive(RenderTexture rt);
        /// <summary>
        /// Currently active render texture.
        /// </summary>
        /// <description>
        /// All rendering goes into the active RenderTexture.
        /// If the active RenderTexture is /null/ everything is rendered in the main window.
        /// Setting [[RenderTexture.active]] is the same as calling [[Graphics.SetRenderTarget]]. Typically
        /// you change or query the active render texture when implementing custom graphics effects;
        /// if all you need is to make a Camera render into a texture then use [[Camera.targetTexture]]
        /// instead.
        /// When a RenderTexture becomes active its hardware rendering context is automatically created if
        /// it hasn't been created already.
        /// SA: [[Graphics.SetRenderTarget]].
        /// </description>
        public static RenderTexture active { get { return GetActive(); } set { SetActive(value); } }

        [FreeFunction(Name = "RenderTextureScripting::GetColorBuffer", HasExplicitThis = true)]
        extern private RenderBuffer GetColorBuffer();
        [FreeFunction(Name = "RenderTextureScripting::GetDepthBuffer", HasExplicitThis = true)]
        extern private RenderBuffer GetDepthBuffer();

        /// <summary>
        /// Color buffer of the render texture (RO).
        /// </summary>
        /// <description>
        /// SA: [[RenderBuffer]], depthBuffer, [[Graphics.SetRenderTarget]], [[Graphics.Blit]].
        /// </description>
        public RenderBuffer colorBuffer { get { return GetColorBuffer(); } }
        /// <summary>
        /// Depth/stencil buffer of the render texture (RO).
        /// </summary>
        /// <description>
        /// SA: [[RenderBuffer]], colorBuffer, [[Graphics.SetRenderTarget]], [[Graphics.Blit]].
        /// </description>
        public RenderBuffer depthBuffer { get { return GetDepthBuffer(); } }

        /// <summary>
        /// Retrieve a native (underlying graphics API) pointer to the depth buffer resource.
        /// </summary>
        /// <returns>
        /// Pointer to an underlying graphics API depth buffer resource.
        /// </returns>
        /// <description>
        /// Use this function to retrieve a pointer/handle corresponding to the depth buffer
        /// part of the RenderTexture, as it is represented on the native graphics API level.
        /// This can be used to enable depth buffer manipulation from [[wiki:NativePluginInterface|native code plugins]].
        /// Use [[Texture.GetNativeTexturePtr]] to get a native pointer to the color buffer
        /// of a render texture, and this function to get to the depth buffer part. For Depth and ShadowMap
        /// render texture formats, the two functions return the same resource.  The two functions will also return
        /// the same resource if anti aliasing is enabled in the project's quality settings.
        /// Note that calling this function when using multi-threaded rendering will synchronize with the rendering
        /// thread (a slow operation), so best practice is to set up needed texture pointers only at initialization
        /// time.
        /// SA: [[Texture.GetNativeTexturePtr]], [[wiki:NativePluginInterface|Native code plugins]].
        /// </description>
        extern public IntPtr GetNativeDepthBufferPtr();


        /// <summary>
        /// Hint the GPU driver that the contents of the RenderTexture will not be used.
        /// </summary>
        /// <param name="discardColor">
        /// Should the colour buffer be discarded?
        /// </param>
        /// <param name="discardDepth">
        /// Should the depth buffer be discarded?
        /// </param>
        /// <description>
        /// On some platforms, it can be good for performance if you indicate when the
        /// current contents of a RenderTexture aren't needed any more. This can save
        /// copying it from one kind of memory to another when the texture is reused.
        /// Xbox 360, XBox One and many mobile GPUs benefit from this.
        /// This call is typically only meaningful when the given RenderTexture is currently
        /// an active render target. After this call, the contents of the RenderTexture are
        /// undefined, so the user should not attempt to access its contents before either
        /// clearing the RenderTexture or drawing into each pixel of it.
        /// Both the colour buffer and depth buffer are discarded by default but either can be selected individually using the optional boolean parameters.
        /// </description>
        extern public void DiscardContents(bool discardColor, bool discardDepth);
        /// <summary>
        /// Indicate that there's a RenderTexture restore operation expected.
        /// </summary>
        /// <description>
        /// When in mobile graphics emulation mode, Unity issues warnings when a RenderTexture "restore" operation is performed. Restore happens when rendering into a texture, without clearing or discarding (.DiscardContents) it first. This is a costly operation on many mobile GPUs and multi-GPU systems and best should be avoided.
        /// However, if your rendering effect absolutely needs a RenderTexture restore, you can call this function to indicate that yes, a restore is expected, and Unity will not issue a warning here.
        /// </description>
        extern public void MarkRestoreExpected();
        /// <summary>
        /// Hint the GPU driver that the contents of the RenderTexture will not be used.
        /// </summary>
        /// <description>
        /// On some platforms, it can be good for performance if you indicate when the
        /// current contents of a RenderTexture aren't needed any more. This can save
        /// copying it from one kind of memory to another when the texture is reused.
        /// Xbox 360, XBox One and many mobile GPUs benefit from this.
        /// This call is typically only meaningful when the given RenderTexture is currently
        /// an active render target. After this call, the contents of the RenderTexture are
        /// undefined, so the user should not attempt to access its contents before either
        /// clearing the RenderTexture or drawing into each pixel of it.
        /// Both the colour buffer and depth buffer are discarded by default but either can be selected individually using the optional boolean parameters.
        /// </description>
        public void DiscardContents() { DiscardContents(true, true); }


        [NativeName("ResolveAntiAliasedSurface")] extern private void ResolveAA();
        [NativeName("ResolveAntiAliasedSurface")] extern private void ResolveAATo(RenderTexture rt);

        /// <summary>
        /// Force an antialiased render texture to be resolved.
        /// </summary>
        /// <description>
        /// If an antialiased render texture has the bindTextureMS flag set, it will not be automatically resolved.  Sometimes, it's useful to have both the resolved and the unresolved version of the texture at different stages of the pipeline.
        /// If the target parameter is omitted, the render texture will be resolved into itself.
        /// </description>
        public void ResolveAntiAliasedSurface() { ResolveAA(); }
        /// <summary>
        /// Force an antialiased render texture to be resolved.
        /// </summary>
        /// <param name="target">
        /// The render texture to resolve into.  If set, the target render texture must have the same dimensions and format as the source.
        /// </param>
        /// <description>
        /// If an antialiased render texture has the bindTextureMS flag set, it will not be automatically resolved.  Sometimes, it's useful to have both the resolved and the unresolved version of the texture at different stages of the pipeline.
        /// If the target parameter is omitted, the render texture will be resolved into itself.
        /// </description>
        public void ResolveAntiAliasedSurface(RenderTexture target) { ResolveAATo(target); }


        /// <summary>
        /// Assigns this RenderTexture as a global shader property named /propertyName/.
        /// </summary>
        [FreeFunction(Name = "RenderTextureScripting::SetGlobalShaderProperty", HasExplicitThis = true)]
        extern public void SetGlobalShaderProperty(string propertyName);


        /// <summary>
        /// Actually creates the RenderTexture.
        /// </summary>
        /// <returns>
        /// True if the texture is created, else false.
        /// </returns>
        /// <description>
        /// RenderTexture constructor does not actually create the hardware texture;
        /// by default the texture is created the first time it is set active.
        /// Calling @@Create@@ lets you create it up front. @@Create@@ does nothing
        /// if the texture is already created.
        /// SA: Release, IsCreated functions.
        /// </description>
        extern public bool Create();
        /// <summary>
        /// Releases the RenderTexture.
        /// </summary>
        /// <description>
        /// This function releases the hardware resources used by the render texture.
        /// The texture itself is not destroyed, and will be automatically
        /// created again when being used.
        /// As with other "native engine object" types, it is important to pay attention to the lifetime of any
        /// render textures and release them when you are finished using them, as they will not be garbage
        /// collected like normal managed types.
        /// SA: Create, IsCreated functions.
        /// </description>
        extern public void Release();
        /// <summary>
        /// Is the render texture actually created?
        /// </summary>
        /// <description>
        /// RenderTexture constructor does not actually create the hardware texture;
        /// by default the texture is created the first time it is set active.
        /// @@IsCreated@@ returns /true/ if the hardware resources for this render
        /// are created.
        /// SA: Create, Release functions.
        /// </description>
        extern public bool IsCreated();
        /// <summary>
        /// Generate mipmap levels of a render texture.
        /// </summary>
        /// <description>
        /// Use this function to manually re-generate mipmap levels of a render texture. The render texture has to have mipmaps (.useMipMap set to true),
        /// and automatic mip generation turned off (.autoGenerateMips set to false).
        /// On some platforms (most notably, D3D9), there is no way to manually generate render texture mip levels; in these cases this function does nothing.
        /// SA: useMipMap, autoGenerateMips.
        /// </description>
        extern public void GenerateMips();
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        extern public void ConvertToEquirect(RenderTexture equirect, Camera.MonoOrStereoscopicEye eye = Camera.MonoOrStereoscopicEye.Mono);

        extern internal void SetSRGBReadWrite(bool srgb);

        [FreeFunction("RenderTextureScripting::Create")] extern private static void Internal_Create([Writable] RenderTexture rt);

        /// <summary>
        /// Does a RenderTexture have stencil buffer?
        /// </summary>
        /// <param name="rt">
        /// Render texture, or null for main screen.
        /// </param>
        /// <description>
        /// If rt is null, will report the status for the main screen.
        /// </description>
        [FreeFunction("RenderTextureSupportsStencil")] extern public static bool SupportsStencil(RenderTexture rt);

        [NativeName("SetRenderTextureDescFromScript")]
        extern private void SetRenderTextureDescriptor(RenderTextureDescriptor desc);

        [NativeName("GetRenderTextureDesc")]
        extern private RenderTextureDescriptor GetDescriptor();

        [FreeFunction("GetRenderBufferManager().GetTextures().GetTempBuffer")]
        extern private static RenderTexture GetTemporary_Internal(RenderTextureDescriptor desc);


        /// <summary>
        /// Release a temporary texture allocated with GetTemporary.
        /// </summary>
        /// <description>
        /// Later calls to GetTemporary will reuse the RenderTexture created earlier if possible.
        /// When no one has requested the temporary RenderTexture for a few frames it will be destroyed.
        /// SA: GetTemporary function.
        /// </description>
        [FreeFunction("GetRenderBufferManager().GetTextures().ReleaseTempBuffer")]
        extern public static void ReleaseTemporary(RenderTexture temp);

        /// <summary>
        /// The precision of the render texture's depth buffer in bits (0, 16, 24/32 are supported).
        /// </summary>
        /// <description>
        /// When 0 is used, then no Z buffer is created by a render texture.
        /// 16 means at least 16 bit Z buffer and no stencil buffer.
        /// 24 or 32 means at least 24 bit Z buffer, and a stencil buffer.
        /// When requesting 24 bit Z Unity will prefer 32 bit floating point Z buffer if available on the platform.
        /// SA: format, width, height.
        /// </description>
        extern public int depth
        {
            [FreeFunction("RenderTextureScripting::GetDepth", HasExplicitThis = true)]
            get;
            [FreeFunction("RenderTextureScripting::SetDepth", HasExplicitThis = true)]
            set;
        }
    }

    [System.Serializable]
    [UsedByNativeCode]
    public struct CustomRenderTextureUpdateZone
    {
        public Vector3 updateZoneCenter;
        public Vector3 updateZoneSize;
        public float rotation;
        public int passIndex;
        public bool needSwap;
    }

    [UsedByNativeCode]
    [NativeHeader("Runtime/Graphics/CustomRenderTexture.h")]
    public sealed partial class CustomRenderTexture : RenderTexture
    {
        [FreeFunction(Name = "CustomRenderTextureScripting::Create")]
        extern private static void Internal_CreateCustomRenderTexture([Writable] CustomRenderTexture rt);

        [NativeName("TriggerUpdate")]
        extern public void Update(int count);

        public void Update()
        {
            Update(1);
        }

        [NativeName("TriggerInitialization")]
        extern public void Initialize();

        extern public void ClearUpdateZones();

        extern public Material material { get; set; }

        extern public Material initializationMaterial { get; set; }

        extern public Texture initializationTexture { get; set; }

        [FreeFunction(Name = "CustomRenderTextureScripting::GetUpdateZonesInternal", HasExplicitThis = true)]
        extern internal void GetUpdateZonesInternal([NotNull] object updateZones);

        public void GetUpdateZones(List<CustomRenderTextureUpdateZone> updateZones)
        {
            GetUpdateZonesInternal(updateZones);
        }

        [FreeFunction(Name = "CustomRenderTextureScripting::SetUpdateZonesInternal", HasExplicitThis = true)]
        extern private void SetUpdateZonesInternal(CustomRenderTextureUpdateZone[] updateZones);

        public void SetUpdateZones(CustomRenderTextureUpdateZone[] updateZones)
        {
            if (updateZones == null)
                throw new ArgumentNullException("updateZones");

            SetUpdateZonesInternal(updateZones);
        }

        extern public CustomRenderTextureInitializationSource initializationSource { get; set; }
        extern public Color initializationColor { get; set; }
        extern public CustomRenderTextureUpdateMode updateMode { get; set; }
        extern public CustomRenderTextureUpdateMode initializationMode { get; set; }
        extern public CustomRenderTextureUpdateZoneSpace updateZoneSpace { get; set; }
        extern public int shaderPass { get; set; }
        extern public uint cubemapFaceMask { get; set; }
        extern public bool doubleBuffered { get; set; }
        extern public bool wrapUpdateZones { get; set; }
    }
}
