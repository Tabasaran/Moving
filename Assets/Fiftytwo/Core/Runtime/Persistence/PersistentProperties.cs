using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using JetBrains.Annotations;

namespace Fiftytwo
{
    public class PersistentProperties
    {
        public event EventHandlerVoid Updated;

        private IPrefsProvider _provider;
        private string _prefix;

        public PersistentProperties( IPrefsProvider provider, Type classType )
        {
            _provider = provider;
            _prefix = classType.FullName + ".";
        }

        public PersistentProperties( IPrefsProvider provider, string uniqueClassName )
        {
            _provider = provider;
            _prefix = uniqueClassName;
        }

        public bool Load ( Expression<Func<bool>> propertySelector, bool defaultValue = false )
        {
            return Load( ( ( MemberExpression )propertySelector.Body ).Member.Name, defaultValue );
        }
        public bool Load ( string propertyName, bool defaultValue = false )
        {
            return _provider.GetBool( _prefix + propertyName, defaultValue );
        }
        public void Save ( Expression<Func<bool>> propertySelector, bool value )
        {
            Save( ( ( MemberExpression )propertySelector.Body ).Member.Name, value );
        }
        public void Save ( string propertyName, bool value )
        {
            _provider.SetBool( _prefix + propertyName, value );
            Updated.Notify();
        }

        public int Load ( Expression<Func<int>> propertySelector, int defaultValue = 0 )
        {
            return Load( ( ( MemberExpression )propertySelector.Body ).Member.Name, defaultValue );
        }
        public int Load ( string propertyName, int defaultValue = 0 )
        {
            return _provider.GetInt( _prefix + propertyName, defaultValue );
        }
        public void Save ( Expression<Func<int>> propertySelector, int value )
        {
            Save( ( ( MemberExpression )propertySelector.Body ).Member.Name, value );
        }
        public void Save ( string propertyName, int value )
        {
            _provider.SetInt( _prefix + propertyName, value );
            Updated.Notify();
        }

        public float Load ( Expression<Func<float>> propertySelector, float defaultValue = 0.0f )
        {
            return Load( ( ( MemberExpression )propertySelector.Body ).Member.Name, defaultValue );
        }
        public float Load ( string propertyName, float defaultValue = 0.0f )
        {
            return _provider.GetFloat( _prefix + propertyName, defaultValue );
        }
        public void Save ( Expression<Func<float>> propertySelector, float value )
        {
            Save( ( ( MemberExpression )propertySelector.Body ).Member.Name, value );
        }
        public void Save ( string propertyName, float value )
        {
            _provider.SetFloat( _prefix + propertyName, value );
            Updated.Notify();
        }

        public string Load ( Expression<Func<string>> propertySelector, string defaultValue = "" )
        {
            return Load( ( ( MemberExpression )propertySelector.Body ).Member.Name, defaultValue );
        }
        public string Load ( string propertyName, string defaultValue = "" )
        {
            return _provider.GetString( _prefix + propertyName, defaultValue );
        }
        public void Save ( Expression<Func<string>> propertySelector, string value )
        {
            Save( ( ( MemberExpression )propertySelector.Body ).Member.Name, value );
        }
        public void Save ( string propertyName, string value )
        {
            _provider.SetString( _prefix + propertyName, value );
            Updated.Notify();
        }

        public void Delete ( Expression<Func<string>> propertySelector )
        {
            Delete( ( ( MemberExpression )propertySelector.Body ).Member.Name );
        }
        public void Delete ( string propertyName )
        {
            _provider.DeleteKey( _prefix + propertyName );
            Updated.Notify();
        }
    }
}
