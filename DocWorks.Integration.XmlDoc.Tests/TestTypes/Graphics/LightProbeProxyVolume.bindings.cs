using System;
using UnityEngine.Bindings;

namespace UnityEngine
{
    [NativeHeader("Runtime/Camera/LightProbeProxyVolume.h")]
    public sealed partial class LightProbeProxyVolume : Behaviour
    {
        /// <summary>
        /// Checks if Light Probe Proxy Volumes are supported.
        /// </summary>
        /// <description>
        /// The value depends on graphics hardware and graphics API used. LPPV requires at least __Shader Model 4__, including TextureFormat.RGBAFloat 3D texture support with linear filtering.
        /// </description>
        public static extern bool isFeatureSupported {[NativeName("IsFeatureSupported")] get; }

        /// <summary>
        /// The world-space bounding box in which the 3D grid of interpolated Light Probes is generated.
        /// </summary>
        [NativeName("GlobalAABB")]
        public extern Bounds boundsGlobal { get; }

        /// <summary>
        /// The size of the bounding box in which the 3D grid of interpolated Light Probes is generated.
        /// </summary>
        /// <description>
        /// This is used when the boundingBoxMode property is set to __Custom__.
        /// </description>
        [NativeName("BoundingBoxSizeCustom")]
        public extern Vector3 sizeCustom { get; set; }

        /// <summary>
        /// The local-space origin of the bounding box in which the 3D grid of interpolated Light Probes is generated.
        /// </summary>
        /// <description>
        /// This is used when the boundingBoxMode property is set to __Custom__.
        /// </description>
        [NativeName("BoundingBoxOriginCustom")]
        public extern Vector3 originCustom { get; set; }

        /// <summary>
        /// Interpolated Light Probe density.
        /// </summary>
        /// <description>
        /// This value is used only when the resolutionMode is __Automatic__.
        /// </description>
        public extern float probeDensity { get; set; }

        /// <summary>
        /// The 3D grid resolution on the z-axis.
        /// </summary>
        /// <description>
        /// This property is used only when the resolutionMode is set to __Custom__. The final resolution is the closest power of 2.
        /// </description>
        public extern int gridResolutionX { get; set; }

        /// <summary>
        /// The 3D grid resolution on the y-axis.
        /// </summary>
        /// <description>
        /// This property is used only when the resolutionMode is set to __Custom__. The final resolution is the closest power of 2.
        /// </description>
        public extern int gridResolutionY { get; set; }

        /// <summary>
        /// The 3D grid resolution on the z-axis.
        /// </summary>
        /// <description>
        /// This property is used only when the resolutionMode is set to __Custom__. The final resolution will be the closest power of 2.
        /// </description>
        public extern int gridResolutionZ { get; set; }

        /// <summary>
        /// The bounding box mode for generating the 3D grid of interpolated Light Probes.
        /// </summary>
        public extern LightProbeProxyVolume.BoundingBoxMode boundingBoxMode { get; set; }

        /// <summary>
        /// The resolution mode for generating the grid of interpolated Light Probes.
        /// </summary>
        public extern LightProbeProxyVolume.ResolutionMode resolutionMode { get; set; }

        /// <summary>
        /// The mode in which the interpolated Light Probe positions are generated.
        /// </summary>
        public extern LightProbeProxyVolume.ProbePositionMode probePositionMode { get; set; }

        /// <summary>
        /// Sets the way the Light Probe Proxy Volume refreshes.
        /// </summary>
        /// <description>
        /// SA: [[LightProbeProxyVolume.RefreshMode]].
        /// </description>
        public extern LightProbeProxyVolume.RefreshMode refreshMode { get; set; }

        /// <summary>
        /// Determines how many Spherical Harmonics bands will be evaluated to compute the ambient color.
        /// </summary>
        /// <description>
        /// SA: [[wiki:LightProbes|Light Probes]], [[LightProbeProxyVolume|Light Probe Proxy Volume]],  [[SphericalHarmonicsL2|Spherical Harmonics(SH)]].
        /// </description>
        public extern LightProbeProxyVolume.QualityMode qualityMode { get; set; }

        /// <summary>
        /// Triggers an update of the Light Probe Proxy Volume.
        /// </summary>
        /// <description>
        /// SA: [[LightProbeProxyVolume]], [[LightProbeProxyVolume.RefreshMode]].
        /// </description>
        public void Update()
        {
            SetDirtyFlag(true);
        }

        private extern void SetDirtyFlag(bool flag);
    }
}
