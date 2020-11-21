using System;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Fiftytwo
{
    public static class ApplicationTools
    {
        private const string Na = "n/a";
        private static int _logId = 0;

        public static void LogState ( string msg, Object obj = null )
        {
            var logId = Interlocked.Increment( ref _logId );

            string appIsPlaying;
            try { appIsPlaying = Application.isPlaying.ToString(); } catch { appIsPlaying = Na; }

            string appIsPlayingObj;
            try { appIsPlayingObj = obj == null ? "null" : Application.IsPlaying( obj ).ToString(); }
            catch { appIsPlayingObj = Na; }

#if UNITY_EDITOR
            string editorIsPlaying;
            try { editorIsPlaying = EditorApplication.isPlaying.ToString(); } catch { editorIsPlaying = Na; }

            string isPlayingOrWillChangePlaymode;
            try { isPlayingOrWillChangePlaymode = EditorApplication.isPlayingOrWillChangePlaymode.ToString(); }
            catch { isPlayingOrWillChangePlaymode = Na; }

            string isPaused;
            try { isPaused = EditorApplication.isPaused.ToString(); } catch { isPaused = Na; }

            string isRemoteConnected;
            try { isRemoteConnected = EditorApplication.isRemoteConnected.ToString(); } catch { isRemoteConnected = Na; }

            string isTemporaryProject;
            try { isTemporaryProject = EditorApplication.isTemporaryProject.ToString(); } catch { isTemporaryProject = Na; }

            string isUpdating;
            try { isUpdating = EditorApplication.isUpdating.ToString(); } catch { isUpdating = Na; }
#endif

            Debug.Log(
                $"[{logId}:{Thread.CurrentThread.ManagedThreadId}]: " +
                $"{msg} " +
                $"aipl={appIsPlaying}; " +
                $"AIPL={appIsPlayingObj}"
#if UNITY_EDITOR
                + $"; eipl={editorIsPlaying}; " +
                $"ipcp={isPlayingOrWillChangePlaymode}; " +
                $"ipau={isPaused}; " +
                $"icom={EditorApplication.isCompiling}; " +
                $"irco={isRemoteConnected}; " +
                $"itpr={isTemporaryProject}; " +
                $"iupd={isUpdating}"
#endif
                , obj
            );
        }

#if UNITY_EDITOR
        public static bool IsPlayingOrChanging
        {
            get { return Application.isPlaying || EditorApplication.isPlayingOrWillChangePlaymode; }
        }
#else
        public static bool IsPlayingOrChanging { get { return true; } }
#endif

        public static void Quit ()
        {
            Application.Quit();
        }
    }
}
