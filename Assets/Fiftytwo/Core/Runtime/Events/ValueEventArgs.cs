using System;

namespace Fiftytwo
{
    public class ValueEventArgs<T> : EventArgs
    {
        public readonly T Value;

        public ValueEventArgs ( T value )
        {
            Value = value;
        }

        public static implicit operator ValueEventArgs<T> ( T value )
        {
            return new ValueEventArgs<T>( value );
        }

        public static implicit operator T ( ValueEventArgs<T> args )
        {
            return args.Value;
        }
    }
}
