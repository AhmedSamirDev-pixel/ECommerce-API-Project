using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Exceptions
{
    public class BadRequestException(List<string> errors) : Exception("Validation Faild")
    {
        public List<string> Errors = errors;
    }
}
