using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using uei = UnityEngine.Internal;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace UnityEngine
{
    internal enum EnabledOrientation
    {
        kAutorotateToPortrait           = 1,
        kAutorotateToPortraitUpsideDown = 2,
        kAutorotateToLandscapeLeft      = 4,
        kAutorotateToLandscapeRight     = 8,
    };

    /// <summary>
    /// Platform agnostic fullscreen mode. Not all platforms support all modes.
    /// </summary>
    /// <description>
    /// This defines how full screen modes are handled on all platforms.
    /// </description>
    public enum FullScreenMode
    {
        /// <summary>
        /// Exclusive Mode.
        /// </summary>
        /// <description>
        /// In this mode Unity will change the monitor resolution and claim exclusive use of the target display on platforms that support it.
        /// Note that monitor selection is not available from the startup dialog when using exclusive mode. Also, run in background is not supported when the Player is minimized.
        /// </description>
        ExclusiveFullScreen = 0,
        /// <summary>
        /// Fullscreen window.
        /// </summary>
        /// <description>
        /// In this mode, Unity will create a window which is covering the whole screen. It will always run at desktop resolution, and other requested resolutions will be scaled to fit. OS UI will correctly show on top of the fullscreen window (such as IME input windows).
        /// </description>
        FullScreenWindow = 1,
        /// <summary>
        /// Maximized window.
        /// </summary>
        /// <description>
        /// In this mode Unity will create window that is windowed, but maximized, on platforms that support this. Equivalent to FullscreenWindowWithDockAndMenuBar on macOS.
        /// </description>
        MaximizedWindow = 2,
        /// <summary>
        /// Windowed.
        /// </summary>
        /// <description>
        /// In this mode, Unity will create a standard non-fullscreen window, on platforms that support this.
        /// </description>
        Windowed = 3,
    }

    #if UNITY_EDITOR
    public enum TextureCompressionQuality
    {
        // Fast compression
        Fast = 0,
        // Normal compression (default)
        Normal = 50,
        // Best compression
        Best = 100
    }
    #endif

    /// <summary>
    /// Constants for special values of [[Screen.sleepTimeout]].
    /// </summary>
    /// <description>
    /// Use them to specify something other than a fixed amount of seconds before dimming the screen.
    /// </description>
    public sealed partial class SleepTimeout
    {
        /// <summary>
        /// Prevent screen dimming.
        /// </summary>
        public const int NeverSleep = -1;
        /// <summary>
        /// Set the sleep timeout to whatever the user has specified in the system settings.
        /// </summary>
        /// <description>
        /// Useful when restoring back to the state the system was in before running your app.
        /// </description>
        public const int SystemSetting = -2;
    }

    [NativeHeader("Runtime/Graphics/ScreenManager.h")]
    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    [StaticAccessor("GetScreenManager()", StaticAccessorType.Dot)]
    public sealed partial class Screen
    {
        extern public static int   width  {[NativeMethod(Name = "GetWidth",  IsThreadSafe = true)] get; }
        extern public static int   height {[NativeMethod(Name = "GetHeight", IsThreadSafe = true)] get; }
        extern public static float dpi    {[NativeName("GetDPI")] get; }

        extern private static void RequestOrientation(ScreenOrientation orient);
        extern private static ScreenOrientation GetScreenOrientation();

        public static ScreenOrientation orientation
        {
            get { return GetScreenOrientation(); }
            set
            {
            #pragma warning disable 618 // UnityEngine.ScreenOrientation.Unknown is obsolete
                if (value == ScreenOrientation.Unknown)
            #pragma warning restore 649
                {
                    Debug.Log("ScreenOrientation.Unknown is deprecated. Please use ScreenOrientation.AutoRotation");
                    value = ScreenOrientation.AutoRotation;
                }
                RequestOrientation(value);
            }
        }
        [NativeProperty("ScreenTimeout")] extern public static int sleepTimeout { get; set; }

        [NativeName("GetIsOrientationEnabled")] extern private static bool IsOrientationEnabled(EnabledOrientation orient);
        [NativeName("SetIsOrientationEnabled")] extern private static void SetOrientationEnabled(EnabledOrientation orient, bool enabled);

        public static bool autorotateToPortrait
        {
            get { return IsOrientationEnabled(EnabledOrientation.kAutorotateToPortrait); }
            set { SetOrientationEnabled(EnabledOrientation.kAutorotateToPortrait, value); }
        }
        public static bool autorotateToPortraitUpsideDown
        {
            get { return IsOrientationEnabled(EnabledOrientation.kAutorotateToPortraitUpsideDown); }
            set { SetOrientationEnabled(EnabledOrientation.kAutorotateToPortraitUpsideDown, value); }
        }
        public static bool autorotateToLandscapeLeft
        {
            get { return IsOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeLeft); }
            set { SetOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeLeft, value); }
        }
        public static bool autorotateToLandscapeRight
        {
            get { return IsOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeRight); }
            set { SetOrientationEnabled(EnabledOrientation.kAutorotateToLandscapeRight, value); }
        }

        extern public static Resolution currentResolution { get; }
        extern public static bool fullScreen {[NativeName("IsFullscreen")] get; [NativeName("RequestSetFullscreenFromScript")] set; }
        extern public static FullScreenMode fullScreenMode {[NativeName("GetFullscreenMode")] get; [NativeName("RequestSetFullscreenModeFromScript")] set; }

        extern public static Rect safeArea { get; }
        extern public static Rect[] cutouts {[FreeFunction("ScreenScripting::GetCutouts")] get; }

        [NativeName("RequestResolution")]
        extern public static void SetResolution(int width, int height, FullScreenMode fullscreenMode, [uei.DefaultValue("0")] int preferredRefreshRate);

        public static void SetResolution(int width, int height, FullScreenMode fullscreenMode)
        {
            SetResolution(width, height, fullscreenMode, 0);
        }

        public static void SetResolution(int width, int height, bool fullscreen, [uei.DefaultValue("0")] int preferredRefreshRate)
        {
            SetResolution(width, height, fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed, preferredRefreshRate);
        }

        public static void SetResolution(int width, int height, bool fullscreen)
        {
            SetResolution(width, height, fullscreen, 0);
        }

        extern public static Resolution[] resolutions {[FreeFunction("ScreenScripting::GetResolutions")] get; }

        extern public static float brightness { get; set; }
    }
}

namespace UnityEngine
{
    /// <summary>
    /// Color or depth buffer part of a [[RenderTexture]].
    /// </summary>
    /// <description>
    /// A single [[RenderTexture]] object represents both color and depth buffers,
    /// but many complex rendering algorithms require using the same depth buffer
    /// with multiple color buffers or vice versa.
    /// This class represents either a color or a depth buffer part of a RenderTexture.
    /// SA: [[RenderTexture.colorBuffer]], [[RenderTexture.depthBuffer]], [[Graphics.activeColorBuffer]],
    /// [[Graphics.activeDepthBuffer]], [[Graphics.SetRenderTarget]].
    /// </description>
    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    public partial struct RenderBuffer
    {
        [FreeFunction(Name = "RenderBufferScripting::SetLoadAction", HasExplicitThis = true)]
        extern internal void SetLoadAction(RenderBufferLoadAction action);
        [FreeFunction(Name = "RenderBufferScripting::SetStoreAction", HasExplicitThis = true)]
        extern internal void SetStoreAction(RenderBufferStoreAction action);

        [FreeFunction(Name = "RenderBufferScripting::GetLoadAction", HasExplicitThis = true)]
        extern internal RenderBufferLoadAction GetLoadAction();
        [FreeFunction(Name = "RenderBufferScripting::GetStoreAction", HasExplicitThis = true)]
        extern internal RenderBufferStoreAction GetStoreAction();

        /// <summary>
        /// Returns native RenderBuffer. Be warned this is not native Texture, but rather pointer to unity struct that can be used with native unity API. Currently such API exists only on iOS.
        /// </summary>
        [FreeFunction(Name = "RenderBufferScripting::GetNativeRenderBufferPtr", HasExplicitThis = true)]
        extern public IntPtr GetNativeRenderBufferPtr();
    }
}

namespace UnityEngineInternal
{
    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    public enum MemorylessMode
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        Unused,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        Forced,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        Automatic,
    }
    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    [NativeHeader("Runtime/Misc/PlayerSettings.h")]
    public class MemorylessManager
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static MemorylessMode depthMemorylessMode
        {
            get { return GetFramebufferDepthMemorylessMode(); }
            set { SetFramebufferDepthMemorylessMode(value); }
        }
        [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
        [NativeMethod(Name = "GetFramebufferDepthMemorylessMode")]
        extern internal static MemorylessMode GetFramebufferDepthMemorylessMode();
        [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
        [NativeMethod(Name = "SetFramebufferDepthMemorylessMode")]
        extern internal static void SetFramebufferDepthMemorylessMode(MemorylessMode mode);
    }
}

namespace UnityEngine
{
    /// <summary>
    /// Intended usage of the buffer.
    /// </summary>
    /// <description>
    /// Use this enum to convey the intended usage of the buffer to the engine, so that Unity can decide where and how to store the buffer contents.
    /// </description>
    [NativeType("Runtime/GfxDevice/GfxDeviceTypes.h")]
    public enum ComputeBufferMode
    {
        /// <summary>
        /// Static buffer, only initial upload allowed by the CPU
        /// </summary>
        /// <description>
        /// The buffer is not modified by the CPU apart from optionally providing the initial contents of the buffer at buffer creation time. Unity typically stores these buffers in the GPU-only memory (where available). Compute shaders and other GPU operations are allowed to modify the contents of the buffer. 
        /// </description>
        Immutable = 0,
        /// <summary>
        /// Dynamic buffer.
        /// </summary>
        /// <description>
        /// Use this if the buffer is modified often by the CPU (by calls to [[ComputeBuffer.SetData]] or [[ComputeBuffer.BeginBufferWrite]]). Unity typically stores buffers of this type into GPU-visible CPU memory, to enable fast CPU uploads at the cost of GPU performance when it accesses the buffer. If the contents of the buffer are modified while the GPU is reading from it, Unity makes the GPU see the buffer contents as they were at the time the GPU command was issued. This can create extra transient copies of the buffer, which are deleted once the GPU operation is has completed.
        /// </description>
        Dynamic,
        /// <summary>
        /// Legacy mode, do not use.
        /// </summary>
        Circular,
        /// <summary>
        /// Stream Out / Transform Feedback output buffer. Internal use only.
        /// </summary>
        StreamOut,
        /// <summary>
        /// Dynamic, unsynchronized access to the buffer.
        /// </summary>
        /// <description>
        /// Same as [[ComputeBufferMode.Dynamic]] except Unity does not perform any CPU-GPU synchronization; if the user modifies an area of the buffer where the GPU is currently reading from, the results are undefined. For example, this mode together with [[GraphicsFence]] can be used to implement circular buffers.
        /// </description>
        SubUpdates,
    }
}

namespace UnityEngine
{
    /// <summary>
    /// Raw interface to Unity's drawing functions.
    /// </summary>
    /// <description>
    /// This is the high-level shortcut into the optimized mesh drawing functionality of Unity.
    /// </description>
    [NativeHeader("Runtime/Camera/LightProbeProxyVolume.h")]
    [NativeHeader("Runtime/Graphics/ColorGamut.h")]
    [NativeHeader("Runtime/Graphics/CopyTexture.h")]
    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    [NativeHeader("Runtime/Shaders/ComputeShader.h")]
    [NativeHeader("Runtime/Misc/PlayerSettings.h")]
    public partial class Graphics
    {
        [FreeFunction("GraphicsScripting::GetMaxDrawMeshInstanceCount")] extern private static int Internal_GetMaxDrawMeshInstanceCount();
        internal static readonly int kMaxDrawMeshInstanceCount = Internal_GetMaxDrawMeshInstanceCount();

        [FreeFunction] extern private static ColorGamut GetActiveColorGamut();
        /// <summary>
        /// Returns the currently active color gamut.
        /// </summary>
        /// <description>
        /// The active color gamut is guaranteed to not change mid-frame.
        /// </description>
        public static ColorGamut activeColorGamut { get { return GetActiveColorGamut(); } }

        /// <summary>
        /// Graphics Tier classification for current device.
        /// Changing this value affects any subsequently loaded shaders. Initially this value is auto-detected from the hardware in use.
        /// </summary>
        [StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)] extern public static UnityEngine.Rendering.GraphicsTier activeTier { get; set; }

        [StaticAccessor("GetPlayerSettings()", StaticAccessorType.Dot)]
        [NativeMethod(Name = "GetPreserveFramebufferAlpha")]
        extern internal static bool GetPreserveFramebufferAlpha();
        /// <summary>
        /// True when rendering over native UI is enabled in Player Settings (readonly).
        /// </summary>
        /// <description>
        /// SA: [[PlayerSettings.preserveFramebufferAlpha]].
        /// </description>
        public static bool preserveFramebufferAlpha { get { return GetPreserveFramebufferAlpha(); } }

        [FreeFunction("GraphicsScripting::GetActiveColorBuffer")] extern private static RenderBuffer GetActiveColorBuffer();
        [FreeFunction("GraphicsScripting::GetActiveDepthBuffer")] extern private static RenderBuffer GetActiveDepthBuffer();

        [FreeFunction("GraphicsScripting::SetNullRT")] extern private static void Internal_SetNullRT();
        [NativeMethod(Name = "GraphicsScripting::SetRTSimple", IsFreeFunction = true, ThrowsException = true)]
        extern private static void Internal_SetRTSimple(RenderBuffer color, RenderBuffer depth, int mip, CubemapFace face, int depthSlice);
        [NativeMethod(Name = "GraphicsScripting::SetMRTSimple", IsFreeFunction = true, ThrowsException = true)]
        extern private static void Internal_SetMRTSimple([NotNull] RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face, int depthSlice);
        [NativeMethod(Name = "GraphicsScripting::SetMRTFull", IsFreeFunction = true, ThrowsException = true)]
        extern private static void Internal_SetMRTFullSetup(
            [NotNull] RenderBuffer[] color, RenderBuffer depth, int mip, CubemapFace face, int depthSlice,
            [NotNull] RenderBufferLoadAction[] colorLA, [NotNull] RenderBufferStoreAction[] colorSA,
            RenderBufferLoadAction depthLA, RenderBufferStoreAction depthSA
        );

        [NativeMethod(Name = "GraphicsScripting::SetRandomWriteTargetRT", IsFreeFunction = true, ThrowsException = true)]
        extern private static void Internal_SetRandomWriteTargetRT(int index, RenderTexture uav);
        [FreeFunction("GraphicsScripting::SetRandomWriteTargetBuffer")]
        extern private static void Internal_SetRandomWriteTargetBuffer(int index, ComputeBuffer uav, bool preserveCounterValue);

        /// <summary>
        /// Clear random write targets for [[wiki:SL-ShaderCompileTargets|Shader Model 4.5]] level pixel shaders.
        /// </summary>
        /// <description>
        /// This function clears any "random write" targets that were previously set with SetRandomWriteTarget.
        /// </description>
        [StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)] extern public static void ClearRandomWriteTargets();

        [FreeFunction("CopyTexture")] extern private static void CopyTexture_Full(Texture src, Texture dst);
        [FreeFunction("CopyTexture")] extern private static void CopyTexture_Slice_AllMips(Texture src, int srcElement, Texture dst, int dstElement);
        [FreeFunction("CopyTexture")] extern private static void CopyTexture_Slice(Texture src, int srcElement, int srcMip, Texture dst, int dstElement, int dstMip);
        [FreeFunction("CopyTexture")] extern private static void CopyTexture_Region(Texture src, int srcElement, int srcMip, int srcX, int srcY, int srcWidth, int srcHeight, Texture dst, int dstElement, int dstMip, int dstX, int dstY);
        [FreeFunction("ConvertTexture")] extern private static bool ConvertTexture_Full(Texture src, Texture dst);
        [FreeFunction("ConvertTexture")] extern private static bool ConvertTexture_Slice(Texture src, int srcElement, Texture dst, int dstElement);

        [FreeFunction("GraphicsScripting::DrawMeshNow")] extern private static void Internal_DrawMeshNow1(Mesh mesh, int subsetIndex, Vector3 position, Quaternion rotation);
        [FreeFunction("GraphicsScripting::DrawMeshNow")] extern private static void Internal_DrawMeshNow2(Mesh mesh, int subsetIndex, Matrix4x4 matrix);

        [FreeFunction("GraphicsScripting::DrawTexture")][VisibleToOtherModules("UnityEngine.IMGUIModule")]
        extern internal static void Internal_DrawTexture(ref Internal_DrawTextureArguments args);

        [FreeFunction("GraphicsScripting::DrawMesh")]
        extern private static void Internal_DrawMesh(Mesh mesh, int submeshIndex, Matrix4x4 matrix, Material material, int layer, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume);

        [FreeFunction("GraphicsScripting::DrawMeshInstanced")]
        extern private static void Internal_DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume);

        [FreeFunction("GraphicsScripting::DrawMeshInstancedIndirect")]
        extern private static void Internal_DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage, LightProbeProxyVolume lightProbeProxyVolume);

        [FreeFunction("GraphicsScripting::DrawProceduralNow")]
        extern private static void Internal_DrawProceduralNow(MeshTopology topology, int vertexCount, int instanceCount);

        [FreeFunction("GraphicsScripting::DrawProceduralIndexedNow")]
        extern private static void Internal_DrawProceduralIndexedNow(MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount);

        [FreeFunction("GraphicsScripting::DrawProceduralIndirectNow")]
        extern private static void Internal_DrawProceduralIndirectNow(MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset);

        [FreeFunction("GraphicsScripting::DrawProceduralIndexedIndirectNow")]
        extern private static void Internal_DrawProceduralIndexedIndirectNow(MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset);

        [FreeFunction("GraphicsScripting::DrawProcedural")]
        extern private static void Internal_DrawProcedural(Material material, Bounds bounds, MeshTopology topology, int vertexCount, int instanceCount, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

        [FreeFunction("GraphicsScripting::DrawProceduralIndexed")]
        extern private static void Internal_DrawProceduralIndexed(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, int indexCount, int instanceCount, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

        [FreeFunction("GraphicsScripting::DrawProceduralIndirect")]
        extern private static void Internal_DrawProceduralIndirect(Material material, Bounds bounds, MeshTopology topology, ComputeBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

        [FreeFunction("GraphicsScripting::DrawProceduralIndexedIndirect")]
        extern private static void Internal_DrawProceduralIndexedIndirect(Material material, Bounds bounds, MeshTopology topology, GraphicsBuffer indexBuffer, ComputeBuffer bufferWithArgs, int argsOffset, Camera camera, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer);

        [FreeFunction("GraphicsScripting::BlitMaterial")]
        extern private static void Internal_BlitMaterial5(Texture source, RenderTexture dest, [NotNull] Material mat, int pass, bool setRT);

        [FreeFunction("GraphicsScripting::BlitMaterial")]
        extern private static void Internal_BlitMaterial6(Texture source, RenderTexture dest, [NotNull] Material mat, int pass, bool setRT, int destDepthSlice);

        [FreeFunction("GraphicsScripting::BlitMultitap")]
        extern private static void Internal_BlitMultiTap4(Texture source, RenderTexture dest, [NotNull] Material mat, [NotNull] Vector2[] offsets);

        [FreeFunction("GraphicsScripting::BlitMultitap")]
        extern private static void Internal_BlitMultiTap5(Texture source, RenderTexture dest, [NotNull] Material mat, [NotNull] Vector2[] offsets, int destDepthSlice);

        [FreeFunction("GraphicsScripting::Blit")]
        extern private static void Blit2(Texture source, RenderTexture dest);

        [FreeFunction("GraphicsScripting::Blit")]
        extern private static void Blit3(Texture source, RenderTexture dest, int sourceDepthSlice, int destDepthSlice);

        [FreeFunction("GraphicsScripting::Blit")]
        extern private static void Blit4(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset);

        [FreeFunction("GraphicsScripting::Blit")]
        extern private static void Blit5(Texture source, RenderTexture dest, Vector2 scale, Vector2 offset, int sourceDepthSlice, int destDepthSlice);

        [NativeMethod(Name = "GraphicsScripting::CreateGPUFence", IsFreeFunction = true, ThrowsException = true)]
        extern private static IntPtr CreateGPUFenceImpl(GraphicsFenceType fenceType, SynchronisationStageFlags stage);

        [NativeMethod(Name = "GraphicsScripting::WaitOnGPUFence", IsFreeFunction = true, ThrowsException = true)]
        extern private static void WaitOnGPUFenceImpl(IntPtr fencePtr, SynchronisationStageFlags stage);

        /// <summary>
        /// Execute a command buffer.
        /// </summary>
        /// <param name="buffer">
        /// The buffer to execute.
        /// </param>
        /// <description>
        /// All commands in the buffer will be executed immediately.
        /// SA: [[Rendering.CommandBuffer]].
        /// </description>
        [NativeMethod(Name = "GraphicsScripting::ExecuteCommandBuffer", IsFreeFunction = true, ThrowsException = true)]
        extern public static void ExecuteCommandBuffer([NotNull] CommandBuffer buffer);

        /// <summary>
        /// Executes a command buffer on an async compute queue with the queue selected based on the [[ComputeQueueType]] parameter passed.
        /// It is required that all of the commands within the command buffer be of a type suitable for execution on the async compute queues. If the buffer contains any commands that are not appropriate then an error will be logged and displayed in the editor window.  Specifically the following commands are permitted in a [[CommandBuffer]] intended for async execution:
        /// [[CommandBuffer.BeginSample]]
        /// [[CommandBuffer.CopyCounterValue]]
        /// [[CommandBuffer.CopyTexture]]
        /// [[CommandBuffer.CreateGPUFence]]
        /// [[CommandBuffer.DisableShaderKeyword]]
        /// [[CommandBuffer.DispatchCompute]]
        /// [[CommandBuffer.EnableShaderKeyword]]
        /// [[CommandBuffer.EndSample]]
        /// [[CommandBuffer.GetTemporaryRT]]
        /// [[CommandBuffer.GetTemporaryRTArray]]
        /// [[CommandBuffer.IssuePluginEvent]]
        /// [[CommandBuffer.ReleaseTemporaryRT]]
        /// [[CommandBuffer.SetComputeBufferParam]]
        /// [[CommandBuffer.SetComputeFloatParam]]
        /// [[CommandBuffer.SetComputeFloatParams]]
        /// [[CommandBuffer.SetComputeIntParam]]
        /// [[CommandBuffer.SetComputeIntParams]]
        /// [[CommandBuffer.SetComputeMatrixArrayParam]]
        /// [[CommandBuffer.SetComputeMatrixParam]]
        /// [[CommandBuffer.SetComputeTextureParam]]
        /// [[CommandBuffer.SetComputeVectorParam]]
        /// [[CommandBuffer.SetComputeVectorArrayParam]]
        /// [[CommandBuffer.SetGlobalBuffer]]
        /// [[CommandBuffer.SetGlobalColor]]
        /// [[CommandBuffer.SetGlobalFloat]]
        /// [[CommandBuffer.SetGlobalFloatArray]]
        /// [[CommandBuffer.SetGlobalInt]]
        /// [[CommandBuffer.SetGlobalMatrix]]
        /// [[CommandBuffer.SetGlobalMatrixArray]]
        /// [[CommandBuffer.SetGlobalTexture]]
        /// [[CommandBuffer.SetGlobalVector]]
        /// [[CommandBuffer.SetGlobalVectorArray]]
        /// [[CommandBuffer.WaitOnGPUFence]]
        /// All of the commands within the buffer are guaranteed to be executed on the same queue. If the target platform does not support async compute queues then the work is dispatched on the graphics queue.
        /// </summary>
        /// <param name="buffer">
        /// The [[CommandBuffer]] to be executed.
        /// </param>
        /// <param name="queueType">
        /// Describes the desired async compute queue the suuplied [[CommandBuffer]] should be executed on.
        /// </param>
        /// <description>
        /// SA: [[SystemInfo.supportsAsyncCompute]] , [[GPUFence]], [[CommandBuffer]].
        /// </description>
        [NativeMethod(Name = "GraphicsScripting::ExecuteCommandBufferAsync", IsFreeFunction = true, ThrowsException = true)]
        extern public  static void ExecuteCommandBufferAsync([NotNull] CommandBuffer buffer, ComputeQueueType queueType);
    }
}

namespace UnityEngine
{
    /// <summary>
    /// Low-level graphics library.
    /// </summary>
    /// <description>
    /// Use this class to manipulate active transformation matrices,
    /// issue rendering commands similar to OpenGL's immediate mode and do other low-level
    /// graphics tasks. Note that in almost all cases using [[Graphics.DrawMesh]] or [[Rendering.CommandBuffer]]
    /// is more efficient than using immediate mode drawing.
    /// GL immediate drawing functions use whatever is the "current material" set up right now (see [[Material.SetPass]]).
    /// The material controls how the rendering is done (blending, textures, etc.), so unless you explicitly
    /// set it to something before using GL draw functions, the material can happen to be anything.
    /// Also, if you call any other drawing commands from inside GL drawing code, they can set
    /// material to something else, so make sure it's under control as well.
    /// GL drawing commands execute immediately. That means if you call them in Update(), they will be executed
    /// before the camera is rendered (and the camera will most likely clear the screen, making the GL drawing
    /// not visible).
    /// The usual place to call GL drawing is most often in Camera.OnPostRender() from a script attached to a
    /// camera, or inside an image effect function (Camera.OnRenderImage).
    /// </description>
    /// <description>
    /// __Note:__ This class is almost always used when you need to draw a couple of lines or triangles, and don't want to deal with meshes.
    /// If you want to avoid surprises the usage pattern is this:
    /// </description>
    /// <description>
    /// Where at the "// Draw your stuff" you should do SetPass() on some material previously declared, which will be used for drawing.
    /// If you dont call SetPass, then you'll get basically a random material (whatever was used before) which is not good. So do it.
    /// </description>
    public sealed partial class GL
    {
        /// <summary>
        /// Mode for Begin: draw triangles.
        /// </summary>
        /// <description>
        /// Draws triangles using each set of 3 vertices passed. If you pass 3 vertices, one triangle is drawn, where each vertex becomes one corner of the triangle. If you pass 6 vertices, 2 triangles will be drawn.
        /// To set up the screen for drawing in 2D, use [[GL.LoadOrtho]] or [[GL.LoadPixelMatrix]].
        /// To set up the screen for drawing in 3D, use [[GL.LoadIdentity]] followed by [[GL.MultMatrix]] with the desired transformation matrix.
        /// SA: [[GL.Begin]], [[GL.End]].
        /// </description>
        public const int TRIANGLES      = 0x0004;
        /// <summary>
        /// Mode for Begin: draw triangle strip.
        /// </summary>
        /// <description>
        /// Draws triangles between each vertex passed, from the beginning to the end. If you pass five vertices, A, B, C, D and E, three triangles are drawn. The first triangle is drawn between the first 3 vertices. All subsequent triangles use the previous 2 vertices, plus the next additional vertex. In this example, the three drawn triangles will be A, B, C, followed by B, C, D, and finally C, D, E.
        /// To set up the screen for drawing in 2D, use [[GL.LoadOrtho]] or [[GL.LoadPixelMatrix]].
        /// To set up the screen for drawing in 3D, use [[GL.LoadIdentity]] followed by [[GL.MultMatrix]] with the desired transformation matrix.
        /// SA: [[GL.Begin]], [[GL.End]].
        /// </description>
        public const int TRIANGLE_STRIP = 0x0005;
        /// <summary>
        /// Mode for Begin: draw quads.
        /// </summary>
        /// <description>
        /// Draws quads using each set of 4 vertices passed. If you pass 4 vertices, one quad is drawn, where each vertex becomes one corner of the quad. If you pass 8 vertices, 2 quads will be drawn.
        /// To set up the screen for drawing in 2D, use [[GL.LoadOrtho]] or [[GL.LoadPixelMatrix]].
        /// To set up the screen for drawing in 3D, use [[GL.LoadIdentity]] followed by [[GL.MultMatrix]] with the desired transformation matrix.
        /// SA: [[GL.Begin]], [[GL.End]].
        /// </description>
        public const int QUADS          = 0x0007;
        /// <summary>
        /// Mode for Begin: draw lines.
        /// </summary>
        /// <description>
        /// Draws lines between each pair of vertices passed. If you pass four vertices, A, B, C and D, two lines are drawn: one between A and B, and one between C and D.
        /// To set up the screen for drawing in 2D, use [[GL.LoadOrtho]] or [[GL.LoadPixelMatrix]].
        /// To set up the screen for drawing in 3D, use [[GL.LoadIdentity]] followed by [[GL.MultMatrix]] with the desired transformation matrix.
        /// SA: [[GL.Begin]], [[GL.End]].
        /// </description>
        public const int LINES          = 0x0001;
        /// <summary>
        /// Mode for Begin: draw line strip.
        /// </summary>
        /// <description>
        /// Draws lines between each vertex passed, from the beginning to the end. If you pass three vertices, A, B and C, two lines are drawn: one between A and B, and one between B and C.
        /// To set up the screen for drawing in 2D, use [[GL.LoadOrtho]] or [[GL.LoadPixelMatrix]].
        /// To set up the screen for drawing in 3D, use [[GL.LoadIdentity]] followed by [[GL.MultMatrix]] with the desired transformation matrix.
        /// SA: [[GL.Begin]], [[GL.End]].
        /// </description>
        public const int LINE_STRIP     = 0x0002;
    }


    [NativeHeader("Runtime/GfxDevice/GfxDevice.h")]
    [StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
    public sealed partial class GL
    {
        /// <summary>
        /// Submit a vertex.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glVertex3f(x,y,z)@@; on other graphics APIs the same
        /// functionality is emulated.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        [NativeName("ImmediateVertex")] extern public static void Vertex3(float x, float y, float z);
        /// <summary>
        /// Submit a vertex.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glVertex3f(v.x,v.y,v.z)@@; on other graphics APIs the same
        /// functionality is emulated.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        public static void Vertex(Vector3 v) { Vertex3(v.x, v.y, v.z); }

        /// <summary>
        /// Sets current texture coordinate (x,y,z) for all texture units.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glMultiTexCoord@@ for all texture units or @@glTexCoord@@
        /// when no multi-texturing is available. On other graphics APIs the same
        /// functionality is emulated.
        /// The Z component is used only when:\\
        /// __1.__ You access a cubemap (which you access with a vector coordinate, hence x,y & z).\\
        /// __2.__ You do "projective texturing", where the X & Y coordinates are divided by Z to get the final coordinate. This would be mostly useful for water reflections and similar things.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        [NativeName("ImmediateTexCoordAll")] extern public static void TexCoord3(float x, float y, float z);
        /// <summary>
        /// Sets current texture coordinate (v.x,v.y,v.z) for all texture units.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glMultiTexCoord@@ for all texture units or @@glTexCoord@@
        /// when no multi-texturing is available. On other graphics APIs the same
        /// functionality is emulated.
        /// The Z component is used only when:\\
        /// __1.__ You access a cubemap (which you access with a vector coordinate, hence x,y & z).\\
        /// __2.__ You do "projective texturing", where the X & Y coordinates are divided by Z to get the final coordinate. This would be mostly useful for water reflections and similar things.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        public static void TexCoord(Vector3 v)          { TexCoord3(v.x, v.y, v.z); }
        /// <summary>
        /// Sets current texture coordinate (x,y) for all texture units.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glMultiTexCoord@@ for all texture units or @@glTexCoord@@
        /// when no multi-texturing is available. On other graphics APIs the same
        /// functionality is emulated.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        public static void TexCoord2(float x, float y)  { TexCoord3(x, y, 0.0f); }

        /// <summary>
        /// Sets current texture coordinate (x,y,z) to the actual texture /unit/.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glMultiTexCoord@@ for the given texture unit
        /// if multi-texturing is available. On other graphics APIs the same
        /// functionality is emulated.
        /// The Z component is used only when:\\
        /// __1.__ You access a cubemap (which you access with a vector coordinate, hence x,y & z).\\
        /// __2.__ You do "projective texturing", where the X & Y coordinates are divided by Z to get the final coordinate. This would be mostly useful for water reflections and similar things.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        [NativeName("ImmediateTexCoord")] extern public static void MultiTexCoord3(int unit, float x, float y, float z);
        /// <summary>
        /// Sets current texture coordinate (v.x,v.y,v.z) to the actual texture /unit/.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glMultiTexCoord@@ for the given texture unit
        /// if multi-texturing is available. On other graphics APIs the same
        /// functionality is emulated.
        /// The Z component is used only when:\\
        /// __1.__ You access a cubemap (which you access with a vector coordinate, hence x,y & z).\\
        /// __2.__ You do "projective texturing", where the X & Y coordinates are divided by Z to get the final coordinate. This would be mostly useful for water reflections and similar things.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        public static void MultiTexCoord(int unit, Vector3 v)           { MultiTexCoord3(unit, v.x, v.y, v.z); }
        /// <summary>
        /// Sets current texture coordinate (x,y) for the actual texture /unit/.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glMultiTexCoord@@ for the given texture unit
        /// if multi-texturing is available. On other graphics APIs the same
        /// functionality is emulated.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        public static void MultiTexCoord2(int unit, float x, float y)   { MultiTexCoord3(unit, x, y, 0.0f); }

        [NativeName("ImmediateColor")] extern private static void ImmediateColor(float r, float g, float b, float a);
        /// <summary>
        /// Sets current vertex color.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glColor4f(c.r,c.g,c.b,c.a)@@; on other graphics APIs the same
        /// functionality is emulated.
        /// In order for per-vertex colors to work reliably across different hardware, you have to use
        /// a shader that binds in the color channel. See [[wiki:SL-BindChannels|BindChannels]] documentation.
        /// This function can only be called between [[GL.Begin]] and [[GL.End]] functions.
        /// </description>
        public static void Color(Color c) { ImmediateColor(c.r, c.g, c.b, c.a); }

        /// <summary>
        /// Should rendering be done in wireframe?
        /// </summary>
        /// <description>
        /// Turning on wireframe mode will affect all objects rendered after the call, until you turn
        /// wireframe back off. In the Unity editor, wireframe mode is always turned off
        /// before repainting any window.
        /// Note that some platforms, for example mobile (OpenGL ES) does not support
        /// wireframe rendering.
        /// </description>
        extern public static bool wireframe     { get; set; }
        /// <summary>
        /// Controls whether Linear-to-sRGB color conversion is performed while rendering.
        /// </summary>
        /// <description>
        /// This property is only relevant when [[wiki:LinearLighting|Linear Color Space]] rendering is used.
        /// Typically when linear color space is used, non-HDR render textures are treated as sRGB data
        /// (i.e. "regular colors"), and fragment shaders outputs are treated as linear color values. So by default
        /// the fragment shader color value is converted into sRGB.
        /// However, if you know your fragment shader already outputs sRGB color value for some reason and want to
        /// temporarily turn off Linear-to-sRGB write color conversion, you can use this property to achieve that.
        /// Note that the ability to turn off sRGB writes is not supported on all platforms (typically mobile "tile based" GPUs
        /// can not do it), so this is considered a "feature of last resort". Usually it is better to create [[RenderTexture|RenderTextures]]
        /// with appropriate color space flag (linear vs sRGB) and not switch the conversions in the middle of rendering into it.
        /// SA: [[wiki:LinearLighting|Linear Color Space]], [[RenderTexture.sRGB]], [[RenderTextureReadWrite]], [[PlayerSettings.colorSpace]].
        /// </description>
        extern public static bool sRGBWrite     { get; set; }
        /// <summary>
        /// Select whether to invert the backface culling (true) or not (false).
        /// </summary>
        /// <description>
        /// This flag can "flip" the culling mode of all rendered objects. Major use case: rendering reflections for mirrors, water etc. Since virtual camera for rendering the reflection is mirrored, the culling order has to be inverted. You can see how the Water script in Effects standard package does that.
        /// </description>
        [NativeProperty("UserBackfaceMode")] extern public static bool invertCulling { get;  set; }

        /// <summary>
        /// Sends queued-up commands in the driver's command buffer to the GPU.
        /// </summary>
        /// <description>
        /// When Direct3D 11 is the active graphics API, this call maps to ID3D11DeviceContext::Flush.
        /// When Direct3D 12 is the active graphics API, pending command lists are executed.
        /// When OpenGL is the active graphics API, this call maps to glFlush.
        /// </description>
        extern public static void Flush();
        /// <summary>
        /// Resolves the render target for subsequent operations sampling from it.
        /// </summary>
        /// <description>
        /// At the moment the advanced OpenGL blend operations are the only case requiring this barrier.
        /// SA: [[Rendering.BlendOp]].
        /// </description>
        extern public static void RenderTargetBarrier();

        extern private static Matrix4x4 GetWorldViewMatrix();
        extern private static void SetViewMatrix(Matrix4x4 m);
        /// <summary>
        /// The current modelview matrix.
        /// </summary>
        /// <description>
        /// Assigning to this variable is equivalent to @@glLoadMatrix(mat)@@ in OpenGL; in other
        /// graphics APIs the corresponding functionality is emulated.
        /// Changing modelview matrix overrides current camera's view parameters, so most
        /// often you want to save and restore matrix using [[GL.PushMatrix]] and [[GL.PopMatrix]].
        /// Reading this variable returns the current modelview matrix.
        /// </description>
        static public Matrix4x4 modelview { get { return GetWorldViewMatrix(); } set { SetViewMatrix(value); } }

        /// <summary>
        /// Sets the current modelview matrix to the one specified.
        /// </summary>
        /// <description>
        /// This method is equivalent to @@glLoadMatrix(mat)@@ in OpenGL. In other graphics APIs, the corresponding functionality is emulated.
        /// Because changing the modelview matrix overrides the view parameters of the current camera, it is recommended that you save and restore the matrix using [[GL.PushMatrix]] and [[GL.PopMatrix]].
        /// </description>
        [NativeName("SetWorldMatrix")] extern public static void MultMatrix(Matrix4x4 m);

        /// <summary>
        /// Send a user-defined event to a native code plugin.
        /// </summary>
        /// <param name="eventID">
        /// User defined id to send to the callback.
        /// </param>
        /// <description>
        /// Rendering in Unity can be multithreaded if the platform and number
        /// of available CPUs will allow for it. When multithreaded rendering is
        /// used, the rendering API commands happen on a thread which is
        /// completely separate from the one that runs the scripts. Consequently,
        /// it is not possible for your plugin to start rendering immediately,
        /// since it might interfere with what the render thread is doing at the time.
        /// In order to do any rendering from the plugin, you should call
        /// GL.IssuePluginEvent from your script, which will cause your native
        /// plugin to be called from the render thread. For example, if you
        /// call GL.IssuePluginEvent from the camera's OnPostRender function, you'll
        /// get a plugin callback immediately after the camera has finished rendering.
        /// Callback must be a native function of "void UNITY_INTERFACE_API UnityRenderingEvent(int eventId)" signature.
        /// See [[wiki:NativePluginInterface|Native Plugin Interface]] for more details
        /// and an example.
        /// SA: [[SystemInfo.graphicsMultiThreaded]].
        /// </description>
        [Obsolete("IssuePluginEvent(eventID) is deprecated. Use IssuePluginEvent(callback, eventID) instead.", false)]
        [NativeName("InsertCustomMarker")] extern public static void IssuePluginEvent(int eventID);

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [Obsolete("SetRevertBackfacing(revertBackFaces) is deprecated. Use invertCulling property instead.", false)]
        [NativeName("SetUserBackfaceMode")] extern public static void SetRevertBackfacing(bool revertBackFaces);
    }

    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    [NativeHeader("Runtime/Camera/Camera.h")]
    [NativeHeader("Runtime/Camera/CameraUtil.h")]
    public sealed partial class GL
    {
        /// <summary>
        /// Saves both projection and modelview matrices to the matrix stack.
        /// </summary>
        /// <description>
        /// Changing modelview or projection matrices overrides current camera's parameters.
        /// These matrices can be saved and restored using [[GL.PushMatrix]] and [[GL.PopMatrix]].
        /// SA: PopMatrix function.
        /// </description>
        [FreeFunction("GLPushMatrixScript")]            extern public static void PushMatrix();
        /// <summary>
        /// Restores both projection and modelview matrices off the top of the matrix stack.
        /// </summary>
        /// <description>
        /// Changing modelview or projection matrices overrides current camera's parameters.
        /// These matrices can be saved and restored using [[GL.PushMatrix]] and [[GL.PopMatrix]].
        /// SA: PushMatrix function.
        /// </description>
        [FreeFunction("GLPopMatrixScript")]             extern public static void PopMatrix();
        /// <summary>
        /// Load the identity matrix to the current modelview matrix.
        /// </summary>
        /// <description>
        /// This function overrides current camera's view parameters, so most often you want
        /// to save and restore matrix using [[GL.PushMatrix]] and [[GL.PopMatrix]].
        /// SA: [[GL.MultMatrix]].
        /// </description>
        [FreeFunction("GLLoadIdentityScript")]          extern public static void LoadIdentity();
        /// <summary>
        /// Helper function to set up an ortho perspective transform.
        /// </summary>
        /// <description>
        /// After calling LoadOrtho, the viewing frustum goes from (0,0,-1) to (1,1,100).\\
        /// LoadOrtho can be used for drawing primitives in 2D.
        /// </description>
        /// <description>
        /// SA: [[GL.LoadProjectionMatrix]].
        /// </description>
        [FreeFunction("GLLoadOrthoScript")]             extern public static void LoadOrtho();
        /// <summary>
        /// Setup a matrix for pixel-correct rendering.
        /// </summary>
        /// <description>
        /// This sets up modelview and projection matrices so that X, Y coordinates map
        /// directly to pixels. The (0,0) is at the bottom left corner of current camera's
        /// viewport. The Z coordinate goes from -1 to +1.
        /// This function overrides current camera's parameters, so most often you want
        /// to save and restore matrices using [[GL.PushMatrix]] and [[GL.PopMatrix]].
        /// </description>
        [FreeFunction("GLLoadPixelMatrixScript")]       extern public static void LoadPixelMatrix();
        /// <summary>
        /// Load an arbitrary matrix to the current projection matrix.
        /// </summary>
        /// <description>
        /// This function overrides current camera's projection parameters, so most often you want
        /// to save and restore projection matrix using [[GL.PushMatrix]] and [[GL.PopMatrix]].
        /// </description>
        /// <description>
        /// SA: [[GL.LoadOrtho]].
        /// </description>
        [FreeFunction("GLLoadProjectionMatrixScript")]  extern public static void LoadProjectionMatrix(Matrix4x4 mat);
        /// <summary>
        /// Invalidate the internally cached render state.
        /// </summary>
        /// <description>
        /// This invalidates any cached render state tied to the active graphics API.
        /// If for example a (native) plugin alters the render state settings
        /// then Unity's rendering engine must be made aware of that.
        /// SA: [[GL.IssuePluginEvent]].
        /// </description>
        [FreeFunction("GLInvalidateState")]             extern public static void InvalidateState();
        /// <summary>
        /// Compute GPU projection matrix from camera's projection matrix.
        /// </summary>
        /// <param name="proj">
        /// Source projection matrix.
        /// </param>
        /// <param name="renderIntoTexture">
        /// Will this projection be used for rendering into a RenderTexture?
        /// </param>
        /// <returns>
        /// Adjusted projection matrix for the current graphics API.
        /// </returns>
        /// <description>
        /// In Unity, projection matrices follow OpenGL convention. However on some platforms they
        /// have to be transformed a bit to match the native API requirements. Use this function
        /// to calculate how the final projection matrix will be like. The value will match what
        /// comes as @@UNITY_MATRIX_P@@ matrix in a shader.
        /// The /renderIntoTexture/ value should be set to true if you intend to render into a
        /// [[RenderTexture]] with this projection matrix. On some platforms it affects how
        /// the final matrix will look like.
        /// SA: [[Camera.projectionMatrix]], [[wiki:SL-PlatformDifferences|Platform differences]], [[wiki:SL-UnityShaderVariables|Built-in shader variables]].
        /// </description>
        [FreeFunction("GLGetGPUProjectionMatrix")]      extern public static Matrix4x4 GetGPUProjectionMatrix(Matrix4x4 proj, bool renderIntoTexture);

        [FreeFunction] extern private static void GLLoadPixelMatrixScript(float left, float right, float bottom, float top);
        /// <summary>
        /// Setup a matrix for pixel-correct rendering.
        /// </summary>
        /// <description>
        /// This sets up modelview and projection matrices so that X, Y coordinates map
        /// directly to pixels. The (left,bottom is at the bottom left corner of current camera's
        /// viewport; and (top,right) is at the top right corner of current camera's viewport.
        /// The Z coordinate goes from -1 to +1.
        /// This function overrides current camera's parameters, so most often you want
        /// to save and restore matrices using [[GL.PushMatrix]] and [[GL.PopMatrix]].
        /// </description>
        public static void LoadPixelMatrix(float left, float right, float bottom, float top)
        {
            GLLoadPixelMatrixScript(left, right, bottom, top);
        }

        [FreeFunction] extern private static void GLIssuePluginEvent(IntPtr callback, int eventID);
        /// <summary>
        /// Send a user-defined event to a native code plugin.
        /// </summary>
        /// <param name="callback">
        /// Native code callback to queue for Unity's renderer to invoke.
        /// </param>
        /// <param name="eventID">
        /// User defined id to send to the callback.
        /// </param>
        /// <description>
        /// Rendering in Unity can be multithreaded if the platform and number
        /// of available CPUs will allow for it. When multithreaded rendering is
        /// used, the rendering API commands happen on a thread which is
        /// completely separate from the one that runs the scripts. Consequently,
        /// it is not possible for your plugin to start rendering immediately,
        /// since it might interfere with what the render thread is doing at the time.
        /// In order to do any rendering from the plugin, you should call
        /// GL.IssuePluginEvent from your script, which will cause your native
        /// plugin to be called from the render thread. For example, if you
        /// call GL.IssuePluginEvent from the camera's OnPostRender function, you'll
        /// get a plugin callback immediately after the camera has finished rendering.
        /// Callback must be a native function of "void UNITY_INTERFACE_API UnityRenderingEvent(int eventId)" signature.
        /// See [[wiki:NativePluginInterface|Native Plugin Interface]] for more details
        /// and an example.
        /// SA: [[SystemInfo.graphicsMultiThreaded]].
        /// </description>
        public static void IssuePluginEvent(IntPtr callback, int eventID)
        {
            if (callback == IntPtr.Zero)
                throw new ArgumentException("Null callback specified.", "callback");
            GLIssuePluginEvent(callback, eventID);
        }

        /// <summary>
        /// Begin drawing 3D primitives.
        /// </summary>
        /// <param name="mode">
        /// Primitives to draw: can be TRIANGLES, TRIANGLE_STRIP, QUADS or LINES.
        /// </param>
        /// <description>
        /// In OpenGL this matches @@glBegin@@; on other graphics APIs the same
        /// functionality is emulated. Between GL.Begin and [[GL.End]] it is valid to
        /// call [[GL.Vertex]], [[GL.Color]], [[GL.TexCoord]] and other immediate mode drawing
        /// functions.
        /// You should be careful about culling when drawing primitives yourself. The culling rules
        /// may be different depending on which graphics API the game is running. In most cases the
        /// safest way is to use @@Cull Off@@ command in the shader.
        /// SA: [[GL.End]].
        /// </description>
        [FreeFunction("GLBegin", ThrowsException = true)] extern public static void Begin(int mode);
        /// <summary>
        /// End drawing 3D primitives.
        /// </summary>
        /// <description>
        /// In OpenGL this matches @@glEnd@@; on other graphics APIs the same
        /// functionality is emulated.
        /// SA: [[GL.Begin]].
        /// </description>
        [FreeFunction("GLEnd")]                           extern public static void End();

        [FreeFunction] extern private static void GLClear(bool clearDepth, bool clearColor, Color backgroundColor, float depth);
        /// <summary>
        /// Clear the current render buffer.
        /// </summary>
        /// <param name="clearDepth">
        /// Should the depth buffer be cleared?
        /// </param>
        /// <param name="clearColor">
        /// Should the color buffer be cleared?
        /// </param>
        /// <param name="backgroundColor">
        /// The color to clear with, used only if /clearColor/ is /true/.
        /// </param>
        /// <param name="depth">
        /// The depth to clear Z buffer with, used only if /clearDepth/ is /true/.
        /// </param>
        /// <description>
        /// This clears the screen or the active [[RenderTexture]] you are drawing into.
        /// In most other situations, some camera is drawing something somewhere, and probably
        /// is clearing already with the background color of the skybox.
        /// SA: [[GL.ClearWithSkybox]].
        /// </description>
        static public void Clear(bool clearDepth, bool clearColor, Color backgroundColor, [uei.DefaultValue("1.0f")] float depth)
        {
            GLClear(clearDepth, clearColor, backgroundColor, depth);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        static public void Clear(bool clearDepth, bool clearColor, Color backgroundColor)
        {
            GLClear(clearDepth, clearColor, backgroundColor, 1.0f);
        }

        /// <summary>
        /// Set the rendering viewport.
        /// </summary>
        /// <description>
        /// All rendering is constrained to be inside the passed /pixelRect/.
        /// If the Viewport is modified, all the rendered content inside of it gets stretched.
        /// </description>
        [FreeFunction("SetGLViewport")] extern public static void Viewport(Rect pixelRect);
        /// <summary>
        /// Clear the current render buffer with camera's skybox.
        /// </summary>
        /// <param name="clearDepth">
        /// Should the depth buffer be cleared?
        /// </param>
        /// <param name="camera">
        /// Camera to get projection parameters and skybox from.
        /// </param>
        /// <description>
        /// This draws skybox into the screen or the active [[RenderTexture]].
        /// If the passed camera does not have custom [[Skybox]] component,
        /// the global skybox from [[RenderSettings]] will be used.
        /// SA: [[GL.Clear]].
        /// </description>
        [FreeFunction("ClearWithSkybox")] extern public static void ClearWithSkybox(bool clearDepth, Camera camera);
    }
}

namespace UnityEngine
{
    // Scales render textures to support dynamic resolution.
    /// <summary>
    /// Scales render textures to support dynamic resolution if the target platform/graphics API supports it.
    /// </summary>
    /// <description>
    /// The ScalableBufferManager handles the scaling of any render textures that you have marked to be DynamicallyScalable, when ResizeBuffers is called. All render textures marked as DynamicallyScalable are scaled by a width and height scale factor, the reason the scale is controlled through a scale factor and not with a specific width and height value is because different render textures will be different sizes but will want to be scaled by a common factor.
    /// </description>
    [NativeHeader("Runtime/GfxDevice/ScalableBufferManager.h")]
    [StaticAccessor("ScalableBufferManager::GetInstance()", StaticAccessorType.Dot)]
    static public class ScalableBufferManager
    {
        /// <summary>
        /// Width scale factor to control dynamic resolution.
        /// </summary>
        /// <description>
        /// This is a scale factor between epsilon and 1.0 that is applied to the width of all render textures that you have marked as DynamicallyScalable.
        /// </description>
        extern static public float widthScaleFactor { get; }
        /// <summary>
        /// Height scale factor to control dynamic resolution.
        /// </summary>
        /// <description>
        /// This is a scale factor between epsilon and 1.0 that is applied to the height of all render textures that you have marked as DynamicallyScalable.
        /// </description>
        extern static public float heightScaleFactor { get; }

        /// <summary>
        /// Function to resize all buffers marked as DynamicallyScalable.
        /// </summary>
        /// <param name="widthScale">
        /// New scale factor for the width the ScalableBufferManager will use to resize all render textures the user marked as DynamicallyScalable, has to be some value greater than 0.0 and less than or equal to 1.0.
        /// </param>
        /// <param name="heightScale">
        /// New scale factor for the height the ScalableBufferManager will use to resize all render textures the user marked as DynamicallyScalable, has to be some value greater than 0.0 and less than or equal to 1.0.
        /// </param>
        /// <description>
        /// Takes in new width and height scale and stores and applies it to all render textures marked as DynamicallyScalable. Note that the scale is applied to the render textures original dimensions so a scale factor of 1.0 will always be the full dimensions for the specified render target, etc.
        /// </description>
        static extern public void ResizeBuffers(float widthScale, float heightScale);
    }

    /// <summary>
    /// Struct containing basic FrameTimings and accompanying relevant data.
    /// </summary>
    [NativeHeader("Runtime/GfxDevice/FrameTiming.h")]
    [StructLayout(LayoutKind.Sequential)]
    public struct FrameTiming
    {
        // CPU events

        /// <summary>
        /// This is the CPU clock time at the point Present was called for the current frame.
        /// </summary>
        [NativeName("m_CPUTimePresentCalled")]  public UInt64 cpuTimePresentCalled;
        /// <summary>
        /// The CPU time for a given frame, in ms.
        /// </summary>
        [NativeName("m_CPUFrameTime")]          public double cpuFrameTime;

        // GPU events

        //This is the time the GPU finishes rendering the frame and interrupts the CPU
        /// <summary>
        /// This is the CPU clock time at the point GPU finished rendering the frame and interrupted the CPU.
        /// </summary>
        [NativeName("m_CPUTimeFrameComplete")]  public UInt64 cpuTimeFrameComplete;
        /// <summary>
        /// The GPU time for a given frame, in ms.
        /// </summary>
        [NativeName("m_GPUFrameTime")]          public double gpuFrameTime;

        //Linked data

        /// <summary>
        /// This was the height scale factor of the Dynamic Resolution system(if used) for the given frame and the linked frame timings.
        /// </summary>
        [NativeName("m_HeightScale")]           public float heightScale;
        /// <summary>
        /// This was the width scale factor of the Dynamic Resolution system(if used) for the given frame and the linked frame timings.
        /// </summary>
        [NativeName("m_WidthScale")]            public float widthScale;
        /// <summary>
        /// This was the vsync mode for the given frame and the linked frame timings.
        /// </summary>
        [NativeName("m_SyncInterval")]          public UInt32 syncInterval;
    }

    /// <summary>
    /// The FrameTimingManager allows the user to capture and access FrameTiming data for multple frames.
    /// </summary>
    [StaticAccessor("GetUncheckedRealGfxDevice().GetFrameTimingManager()", StaticAccessorType.Dot)]
    static public class FrameTimingManager
    {
        /// <summary>
        /// This function triggers the FrameTimingManager to capture a snapshot of FrameTiming's data, that can then be accessed by the user.
        /// </summary>
        /// <description>
        /// The FrameTimingManager tries to capture as many frames as the platform allows but will only capture complete timings from finished and valid frames so the number of frames it captures may vary. This will also capture platform specific extended frame timing data if the platform supports more in depth data specifically available to it.
        /// </description>
        static extern public void CaptureFrameTimings();
        /// <summary>
        /// Allows the user to access the currently captured FrameTimings.
        /// </summary>
        /// <param name="numFrames">
        /// User supplies a desired number of frames they would like FrameTimings for. This should be equal to or less than the maximum FrameTimings the platform can capture.
        /// </param>
        /// <param name="timings">
        /// An array of FrameTiming structs that is passed in by the user and will be filled with data as requested. It is the users job to make sure the array that is passed is large enough to hold the requested number of FrameTimings.
        /// </param>
        /// <returns>
        /// Returns the number of FrameTimings it actually was able to get. This will always be equal to or less than the requested numFrames depending on availability of captured FrameTimings.
        /// </returns>
        /// <description>
        /// Fills in a user supplied array with the requested number of FrameTimings, assuming there are enough available from the last call to CaptureFrameTimings. The array is filled in from the start with most recent completed frames FrameTimings and works backwards. So element 0 of the returned array will contain the data for the last fully finished frame. Depending on platform, the maximum frames that will ever be captured will vary and it can never return more than its maximum.
        /// </description>
        static extern public UInt32 GetLatestTimings(UInt32 numFrames, FrameTiming[] timings);

        /// <summary>
        /// This returns the number of vsyncs per second on the current platform, used to interpret timing results. If the platform does not support returning this value it will return 0.
        /// </summary>
        /// <returns>
        /// Number of vsyncs per second of the current platform.
        /// </returns>
        static extern public float GetVSyncsPerSecond();
        /// <summary>
        /// This returns the frequency of GPU timer on the current platform, used to interpret timing results. If the platform does not support returning this value it will return 0.
        /// </summary>
        /// <returns>
        /// GPU timer frequency for current platform.
        /// </returns>
        static extern public UInt64 GetGpuTimerFrequency();
        /// <summary>
        /// This returns the frequency of CPU timer on the current platform, used to interpret timing results. If the platform does not support returning this value it will return 0.
        /// </summary>
        /// <returns>
        /// CPU timer frequency for current platform.
        /// </returns>
        static extern public UInt64 GetCpuTimerFrequency();
    }
}

namespace UnityEngine
{
    [NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
    [StaticAccessor("GeometryUtilityScripting", StaticAccessorType.DoubleColon)]
    public sealed partial class GeometryUtility
    {
        /// <summary>
        /// Returns true if bounds are inside the plane array.
        /// </summary>
        /// <description>
        /// Will return true if the bounding box is inside the planes or intersects any of the planes.
        /// The TestPlanesAABB function uses the Plane array to test whether a bounding box is in the frustum or not.\\
        /// You can use this function with CalculateFrustrumPlanes to test whether a camera's view contains an object
        /// regardless of whether it is rendered or not.
        /// SA: [[GeometryUtility.CalculateFrustumPlanes]].
        /// </description>
        extern public static bool TestPlanesAABB(Plane[] planes, Bounds bounds);

        [NativeName("ExtractPlanes")]   extern private static void Internal_ExtractPlanes([Out] Plane[] planes, Matrix4x4 worldToProjectionMatrix);
        [NativeName("CalculateBounds")] extern private static Bounds Internal_CalculateBounds(Vector3[] positions, Matrix4x4 transform);
    }
}

namespace UnityEngine
{
    /// <summary>
    /// Data of a lightmap.
    /// </summary>
    /// <description>
    /// A Scene can have several lightmaps stored in it, and [[Renderer]] components can use those
    /// lightmaps. This makes it possible to use the same material on multiple objects, while
    /// each object can refer to a different lightmap or different portion of the same lightmap.
    /// SA: [[LightmapSettings]] class, [[Renderer.lightmapIndex]]
    /// </description>
    [UsedByNativeCode]
    [StructLayout(LayoutKind.Sequential)]
    [NativeHeader("Runtime/Graphics/LightmapData.h")]
    public sealed partial class LightmapData
    {
        internal Texture2D m_Light;
        internal Texture2D m_Dir;
        internal Texture2D m_ShadowMask;

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [System.Obsolete("Use lightmapColor property (UnityUpgradable) -> lightmapColor", false)]
        public Texture2D lightmapLight { get { return m_Light; }        set { m_Light = value; } }

        /// <summary>
        /// Lightmap storing color of incoming light.
        /// </summary>
        /// <description>
        /// SA: [[LightmapSettings]], [[Renderer.lightmapIndex]].
        /// </description>
        public Texture2D lightmapColor { get { return m_Light; }        set { m_Light = value; } }
        /// <summary>
        /// Lightmap storing dominant direction of incoming light.
        /// </summary>
        /// <description>
        /// SA: [[LightmapSettings]], [[Renderer.lightmapIndex]].
        /// </description>
        public Texture2D lightmapDir   { get { return m_Dir; }          set { m_Dir = value; } }
        /// <summary>
        /// Texture storing occlusion mask per light (ShadowMask, up to four lights).
        /// </summary>
        /// <description>
        /// SA: [[LightmapSettings]] class, [[Renderer.lightmapIndex]].
        /// </description>
        public Texture2D shadowMask    { get { return m_ShadowMask; }   set { m_ShadowMask = value; } }
    }

    // Stores lightmaps of the scene.
    /// <summary>
    /// Stores lightmaps of the Scene.
    /// </summary>
    /// <description>
    /// A Scene can have several lightmaps stored in it, and [[Renderer]] components can use those
    /// lightmaps. This makes it possible to use the same material on multiple objects, while
    /// each object can refer to a different lightmap or different portion of the same lightmap.
    /// SA: [[LightmapData]] class, [[Renderer.lightmapIndex]]
    /// </description>
    [NativeHeader("Runtime/Graphics/LightmapSettings.h")]
    [StaticAccessor("GetLightmapSettings()")]
    public sealed partial class LightmapSettings : Object
    {
        private LightmapSettings() {}

        // Lightmap array.
        /// <summary>
        /// Lightmap array.
        /// </summary>
        /// <description>
        /// SA: [[LightmapData]] class, [[Renderer.lightmapIndex]]
        /// </description>
        public extern static LightmapData[] lightmaps {[FreeFunction] get; [FreeFunction] set; }

        /// <summary>
        /// NonDirectional or CombinedDirectional Specular lightmaps rendering mode.
        /// </summary>
        /// <description>
        /// Note: this property is only serialized when building the player. In all the other cases it's the responsibility of the Unity lightmapping system (or a custom script that brings external lightmapping data) to set it when the Scene loads or playmode is entered.
        /// You can continue reading [[wiki:LightmappingDirectional|here]] if you want to know what the different lightmap modes do.
        /// </description>
        public extern static LightmapsMode lightmapsMode { get; [FreeFunction(ThrowsException = true)] set; }

        // Holds all data needed by the light probes.
        /// <summary>
        /// Holds all data needed by the light probes.
        /// </summary>
        /// <description>
        /// It can be swapped to a different pre-baked one at runtime.
        /// </description>
        public extern static LightProbes lightProbes { get; set; }

        [NativeName("ResetAndAwakeFromLoad")]
        internal static extern void Reset();
    }
}

namespace UnityEngine
{
    // Stores light probes for the scene.
    /// <summary>
    /// Stores light probes for the Scene.
    /// </summary>
    /// <description>
    /// The baked data includes: probe positions, Spherical Harmonics (SH) coefficients and the tetrahedral tessellation.
    /// You can modify the coefficients at runtime. You can also swap the entire LightProbes object to
    /// a different pre-baked one using [[LightmapSettings.lightProbes]].
    /// SA: [[LightmapSettings]] class, [[Renderer.useLightProbes]] property.
    /// </description>
    [NativeAsStruct]
    [StructLayout(LayoutKind.Sequential)]
    [NativeHeader("Runtime/Export/Graphics/Graphics.bindings.h")]
    public sealed partial class LightProbes : Object
    {
        private LightProbes() {}

        /// <summary>
        /// Returns an interpolated probe for the given position for both realtime and baked light probes combined.
        /// </summary>
        /// <description>
        /// Renderer is only needed to speed up the search for the current tetrahedron, as it caches the index of the tetrahedron it was in the last frame.
        /// </description>
        [FreeFunction]
        public extern static void GetInterpolatedProbe(Vector3 position, Renderer renderer, out UnityEngine.Rendering.SphericalHarmonicsL2 probe);

        [FreeFunction]
        internal static extern bool AreLightProbesAllowed(Renderer renderer);

        /// <summary>
        /// Calculate light probes and occlusion probes at the given world space positions.
        /// </summary>
        /// <param name="positions">
        /// The array of world space positions used to evaluate the probes.
        /// </param>
        /// <param name="lightProbes">
        /// The array where the resulting light probes are written to.
        /// </param>
        /// <param name="occlusionProbes">
        /// The array where the resulting occlusion probes are written to.
        /// </param>
        /// <description>
        /// If there are no probes baked in the Scene, the ambient probe will be written to the /lightProbes/ array and /Vector4/ (1,1,1,1) will be written to the /occlusionProbes/ array. \\
        /// ArgumentNullException is thrown if /positions/ is /null/.\\
        /// You can omit either /lightProbes/ or /occlusionProbes/ array by passing /null/ to the function, but you cannot omit both at the same time. If both arrays are omitted, an ArgumentException is thrown. /lightProbes/ and /occlusionProbes/ should be calculated together for better performance.\\
        /// For the overload which takes arrays as arguments, the /lightProbes/ and /occlusionProbes/ must have at least the same number of elements as the /positions/ array.\\
        /// For the overload which takes lists as arguments, the output lists will be resized to fit the size of the /positions/ array if there is not enough space in the given lists.\\
        /// The returned probes may be further used for instanced rendering by copying them to a MaterialPropertyBlock object via [[MaterialPropertyBlock.CopySHCoefficientArraysFrom]] and [[MaterialPropertyBlock.CopyProbeOcclusionArrayFrom]].
        /// </description>
        /// <description>
        /// The example demonstrates how to leverage the baked light probes to enhance the visual quality of instanced rendering.
        /// </description>
        public static void CalculateInterpolatedLightAndOcclusionProbes(Vector3[] positions, SphericalHarmonicsL2[] lightProbes, Vector4[] occlusionProbes)
        {
            if (positions == null)
                throw new ArgumentNullException("positions");
            else if (lightProbes == null && occlusionProbes == null)
                throw new ArgumentException("Argument lightProbes and occlusionProbes cannot both be null.");
            else if (lightProbes != null && lightProbes.Length < positions.Length)
                throw new ArgumentException("lightProbes", "Argument lightProbes has less elements than positions");
            else if (occlusionProbes != null && occlusionProbes.Length < positions.Length)
                throw new ArgumentException("occlusionProbes", "Argument occlusionProbes has less elements than positions");

            CalculateInterpolatedLightAndOcclusionProbes_Internal(positions, positions.Length, lightProbes, occlusionProbes);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void CalculateInterpolatedLightAndOcclusionProbes(List<Vector3> positions, List<SphericalHarmonicsL2> lightProbes, List<Vector4> occlusionProbes)
        {
            if (positions == null)
                throw new ArgumentNullException("positions");
            else if (lightProbes == null && occlusionProbes == null)
                throw new ArgumentException("Argument lightProbes and occlusionProbes cannot both be null.");

            if (lightProbes != null)
            {
                if (lightProbes.Capacity < positions.Count)
                    lightProbes.Capacity = positions.Count;
                if (lightProbes.Count < positions.Count)
                    NoAllocHelpers.ResizeList(lightProbes, positions.Count);
            }

            if (occlusionProbes != null)
            {
                if (occlusionProbes.Capacity < positions.Count)
                    occlusionProbes.Capacity = positions.Count;
                if (occlusionProbes.Count < positions.Count)
                    NoAllocHelpers.ResizeList(occlusionProbes, positions.Count);
            }

            CalculateInterpolatedLightAndOcclusionProbes_Internal(NoAllocHelpers.ExtractArrayFromListT(positions), positions.Count, NoAllocHelpers.ExtractArrayFromListT(lightProbes), NoAllocHelpers.ExtractArrayFromListT(occlusionProbes));
        }

        [FreeFunction]
        [NativeName("CalculateInterpolatedLightAndOcclusionProbes")]
        internal extern static void CalculateInterpolatedLightAndOcclusionProbes_Internal(Vector3[] positions, int positionsCount, SphericalHarmonicsL2[] lightProbes, Vector4[] occlusionProbes);

        // Positions of the baked light probes.
        /// <summary>
        /// Positions of the baked light probes (RO).
        /// </summary>
        /// <description>
        /// SA: count property.
        /// </description>
        public extern Vector3[] positions { get; }

        /// <summary>
        /// Coefficients of baked light probes.
        /// </summary>
        /// <description>
        /// SA: [[Rendering.SphericalHarmonicsL2]].
        /// </description>
        /// <dw-legacy-code>
        /// using UnityEngine;
        /// using UnityEngine.Rendering;
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///     public Color m_Ambient;
        ///     public Light[] m_Lights;
        ///     // On start add the contribution of the ambient light and all lights
        ///     // assigned to the lights array to all baked probes.
        ///     void Start()
        ///     {
        ///         SphericalHarmonicsL2[] bakedProbes = LightmapSettings.lightProbes.bakedProbes;
        ///         Vector3[] probePositions = LightmapSettings.lightProbes.positions;
        ///         int probeCount = LightmapSettings.lightProbes.count;
        ///         // Clear all probes
        ///         for (int i = 0; i < probeCount; i++)
        ///             bakedProbes[i].Clear();
        ///         // Add ambient light to all probes
        ///         for (int i = 0; i < probeCount; i++)
        ///             bakedProbes[i].AddAmbientLight(m_Ambient);
        ///         // Add directional and point lights' contribution to all probes
        ///         foreach (Light l in m_Lights)
        ///         {
        ///             if (l.type == LightType.Directional)
        ///             {
        ///                 for (int i = 0; i < probeCount; i++)
        ///                     bakedProbes[i].AddDirectionalLight(-l.transform.forward, l.color, l.intensity);
        ///             }
        ///             else if (l.type == LightType.Point)
        ///             {
        ///                 for (int i = 0; i < probeCount; i++)
        ///                     SHAddPointLight(probePositions[i], l.transform.position, l.range, l.color, l.intensity, ref bakedProbes[i]);
        ///             }
        ///         }
        ///         LightmapSettings.lightProbes.bakedProbes = bakedProbes;
        ///     }
        ///     void SHAddPointLight(Vector3 probePosition, Vector3 position, float range, Color color, float intensity, ref SphericalHarmonicsL2 sh)
        ///     {
        ///         // From the point of view of an SH probe, point light looks no different than a directional light,
        ///         // just attenuated and coming from the right direction.
        ///         Vector3 probeToLight = position - probePosition;
        ///         float attenuation = 1.0F / (1.0F + 25.0F * probeToLight.sqrMagnitude / (range * range));
        ///         sh.AddDirectionalLight(probeToLight.normalized, color, intensity * attenuation);
        ///     }
        /// }
        /// </dw-legacy-code>
        public extern UnityEngine.Rendering.SphericalHarmonicsL2[] bakedProbes
        {
            [NativeName("GetBakedCoefficients")] get;
            [NativeName("SetBakedCoefficients")] set;

            // if (GetScriptingArraySize(value) != self->GetLightProbeData().GetNumProbes())
            //     Scripting::RaiseArgumentException("Coefficients array must have the same amount of elements as the probe count.");
        }

        // The number of light probes.
        /// <summary>
        /// The number of light probes (RO).
        /// </summary>
        public extern int count { get; }

        // The number of cells (tetrahedra + outer cells) the space is divided to.
        /// <summary>
        /// The number of cells space is divided into (RO).
        /// </summary>
        /// <description>
        /// This includes both interior cells (tetrahedra) and outer space cells.
        /// </description>
        public extern int cellCount {[NativeName("GetTetrahedraSize")] get; }
    }
}

namespace UnityEngine.Experimental.Rendering
{
    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    public enum WaitForPresentSyncPoint
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        BeginFrame = 0,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        EndFrame = 1
    }

    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    public enum GraphicsJobsSyncPoint
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        EndOfFrame = 0,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        AfterScriptUpdate = 1,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        AfterScriptLateUpdate = 2,
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        WaitForPresent = 3
    };

    /// <summary>
    /// There is currently no documentation for this api.
    /// </summary>
    public static partial class GraphicsDeviceSettings
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
        extern public static WaitForPresentSyncPoint waitForPresentSyncPoint { get; set; }
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
        extern public static GraphicsJobsSyncPoint graphicsJobsSyncPoint { get; set; }
    }
}
