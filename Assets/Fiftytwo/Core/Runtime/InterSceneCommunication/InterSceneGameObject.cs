using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Fiftytwo
{
    [CreateAssetMenu( menuName = "Fiftytwo/Inter Scene Game Object", fileName = "InterSceneGameObject" )]
    public class InterSceneGameObject : ScriptableObject
    {
        [SerializeField] private UnityEvent _changed;

        [NonSerialized] public GameObject GameObject;

        public void Set ( GameObject go )
        {
            GameObject = go;
            _changed.Invoke();
        }
    }
}
