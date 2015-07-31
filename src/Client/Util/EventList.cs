using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiemyu.Util
{
    /// <summary>
    /// Easy definition for holder of event lists. When this list goes out of scope
    /// all of the event handlers are guranteed to clean up
    /// </summary>
    class EventList : List<Scope>
    {
    }
}
