using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgSheet.Api.Data.Repositories;
using RpgSheet.Api.Dtos.Requests;

namespace RpgSheet.Api.UseCases.Skill.Create
{
    public class AddSkillUseCase
    {
        private readonly SheetRepository _sheetsRepository;
        private readonly SkillRepository _skillsRepository;
        private readonly UnitOfWork _unityOfWork;

        public AddSkillUseCase(SheetRepository sheets, SkillRepository skills, UnitOfWork uow)
        {
            _sheetsRepository = sheets;
            _skillsRepository = skills;
            _unityOfWork = uow;
        }

        public async Task<Models.Skill?> ExecuteAsync(Guid sheetId, AddSkillRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                throw new ArgumentException("Name é obrigatório.");

            var sheet = await _sheetsRepository.GetByIdAsync(sheetId);

            if (sheet is null) return null;

            var skill = new Models.Skill
            {
                SheetId = sheetId,
                Name = req.Name.Trim(),
                Description = req.Description ?? string.Empty,
                ManaCost = req.ManaCost,
                Cooldown = req.Cooldown,
                Notes = req.Notes ?? string.Empty
            };

            await _skillsRepository.AddAsync(skill);

            sheet.UpdatedAtUtc = DateTime.UtcNow;
            await _unityOfWork.CommitAsync();

            return skill;
        }
    }
}