using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using System.IO;
using UnityEngine.Assertions;
using UnityEditor.Presets;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Fiftytwo
{
    [ExecuteAlways]
    public class ComponentsSaver : MonoBehaviour
    {
        private static readonly Dictionary<string, ComponentsSaver> _usedGuids = new Dictionary<string, ComponentsSaver>();

        [SerializeField]
        private string _guid;

        public ComponentItem[] Components;

        private bool _isUndoValidateRecorded;


#if UNITY_EDITOR

        private const string TempPresetsPath = "Assets/Fiftytwo/_Temp/Editor/SaveOnPlay";

        private string NeedToRestoreKey
        {
            get { return GetType() + _guid + ".needToRestore"; }
        }

        private void OnValidate ()
        {
            if( Application.isPlaying || !gameObject.scene.IsValid() )
                return;

            _isUndoValidateRecorded = false;

            ValidateGuid();

            ValidateComponents();
        }

        private void UndoValidate()
        {
            if( _isUndoValidateRecorded )
                return;
            Undo.RecordObject( this, GetType() + ": Validate" );
            _isUndoValidateRecorded = true;
        }

        private void ValidateGuid ()
        {
            if( string.IsNullOrEmpty( _guid ) )
            {
                GenerateNewGuid();
                return;
            }

            if( !_usedGuids.TryGetValue( _guid, out var component ) )
            {
                _usedGuids.Add( _guid, this );
                return;
            }

            if( this == component )
                return;

            if( component == null )
                _usedGuids[_guid] = this;
            else
                GenerateNewGuid();
        }

        private void GenerateNewGuid ()
        {
            UndoValidate();
            _guid = System.Guid.NewGuid().ToString();
            _usedGuids.Add( _guid, this );
        }

        private void ValidateComponents ()
        {
            int i, j, count;
            Component srcComponent;

            List<Component> srcComponents = new List<Component>();
            GetComponents( srcComponents );
            for( i = srcComponents.Count; --i >= 0; )
            {
                srcComponent = srcComponents[i];
                if( srcComponent == null || srcComponent == this )
                    srcComponents.RemoveAt( i );
            }

            count = srcComponents.Count;

            Assert.AreNotEqual( 0, count, "There are no components in " + this );

            if( Components == null || Components.Length != count )
            {
                UndoValidate();
                Array.Resize<ComponentItem>( ref Components, count );
            }

            for( i = 0; i < count; ++i )
            {
                srcComponent = srcComponents[i];

                var componentItem = Components[i];
                if( componentItem == null || componentItem.Component != srcComponent )
                {
                    bool isItemFound = false;
                    for( j = i + 1; j < count; ++j )
                    {
                        var item = Components[j];
                        if( item != null && item.Component == srcComponent )
                        {
                            UndoValidate();
                            Components[j] = Components[i];
                            Components[i] = item;
                            isItemFound = true;
                            break;
                        }
                    }

                    if( !isItemFound )
                    {
                        UndoValidate();
                        Components[i] = new ComponentItem
                        {
                            Component = srcComponent
                        };
                    }
                }
            }
        }

        private void Awake ()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChange;

            if( !Application.isPlaying )
                Restore();
        }

        private void OnPlayModeStateChange ( PlayModeStateChange change )
        {
            //ApplicationTools.LogState( this, "INSP CHANGE " + change );
            if( change == PlayModeStateChange.ExitingPlayMode )
            {
                EditorApplication.playModeStateChanged -= OnPlayModeStateChange;
                    Save();
            }
            //else if( change == PlayModeStateChange.EnteredEditMode )
            //{
            //    //AssetDatabase.DeleteAsset( TempPresetsPath );
            //}
        }

        private void Save ()
        {
            if( !Directory.Exists( TempPresetsPath ) )
                AssetTools.CreateFolderIfMissing( TempPresetsPath );

            bool isSomethingSaved = false;

            var count = Components.Length;

            for( int i = 0; i < count; ++i )
            {
                var componentItem = Components[i];
                if( componentItem == null || componentItem.Component == null ||
                    componentItem.PropertyPaths == null || componentItem.PropertyPaths.Length == 0 )
                {
                    continue;
                }

                var preset = new Preset( componentItem.Component );
                if( preset == null )
                    continue;

                isSomethingSaved = true;

                preset.name = _guid + " " + i + " " + count;
                var path = TempPresetsPath + "/" + preset.name + ".preset";
                if( File.Exists( path ) )
                    AssetDatabase.DeleteAsset( path );
                AssetDatabase.CreateAsset( preset, path );
            }

            Persistence.Editor.SetBool( NeedToRestoreKey, isSomethingSaved );
        }

        private void Restore ()
        {
            var needToRestoreKey = NeedToRestoreKey;
            if( !Persistence.Editor.HasKey( needToRestoreKey ) )
                return;
            var needToRestore = Persistence.Editor.GetBool( needToRestoreKey, false );
            Persistence.Editor.DeleteKey( needToRestoreKey );
            if( !needToRestore )
                return;

            if( !Directory.Exists( TempPresetsPath ) )
                return;

            var count = Components.Length;
            for( int i = 0; i < count; ++i )
            {
                var componentItem = Components[i];
                if( componentItem == null || componentItem.Component == null ||
                    componentItem.PropertyPaths == null || componentItem.PropertyPaths.Length == 0 )
                {
                    continue;
                }

                var presetPath = TempPresetsPath + "/" + _guid + " " + i + " " + count + ".preset";
                var preset = AssetDatabase.LoadAssetAtPath<Preset>( presetPath );
                if( preset == null )
                    continue;

                Undo.RecordObject( componentItem.Component, GetType() + ": Save After Play" );
                preset.ApplyTo( componentItem.Component, componentItem.PropertyPaths );
                AssetDatabase.DeleteAsset( presetPath );
            }
        }

        [Serializable]
        public class ComponentItem
        {
            public Component Component;
            public string[] PropertyPaths;
        }

#endif
    }
}
