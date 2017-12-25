using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// BillboardAsset describes how a billboard is rendered.
    /// </summary>
    public class BillboardAsset : Object
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BillboardAsset()
        {

        }

        /// <summary>
        /// Get the indices of the billboard mesh
        /// </summary>
        /// <Description>Billboard meshes are always made of triangles. Specify the index of each vertex (in the vertices array) for each triangle. The second overload guarantees no memory allocation happening if the list capacity is big enough to hold the data.</Description>
        /// <returns>Index numbers</returns>
        public ushort[] GetIndices()
        {
            return null;
        }
    }
}
