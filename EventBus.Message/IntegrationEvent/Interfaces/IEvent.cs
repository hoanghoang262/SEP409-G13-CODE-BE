using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.IntegrationEvent.Interfaces
{
    public interface IEvent : IIntegrationEvent
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
    }
}
