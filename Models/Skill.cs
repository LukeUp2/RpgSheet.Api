using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgSheet.Api.Models
{
    public class Skill
    {
        public Guid Id { get; set; }
        public Guid SheetId { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int? ManaCost { get; set; }
        public string? Cooldown { get; set; }

        public string? Notes { get; set; }

        public Sheet? Sheet { get; set; }
    }
}