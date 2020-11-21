using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
//using UnityEditor;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Fiftytwo
{
    public static class ComponentTools
    {
        public static string HierarchyPath ( this Component component, bool isRelative = false )
        {
            return component.transform.HierarchyPath( isRelative );
        }

        public static void HierarchyPath ( this Component component, StringBuilder sb, bool isRelative = false )
        {
            component.transform.HierarchyPath( sb, isRelative );
        }

        public static string HierarchyPathWithMe ( this Component component, bool isRelative = false )
        {
            var sb = new StringBuilder();
            HierarchyPathWithMe( component, sb, isRelative );
            return sb.ToString();
        }

        public static void HierarchyPathWithMe ( this Component component, StringBuilder sb, bool isRelative = false )
        {
            if ( component == null )
            {
                sb.Append( "(null)" );
                return;
            }
            component.transform.HierarchyPath( sb, isRelative );
            sb.Append( " (" );
            sb.Append( component.GetType().ToString() );
            sb.Append( ")" );
        }

        public static string HierarchyPathReverse ( this Component component, bool isRelative = false )
        {
            return component.transform.HierarchyPathReverse( isRelative );
        }

        public static string HierarchyPathWithMeReverse ( this Component component, bool isRelative = false )
        {
            if ( component == null )
                return "(null)";
            return component.transform.HierarchyPathReverse( isRelative ) + " (" + component.GetType().ToString() + ")";
        }

        public static void HierarchyToList ( this Component component, List<string> pathComponents )
        {
            component.transform.HierarchyToList( pathComponents );
        }
    }
}
