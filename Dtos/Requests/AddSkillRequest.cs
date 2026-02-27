using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgSheet.Api.Dtos.Requests
{
    public class AddSkillRequest
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public int? ManaCost { get; set; }
        public string? Cooldown { get; set; }
        public string? Notes { get; set; }
    }
}