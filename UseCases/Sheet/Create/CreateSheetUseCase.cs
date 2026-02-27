using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgSheet.Api.Data.Repositories;
using RpgSheet.Api.Dtos.Requests;

namespace RpgSheet.Api.UseCases.Sheet.Create
{
    public class CreateSheetUseCase
    {
        private readonly SheetRepository _sheetsRepository;
        private readonly UnitOfWork _unityOfWork;

        public CreateSheetUseCase(SheetRepository sheets, UnitOfWork unityOfWork)
        {
            _sheetsRepository = sheets;
            _unityOfWork = unityOfWork;
        }

        public async Task<Models.Sheet> ExecuteAsync(CreateSheetRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.PlayerName) || string.IsNullOrWhiteSpace(req.CharacterName))
                throw new ArgumentException("PlayerName e CharacterName são obrigatórios.");

            if (req.HpMax <= 0) throw new ArgumentException("HpMax deve ser > 0.");
            if (req.ManaMax < 0) throw new ArgumentException("ManaMax deve ser >= 0.");

            var sheet = new Models.Sheet
            {
                PlayerName = req.PlayerName.Trim(),
                CharacterName = req.CharacterName.Trim(),
                HpMax = req.HpMax,
                HpCurrent = req.HpMax,
                ManaMax = req.ManaMax,
                ManaCurrent = req.ManaMax,
                UpdatedAtUtc = DateTime.UtcNow
            };

            await _sheetsRepository.AddAsync(sheet);
            await _unityOfWork.CommitAsync();
            return sheet;
        }
    }
}