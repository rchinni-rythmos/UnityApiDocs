using System;
using UnityEditor;

namespace UnityEditor.Experimental.Animations
{
    public class GameObjectRecorder : Object
    {
        [Obsolete("\"BindComponent<T>() where T : Component\" is obsolete, use BindComponentsOfType<T>() instead (UnityUpgradable) -> BindComponentsOfType<T>(*)", true)]
        public void BindComponent<T>() { }
    }
    }