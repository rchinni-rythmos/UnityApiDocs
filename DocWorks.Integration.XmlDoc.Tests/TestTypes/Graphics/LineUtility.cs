using System;
using System.Collections.Generic;

namespace UnityEngine
{
    /// <summary>
    /// A collection of common line functions.
    /// </summary>
    public partial class LineUtility
    {
        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void Simplify(List<Vector3> points, float tolerance, List<int> pointsToKeep)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            if (pointsToKeep == null)
                throw new ArgumentNullException("pointsToKeep");

            GeneratePointsToKeep3D(points, tolerance, pointsToKeep);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void Simplify(List<Vector3> points, float tolerance, List<Vector3> simplifiedPoints)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            if (simplifiedPoints == null)
                throw new ArgumentNullException("simplifiedPoints");

            GenerateSimplifiedPoints3D(points, tolerance, simplifiedPoints);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void Simplify(List<Vector2> points, float tolerance, List<int> pointsToKeep)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            if (pointsToKeep == null)
                throw new ArgumentNullException("pointsToKeep");

            GeneratePointsToKeep2D(points, tolerance, pointsToKeep);
        }

        /// <summary>
        /// There is currently no documentation for this api.
        /// </summary>
        public static void Simplify(List<Vector2> points, float tolerance, List<Vector2> simplifiedPoints)
        {
            if (points == null)
                throw new ArgumentNullException("points");
            if (simplifiedPoints == null)
                throw new ArgumentNullException("simplifiedPoints");

            GenerateSimplifiedPoints2D(points, tolerance, simplifiedPoints);
        }
    }
}
