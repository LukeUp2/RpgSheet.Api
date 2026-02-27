using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgSheet.Api.Data.Repositories;
using RpgSheet.Api.Dtos.Requests;

namespace RpgSheet.Api.UseCases.Sheet.UpdateVitals
{
    public class UpdateVitalsUseCase
    {
        private readonly SheetRepository _sheetsRepository;
        private readonly UnitOfWork _unityOfWork;

        public UpdateVitalsUseCase(SheetRepository sheets, UnitOfWork uow)
        {
            _sheetsRepository = sheets;
            _unityOfWork = uow;
        }

        public async Task<Models.Sheet?> ExecuteAsync(Guid id, UpdateVitalsRequest req)
        {
            var sheet = await _sheetsRepository.GetByIdAsync(id);
            if (sheet is null) return null;

            if (req.HpMax is not null && req.HpMax <= 0) throw new ArgumentException("HpMax deve ser > 0.");
            if (req.ManaMax is not null && req.ManaMax < 0) throw new ArgumentException("ManaMax deve ser >= 0.");

            if (req.HpMax is not null) sheet.HpMax = req.HpMax.Value;
            if (req.ManaMax is not null) sheet.ManaMax = req.ManaMax.Value;
            if (req.HpCurrent is not null) sheet.HpCurrent = req.HpCurrent.Value;
            if (req.ManaCurrent is not null) sheet.ManaCurrent = req.ManaCurrent.Value;

            sheet.HpCurrent = Math.Clamp(sheet.HpCurrent, 0, sheet.HpMax);
            sheet.ManaCurrent = Math.Clamp(sheet.ManaCurrent, 0, sheet.ManaMax);

            sheet.UpdatedAtUtc = DateTime.UtcNow;
            await _unityOfWork.CommitAsync();

            return sheet;
        }
    }
}