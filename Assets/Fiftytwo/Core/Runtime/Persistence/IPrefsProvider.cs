using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JetBrains.Annotations;

namespace Fiftytwo
{
    public interface IPrefsProvider
    {
        bool GetBool( string key, bool defaultValue = false );

        void SetBool( string key, bool value );

        int GetInt( string key, int defaultValue = 0 );

        void SetInt( string key, int value );

        float GetFloat( string key, float defaultValue = 0.0f );

        void SetFloat( string key, float value );

        string GetString( string key, string defaultValue = "" );

        void SetString( string key, string value );

        bool HasKey ( string key );

        void DeleteKey ( string key );

        void DeleteAll ();
    }
}
