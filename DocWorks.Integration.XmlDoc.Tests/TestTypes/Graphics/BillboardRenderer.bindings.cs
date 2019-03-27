using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Bindings;

namespace UnityEngine
{
    /// <summary>
        /// BillboardAsset describes how a billboard is rendered.
        /// </summary>
        /// <description>
        /// Billboards are a level-of-detail (LOD) method of drawing complicated 3D objects in a simpler manner if they are further away. Because the object is further away, there is often less requirement to draw the object at full detail due to its size on-screen and low likelihood of being a focal point in the Camera view. Instead, the object can be pre-rendered to a texture, and this texture used on a very simple Camera-facing Mesh of flat geometry (often simply a quadrilateral) known as a billboard. At great distances an object does not significantly change its orientation relative to the camera, so a billboard looks much like the object it represents from frame to frame, without having to be redrawn from the model. The BillboardAsset class allows the creation of billboards that are rendered from several directions, allowing a BillboardAsset to efficiently represent an object at low level of detail from any approximately-horizontal viewpoint.
        /// A BillboardAsset is usually created by importing SpeedTree assets. You can also create your own once you know how the billboard is described.
        /// SpeedTree billboard geometry is usually more complex than a plain quadrilateral. By using more vertices to cut out the empty part of the billboard image, rendering performance can potentially be improved due to the graphics system not having to draw as many redundant transparent pixels. You have access to the geometry data via [[BillboardAsset.vertices]] and [[BillboardAsset.indices]].
        /// All vertices are considered in UV-space (see Fig. 1 below), because the geometry is due to be textured by the billboard images. UV vertices are easily expanded to 3D-space vertices by knowing the billboard's width, height, bottom, and what direction the billboard is currently facing. Assuming we have a billboard located at (0,0,0) looking at negative Z axis, the 3D-space coordinates are calculated as:
        /// ''X'' = (''u'' - 0.5) * ''width''\\
        /// ''Y'' = ''v'' * ''height'' + ''bottom''\\
        /// ''Z'' = 0
        /// ![Billboard_Geometry](Billboard_Geometry.png)
        /// Figure 1: How UV vertices are expanded to 3D vertices
        /// In order to display something similar to the real 3D mesh being billboarded, SpeedTree billboards select an appropriate image from several pre-rendered images according to the current view direction. The images in Figure 2 below are created by capturing the rendered image of the 3D tree at different view angles, evenly distributed around the Y-axis. The first image always starts at positive X axis direction (or 0° if you imagine a unit circle from above).
        /// ![Billboard_Images](Billboard_Images.png)
        /// Figure 2: How the eight billboard images are baked
        /// All images should be atlased together in one single texture. Each image is represented as a [[Vector4]] of {''left u'', ''top v'', ''width in u'', ''height in v''} in the atlas. You have access to all the images via [[BillboardAsset.imageTexCoords]]. SpeedTree Modeler always exports a normal texture alongside the diffuse texture for even better approximation of the lighting, and it shares the same atlas layout with the diffuse texutre.
        /// Once the BillboardAsset is constructed, use [[BillboardRenderer]] to render it.
        /// </description>
            [NativeHeader("Runtime/Graphics/Billboard/BillboardAsset.h")]
    [NativeHeader("Runtime/Export/Graphics/BillboardRenderer.bindings.h")]
    public sealed class BillboardAsset : Object
    {
        /// <summary>
        /// Constructs a new BillboardAsset.
        /// </summary>
        public BillboardAsset()
        {
            Internal_Create(this);
        }

        [FreeFunction(Name = "BillboardRenderer_Bindings::Internal_Create")]
        extern private static void Internal_Create([Writable] BillboardAsset obj);

        /// <summary>
        /// Width of the billboard.
        /// </summary>
        extern public float width { get; set; }
        /// <summary>
        /// Height of the billboard.
        /// </summary>
        extern public float height { get; set; }
        /// <summary>
        /// Height of the billboard that is below ground.
        /// </summary>
        extern public float bottom { get; set; }

        /// <summary>
        /// Number of pre-rendered images that can be switched when the billboard is viewed from different angles.
        /// </summary>
        /// <description>
        /// SA: GetImageTexCoords, SetImageTexCoords.
        /// </description>
        extern public int imageCount
        {
            [NativeMethod("GetNumImages")]
            get;
        }

        /// <summary>
        /// Number of vertices in the billboard mesh.
        /// </summary>
        /// <description>
        /// SA: GetVertices, SetVertices.
        /// </description>
        extern public int vertexCount
        {
            [NativeMethod("GetNumVertices")]
            get;
        }

        /// <summary>
        /// Number of indices in the billboard mesh.
        /// </summary>
        /// <description>
        /// SA: GetIndices, SetIndices.
        /// </description>
        extern public int indexCount
        {
            [NativeMethod("GetNumIndices")]
            get;
        }

        /// <summary>
        /// The material used for rendering.
        /// </summary>
        extern public Material material { get; set; }

        // List<T> version
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetImageTexCoords(List<Vector4> imageTexCoords)
        {
            if (imageTexCoords == null)
                throw new ArgumentNullException("imageTexCoords");

            GetImageTexCoordsInternal(imageTexCoords);
        }

        // T[] version
        /// <summary>
        /// Get the array of billboard image texture coordinate data.
        /// </summary>
        /// <description>
        /// Each element in the array represents a rectangular UV area of the texture. The second overload guarantees no memory allocation happening if the list capacity is big enough to hold the data.
        /// SA: [[BillboardAsset]], SetImageTexCoords.
        /// </description>
        [NativeMethod("GetBillboardDataReadonly().GetImageTexCoords")]
        extern public Vector4[] GetImageTexCoords();

        [FreeFunction(Name = "BillboardRenderer_Bindings::GetImageTexCoordsInternal", HasExplicitThis = true)]
        extern internal void GetImageTexCoordsInternal(object list);

        // List<T> version
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetImageTexCoords(List<Vector4> imageTexCoords)
        {
            if (imageTexCoords == null)
                throw new ArgumentNullException("imageTexCoords");

            SetImageTexCoordsInternalList(imageTexCoords);
        }

        // T[] version
        /// <summary>
        /// Set the array of billboard image texture coordinate data.
        /// </summary>
        /// <param name="imageTexCoords">
        /// The array of data to set.
        /// </param>
        /// <description>
        /// SA: [[BillboardAsset]], GetImageTexCoords.
        /// </description>
        [FreeFunction(Name = "BillboardRenderer_Bindings::SetImageTexCoords", HasExplicitThis = true)]
        extern public void SetImageTexCoords([NotNull] Vector4[] imageTexCoords);

        [FreeFunction(Name = "BillboardRenderer_Bindings::SetImageTexCoordsInternalList", HasExplicitThis = true)]
        extern internal void SetImageTexCoordsInternalList(object list);

        // List<T> version
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void GetVertices(List<Vector2> vertices)
        {
            if (vertices == null)
                throw new ArgumentNullException("vertices");

            GetVerticesInternal(vertices);
        }

        // T[] version
        /// <summary>
        /// Get the vertices of the billboard mesh.
        /// </summary>
        /// <description>
        /// Each vertex is a [[Vector2]] in UV space. The second overload guarantees no memory allocation happening if the list capacity is big enough to hold the data.
        /// SA: [[BillboardAsset]], SetVertices.
        /// </description>
        [NativeMethod("GetBillboardDataReadonly().GetVertices")]
        extern public Vector2[] GetVertices();

        [FreeFunction(Name = "BillboardRenderer_Bindings::GetVerticesInternal", HasExplicitThis = true)]
        extern internal void GetVerticesInternal(object list);

        // List<T> version
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public void SetVertices(List<Vector2> vertices)
        {
            if (vertices == null)
                throw new ArgumentNullException("vertices");

            SetVerticesInternalList(vertices);
        }

        // T[] version
        /// <summary>
        /// Set the vertices of the billboard mesh.
        /// </summary>
        /// <param name="vertices">
        /// The array of data to set.
        /// </param>
        /// <description>
        /// SA: [[BillboardAsset]], GetVertices.
        /// </description>
        [FreeFunction(Name = "BillboardRenderer_Bindings::SetVertices", HasExplicitThis = true)]
        extern public void SetVertices([NotNull] Vector2[] vertices);

        [FreeFunction(Name = "BillboardRenderer_Bindings::SetVerticesInternalList", HasExplicitThis = true)]
        extern internal void SetVerticesInternalList(object list);

        // List<T> version
        /// <summary>
        /// Get the indices of the billboard mesh.
        /// </summary>
        /// <param name="indices">
        /// The list that receives the array.
        /// </param>
        /// <description>
        /// Billboard meshes are always made of triangles. Specify the index of each vertex (in the vertices array) for each triangle. The second overload guarantees no memory allocation happening if the list capacity is big enough to hold the data.
        /// SA: [[BillboardAsset]], SetIndices.
        /// </description>
        public void GetIndices(List<UInt16> indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");

            GetIndicesInternal(indices);
        }

        // T[] version
        /// <summary>
        /// Get the indices of the billboard mesh.
        /// </summary>
        /// <description>
        /// Billboard meshes are always made of triangles. Specify the index of each vertex (in the vertices array) for each triangle. The second overload guarantees no memory allocation happening if the list capacity is big enough to hold the data.
        /// SA: [[BillboardAsset]], SetIndices.
        /// </description>
        [NativeMethod("GetBillboardDataReadonly().GetIndices")]
        extern public UInt16[] GetIndices();

        [FreeFunction(Name = "BillboardRenderer_Bindings::GetIndicesInternal", HasExplicitThis = true)]
        extern internal void GetIndicesInternal(object list);

        // List<T> version
        /// <summary>
        /// Set the indices of the billboard mesh.
        /// </summary>
        /// <param name="indices">
        /// The array of data to set.
        /// </param>
        /// <description>
        /// SA: [[BillboardAsset]], GetIndices.
        /// </description>
        public void SetIndices(List<UInt16> indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");

            SetIndicesInternalList(indices);
        }

        // T[] version
        /// <summary>
        /// Set the indices of the billboard mesh.
        /// </summary>
        /// <param name="indices">
        /// The array of data to set.
        /// </param>
        /// <description>
        /// SA: [[BillboardAsset]], GetIndices.
        /// </description>
        [FreeFunction(Name = "BillboardRenderer_Bindings::SetIndices", HasExplicitThis = true)]
        extern public void SetIndices([NotNull] UInt16[] indices);

        [FreeFunction(Name = "BillboardRenderer_Bindings::SetIndicesInternalList", HasExplicitThis = true)]
        extern internal void SetIndicesInternalList(object list);

        [FreeFunction(Name = "BillboardRenderer_Bindings::MakeMaterialProperties", HasExplicitThis = true)]
        extern internal void MakeMaterialProperties(MaterialPropertyBlock properties, Camera camera);
    }

    /// <summary>
        /// Renders a billboard from a BillboardAsset.
        /// </summary>
        /// <description>
        /// BillboardRenderers that share the same BillboardAsset can be rendered in a batch if they are next to each other in the order of rendering. The batching is always enabled regardless of whether dynamic batching is enabled or not.
        /// Due to the always-upright nature of a tree billboard, BillboardRenderer can only rotate around Y-axis.
        /// SA: [[BillboardAsset]].
        /// </description>
            [NativeHeader("Runtime/Graphics/Billboard/BillboardRenderer.h")]
    public sealed class BillboardRenderer : Renderer
    {
        /// <summary>
        /// The [[BillboardAsset]] to render.
        /// </summary>
        extern public BillboardAsset billboard { get; set; }
    }
}
