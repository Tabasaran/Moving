using System.Collections.Generic;
using UnityEngine;


namespace Fiftytwo
{
    [CreateAssetMenu( menuName = "Fiftytwo/Game Event", fileName = "GameEvent" )]
    public class GameEvent : ScriptableObject
    {
        private List<IGameEventListener> _listeners = new List<IGameEventListener>();

        public void Raise ()
        {
            for( int i = _listeners.Count; --i >= 0; )
                _listeners[i].OnEventRaised();
        }

        public void RegisterListener ( IGameEventListener listener )
        {
            _listeners.Add( listener );
        }

        public void UnregisterListener ( IGameEventListener listener )
        {
            _listeners.Remove( listener );
        }
    }

    public class GameEvent<T> : ScriptableObject
    {
        private List< IGameEventListener<T> > _listeners = new List< IGameEventListener<T> >();

        public void Raise ( T value )
        {
            for( int i = _listeners.Count; --i >= 0; )
                _listeners[i].OnEventRaised( value );
        }

        public void RegisterListener ( IGameEventListener<T> listener )
        {
            _listeners.Add( listener );
        }

        public void UnregisterListener ( IGameEventListener<T> listener )
        {
            _listeners.Remove( listener );
        }
    }
}
