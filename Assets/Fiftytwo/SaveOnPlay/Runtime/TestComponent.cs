using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Fiftytwo
{
    public class TestComponent : MonoBehaviour
    {
        [Serializable]
        public class TestClass
        {
            public int Int;
            public int[] IntArray;
            public string Str;
            public string[] StrArray;
            public Object Ref;
            public Object[] RefArray;
        }

        public int IntField;
        public int[] IntArrayField;
        public string StrField;
        public string[] StrArrayField;
        public Object RefField;
        public Object[] RefArrayField;
        public TestClass TestClassField;
        public TestClass[] TestClassArrayField;
    }
}
