using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiemyu.Util
{
    /// <summary>
    /// Simple class that executes a lambda whenever 
    /// it is destroyed
    /// </summary>
    class Scope
    {
        Action exitAction;

        private Scope(Action action)
        {
            exitAction = action;
        }

        ~Scope()
        {
            exitAction();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        internal static Scope Create(Action p)
        {
            return new Scope(p);
        }
    }
}
