using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RpgSheet.Api.Infra.Supabase
{
    public class SupabaseSettings
    {
        public string Url { get; set; } = "";
        public string ServiceRoleKey { get; set; } = "";
        public string Bucket { get; set; } = "characters_portrairs";
        public bool IsBucketPublic { get; set; } = true;
        public int SignedUrlSeconds { get; set; } = 3600;

    }
}