using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgSheet.Api.Models;

namespace RpgSheet.Api.Data.Repositories
{
    public class SkillRepository
    {
        private readonly AppDbContext _context;

        public SkillRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Skill skill)
        {
            _context.Skills.Add(skill);
            return Task.CompletedTask;
        }
    }
}