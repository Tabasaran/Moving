using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fiftytwo
{
    [CanEditMultipleObjects]
    [CustomEditor( typeof( ComponentsSaver ) )]
    public class ComponentsSaverEditor : Editor
    {
        private static HashSet<string> _excludeProperties = new HashSet<string>
        {
            "m_ObjectHideFlags",
            //"m_CorrespondingSourceObject.m_FileID",
            //"m_CorrespondingSourceObject.m_PathID",
            //"m_PrefabInstance.m_FileID",
            //"m_PrefabInstance.m_PathID",
            //"m_PrefabAsset.m_FileID",
            //"m_PrefabAsset.m_PathID",
            //"m_PrefabAsset.m_PathID",
            //"m_GameObject.m_FileID",
            //"m_GameObject.m_PathID",

            // MonoBehaviour specific
            "m_EditorHideFlags",
            //"m_Script.m_FileID",
            //"m_Script.m_PathID",
            "m_Name",
            "m_EditorClassIdentifier",

            // Transform specific
            "m_Children.Array.size",
            //"m_Father.m_FileID",
            //"m_Father.m_PathID",
            "m_RootOrder",
            "m_LocalEulerAnglesHint.x",
            "m_LocalEulerAnglesHint.y",
            "m_LocalEulerAnglesHint.z",
        };

        private ComponentsSaver _target;
        private SerializedProperty _components;

        private Rect _dropdownRect;

        private void OnEnable ()
        {
            _target = ( ComponentsSaver )target;
            _components = serializedObject.FindProperty( "Components" );
        }

        public override void OnInspectorGUI ()
        {
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            var size = _components.arraySize;
            for( int i = 0; i < size; ++i )
            {
                var componentItem = _components.GetArrayElementAtIndex( i );

                var content = componentItem.Copy();
                content.Next( true );
                var component = content.objectReferenceValue;

                var componentContent = EditorGUIUtility.ObjectContent( component, component.GetType() );
                componentContent.text = component.GetType().Name;
                componentItem.isExpanded = EditorGUILayout.BeginFoldoutHeaderGroup( componentItem.isExpanded, componentContent );
                if( componentItem.isExpanded )
                {
                    content.Next( false );
                    DrawObjectPropertiesGUI( component, content );
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }

            if( EditorGUI.EndChangeCheck() )
                serializedObject.ApplyModifiedProperties();

            /*if( EditorGUILayout.DropdownButton( new GUIContent( "Choose properties" ), FocusType.Passive ) )
            {
                var popup = new PopupTree( _dropdownRect, _target );
                PopupWindow.Show( _dropdownRect, popup );
            }
            if( Event.current.type == EventType.Repaint )
                _dropdownRect = GUILayoutUtility.GetLastRect();

            if( GUILayout.Button( "Print Props" ) )
            {
                _target.PrintPropertiesTree();
            }*/
        }

        private void DrawObjectPropertiesGUI ( Object obj, SerializedProperty propertyPaths )
        {
            var paths = new List<string>();
            for( int i = 0; i < propertyPaths.arraySize; ++i )
                paths.Add( propertyPaths.GetArrayElementAtIndex( i ).stringValue );

            bool arePathsChanged = false;

            using( var so = new SerializedObject( obj ) )
            {
                for( var prop = so.GetIterator();
                    prop.Next( prop.propertyType != SerializedPropertyType.String &&
                        prop.propertyType != SerializedPropertyType.ObjectReference ); )
                {
                    switch( prop.propertyType )
                    {
                        case SerializedPropertyType.Integer:
                        case SerializedPropertyType.Boolean:
                        case SerializedPropertyType.Float:
                        case SerializedPropertyType.String:
                        case SerializedPropertyType.Color:
                        //case SerializedPropertyType.ObjectReference:
                        case SerializedPropertyType.LayerMask:
                        case SerializedPropertyType.Enum:
                        //case SerializedPropertyType.Vector2:
                        //case SerializedPropertyType.Vector3:
                        //case SerializedPropertyType.Vector4:
                        //case SerializedPropertyType.Rect:
                        //case SerializedPropertyType.ArraySize:
                        case SerializedPropertyType.Character:
                        case SerializedPropertyType.AnimationCurve:
                        //case SerializedPropertyType.Bounds:
                        //case SerializedPropertyType.Gradient:
                        //case SerializedPropertyType.Quaternion:
                        //case SerializedPropertyType.ExposedReference:
                        //case SerializedPropertyType.FixedBufferSize:
                        //case SerializedPropertyType.Vector2Int:
                        //case SerializedPropertyType.Vector3Int:
                        //case SerializedPropertyType.RectInt:
                        //case SerializedPropertyType.BoundsInt:
                            if( _excludeProperties.Contains( prop.propertyPath ) )
                                break;

                            bool isEnabledOld = paths.Contains( prop.propertyPath );
                            var isEnabled = EditorGUILayout.ToggleLeft( prop.propertyPath, isEnabledOld );
                            if( isEnabled != isEnabledOld )
                            {
                                if( isEnabled )
                                    paths.Add( prop.propertyPath );
                                else
                                    paths.Remove( prop.propertyPath );
                                arePathsChanged = true;
                            }
                            break;
                    }
                }
            }

            if( arePathsChanged )
            {
                propertyPaths.arraySize = paths.Count;
                for( int i = 0; i < paths.Count; ++i )
                {
                    var pathProp = propertyPaths.GetArrayElementAtIndex( i );
                    pathProp.stringValue = paths[i];
                }
            }
        }
    }
}
