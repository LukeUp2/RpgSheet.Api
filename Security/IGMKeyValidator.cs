using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgSheet.Api.Security
{
    public interface IGMKeyValidator
    {
        bool IsValid(string? gmKey);
    }
}