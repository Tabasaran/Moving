#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Fiftytwo
{
    public static class AssetTools
    {
        public const string AssetsPath = "Assets";

        public static string CreateFolderIfMissing( string path )
        {
            if( string.IsNullOrEmpty( path ) )
                return null;

            path = path.Replace( '\\', '/' );
            if( path[path.Length - 1] == '/' )
                path = path.Substring( 0, path.Length - 1 );

            if( !path.StartsWith( AssetsPath, true, CultureInfo.CurrentCulture ) )
            {

                if( path.StartsWith( Application.dataPath, true, CultureInfo.CurrentCulture ) )
                    path = AssetsPath + "/" + path.Substring( Application.dataPath.Length );
                else
                {
                    Assert.IsTrue( false, $"Can't create folder with path \"{path}\", because it is out of Assets folder" );
                }
            }

            var components = new Stack<string>();
            var existingPath = path;
            while( !Directory.Exists( existingPath ) )
            {
                Assert.IsFalse( File.Exists( existingPath ),
                    $"Can't create folder with path \"{path}\", because of existing file at path \"{existingPath}\"" );

                var index = existingPath.LastIndexOf( '/' );
                Assert.IsTrue( index >= AssetsPath.Length,
                    $"Can't create folder with path \"{path}\", because Assets folder does not exist" );

                var component = existingPath.Substring( index + 1 );
                Assert.IsFalse( string.IsNullOrEmpty( component ),
                    $"Can't create folder with path \"{path}\", because it has empty path components" );
                components.Push( component );
                existingPath = existingPath.Remove( index );
            }

            Assert.IsTrue( existingPath[existingPath.Length - 1] != '/',
                $"Can't create folder with path \"{path}\", because it has empty path components" );

            string guid = null;

            if( components.Count == 0 )
            {
                guid = AssetDatabase.AssetPathToGUID( existingPath );
                Assert.IsFalse( string.IsNullOrEmpty( guid ),
                    $"Folder \"{existingPath}\" exists but AssetDatabese can't open it" );
                return guid;
            }

            var pathToDeleteInTheCaseOfFailure = existingPath + "/" + components.Peek();

            while( components.Count > 0 )
            {
                var component = components.Pop();
                guid = AssetDatabase.CreateFolder( existingPath, component );
                if( string.IsNullOrEmpty( guid ) )
                {
                    AssetDatabase.DeleteAsset( pathToDeleteInTheCaseOfFailure );
                    Assert.IsTrue( false, $"Can't create folder \"{existingPath}/{component}\"," +
                        " because AssetDatabase returned empty GUID" );
                }
                existingPath = existingPath + "/" + component;
            }

            return guid;
        }

        public static bool FolderExists ( string path )
        {
            if( string.IsNullOrEmpty( path ) )
                return false;
            return Directory.Exists( path );
        }

        public static bool FileExists ( string path )
        {
            if( string.IsNullOrEmpty( path ) )
                return false;
            return File.Exists( path );
        }

        public static bool PathExists ( string path )
        {
            if( string.IsNullOrEmpty( path ) )
                return false;
            return Directory.Exists( path ) || File.Exists( path );
        }
    }
}

#endif
