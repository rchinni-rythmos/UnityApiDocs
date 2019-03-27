using System;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
    /// <summary>
    /// Script interface for [[wiki:class-GraphicsSettings|Graphics Settings]].
    /// </summary>
    [NativeHeader("Runtime/Camera/GraphicsSettings.h")]
    [StaticAccessor("GetGraphicsSettings()", StaticAccessorType.Dot)]
    public sealed partial class GraphicsSettings : Object
    {
        private GraphicsSettings() {}

        /// <summary>
        /// Transparent object sorting mode.
        /// </summary>
        /// <description>
        /// This is the default [[TransparencySortMode]] that the rendering pipeline uses for all Cameras unless the settings are overridden for a specific Camera.\\
        /// \\
        /// SA: [[TransparencySortMode]] enum, [[Camera.transparencySortMode]].
        /// </description>
        extern public static TransparencySortMode   transparencySortMode { get; set; }
        /// <summary>
        /// An axis that describes the direction along which the distances of objects are measured for the purpose of sorting.
        /// </summary>
        /// <description>
        /// This is the default axis used by the rendering pipeline for sorting in [[TransparencySortMode.CustomAxis]] mode for all Cameras including the SceneView's. You may override this for each invididual Camera by calling [[Camera.transparencySortAxis]] and [[Camera.transparencySortMode]] APIs.\\
        /// \\
        /// SA: [[TransparencySortMode]] enum, [[Camera.transparencySortAxis]], [[Camera.transparencySortMode]].
        /// </description>
        extern public static Vector3                transparencySortAxis { get; set; }
        /// <summary>
        /// Is the current render pipeline capable of rendering direct lighting for rectangular area Lights?
        /// </summary>
        /// <description>
        ///           The current render pipeline can use this property to indicate that it is capable of rendering direct lighting for rectangular area Lights.
        ///           If it is false, direct lighting for rectangular area Lights is assumed to be baked..\\
        /// \\
        ///           SA: [[GraphicsSettings.renderPipelineAsset]].
        /// </description>
        extern public static bool realtimeDirectRectangularAreaLights { get; set; }
        /// <summary>
        /// If this is true, Light intensity is multiplied against linear color values. If it is false, gamma color values are used.
        /// </summary>
        /// <description>
        ///           Light intensity is multiplied by the linear color value when lightsUseLinearIntensity
        ///           is enabled. This is physically correct but not the default for 3D projects created with
        ///           Unity 5.6 and newer.  By default lightsUseLinearIntensity is set to false.\\
        /// \\
        ///           2D projects will have lightsUseLinearIntensity set to disabled by default.
        ///           When disabled, the gamma color value is multiplied with the intensity.
        ///           If you want to use lightsUseColorTemperature, lightsUseLinearIntensity has to be
        ///           enabled to ensure physically correct output.\\
        /// \\
        ///           If you enable lightsUseLinearIntensity on an existing project, all your Lights will
        ///           need to be tweaked to regain their original look.\\
        /// \\
        ///           SA: [[GraphicsSettings.lightsUseColorTemperature]], [[Light.colorTemperature]].
        /// </description>
        extern public static bool lightsUseLinearIntensity   { get; set; }
        /// <summary>
        /// Whether to use a Light's color temperature when calculating the final color of that Light."
        /// </summary>
        /// <description>
        ///           Enable to use the correlated color temperature (abbreviated as CCT) for adjusting light color. CCT is a natural way to set light color based on the physical properties of the light source. The CCT is multiplied with the color filter when calculating the final color of a light source. The color temperature of the electromagnetic radiation emitted from an ideal black body is defined as its surface temperature in degrees Kelvin. White is 6500K according to the D65 standard. Candle light is 1800K.
        ///           If you want to use lightsUseColorTemperature, lightsUseLinearIntensity has to be enabled to ensure physically correct output.\\
        /// \\
        ///           SA: [[GraphicsSettings.lightsUseLinearIntensity]], [[Light.ColorTemperature]].
        /// </description>
        extern public static bool lightsUseColorTemperature  { get; set; }
        /// <summary>
        /// Enable/Disable SRP batcher (experimental) at runtime.
        /// </summary>
        extern public static bool useScriptableRenderPipelineBatching { get; set; }

        /// <summary>
        /// Returns true if shader define was set when compiling shaders for current [[GraphicsTier]].
        /// </summary>
        extern public static bool HasShaderDefine(GraphicsTier tier, BuiltinShaderDefine defineHash);
        /// <summary>
        /// Returns true if shader define was set when compiling shaders for given tier.
        /// </summary>
        public static bool HasShaderDefine(BuiltinShaderDefine defineHash)
        {
            return HasShaderDefine(Graphics.activeTier, defineHash);
        }

        [NativeName("RenderPipeline")] extern private static ScriptableObject INTERNAL_renderPipelineAsset { get; set; }
        /// <summary>
        /// The [[RenderPipelineAsset]] that describes how the Scene should be rendered.
        /// </summary>
        /// <description>
        /// The current [[RenderPipelineAsset]] is used to render Scene, View and Preview Cameras.
        /// </description>
        public static RenderPipelineAsset renderPipelineAsset
        {
            get { return INTERNAL_renderPipelineAsset as RenderPipelineAsset; }
            set { INTERNAL_renderPipelineAsset = value; }
        }

        [FreeFunction] extern internal static Object GetGraphicsSettings();

        /// <summary>
        /// Set built-in shader mode.
        /// </summary>
        /// <param name="type">
        /// Built-in shader type to change.
        /// </param>
        /// <param name="mode">
        /// Mode to use for built-in shader.
        /// </param>
        /// <description>
        /// By default, certain parts of rendering pipeline in Unity (e.g. deferred shading light calculations) use built-in shader.
        /// However, it is possible to setup a custom shader to replace the built-in functionality, or to turn off support for it
        /// altogether.
        /// When setting [[BuiltinShaderMode.UseCustom]], you also need to indicate which shader to use. See SetCustomShader.\\
        /// \\
        /// SA: GetShaderMode, SetCustomShader, [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        [NativeName("SetShaderModeScript")]   extern static public void                 SetShaderMode(BuiltinShaderType type, BuiltinShaderMode mode);
        /// <summary>
        /// Get built-in shader mode.
        /// </summary>
        /// <param name="type">
        /// Built-in shader type to query.
        /// </param>
        /// <returns>
        /// Mode used for built-in shader.
        /// </returns>
        /// <description>
        /// SA: SetShaderMode, GetCustomShader, [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        [NativeName("GetShaderModeScript")]   extern static public BuiltinShaderMode    GetShaderMode(BuiltinShaderType type);

        /// <summary>
        /// Set custom shader to use instead of a built-in shader.
        /// </summary>
        /// <param name="type">
        /// Built-in shader type to set custom shader to.
        /// </param>
        /// <param name="shader">
        /// The shader to use.
        /// </param>
        /// <description>
        /// This is only used when built-in shader is set to [[BuiltinShaderMode.UseCustom]] mode; see SetShaderMode.\\
        /// \\
        /// SA: GetCustomShader, SetShaderMode, [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        [NativeName("SetCustomShaderScript")] extern static public void     SetCustomShader(BuiltinShaderType type, Shader shader);
        /// <summary>
        /// Get custom shader used instead of a built-in shader.
        /// </summary>
        /// <param name="type">
        /// Built-in shader type to query custom shader for.
        /// </param>
        /// <returns>
        /// The shader used.
        /// </returns>
        /// <description>
        /// SA: SetCustomShader, GetShaderMode, [[wiki:class-GraphicsSettings|Graphics Settings]].
        /// </description>
        [NativeName("GetCustomShaderScript")] extern static public Shader   GetCustomShader(BuiltinShaderType type);
    }
}
