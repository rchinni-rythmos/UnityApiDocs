using System;
using System.Collections.Generic;
using System.Text;

namespace UnityEngine
{
    /// <summary>
    /// Base class for all objects Unity can reference.
    /// </summary>
    /// <Description>
    /// Any public variable you make that derives from Object gets shown in the inspector as a drop target, allowing you to set the value from the GUI.
    /// </Description>
    public partial class Object
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Object()
        {

        }

        /// <summary>
        /// Should the object be hidden, saved with the scene or modifiable by the user?
        /// </summary>
        /// <Description>
        /// </Description>
        /// <example>
        /// See the code sample to understand how to use hideFlags
        /// <code>
        /// <![CDATA[
        /// using UnityEngine;
        /// using System.Collections;
        ///
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///
        ///         // Creates a material that is explicitly created & destroyed by the component.
        ///         // Resources.UnloadUnusedAssets will not unload it, and it will not be editable by the inspector.
        ///         private Material ownedMaterial;
        ///         void OnEnable()
        ///         {
        ///             ownedMaterial = new Material(Shader.Find("Diffuse"));
        ///             ownedMaterial.hideFlags = HideFlags.HideAndDontSave;
        ///             GetComponent<Renderer>().sharedMaterial = ownedMaterial;
        ///         }
        ///
        ///         // Objects created as hide and don't save must be explicitly destroyed by the owner of the object.
        ///         void OnDisable()
        ///         {
        ///             DestroyImmediate(ownedMaterial);
        ///         }
        /// }
        /// ]]>
        /// </code>
        /// </example>
        public bool hideFlags { get; set; }

        /// <summary>
        /// Returns the instance id of the object.
        /// </summary>
        /// <Description>
        /// The instance id of an object is always guaranteed to be unique.
        /// </Description>
        /// <returns></returns>
        /// <example>
        /// <code>
        /// using UnityEngine;
        /// using System.Collections;
        ///
        /// public class ExampleClass : MonoBehaviour
        /// {
        ///     void Example()
        ///     {
        ///         print(GetInstanceID());
        ///     }
        /// }
        /// </code>
        /// </example>
        /// <returns>The instanceId of the current object</returns>
        public int GetInstanceID()
        {
            return 0;
        }


        /// <summary>
        /// This is a Protected method and can be used only by derived types
        /// </summary>
        /// <returns>An integer</returns>
        protected int SampleProtectedMethod()
        {
            return 0;
        }

        /// <summary>
        /// This is an obsolete method
        /// </summary>
        [Obsolete]
        public void SampleObsoleteMethod()
        {

        }

        public void SampleMethodWithNoDocumentation()
        {

        }

        /// <summary>
        /// This is a Static Method
        /// </summary>
        public static void SampleStaticMethod()
        {

        }

        /// <summary>
        /// Documentation for this method shouldn't be in the output, as this is a private method
        /// </summary>
        private void SamplePrivateMethod()
        {

        }

        /// <summary>
        /// This PUBLIC method shouldnt be in the output. Contains undoc tag.
        /// Assumption: Only developers (and not authors) can decide if a member can or cannot be documented
        /// </summary>
        /// <undoc />
        public void SampleMethodNotDocumented()
        {

        }

        /// <summary>
        /// This is a delegate. 
        /// </summary>
        /// <param name="s">The input string</param>
        /// <returns>Returns the calculated integer output</returns>
        public delegate int MyDelegate(string s);

        /// <summary>
        /// This is Overloaded Method sample 1
        /// </summary>
        public void SampleOverloadMethod()
        {

        }

        /// <summary>
        /// This is Overloaded Method sample 2
        /// </summary>
        /// <param name="i">This is first parameter</param>
        public void SampleOverloadMethod(int i)
        {

        }

        /// <summary>
        /// This is Overloaded Method sample 3
        /// Note: The second parameter contains angle brackets. So it cannot be carried as it is to the "Name" attribute of the Member tag. 
        /// CData is not valid for an XML attribute.
        /// So angle brackets are to be replaced with flower brackets.
        /// </summary>
        /// <param name="i">First parameter integer</param>
        /// <param name="ls">Second parameter - List of strings</param>
        public void SampleOverloadMethod(int i, List<string> ls)
        {

        }
    }
}
