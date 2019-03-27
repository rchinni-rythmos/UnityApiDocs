using System;
using System.Collections.Generic;
using UnityEngine;
using uei = UnityEngine.Internal;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Scripting;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;


namespace UnityEngine
{
    /// <summary>
    /// This struct contains all the information required to create a RenderTexture. It can be copied, cached, and reused to easily create RenderTextures that all share the same properties.
    /// </summary>
    public struct RenderTextureDescriptor
    {
        /// <summary>
        /// The width of the render texture in pixels.
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// The height of the render texture in pixels.
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// The multisample antialiasing level for the RenderTexture.
        /// See [[RenderTexture.antiAliasing]].
        /// </summary>
        public int msaaSamples { get; set; }
        /// <summary>
        /// Volume extent of a 3D render texture.
        /// </summary>
        /// <description>
        /// For volumetric render textures (see dimension), this variable determines the volume extent.
        /// Ignored for non-3D textures. The valid range for 3D textures is 1 to 2000.
        /// </description>
        public int volumeDepth { get; set; }

        private GraphicsFormat _graphicsFormat;// { get; set; }
        /// <summary>
        /// The color format for the RenderTexture.
        /// </summary>
        /// <description>
        /// See Also: [[GraphicsFormat]].
        /// </description>
        public GraphicsFormat graphicsFormat
        {
            get
            {
                return _graphicsFormat;
            }

            set
            {
                _graphicsFormat = value;
                SetOrClearRenderTextureCreationFlag(GraphicsFormatUtility.IsSRGBFormat(value), RenderTextureCreationFlags.SRGB);
            }
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTextureFormat colorFormat
        {
            get { return GraphicsFormatUtility.GetRenderTextureFormat(graphicsFormat); }
            set { graphicsFormat = SystemInfo.GetCompatibleFormat(GraphicsFormatUtility.GetGraphicsFormat(value, sRGB), FormatUsage.Render); }
        }

        private int _depthBufferBits;
        private static int[] depthFormatBits = new int[] { 0, 16, 24 };
        /// <summary>
        /// The precision of the render texture's depth buffer in bits (0, 16, 24/32 are supported).
        /// See [[RenderTexture.depth]].
        /// </summary>
        public int depthBufferBits
        {
            get { return depthFormatBits[_depthBufferBits]; }
            set
            {
                if (value <= 0)
                    _depthBufferBits = 0;
                else if (value <= 16)
                    _depthBufferBits = 1;
                else
                    _depthBufferBits = 2;
            }
        }

        /// <summary>
        /// Dimensionality (type) of the render texture.
        /// See [[RenderTexture.dimension]].
        /// </summary>
        public Rendering.TextureDimension dimension { get; set; }
        /// <summary>
        /// Determines how the RenderTexture is sampled if it is used as a shadow map.
        /// See [[ShadowSamplingMode]] for more details.
        /// </summary>
        public Rendering.ShadowSamplingMode shadowSamplingMode { get; set; }
        /// <summary>
        /// If this RenderTexture is a VR eye texture used in stereoscopic rendering, this property decides what special rendering occurs, if any. Instead of setting this manually, use the value returned by [[XR.XRSettings.eyeTextureDesc|eyeTextureDesc]] or other VR functions returning a [[RenderTextureDescriptor]].
        /// </summary>
        public VRTextureUsage vrUsage { get; set; }
        private RenderTextureCreationFlags _flags;
        /// <summary>
        /// A set of [[RenderTextureCreationFlags]] that control how the texture is created.
        /// </summary>
        public RenderTextureCreationFlags flags { get { return _flags; } }
        /// <summary>
        /// The render texture memoryless mode property.
        /// </summary>
        /// <description>
        /// Use this property to set or return the render texture memoryless modes.
        /// SA. [[RenderTextureMemoryless]].
        /// </description>
        public RenderTextureMemoryless memoryless { get; set; }
        /// <summary>
        /// Create a RenderTextureDescriptor with default values, or a certain width, height, and format.
        /// </summary>
        /// <param name="width">
        /// Width of the RenderTexture in pixels.
        /// </param>
        /// <param name="height">
        /// Height of the RenderTexture in pixels.
        /// </param>
        public RenderTextureDescriptor(int width, int height) : this(width, height, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR), 0) {}
        /// <summary>
        /// Create a RenderTextureDescriptor with default values, or a certain width, height, and format.
        /// </summary>
        /// <param name="width">
        /// Width of the RenderTexture in pixels.
        /// </param>
        /// <param name="height">
        /// Height of the RenderTexture in pixels.
        /// </param>
        /// <param name="colorFormat">
        /// The color format for the RenderTexture.
        /// </param>
        public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat) : this(width, height, colorFormat, 0) {}
        /// <summary>
        /// Create a RenderTextureDescriptor with default values, or a certain width, height, and format.
        /// </summary>
        /// <param name="width">
        /// Width of the RenderTexture in pixels.
        /// </param>
        /// <param name="height">
        /// Height of the RenderTexture in pixels.
        /// </param>
        /// <param name="colorFormat">
        /// The color format for the RenderTexture.
        /// </param>
        /// <param name="depthBufferBits">
        /// The number of bits to use for the depth buffer.
        /// </param>
        public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat, int depthBufferBits) : this(width, height, SystemInfo.GetCompatibleFormat(GraphicsFormatUtility.GetGraphicsFormat(colorFormat, false), FormatUsage.Render), depthBufferBits) {}
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, int depthBufferBits) : this()
        {
            this.width = width;
            this.height = height;
            volumeDepth = 1;
            msaaSamples = 1;
            this.graphicsFormat = colorFormat;
            this.depthBufferBits = depthBufferBits;
            this.sRGB = GraphicsFormatUtility.IsSRGBFormat(colorFormat);
            dimension = Rendering.TextureDimension.Tex2D;
            shadowSamplingMode = Rendering.ShadowSamplingMode.None;
            vrUsage = VRTextureUsage.None;
            _flags = RenderTextureCreationFlags.AutoGenerateMips | RenderTextureCreationFlags.AllowVerticalFlip;
            memoryless = RenderTextureMemoryless.None;
        }

        private void SetOrClearRenderTextureCreationFlag(bool value, RenderTextureCreationFlags flag)
        {
            if (value)
            {
                _flags |= flag;
            }
            else
            {
                _flags &= ~flag;
            }
        }

        /// <summary>
        /// This flag causes the render texture uses sRGB read/write conversions.
        /// </summary>
        /// <description>
        /// See [[RenderTexture.sRGB]].
        /// </description>
        public bool sRGB
        {
            get { return (_flags & RenderTextureCreationFlags.SRGB) != 0; }
            set
            {
                if (value)
                    graphicsFormat = GraphicsFormatUtility.GetSRGBFormat(graphicsFormat);
                else
                    graphicsFormat = GraphicsFormatUtility.GetLinearFormat(graphicsFormat);
                SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.SRGB);
            }
        }

        /// <summary>
        /// Render texture has mipmaps when this flag is set.
        /// See [[RenderTexture.useMipMap]].
        /// </summary>
        public bool useMipMap
        {
            get { return (_flags & RenderTextureCreationFlags.MipMap) != 0; }
            set { SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.MipMap); }
        }

        /// <summary>
        /// Mipmap levels are generated automatically when this flag is set.
        /// </summary>
        /// <description>
        /// See [[RenderTexture.autoGenerateMips]].
        /// </description>
        public bool autoGenerateMips
        {
            get { return (_flags & RenderTextureCreationFlags.AutoGenerateMips) != 0; }
            set { SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.AutoGenerateMips); }
        }

        /// <summary>
        /// Enable random access write into this render texture on Shader Model 5.0 level shaders.
        /// See [[RenderTexture.enableRandomWrite]].
        /// </summary>
        public bool enableRandomWrite
        {
            get { return (_flags & RenderTextureCreationFlags.EnableRandomWrite) != 0; }
            set { SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.EnableRandomWrite); }
        }

        /// <summary>
        /// If true and msaaSamples is greater than 1, the render texture will not be resolved by default.  Use this if the render texture needs to be bound as a multisampled texture in a shader.
        /// </summary>
        public bool bindMS
        {
            get { return (_flags & RenderTextureCreationFlags.BindMS) != 0; }
            set { SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.BindMS); }
        }

        internal bool createdFromScript
        {
            get { return (_flags & RenderTextureCreationFlags.CreatedFromScript) != 0; }
            set { SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.CreatedFromScript); }
        }

        /// <summary>
        /// Set to true to enable dynamic resolution scaling on this render texture.
        /// SA [[RenderTexture.useDynamicScale]].
        /// </summary>
        public bool useDynamicScale
        {
            get { return (_flags & RenderTextureCreationFlags.DynamicallyScalable) != 0; }
            set { SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.DynamicallyScalable); }
        }
    }

    public partial class RenderTexture : Texture
    {
        [RequiredByNativeCode] // used to create builtin textures
        internal protected RenderTexture()
        {
        }

        /// <summary>
        /// Creates a new RenderTexture object.
        /// </summary>
        /// <param name="desc">
        /// Create the RenderTexture with the settings in the RenderTextureDescriptor.
        /// </param>
        /// <description>
        /// The render texture is created with /width/ by /height/ size, with a depth buffer
        /// of /depth/ bits (depth can be 0, 16 or 24), and in /format/ format and with sRGB read / write on or off.
        /// Note that constructing a RenderTexture object does not create the hardware representation immediately.
        /// The actual render texture is created upon first use or when Create is called manually. So after
        /// constructing the render texture, it is possible to set additional variables, like format,
        public RenderTexture(RenderTextureDescriptor desc)
        {
            ValidateRenderTextureDesc(desc);
            Internal_Create(this);
            SetRenderTextureDescriptor(desc);
        }

        /// <summary>
        /// Creates a new RenderTexture object.
        /// </summary>
        /// <param name="textureToCopy">
        /// Copy the settings from another RenderTexture.
        /// </param>
        /// <description>
        /// The render texture is created with /width/ by /height/ size, with a depth buffer
        /// of /depth/ bits (depth can be 0, 16 or 24), and in /format/ format and with sRGB read / write on or off.
        /// Note that constructing a RenderTexture object does not create the hardware representation immediately.
        /// The actual render texture is created upon first use or when Create is called manually. So after
        /// constructing the render texture, it is possible to set additional variables, like format,
        public RenderTexture(RenderTexture textureToCopy)
        {
            if (textureToCopy == null)
                throw new ArgumentNullException("textureToCopy");

            ValidateRenderTextureDesc(textureToCopy.descriptor);
            Internal_Create(this);
            SetRenderTextureDescriptor(textureToCopy.descriptor);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTexture(int width, int height, int depth, DefaultFormat format)
            : this(width, height, depth, SystemInfo.GetGraphicsFormat(format))
        {
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public RenderTexture(int width, int height, int depth, GraphicsFormat format)
        {
            if (!ValidateFormat(format, FormatUsage.Render))
                return;

            Internal_Create(this);
            this.width = width; this.height = height; this.depth = depth; this.graphicsFormat = format;

            SetSRGBReadWrite(GraphicsFormatUtility.IsSRGBFormat(format));
        }

        /// <summary>
        /// Creates a new RenderTexture object.
        /// </summary>
        /// <param name="width">
        /// Texture width in pixels.
        /// </param>
        /// <param name="height">
        /// Texture height in pixels.
        /// </param>
        /// <param name="depth">
        /// Number of bits in depth buffer (0, 16 or 24). Note that only 24 bit depth has stencil buffer.
        /// </param>
        /// <param name="format">
        /// Texture color format.
        /// </param>
        /// <param name="readWrite">
        /// How or if color space conversions should be done on texture read/write.
        /// </param>
        /// <description>
        /// The render texture is created with /width/ by /height/ size, with a depth buffer
        /// of /depth/ bits (depth can be 0, 16 or 24), and in /format/ format and with sRGB read / write on or off.
        /// Note that constructing a RenderTexture object does not create the hardware representation immediately.
        /// The actual render texture is created upon first use or when Create is called manually. So after
        /// constructing the render texture, it is possible to set additional variables, like format,
        public RenderTexture(int width, int height, int depth, [uei.DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format, [uei.DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite)
            : this(width, height, depth, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite))
        {
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public RenderTexture(int width, int height, int depth, RenderTextureFormat format)
            : this(width, height, depth, GraphicsFormatUtility.GetGraphicsFormat(format, RenderTextureReadWrite.Default))
        {
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public RenderTexture(int width, int height, int depth)
            : this(width, height, depth, GraphicsFormatUtility.GetGraphicsFormat(RenderTextureFormat.Default, RenderTextureReadWrite.Default))
        {
        }

        /// <summary>
        /// This struct contains all the information required to create a RenderTexture. It can be copied, cached, and reused to easily create RenderTextures that all share the same properties.
        /// </summary>
        public RenderTextureDescriptor descriptor
        {
            get { return GetDescriptor(); }
            set { ValidateRenderTextureDesc(value); SetRenderTextureDescriptor(value); }
        }


        private static void ValidateRenderTextureDesc(RenderTextureDescriptor desc)
        {
            if (!SystemInfo.IsFormatSupported(desc.graphicsFormat, FormatUsage.Render))
                throw new ArgumentException("RenderTextureDesc graphicsFormat must be a supported GraphicsFormat. " + desc.graphicsFormat + " is not supported.", "desc.graphicsFormat");
            if (desc.width <= 0)
                throw new ArgumentException("RenderTextureDesc width must be greater than zero.", "desc.width");
            if (desc.height <= 0)
                throw new ArgumentException("RenderTextureDesc height must be greater than zero.", "desc.height");
            if (desc.volumeDepth <= 0)
                throw new ArgumentException("RenderTextureDesc volumeDepth must be greater than zero.", "desc.volumeDepth");
            if (desc.msaaSamples != 1 && desc.msaaSamples != 2 && desc.msaaSamples != 4 && desc.msaaSamples != 8)
                throw new ArgumentException("RenderTextureDesc msaaSamples must be 1, 2, 4, or 8.", "desc.msaaSamples");
            if (desc.depthBufferBits != 0 && desc.depthBufferBits != 16 && desc.depthBufferBits != 24)
                throw new ArgumentException("RenderTextureDesc depthBufferBits must be 0, 16, or 24.", "desc.depthBufferBits");
        }
    }

    public partial class RenderTexture : Texture
    {
        internal static GraphicsFormat GetCompatibleFormat(RenderTextureFormat renderTextureFormat, RenderTextureReadWrite readWrite)
        {
            GraphicsFormat requestedFormat = GraphicsFormatUtility.GetGraphicsFormat(renderTextureFormat, readWrite);
            GraphicsFormat compatibleFormat = SystemInfo.GetCompatibleFormat(requestedFormat, FormatUsage.Render);

            if (requestedFormat == compatibleFormat)
            {
                return requestedFormat;
            }
            else
            {
                Debug.LogWarning(String.Format("'{0}' is not supported. RenderTexture::GetTemporary fallbacks to {1} format on this platform. Use 'SystemInfo.IsFormatSupported' C# API to check format support.", requestedFormat.ToString(), compatibleFormat.ToString()));
                return compatibleFormat;
            }
        }

        /// <summary>
        /// Allocate a temporary render texture.
        /// </summary>
        /// <param name="desc">
        /// Use this RenderTextureDesc for the settings when creating the temporary RenderTexture.
        /// </param>
        /// <description>
        /// This function is optimized for when you need a quick RenderTexture to do some temporary calculations.
        /// Release it using ReleaseTemporary as soon as you're done with it, so another call can
        /// start reusing it if needed.
        /// Internally Unity keeps a pool of temporary render textures, so a call to GetTemporary most often
        /// just returns an already created one (if the size and format matches). These temporary render textures
        /// are actually destroyed when they aren't used for a couple of frames.
        /// If you are doing a series of post-processing "blits", it's best for performance to get and release
        /// a temporary render texture for each blit, instead of getting one or two render textures upfront and reusing
        /// them. This is mostly beneficial for mobile (tile-based) and multi-GPU systems: GetTemporary will internally
        /// do a DiscardContents call which helps to avoid costly restore operations on the previous
        /// render texture contents.
        /// You can not depend on any particular contents of the RenderTexture you get from GetTemporary function.
        /// It might be garbage, or it might be cleared to some color, depending on the platform.
        /// SA: ReleaseTemporary.
        /// </description>
        public static RenderTexture GetTemporary(RenderTextureDescriptor desc)
        {
            ValidateRenderTextureDesc(desc); desc.createdFromScript = true;
            return GetTemporary_Internal(desc);
        }

        // in old bindings "default args" were expanded into overloads and we must mimic that when migrating to new bindings
        // to keep things sane we will do internal methods WITH default args and do overloads that simply call it

        private static RenderTexture GetTemporaryImpl(int width, int height, int depthBuffer,
            GraphicsFormat format,
            int antiAliasing = 1, RenderTextureMemoryless memorylessMode = RenderTextureMemoryless.None,
            VRTextureUsage vrUsage = VRTextureUsage.None, bool useDynamicScale = false)
        {
            RenderTextureDescriptor desc = new RenderTextureDescriptor(width, height, format, depthBuffer);
            desc.msaaSamples = antiAliasing;
            desc.memoryless = memorylessMode;
            desc.vrUsage = vrUsage;
            desc.useDynamicScale = useDynamicScale;

            return GetTemporary(desc);
        }

        // most detailed overload: use it to specify default values for docs
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format,
            [uei.DefaultValue("1")] int antiAliasing,
            [uei.DefaultValue("RenderTextureMemoryless.None")] RenderTextureMemoryless memorylessMode,
            [uei.DefaultValue("VRTextureUsage.None")] VRTextureUsage vrUsage,
            [uei.DefaultValue("false")] bool useDynamicScale)
        {
            return GetTemporaryImpl(width, height, depthBuffer, format, antiAliasing, memorylessMode, vrUsage, useDynamicScale);
        }

        // the rest will be excluded from docs (to "pretend" we have one method with default args)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format, int antiAliasing, RenderTextureMemoryless memorylessMode, VRTextureUsage vrUsage)
        {
            return GetTemporaryImpl(width, height, depthBuffer, format, antiAliasing, memorylessMode, vrUsage);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format, int antiAliasing, RenderTextureMemoryless memorylessMode)
        {
            return GetTemporaryImpl(width, height, depthBuffer, format, antiAliasing, memorylessMode);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format, int antiAliasing)
        {
            return GetTemporaryImpl(width, height, depthBuffer, format, antiAliasing);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, GraphicsFormat format)
        {
            return GetTemporaryImpl(width, height, depthBuffer, format);
        }

        // most detailed overload: use it to specify default values for docs
        /// <summary>
        /// Allocate a temporary render texture.
        /// </summary>
        /// <param name="width">
        /// Width in pixels.
        /// </param>
        /// <param name="height">
        /// Height in pixels.
        /// </param>
        /// <param name="depthBuffer">
        /// Depth buffer bits (0, 16 or 24). Note that only 24 bit depth has stencil buffer.
        /// </param>
        /// <param name="format">
        /// Render texture format.
        /// </param>
        /// <param name="readWrite">
        /// Color space conversion mode.
        /// </param>
        /// <param name="antiAliasing">
        /// Number of antialiasing samples to store in the texture. Valid values are 1, 2, 4, and 8. Throws an exception if any other value is passed.
        /// </param>
        /// <param name="memorylessMode">
        /// Render texture memoryless mode.
        /// </param>
        /// <description>
        /// This function is optimized for when you need a quick RenderTexture to do some temporary calculations.
        /// Release it using ReleaseTemporary as soon as you're done with it, so another call can
        /// start reusing it if needed.
        /// Internally Unity keeps a pool of temporary render textures, so a call to GetTemporary most often
        /// just returns an already created one (if the size and format matches). These temporary render textures
        /// are actually destroyed when they aren't used for a couple of frames.
        /// If you are doing a series of post-processing "blits", it's best for performance to get and release
        /// a temporary render texture for each blit, instead of getting one or two render textures upfront and reusing
        /// them. This is mostly beneficial for mobile (tile-based) and multi-GPU systems: GetTemporary will internally
        /// do a DiscardContents call which helps to avoid costly restore operations on the previous
        /// render texture contents.
        /// You can not depend on any particular contents of the RenderTexture you get from GetTemporary function.
        /// It might be garbage, or it might be cleared to some color, depending on the platform.
        /// SA: ReleaseTemporary.
        /// </description>
        public static RenderTexture GetTemporary(int width, int height,
            [uei.DefaultValue("0")] int depthBuffer, [uei.DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat format,
            [uei.DefaultValue("RenderTextureReadWrite.Default")] RenderTextureReadWrite readWrite, [uei.DefaultValue("1")] int antiAliasing,
            [uei.DefaultValue("RenderTextureMemoryless.None")] RenderTextureMemoryless memorylessMode,
            [uei.DefaultValue("VRTextureUsage.None")] VRTextureUsage vrUsage, [uei.DefaultValue("false")] bool useDynamicScale
        )
        {
            return GetTemporaryImpl(width, height, depthBuffer, GraphicsFormatUtility.GetGraphicsFormat(format, readWrite), antiAliasing, memorylessMode, vrUsage, useDynamicScale);
        }

        // the rest will be excluded from docs (to "pretend" we have one method with default args)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, RenderTextureMemoryless memorylessMode, VRTextureUsage vrUsage)
        {
            return GetTemporaryImpl(width, height, depthBuffer, GetCompatibleFormat(format, readWrite), antiAliasing, memorylessMode, vrUsage);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing, RenderTextureMemoryless memorylessMode)
        {
            return GetTemporaryImpl(width, height, depthBuffer, GetCompatibleFormat(format, readWrite), antiAliasing, memorylessMode);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite, int antiAliasing)
        {
            return GetTemporaryImpl(width, height, depthBuffer, GetCompatibleFormat(format, readWrite), antiAliasing);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format, RenderTextureReadWrite readWrite)
        {
            return GetTemporaryImpl(width, height, depthBuffer, GetCompatibleFormat(format, readWrite));
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer, RenderTextureFormat format)
        {
            return GetTemporaryImpl(width, height, depthBuffer, GetCompatibleFormat(format, RenderTextureReadWrite.Default));
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height, int depthBuffer)
        {
            return GetTemporaryImpl(width, height, depthBuffer, GetCompatibleFormat(RenderTextureFormat.Default, RenderTextureReadWrite.Default));
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static RenderTexture GetTemporary(int width, int height)
        {
            return GetTemporaryImpl(width, height, 0, GetCompatibleFormat(RenderTextureFormat.Default, RenderTextureReadWrite.Default));
        }
    }

    public sealed partial class CustomRenderTexture : RenderTexture
    {
        // Be careful. We can't call base constructor here because it would create the native object twice.
        public CustomRenderTexture(int width, int height, RenderTextureFormat format, RenderTextureReadWrite readWrite)
            : this(width, height, GetCompatibleFormat(format, readWrite))
        {
        }

        public CustomRenderTexture(int width, int height, RenderTextureFormat format)
            : this(width, height, GetCompatibleFormat(format, RenderTextureReadWrite.Default))
        {
        }

        public CustomRenderTexture(int width, int height)
            : this(width, height, SystemInfo.GetGraphicsFormat(DefaultFormat.LDR))
        {
        }

        public CustomRenderTexture(int width, int height, DefaultFormat defaultFormat)
            : this(width, height, SystemInfo.GetGraphicsFormat(defaultFormat))
        {
        }

        public CustomRenderTexture(int width, int height, GraphicsFormat format)
        {
            if (!ValidateFormat(format, FormatUsage.Render))
                return;

            Internal_CreateCustomRenderTexture(this);
            this.width = width;
            this.height = height;
            this.graphicsFormat = format;

            SetSRGBReadWrite(GraphicsFormatUtility.IsSRGBFormat(format));
        }

        bool IsCubemapFaceEnabled(CubemapFace face)
        {
            return (cubemapFaceMask & (1 << (int)face)) != 0;
        }

        void EnableCubemapFace(CubemapFace face, bool value)
        {
            uint oldValue = cubemapFaceMask;
            uint bit = 1u << (int)face;
            if (value)
                oldValue |= bit;
            else
                oldValue &= ~bit;
            cubemapFaceMask = oldValue;
        }
    }

    public partial class Texture : Object
    {
        internal bool ValidateFormat(RenderTextureFormat format)
        {
            if (SystemInfo.SupportsRenderTextureFormat(format))
            {
                return true;
            }
            else
            {
                Debug.LogError(String.Format("RenderTexture creation failed. '{0}' is not supported on this platform. Use 'SystemInfo.SupportsRenderTextureFormat' C# API to check format support.", format.ToString()), this);
                return false;
            }
        }

        internal bool ValidateFormat(TextureFormat format)
        {
            if (SystemInfo.SupportsTextureFormat(format))
            {
                return true;
            }
            else if (GraphicsFormatUtility.IsCompressedTextureFormat(format))
            {
                Debug.LogWarning(String.Format("'{0}' is not supported on this platform. Decompressing texture. Use 'SystemInfo.SupportsTextureFormat' C# API to check format support.", format.ToString()), this);
                return true;
            }
            else
            {
                Debug.LogError(String.Format("Texture creation failed. '{0}' is not supported on this platform. Use 'SystemInfo.SupportsTextureFormat' C# API to check format support.", format.ToString()), this);
                return false;
            }
        }

        internal bool ValidateFormat(GraphicsFormat format, FormatUsage usage)
        {
            if (SystemInfo.IsFormatSupported(format, usage))
            {
                return true;
            }
            else
            {
                Debug.LogError(String.Format("Texture creation failed. '{0}' is not supported for {1} usage on this platform. Use 'SystemInfo.IsFormatSupported' C# API to check format support.", format.ToString(), usage.ToString()), this);
                return false;
            }
        }

        internal UnityException CreateNonReadableException(Texture t)
        {
            return new UnityException(
                String.Format("Texture '{0}' is not readable, the texture memory can not be accessed from scripts. You can make the texture readable in the Texture Import Settings.", t.name)
            );
        }
    }


    public partial class Texture2D : Texture
    {
        internal Texture2D(int width, int height, GraphicsFormat format, TextureCreationFlags flags, IntPtr nativeTex)
        {
            if (ValidateFormat(format, FormatUsage.Sample))
                Internal_Create(this, width, height, format, flags, nativeTex);
        }

        public Texture2D(int width, int height, DefaultFormat format, TextureCreationFlags flags)
            : this(width, height, SystemInfo.GetGraphicsFormat(format), flags)
        {
        }

        public Texture2D(int width, int height, GraphicsFormat format, TextureCreationFlags flags)
            : this(width, height, format, flags, IntPtr.Zero)
        {
        }

        internal Texture2D(int width, int height, TextureFormat textureFormat, bool mipChain, bool linear, IntPtr nativeTex)
        {
            if (!ValidateFormat(textureFormat))
                return;

            GraphicsFormat format = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, !linear);
            TextureCreationFlags flags = TextureCreationFlags.None;
            if (mipChain)
                flags |= TextureCreationFlags.MipChain;
            if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
                flags |= TextureCreationFlags.Crunch;
            Internal_Create(this, width, height, format, flags, nativeTex);
        }

        public Texture2D(int width, int height, [uei.DefaultValue("TextureFormat.RGBA32")] TextureFormat textureFormat, [uei.DefaultValue("true")] bool mipChain, [uei.DefaultValue("false")] bool linear)
            : this(width, height, textureFormat, mipChain, linear, IntPtr.Zero)
        {
        }

        public Texture2D(int width, int height, TextureFormat textureFormat, bool mipChain)
            : this(width, height, textureFormat, mipChain, false, IntPtr.Zero)
        {
        }

        public Texture2D(int width, int height)
            : this(width, height, TextureFormat.RGBA32, true, false, IntPtr.Zero)
        {
        }

        public static Texture2D CreateExternalTexture(int width, int height, TextureFormat format, bool mipChain, bool linear, IntPtr nativeTex)
        {
            if (nativeTex == IntPtr.Zero)
                throw new ArgumentException("nativeTex can not be null");
            return new Texture2D(width, height, format, mipChain, linear, nativeTex);
        }

        public void SetPixel(int x, int y, Color color)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            SetPixelImpl(0, x, y, color);
        }

        public void SetPixel(int x, int y, Color color, int mipLevel)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            SetPixelImpl(mipLevel, x, y, color);
        }

        public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors, [uei.DefaultValue("0")] int miplevel)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            SetPixelsImpl(x, y, blockWidth, blockHeight, colors, miplevel, 0);
        }

        public void SetPixels(int x, int y, int blockWidth, int blockHeight, Color[] colors)
        {
            SetPixels(x, y, blockWidth, blockHeight, colors, 0);
        }

        public void SetPixels(Color[] colors, [uei.DefaultValue("0")] int miplevel)
        {
            int w = width >> miplevel; if (w < 1) w = 1;
            int h = height >> miplevel; if (h < 1) h = 1;
            SetPixels(0, 0, w, h, colors, miplevel);
        }

        public void SetPixels(Color[] colors)
        {
            SetPixels(0, 0, width, height, colors, 0);
        }

        public Color GetPixel(int x, int y)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelImpl(0, x, y);
        }

        public Color GetPixel(int x, int y, int mipLevel)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelImpl(mipLevel, x, y);
        }

        public Color GetPixelBilinear(float u, float v)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelBilinearImpl(0, u, v);
        }

        public Color GetPixelBilinear(float u, float v, int mipLevel)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelBilinearImpl(mipLevel, u, v);
        }

        public void LoadRawTextureData(IntPtr data, int size)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            if (data == IntPtr.Zero || size == 0) { Debug.LogError("No texture data provided to LoadRawTextureData", this); return; }
            if (!LoadRawTextureDataImpl(data, size))
                throw new UnityException("LoadRawTextureData: not enough data provided (will result in overread).");
        }

        public void LoadRawTextureData(byte[] data)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            if (data == null || data.Length == 0) { Debug.LogError("No texture data provided to LoadRawTextureData", this); return; }
            if (!LoadRawTextureDataImplArray(data))
                throw new UnityException("LoadRawTextureData: not enough data provided (will result in overread).");
        }

        unsafe public void LoadRawTextureData<T>(NativeArray<T> data) where T : struct
        {
            if (!isReadable) throw CreateNonReadableException(this);
            if (!data.IsCreated || data.Length == 0) throw new UnityException("No texture data provided to LoadRawTextureData");
            if (!LoadRawTextureDataImpl((IntPtr)data.GetUnsafeReadOnlyPtr(), data.Length * UnsafeUtility.SizeOf<T>()))
                throw new UnityException("LoadRawTextureData: not enough data provided (will result in overread).");
        }

        public unsafe NativeArray<T> GetRawTextureData<T>() where T : struct
        {
            if (!isReadable) throw CreateNonReadableException(this);

            int stride = UnsafeUtility.SizeOf<T>();
            var array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)GetWritableImageData(0), (int)(GetRawImageDataSize() / stride), Allocator.None);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref array, Texture2D.GetSafetyHandle(this));
#endif
            return array;
        }

        public void Apply([uei.DefaultValue("true")] bool updateMipmaps, [uei.DefaultValue("false")] bool makeNoLongerReadable)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            ApplyImpl(updateMipmaps, makeNoLongerReadable);
        }

        public void Apply(bool updateMipmaps) { Apply(updateMipmaps, false); }
        public void Apply() { Apply(true, false); }

        public bool Resize(int width, int height)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return ResizeImpl(width, height);
        }

        public bool Resize(int width, int height, TextureFormat format, bool hasMipMap)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return ResizeWithFormatImpl(width, height, format, hasMipMap);
        }

        public void ReadPixels(Rect source, int destX, int destY, [uei.DefaultValue("true")] bool recalculateMipMaps)
        {
            //if (ValidateFormat(GraphicsFormatUtility.GetGraphicsFormat(format, ), FormatUsage.ReadPixels))
            //    Debug.LogError("No texture data provided to LoadRawTextureData", this);
            if (!isReadable) throw CreateNonReadableException(this);
            ReadPixelsImpl(source, destX, destY, recalculateMipMaps);
        }

        [uei.ExcludeFromDocs] public void ReadPixels(Rect source, int destX, int destY) { ReadPixels(source, destX, destY, true); }

        public static bool GenerateAtlas(Vector2[] sizes, int padding, int atlasSize, List<Rect> results)
        {
            if (sizes == null)
                throw new ArgumentException("sizes array can not be null");
            if (results == null)
                throw new ArgumentException("results list cannot be null");
            if (padding < 0)
                throw new ArgumentException("padding can not be negative");
            if (atlasSize <= 0)
                throw new ArgumentException("atlas size must be positive");

            results.Clear();
            if (sizes.Length == 0)
                return true;

            NoAllocHelpers.EnsureListElemCount(results, sizes.Length);
            GenerateAtlasImpl(sizes, padding, atlasSize, NoAllocHelpers.ExtractArrayFromListT(results));
            return results.Count != 0;
        }

        public void SetPixels32(Color32[] colors, int miplevel)
        {
            SetAllPixels32(colors, miplevel);
        }

        public void SetPixels32(Color32[] colors)
        {
            SetPixels32(colors, 0);
        }

        public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors, int miplevel)
        {
            SetBlockOfPixels32(x, y, blockWidth, blockHeight, colors, miplevel);
        }

        public void SetPixels32(int x, int y, int blockWidth, int blockHeight, Color32[] colors)
        {
            SetPixels32(x, y, blockWidth, blockHeight, colors, 0);
        }

        public Color[] GetPixels(int miplevel)
        {
            int w = width >> miplevel; if (w < 1) w = 1;
            int h = height >> miplevel; if (h < 1) h = 1;
            return GetPixels(0, 0, w, h, miplevel);
        }

        public Color[] GetPixels()
        {
            return GetPixels(0);
        }

        /// <summary>
        /// Flags used to control the encoding to an EXR file.
        /// </summary>
        /// <description>
        /// SA: ImageConversion.EncodeToEXR.
        /// </description>
        [Flags]
        public enum EXRFlags
        {
            /// <summary>
            /// No flag. This will result in an uncompressed 16-bit float EXR file.
            /// </summary>
            None = 0,
            /// <summary>
            /// The texture will be exported as a 32-bit float EXR file (default is 16-bit).
            /// </summary>
            OutputAsFloat = 1 << 0, // Default is Half
            // Compression are mutually exclusive.
            /// <summary>
            /// The texture will use the EXR ZIP compression format.
            /// </summary>
            CompressZIP = 1 << 1,
            /// <summary>
            /// The texture will use RLE (Run Length Encoding) EXR compression format (similar to Targa RLE compression).
            /// </summary>
            CompressRLE = 1 << 2,
            /// <summary>
            /// This texture will use Wavelet compression. This is best used for grainy images.
            /// </summary>
            CompressPIZ = 1 << 3,
        }
    }

    public sealed partial class Cubemap : Texture
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Cubemap(int width, DefaultFormat format, TextureCreationFlags flags)
            : this(width, SystemInfo.GetGraphicsFormat(format), flags)
        {
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [RequiredByNativeCode] // used to create builtin textures
        public Cubemap(int width, GraphicsFormat format, TextureCreationFlags flags)
        {
            if (ValidateFormat(format, FormatUsage.Sample))
                Internal_Create(this, width, format, flags, IntPtr.Zero);
        }

        internal Cubemap(int width, TextureFormat textureFormat, bool mipChain, IntPtr nativeTex)
        {
            if (!ValidateFormat(textureFormat))
                return;

            GraphicsFormat format = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, false);
            TextureCreationFlags flags = TextureCreationFlags.None;
            if (mipChain)
                flags |= TextureCreationFlags.MipChain;
            if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
                flags |= TextureCreationFlags.Crunch;

            Internal_Create(this, width, format, flags, nativeTex);
        }

        /// <summary>
        /// Create a new empty cubemap texture.
        /// </summary>
        /// <description>
        /// The texture will be /size/ on each side and can be created with or without mipmaps.
        /// Usually you will want to set the colors of the texture after creating it, using
        public Cubemap(int width, TextureFormat textureFormat, bool mipChain)
            : this(width, textureFormat, mipChain, IntPtr.Zero)
        {
        }

        /// <summary>
        /// Creates a Unity cubemap out of externally created native cubemap object.
        /// </summary>
        /// <param name="format">
        /// Format of underlying cubemap object.
        /// </param>
        /// <param name="mipmap">
        /// Does the cubemap have mipmaps?
        /// </param>
        /// <param name="nativeTex">
        /// Native cubemap texture object.
        /// </param>
        /// <description>
        /// This method is mostly useful for [[wiki:NativePluginInterface|native code plugins]] that create platform specific cubemap texture
        /// objects outside of Unity, and need to use these cubemaps in Unity Scenes.
        /// Parameters passed to CreateExternalTexture should match what the texture actually is; and the underlying texture should be a Cubemap (2D textures will not work).
        /// Native texture object on Direct3D-like devices is a pointer to the base type, from which a texture can be created
        /// (ID3D11ShaderResourceView on D3D11). On OpenGL/OpenGL ES it is GLuint. On Metal it is id<MTLTexture>.
        /// SA: UpdateExternalTexture, [[Texture.GetNativeTexturePtr]].
        /// </description>
        public static Cubemap CreateExternalTexture(int width, TextureFormat format, bool mipmap, IntPtr nativeTex)
        {
            if (nativeTex == IntPtr.Zero)
                throw new ArgumentException("nativeTex can not be null");
            return new Cubemap(width, format, mipmap, nativeTex);
        }

        /// <summary>
        /// Sets pixel color at coordinates (face, x, y).
        /// </summary>
        /// <description>
        /// Call Apply to actually upload the changed pixels to the graphics card.
        /// Uploading is an expensive operation, so you'll want to change as many pixels
        /// as possible between /Apply/ calls.
        /// This method works only on /RGBA32/, /ARGB32/, /RGB24/ and /Alpha8/ texture formats.
        /// For other formats /SetPixel/ is ignored.
        /// SA: Apply method.
        /// </description>
        public void SetPixel(CubemapFace face, int x, int y, Color color)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            SetPixelImpl((int)face, x, y, color);
        }

        /// <summary>
        /// Returns pixel color at coordinates (face, x, y).
        /// </summary>
        /// <description>
        /// If the pixel coordinates are out of bounds (larger than width/height or small than 0),
        /// they will be clamped or repeat based on the texture's wrap mode.
        /// The texture must have the __Is Readable__ flag set in the import settings, otherwise this method will fail. GetPixel is not available on Textures using Crunch texture compression.
        /// </description>
        public Color GetPixel(CubemapFace face, int x, int y)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelImpl((int)face, x, y);
        }

        /// <summary>
        /// Actually apply all previous SetPixel and SetPixels changes.
        /// </summary>
        /// <param name="updateMipmaps">
        /// When set to true, mipmap levels are recalculated.
        /// </param>
        /// <param name="makeNoLongerReadable">
        /// When set to true, system memory copy of a texture is released.
        /// </param>
        /// <description>
        /// If /updateMipmaps/ is /true/, the mipmap levels are recalculated as well, using
        /// the base level as a source. Usually you want to use /true/ in all cases except when
        /// you've modified the mip levels yourself using SetPixels.
        /// Apply is a potentially expensive operation, so you'll want to change as many pixels
        /// as possible between /Apply/ calls.
        /// SA: SetPixel, SetPixels functions.
        /// </description>
        public void Apply([uei.DefaultValue("true")] bool updateMipmaps, [uei.DefaultValue("false")] bool makeNoLongerReadable)
        {
#if !UNITY_EDITOR
            if (!isReadable) throw CreateNonReadableException(this);
#endif
            ApplyImpl(updateMipmaps, makeNoLongerReadable);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void Apply(bool updateMipmaps) { Apply(updateMipmaps, false); }
        /// <summary>
        /// Actually apply all previous SetPixel and SetPixels changes.
        /// </summary>
        /// <description>
        /// If /updateMipmaps/ is /true/, the mipmap levels are recalculated as well, using
        /// the base level as a source. Usually you want to use /true/ in all cases except when
        /// you've modified the mip levels yourself using SetPixels.
        /// Apply is a potentially expensive operation, so you'll want to change as many pixels
        /// as possible between /Apply/ calls.
        /// SA: SetPixel, SetPixels functions.
        /// </description>
        public void Apply() { Apply(true, false); }
    }

    public sealed partial class Texture3D : Texture
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Texture3D(int width, int height, int depth, DefaultFormat format, TextureCreationFlags flags)
            : this(width, height, depth, SystemInfo.GetGraphicsFormat(format), flags)
        {
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [RequiredByNativeCode] // used to create builtin textures
        public Texture3D(int width, int height, int depth, GraphicsFormat format, TextureCreationFlags flags)
        {
            if (ValidateFormat(format, FormatUsage.Sample))
                Internal_Create(this, width, height, depth, format, flags);
        }

        /// <summary>
        /// Create a new empty 3D Texture.
        /// </summary>
        /// <param name="width">
        /// Width of texture in pixels.
        /// </param>
        /// <param name="height">
        /// Height of texture in pixels.
        /// </param>
        /// <param name="depth">
        /// Depth of texture in pixels.
        /// </param>
        /// <param name="textureFormat">
        /// Texture data format.
        /// </param>
        /// <param name="mipChain">
        /// Determines whether the texture has mipmaps or not. A value of 1 (true) means the texture does have mipmaps, and a value of 0 (false) means the texture doesn't have mipmaps.
        /// </param>
        /// <description>
        /// 3D textures can be thought of as a box of pixels, with width, height and depth. Note that large textures can consume a lot of memory, for example a 1024x512x256 texture with [[TextureFormat.ARGB32]] format and no mipmaps will consume 512MB of memory.
        /// SA: SetPixel, SetPixels, SetPixels32, Apply functions.
        /// </description>
        public Texture3D(int width, int height, int depth, TextureFormat textureFormat, bool mipChain)
        {
            if (!ValidateFormat(textureFormat))
                return;

            GraphicsFormat format = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, false);
            TextureCreationFlags flags = TextureCreationFlags.None;
            if (mipChain)
                flags |= TextureCreationFlags.MipChain;
            if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
                flags |= TextureCreationFlags.Crunch;
            Internal_Create(this, width, height, depth, format, flags);
        }

        /// <summary>
        /// Actually apply all previous SetPixels changes.
        /// </summary>
        /// <param name="updateMipmaps">
        /// When set to true, mipmap levels are recalculated.
        /// </param>
        /// <param name="makeNoLongerReadable">
        /// When set to true, system memory copy of a texture is released.
        /// </param>
        /// <description>
        /// If /updateMipmaps/ is /true/, the mipmap levels are recalculated as well, using
        /// the base level as a source. Usually you want to use /true/ in all cases except when
        /// you've modified the mip levels yourself using SetPixels.
        /// If /makeNoLongerReadable/ is /true/, texture will be marked as no longer readable
        /// and memory will be freed after uploading to GPU.
        /// By default /makeNoLongerReadable/ is set to /false/.
        /// Apply is a potentially expensive operation, so you'll want to change as many pixels
        /// as possible between /Apply/ calls.
        /// Alternatively, if you don't need to access the pixels on the CPU, you could use [[Graphics.CopyTexture]]
        /// for fast GPU-side texture data copies. Note that calling /Apply/ may undo the results of previous calls to [[Graphics.CopyTexture]].
        /// SA: SetPixels, [[Graphics.CopyTexture]].
        /// </description>
        public void Apply([uei.DefaultValue("true")] bool updateMipmaps, [uei.DefaultValue("false")] bool makeNoLongerReadable)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            ApplyImpl(updateMipmaps, makeNoLongerReadable);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void Apply(bool updateMipmaps) { Apply(updateMipmaps, false); }
        /// <summary>
        /// Actually apply all previous SetPixels changes.
        /// </summary>
        /// <description>
        /// If /updateMipmaps/ is /true/, the mipmap levels are recalculated as well, using
        /// the base level as a source. Usually you want to use /true/ in all cases except when
        /// you've modified the mip levels yourself using SetPixels.
        /// If /makeNoLongerReadable/ is /true/, texture will be marked as no longer readable
        /// and memory will be freed after uploading to GPU.
        /// By default /makeNoLongerReadable/ is set to /false/.
        /// Apply is a potentially expensive operation, so you'll want to change as many pixels
        /// as possible between /Apply/ calls.
        /// Alternatively, if you don't need to access the pixels on the CPU, you could use [[Graphics.CopyTexture]]
        /// for fast GPU-side texture data copies. Note that calling /Apply/ may undo the results of previous calls to [[Graphics.CopyTexture]].
        /// SA: SetPixels, [[Graphics.CopyTexture]].
        /// </description>
        public void Apply() { Apply(true, false); }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetPixel(int x, int y, int z, Color color)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            SetPixelImpl(0, x, y, z, color);
        }

        /// <summary>
        /// Sets the pixel color that represents one mip level of the 3D texture at coordinates (x,y,z).
        /// </summary>
        /// <param name="x">
        /// X coordinate to access a pixel.
        /// </param>
        /// <param name="y">
        /// Y coordinate to access a pixel.
        /// </param>
        /// <param name="z">
        /// Z coordinate to access a pixel.
        /// </param>
        /// <param name="color">
        /// The colors to set the pixels to.
        /// </param>
        /// <param name="mipLevel">
        /// The mipmap level to be affected by the new colors.
        /// </param>
        /// <description>
        /// This function works only on RGBA32, ARGB32, RGB24 and Alpha8 texture formats. For other formats, SetPixel is ignored. The texture also has to have the Read/Write Enabled flag set in the Import Settings.
        /// SA: GetPixel, GetPixelBilinear, GetPixels, GetPixels32, SetPixels, SetPixels32, Apply functions.
        /// </description>
        public void SetPixel(int x, int y, int z, Color color, int mipLevel)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            SetPixelImpl(mipLevel, x, y, z, color);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Color GetPixel(int x, int y, int z)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelImpl(0, x, y, z);
        }

        /// <summary>
        /// Returns the pixel color that represents one mip level of the 3D texture at coordinates (x,y,z).
        /// </summary>
        /// <param name="x">
        /// X coordinate to access a pixel.
        /// </param>
        /// <param name="y">
        /// Y coordinate to access a pixel.
        /// </param>
        /// <param name="z">
        /// Z coordinate to access a pixel.
        /// </param>
        /// <param name="mipLevel">
        /// The mipmap level to be accessed.
        /// </param>
        /// <returns>
        /// The color of the pixel.
        /// </returns>
        /// <description>
        /// The texture must have the Read/Write Enabled flag set in the Import Settings, otherwise this function will fail. GetPixel is not available on Textures using Crunch texture compression.
        /// SA: SetPixel, SetPixels, SetPixels32, GetPixelBilinear, GetPixels, GetPixels32, Apply functions.
        /// </description>
        public Color GetPixel(int x, int y, int z, int mipLevel)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelImpl(mipLevel, x, y, z);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Color GetPixelBilinear(float u, float v, float w)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelBilinearImpl(0, u, v, w);
        }

        /// <summary>
        /// Returns the filtered pixel color that represents one mip level of the 3D texture at normalized coordinates (u,v,w).
        /// </summary>
        /// <param name="u">
        /// U normalized coordinate to access a pixel.
        /// </param>
        /// <param name="v">
        /// V normalized coordinate to access a pixel.
        /// </param>
        /// <param name="w">
        /// W normalized coordinate to access a pixel.
        /// </param>
        /// <param name="mipLevel">
        /// The mipmap level to be accessed.
        /// </param>
        /// <returns>
        /// The colors to return by bilinear filtering.
        /// </returns>
        /// <description>
        /// Coordinates /u/, /v/, and /w/ go from 0.0 to 1.0. Texture3D.GetPixelBilinear works in a similar way to Texture2D.GetPixelBilinear(), but with an extra /w/ coordinate. Also, the bounds are expanded to width, height, and depth.
        /// SA: SetPixel, SetPixels, SetPixels32, GetPixel, GetPixels, GetPixels32, Apply functions.
        /// </description>
        public Color GetPixelBilinear(float u, float v, float w, int mipLevel)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            return GetPixelBilinearImpl(mipLevel, u, v, w);
        }
    }

    public sealed partial class Texture2DArray : Texture
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public Texture2DArray(int width, int height, int depth, DefaultFormat format, TextureCreationFlags flags)
            : this(width, height, depth, SystemInfo.GetGraphicsFormat(format), flags)
        {
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [RequiredByNativeCode] // used to create builtin textures
        public Texture2DArray(int width, int height, int depth, GraphicsFormat format, TextureCreationFlags flags)
        {
            if (ValidateFormat(format, FormatUsage.Sample))
                Internal_Create(this, width, height, depth, format, flags);
        }

        /// <summary>
        /// Create a new texture array.
        /// </summary>
        /// <param name="width">
        /// Width of texture array in pixels.
        /// </param>
        /// <param name="height">
        /// Height of texture array in pixels.
        /// </param>
        /// <param name="depth">
        /// Number of elements in the texture array.
        /// </param>
        /// <param name="linear">
        /// Does the texture contain non-color data (i.e. don't do any color space conversions when sampling)? Default is false.
        /// </param>
        /// <description>
        /// Usually you will want to set the colors of the texture after creating it.
        /// Use SetPixels, SetPixels32 or [[Graphics.CopyTexture]] functions for that.
        /// </description>
        public Texture2DArray(int width, int height, int depth, TextureFormat textureFormat, bool mipChain, [uei.DefaultValue("true")] bool linear)
        {
            if (!ValidateFormat(textureFormat))
                return;

            GraphicsFormat format = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, !linear);
            TextureCreationFlags flags = TextureCreationFlags.None;
            if (mipChain)
                flags |= TextureCreationFlags.MipChain;
            if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
                flags |= TextureCreationFlags.Crunch;
            Internal_Create(this, width, height, depth, format, flags);
        }

        /// <summary>
        /// Create a new texture array.
        /// </summary>
        /// <param name="width">
        /// Width of texture array in pixels.
        /// </param>
        /// <param name="height">
        /// Height of texture array in pixels.
        /// </param>
        /// <param name="depth">
        /// Number of elements in the texture array.
        /// </param>
        /// <description>
        /// Usually you will want to set the colors of the texture after creating it.
        /// Use SetPixels, SetPixels32 or [[Graphics.CopyTexture]] functions for that.
        /// </description>
        public Texture2DArray(int width, int height, int depth, TextureFormat textureFormat, bool mipChain)
            : this(width, height, depth, textureFormat, mipChain, false)
        {
        }

        /// <summary>
        /// Actually apply all previous SetPixels changes.
        /// </summary>
        /// <param name="updateMipmaps">
        /// When set to true, mipmap levels are recalculated.
        /// </param>
        /// <param name="makeNoLongerReadable">
        /// When set to true, system memory copy of a texture is released.
        /// </param>
        /// <description>
        /// If /updateMipmaps/ is /true/, the mipmap levels are recalculated as well, using
        /// the base level as a source. Usually you want to use /true/ in all cases except when
        /// you've modified the mip levels yourself using SetPixels.
        /// If /makeNoLongerReadable/ is /true/, texture will be marked as no longer readable
        /// and memory will be freed after uploading to GPU.
        /// By default /makeNoLongerReadable/ is set to /false/.
        /// Apply is a potentially expensive operation, so you'll want to change as many pixels
        /// as possible between /Apply/ calls.
        /// Alternatively, if you don't need to access the pixels on the CPU, you could use [[Graphics.CopyTexture]]
        /// for fast GPU-side texture data copies. Note that calling /Apply/ may undo the results of previous calls to [[Graphics.CopyTexture]].
        /// SA: SetPixels, [[Graphics.CopyTexture]].
        /// </description>
        public void Apply([uei.DefaultValue("true")] bool updateMipmaps, [uei.DefaultValue("false")] bool makeNoLongerReadable)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            ApplyImpl(updateMipmaps, makeNoLongerReadable);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void Apply(bool updateMipmaps) { Apply(updateMipmaps, false); }
        /// <summary>
        /// Actually apply all previous SetPixels changes.
        /// </summary>
        /// <description>
        /// If /updateMipmaps/ is /true/, the mipmap levels are recalculated as well, using
        /// the base level as a source. Usually you want to use /true/ in all cases except when
        /// you've modified the mip levels yourself using SetPixels.
        /// If /makeNoLongerReadable/ is /true/, texture will be marked as no longer readable
        /// and memory will be freed after uploading to GPU.
        /// By default /makeNoLongerReadable/ is set to /false/.
        /// Apply is a potentially expensive operation, so you'll want to change as many pixels
        /// as possible between /Apply/ calls.
        /// Alternatively, if you don't need to access the pixels on the CPU, you could use [[Graphics.CopyTexture]]
        /// for fast GPU-side texture data copies. Note that calling /Apply/ may undo the results of previous calls to [[Graphics.CopyTexture]].
        /// SA: SetPixels, [[Graphics.CopyTexture]].
        /// </description>
        public void Apply() { Apply(true, false); }
    }

    public sealed partial class CubemapArray : Texture
    {
        public CubemapArray(int width, int cubemapCount, DefaultFormat format, TextureCreationFlags flags)
            : this(width, cubemapCount, SystemInfo.GetGraphicsFormat(format), flags)
        {
        }

        [RequiredByNativeCode]
        public CubemapArray(int width, int cubemapCount, GraphicsFormat format, TextureCreationFlags flags)
        {
            if (ValidateFormat(format, FormatUsage.Sample))
                Internal_Create(this, width, cubemapCount, format, flags);
        }

        public CubemapArray(int width, int cubemapCount, TextureFormat textureFormat, bool mipChain, [uei.DefaultValue("true")] bool linear)
        {
            if (!ValidateFormat(textureFormat))
                return;

            GraphicsFormat format = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, !linear);
            TextureCreationFlags flags = TextureCreationFlags.None;
            if (mipChain)
                flags |= TextureCreationFlags.MipChain;
            if (GraphicsFormatUtility.IsCrunchFormat(textureFormat))
                flags |= TextureCreationFlags.Crunch;
            Internal_Create(this, width, cubemapCount, format, flags);
        }

        public CubemapArray(int width, int cubemapCount, TextureFormat textureFormat, bool mipChain)
            : this(width, cubemapCount, textureFormat, mipChain, false)
        {
        }

        public void Apply([uei.DefaultValue("true")] bool updateMipmaps, [uei.DefaultValue("false")] bool makeNoLongerReadable)
        {
            if (!isReadable) throw CreateNonReadableException(this);
            ApplyImpl(updateMipmaps, makeNoLongerReadable);
        }

        public void Apply(bool updateMipmaps) { Apply(updateMipmaps, false); }
        public void Apply() { Apply(true, false); }
    }

    public sealed partial class SparseTexture : Texture
    {
        internal bool ValidateSize(int width, int height, GraphicsFormat format)
        {
            if (GraphicsFormatUtility.GetBlockSize(format) * (width / GraphicsFormatUtility.GetBlockWidth(format)) * (height / GraphicsFormatUtility.GetBlockHeight(format)) < 65536)
            {
                Debug.LogError(String.Format("SparseTexture creation failed. The minimum size in bytes of a SparseTexture is 64KB."), this);
                return false;
            }
            return true;
        }

        public SparseTexture(int width, int height, DefaultFormat format, int mipCount)
            : this(width, height, SystemInfo.GetGraphicsFormat(format), mipCount)
        {
        }

        public SparseTexture(int width, int height, GraphicsFormat format, int mipCount)
        {
            if (!ValidateFormat(format, FormatUsage.Sample))
                return;

            if (!ValidateSize(width, height, format))
                return;

            Internal_Create(this, width, height, format, mipCount);
        }

        public SparseTexture(int width, int height, TextureFormat textureFormat, int mipCount)
            : this(width, height, textureFormat, mipCount, false)
        {
        }

        public SparseTexture(int width, int height, TextureFormat textureFormat, int mipCount, bool linear)
        {
            if (!ValidateFormat(textureFormat))
                return;

            GraphicsFormat format = GraphicsFormatUtility.GetGraphicsFormat(textureFormat, !linear);
            if (!ValidateSize(width, height, format))
                return;

            Internal_Create(this, width, height, format, mipCount);
        }
    }
}
