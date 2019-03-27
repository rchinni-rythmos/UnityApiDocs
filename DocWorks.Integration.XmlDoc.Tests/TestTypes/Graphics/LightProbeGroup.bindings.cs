using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
    [NativeHeader("Runtime/Graphics/LightProbeGroup.h")]
    public sealed partial class LightProbeGroup : Behaviour
    {
#if UNITY_EDITOR
        [NativeName("Positions")]
        public extern Vector3[] probePositions { get; set; }
        [NativeName("Dering")]
        public extern bool dering { get; set; }
#else
        public Vector3[] probePositions { get { return null; } }
#endif
    }
}
