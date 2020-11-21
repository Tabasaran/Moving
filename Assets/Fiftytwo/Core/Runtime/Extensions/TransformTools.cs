using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fiftytwo
{
    public static class TransformTools
    {
        public static Transform[] GetChildren ( this Transform transform )
        {
            var children = new Transform[transform.childCount];

            for( var i = 0; i < transform.childCount; i++ )
            {
                children[i] = transform.GetChild( i );
            }

            return children;
        }

        public static void GetChildrenRecursively ( this Transform transform, List<Transform> children )
        {
            children.Clear();
            GetChildrenRecursivelyInternal( transform, children );
        }

        private static void GetChildrenRecursivelyInternal ( Transform transform, List<Transform> children )
        {
            for( var i = 0; i < transform.childCount; i++ )
            {
                var child = transform.GetChild( i );
                children.Add( child );
                if( child.childCount > 0 )
                    GetChildrenRecursivelyInternal( child, children );
            }
        }

        public static string HierarchyPath ( this Transform transform, bool isRelative = false )
        {
            var sb = new StringBuilder();
            HierarchyPath( transform, sb, isRelative );
            return sb.ToString();
        }

        public static void HierarchyPath ( this Transform transform, StringBuilder sb, bool isRelative = false )
        {
            if( transform == null )
            {
                sb.Append( "(null)" );
                return;
            }

            var parent = transform.parent;
            if( parent != null )
                HierarchyPath( parent, sb, isRelative );
            else if( !isRelative )
            {
                var scene = transform.gameObject.scene;
                if( scene.IsValid() )
                    sb.Append( scene.path );
#if UNITY_EDITOR
                else if( UnityEditor.EditorUtility.IsPersistent( transform.gameObject ) )
                    sb.Append( UnityEditor.AssetDatabase.GetAssetPath( transform.gameObject ) );
#endif
            }
            sb.Append( "/" );
            sb.Append( transform.name );
        }

        public static string HierarchyPathReverse ( this Transform transform, bool isRelative = false )
        {
            var sb = new StringBuilder();
            HierarchyPathReverse( transform, sb, isRelative );
            return sb.ToString();
        }

        public static void HierarchyPathReverse ( this Transform transform, StringBuilder sb, bool isRelative = false )
        {
            if( transform == null )
            {
                sb.Append( "(null)" );
                return;
            }

            while( true )
            {
                sb.Append( transform.name );
                sb.Append( "/" );

                var parent = transform.parent;
                if( parent == null )
                {
                    if( !isRelative )
                    {
                        var scene = transform.gameObject.scene;
                        if( scene.IsValid() )
                            sb.Append( scene.path );
#if UNITY_EDITOR
                        else if( UnityEditor.EditorUtility.IsPersistent( transform.gameObject ) )
                            sb.Append( UnityEditor.AssetDatabase.GetAssetPath( transform.gameObject ) );
#endif
                    }

                    break;
                }

                transform = parent;
            }
        }

        public static void HierarchyToList ( this Transform transform, List<string> pathComponents )
        {
            pathComponents.Clear();

            while( transform != null )
            {
                pathComponents.Add( transform.name );
                transform = transform.parent;
            }
        }

        public static void SetX ( this Transform transform, float x )
        {
            var position = transform.position;
            transform.position = new Vector3( x, position.y, position.z );
        }

        public static void SetY ( this Transform transform, float y )
        {
            var position = transform.position;
            transform.position = new Vector3( position.x, y, position.z );;
        }

        public static void SetZ ( this Transform transform, float z )
        {
            var position = transform.position;
            transform.position = new Vector3( position.x, position.y, z );
        }
    }
}
