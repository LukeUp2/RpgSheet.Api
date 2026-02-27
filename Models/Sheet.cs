using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgSheet.Api.Models
{
    public class Sheet
    {
        public Guid Id { get; set; }

        public string PlayerName { get; set; } = string.Empty;
        public string CharacterName { get; set; } = string.Empty;

        public int HpCurrent { get; set; }
        public int HpMax { get; set; }

        public int ManaCurrent { get; set; }
        public int ManaMax { get; set; }

        public string? PortraitUrl { get; set; }
        public string? PortraitPath { get; set; }

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        public List<Skill> Skills { get; set; } = new();
    }
}