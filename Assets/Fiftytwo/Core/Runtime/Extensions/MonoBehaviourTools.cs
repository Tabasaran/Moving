using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fiftytwo
{
    public static class MonoBehaviourTools
    {
        public static T CreateWithGameObject<T> () where T : MonoBehaviour
        {
            var type = typeof( T );
            var gameObject = new GameObject( type.Name, type );
            return ( T )gameObject.GetComponent( type );
        }

        public static T CreateWithGameObject<T> ( string name ) where T : MonoBehaviour
        {
            var type = typeof( T );
            var gameObject = new GameObject( name, type );
            return ( T )gameObject.GetComponent( type );
        }

        public static GameObject InstantiateAsChild( this MonoBehaviour me, GameObject original )
        {
            var instance = GameObject.Instantiate( original );
            instance.transform.SetParent( me.transform, false );
            return instance;
        }

        public static T InstantiateAsChild<T>( this MonoBehaviour me, T original )
            where T : MonoBehaviour
        {
            var instance = GameObject.Instantiate( original );
            instance.transform.SetParent( me.transform, false );
            return instance;
        }

        public static GameObject[] InstantiateAsChildren( this MonoBehaviour me, params GameObject[] originals )
        {
            if( originals == null )
                return null;

            var instances = new GameObject[originals.Length];
            for( int i = 0; i < originals.Length; i++ )
            {
                instances[i] = GameObject.Instantiate( originals[i] );
                instances[i].transform.SetParent( me.transform, false );
            }

            return instances;
        }

        public static T[] InstantiateAsChildren<T>( this MonoBehaviour me, params T[] originals )
            where T : MonoBehaviour
        {
            if( originals == null )
                return null;

            var instances = new T[originals.Length];
            for( int i = 0; i < originals.Length; i++ )
            {
                instances[i] = GameObject.Instantiate( originals[i] );
                instances[i].transform.SetParent( me.transform, false );
            }

            return instances;
        }

        public static void InstantiateAsChildrenReplacingOriginals( this MonoBehaviour me,
            GameObject[] originalsAndOutInstances )
        {
            if( originalsAndOutInstances == null )
                return;

            for( int i = 0; i < originalsAndOutInstances.Length; i++ )
            {
                originalsAndOutInstances[i] = GameObject.Instantiate( originalsAndOutInstances[i] );
                originalsAndOutInstances[i].transform.SetParent( me.transform, false );
            }
        }

        public static void InstantiateAsChildrenReplacingOriginals<T>( this MonoBehaviour me,
            T[] originalsAndOutInstances )
            where T : MonoBehaviour
        {
            if( originalsAndOutInstances == null )
                return;

            for( int i = 0; i < originalsAndOutInstances.Length; i++ )
            {
                originalsAndOutInstances[i] = GameObject.Instantiate( originalsAndOutInstances[i] );
                originalsAndOutInstances[i].transform.SetParent( me.transform, false );
            }
        }

        public static IEnumerator UnloadScenesAfterMine ( this MonoBehaviour me )
        {
            var myScene = me.gameObject.scene;
            var count = SceneManager.sceneCount;
            while( --count > 0 )
            {
                var scene = SceneManager.GetSceneAt( count );
                if( scene == myScene )
                    break;
                if( scene.IsValid() )
                    yield return SceneManager.UnloadSceneAsync( scene );
            }
        }
    }
}
