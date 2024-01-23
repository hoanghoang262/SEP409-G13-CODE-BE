using EventBus.Message.IntegrationEvent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.IntegrationEvent.Event
{
    public record LoginEvent() : IntegrationBaseEvent, ILoginEvent
    {
        public string UserName { get ; set ; }
        public string PassWord { get ; set ; }
    }
}
