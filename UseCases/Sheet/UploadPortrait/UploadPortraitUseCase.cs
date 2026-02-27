using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RpgSheet.Api.Data.Repositories;
using RpgSheet.Api.Infra.Storage;

namespace RpgSheet.Api.UseCases.Sheet.UploadPortrait
{
    public class UploadPortraitUseCase
    {
        private static readonly HashSet<string> AllowedTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/png", "image/jpeg", "image/webp"
    };

        private const long MaxBytes = 5 * 1024 * 1024; // 5MB

        private readonly SheetRepository _sheetsRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly SupabaseStorage _storage;

        public UploadPortraitUseCase(SheetRepository sheets, UnitOfWork uow, SupabaseStorage storage)
        {
            _sheetsRepository = sheets;
            _unitOfWork = uow;
            _storage = storage;
        }

        public async Task<(string Path, string Url)?> ExecuteAsync(Guid sheetId, byte[] fileBytes, string contentType, string fileName)
        {
            var sheet = await _sheetsRepository.GetByIdAsync(sheetId);
            if (sheet is null) return null;

            if (!AllowedTypes.Contains(contentType))
                throw new ArgumentException("Tipo de imagem inválido. Use PNG, JPEG ou WEBP.");

            if (fileBytes.LongLength <= 0 || fileBytes.LongLength > MaxBytes)
                throw new ArgumentException("Arquivo inválido ou muito grande (máx 5MB).");

            var uploaded = await _storage.UploadPortraitAsync(sheetId, fileBytes, contentType, fileName);

            sheet.PortraitPath = uploaded.Path;
            sheet.PortraitUrl = uploaded.Url;
            sheet.UpdatedAtUtc = DateTime.UtcNow;

            await _unitOfWork.CommitAsync();

            return (uploaded.Path, uploaded.Url);
        }
    }
}