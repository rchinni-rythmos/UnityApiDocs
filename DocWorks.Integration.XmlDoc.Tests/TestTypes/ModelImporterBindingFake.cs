using scm = System.ComponentModel;
using System.Runtime.InteropServices;
using System;


namespace UnityEditor
{
    public partial struct ClipAnimationInfoCurve
    {
        public string name;
    }
    public partial struct TakeInfo
    {
        public string name;
        public string defaultClipName;
        public float startTime;
        public float stopTime;
        public float bakeStartTime;
        public float bakeStopTime;
        public float sampleRate;
    }
}