
namespace Fiftytwo
{
    public static class Persistence
    {
        public static IPrefsProvider Player { get; private set; }

#if UNITY_EDITOR
        public static IPrefsProvider Editor { get; private set; }
#endif

        static Persistence ()
        {
            Player = new PlayerPrefsProvider();

#if UNITY_EDITOR
            Editor = new EditorPrefsProvider();
#endif
        }
    }
}
