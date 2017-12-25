using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine.Playables
{
    /// <summary>
    /// Status of a Playable
    /// </summary>
    public enum PlayState
    {
        /// <summary>
        /// The Playable has been paused. Its local time will not advance.
        /// </summary>
        Paused,
        /// <summary>
        /// The Playable is currently Playing.
        /// </summary>
        Playing,
        /// <summary>
        /// The Playable has been delayed, using PlayableExtensions.SetDelay. It will not start until the delay is entirely consumed.
        /// </summary>
        Delayed
    }
}
