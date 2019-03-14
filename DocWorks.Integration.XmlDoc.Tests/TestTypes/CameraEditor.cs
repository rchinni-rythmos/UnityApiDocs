using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;


namespace UnityEditor
{
    public class CameraEditor 
    {
        private static class Styles
        {
            public static string _clearFlags;
        }
        public sealed class Settings
        {
            static readonly Vector2[] k_ApertureFormats =
            {
                new Vector2(4.8f, 3.5f) , // 8mm
                new Vector2(5.79f, 4.01f) , // Super 8mm
                new Vector2(10.26f, 7.49f) , // 16mm
                new Vector2(12.52f, 7.41f) , // Super 16mm
                new Vector2(21.95f, 9.35f) , // 35mm 2-perf
                new Vector2(21.0f, 15.2f) , // 35mm academy
                new Vector2(24.89f, 18.66f) , // Super-35
                new Vector2(54.12f, 25.59f) , // 65mm ALEXA
                new Vector2(70.0f, 51.0f) , // 70mm
                new Vector2(70.41f, 52.63f), // 70mm IMAX
            };



            public string _clearFlags { get; private set; }
           
        }
    }
}
