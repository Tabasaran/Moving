using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Fiftytwo
{
    public static class ObjectTools
    {
        public static string FullPath ( this Object obj, bool isRelative = false )
        {
            var sb = new StringBuilder();
            FullPath( obj, sb, isRelative );
            return sb.ToString();
        }

        public static void FullPath ( this Object obj, StringBuilder sb, bool isRelative = false )
        {
            if( obj == null )
            {
                sb.Append( "(null)" );
                return;
            }

            GameObject go;
            Component component;

            if( ( go = obj as GameObject ) != null )
                go.transform.HierarchyPath( sb, isRelative );
            else if( ( component = obj as Component ) != null )
                component.HierarchyPathWithMe( sb, isRelative );
            else
            {
#if UNITY_EDITOR
                sb.Append( UnityEditor.AssetDatabase.GetAssetPath( obj ) );
                sb.Append( " " );
#endif
                sb.Append( obj.ToString() );
            }
        }

#if UNITY_EDITOR
        public static void LogPrefabStatusAndCopyToClipboard ( this Object obj )
        {
            if( obj == null )
            {
                Debug.Log( "obj is null" );
                return;
            }

            var go = obj as GameObject;
            var component = obj as Component;
            if( component != null && go == null )
                go = component.gameObject;

            var sb = new StringBuilder();

            sb.AppendFormat( "=== Inspect prefab: {0} ===\n", obj.FullPath() );
            sb.AppendFormat( "GetPrefabAssetType()={0}\n", PrefabUtility.GetPrefabAssetType( obj ) );
            sb.AppendFormat( "GetPrefabInstanceStatus()={0}\n", PrefabUtility.GetPrefabInstanceStatus( obj ) );
            sb.AppendFormat( "IsDisconnectedFromPrefabAsset()={0}\n", PrefabUtility.IsDisconnectedFromPrefabAsset( obj ) );
            sb.AppendFormat( "IsPartOfAnyPrefab()={0}\n", PrefabUtility.IsPartOfAnyPrefab( obj ) );
            sb.AppendFormat( "IsPartOfImmutablePrefab()={0}\n", PrefabUtility.IsPartOfImmutablePrefab( obj ) );
            sb.AppendFormat( "IsPartOfModelPrefab()={0}\n", PrefabUtility.IsPartOfModelPrefab( obj ) );
            sb.AppendFormat( "IsPartOfNonAssetPrefabInstance()={0}\n", PrefabUtility.IsPartOfNonAssetPrefabInstance( obj ) );
            sb.AppendFormat( "IsPartOfPrefabAsset()={0}\n", PrefabUtility.IsPartOfPrefabAsset( obj ) );
            sb.AppendFormat( "IsPartOfPrefabInstance()={0}\n", PrefabUtility.IsPartOfPrefabInstance( obj ) );
            sb.AppendFormat( "IsPartOfPrefabThatCanBeAppliedTo()={0}\n", PrefabUtility.IsPartOfPrefabThatCanBeAppliedTo( obj ) );
            sb.AppendFormat( "IsPartOfRegularPrefab()={0}\n", PrefabUtility.IsPartOfRegularPrefab( obj ) );
            sb.AppendFormat( "IsPartOfVariantPrefab()={0}\n", PrefabUtility.IsPartOfVariantPrefab( obj ) );
            sb.AppendFormat( "IsPrefabAssetMissing()={0}\n", PrefabUtility.IsPrefabAssetMissing( obj ) );

            if( component != null )
            {
                sb.AppendFormat( "== Component ({0}) ==\n", component.GetType() );
                sb.AppendFormat( "IsAddedComponentOverride(false)={0}\n", PrefabUtility.IsAddedComponentOverride( component ) );
            }

            if( go != null )
            {
                sb.Append( "== GameObject ==\n" );
                sb.AppendFormat( "HasPrefabInstanceAnyOverrides(true)={0}\n", PrefabUtility.HasPrefabInstanceAnyOverrides( go, true ) );
                sb.AppendFormat( "HasPrefabInstanceAnyOverrides(false)={0}\n", PrefabUtility.HasPrefabInstanceAnyOverrides( go, false ) );
                sb.AppendFormat( "IsAddedGameObjectOverride()={0}\n", PrefabUtility.IsAddedGameObjectOverride( go ) );
                sb.AppendFormat( "IsAnyPrefabInstanceRoot()={0}\n", PrefabUtility.IsAnyPrefabInstanceRoot( go ) );
                sb.AppendFormat( "IsOutermostPrefabInstanceRoot()={0}\n", PrefabUtility.IsOutermostPrefabInstanceRoot( go ) );

                sb.Append( "== GameObject Modifications ==\n" );

                sb.Append( "GetAddedComponents()=" );
                try
                {
                    sb.Append( string.Join( "; ",
                        PrefabUtility.GetAddedComponents( go ).Select( x => x.instanceComponent.ToString() ) ) );
                }
                catch( Exception ex )
                {
                    sb.Append( ex.Message );
                }
                sb.Append( "\n" );

                sb.Append( "GetRemovedComponents()=" );
                try
                {
                    sb.Append( string.Join( "; ",
                        PrefabUtility.GetRemovedComponents( go ).Select( x => x.assetComponent.ToString() ) ) );
                }
                catch( Exception ex )
                {
                    sb.Append( ex.Message );
                }
                sb.Append( "\n" );

                sb.Append( "GetAddedGameObjects()=" );
                try
                {
                    sb.Append( string.Join( "; ",
                        PrefabUtility.GetAddedGameObjects( go ).Select(
                            x => "[" + x.siblingIndex + "]:" + x.instanceGameObject ) ) );
                }
                catch( Exception ex )
                {
                    sb.Append( ex.Message );
                }
                sb.Append( "\n" );
            }

            sb.Append( "== GetPropertyModifications() ==\n" );
            try
            {
                foreach( var pm in PrefabUtility.GetPropertyModifications( obj ) )
                {
                    sb.Append( pm.propertyPath );
                    sb.Append( "='" );
                    sb.Append( pm.objectReference == null ? pm.value : pm.objectReference.FullPath() );
                    sb.Append( "'; target='" );
                    sb.Append( pm.target.FullPath() );
                    sb.Append( "'\n" );
                }
            }
            catch( Exception ex )
            {
                sb.Append( ex.Message );
                sb.Append( "\n" );
            }

            sb.Append( "== Relations ==\n" );
            sb.AppendFormat( "GetCorrespondingObjectFromSource='{0}'\n",
                PrefabUtility.GetCorrespondingObjectFromSource( obj ).FullPath() );
            sb.AppendFormat( "GetCorrespondingObjectFromOriginalSource='{0}'\n",
                PrefabUtility.GetCorrespondingObjectFromOriginalSource( obj ).FullPath() );
            sb.AppendFormat( "GetNearestPrefabInstanceRoot='{0}'\n",
                PrefabUtility.GetNearestPrefabInstanceRoot( obj ).FullPath() );
            sb.AppendFormat( "GetOutermostPrefabInstanceRoot='{0}'\n",
                PrefabUtility.GetOutermostPrefabInstanceRoot( obj ).FullPath() );
            sb.AppendFormat( "GetPrefabAssetPathOfNearestInstanceRoot='{0}'\n",
                PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot( obj ) );
            sb.AppendFormat( "GetPrefabInstanceHandle='{0}'\n",
                PrefabUtility.GetPrefabInstanceHandle( obj ).FullPath() );

            sb.Append( "== Application ==\n");
            sb.AppendFormat( "Application.IsPlaying({0})={1}\n", obj, Application.IsPlaying( obj ) );
            sb.AppendFormat( "Application.isPlaying={0}\n", Application.isPlaying );
            sb.AppendFormat( "EditorApplication.isPlayingOrWillChangePlaymode={0}\n",
                EditorApplication.isPlayingOrWillChangePlaymode );

            GUIUtility.systemCopyBuffer = sb.ToString();

            sb.Insert( 0, "Copied to clipboard as well\n" );

            Debug.Log( sb.ToString() );
        }
#endif
    }
}
