#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Fiftytwo
{
    public static class SerializationTools
    {
        private static PropertyInfo s_propGradientValue;

        static SerializationTools ()
        {
            s_propGradientValue = typeof(SerializedProperty).GetProperty( "gradientValue",
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null, typeof( Gradient ), new Type[0], null );
        }

        public static string ConvertValueToString ( this SerializedProperty prop, string floatFormat = null )
        {
            switch( prop.propertyType )
            {
                case SerializedPropertyType.Integer:
                    return prop.longValue.ToString();
                case SerializedPropertyType.Boolean:
                    return prop.boolValue.ToString();
                case SerializedPropertyType.Float:
                    return prop.doubleValue.ToString();
                case SerializedPropertyType.String:
                    return prop.stringValue;
                case SerializedPropertyType.Color:
                    return prop.colorValue.ToString();
                case SerializedPropertyType.ObjectReference:
                    return ( prop.objectReferenceValue == null ? "(null)" : prop.objectReferenceValue.ToString() );
                case SerializedPropertyType.LayerMask:
                    return prop.intValue.ToString();
                case SerializedPropertyType.Enum:
                    return prop.enumNames[prop.enumValueIndex];
                case SerializedPropertyType.Vector2:
                    return prop.vector2Value.ToString( "F3" );
                case SerializedPropertyType.Vector3:
                    return prop.vector3Value.ToString( "F3" );
                case SerializedPropertyType.Vector4:
                    return prop.vector4Value.ToString( "F3" );
                case SerializedPropertyType.Rect:
                    return prop.rectValue.ToString( "F3" );
                case SerializedPropertyType.ArraySize:
                    return prop.intValue.ToString();
                case SerializedPropertyType.Character:
                    return ( ( char )prop.intValue ).ToString();
                case SerializedPropertyType.AnimationCurve:
                    return prop.animationCurveValue.ToString();
                case SerializedPropertyType.Bounds:
                    return prop.boundsValue.ToString( "F3" );
                case SerializedPropertyType.Gradient:
                    return prop.GradientValue().ToString();
                case SerializedPropertyType.Quaternion:
                    return prop.quaternionValue.ToString( "F3" );
                case SerializedPropertyType.ExposedReference:
                    return ( prop.exposedReferenceValue == null ? "(null)" : prop.exposedReferenceValue.ToString() );
                case SerializedPropertyType.FixedBufferSize:
                    return prop.intValue.ToString();
                default:
                    return null;
            }
        }

        public static string ToDebugString ( this SerializedProperty prop )
        {
            string val = prop.ConvertValueToString();

            if( val == null )
                return prop.propertyPath + " " + prop.type + ":" + prop.propertyType;
            return prop.propertyPath + " " + prop.type + ":" + prop.propertyType + " = " + val;
        }

        public static Gradient GradientValue( this SerializedProperty prop )
        {
            return ( Gradient )s_propGradientValue.GetValue( prop, null );
        }
    }
}

#endif
