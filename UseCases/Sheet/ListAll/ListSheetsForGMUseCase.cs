using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgSheet.Api.Data.Repositories;

namespace RpgSheet.Api.UseCases.Sheet.ListAll
{
    public class ListSheetsForGMUseCase
    {
        private readonly SheetRepository _sheetsRepository;

        public ListSheetsForGMUseCase(SheetRepository sheets) => _sheetsRepository = sheets;

        public Task<List<Models.Sheet>> ExecuteAsync()
            => _sheetsRepository.ListAllWithSkillsAsync();
    }
}