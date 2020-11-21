using UnityEngine;
using UnityEngine.Events;


namespace Fiftytwo
{
    public class GameEventListener : MonoBehaviour, IGameEventListener
    {
        public GameEvent Event;
        public UnityEvent Response;

        public void OnEventRaised()
        {
            Response.Invoke();
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
