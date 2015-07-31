using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiemyu.Util
{
    /// <summary>
    /// Class that encapsulates an event, making it easy to subscribe/unsubscribe
    /// </summary>
    class EventInfo<TSource, TEventHandler>
    {
        private Action<TSource, TEventHandler> fnAdd;
        private Action<TSource, TEventHandler> fnRemove;

        public EventInfo(Action<TSource, TEventHandler> fnAddHandler, Action<TSource, TEventHandler> fnRemoveHandler)
        {
            fnAdd = fnAddHandler;
            fnRemove = fnRemoveHandler;
        }

        protected void AddHandler(TSource source, TEventHandler handler)
        {
            fnAdd(source, handler);
        }

        protected void RemoveHandler(TSource source, TEventHandler handler)
        {
            fnRemove(source, handler);
        }

        public Scope Subscribe(TSource source, TEventHandler handler)
        {
            AddHandler(source, handler);
            return Scope.Create(() => RemoveHandler(source, handler));
        }
    }
}
