using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace VisionCode.Framework.Helpers.Events
{
    public static class EventsHelper
    {
        // For asynchronous event invocation
        private delegate void AsyncFireDelegate(Delegate del, params object[] args);

        private static void UnsafeFire(Delegate del, params object[] args)
        {
            if (del != null)
            {
                Delegate[] delegates = del.GetInvocationList();
                foreach (Delegate sink in delegates)
                {
                    try
                    {
                        sink.DynamicInvoke(args);
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

        private static void AsyncUnsafeFire(Delegate del, params object[] args)
        {
            if (del != null)
            {
                Delegate[] delegates = del.GetInvocationList();
                foreach (Delegate sink in delegates)
                {
                    AsyncFireDelegate asyncFire = AsyncUnsafeFireHelper;

                    // No exception will be thrown since invoking in a separate thread
                    // using BeginInvoke
                    asyncFire.BeginInvoke(sink, args, null, null);
                }
            }
        }

        private static void AsyncUnsafeFireHelper(Delegate del, params object[] args)
        {
            del.DynamicInvoke(args);
        }

        public static void AsyncFire<T>(EventHandler<T> handler, object sender, T e) where T : EventArgs
        {
            AsyncUnsafeFire(handler, sender, e);
        }

        public static void Fire<T>(EventHandler<T> handler, object sender, T e) where T : EventArgs
        {
            UnsafeFire(handler, sender, e);
        }

        public static void Fire(PropertyChangedEventHandler handler, object sender, PropertyChangedEventArgs e)
        {
            UnsafeFire(handler, sender, e);
        }
    }
}
