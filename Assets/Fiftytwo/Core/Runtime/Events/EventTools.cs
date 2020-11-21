using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace Fiftytwo
{
    public delegate void EventHandler<TSender, TArgs>( TSender sender, TArgs args );
    public delegate void EventHandlerVoid();
    public delegate void EventHandlerSender<TSender>( TSender sender );
    public delegate void EventHandlerArgs<TArgs>( TArgs args );

    public static class EventTools
    {
        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify<TSender, TArgs> ( this EventHandler<TSender, TArgs> handler, TSender sender, TArgs args )
        {
            if( handler != null )
                handler( sender, args );
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify ( this EventHandlerVoid handler )
        {
            if( handler != null )
                handler();
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify<TSender> ( this EventHandlerSender<TSender> handler, TSender sender )
        {
            if( handler != null )
                handler( sender );
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify<TArgs> ( this EventHandlerArgs<TArgs> handler, TArgs args )
        {
            if( handler != null )
                handler( args );
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify ( this EventHandler handler, object sender, EventArgs args )
        {
            if( handler != null )
                handler( sender, args );
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify ( this EventHandler handler, object sender )
        {
            if( handler != null )
                handler( sender, EventArgs.Empty );
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify<T> ( this EventHandler<T> handler, object sender, T args ) where T : EventArgs
        {
            if( handler != null )
                handler( sender, args );
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify ( this PropertyChangedEventHandler handler, object sender, PropertyChangedEventArgs args )
        {
            if( handler != null )
                handler( sender, args );
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void Notify ( this PropertyChangedEventHandler handler, object sender, string name )
        {
            if( handler != null )
                handler( sender, new PropertyChangedEventArgs( name ) );
        }

        [MethodImpl( MethodImplOptions.NoInlining )]
        public static void ChangePropertyAndNotify<T> ( this PropertyChangedEventHandler handler,
            INotifyPropertyChanged sender, string propertyName, ref T backingStore, T value )
        {
            AssertCalledFromPropertySetter( propertyName );

            if( EqualityComparer<T>.Default.Equals( backingStore, value ) )
                return;

            backingStore = value;

            if( handler != null )
                handler( sender, new PropertyChangedEventArgs( propertyName ) );
        }

        [Conditional( "DEBUG" )]
        private static void AssertCalledFromPropertySetter ( string propertyName )
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrames()[2];
            var caller = frame.GetMethod();

            Assert.AreEqual( caller.Name, "set_" + propertyName,
                "Called SetProperty for " + propertyName + " from " + caller.Name );
        }
    }
}
