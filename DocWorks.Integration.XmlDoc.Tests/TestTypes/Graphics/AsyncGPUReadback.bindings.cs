using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Bindings;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Collections;
using UnityEngine.Scripting;
using UnityEngine.Experimental.Rendering;

namespace UnityEngine.Rendering
{
    /// <summary>
    /// Represents an asynchronous request for a GPU resource.
    /// </summary>
    /// <description>
    /// Use [[AsyncGPUReadback.Request]] to retrieve an asynchronous request for a GPU resource.
    /// Pending requests are automatically updated each frame. The result is accessible only for a single frame once is successfully fulfilled and this request is then disposed of in the following frame.
    /// Common uses for this are to query [[AsyncGPUReadbackRequest.done]] each frame (or within a coroutine) and then call [[AsyncGPUReadbackRequest.GetData]] if the [[AsyncGPUReadbackRequest.hasError]] is false.
    /// You don't have to manage the request lifetime as this is managed internally. A request that has been disposed of will result in the [[AsyncGPUReadbackRequest.hasError]] property being true.
    /// SA:[[AsyncGPUReadback]].
    /// </description>
    [NativeHeader("Runtime/Graphics/AsyncGPUReadbackManaged.h")]
    [NativeHeader("Runtime/Graphics/Texture.h")]
    [NativeHeader("Runtime/Shaders/ComputeShader.h")]
    [StructLayout(LayoutKind.Sequential)]
    [UsedByNativeCode]
    public struct AsyncGPUReadbackRequest
    {
        internal IntPtr m_Ptr;
        internal int m_Version;

        /// <summary>
        /// Triggers an update of the request.
        /// </summary>
        /// <description>
        /// Pending requests are automatically updated each frame and require no further action to complete.
        /// </description>
        public extern void Update();
        /// <summary>
        /// Waits for completion of the request.
        /// </summary>
        /// <description>
        /// Calling [[AsyncGPUReadbackRequest.done]] on the request after this call, will always result in true whether the request has completed successfully or not.
        /// [[AsyncGPUReadbackRequest.hasError]] can be used to find out whether this request has been successful.
        /// Since WaitForCompletion stalls both GPU and CPU, calling this function will result in a large performance hit and should be used sparingly.
        /// </description>
        public extern void WaitForCompletion();

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public unsafe NativeArray<T> GetData<T>(int layer = 0) where T : struct
        {
            if (!done || hasError)
                throw new InvalidOperationException("Cannot access the data as it is not available");

            if (layer < 0 || layer >= layerCount)
                throw new ArgumentException(string.Format("Layer index is out of range {0} / {1}", layer, layerCount));

            int stride = UnsafeUtility.SizeOf<T>();

            var array = NativeArrayUnsafeUtility.ConvertExistingDataToNativeArray<T>((void*)GetDataRaw(layer), layerDataSize / stride, Allocator.None);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            NativeArrayUnsafeUtility.SetAtomicSafetyHandle(ref array, GetSafetyHandle());
#endif
            return array;
        }

        /// <summary>
        /// Checks whether the request has been processed.
        /// </summary>
        /// <description>
        /// If an error has occurred during processing, then calling [[AsyncGPUReadbackRequest.hasError]] on this request will return true.
        /// </description>
        public bool done { get { return IsDone(); } }
        /// <summary>
        /// This property is true if the request has encountered an error.
        /// </summary>
        public bool hasError { get { return HasError(); } }
        /// <summary>
        /// Number of layers in the current request.
        /// </summary>
        /// <description>
        /// SA: GetData.
        /// </description>
        public int layerCount { get { return GetLayerCount(); } }
        /// <summary>
        /// The size in bytes of one layer of the readback data.
        /// </summary>
        /// <description>
        /// SA: GetData.
        /// </description>
        public int layerDataSize { get { return GetLayerDataSize(); } }
        /// <summary>
        /// The width of the requested GPU data.
        /// </summary>
        /// <description>
        /// The size in bytes for [[ComputeBuffer]] readback.
        /// </description>
        public int width { get { return GetWidth(); } }
        /// <summary>
        /// When reading data from a [[ComputeBuffer]], height is 1, otherwise, the property takes the value of the requested height from the texture.
        /// </summary>
        public int height { get { return GetHeight(); } }
        /// <summary>
        /// When reading data from a [[ComputeBuffer]], depth is 1, otherwise, the property takes the value of the requested depth from the texture.
        /// </summary>
        public int depth { get { return GetDepth(); } }

        private extern bool IsDone();
        private extern bool HasError();
        private extern int GetLayerCount();
        private extern int GetLayerDataSize();
        private extern int GetWidth();
        private extern int GetHeight();
        private extern int GetDepth();

#if ENABLE_UNITY_COLLECTIONS_CHECKS
        internal extern void CreateSafetyHandle();
        private extern AtomicSafetyHandle GetSafetyHandle();
#endif
        internal extern void SetScriptingCallback(Action<AsyncGPUReadbackRequest> callback);
        unsafe private extern IntPtr GetDataRaw(int layer);
    }

    /// <summary>
    /// Allows the asynchronous read back of GPU resources.
    /// </summary>
    /// <description>
    /// This class is used to copy resource data from the GPU to the CPU without any stall (GPU or CPU), but adds a few frames of latency.
    /// SA: [[AsyncGPUReadbackRequest]].
    /// </description>
    [StaticAccessor("AsyncGPUReadbackManager::GetInstance()", StaticAccessorType.Dot)]
    public static class AsyncGPUReadback
    {
        internal static void ValidateFormat(Texture src, GraphicsFormat dstformat)
        {
            GraphicsFormat srcformat = GraphicsFormatUtility.GetFormat(src);
            if (!SystemInfo.IsFormatSupported(srcformat, FormatUsage.ReadPixels))
                Debug.LogError(String.Format("'{0}' doesn't support ReadPixels usage on this platform. Async GPU readback failed.", srcformat));
        }

        static private void SetUpScriptingRequest(AsyncGPUReadbackRequest request, Action<AsyncGPUReadbackRequest> callback)
        {
            request.SetScriptingCallback(callback);
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            request.CreateSafetyHandle();
#endif
        }

        /// <summary>
        /// Triggers a request to asynchronously fetch the data from a GPU resource.
        /// </summary>
        /// <param name="src">
        /// The resource to read the data from.
        /// </param>
        /// <returns>
        /// An [[AsyncGPUReadbackRequest]] that can be used to both access the data and check whether it is available.
        /// </returns>
        /// <description>
        /// If an error occurs due to some invalid arguments being passed to this method, a request with an error is returned. If a request with an error is returned, calling [[AsyncGPUReadbackRequest.hasError]] on that request will return true.
        /// For texture data the extents are checked against the size of the source texture. If graphics [[QualitySettings]] are set low enough to trigger the generation of reduced size textures then the reduced size must be requested. Use [[QualitySettings.masterTextureLimit]] to adjust the width and height (and x,y if required), by bit shifting right.
        /// </description>
        static public AsyncGPUReadbackRequest Request(ComputeBuffer src, Action<AsyncGPUReadbackRequest> callback = null)
        {
            AsyncGPUReadbackRequest request = Request_Internal_ComputeBuffer_1(src);
            SetUpScriptingRequest(request, callback);
            return request;
        }

        /// <summary>
        /// Triggers a request to asynchronously fetch the data from a GPU resource.
        /// </summary>
        /// <param name="src">
        /// The resource to read the data from.
        /// </param>
        /// <param name="size">
        /// Size in bytes of the data to be retrieved from the [[ComputeBuffer]].
        /// </param>
        /// <param name="offset">
        /// Offset in bytes in the [[ComputeBuffer]].
        /// </param>
        /// <returns>
        /// An [[AsyncGPUReadbackRequest]] that can be used to both access the data and check whether it is available.
        /// </returns>
        /// <description>
        /// If an error occurs due to some invalid arguments being passed to this method, a request with an error is returned. If a request with an error is returned, calling [[AsyncGPUReadbackRequest.hasError]] on that request will return true.
        /// For texture data the extents are checked against the size of the source texture. If graphics [[QualitySettings]] are set low enough to trigger the generation of reduced size textures then the reduced size must be requested. Use [[QualitySettings.masterTextureLimit]] to adjust the width and height (and x,y if required), by bit shifting right.
        /// </description>
        static public AsyncGPUReadbackRequest Request(ComputeBuffer src, int size, int offset, Action<AsyncGPUReadbackRequest> callback = null)
        {
            AsyncGPUReadbackRequest request = Request_Internal_ComputeBuffer_2(src, size, offset);
            SetUpScriptingRequest(request, callback);
            return request;
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        static public AsyncGPUReadbackRequest Request(Texture src, int mipIndex = 0, Action<AsyncGPUReadbackRequest> callback = null)
        {
            AsyncGPUReadbackRequest request = Request_Internal_Texture_1(src, mipIndex);
            SetUpScriptingRequest(request, callback);
            return request;
        }

        /// <summary>
        /// Triggers a request to asynchronously fetch the data from a GPU resource.
        /// </summary>
        /// <param name="src">
        /// The resource to read the data from.
        /// </param>
        /// <param name="mipIndex">
        /// The index of the mipmap to be fetched.
        /// </param>
        /// <param name="dstFormat">
        /// The target [[TextureFormat]] of the data. Conversion will happen automatically if format is different from the format stored on GPU.
        /// </param>
        /// <returns>
        /// An [[AsyncGPUReadbackRequest]] that can be used to both access the data and check whether it is available.
        /// </returns>
        /// <description>
        /// If an error occurs due to some invalid arguments being passed to this method, a request with an error is returned. If a request with an error is returned, calling [[AsyncGPUReadbackRequest.hasError]] on that request will return true.
        /// For texture data the extents are checked against the size of the source texture. If graphics [[QualitySettings]] are set low enough to trigger the generation of reduced size textures then the reduced size must be requested. Use [[QualitySettings.masterTextureLimit]] to adjust the width and height (and x,y if required), by bit shifting right.
        /// </description>
        static public AsyncGPUReadbackRequest Request(Texture src, int mipIndex, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
        {
            return Request(src, mipIndex, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        static public AsyncGPUReadbackRequest Request(Texture src, int mipIndex, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
        {
            ValidateFormat(src, dstFormat);
            AsyncGPUReadbackRequest request = Request_Internal_Texture_2(src, mipIndex, dstFormat);
            SetUpScriptingRequest(request, callback);
            return request;
        }

        /// <summary>
        /// Triggers a request to asynchronously fetch the data from a GPU resource.
        /// </summary>
        /// <param name="src">
        /// The resource to read the data from.
        /// </param>
        /// <param name="mipIndex">
        /// The index of the mipmap to be fetched.
        /// </param>
        /// <param name="x">
        /// Starting X coordinate in pixels of the Texture data to be fetched.
        /// </param>
        /// <param name="width">
        /// Width in pixels of the Texture data to be fetched.
        /// </param>
        /// <param name="y">
        /// Starting Y coordinate in pixels of the Texture data to be fetched.
        /// </param>
        /// <param name="height">
        /// Height in pixels of the Texture data to be fetched.
        /// </param>
        /// <param name="z">
        /// Start Z coordinate in pixels for the [[Texture3D]] being fetched. Index of Start layer for [[TextureCube]], [[Texture2DArray]] and [[TextureCubeArray]] being fetched.
        /// </param>
        /// <param name="depth">
        /// Depth in pixels for [[Texture3D]] being fetched. Number of layers for [[TextureCube]], [[TextureArray]] and [[TextureCubeArray]].
        /// </param>
        /// <returns>
        /// An [[AsyncGPUReadbackRequest]] that can be used to both access the data and check whether it is available.
        /// </returns>
        /// <description>
        /// If an error occurs due to some invalid arguments being passed to this method, a request with an error is returned. If a request with an error is returned, calling [[AsyncGPUReadbackRequest.hasError]] on that request will return true.
        /// For texture data the extents are checked against the size of the source texture. If graphics [[QualitySettings]] are set low enough to trigger the generation of reduced size textures then the reduced size must be requested. Use [[QualitySettings.masterTextureLimit]] to adjust the width and height (and x,y if required), by bit shifting right.
        /// </description>
        static public AsyncGPUReadbackRequest Request(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, Action<AsyncGPUReadbackRequest> callback = null)
        {
            AsyncGPUReadbackRequest request = Request_Internal_Texture_3(src, mipIndex, x, width, y, height, z, depth);
            SetUpScriptingRequest(request, callback);
            return request;
        }

        /// <summary>
        /// Triggers a request to asynchronously fetch the data from a GPU resource.
        /// </summary>
        /// <param name="src">
        /// The resource to read the data from.
        /// </param>
        /// <param name="mipIndex">
        /// The index of the mipmap to be fetched.
        /// </param>
        /// <param name="x">
        /// Starting X coordinate in pixels of the Texture data to be fetched.
        /// </param>
        /// <param name="width">
        /// Width in pixels of the Texture data to be fetched.
        /// </param>
        /// <param name="y">
        /// Starting Y coordinate in pixels of the Texture data to be fetched.
        /// </param>
        /// <param name="height">
        /// Height in pixels of the Texture data to be fetched.
        /// </param>
        /// <param name="z">
        /// Start Z coordinate in pixels for the [[Texture3D]] being fetched. Index of Start layer for [[TextureCube]], [[Texture2DArray]] and [[TextureCubeArray]] being fetched.
        /// </param>
        /// <param name="depth">
        /// Depth in pixels for [[Texture3D]] being fetched. Number of layers for [[TextureCube]], [[TextureArray]] and [[TextureCubeArray]].
        /// </param>
        /// <param name="dstFormat">
        /// The target [[TextureFormat]] of the data. Conversion will happen automatically if format is different from the format stored on GPU.
        /// </param>
        /// <returns>
        /// An [[AsyncGPUReadbackRequest]] that can be used to both access the data and check whether it is available.
        /// </returns>
        /// <description>
        /// If an error occurs due to some invalid arguments being passed to this method, a request with an error is returned. If a request with an error is returned, calling [[AsyncGPUReadbackRequest.hasError]] on that request will return true.
        /// For texture data the extents are checked against the size of the source texture. If graphics [[QualitySettings]] are set low enough to trigger the generation of reduced size textures then the reduced size must be requested. Use [[QualitySettings.masterTextureLimit]] to adjust the width and height (and x,y if required), by bit shifting right.
        /// </description>
        static public AsyncGPUReadbackRequest Request(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, TextureFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
        {
            return Request(src, mipIndex, x, width, y, height, z, depth, GraphicsFormatUtility.GetGraphicsFormat(dstFormat, QualitySettings.activeColorSpace == ColorSpace.Linear), callback);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        static public AsyncGPUReadbackRequest Request(Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat, Action<AsyncGPUReadbackRequest> callback = null)
        {
            ValidateFormat(src, dstFormat);
            AsyncGPUReadbackRequest request = Request_Internal_Texture_4(src, mipIndex, x, width, y, height, z, depth, dstFormat);
            SetUpScriptingRequest(request, callback);
            return request;
        }

        [NativeMethod("Request")]
        static private extern AsyncGPUReadbackRequest Request_Internal_ComputeBuffer_1([NotNull] ComputeBuffer buffer);
        [NativeMethod("Request")]
        static private extern AsyncGPUReadbackRequest Request_Internal_ComputeBuffer_2([NotNull] ComputeBuffer src, int size, int offset);
        [NativeMethod("Request")]
        static private extern AsyncGPUReadbackRequest Request_Internal_Texture_1([NotNull] Texture src, int mipIndex);
        [NativeMethod("Request")]
        static private extern AsyncGPUReadbackRequest Request_Internal_Texture_2([NotNull] Texture src, int mipIndex, GraphicsFormat dstFormat);
        [NativeMethod("Request")]
        static private extern AsyncGPUReadbackRequest Request_Internal_Texture_3([NotNull] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth);
        [NativeMethod("Request")]
        static private extern AsyncGPUReadbackRequest Request_Internal_Texture_4([NotNull] Texture src, int mipIndex, int x, int width, int y, int height, int z, int depth, GraphicsFormat dstFormat);
    }
}
