using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgSheet.Api.Data.Repositories;

namespace RpgSheet.Api.UseCases.Sheet.GetById
{
    public class GetSheetByIdUseCase
    {
        private readonly SheetRepository _sheetsRepository;

        public GetSheetByIdUseCase(SheetRepository sheets) => _sheetsRepository = sheets;

        public Task<Models.Sheet?> ExecuteAsync(Guid id)
            => _sheetsRepository.GetByIdWithSkillsAsync(id);
    }
}