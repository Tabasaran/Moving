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
    [CreateAssetMenu( menuName = "Fiftytwo/Game Float Event", fileName = "GameFloatEvent" )]
    public class GameFloatEvent : GameEvent<float>
    {
    }
}
