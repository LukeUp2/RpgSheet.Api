using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgSheet.Api.Dtos.Requests
{
    public class CreateSheetRequest
    {
        public string PlayerName { get; set; } = "";
        public string CharacterName { get; set; } = "";
        public int HpMax { get; set; }
        public int ManaMax { get; set; }
    }
}