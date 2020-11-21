using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using UnityEditor;
using Object = UnityEngine.Object;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace Fiftytwo
{
    public class GameFloatEventListener : MonoBehaviour, IGameEventListener<float>
    {
        public GameFloatEvent Event;
        public UnityFloatEvent Response;

        public void OnEventRaised( float value )
        {
            Response.Invoke( value );
        }

        private void OnEnable ()
        {
            Event.RegisterListener( this );
        }

        private void OnDisable ()
        {
            Event.UnregisterListener( this );
        }
    }
}
