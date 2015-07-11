using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiemyuDll.Network.Messages
{
    /// <summary>
    /// Class for messages targeting an entity
    /// </summary>
    [Serializable()]
    public class EntityMessage : Message
    {
        public Guid EntityId { get; set; }
    }
}
