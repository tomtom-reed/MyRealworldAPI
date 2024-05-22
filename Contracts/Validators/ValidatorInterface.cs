using Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Validators
{
    internal interface ValidatorInterface
    {
        public bool Validate();
        public ErrorDetails GetError();
    }
}
