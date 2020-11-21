using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fiftytwo
{
    public class PopupTree : PopupWindowContent
    {
        private const int MinWidth = 200;

        private Rect _dropdownRect;
        private ComponentsSaver _saver;

        Vector2 _scrollPos;

        public PopupTree ( Rect dropdownRect, ComponentsSaver saver )
        {
            _dropdownRect = dropdownRect;
            _saver = saver;
        }

        public override void OnOpen ()
        {
            base.OnOpen();
        }

        public override void OnClose ()
        {
            base.OnClose();
        }

        private Rect _contentRect;

        public override void OnGUI ( Rect rect )
        {
            _scrollPos = GUILayout.BeginScrollView( _scrollPos );

            EditorGUILayout.BeginVertical();

            //if( GUILayout.Button( "Print height" ) )
            //{
            //    Debug.Log( $"VERT RC: {_contentRect}" );
            //}
            //for( int i = 0; i < 25; ++i )
            //{
            //    GUILayout.Button( "Button " + i );
            //}

            //var foldoutHeaderStyle = new GUIStyle( EditorStyles.foldoutHeader );
            //Debug.Log( $"HEIGHT: {foldoutHeaderStyle.fixedHeight}" );
            //foldoutHeaderStyle.fixedHeight = 24;


            var components = _saver.GetComponents<Component>();
            for( int i = 0; i < components.Length; ++i )
            {
                var component = components[i];

                EditorGUILayout.BeginHorizontal();
                var componentContent = EditorGUIUtility.ObjectContent( component, component.GetType() );
                componentContent.text = component.name;

                //var style = new GUIStyle( EditorStyles.foldoutHeader );
                //var componentSize = style.CalcHeight( componentContent, 100 );
                //style.fixedHeight = componentSize;

                EditorGUILayout.BeginFoldoutHeaderGroup( false, componentContent );
                EditorGUILayout.EndFoldoutHeaderGroup();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            if( Event.current.type == EventType.Repaint )
                _contentRect = GUILayoutUtility.GetLastRect();

            GUILayout.EndScrollView();
        }

        public override Vector2 GetWindowSize()
        {
            var height = _contentRect.height + 2 * _contentRect.y;

            return new Vector2(
                _dropdownRect.width <= MinWidth ? MinWidth : _dropdownRect.width,
                height <= 16 ? 16 : height );
        }
    }
}
