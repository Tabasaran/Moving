using System;
using System.Globalization;

namespace Fiftytwo
{
    public static class StringTools
    {
        public static string SafeSubstring ( this string aString, int startIndex, int length )
        {
            if( length <= 0 )
                return string.Empty;

            if( startIndex >= aString.Length )
                return string.Empty;

            if( startIndex + length <= 0 )
                return string.Empty;

            if( startIndex < 0 )
            {
                startIndex = 0;
                length += startIndex;
            }

            if( startIndex + length >= aString.Length )
                length = aString.Length - startIndex;

            return aString.Substring( startIndex, length );
        }

        public static string SafeSubstring ( this string aString, int startIndex )
        {
            if( startIndex >= aString.Length )
                return string.Empty;

            if( startIndex < 0 )
                return aString;

            return aString.Substring( startIndex );
        }

        public static string ReplaceFirst ( this string text, string search, string replace )
        {
            int pos = text.IndexOf( search );

            if( pos < 0 )
            {
                return text;
            }

            return text.Substring( 0, pos ) + replace + text.Substring( pos + search.Length );
        }

        public static float ToFloat( this string str, float defaultFloat = 0.0f )
        {
            float result;
            if( float.TryParse( str, NumberStyles.Number, CultureInfo.InvariantCulture, out result ) )
                return result;
            return defaultFloat;
        }
    }
}
