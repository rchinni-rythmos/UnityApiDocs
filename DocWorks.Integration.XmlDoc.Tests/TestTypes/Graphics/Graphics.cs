using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using uei = UnityEngine.Internal;

namespace UnityEngine
{
    /// <summary>
    /// Represents a display resolution.
    /// </summary>
    /// <description>
    /// Resolution structures are returned by [[Screen.resolutions]] property.
    /// </description>
    [RequiredByNativeCode]
    public struct Resolution
    {
        // Keep in sync with ScreenManager::Resolution
        private int m_Width;
        private int m_Height;
        private int m_RefreshRate;

        /// <summary>
        /// Resolution width in pixels.
        /// </summary>
        public int width        { get { return m_Width; } set { m_Width = value; } }
        /// <summary>
        /// Resolution height in pixels.
        /// </summary>
        public int height       { get { return m_Height; } set { m_Height = value; } }
        /// <summary>
        /// Resolution's vertical refresh rate in Hz.
        /// </summary>
        public int refreshRate  { get { return m_RefreshRate; } set { m_RefreshRate = value; } }

        /// <summary>
        /// Returns a nicely formatted string of the resolution.
        /// </summary>
        /// <returns>
        /// A string with the format "width x height @ refreshRateHz".
        /// </returns>
        public override string ToString()
        {
            return UnityString.Format("{0} x {1} @ {2}Hz", m_Width, m_Height, m_RefreshRate);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public partial struct RenderBuffer
    {
        internal int m_RenderTextureInstanceID;
        internal IntPtr m_BufferPtr;

        internal RenderBufferLoadAction  loadAction  { get { return GetLoadAction();  } set { SetLoadAction(value); } }
        internal RenderBufferStoreAction storeAction { get { return GetStoreAction(); } set { SetStoreAction(value); } }
    }

    /// <summary>
    /// Fully describes setup of RenderTarget.
    /// </summary>
    public struct RenderTargetSetup
    {
        /// <summary>
        /// Color Buffers to set.
        /// </summary>
        public RenderBuffer[]   color;
        /// <summary>
        /// Depth Buffer to set.
        /// </summary>
        public RenderBuffer     depth;

        /// <summary>
        /// Mip Level to render to.
        /// </summary>
        public int              mipLevel;
        /// <summary>
        /// Cubemap face to render to.
        /// </summary>
        public CubemapFace      cubemapFace;
        /// <summary>
        /// Slice of a [[Texture3D]] or [[Texture2DArray]] to set as a render target.
        /// </summary>
        /// <description>
        /// Some platforms (e.g. D3D11) support setting -1 as the slice, which binds whole render target for rendering. Then typically a geometry
        /// shader is used to direct rendering into the appropriate slice.
        /// </description>
        public int              depthSlice;

        /// <summary>
        /// Load Actions for Color Buffers. It will override any actions set on RenderBuffers themselves.
        /// </summary>
        /// <description>
        /// Please note that not all platforms have load/store actions, so this setting might be ignored at runtime. Generally mobile-oriented graphics APIs (OpenGL ES, Metal) take advantage of these settings.
        /// </description>
        public Rendering.RenderBufferLoadAction[]   colorLoad;
        /// <summary>
        /// Store Actions for Color Buffers. It will override any actions set on RenderBuffers themselves.
        /// </summary>
        /// <description>
        /// Please note that not all platforms have load/store actions, so this setting might be ignored at runtime. Generally mobile-oriented graphics APIs (OpenGL ES, Metal) take advantage of these settings.
        /// </description>
        public Rendering.RenderBufferStoreAction[]  colorStore;

        /// <summary>
        /// Load Action for Depth Buffer. It will override any actions set on RenderBuffer itself.
        /// </summary>
        /// <description>
        /// Please note that not all platforms have load/store actions, so this setting might be ignored at runtime. Generally mobile-oriented graphics APIs (OpenGL ES, Metal) take advantage of these settings.
        /// </description>
        public Rendering.RenderBufferLoadAction     depthLoad;
        /// <summary>
        /// Store Actions for Depth Buffer. It will override any actions set on RenderBuffer itself.
        /// </summary>
        /// <description>
        /// Please note that not all platforms have load/store actions, so this setting might be ignored at runtime. Generally mobile-oriented graphics APIs (OpenGL ES, Metal) take advantage of these settings.
        /// </description>
        public Rendering.RenderBufferStoreAction    depthStore;


        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTargetSetup(
            RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face,
            Rendering.RenderBufferLoadAction[] colorLoad, Rendering.RenderBufferStoreAction[] colorStore,
            Rendering.RenderBufferLoadAction depthLoad, Rendering.RenderBufferStoreAction depthStore
        )
        {
            this.color          = color;
            this.depth          = depth;

            this.mipLevel       = mip;
            this.cubemapFace    = face;
            this.depthSlice     = 0;

            this.colorLoad      = colorLoad;
            this.colorStore     = colorStore;

            this.depthLoad      = depthLoad;
            this.depthStore     = depthStore;
        }

        internal static Rendering.RenderBufferLoadAction[] LoadActions(RenderBuffer[] buf)
        {
            // preserve old discard behaviour: render surface flags are applied only on first activation
            // this will be used only in ctor without load/store actions specified
            Rendering.RenderBufferLoadAction[] ret = new Rendering.RenderBufferLoadAction[buf.Length];
            for (int i = 0; i < buf.Length; ++i)
            {
                ret[i] = buf[i].loadAction;
                buf[i].loadAction = Rendering.RenderBufferLoadAction.Load;
            }
            return ret;
        }

        internal static Rendering.RenderBufferStoreAction[] StoreActions(RenderBuffer[] buf)
        {
            // preserve old discard behaviour: render surface flags are applied only on first activation
            // this will be used only in ctor without load/store actions specified
            Rendering.RenderBufferStoreAction[] ret = new Rendering.RenderBufferStoreAction[buf.Length];
            for (int i = 0; i < buf.Length; ++i)
            {
                ret[i] = buf[i].storeAction;
                buf[i].storeAction = Rendering.RenderBufferStoreAction.Store;
            }
            return ret;
        }

        // TODO: when we enable default arguments support these can be combined into one method
        /// <summary>
        /// Constructs RenderTargetSetup.
        /// </summary>
        /// <param name="color">
        /// Color Buffer(s) to set.
        /// </param>
        /// <param name="depth">
        /// Depth Buffer to set.
        /// </param>
        public RenderTargetSetup(RenderBuffer color, RenderBuffer depth)
            : this(new RenderBuffer[] { color }, depth)
        {
        }

        /// <summary>
        /// Constructs RenderTargetSetup.
        /// </summary>
        /// <param name="color">
        /// Color Buffer(s) to set.
        /// </param>
        /// <param name="depth">
        /// Depth Buffer to set.
        /// </param>
        /// <param name="mipLevel">
        /// Mip Level to render to.
        /// </param>
        public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel)
            : this(new RenderBuffer[] { color }, depth, mipLevel)
        {
        }

        /// <summary>
        /// Constructs RenderTargetSetup.
        /// </summary>
        /// <param name="color">
        /// Color Buffer(s) to set.
        /// </param>
        /// <param name="depth">
        /// Depth Buffer to set.
        /// </param>
        /// <param name="mipLevel">
        /// Mip Level to render to.
        /// </param>
        /// <param name="face">
        /// Cubemap face to render to.
        /// </param>
        public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face)
            : this(new RenderBuffer[] { color }, depth, mipLevel, face)
        {
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTargetSetup(RenderBuffer color, RenderBuffer depth, int mipLevel, CubemapFace face, int depthSlice)
            : this(new RenderBuffer[] { color }, depth, mipLevel, face)
        {
            this.depthSlice = depthSlice;
        }

        // TODO: when we enable default arguments support these can be combined into one method
        /// <summary>
        /// Constructs RenderTargetSetup.
        /// </summary>
        /// <param name="color">
        /// Color Buffer(s) to set.
        /// </param>
        /// <param name="depth">
        /// Depth Buffer to set.
        /// </param>
        public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth)
            : this(color, depth, 0, CubemapFace.Unknown)
        {
        }

        /// <summary>
        /// Constructs RenderTargetSetup.
        /// </summary>
        /// <param name="color">
        /// Color Buffer(s) to set.
        /// </param>
        /// <param name="depth">
        /// Depth Buffer to set.
        /// </param>
        /// <param name="mipLevel">
        /// Mip Level to render to.
        /// </param>
        public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mipLevel)
            : this(color, depth, mipLevel, CubemapFace.Unknown)
        {
        }

        /// <summary>
        /// Constructs RenderTargetSetup.
        /// </summary>
        /// <param name="color">
        /// Color Buffer(s) to set.
        /// </param>
        /// <param name="depth">
        /// Depth Buffer to set.
        /// </param>
        /// <param name="face">
        /// Cubemap face to render to.
        /// </param>
        public RenderTargetSetup(RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face)
            : this(color, depth, mip, face, LoadActions(color), StoreActions(color), depth.loadAction, depth.storeAction)
        {
        }
    }
}


//
// Graphics.SetRenderTarget
//


namespace UnityEngine
{
    public partial class Graphics
    {
        internal static void CheckLoadActionValid(Rendering.RenderBufferLoadAction load, string bufferType)
        {
            if (load != Rendering.RenderBufferLoadAction.Load && load != Rendering.RenderBufferLoadAction.DontCare)
                throw new ArgumentException(UnityString.Format("Bad {0} LoadAction provided.", bufferType));
        }

        internal static void CheckStoreActionValid(Rendering.RenderBufferStoreAction store, string bufferType)
        {
            if (store != Rendering.RenderBufferStoreAction.Store && store != Rendering.RenderBufferStoreAction.DontCare)
                throw new ArgumentException(UnityString.Format("Bad {0} StoreAction provided.", bufferType));
        }

        internal static void SetRenderTargetImpl(RenderTargetSetup setup)
        {
            if (setup.color.Length == 0)
                throw new ArgumentException("Invalid color buffer count for SetRenderTarget");
            if (setup.color.Length != setup.colorLoad.Length)
                throw new ArgumentException("Color LoadAction and Buffer arrays have different sizes");
            if (setup.color.Length != setup.colorStore.Length)
                throw new ArgumentException("Color StoreAction and Buffer arrays have different sizes");

            foreach (var load in setup.colorLoad)
                CheckLoadActionValid(load, "Color");
            foreach (var store in setup.colorStore)
                CheckStoreActionValid(store, "Color");

            CheckLoadActionValid(setup.depthLoad, "Depth");
            CheckStoreActionValid(setup.depthStore, "Depth");

            if ((int)setup.cubemapFace < (int)CubemapFace.Unknown || (int)setup.cubemapFace > (int)CubemapFace.NegativeZ)
                throw new ArgumentException("Bad CubemapFace provided");

            Internal_SetMRTFullSetup(
                setup.color, setup.depth, setup.mipLevel, setup.cubemapFace, setup.depthSlice,
                setup.colorLoad, setup.colorStore, setup.depthLoad, setup.depthStore
            );
        }

        internal static void SetRenderTargetImpl(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel, CubemapFace face, int depthSlice)
        {
            Internal_SetRTSimple(colorBuffer, depthBuffer, mipLevel, face, depthSlice);
        }

        internal static void SetRenderTargetImpl(RenderTexture rt, int mipLevel, CubemapFace face, int depthSlice)
        {
            if (rt) SetRenderTargetImpl(rt.colorBuffer, rt.depthBuffer, mipLevel, face, depthSlice);
            else    Internal_SetNullRT();
        }

        internal static void SetRenderTargetImpl(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer, int mipLevel, CubemapFace face, int depthSlice)
        {
            RenderBuffer depth = depthBuffer;
            Internal_SetMRTSimple(colorBuffers, depth, mipLevel, face, depthSlice);
        }

        /// <summary>
        /// Sets current render target.
        /// </summary>
        /// <param name="rt">
        /// [[RenderTexture]] to set as active render target.
        /// </param>
        /// <param name="mipLevel">
        /// Mipmap level to render into (use 0 if not mipmapped).
        /// </param>
        /// <param name="face">
        /// Cubemap face to render into (use Unknown if not a cubemap).
        /// </param>
        /// <param name="depthSlice">
        /// Depth slice to render into (use 0 if not a 3D or 2DArray render target).
        /// </param>
        /// <description>
        /// This function sets which [[RenderTexture]] or a [[RenderBuffer]] combination will be
        /// rendered into next. Use it when implementing custom rendering algorithms, where
        /// you need to render something into a render texture manually.
        /// Variants with mipLevel and face arguments enable rendering into a specific mipmap level
        /// of a render texture, or specific cubemap face of a cubemap RenderTexture.
        /// Variants with depthSlice allow rendering into a specific slice of a 3D or 2DArray render texture.
        /// The function call with colorBuffers array enables techniques that use
        /// Multiple Render Targets (MRT), where fragment shader can output more
        /// than one final color.
        /// Calling SetRenderTarget with just a RenderTexture argument is the same as setting [[RenderTexture.active]] property.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// SA: [[RenderTexture]], [[Graphics.activeColorBuffer]], [[Graphics.activeDepthBuffer]], [[SystemInfo.supportedRenderTargetCount]].
        /// </description>
        public static void SetRenderTarget(RenderTexture rt, [uei.DefaultValue("0")] int mipLevel, [uei.DefaultValue("CubemapFace.Unknown")] CubemapFace face, [uei.DefaultValue("0")] int depthSlice)
        {
            SetRenderTargetImpl(rt, mipLevel, face, depthSlice);
        }

        /// <summary>
        /// Sets current render target.
        /// </summary>
        /// <param name="colorBuffer">
        /// Color buffer to render into.
        /// </param>
        /// <param name="depthBuffer">
        /// Depth buffer to render into.
        /// </param>
        /// <param name="mipLevel">
        /// Mipmap level to render into (use 0 if not mipmapped).
        /// </param>
        /// <param name="face">
        /// Cubemap face to render into (use Unknown if not a cubemap).
        /// </param>
        /// <param name="depthSlice">
        /// Depth slice to render into (use 0 if not a 3D or 2DArray render target).
        /// </param>
        /// <description>
        /// This function sets which [[RenderTexture]] or a [[RenderBuffer]] combination will be
        /// rendered into next. Use it when implementing custom rendering algorithms, where
        /// you need to render something into a render texture manually.
        /// Variants with mipLevel and face arguments enable rendering into a specific mipmap level
        /// of a render texture, or specific cubemap face of a cubemap RenderTexture.
        /// Variants with depthSlice allow rendering into a specific slice of a 3D or 2DArray render texture.
        /// The function call with colorBuffers array enables techniques that use
        /// Multiple Render Targets (MRT), where fragment shader can output more
        /// than one final color.
        /// Calling SetRenderTarget with just a RenderTexture argument is the same as setting [[RenderTexture.active]] property.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// SA: [[RenderTexture]], [[Graphics.activeColorBuffer]], [[Graphics.activeDepthBuffer]], [[SystemInfo.supportedRenderTargetCount]].
        /// </description>
        public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, [uei.DefaultValue("0")] int mipLevel, [uei.DefaultValue("CubemapFace.Unknown")] CubemapFace face, [uei.DefaultValue("0")] int depthSlice)
        {
            SetRenderTargetImpl(colorBuffer, depthBuffer, mipLevel, face, depthSlice);
        }

        /// <summary>
        /// Sets current render target.
        /// </summary>
        /// <param name="colorBuffers">
        /// Color buffers to render into (for multiple render target effects).
        /// </param>
        /// <param name="depthBuffer">
        /// Depth buffer to render into.
        /// </param>
        /// <description>
        /// This function sets which [[RenderTexture]] or a [[RenderBuffer]] combination will be
        /// rendered into next. Use it when implementing custom rendering algorithms, where
        /// you need to render something into a render texture manually.
        /// Variants with mipLevel and face arguments enable rendering into a specific mipmap level
        /// of a render texture, or specific cubemap face of a cubemap RenderTexture.
        /// Variants with depthSlice allow rendering into a specific slice of a 3D or 2DArray render texture.
        /// The function call with colorBuffers array enables techniques that use
        /// Multiple Render Targets (MRT), where fragment shader can output more
        /// than one final color.
        /// Calling SetRenderTarget with just a RenderTexture argument is the same as setting [[RenderTexture.active]] property.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// SA: [[RenderTexture]], [[Graphics.activeColorBuffer]], [[Graphics.activeDepthBuffer]], [[SystemInfo.supportedRenderTargetCount]].
        /// </description>
        public static void SetRenderTarget(RenderBuffer[] colorBuffers, RenderBuffer depthBuffer)
        {
            SetRenderTargetImpl(colorBuffers, depthBuffer, 0, CubemapFace.Unknown, 0);
        }

        /// <summary>
        /// Sets current render target.
        /// </summary>
        /// <param name="setup">
        /// Full render target setup information.
        /// </param>
        /// <description>
        /// This function sets which [[RenderTexture]] or a [[RenderBuffer]] combination will be
        /// rendered into next. Use it when implementing custom rendering algorithms, where
        /// you need to render something into a render texture manually.
        /// Variants with mipLevel and face arguments enable rendering into a specific mipmap level
        /// of a render texture, or specific cubemap face of a cubemap RenderTexture.
        /// Variants with depthSlice allow rendering into a specific slice of a 3D or 2DArray render texture.
        /// The function call with colorBuffers array enables techniques that use
        /// Multiple Render Targets (MRT), where fragment shader can output more
        /// than one final color.
        /// Calling SetRenderTarget with just a RenderTexture argument is the same as setting [[RenderTexture.active]] property.
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing SetRenderTarget or any other
        /// manual rendering.
        /// SA: [[RenderTexture]], [[Graphics.activeColorBuffer]], [[Graphics.activeDepthBuffer]], [[SystemInfo.supportedRenderTargetCount]].
        /// </description>
        public static void SetRenderTarget(RenderTargetSetup setup)
        {
            SetRenderTargetImpl(setup);
        }
    }

    public partial class Graphics
    {
        /// <summary>
        /// Currently active color buffer (RO).
        /// </summary>
        /// <description>
        /// SA: [[RenderBuffer]], [[Graphics.activeDepthBuffer]], [[Graphics.SetRenderTarget]], [[Graphics.Blit]].
        /// </description>
        public static RenderBuffer activeColorBuffer { get { return GetActiveColorBuffer(); } }
        /// <summary>
        /// Currently active depth/stencil buffer (RO).
        /// </summary>
        /// <description>
        /// SA: [[RenderBuffer]], [[Graphics.activeColorBuffer]], [[Graphics.SetRenderTarget]], [[Graphics.Blit]].
        /// </description>
        public static RenderBuffer activeDepthBuffer { get { return GetActiveDepthBuffer(); } }

        /// <summary>
        /// Set random write target for [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level pixel shaders.
        /// </summary>
        /// <param name="index">
        /// Index of the random write target in the shader.
        /// </param>
        /// <param name="uav">
        /// RenderTexture to set as write target.
        /// </param>
        /// <description>
        /// Shader Model 4.5 and above level pixel shaders can write into arbitrary locations of some textures and buffers, called "unordered access views" (UAV) in [[wiki:UsingDX11GL3Features]]. These "random write" targets are set similarly to how multiple render targets are set. You can either use a [[RenderTexture]] with /enableRandomWrite/ flag set, or a [[ComputeBuffer]] as target.
        /// The UAV indexing varies a bit between different platforms. On DX11 the first valid UAV index is the number of active render targets. So the common case of single render target the UAV indexing will start from 1. Platforms using automatically translated HLSL shaders will match this behaviour. However, with hand-written GLSL shaders the indexes will match the bindings. On PS4 the indexing starts always from 1 to match the most common case.
        /// When setting a [[ComputeBuffer]], the /preserveCounterValue/ parameter indicates whether to leave the counter value unchanged, or reset it to 0 (the default behaviour).
        /// The targets stay set until you manually clear them with ClearRandomWriteTargets.
        /// SA: [[RenderTexture.enableRandomWrite]], [[ComputeBuffer]], [[ComputeBuffer.SetCounterValue]], [[wiki:UsingDX11GL3Features]].
        /// </description>
        public static void SetRandomWriteTarget(int index, RenderTexture uav)
        {
            if (index < 0 || index >= SystemInfo.supportedRandomWriteTargetCount)
                throw new ArgumentOutOfRangeException("index", string.Format("must be non-negative less than {0}.", SystemInfo.supportedRandomWriteTargetCount));

            Internal_SetRandomWriteTargetRT(index, uav);
        }

        /// <summary>
        /// Set random write target for [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level pixel shaders.
        /// </summary>
        /// <param name="index">
        /// Index of the random write target in the shader.
        /// </param>
        /// <param name="uav">
        /// RenderTexture to set as write target.
        /// </param>
        /// <param name="preserveCounterValue">
        /// Whether to leave the append/consume counter value unchanged.
        /// </param>
        /// <description>
        /// Shader Model 4.5 and above level pixel shaders can write into arbitrary locations of some textures and buffers, called "unordered access views" (UAV) in [[wiki:UsingDX11GL3Features]]. These "random write" targets are set similarly to how multiple render targets are set. You can either use a [[RenderTexture]] with /enableRandomWrite/ flag set, or a [[ComputeBuffer]] as target.
        /// The UAV indexing varies a bit between different platforms. On DX11 the first valid UAV index is the number of active render targets. So the common case of single render target the UAV indexing will start from 1. Platforms using automatically translated HLSL shaders will match this behaviour. However, with hand-written GLSL shaders the indexes will match the bindings. On PS4 the indexing starts always from 1 to match the most common case.
        /// When setting a [[ComputeBuffer]], the /preserveCounterValue/ parameter indicates whether to leave the counter value unchanged, or reset it to 0 (the default behaviour).
        /// The targets stay set until you manually clear them with ClearRandomWriteTargets.
        /// SA: [[RenderTexture.enableRandomWrite]], [[ComputeBuffer]], [[ComputeBuffer.SetCounterValue]], [[wiki:UsingDX11GL3Features]].
        /// </description>
        public static void SetRandomWriteTarget(int index, ComputeBuffer uav, [uei.DefaultValue("false")] bool preserveCounterValue)
        {
            if (uav == null) throw new ArgumentNullException("uav");
            if (uav.m_Ptr == IntPtr.Zero) throw new System.ObjectDisposedException("uav");
            if (index < 0 || index >= SystemInfo.supportedRandomWriteTargetCount)
                throw new ArgumentOutOfRangeException("index", string.Format("must be non-negative less than {0}.", SystemInfo.supportedRandomWriteTargetCount));

            Internal_SetRandomWriteTargetBuffer(index, uav, preserveCounterValue);
        }

        /// <summary>
        /// Copy texture contents.
        /// </summary>
        /// <param name="src">
        /// Source texture.
        /// </param>
        /// <param name="dst">
        /// Destination texture.
        /// </param>
        /// <description>
        /// This function allows copying pixel data from one texture into another efficiently.
        /// It also allows copying from an element (e.g. cubemap face) or a specific mip level, and from a subregion of a texture.
        /// Copying does not do any scaling, i.e. source and destination sizes must be the same. Texture formats should be compatible
        /// (for example, [[TextureFormat.ARGB32]] and [[RenderTextureFormat.ARGB32]] are compatible). Exact rules for which formats
        /// are compatible vary a bit between graphics APIs; generally formats that are exactly the same can always be copied.
        /// On some platforms (e.g. D3D11) you can also copy between formats that are of the same bit width.
        /// Compressed texture formats add some restrictions to the CopyTexture with a region variant. For example, PVRTC formats
        /// are not supported since they are not block-based (for these formats you can only copy whole texture or whole mip level).
        /// For block-based formats (e.g. DXT, ETC), the region size and coordinates must be a multiple of compression block size
        /// (4 pixels for DXT).
        /// If both source and destination textures are marked as "readable" (i.e. copy of data exists
        /// in system memory for reading/writing on the CPU), these functions copy it as well.
        /// Some platforms might not have functionality of all sorts of texture copying (e.g. copy from a
        /// render texture into a regular texture). See [[Rendering.CopyTextureSupport]], and use
        /// [[SystemInfo.copyTextureSupport]] to check.
        /// Calling [[Texture2D.Apply]], [[Texture2DArray.Apply]] or [[Texture3D.Apply]] after /CopyTexture/ yields undefined results as /CopyTexture/ operates on GPU-side data exclusively, whereas /Apply/ transfers data from CPU to GPU-side.
        /// SA: [[Rendering.CopyTextureSupport]].
        /// </description>
        public static void CopyTexture(Texture src, Texture dst)
        {
            CopyTexture_Full(src, dst);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void CopyTexture(Texture src, int srcElement, Texture dst, int dstElement)
        {
            CopyTexture_Slice_AllMips(src, srcElement, dst, dstElement);
        }

        /// <summary>
        /// Copy texture contents.
        /// </summary>
        /// <param name="src">
        /// Source texture.
        /// </param>
        /// <param name="srcElement">
        /// Source texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="srcMip">
        /// Source texture mipmap level.
        /// </param>
        /// <param name="dst">
        /// Destination texture.
        /// </param>
        /// <param name="dstElement">
        /// Destination texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="dstMip">
        /// Destination texture mipmap level.
        /// </param>
        /// <description>
        /// This function allows copying pixel data from one texture into another efficiently.
        /// It also allows copying from an element (e.g. cubemap face) or a specific mip level, and from a subregion of a texture.
        /// Copying does not do any scaling, i.e. source and destination sizes must be the same. Texture formats should be compatible
        /// (for example, [[TextureFormat.ARGB32]] and [[RenderTextureFormat.ARGB32]] are compatible). Exact rules for which formats
        /// are compatible vary a bit between graphics APIs; generally formats that are exactly the same can always be copied.
        /// On some platforms (e.g. D3D11) you can also copy between formats that are of the same bit width.
        /// Compressed texture formats add some restrictions to the CopyTexture with a region variant. For example, PVRTC formats
        /// are not supported since they are not block-based (for these formats you can only copy whole texture or whole mip level).
        /// For block-based formats (e.g. DXT, ETC), the region size and coordinates must be a multiple of compression block size
        /// (4 pixels for DXT).
        /// If both source and destination textures are marked as "readable" (i.e. copy of data exists
        /// in system memory for reading/writing on the CPU), these functions copy it as well.
        /// Some platforms might not have functionality of all sorts of texture copying (e.g. copy from a
        /// render texture into a regular texture). See [[Rendering.CopyTextureSupport]], and use
        /// [[SystemInfo.copyTextureSupport]] to check.
        /// Calling [[Texture2D.Apply]], [[Texture2DArray.Apply]] or [[Texture3D.Apply]] after /CopyTexture/ yields undefined results as /CopyTexture/ operates on GPU-side data exclusively, whereas /Apply/ transfers data from CPU to GPU-side.
        /// SA: [[Rendering.CopyTextureSupport]].
        /// </description>
        public static void CopyTexture(Texture src, int srcElement, int srcMip, Texture dst, int dstElement, int dstMip)
        {
            CopyTexture_Slice(src, srcElement, srcMip, dst, dstElement, dstMip);
        }

        /// <summary>
        /// Copy texture contents.
        /// </summary>
        /// <param name="src">
        /// Source texture.
        /// </param>
        /// <param name="srcElement">
        /// Source texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="srcMip">
        /// Source texture mipmap level.
        /// </param>
        /// <param name="srcX">
        /// X coordinate of source texture region to copy (left side is zero).
        /// </param>
        /// <param name="srcY">
        /// Y coordinate of source texture region to copy (bottom is zero).
        /// </param>
        /// <param name="srcWidth">
        /// Width of source texture region to copy.
        /// </param>
        /// <param name="srcHeight">
        /// Height of source texture region to copy.
        /// </param>
        /// <param name="dst">
        /// Destination texture.
        /// </param>
        /// <param name="dstElement">
        /// Destination texture element (cubemap face, texture array layer or 3D texture depth slice).
        /// </param>
        /// <param name="dstMip">
        /// Destination texture mipmap level.
        /// </param>
        /// <param name="dstX">
        /// X coordinate of where to copy region in destination texture (left side is zero).
        /// </param>
        /// <param name="dstY">
        /// Y coordinate of where to copy region in destination texture (bottom is zero).
        /// </param>
        /// <description>
        /// This function allows copying pixel data from one texture into another efficiently.
        /// It also allows copying from an element (e.g. cubemap face) or a specific mip level, and from a subregion of a texture.
        /// Copying does not do any scaling, i.e. source and destination sizes must be the same. Texture formats should be compatible
        /// (for example, [[TextureFormat.ARGB32]] and [[RenderTextureFormat.ARGB32]] are compatible). Exact rules for which formats
        /// are compatible vary a bit between graphics APIs; generally formats that are exactly the same can always be copied.
        /// On some platforms (e.g. D3D11) you can also copy between formats that are of the same bit width.
        /// Compressed texture formats add some restrictions to the CopyTexture with a region variant. For example, PVRTC formats
        /// are not supported since they are not block-based (for these formats you can only copy whole texture or whole mip level).
        /// For block-based formats (e.g. DXT, ETC), the region size and coordinates must be a multiple of compression block size
        /// (4 pixels for DXT).
        /// If both source and destination textures are marked as "readable" (i.e. copy of data exists
        /// in system memory for reading/writing on the CPU), these functions copy it as well.
        /// Some platforms might not have functionality of all sorts of texture copying (e.g. copy from a
        /// render texture into a regular texture). See [[Rendering.CopyTextureSupport]], and use
        /// [[SystemInfo.copyTextureSupport]] to check.
        /// Calling [[Texture2D.Apply]], [[Texture2DArray.Apply]] or [[Texture3D.Apply]] after /CopyTexture/ yields undefined results as /CopyTexture/ operates on GPU-side data exclusively, whereas /Apply/ transfers data from CPU to GPU-side.
        /// SA: [[Rendering.CopyTextureSupport]].
        /// </description>
        public static void CopyTexture(Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture dst, int dstElement, int dstMip, int dstX, int dstY)
        {
            CopyTexture_Region(src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight, dst, dstElement, dstMip, dstX, dstY);
        }

        /// <summary>
        /// This function provides an efficient way to convert between textures of different formats and dimensions.
        /// The destination texture format should be uncompressed and correspond to a supported [[RenderTextureFormat]].
        /// </summary>
        /// <param name="src">
        /// Source texture.
        /// </param>
        /// <param name="dst">
        /// Destination texture.
        /// </param>
        /// <returns>
        /// True if the call succeeded.
        /// </returns>
        /// <description>
        /// Currently supported are 2d and cubemap textures as the source, and 2d, cubemap, 2d array and cubemap array textures as the destination.
        /// Please note that due to API limitations, this function is not supported on DX9 or Mac+OpenGL.
        /// </description>
        public static bool ConvertTexture(Texture src, Texture dst)
        {
            return ConvertTexture_Full(src, dst);
        }

        /// <summary>
        /// This function provides an efficient way to convert between textures of different formats and dimensions.
        /// The destination texture format should be uncompressed and correspond to a supported [[RenderTextureFormat]].
        /// </summary>
        /// <param name="src">
        /// Source texture.
        /// </param>
        /// <param name="srcElement">
        /// Source element (e.g. cubemap face).  Set this to 0 for 2d source textures.
        /// </param>
        /// <param name="dst">
        /// Destination texture.
        /// </param>
        /// <param name="dstElement">
        /// Destination element (e.g. cubemap face or texture array element).
        /// </param>
        /// <returns>
        /// True if the call succeeded.
        /// </returns>
        /// <description>
        /// Currently supported are 2d and cubemap textures as the source, and 2d, cubemap, 2d array and cubemap array textures as the destination.
        /// Please note that due to API limitations, this function is not supported on DX9 or Mac+OpenGL.
        /// </description>
        public static bool ConvertTexture(Texture src, int srcElement, Texture dst, int dstElement)
        {
            return ConvertTexture_Slice(src, srcElement, dst, dstElement);
        }

        /// <summary>
        /// Shortcut for calling [[Graphics.CreateGraphicsFence]] with [[GraphicsFenceType.AsyncQueueSynchronization]] as the first parameter.
        /// </summary>
        /// <param name="stage">
        /// The synchronization stage. See [[Graphics.CreateGraphicsFence]].
        /// </param>
        /// <returns>
        /// Returns a new [[GraphicsFence]].
        /// </returns>
        public static GraphicsFence CreateAsyncGraphicsFence([uei.DefaultValue("SynchronisationStage.PixelProcessing")] SynchronisationStage stage)
        {
            return CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, GraphicsFence.TranslateSynchronizationStageToFlags(stage));
        }

        /// <summary>
        /// Shortcut for calling [[Graphics.CreateGraphicsFence]] with [[GraphicsFenceType.AsyncQueueSynchronization]] as the first parameter.
        /// </summary>
        /// <returns>
        /// Returns a new [[GraphicsFence]].
        /// </returns>
        public static GraphicsFence CreateAsyncGraphicsFence()
        {
            return CreateGraphicsFence(GraphicsFenceType.AsyncQueueSynchronisation, SynchronisationStageFlags.PixelProcessing);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static GraphicsFence CreateGraphicsFence(GraphicsFenceType fenceType, [uei.DefaultValue("SynchronisationStage.PixelProcessing")] SynchronisationStageFlags stage)
        {
            GraphicsFence newFence = new GraphicsFence();
            newFence.m_FenceType = fenceType;
            newFence.m_Ptr = CreateGPUFenceImpl(fenceType, stage);
            newFence.InitPostAllocation();
            newFence.Validate();
            return newFence;
        }

        /// <summary>
        /// Instructs the GPU's processing of the graphics queue to wait until the given [[GraphicsFence]] is passed.
        /// </summary>
        /// <param name="fence">
        /// The [[GraphicsFence]] that the GPU will be instructed to wait upon before proceeding with its processing of the graphics queue.
        /// </param>
        /// <description>
        /// Some platforms can not differentiate between the start of vertex and pixel processing, these platforms will wait before the next items vertex processing regardless of the value passed to the stage parameter.
        /// The [[GraphicsFence]] object given as a parameter to this function must be created with a [[GraphicsFenceType.AsyncQueueSynchronization]] fence type.
        /// On platforms which do not support GraphicsFences, this call does nothing see [[SystemInfo.supportsGraphicsFence]].
        /// It is possible for the user to create GPU deadlocks with this function. Care should be taken to ensure that the [[GraphicsFence]] passed can be completed before the GPU is instructed to wait.
        /// This function returns immediately on the CPU, only GPU processing is effected by the fence.
        /// SA: [[GraphicsFence]], [[Graphics.CreateGraphicsFence]].
        /// </description>
        public static void WaitOnAsyncGraphicsFence(GraphicsFence fence)
        {
            WaitOnAsyncGraphicsFence(fence, SynchronisationStage.PixelProcessing);
        }

        /// <summary>
        /// Instructs the GPU's processing of the graphics queue to wait until the given [[GraphicsFence]] is passed.
        /// </summary>
        /// <param name="fence">
        /// The [[GraphicsFence]] that the GPU will be instructed to wait upon before proceeding with its processing of the graphics queue.
        /// </param>
        /// <param name="stage">
        /// On some platforms there is a significant gap between the vertex processing completing and the pixel processing begining for a given draw call. This parameter allows for requested wait to be before the next items vertex or pixel processing begins. If a compute shader dispatch is the next item to be submitted then this parameter is ignored.
        /// </param>
        /// <description>
        /// Some platforms can not differentiate between the start of vertex and pixel processing, these platforms will wait before the next items vertex processing regardless of the value passed to the stage parameter.
        /// The [[GraphicsFence]] object given as a parameter to this function must be created with a [[GraphicsFenceType.AsyncQueueSynchronization]] fence type.
        /// On platforms which do not support GraphicsFences, this call does nothing see [[SystemInfo.supportsGraphicsFence]].
        /// It is possible for the user to create GPU deadlocks with this function. Care should be taken to ensure that the [[GraphicsFence]] passed can be completed before the GPU is instructed to wait.
        /// This function returns immediately on the CPU, only GPU processing is effected by the fence.
        /// SA: [[GraphicsFence]], [[Graphics.CreateGraphicsFence]].
        /// </description>
        public static void WaitOnAsyncGraphicsFence(GraphicsFence fence, [uei.DefaultValue("SynchronisationStage.PixelProcessing")] SynchronisationStage stage)
        {
            if (fence.m_FenceType != GraphicsFenceType.AsyncQueueSynchronisation)
                throw new ArgumentException("Graphics.WaitOnGraphicsFence can only be called with fences created with GraphicsFenceType.AsyncQueueSynchronization.");

            fence.Validate();

            //Don't wait on a fence that's already known to have passed
            if (fence.IsFencePending())
                WaitOnGPUFenceImpl(fence.m_Ptr, GraphicsFence.TranslateSynchronizationStageToFlags(stage));
        }
    }
}


//
// Graphics.Draw*
//


namespace UnityEngine
{
    [VisibleToOtherModules("UnityEngine.IMGUIModule")]
    internal struct Internal_DrawTextureArguments
    {
        public Rect screenRect, sourceRect;
        public int leftBorder, rightBorder, topBorder, bottomBorder;
        public Color color;
        public Vector4 borderWidths;
        public Vector4 cornerRadiuses;
        public int pass;
        public Texture texture;
        public Material mat;
    }


    public partial class Graphics
    {
        private static void DrawTextureImpl(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, Material mat, int pass)
        {
            Internal_DrawTextureArguments args = new Internal_DrawTextureArguments();
            args.screenRect = screenRect; args.sourceRect = sourceRect;
            args.leftBorder = leftBorder; args.rightBorder = rightBorder; args.topBorder = topBorder; args.bottomBorder = bottomBorder;
            args.color = color;
            args.pass = pass;
            args.texture = texture;
            args.mat = mat;

            Internal_DrawTexture(ref args);
        }

        /// <summary>
        /// Draw a texture in screen coordinates.
        /// </summary>
        /// <param name="screenRect">
        /// Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
        /// </param>
        /// <param name="texture">
        /// [[Texture]] to draw.
        /// </param>
        /// <param name="sourceRect">
        /// Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
        /// </param>
        /// <param name="leftBorder">
        /// Number of pixels from the left that are not affected by scale.
        /// </param>
        /// <param name="rightBorder">
        /// Number of pixels from the right that are not affected by scale.
        /// </param>
        /// <param name="topBorder">
        /// Number of pixels from the top that are not affected by scale.
        /// </param>
        /// <param name="bottomBorder">
        /// Number of pixels from the bottom that are not affected by scale.
        /// </param>
        /// <param name="color">
        /// [[Color]] that modulates the output. The neutral value is (0.5, 0.5, 0.5, 0.5). Set as vertex color for the shader.
        /// </param>
        /// <param name="mat">
        /// Custom [[Material]] that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
        /// </param>
        /// <param name="pass">
        /// If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
        /// </param>
        /// <description>
        /// If you want to draw a texture from inside of OnGUI code, you should only do that from [[EventType.Repaint]]
        /// events. It's probably better to use [[GUI.DrawTexture]] for GUI code.
        /// </description>
        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, [uei.DefaultValue("null")] Material mat, [uei.DefaultValue("-1")] int pass)
        {
            DrawTextureImpl(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat, pass);
        }

        /// <summary>
        /// Draw a texture in screen coordinates.
        /// </summary>
        /// <param name="screenRect">
        /// Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
        /// </param>
        /// <param name="texture">
        /// [[Texture]] to draw.
        /// </param>
        /// <param name="sourceRect">
        /// Region of the texture to use. In normalized coordinates with (0,0) in the bottom-left corner.
        /// </param>
        /// <param name="leftBorder">
        /// Number of pixels from the left that are not affected by scale.
        /// </param>
        /// <param name="rightBorder">
        /// Number of pixels from the right that are not affected by scale.
        /// </param>
        /// <param name="topBorder">
        /// Number of pixels from the top that are not affected by scale.
        /// </param>
        /// <param name="bottomBorder">
        /// Number of pixels from the bottom that are not affected by scale.
        /// </param>
        /// <param name="mat">
        /// Custom [[Material]] that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
        /// </param>
        /// <param name="pass">
        /// If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
        /// </param>
        /// <description>
        /// If you want to draw a texture from inside of OnGUI code, you should only do that from [[EventType.Repaint]]
        /// events. It's probably better to use [[GUI.DrawTexture]] for GUI code.
        /// </description>
        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [uei.DefaultValue("null")] Material mat, [uei.DefaultValue("-1")] int pass)
        {
            Color32 color = new Color32(128, 128, 128, 128);
            DrawTextureImpl(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat, pass);
        }

        /// <summary>
        /// Draw a texture in screen coordinates.
        /// </summary>
        /// <param name="screenRect">
        /// Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
        /// </param>
        /// <param name="texture">
        /// [[Texture]] to draw.
        /// </param>
        /// <param name="leftBorder">
        /// Number of pixels from the left that are not affected by scale.
        /// </param>
        /// <param name="rightBorder">
        /// Number of pixels from the right that are not affected by scale.
        /// </param>
        /// <param name="topBorder">
        /// Number of pixels from the top that are not affected by scale.
        /// </param>
        /// <param name="bottomBorder">
        /// Number of pixels from the bottom that are not affected by scale.
        /// </param>
        /// <param name="mat">
        /// Custom [[Material]] that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
        /// </param>
        /// <param name="pass">
        /// If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
        /// </param>
        /// <description>
        /// If you want to draw a texture from inside of OnGUI code, you should only do that from [[EventType.Repaint]]
        /// events. It's probably better to use [[GUI.DrawTexture]] for GUI code.
        /// </description>
        public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [uei.DefaultValue("null")] Material mat, [uei.DefaultValue("-1")] int pass)
        {
            DrawTexture(screenRect, texture, new Rect(0, 0, 1, 1), leftBorder, rightBorder, topBorder, bottomBorder, mat, pass);
        }

        /// <summary>
        /// Draw a texture in screen coordinates.
        /// </summary>
        /// <param name="screenRect">
        /// Rectangle on the screen to use for the texture. In pixel coordinates with (0,0) in the upper-left corner.
        /// </param>
        /// <param name="texture">
        /// [[Texture]] to draw.
        /// </param>
        /// <param name="mat">
        /// Custom [[Material]] that can be used to draw the texture. If null is passed, a default material with the Internal-GUITexture.shader is used.
        /// </param>
        /// <param name="pass">
        /// If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
        /// </param>
        /// <description>
        /// If you want to draw a texture from inside of OnGUI code, you should only do that from [[EventType.Repaint]]
        /// events. It's probably better to use [[GUI.DrawTexture]] for GUI code.
        /// </description>
        public static void DrawTexture(Rect screenRect, Texture texture, [uei.DefaultValue("null")] Material mat, [uei.DefaultValue("-1")] int pass)
        {
            DrawTexture(screenRect, texture, 0, 0, 0, 0, mat, pass);
        }

        /// <summary>
        /// Draw a mesh immediately.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="position">
        /// Position of the mesh.
        /// </param>
        /// <param name="rotation">
        /// Rotation of the mesh.
        /// </param>
        /// <param name="materialIndex">
        /// Subset of the mesh to draw.
        /// </param>
        /// <description>
        /// This function will draw a given mesh immediately. Currently set shader and
        /// material (see [[Material.SetPass]]) will be used.
        /// The mesh will be just drawn once,
        /// it won't be per-pixel lit and will not cast or receive realtime shadows. If you want
        /// full integration with lighting and shadowing, use [[Graphics.DrawMesh]] instead.
        /// </description>
        /// <description>
        /// SA: [[Graphics.DrawMesh]], [[Material.SetPass]].
        /// </description>
        public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation, int materialIndex)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");
            Internal_DrawMeshNow1(mesh, materialIndex, position, rotation);
        }

        /// <summary>
        /// Draw a mesh immediately.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.
        /// </param>
        /// <param name="materialIndex">
        /// Subset of the mesh to draw.
        /// </param>
        /// <description>
        /// This function will draw a given mesh immediately. Currently set shader and
        /// material (see [[Material.SetPass]]) will be used.
        /// The mesh will be just drawn once,
        /// it won't be per-pixel lit and will not cast or receive realtime shadows. If you want
        /// full integration with lighting and shadowing, use [[Graphics.DrawMesh]] instead.
        /// </description>
        /// <description>
        /// SA: [[Graphics.DrawMesh]], [[Material.SetPass]].
        /// </description>
        public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix, int materialIndex)
        {
            if (mesh == null)
                throw new ArgumentNullException("mesh");
            Internal_DrawMeshNow2(mesh, materialIndex, matrix);
        }

        /// <summary>
        /// Draw a mesh immediately.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="position">
        /// Position of the mesh.
        /// </param>
        /// <param name="rotation">
        /// Rotation of the mesh.
        /// </param>
        /// <description>
        /// This function will draw a given mesh immediately. Currently set shader and
        /// material (see [[Material.SetPass]]) will be used.
        /// The mesh will be just drawn once,
        /// it won't be per-pixel lit and will not cast or receive realtime shadows. If you want
        /// full integration with lighting and shadowing, use [[Graphics.DrawMesh]] instead.
        /// </description>
        /// <description>
        /// SA: [[Graphics.DrawMesh]], [[Material.SetPass]].
        /// </description>
        public static void DrawMeshNow(Mesh mesh, Vector3 position, Quaternion rotation) { DrawMeshNow(mesh, position, rotation, -1); }
        /// <summary>
        /// Draw a mesh immediately.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix of the mesh (combines position, rotation and other transformations). Note that the mesh will not be displayed correctly if matrix has negative scale.
        /// </param>
        /// <description>
        /// This function will draw a given mesh immediately. Currently set shader and
        /// material (see [[Material.SetPass]]) will be used.
        /// The mesh will be just drawn once,
        /// it won't be per-pixel lit and will not cast or receive realtime shadows. If you want
        /// full integration with lighting and shadowing, use [[Graphics.DrawMesh]] instead.
        /// </description>
        /// <description>
        /// SA: [[Graphics.DrawMesh]], [[Material.SetPass]].
        /// </description>
        public static void DrawMeshNow(Mesh mesh, Matrix4x4 matrix) { DrawMeshNow(mesh, matrix, -1); }


        /// <summary>
        /// Draw a mesh.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="position">
        /// Position of the mesh.
        /// </param>
        /// <param name="rotation">
        /// Rotation of the mesh.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="layer">
        /// [[wiki:Layers|Layer]] to use.
        /// </param>
        /// <param name="camera">
        /// If /null/ (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply onto material just before this mesh will be drawn. See [[MaterialPropertyBlock]].
        /// </param>
        /// <param name="castShadows">
        /// Determines whether the mesh can cast shadows.
        /// </param>
        /// <param name="receiveShadows">
        /// Determines whether the mesh can receive shadows.
        /// </param>
        /// <param name="useLightProbes">
        /// Should the mesh use light probes?
        /// </param>
        /// <description>
        /// DrawMesh draws a mesh for one frame. The mesh will be affected by the lights, can cast and receive shadows and be
        /// affected by Projectors - just like it was part of some game object. It can be drawn for all cameras or just for
        /// some specific camera.
        /// Use DrawMesh in situations where you want to draw large amount of meshes, but don't want the overhead of creating and
        /// managing game objects. Note that DrawMesh does not draw the mesh immediately; it merely "submits" it for rendering. The mesh
        /// will be rendered as part of normal rendering process. If you want to draw a mesh immediately, use [[Graphics.DrawMeshNow]].
        /// Because DrawMesh does not draw mesh immediately, modifying material properties between calls to this function won't make
        /// the meshes pick up them. If you want to draw series of meshes with the same material, but slightly different
        /// properties (e.g. change color of each mesh), use [[MaterialPropertyBlock]] parameter.
        /// Note that this call will create some internal resources while the mesh is queued up for rendering. The allocation happens immediately and will be kept around until the end of frame (if the object was queued for all cameras) or until the specified camera renders itself.
        /// SA: [[MaterialPropertyBlock]].
        /// </description>
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, [uei.DefaultValue("null")] Camera camera, [uei.DefaultValue("0")] int submeshIndex, [uei.DefaultValue("null")] MaterialPropertyBlock properties, [uei.DefaultValue("true")] bool castShadows, [uei.DefaultValue("true")] bool receiveShadows, [uei.DefaultValue("true")] bool useLightProbes)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows, null, useLightProbes ? LightProbeUsage.BlendProbes : LightProbeUsage.Off, null);
        }

        /// <summary>
        /// Draw a mesh.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="position">
        /// Position of the mesh.
        /// </param>
        /// <param name="rotation">
        /// Rotation of the mesh.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="layer">
        /// [[wiki:Layers|Layer]] to use.
        /// </param>
        /// <param name="camera">
        /// If /null/ (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply onto material just before this mesh will be drawn. See [[MaterialPropertyBlock]].
        /// </param>
        /// <param name="castShadows">
        /// Determines whether the mesh can cast shadows.
        /// </param>
        /// <param name="receiveShadows">
        /// Determines whether the mesh can receive shadows.
        /// </param>
        /// <param name="probeAnchor">
        /// If used, the mesh will use this Transform's position to sample light probes and find the matching reflection probe.
        /// </param>
        /// <param name="useLightProbes">
        /// Should the mesh use light probes?
        /// </param>
        /// <description>
        /// DrawMesh draws a mesh for one frame. The mesh will be affected by the lights, can cast and receive shadows and be
        /// affected by Projectors - just like it was part of some game object. It can be drawn for all cameras or just for
        /// some specific camera.
        /// Use DrawMesh in situations where you want to draw large amount of meshes, but don't want the overhead of creating and
        /// managing game objects. Note that DrawMesh does not draw the mesh immediately; it merely "submits" it for rendering. The mesh
        /// will be rendered as part of normal rendering process. If you want to draw a mesh immediately, use [[Graphics.DrawMeshNow]].
        /// Because DrawMesh does not draw mesh immediately, modifying material properties between calls to this function won't make
        /// the meshes pick up them. If you want to draw series of meshes with the same material, but slightly different
        /// properties (e.g. change color of each mesh), use [[MaterialPropertyBlock]] parameter.
        /// Note that this call will create some internal resources while the mesh is queued up for rendering. The allocation happens immediately and will be kept around until the end of frame (if the object was queued for all cameras) or until the specified camera renders itself.
        /// SA: [[MaterialPropertyBlock]].
        /// </description>
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, [uei.DefaultValue("true")] bool receiveShadows, [uei.DefaultValue("null")] Transform probeAnchor, [uei.DefaultValue("true")] bool useLightProbes)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, useLightProbes ? LightProbeUsage.BlendProbes : LightProbeUsage.Off, null);
        }

        /// <summary>
        /// Draw a mesh.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix of the mesh (combines position, rotation and other transformations).
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="layer">
        /// [[wiki:Layers|Layer]] to use.
        /// </param>
        /// <param name="camera">
        /// If /null/ (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply onto material just before this mesh will be drawn. See [[MaterialPropertyBlock]].
        /// </param>
        /// <param name="castShadows">
        /// Determines whether the mesh can cast shadows.
        /// </param>
        /// <param name="receiveShadows">
        /// Determines whether the mesh can receive shadows.
        /// </param>
        /// <param name="useLightProbes">
        /// Should the mesh use light probes?
        /// </param>
        /// <description>
        /// DrawMesh draws a mesh for one frame. The mesh will be affected by the lights, can cast and receive shadows and be
        /// affected by Projectors - just like it was part of some game object. It can be drawn for all cameras or just for
        /// some specific camera.
        /// Use DrawMesh in situations where you want to draw large amount of meshes, but don't want the overhead of creating and
        /// managing game objects. Note that DrawMesh does not draw the mesh immediately; it merely "submits" it for rendering. The mesh
        /// will be rendered as part of normal rendering process. If you want to draw a mesh immediately, use [[Graphics.DrawMeshNow]].
        /// Because DrawMesh does not draw mesh immediately, modifying material properties between calls to this function won't make
        /// the meshes pick up them. If you want to draw series of meshes with the same material, but slightly different
        /// properties (e.g. change color of each mesh), use [[MaterialPropertyBlock]] parameter.
        /// Note that this call will create some internal resources while the mesh is queued up for rendering. The allocation happens immediately and will be kept around until the end of frame (if the object was queued for all cameras) or until the specified camera renders itself.
        /// SA: [[MaterialPropertyBlock]].
        /// </description>
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, [uei.DefaultValue("null")] Camera camera, [uei.DefaultValue("0")] int submeshIndex, [uei.DefaultValue("null")] MaterialPropertyBlock properties, [uei.DefaultValue("true")] bool castShadows, [uei.DefaultValue("true")] bool receiveShadows, [uei.DefaultValue("true")] bool useLightProbes)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows, null, useLightProbes ? LightProbeUsage.BlendProbes : LightProbeUsage.Off, null);
        }

        /// <summary>
        /// Draw a mesh.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="matrix">
        /// Transformation matrix of the mesh (combines position, rotation and other transformations).
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="layer">
        /// [[wiki:Layers|Layer]] to use.
        /// </param>
        /// <param name="camera">
        /// If /null/ (default), the mesh will be drawn in all cameras. Otherwise it will be rendered in the given camera only.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply onto material just before this mesh will be drawn. See [[MaterialPropertyBlock]].
        /// </param>
        /// <param name="castShadows">
        /// Determines whether the mesh can cast shadows.
        /// </param>
        /// <param name="receiveShadows">
        /// Determines whether the mesh can receive shadows.
        /// </param>
        /// <param name="probeAnchor">
        /// If used, the mesh will use this Transform's position to sample light probes and find the matching reflection probe.
        /// </param>
        /// <param name="lightProbeUsage">
        /// [[LightProbeUsage]] for the mesh.
        /// </param>
        /// <description>
        /// DrawMesh draws a mesh for one frame. The mesh will be affected by the lights, can cast and receive shadows and be
        /// affected by Projectors - just like it was part of some game object. It can be drawn for all cameras or just for
        /// some specific camera.
        /// Use DrawMesh in situations where you want to draw large amount of meshes, but don't want the overhead of creating and
        /// managing game objects. Note that DrawMesh does not draw the mesh immediately; it merely "submits" it for rendering. The mesh
        /// will be rendered as part of normal rendering process. If you want to draw a mesh immediately, use [[Graphics.DrawMeshNow]].
        /// Because DrawMesh does not draw mesh immediately, modifying material properties between calls to this function won't make
        /// the meshes pick up them. If you want to draw series of meshes with the same material, but slightly different
        /// properties (e.g. change color of each mesh), use [[MaterialPropertyBlock]] parameter.
        /// Note that this call will create some internal resources while the mesh is queued up for rendering. The allocation happens immediately and will be kept around until the end of frame (if the object was queued for all cameras) or until the specified camera renders itself.
        /// SA: [[MaterialPropertyBlock]].
        /// </description>
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage, [uei.DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
        {
            if (lightProbeUsage == LightProbeUsage.UseProxyVolume && lightProbeProxyVolume == null)
                throw new ArgumentException("Argument lightProbeProxyVolume must not be null if lightProbeUsage is set to UseProxyVolume.", "lightProbeProxyVolume");
            Internal_DrawMesh(mesh, submeshIndex, matrix, material, layer, camera, properties, castShadows, receiveShadows, probeAnchor, lightProbeUsage, lightProbeProxyVolume);
        }

        /// <summary>
        /// Draw the same mesh multiple times using GPU instancing.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="matrices">
        /// The array of object transformation matrices.
        /// </param>
        /// <param name="count">
        /// The number of instances to be drawn.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply. See [[MaterialPropertyBlock]].
        /// </param>
        /// <param name="castShadows">
        /// Should the meshes cast shadows?
        /// </param>
        /// <param name="receiveShadows">
        /// Should the meshes receive shadows?
        /// </param>
        /// <param name="layer">
        /// [[wiki:Layers|Layer]] to use.
        /// </param>
        /// <param name="camera">
        /// If /null/ (default), the mesh will be drawn in all cameras. Otherwise it will be drawn in the given camera only.
        /// </param>
        /// <param name="lightProbeUsage">
        /// [[LightProbeUsage]] for the instances.
        /// </param>
        /// <description>
        /// Similar to [[Graphics.DrawMesh]], this function draws meshes for one frame without the overhead of creating unnecessary game objects.
        /// Use this function in situations where you want to draw the same mesh for a particular amount of times using an instanced shader. Meshes are not further culled by the view frustum or baked occluders, nor sorted for transparency or z efficiency.
        /// The transformation matrix of each instance of the mesh should be packed into the /matrices/ array. You can specify the number of instances to draw, or by default it is the length of the /matrices/ array. Other per-instance data, if required by the shader, should be provided by creating arrays on the MaterialPropertyBlock argument using MaterialPropertyBlock.SetFloatArray, MaterialPropertyBlock.SetVectorArray and MaterialPropertyBlock.SetMatrixArray.
        /// To render the instances with light probes, provide the light probe data via the MaterialPropertyBlock and specify /lightProbeUsage/ with [[LightProbeUsage.CustomProvided]]. Check [[LightProbes.CalculateInterpolatedLightAndOcclusionProbes]] for the details.
        /// Note: You can only draw a maximum of 1023 instances at once.
        /// InvalidOperationException will be thrown if the material doesn't have [[Material.enableInstancing]] set to true, or the current platform doesn't support this API (i.e. if GPU instancing is not available). See [[SystemInfo.supportsInstancing]].
        /// SA: DrawMesh.
        /// </description>
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, [uei.DefaultValue("matrices.Length")] int count, [uei.DefaultValue("null")] MaterialPropertyBlock properties, [uei.DefaultValue("ShadowCastingMode.On")] ShadowCastingMode castShadows, [uei.DefaultValue("true")] bool receiveShadows, [uei.DefaultValue("0")] int layer, [uei.DefaultValue("null")] Camera camera, [uei.DefaultValue("LightProbeUsage.BlendProbes")] LightProbeUsage lightProbeUsage, [uei.DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
        {
            if (!SystemInfo.supportsInstancing)
                throw new InvalidOperationException("Instancing is not supported.");
            else if (mesh == null)
                throw new ArgumentNullException("mesh");
            else if (submeshIndex < 0 || submeshIndex >= mesh.subMeshCount)
                throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
            else if (material == null)
                throw new ArgumentNullException("material");
            else if (!material.enableInstancing)
                throw new InvalidOperationException("Material needs to enable instancing for use with DrawMeshInstanced.");
            else if (matrices == null)
                throw new ArgumentNullException("matrices");
            else if (count < 0 || count > Mathf.Min(kMaxDrawMeshInstanceCount, matrices.Length))
                throw new ArgumentOutOfRangeException("count", String.Format("Count must be in the range of 0 to {0}.", Mathf.Min(kMaxDrawMeshInstanceCount, matrices.Length)));
            else if (lightProbeUsage == LightProbeUsage.UseProxyVolume && lightProbeProxyVolume == null)
                throw new ArgumentException("Argument lightProbeProxyVolume must not be null if lightProbeUsage is set to UseProxyVolume.", "lightProbeProxyVolume");

            if (count > 0)
                Internal_DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, [uei.DefaultValue("null")] MaterialPropertyBlock properties, [uei.DefaultValue("ShadowCastingMode.On")] ShadowCastingMode castShadows, [uei.DefaultValue("true")] bool receiveShadows, [uei.DefaultValue("0")] int layer, [uei.DefaultValue("null")] Camera camera, [uei.DefaultValue("LightProbeUsage.BlendProbes")] LightProbeUsage lightProbeUsage, [uei.DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
        {
            if (matrices == null)
                throw new ArgumentNullException("matrices");

            DrawMeshInstanced(mesh, submeshIndex, material, NoAllocHelpers.ExtractArrayFromListT(matrices), matrices.Count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
        }

        /// <summary>
        /// Draw the same mesh multiple times using GPU instancing.
        /// </summary>
        /// <param name="mesh">
        /// The [[Mesh]] to draw.
        /// </param>
        /// <param name="submeshIndex">
        /// Which subset of the mesh to draw. This applies only to meshes that are composed of several materials.
        /// </param>
        /// <param name="material">
        /// [[Material]] to use.
        /// </param>
        /// <param name="bounds">
        /// The bounding volume surrounding the instances you intend to draw.
        /// </param>
        /// <param name="bufferWithArgs">
        /// The GPU buffer containing the arguments for how many instances of this mesh to draw.
        /// </param>
        /// <param name="argsOffset">
        /// The byte offset into the buffer, where the draw arguments start.
        /// </param>
        /// <param name="properties">
        /// Additional material properties to apply. See [[MaterialPropertyBlock]].
        /// </param>
        /// <param name="castShadows">
        /// Determines whether the mesh can cast shadows.
        /// </param>
        /// <param name="receiveShadows">
        /// Determines whether the mesh can receive shadows.
        /// </param>
        /// <param name="layer">
        /// [[wiki:Layers|Layer]] to use.
        /// </param>
        /// <param name="camera">
        /// If /null/ (default), the mesh will be drawn in all cameras. Otherwise it will be drawn in the given camera only.
        /// </param>
        /// <param name="lightProbeUsage">
        /// [[LightProbeUsage]] for the instances.
        /// </param>
        /// <description>
        /// Similar to [[Graphics.DrawMeshInstanced]], this function draws many instances of the same mesh, but unlike that method, the arguments for how many instances to draw come from /bufferWithArgs/.
        /// Use this function in situations where you want to draw the same mesh for a particular amount of times using an instanced shader. Meshes are not further culled by the view frustum or baked occluders, nor sorted for transparency or z efficiency.
        /// Buffer with arguments, /bufferWithArgs/, has to have five integer numbers at given /argsOffset/ offset:
        /// index count per instance, instance count, start index location, base vertex location, start instance location.
        /// Here is a script that can be used to draw many instances of the same mesh:
        /// </description>
        /// <description>
        /// Here is a surface shader that can be used with the example script above:
        /// </description>
        /// <dw-legacy-code>
        /// Shader "Instanced/InstancedSurfaceShader" {
        ///     Properties {
        ///         _MainTex ("Albedo (RGB)", 2D) = "white" {}
        ///         _Glossiness ("Smoothness", Range(0,1)) = 0.5
        ///         _Metallic ("Metallic", Range(0,1)) = 0.0
        ///     }
        ///     SubShader {
        ///         Tags { "RenderType"="Opaque" }
        ///         LOD 200
        ///         CGPROGRAM
        ///         // Physically based Standard lighting model
        ///         #pragma surface surf Standard addshadow fullforwardshadows
        ///         #pragma multi_compile_instancing
        ///         #pragma instancing_options procedural:setup
        ///         sampler2D _MainTex;
        ///         struct Input {
        ///             float2 uv_MainTex;
        ///         };
        ///     #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        ///         StructuredBuffer<float4> positionBuffer;
        ///     #endif
        ///         void rotate2D(inout float2 v, float r)
        ///         {
        ///             float s, c;
        ///             sincos(r, s, c);
        ///             v = float2(v.x * c - v.y * s, v.x * s + v.y * c);
        ///         }
        ///         void setup()
        ///         {
        ///         #ifdef UNITY_PROCEDURAL_INSTANCING_ENABLED
        ///             float4 data = positionBuffer[unity_InstanceID];
        ///             float rotation = data.w * data.w * _Time.y * 0.5f;
        ///             rotate2D(data.xz, rotation);
        ///             unity_ObjectToWorld._11_21_31_41 = float4(data.w, 0, 0, 0);
        ///             unity_ObjectToWorld._12_22_32_42 = float4(0, data.w, 0, 0);
        ///             unity_ObjectToWorld._13_23_33_43 = float4(0, 0, data.w, 0);
        ///             unity_ObjectToWorld._14_24_34_44 = float4(data.xyz, 1);
        ///             unity_WorldToObject = unity_ObjectToWorld;
        ///             unity_WorldToObject._14_24_34 *= -1;
        ///             unity_WorldToObject._11_22_33 = 1.0f / unity_WorldToObject._11_22_33;
        ///         #endif
        ///         }
        ///         half _Glossiness;
        ///         half _Metallic;
        ///         void surf (Input IN, inout SurfaceOutputStandard o) {
        ///             fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
        ///             o.Albedo = c.rgb;
        ///             o.Metallic = _Metallic;
        ///             o.Smoothness = _Glossiness;
        ///             o.Alpha = c.a;
        ///         }
        ///         ENDCG
        ///     }
        ///     FallBack "Diffuse"
        /// }
        /// </dw-legacy-code>
        /// <description>
        /// Here is a custom shader that can be used with the example script above:
        /// </description>
        /// <dw-legacy-code>
        /// Shader "Instanced/InstancedShader" {
        ///     Properties {
        ///         _MainTex ("Albedo (RGB)", 2D) = "white" {}
        ///     }
        ///     SubShader {
        ///         Pass {
        ///             Tags {"LightMode"="ForwardBase"}
        ///             CGPROGRAM
        ///             #pragma vertex vert
        ///             #pragma fragment frag
        ///             #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
        ///             #pragma target 4.5
        ///             #include "UnityCG.cginc"
        ///             #include "UnityLightingCommon.cginc"
        ///             #include "AutoLight.cginc"
        ///             sampler2D _MainTex;
        ///         #if SHADER_TARGET >= 45
        ///             StructuredBuffer<float4> positionBuffer;
        ///         #endif
        ///             struct v2f
        ///             {
        ///                 float4 pos : SV_POSITION;
        ///                 float2 uv_MainTex : TEXCOORD0;
        ///                 float3 ambient : TEXCOORD1;
        ///                 float3 diffuse : TEXCOORD2;
        ///                 float3 color : TEXCOORD3;
        ///                 SHADOW_COORDS(4)
        ///             };
        ///             void rotate2D(inout float2 v, float r)
        ///             {
        ///                 float s, c;
        ///                 sincos(r, s, c);
        ///                 v = float2(v.x * c - v.y * s, v.x * s + v.y * c);
        ///             }
        ///             v2f vert (appdata_full v, uint instanceID : SV_InstanceID)
        ///             {
        ///             #if SHADER_TARGET >= 45
        ///                 float4 data = positionBuffer[instanceID];
        ///             #else
        ///                 float4 data = 0;
        ///             #endif
        ///                 float rotation = data.w * data.w * _Time.x * 0.5f;
        ///                 rotate2D(data.xz, rotation);
        ///                 float3 localPosition = v.vertex.xyz * data.w;
        ///                 float3 worldPosition = data.xyz + localPosition;
        ///                 float3 worldNormal = v.normal;
        ///                 float3 ndotl = saturate(dot(worldNormal, _WorldSpaceLightPos0.xyz));
        ///                 float3 ambient = ShadeSH9(float4(worldNormal, 1.0f));
        ///                 float3 diffuse = (ndotl * _LightColor0.rgb);
        ///                 float3 color = v.color;
        ///                 v2f o;
        ///                 o.pos = mul(UNITY_MATRIX_VP, float4(worldPosition, 1.0f));
        ///                 o.uv_MainTex = v.texcoord;
        ///                 o.ambient = ambient;
        ///                 o.diffuse = diffuse;
        ///                 o.color = color;
        ///                 TRANSFER_SHADOW(o)
        ///                 return o;
        ///             }
        ///             fixed4 frag (v2f i) : SV_Target
        ///             {
        ///                 fixed shadow = SHADOW_ATTENUATION(i);
        ///                 fixed4 albedo = tex2D(_MainTex, i.uv_MainTex);
        ///                 float3 lighting = i.diffuse * shadow + i.ambient;
        ///                 fixed4 output = fixed4(albedo.rgb * i.color * lighting, albedo.w);
        ///                 UNITY_APPLY_FOG(i.fogCoord, output);
        ///                 return output;
        ///             }
        ///             ENDCG
        ///         }
        ///     }
        /// }
        /// </dw-legacy-code>
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, [uei.DefaultValue("0")] int argsOffset, [uei.DefaultValue("null")] MaterialPropertyBlock properties, [uei.DefaultValue("ShadowCastingMode.On")] ShadowCastingMode castShadows, [uei.DefaultValue("true")] bool receiveShadows, [uei.DefaultValue("0")] int layer, [uei.DefaultValue("null")] Camera camera, [uei.DefaultValue("LightProbeUsage.BlendProbes")] LightProbeUsage lightProbeUsage, [uei.DefaultValue("null")] LightProbeProxyVolume lightProbeProxyVolume)
        {
            if (!SystemInfo.supportsInstancing)
                throw new InvalidOperationException("Instancing is not supported.");
            else if (mesh == null)
                throw new ArgumentNullException("mesh");
            else if (submeshIndex < 0 || submeshIndex >= mesh.subMeshCount)
                throw new ArgumentOutOfRangeException("submeshIndex", "submeshIndex out of range.");
            else if (material == null)
                throw new ArgumentNullException("material");
            else if (bufferWithArgs == null)
                throw new ArgumentNullException("bufferWithArgs");
            if (lightProbeUsage == LightProbeUsage.UseProxyVolume && lightProbeProxyVolume == null)
                throw new ArgumentException("Argument lightProbeProxyVolume must not be null if lightProbeUsage is set to UseProxyVolume.", "lightProbeProxyVolume");

            Internal_DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, lightProbeProxyVolume);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawProceduralNow(MeshTopology topology, int vertexCount, int instanceCount = 1)
        {
            Internal_DrawProceduralNow(topology, vertexCount, instanceCount);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawProceduralNow(MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount = 1)
        {
            if (indexBuffer == null)
                throw new ArgumentNullException("indexBuffer");
            Internal_DrawProceduralIndexedNow(topology, indexBuffer, indexCount, instanceCount);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawProceduralIndirectNow(MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset = 0)
        {
            if (bufferWithArgs == null)
                throw new ArgumentNullException("bufferWithArgs");

            Internal_DrawProceduralIndirectNow(topology, bufferWithArgs, argsOffset);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawProceduralIndirectNow(MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset = 0)
        {
            if (indexBuffer == null)
                throw new ArgumentNullException("indexBuffer");
            if (bufferWithArgs == null)
                throw new ArgumentNullException("bufferWithArgs");

            Internal_DrawProceduralIndexedIndirectNow(topology, indexBuffer, bufferWithArgs, argsOffset);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawProcedural(Material material, Bounds bounds, MeshTopology topology, int vertexCount, int instanceCount = 1, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
        {
            Internal_DrawProcedural(material, bounds, topology, vertexCount, instanceCount, camera, properties, castShadows, receiveShadows, layer);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawProcedural(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount = 1, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
        {
            if (indexBuffer == null)
                throw new ArgumentNullException("indexBuffer");
            Internal_DrawProceduralIndexed(material, bounds, topology, indexBuffer, indexCount, instanceCount, camera, properties, castShadows, receiveShadows, layer);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawProceduralIndirect(Material material, Bounds bounds, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset = 0, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
        {
            if (bufferWithArgs == null)
                throw new ArgumentNullException("bufferWithArgs");

            Internal_DrawProceduralIndirect(material, bounds, topology, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void DrawProceduralIndirect(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset = 0, Camera camera = null, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0)
        {
            if (indexBuffer == null)
                throw new ArgumentNullException("indexBuffer");
            if (bufferWithArgs == null)
                throw new ArgumentNullException("bufferWithArgs");

            Internal_DrawProceduralIndexedIndirect(material, bounds, topology, indexBuffer, bufferWithArgs, argsOffset, camera, properties, castShadows, receiveShadows, layer);
        }
    }
}


//
// Graphics.Blit*
//


namespace UnityEngine
{
    public partial class Graphics
    {
        /// <summary>
        /// Copies source texture into destination render texture with a shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="dest">
        /// The destination [[RenderTexture]]. Set this to /null/ to blit directly to screen. See description for more information.
        /// </param>
        /// <description>
        /// This is mostly used for implementing [[wiki:PostProcessingOverview|post-processing effects]].
        /// Blit sets /dest/ as the render target, sets /source/ @@_MainTex@@ property on the
        /// material, and draws a full-screen quad.
        /// If /dest/ is /null/, the screen backbuffer is used as the blit destination, except if the main camera is currently set to render to a RenderTexture (that is [[Camera.main]] has a non-null /targetTexture/ property). In that case the blit uses the render target of the main camera as destination. In order to ensure that the blit is actually done to the screen backbuffer, make sure to set /Camera.main.targetTexture/ to /null/ before calling Blit.
        /// Note that if you want to use depth or stencil buffer that is part of the /source/ (Render)texture,
        /// you'll have to do equivalent of Blit functionality manually - i.e. [[Graphics.SetRenderTarget]]
        /// with destination color buffer and source depth buffer, setup orthographic projection ([[GL.LoadOrtho]]),
        /// setup material pass ([[Material.SetPass]]) and draw a quad ([[GL.Begin]]).
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing Blit or any other
        /// manual rendering.
        /// Note that a call to Blit with /source/ and /dest/ set to the same RenderTexture may result in undefined behaviour. A better approach is to either use [[wiki:CustomRenderTextures|Custom Render Textures]] with double buffering, or use two RenderTextures and alternate between them to implement double buffering manually.
        /// SA: [[Graphics.BlitMultiTap]], [[wiki:PostProcessingOverview|Post-processing effects]].
        /// </description>
        public static void Blit(Texture source, RenderTexture dest)
        {
            Blit2(source, dest);
        }

        /// <summary>
        /// Copies source texture into destination render texture with a shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="dest">
        /// The destination [[RenderTexture]]. Set this to /null/ to blit directly to screen. See description for more information.
        /// </param>
        /// <param name="sourceDepthSlice">
        /// The texture array source slice to perform the blit from.
        /// </param>
        /// <param name="destDepthSlice">
        /// The texture array destination slice to perform the blit to.
        /// </param>
        /// <description>
        /// This is mostly used for implementing [[wiki:PostProcessingOverview|post-processing effects]].
        /// Blit sets /dest/ as the render target, sets /source/ @@_MainTex@@ property on the
        /// material, and draws a full-screen quad.
        /// If /dest/ is /null/, the screen backbuffer is used as the blit destination, except if the main camera is currently set to render to a RenderTexture (that is [[Camera.main]] has a non-null /targetTexture/ property). In that case the blit uses the render target of the main camera as destination. In order to ensure that the blit is actually done to the screen backbuffer, make sure to set /Camera.main.targetTexture/ to /null/ before calling Blit.
        /// Note that if you want to use depth or stencil buffer that is part of the /source/ (Render)texture,
        /// you'll have to do equivalent of Blit functionality manually - i.e. [[Graphics.SetRenderTarget]]
        /// with destination color buffer and source depth buffer, setup orthographic projection ([[GL.LoadOrtho]]),
        /// setup material pass ([[Material.SetPass]]) and draw a quad ([[GL.Begin]]).
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing Blit or any other
        /// manual rendering.
        /// Note that a call to Blit with /source/ and /dest/ set to the same RenderTexture may result in undefined behaviour. A better approach is to either use [[wiki:CustomRenderTextures|Custom Render Textures]] with double buffering, or use two RenderTextures and alternate between them to implement double buffering manually.
        /// SA: [[Graphics.BlitMultiTap]], [[wiki:PostProcessingOverview|Post-processing effects]].
        /// </description>
        public static void Blit(Texture source, RenderTexture dest, int sourceDepthSlice, int destDepthSlice)
        {
            Blit3(source, dest, sourceDepthSlice, destDepthSlice);
        }

        /// <summary>
        /// Copies source texture into destination render texture with a shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="dest">
        /// The destination [[RenderTexture]]. Set this to /null/ to blit directly to screen. See description for more information.
        /// </param>
        /// <param name="scale">
        /// Scale applied to the source texture coordinate.
        /// </param>
        /// <param name="offset">
        /// Offset applied to the source texture coordinate.
        /// </param>
        /// <description>
        /// This is mostly used for implementing [[wiki:PostProcessingOverview|post-processing effects]].
        /// Blit sets /dest/ as the render target, sets /source/ @@_MainTex@@ property on the
        /// material, and draws a full-screen quad.
        /// If /dest/ is /null/, the screen backbuffer is used as the blit destination, except if the main camera is currently set to render to a RenderTexture (that is [[Camera.main]] has a non-null /targetTexture/ property). In that case the blit uses the render target of the main camera as destination. In order to ensure that the blit is actually done to the screen backbuffer, make sure to set /Camera.main.targetTexture/ to /null/ before calling Blit.
        /// Note that if you want to use depth or stencil buffer that is part of the /source/ (Render)texture,
        /// you'll have to do equivalent of Blit functionality manually - i.e. [[Graphics.SetRenderTarget]]
        /// with destination color buffer and source depth buffer, setup orthographic projection ([[GL.LoadOrtho]]),
        /// setup material pass ([[Material.SetPass]]) and draw a quad ([[GL.Begin]]).
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing Blit or any other
        /// manual rendering.
        /// Note that a call to Blit with /source/ and /dest/ set to the same RenderTexture may result in undefined behaviour. A better approach is to either use [[wiki:CustomRenderTextures|Custom Render Textures]] with double buffering, or use two RenderTextures and alternate between them to implement double buffering manually.
        /// SA: [[Graphics.BlitMultiTap]], [[wiki:PostProcessingOverview|Post-processing effects]].
        /// </description>
        public static void Blit(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset)
        {
            Blit4(source, dest, scale, offset);
        }

        /// <summary>
        /// Copies source texture into destination render texture with a shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="dest">
        /// The destination [[RenderTexture]]. Set this to /null/ to blit directly to screen. See description for more information.
        /// </param>
        /// <param name="scale">
        /// Scale applied to the source texture coordinate.
        /// </param>
        /// <param name="offset">
        /// Offset applied to the source texture coordinate.
        /// </param>
        /// <param name="sourceDepthSlice">
        /// The texture array source slice to perform the blit from.
        /// </param>
        /// <param name="destDepthSlice">
        /// The texture array destination slice to perform the blit to.
        /// </param>
        /// <description>
        /// This is mostly used for implementing [[wiki:PostProcessingOverview|post-processing effects]].
        /// Blit sets /dest/ as the render target, sets /source/ @@_MainTex@@ property on the
        /// material, and draws a full-screen quad.
        /// If /dest/ is /null/, the screen backbuffer is used as the blit destination, except if the main camera is currently set to render to a RenderTexture (that is [[Camera.main]] has a non-null /targetTexture/ property). In that case the blit uses the render target of the main camera as destination. In order to ensure that the blit is actually done to the screen backbuffer, make sure to set /Camera.main.targetTexture/ to /null/ before calling Blit.
        /// Note that if you want to use depth or stencil buffer that is part of the /source/ (Render)texture,
        /// you'll have to do equivalent of Blit functionality manually - i.e. [[Graphics.SetRenderTarget]]
        /// with destination color buffer and source depth buffer, setup orthographic projection ([[GL.LoadOrtho]]),
        /// setup material pass ([[Material.SetPass]]) and draw a quad ([[GL.Begin]]).
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing Blit or any other
        /// manual rendering.
        /// Note that a call to Blit with /source/ and /dest/ set to the same RenderTexture may result in undefined behaviour. A better approach is to either use [[wiki:CustomRenderTextures|Custom Render Textures]] with double buffering, or use two RenderTextures and alternate between them to implement double buffering manually.
        /// SA: [[Graphics.BlitMultiTap]], [[wiki:PostProcessingOverview|Post-processing effects]].
        /// </description>
        public static void Blit(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice)
        {
            Blit5(source, dest, scale, offset, sourceDepthSlice, destDepthSlice);
        }

        /// <summary>
        /// Copies source texture into destination render texture with a shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="dest">
        /// The destination [[RenderTexture]]. Set this to /null/ to blit directly to screen. See description for more information.
        /// </param>
        /// <param name="mat">
        /// Material to use. Material's shader could do some post-processing effect, for example.
        /// </param>
        /// <param name="pass">
        /// If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
        /// </param>
        /// <description>
        /// This is mostly used for implementing [[wiki:PostProcessingOverview|post-processing effects]].
        /// Blit sets /dest/ as the render target, sets /source/ @@_MainTex@@ property on the
        /// material, and draws a full-screen quad.
        /// If /dest/ is /null/, the screen backbuffer is used as the blit destination, except if the main camera is currently set to render to a RenderTexture (that is [[Camera.main]] has a non-null /targetTexture/ property). In that case the blit uses the render target of the main camera as destination. In order to ensure that the blit is actually done to the screen backbuffer, make sure to set /Camera.main.targetTexture/ to /null/ before calling Blit.
        /// Note that if you want to use depth or stencil buffer that is part of the /source/ (Render)texture,
        /// you'll have to do equivalent of Blit functionality manually - i.e. [[Graphics.SetRenderTarget]]
        /// with destination color buffer and source depth buffer, setup orthographic projection ([[GL.LoadOrtho]]),
        /// setup material pass ([[Material.SetPass]]) and draw a quad ([[GL.Begin]]).
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing Blit or any other
        /// manual rendering.
        /// Note that a call to Blit with /source/ and /dest/ set to the same RenderTexture may result in undefined behaviour. A better approach is to either use [[wiki:CustomRenderTextures|Custom Render Textures]] with double buffering, or use two RenderTextures and alternate between them to implement double buffering manually.
        /// SA: [[Graphics.BlitMultiTap]], [[wiki:PostProcessingOverview|Post-processing effects]].
        /// </description>
        public static void Blit(Texture source, RenderTexture dest, Material mat, [uei.DefaultValue("-1")] int pass)
        {
            Internal_BlitMaterial5(source, dest, mat, pass, true);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void Blit(Texture source, RenderTexture dest, Material mat, int pass, int destDepthSlice)
        {
            Internal_BlitMaterial6(source, dest, mat, pass, true, destDepthSlice);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void Blit(Texture source, RenderTexture dest, Material mat)
        {
            Blit(source, dest, mat, -1);
        }

        /// <summary>
        /// Copies source texture into destination render texture with a shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="mat">
        /// Material to use. Material's shader could do some post-processing effect, for example.
        /// </param>
        /// <param name="pass">
        /// If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
        /// </param>
        /// <description>
        /// This is mostly used for implementing [[wiki:PostProcessingOverview|post-processing effects]].
        /// Blit sets /dest/ as the render target, sets /source/ @@_MainTex@@ property on the
        /// material, and draws a full-screen quad.
        /// If /dest/ is /null/, the screen backbuffer is used as the blit destination, except if the main camera is currently set to render to a RenderTexture (that is [[Camera.main]] has a non-null /targetTexture/ property). In that case the blit uses the render target of the main camera as destination. In order to ensure that the blit is actually done to the screen backbuffer, make sure to set /Camera.main.targetTexture/ to /null/ before calling Blit.
        /// Note that if you want to use depth or stencil buffer that is part of the /source/ (Render)texture,
        /// you'll have to do equivalent of Blit functionality manually - i.e. [[Graphics.SetRenderTarget]]
        /// with destination color buffer and source depth buffer, setup orthographic projection ([[GL.LoadOrtho]]),
        /// setup material pass ([[Material.SetPass]]) and draw a quad ([[GL.Begin]]).
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing Blit or any other
        /// manual rendering.
        /// Note that a call to Blit with /source/ and /dest/ set to the same RenderTexture may result in undefined behaviour. A better approach is to either use [[wiki:CustomRenderTextures|Custom Render Textures]] with double buffering, or use two RenderTextures and alternate between them to implement double buffering manually.
        /// SA: [[Graphics.BlitMultiTap]], [[wiki:PostProcessingOverview|Post-processing effects]].
        /// </description>
        public static void Blit(Texture source, Material mat, [uei.DefaultValue("-1")] int pass)
        {
            Internal_BlitMaterial5(source, null, mat, pass, false);
        }

        /// <summary>
        /// Copies source texture into destination render texture with a shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="mat">
        /// Material to use. Material's shader could do some post-processing effect, for example.
        /// </param>
        /// <param name="pass">
        /// If -1 (default), draws all passes in the material. Otherwise, draws given pass only.
        /// </param>
        /// <param name="destDepthSlice">
        /// The texture array destination slice to perform the blit to.
        /// </param>
        /// <description>
        /// This is mostly used for implementing [[wiki:PostProcessingOverview|post-processing effects]].
        /// Blit sets /dest/ as the render target, sets /source/ @@_MainTex@@ property on the
        /// material, and draws a full-screen quad.
        /// If /dest/ is /null/, the screen backbuffer is used as the blit destination, except if the main camera is currently set to render to a RenderTexture (that is [[Camera.main]] has a non-null /targetTexture/ property). In that case the blit uses the render target of the main camera as destination. In order to ensure that the blit is actually done to the screen backbuffer, make sure to set /Camera.main.targetTexture/ to /null/ before calling Blit.
        /// Note that if you want to use depth or stencil buffer that is part of the /source/ (Render)texture,
        /// you'll have to do equivalent of Blit functionality manually - i.e. [[Graphics.SetRenderTarget]]
        /// with destination color buffer and source depth buffer, setup orthographic projection ([[GL.LoadOrtho]]),
        /// setup material pass ([[Material.SetPass]]) and draw a quad ([[GL.Begin]]).
        /// Note that in Linear color space, it is important to have the correct sRGB<->Linear color conversion
        /// state set. Depending on what was rendered previously, the current state might not be the one you expect.
        /// You should consider setting [[GL.sRGBWrite]] as you need it before doing Blit or any other
        /// manual rendering.
        /// Note that a call to Blit with /source/ and /dest/ set to the same RenderTexture may result in undefined behaviour. A better approach is to either use [[wiki:CustomRenderTextures|Custom Render Textures]] with double buffering, or use two RenderTextures and alternate between them to implement double buffering manually.
        /// SA: [[Graphics.BlitMultiTap]], [[wiki:PostProcessingOverview|Post-processing effects]].
        /// </description>
        public static void Blit(Texture source, Material mat, int pass, int destDepthSlice)
        {
            Internal_BlitMaterial6(source, null, mat, pass, false, destDepthSlice);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void Blit(Texture source, Material mat)
        {
            Blit(source, mat, -1);
        }

        /// <summary>
        /// Copies source texture into destination, for multi-tap shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="dest">
        /// Destination [[RenderTexture]], or /null/ to blit directly to screen.
        /// </param>
        /// <param name="mat">
        /// Material to use for copying. Material's shader should do some post-processing effect.
        /// </param>
        /// <param name="offsets">
        /// Variable number of filtering offsets. Offsets are given in pixels.
        /// </param>
        /// <description>
        /// This is mostly used for implementing some [[wiki:PostProcessingOverview|post-processing effects]]. For example,
        /// Gaussian or iterative Cone blurring samples source texture at multiple different locations.
        /// BlitMultiTap sets /dest/ to be RenderTexture.active render texture, sets /source/ as
        /// @@_MainTex@@ property on the material, and draws a full-screen quad. Each vertex of the quad
        /// has multiple texture coordinates set up, offset by /offsets/ pixels.
        /// BlitMultiTap has the same limitations as [[Graphics.Blit]].
        /// SA: [[Graphics.Blit]], [[wiki:PostProcessingOverview|post-processing effects]].
        /// </description>
        public static void BlitMultiTap(Texture source, RenderTexture dest, Material mat, params Vector2[] offsets)
        {
            // in case params were not passed, we will end up with empty array (not null) but our cpp code is not ready for that.
            // do explicit argument exception instead of potential nullref coming from native side
            if (offsets.Length == 0)
                throw new ArgumentException("empty offsets list passed.", "offsets");
            Internal_BlitMultiTap4(source, dest, mat, offsets);
        }

        /// <summary>
        /// Copies source texture into destination, for multi-tap shader.
        /// </summary>
        /// <param name="source">
        /// Source texture.
        /// </param>
        /// <param name="dest">
        /// Destination [[RenderTexture]], or /null/ to blit directly to screen.
        /// </param>
        /// <param name="mat">
        /// Material to use for copying. Material's shader should do some post-processing effect.
        /// </param>
        /// <param name="destDepthSlice">
        /// The texture array destination slice to blit to.
        /// </param>
        /// <param name="offsets">
        /// Variable number of filtering offsets. Offsets are given in pixels.
        /// </param>
        /// <description>
        /// This is mostly used for implementing some [[wiki:PostProcessingOverview|post-processing effects]]. For example,
        /// Gaussian or iterative Cone blurring samples source texture at multiple different locations.
        /// BlitMultiTap sets /dest/ to be RenderTexture.active render texture, sets /source/ as
        /// @@_MainTex@@ property on the material, and draws a full-screen quad. Each vertex of the quad
        /// has multiple texture coordinates set up, offset by /offsets/ pixels.
        /// BlitMultiTap has the same limitations as [[Graphics.Blit]].
        /// SA: [[Graphics.Blit]], [[wiki:PostProcessingOverview|post-processing effects]].
        /// </description>
        public static void BlitMultiTap(Texture source, RenderTexture dest, Material mat, int destDepthSlice, params Vector2[] offsets)
        {
            // in case params were not passed, we will end up with empty array (not null) but our cpp code is not ready for that.
            // do explicit argument exception instead of potential nullref coming from native side
            if (offsets.Length == 0)
                throw new ArgumentException("empty offsets list passed.", "offsets");
            Internal_BlitMultiTap5(source, dest, mat, offsets, destDepthSlice);
        }
    }
}


//
// QualitySettings
//


namespace UnityEngine
{
    /// <summary>
    /// Script interface for [[wiki:class-QualitySettings|Quality Settings]].
    /// </summary>
    /// <description>
    /// There can be an arbitrary number of quality settings. The details of each are set up
    /// in the project's [[wiki:class-QualitySettings|Quality Settings]]. At run time, the
    /// current quality level can be changed using this class.
    /// </description>
    public sealed partial class QualitySettings
    {
        /// <summary>
        /// Increase the current quality level.
        /// </summary>
        /// <param name="applyExpensiveChanges">
        /// Should expensive changes be applied (Anti-aliasing etc).
        /// </param>
        /// <description>
        /// IncreaseLevel and DecreaseLevel functions only apply anti-aliasing if applyExpensiveChanges is true.
        /// SA: DecreaseLevel, [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        public static void IncreaseLevel([uei.DefaultValue("false")] bool applyExpensiveChanges)
        {
            SetQualityLevel(GetQualityLevel() + 1, applyExpensiveChanges);
        }

        /// <summary>
        /// Decrease the current quality level.
        /// </summary>
        /// <param name="applyExpensiveChanges">
        /// Should expensive changes be applied (Anti-aliasing etc).
        /// </param>
        /// <description>
        /// IncreaseLevel and DecreaseLevel functions only apply anti-aliasing if applyExpensiveChanges is true.
        /// SA: IncreaseLevel, [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        public static void DecreaseLevel([uei.DefaultValue("false")] bool applyExpensiveChanges)
        {
            SetQualityLevel(GetQualityLevel() - 1, applyExpensiveChanges);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void SetQualityLevel(int index) { SetQualityLevel(index, true); }
        /// <summary>
        /// Increase the current quality level.
        /// </summary>
        /// <description>
        /// IncreaseLevel and DecreaseLevel functions only apply anti-aliasing if applyExpensiveChanges is true.
        /// SA: DecreaseLevel, [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        public static void IncreaseLevel() { IncreaseLevel(false); }
        /// <summary>
        /// Decrease the current quality level.
        /// </summary>
        /// <description>
        /// IncreaseLevel and DecreaseLevel functions only apply anti-aliasing if applyExpensiveChanges is true.
        /// SA: IncreaseLevel, [[wiki:class-QualitySettings|Quality Settings]].
        /// </description>
        public static void DecreaseLevel() { DecreaseLevel(false); }
    }
}

//
// Extensions
//

namespace UnityEngine
{
    /// <summary>
    /// Extension methods to the Renderer class, used only for the UpdateGIMaterials method used by the Global Illumination System.
    /// </summary>
    public static partial class RendererExtensions
    {
        /// <summary>
        /// Schedules an update of the albedo and emissive Textures of a system that contains the Renderer.
        /// </summary>
        static public void UpdateGIMaterials(this Renderer renderer) { UpdateGIMaterialsForRenderer(renderer); }
    }
}

//
// Attributes
//

namespace UnityEngine
{
    /// <summary>
    /// When using HDR rendering it can sometime be desirable to switch to LDR rendering during ImageEffect rendering.
    /// </summary>
    /// <description>
    /// Using this Attribute on an image effect will cause the destination buffer to be an LDR buffer, and switch the
    /// rest of the Image Effect pipeline into LDR mode. It is the responsibility of the Image Effect that this Attribute
    /// is associated to ensure that the output is in the LDR range.
    /// </description>
    [UsedByNativeCode]
    public sealed partial class ImageEffectTransformsToLDR : Attribute
    {
    }

    /// <summary>
    /// Any Image Effect with this attribute can be rendered into the Scene view camera.
    /// </summary>
    /// <description>
    /// If you wish your image effect to be applied to the Scene view camera add this attribute. The effect will be applied in the same position, and with the same values as those from the camera the effect is on.
    /// </description>
    public sealed partial class ImageEffectAllowedInSceneView : Attribute
    {
    }

    /// <summary>
    /// Any Image Effect with this attribute will be rendered after opaque geometry but before transparent geometry.
    /// </summary>
    /// <description>
    /// This allows for effects which extensively use the depth buffer (SSAO, etc) to affect only opaque pixels. This attribute can be used to reduce the amount of visual artifacts in a Scene with post processing.
    /// </description>
    [UsedByNativeCode]
    public sealed partial class ImageEffectOpaque : Attribute
    {
    }

    /// <summary>
    /// Any Image Effect with this attribute will be rendered after Dynamic Resolution stage.
    /// </summary>
    /// <description>
    /// If you wish your image effect to be applied after the Dynamic Resolution has scaled back up add this attribute. The effect will be rendered at full resolution, important for effects that are in some way dependant on the screen width and height being a certain size.
    /// </description>
    [UsedByNativeCode]
    public sealed partial class ImageEffectAfterScale : Attribute
    {
    }

    /// <summary>
    /// Use this attribute when image effects are implemented using Command Buffers.
    /// </summary>
    /// <description>
    /// When you use this attribute, Unity renders the Scene into a RenderTexture instead of the actual target. Please note that Camera.forceIntoRenderTexture may have the same effect, but only in some cases.
    /// </description>
    [UsedByNativeCode]
    [AttributeUsage(AttributeTargets.Method)]
    public sealed partial class ImageEffectUsesCommandBuffer : Attribute
    {
    }
}
