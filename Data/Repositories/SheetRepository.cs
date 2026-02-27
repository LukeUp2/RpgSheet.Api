using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RpgSheet.Api.Models;

namespace RpgSheet.Api.Data.Repositories
{
    public class SheetRepository
    {
        private readonly AppDbContext _context;

        public SheetRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(Sheet sheet)
        {
            _context.Sheets.Add(sheet);
            return Task.CompletedTask;
        }

        public Task<Sheet?> GetByIdAsync(Guid id)
        {
            return _context.Sheets.FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<Sheet?> GetByIdWithSkillsAsync(Guid id)
        {
            return _context.Sheets
                .Include(s => s.Skills)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return _context.Sheets.AnyAsync(s => s.Id == id);
        }

        public Task<List<Sheet>> ListAllWithSkillsAsync()
        {
            return _context.Sheets
                .Include(s => s.Skills)
                .OrderByDescending(s => s.UpdatedAtUtc)
                .ToListAsync();
        }
    }
}