using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Service
{
    public interface IEmailService<T> where T : class
    {
        Task SendEmailasync(T request, CancellationToken cancellationToken = new CancellationToken());
    }
}
