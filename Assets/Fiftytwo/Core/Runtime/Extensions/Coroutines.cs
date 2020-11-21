using UnityEngine;
using System;
using System.Collections;

namespace Fiftytwo
{
    public static class Coroutines
    {
        public static Coroutine StartChainedCoroutine( this MonoBehaviour monoBehaviour, params IEnumerator[] actions )
        {
            return monoBehaviour.StartCoroutine( MakeChainedCoroutine( actions ) );
        }
        
        public static IEnumerator MakeChainedCoroutine( params IEnumerator[] actions )
        {
            int count = actions.Length;
            for( int i = 0; i < count; ++i )
            {
                yield return actions[i];
            }
        }

        public static IEnumerator Do( YieldInstruction yieldInstruction )
        {
            yield return yieldInstruction;
        }

        public static IEnumerator Do( CustomYieldInstruction yieldInstruction )
        {
            yield return yieldInstruction;
        }

        public static IEnumerator Do( Action action )
        {
            action();
            yield return null;
        }

        public static Coroutine WaitAndDo ( this MonoBehaviour monoBehaviour, float waitTime, Action action )
        {
            return monoBehaviour.StartChainedCoroutine(
                Do( new WaitForSeconds( waitTime ) ),
                Do( action )
            );
        }
    }
}
