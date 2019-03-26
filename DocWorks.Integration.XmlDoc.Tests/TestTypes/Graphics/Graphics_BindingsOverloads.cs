using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Scripting;
using UnityEngine.Bindings;
using uei = UnityEngine.Internal;
using RBLoadAction = UnityEngine.Rendering.RenderBufferLoadAction;
using RBStoreAction = UnityEngine.Rendering.RenderBufferStoreAction;
using LightProbeUsage = UnityEngine.Rendering.LightProbeUsage;
using ShadowCastingMode = UnityEngine.Rendering.ShadowCastingMode;

// old bindings were creating method overloads for default args. we MUST keep it this way for backwards compatibility.
// alas we have some methods with insane amount of args which result in mindless clutter of source files
// so just move it there

namespace UnityEngine
{
    public partial class Graphics
    {
        #region public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera = null, int submeshIndex = 0, MaterialPropertyBlock properties = null, bool castShadows = true, bool receiveShadows = true, bool useLightProbes = true)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, null, 0, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, 0, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows, bool receiveShadows)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows, null, LightProbeUsage.BlendProbes, null);
        }

        #endregion

        #region public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows = true, Transform probeAnchor = null, bool useLightProbes = true)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor)
        {
            DrawMesh(mesh, Matrix4x4.TRS(position, rotation, Vector3.one), material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, LightProbeUsage.BlendProbes, null);
        }

        #endregion

        #region public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera = null, int submeshIndex = 0, MaterialPropertyBlock properties = null, bool castShadows = true, bool receiveShadows = true, bool useLightProbes = true)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer)
        {
            DrawMesh(mesh, matrix, material, layer, null, 0, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera)
        {
            DrawMesh(mesh, matrix, material, layer, camera, 0, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, null, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, ShadowCastingMode.On, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, bool castShadows, bool receiveShadows)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows ? ShadowCastingMode.On : ShadowCastingMode.Off, receiveShadows, null, LightProbeUsage.BlendProbes, null);
        }

        #endregion

        #region public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows = true, Transform probeAnchor = null, bool useLightProbes = true)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, true, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, LightProbeUsage.BlendProbes, null);
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
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, [uei.DefaultValue("true")] bool receiveShadows, [uei.DefaultValue("null")] Transform probeAnchor, [uei.DefaultValue("true")] bool useLightProbes)
        {
            DrawMesh(mesh, matrix, material, layer, camera, submeshIndex, properties, castShadows, receiveShadows, probeAnchor, useLightProbes ? LightProbeUsage.BlendProbes : LightProbeUsage.Off, null);
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
        [uei.ExcludeFromDocs]
        public static void DrawMesh(Mesh mesh, Matrix4x4 matrix, Material material, int layer, Camera camera, int submeshIndex, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, Transform probeAnchor, LightProbeUsage lightProbeUsage)
        {
            Internal_DrawMesh(mesh, submeshIndex, matrix, material, layer, camera, properties, castShadows, receiveShadows, probeAnchor, lightProbeUsage, null);
        }

        #endregion

        #region public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count = matrices.Length, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0, Camera camera = null, LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes, LightProbeProxyVolume lightProbeProxyVolume = null)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, matrices.Length, null, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, null, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, Matrix4x4[] matrices, int count, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, count, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, null);
        }

        #endregion

        // TODO: Migrate these dreadful overloads to default arguments.
        #region public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0, Camera camera = null, LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes, LightProbeyProxyVolume lightProbeProxyVolume = null)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, null, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstanced(Mesh mesh, int submeshIndex, Material material, List<Matrix4x4> matrices, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage)
        {
            DrawMeshInstanced(mesh, submeshIndex, material, matrices, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, null);
        }

        #endregion

        #region public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset = 0, MaterialPropertyBlock properties = null, ShadowCastingMode castShadows = ShadowCastingMode.On, bool receiveShadows = true, int layer = 0, Camera camera = null, LightProbeUsage lightProbeUsage = LightProbeUsage.BlendProbes, LightProbeProxyVolume lightProbeProxyVolume = null)
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, 0, null, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, null, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, ShadowCastingMode.On, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, true, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, 0, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, null, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, LightProbeUsage.BlendProbes, null);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawMeshInstancedIndirect(Mesh mesh, int submeshIndex, Material material, Bounds bounds, ComputeBuffer bufferWithArgs, int argsOffset, MaterialPropertyBlock properties, ShadowCastingMode castShadows, bool receiveShadows, int layer, Camera camera, LightProbeUsage lightProbeUsage)
        {
            DrawMeshInstancedIndirect(mesh, submeshIndex, material, bounds, bufferWithArgs, argsOffset, properties, castShadows, receiveShadows, layer, camera, lightProbeUsage, null);
        }

        #endregion

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
        /// <description>
        /// If you want to draw a texture from inside of OnGUI code, you should only do that from [[EventType.Repaint]]
        /// events. It's probably better to use [[GUI.DrawTexture]] for GUI code.
        /// </description>
        [uei.ExcludeFromDocs]
        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color, Material mat)
        {
            DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, mat, -1);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Color color)
        {
            DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, color, null, -1);
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
        /// <description>
        /// If you want to draw a texture from inside of OnGUI code, you should only do that from [[EventType.Repaint]]
        /// events. It's probably better to use [[GUI.DrawTexture]] for GUI code.
        /// </description>
        [uei.ExcludeFromDocs]
        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat)
        {
            DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, mat, -1);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawTexture(Rect screenRect, Texture texture, Rect sourceRect, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
        {
            DrawTexture(screenRect, texture, sourceRect, leftBorder, rightBorder, topBorder, bottomBorder, null, -1);
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
        /// <description>
        /// If you want to draw a texture from inside of OnGUI code, you should only do that from [[EventType.Repaint]]
        /// events. It's probably better to use [[GUI.DrawTexture]] for GUI code.
        /// </description>
        [uei.ExcludeFromDocs]
        public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, Material mat)
        {
            DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat, -1);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawTexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
        {
            DrawTexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, null, -1);
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
        /// <description>
        /// If you want to draw a texture from inside of OnGUI code, you should only do that from [[EventType.Repaint]]
        /// events. It's probably better to use [[GUI.DrawTexture]] for GUI code.
        /// </description>
        [uei.ExcludeFromDocs]
        public static void DrawTexture(Rect screenRect, Texture texture, Material mat)
        {
            DrawTexture(screenRect, texture, mat, -1);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void DrawTexture(Rect screenRect, Texture texture)
        {
            DrawTexture(screenRect, texture, null, -1);
        }
    }

    public partial class Graphics
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void SetRenderTarget(RenderTexture rt)
        {
            SetRenderTarget(rt, 0, CubemapFace.Unknown, 0);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void SetRenderTarget(RenderTexture rt, int mipLevel)
        {
            SetRenderTarget(rt, mipLevel, CubemapFace.Unknown, 0);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void SetRenderTarget(RenderTexture rt, int mipLevel, CubemapFace face)
        {
            SetRenderTarget(rt, mipLevel, face, 0);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer)
        {
            SetRenderTarget(colorBuffer, depthBuffer, 0, CubemapFace.Unknown, 0);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel)
        {
            SetRenderTarget(colorBuffer, depthBuffer, mipLevel, CubemapFace.Unknown, 0);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void SetRenderTarget(RenderBuffer colorBuffer, RenderBuffer depthBuffer, int mipLevel, CubemapFace face)
        {
            SetRenderTarget(colorBuffer, depthBuffer, mipLevel, face, 0);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        [uei.ExcludeFromDocs]
        public static void SetRandomWriteTarget(int index, ComputeBuffer uav)
        {
            SetRandomWriteTarget(index, uav, false);
        }
    }
}
