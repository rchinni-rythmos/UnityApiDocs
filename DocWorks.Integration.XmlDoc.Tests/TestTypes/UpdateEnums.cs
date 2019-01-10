using System;

namespace UnityEditor
{
    // Imported texture format for [[TextureImporter]].
    public enum TextureImporterFormat
    {
        Automatic = -1,
#if Win64
        // Choose a compressed format automatically.
		[System.Obsolete("Use textureCompression property instead")]
        AutomaticCompressed = -1,
#endif
#if MacOs
        // Choose a 16 bit format automatically.
		[System.Obsolete("Use textureCompression property instead")]
        Automatic16bit = -2,
#endif
    }
}