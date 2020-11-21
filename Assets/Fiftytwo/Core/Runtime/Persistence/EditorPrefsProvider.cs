using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

#if UNITY_EDITOR

using UnityEditor;

namespace Fiftytwo
{
    public class EditorPrefsProvider : IPrefsProvider
    {
        public bool GetBool( string key, bool defaultValue = false )
        {
            return EditorPrefs.GetBool( key, defaultValue );
        }

        public void SetBool( string key, bool value )
        {
            EditorPrefs.SetBool( key, value );
        }

        public int GetInt( string key, int defaultValue = 0 )
        {
            return EditorPrefs.GetInt( key, defaultValue );
        }

        public void SetInt( string key, int value )
        {
            EditorPrefs.SetInt( key, value );
        }

        public float GetFloat( string key, float defaultValue = 0.0f )
        {
            return EditorPrefs.GetFloat( key, defaultValue );
        }

        public void SetFloat( string key, float value )
        {
            EditorPrefs.SetFloat( key, value );
        }

        public string GetString( string key, string defaultValue = "" )
        {
            return EditorPrefs.GetString( key, defaultValue );
        }

        public void SetString( string key, string value )
        {
            EditorPrefs.SetString( key, value );
        }

        public bool HasKey ( string key )
        {
            return EditorPrefs.HasKey( key );
        }

        public void DeleteKey ( string key )
        {
            EditorPrefs.DeleteKey( key );
        }

        public void DeleteAll ()
        {
            EditorPrefs.DeleteAll();
        }
    }
}

#endif