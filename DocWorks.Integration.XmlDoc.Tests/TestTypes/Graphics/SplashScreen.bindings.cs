using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
    /// <summary>
    /// Provides an interface to the Unity splash screen.
    /// </summary>
    [NativeHeader("Runtime/Graphics/DrawSplashScreenAndWatermarks.h")]
    public class SplashScreen
    {
        /// <summary>
        /// Returns true once the splash screen has finished. This is once all logos have been shown for their specified duration.
        /// </summary>
        extern public static bool isFinished {[FreeFunction("IsSplashScreenFinished")] get; }

        /// <summary>
        /// Initializes the splash screen so it is ready to begin drawing. Call this before you start calling [[Rendering.SplashScreen.Draw]]. Internally this function resets the timer and prepares the logos for drawing.
        /// </summary>
        [FreeFunction("BeginSplashScreen_Binding")]
        extern public static void Begin();

        /// <summary>
        /// Immediately draws the splash screen. Ensure you have called [[Rendering.SplashScreen.Begin]] before you start calling this.
        /// </summary>
        [FreeFunction("DrawSplashScreen_Binding")]
        extern public static void Draw();
    }
} // namespace UnityEngine.Rendering
