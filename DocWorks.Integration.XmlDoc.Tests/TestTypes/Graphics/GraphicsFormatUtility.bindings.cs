using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
    namespace Experimental
    {
        namespace Rendering
        {
            /// <summary>
            /// There is currently no documentation for this api.
            /// </summary>
            [NativeHeader("Runtime/Graphics/TextureFormat.h")]
            [NativeHeader("Runtime/Graphics/GraphicsFormatUtility.bindings.h")]
            public class GraphicsFormatUtility
            {
                [FreeFunction]
                extern internal static GraphicsFormat GetFormat(Texture texture);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                public static GraphicsFormat GetGraphicsFormat(TextureFormat format, bool isSRGB)
                {
                    return GetGraphicsFormat_Native_TextureFormat(format, isSRGB);
                }

                [FreeFunction]
                extern private static GraphicsFormat GetGraphicsFormat_Native_TextureFormat(TextureFormat format, bool isSRGB);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                public static TextureFormat GetTextureFormat(GraphicsFormat format)
                {
                    return GetTextureFormat_Native_GraphicsFormat(format);
                }

                [FreeFunction]
                extern private static TextureFormat GetTextureFormat_Native_GraphicsFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                public static GraphicsFormat GetGraphicsFormat(RenderTextureFormat format, bool isSRGB)
                {
                    return GetGraphicsFormat_Native_RenderTextureFormat(format, isSRGB);
                }

                [FreeFunction]
                extern private static GraphicsFormat GetGraphicsFormat_Native_RenderTextureFormat(RenderTextureFormat format, bool isSRGB);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                public static GraphicsFormat GetGraphicsFormat(RenderTextureFormat format, RenderTextureReadWrite readWrite)
                {
                    bool defaultSRGB = QualitySettings.activeColorSpace == ColorSpace.Linear;
                    bool sRGB = readWrite == RenderTextureReadWrite.Default ? defaultSRGB : readWrite == RenderTextureReadWrite.sRGB;
                    return GetGraphicsFormat(format, sRGB);
                }

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsSRGBFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsSwizzleFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static GraphicsFormat GetSRGBFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static GraphicsFormat GetLinearFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static RenderTextureFormat GetRenderTextureFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static UInt32 GetColorComponentCount(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static UInt32 GetAlphaComponentCount(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static UInt32 GetComponentCount(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static string GetFormatString(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsCompressedFormat(GraphicsFormat format);

                [FreeFunction("IsAnyCompressedTextureFormat")]
                extern internal static bool IsCompressedTextureFormat(TextureFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsPackedFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool Is16BitPackedFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static GraphicsFormat ConvertToAlphaFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsAlphaOnlyFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsAlphaTestFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool HasAlphaChannel(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsDepthFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsStencilFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsIEEE754Format(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsFloatFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsHalfFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsUnsignedFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsSignedFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsNormFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsUNormFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsSNormFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsIntegerFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsUIntFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsSIntFormat(GraphicsFormat format);


                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsDXTCFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsRGTCFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsBPTCFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsBCFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsPVRTCFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsETCFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static bool IsASTCFormat(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                public static bool IsCrunchFormat(TextureFormat format)
                {
    #if ENABLE_CRUNCH_TEXTURE_COMPRESSION
                    return format == TextureFormat.DXT1Crunched || format == TextureFormat.DXT5Crunched || format == TextureFormat.ETC_RGB4Crunched || format == TextureFormat.ETC2_RGBA8Crunched;
    #else
                    return false;
    #endif
                }

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static UInt32 GetBlockSize(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static UInt32 GetBlockWidth(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                [FreeFunction]
                extern public static UInt32 GetBlockHeight(GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                public static UInt32 ComputeMipmapSize(int width, int height, GraphicsFormat format)
                {
                    return ComputeMipmapSize_Native_2D(width, height, format);
                }

                [FreeFunction]
                extern private static UInt32 ComputeMipmapSize_Native_2D(int width, int height, GraphicsFormat format);

                /// <summary>
                /// There is currently no documentation for this api.
                /// </summary>
                public static UInt32 ComputeMipmapSize(int width, int height, int depth, GraphicsFormat format)
                {
                    return ComputeMipmapSize_Native_3D(width, height, depth, format);
                }

                [FreeFunction]
                extern private static UInt32 ComputeMipmapSize_Native_3D(int width, int height, int depth, GraphicsFormat format);
            }
        } // namespace Rendering
    } // namespace Experimental
} // Namespace
