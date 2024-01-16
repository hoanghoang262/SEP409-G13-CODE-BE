using EventBus.Message.IntegrationEvent.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.IntegrationEvent.Event
{
    public record Event() : IntegrationBaseEvent, IEvent
    {
        public string UserName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string UserEmail { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


    }
}
