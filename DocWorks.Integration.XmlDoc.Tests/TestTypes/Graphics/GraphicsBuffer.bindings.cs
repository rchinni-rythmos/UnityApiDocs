using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;
using UnityEngine.Bindings;

namespace UnityEngine
{
    // Data buffer to hold data for vertex/index buffers.
    /// <summary>
    /// GPU graphics data buffer, for working with data such as vertex and index buffers.
    /// </summary>
    /// <description>
    /// Most draw calls supply vertex and index buffers to the GPU. This structure exposes those buffers to script, in order to allow for low-level rendering control.
    /// SA: [[Graphics.DrawProcedural]].
    /// </description>
    [UsedByNativeCode]
    [NativeHeader("Runtime/GfxDevice/GfxBuffer.h")]
    [NativeHeader("Runtime/Export/Graphics/GraphicsBuffer.bindings.h")]
    public sealed class GraphicsBuffer : IDisposable
    {
#pragma warning disable 414
        internal IntPtr m_Ptr;
#pragma warning restore 414

        // IDisposable implementation, with Release() for explicit cleanup.

        /// <summary>
        /// The type of graphics buffer.
        /// </summary>
        [Flags]
        public enum Target
        {
            //Vertex = 1 << 0,
            /// <summary>
            /// Use the graphics buffer to supply indices to the GPU.
            /// </summary>
            Index = 1 << 1
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        ~GraphicsBuffer()
        {
            Dispose(false);
        }

        //*undocumented*
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Release native resources
                DestroyBuffer(this);
            }
            else if (m_Ptr != IntPtr.Zero)
            {
                // We cannot call DestroyBuffer through GC - it is scripting_api and requires main thread, prefer leak instead of a crash
                Debug.LogWarning("GarbageCollector disposing of GraphicsBuffer. Please use GraphicsBuffer.Release() or .Dispose() to manually release the buffer.");
            }

            m_Ptr = IntPtr.Zero;
        }

        [FreeFunction("GraphicsBuffer_Bindings::InitBuffer")]
        extern private static IntPtr InitBuffer(Target target, int count, int stride);

        [FreeFunction("GraphicsBuffer_Bindings::DestroyBuffer")]
        extern private static void DestroyBuffer(GraphicsBuffer buf);

        // Create a Graphics Buffer.
        /// <summary>
        /// Create a Graphics Buffer.
        /// </summary>
        /// <param name="target">
        /// Select whether this buffer can be used as a vertex or index buffer.
        /// </param>
        /// <param name="count">
        /// Number of elements in the buffer.
        /// </param>
        /// <param name="stride">
        /// Size of one element in the buffer. For index buffers, this must be either 2 or 4 bytes.
        /// </param>
        /// <description>
        /// Use Release to release the buffer when no longer needed.
        /// SA: [[Graphics.DrawProcedural]].
        /// </description>
        public GraphicsBuffer(Target target, int count, int stride)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Attempting to create a zero length graphics buffer", "count");
            }

            if (stride <= 0)
            {
                throw new ArgumentException("Attempting to create a graphics buffer with a negative or null stride", "stride");
            }

            if ((target & Target.Index) != 0 && stride != 2 && stride != 4)
            {
                throw new ArgumentException("Attempting to create an index buffer with an invalid stride: " + stride, "stride");
            }

            m_Ptr = InitBuffer(target, count, stride);
        }

        // Release a Graphics Buffer.
        /// <summary>
        /// Release a Graphics Buffer.
        /// </summary>
        public void Release()
        {
            Dispose();
        }

        /// <summary>
        /// Returns true if this graphics buffer is valid, or false otherwise.
        /// </summary>
        public bool IsValid()
        {
            return m_Ptr != IntPtr.Zero;
        }

        // Number of elements in the buffer (RO).
        /// <summary>
        /// Number of elements in the buffer (RO).
        /// </summary>
        /// <description>
        /// SA: stride.
        /// </description>
        extern public int count { get; }

        // Size of one element in the buffer (RO).
        /// <summary>
        /// Size of one element in the buffer (RO).
        /// </summary>
        /// <description>
        /// SA: count.
        /// </description>
        extern public int stride { get; }

        // Set buffer data.
        /// <summary>
        /// Set the buffer with values from an array.
        /// </summary>
        /// <param name="data">
        /// Array of values to fill the buffer.
        /// </param>
        [System.Security.SecuritySafeCritical] // due to Marshal.SizeOf
        public void SetData(System.Array data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (!UnsafeUtility.IsArrayBlittable(data))
            {
                throw new ArgumentException(
                    string.Format("Array passed to GraphicsBuffer.SetData(array) must be blittable.\n{0}",
                        UnsafeUtility.GetReasonForArrayNonBlittable(data)));
            }

            InternalSetData(data, 0, 0, data.Length, UnsafeUtility.SizeOf(data.GetType().GetElementType()));
        }

        // Set buffer data.
        /// <summary>
        /// Set the buffer with values from an array.
        /// </summary>
        /// <param name="data">
        /// Array of values to fill the buffer.
        /// </param>
        [System.Security.SecuritySafeCritical] // due to Marshal.SizeOf
        public void SetData<T>(List<T> data) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (!UnsafeUtility.IsGenericListBlittable<T>())
            {
                throw new ArgumentException(
                    string.Format("List<{0}> passed to GraphicsBuffer.SetData(List<>) must be blittable.\n{1}",
                        typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
            }

            InternalSetData(NoAllocHelpers.ExtractArrayFromList(data), 0, 0, NoAllocHelpers.SafeLength(data), Marshal.SizeOf(typeof(T)));
        }

        /// <summary>
        /// Set the buffer with values from an array.
        /// </summary>
        /// <param name="data">
        /// Array of values to fill the buffer.
        /// </param>
        [System.Security.SecuritySafeCritical] // due to Marshal.SizeOf
        unsafe public void SetData<T>(NativeArray<T> data) where T : struct
        {
            // Note: no IsBlittable test here because it's already done at NativeArray creation time
            InternalSetNativeData((IntPtr)data.GetUnsafeReadOnlyPtr(), 0, 0, data.Length, UnsafeUtility.SizeOf<T>());
        }

        // Set partial buffer data
        /// <summary>
        /// Partial copy of data values from an array into the buffer.
        /// </summary>
        /// <param name="data">
        /// Array of values to fill the buffer.
        /// </param>
        /// <param name="managedBufferStartIndex">
        /// The first element index in data to copy to the graphics buffer.
        /// </param>
        /// <param name="graphicsBufferStartIndex">
        /// The first element index in the graphics buffer to receive the data.
        /// </param>
        /// <param name="count">
        /// The number of elements to copy.
        /// </param>
        [System.Security.SecuritySafeCritical] // due to Marshal.SizeOf
        public void SetData(System.Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (!UnsafeUtility.IsArrayBlittable(data))
            {
                throw new ArgumentException(
                    string.Format("Array passed to GraphicsBuffer.SetData(array) must be blittable.\n{0}",
                        UnsafeUtility.GetReasonForArrayNonBlittable(data)));
            }

            if (managedBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Length)
                throw new ArgumentOutOfRangeException(String.Format("Bad indices/count arguments (managedBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", managedBufferStartIndex, graphicsBufferStartIndex, count));

            InternalSetData(data, managedBufferStartIndex, graphicsBufferStartIndex, count, Marshal.SizeOf(data.GetType().GetElementType()));
        }

        // Set partial buffer data
        /// <summary>
        /// Partial copy of data values from an array into the buffer.
        /// </summary>
        /// <param name="data">
        /// Array of values to fill the buffer.
        /// </param>
        /// <param name="managedBufferStartIndex">
        /// The first element index in data to copy to the graphics buffer.
        /// </param>
        /// <param name="graphicsBufferStartIndex">
        /// The first element index in the graphics buffer to receive the data.
        /// </param>
        /// <param name="count">
        /// The number of elements to copy.
        /// </param>
        [System.Security.SecuritySafeCritical] // due to Marshal.SizeOf
        public void SetData<T>(List<T> data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
        {
            if (data == null)
                throw new ArgumentNullException("data");

            if (!UnsafeUtility.IsGenericListBlittable<T>())
            {
                throw new ArgumentException(
                    string.Format("List<{0}> passed to GraphicsBuffer.SetData(List<>) must be blittable.\n{1}",
                        typeof(T), UnsafeUtility.GetReasonForGenericListNonBlittable<T>()));
            }

            if (managedBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || managedBufferStartIndex + count > data.Count)
                throw new ArgumentOutOfRangeException(String.Format("Bad indices/count arguments (managedBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", managedBufferStartIndex, graphicsBufferStartIndex, count));

            InternalSetData(NoAllocHelpers.ExtractArrayFromList(data), managedBufferStartIndex, graphicsBufferStartIndex, count, Marshal.SizeOf(typeof(T)));
        }

        /// <summary>
        /// Partial copy of data values from an array into the buffer.
        /// </summary>
        /// <param name="data">
        /// Array of values to fill the buffer.
        /// </param>
        /// <param name="graphicsBufferStartIndex">
        /// The first element index in the graphics buffer to receive the data.
        /// </param>
        /// <param name="count">
        /// The number of elements to copy.
        /// </param>
        [System.Security.SecuritySafeCritical] // due to Marshal.SizeOf
        public unsafe void SetData<T>(NativeArray<T> data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count) where T : struct
        {
            // Note: no IsBlittable test here because it's already done at NativeArray creation time
            if (nativeBufferStartIndex < 0 || graphicsBufferStartIndex < 0 || count < 0 || nativeBufferStartIndex + count > data.Length)
                throw new ArgumentOutOfRangeException(String.Format("Bad indices/count arguments (nativeBufferStartIndex:{0} graphicsBufferStartIndex:{1} count:{2})", nativeBufferStartIndex, graphicsBufferStartIndex, count));

            InternalSetNativeData((IntPtr)data.GetUnsafeReadOnlyPtr(), nativeBufferStartIndex, graphicsBufferStartIndex, count, UnsafeUtility.SizeOf<T>());
        }

        [System.Security.SecurityCritical] // to prevent accidentally making this public in the future
        [FreeFunction(Name = "GraphicsBuffer_Bindings::InternalSetNativeData", HasExplicitThis = true, ThrowsException = true)]
        extern private void InternalSetNativeData(IntPtr data, int nativeBufferStartIndex, int graphicsBufferStartIndex, int count, int elemSize);

        [System.Security.SecurityCritical] // to prevent accidentally making this public in the future
        [FreeFunction(Name = "GraphicsBuffer_Bindings::InternalSetData", HasExplicitThis = true, ThrowsException = true)]
        extern private void InternalSetData(System.Array data, int managedBufferStartIndex, int graphicsBufferStartIndex, int count, int elemSize);

        /// <summary>
        /// Retrieve a native (underlying graphics API) pointer to the buffer.
        /// </summary>
        /// <returns>
        /// Pointer to the underlying graphics API buffer.
        /// </returns>
        /// <description>
        /// Use this function to retrieve a pointer/handle corresponding to the graphics buffer,
        /// as it is represented in the native graphics API. This can be used to enable
        /// graphics buffer data manipulation from [[wiki:NativePluginInterface|native code plugins]].
        /// The type of data returned depends on the underlying graphics API: ID3D11Buffer on D3D11,
        /// ID3D12Resource on D3D12, buffer "name" (as GLuint) on OpenGL/ES, MTLBuffer on Metal.
        /// Note that calling this function when using multi-threaded rendering will synchronize with the rendering
        /// thread (a slow operation), so best practice is to set up the necessary buffer pointer only at initialization
        /// time.
        /// SA: [[wiki:NativePluginInterface|Native code plugins]].
        /// </description>
        [FreeFunction(Name = "GraphicsBuffer_Bindings::InternalGetNativeBufferPtr", HasExplicitThis = true)]
        extern public IntPtr GetNativeBufferPtr();
    }
}
