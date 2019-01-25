using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSSample.Commands
{
    public class LoginCommand:IRequest<bool>
    {
        public string UserName { get; set; }
    }
}
