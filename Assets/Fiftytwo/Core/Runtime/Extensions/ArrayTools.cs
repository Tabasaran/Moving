using System;
using System.Text;
using UnityEngine.Assertions;

namespace Fiftytwo
{
    public static class ArrayTools
    {
        public static bool IsNullOrEmpty ( Array array )
        {
            return array == null || array.Length == 0;
        }

        public static void AddAt<T> ( this T[] array, ref int count, int index, T element )
        {
            Assert.IsTrue( 0 <= count && count < array.Length, "count is out of range" );
            Assert.IsTrue( 0 <= index && index <= count, "index is out of range" );

            for( int i = ++count; --i > index; )
                array[i] = array[i - 1];
            array[index] = element;
        }

        public static void AddFirst<T> ( this T[] array, ref int count, T element )
        {
            Assert.IsTrue( 0 <= count && count < array.Length, "count is out of range" );

            for( int i = ++count; --i > 0; )
                array[i] = array[i - 1];
            array[0] = element;
        }

        public static void AddLast<T> ( this T[] array, ref int count, T element )
        {
            Assert.IsTrue( 0 <= count && count < array.Length, "count is out of range" );

            array[count++] = element;
        }

        public static void RemoveAt<T> ( this T[] array, ref int count, int index )
        {
            Assert.IsTrue( 0 <= count && count <= array.Length, "count is out of range" );
            Assert.IsTrue( 0 <= index && index < count, "index is out of range" );

            while( ++index < count )
                array[index - 1] = array[index];
            --count;
        }

        public static T[] RemoveAt<T>( this T[] source, int index )
        {
            var dest = new T[ source.Length - 1 ];

            if( index > 0 )
                Array.Copy( source, 0, dest, 0, index );

            if( index < source.Length - 1 )
                Array.Copy( source, index + 1, dest, index, source.Length - index - 1 );

            return dest;
        }

        public static void RemoveFirst<T> ( this T[] array, ref int count )
        {
            Assert.IsTrue( 0 <= count && count <= array.Length, "count is out of range" );

            for( int index = 0; ++index < count; )
                array[index - 1] = array[index];
            --count;
        }

        public static void SetAllValues<T> ( this T[] array, T value )
        {
            for( int i = array.Length; --i >= 0; )
                array[i] = value;
        }

        public static T[] NewWithValues<T> ( int length, T value )
        {
            var array = new T[length];
            SetAllValues( array, value );
            return array;
        }

        public static T[][] JaggedArray<T> ( int rows, int cols )
        {
            T[][] array = new T[rows][];
            while( --rows >= 0 )
                array[rows] = new T[cols];
            return array;
        }

        public static string ToDebugString<T> ( this T[] array, string separator = "; " )
        {
            if( array == null )
                return "null";
            if( array.Length == 0 )
                return "[]";

            var sb = new StringBuilder( "[" );
            sb.Append( array[0].ToString() );
            for( int i = 1; i < array.Length; ++i )
            {
                sb.Append( separator );
                sb.Append( array[i] );
            }
            sb.Append( "]" );

            return sb.ToString();
        }

        public static bool IsInRange<T> ( this T[] array, int index )
        {
            return 0 <= index && index < array.Length;
        }
    }
}
