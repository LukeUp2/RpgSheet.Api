using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgSheet.Api.Dtos.Requests
{
    public class UpdateVitalsRequest
    {
        public int? HpCurrent { get; set; }
        public int? HpMax { get; set; }
        public int? ManaCurrent { get; set; }
        public int? ManaMax { get; set; }
    }
}