using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using JetBrains.Annotations;

namespace Fiftytwo
{
    public static class UnityEventTools
    {
        public static bool HasMethod ( this UnityEvent evt, string method )
        {
            for( int i = evt.GetPersistentEventCount(); --i >= 0; )
            {
                if( evt.GetPersistentMethodName( i ) == method )
                    return true;
            }
            return false;
        }

        public static string ToDebugString ( this UnityEvent evt )
        {
            var sb = new StringBuilder( "{\n" );

            for( int i = 0, count = evt.GetPersistentEventCount(); i < count; ++i )
            {
                sb.Append( "    " );
                evt.GetPersistentTarget( i ).FullPath( sb );
                sb.Append( '.' );
                sb.Append(  evt.GetPersistentMethodName( i ) );
                sb.Append( "()\n" );
            }
            sb.Append("}");

            return sb.ToString();
        }
    }

    [Serializable]
    public class UnityFloatEvent : UnityEvent<float>{}

    [Serializable]
    public class UnityStringEvent: UnityEvent<string>{}

    [Serializable]
    public class UnityBoolEvent: UnityEvent<bool>{}

    [Serializable]
    public class UnityIntEvent: UnityEvent<int>{}

    [Serializable]
    public class UnityVector3Event: UnityEvent<Vector3>{}

    [Serializable]
    public class UnityQuaternionEvent: UnityEvent<Quaternion>{}
}
