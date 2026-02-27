using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgSheet.Api.Security
{
    public class GMKeyValidator : IGMKeyValidator
    {
        private readonly IConfiguration _config;

        public GMKeyValidator(IConfiguration config)
        {
            _config = config;
        }
        public bool IsValid(string? gmKey)
        {
            var expectedKey = _config.GetSection("GM_KEY").Value;
            return !string.IsNullOrWhiteSpace(expectedKey) && gmKey == expectedKey;
        }
    }
}