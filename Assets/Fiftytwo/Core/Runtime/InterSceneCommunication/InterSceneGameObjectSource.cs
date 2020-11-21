using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fiftytwo
{
    public class InterSceneGameObjectSource : MonoBehaviour
    {
        [SerializeField]
        private InterSceneGameObject _interSceneGameObject;

        private void Awake ()
        {
            _interSceneGameObject.Set( this.gameObject );
        }

        private void OnDestroy ()
        {
            if( _interSceneGameObject.GameObject == this.gameObject )
                _interSceneGameObject.Set( null );
        }
    }
}
