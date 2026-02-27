using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RpgSheet.Api.Infra.Supabase;
using Supabase;
using StorageFileOptions = Supabase.Storage.FileOptions;

namespace RpgSheet.Api.Infra.Storage
{
    public class SupabaseStorage
    {
        private readonly SupabaseSettings _settings;
        private readonly Client _client;

        public SupabaseStorage(global::Supabase.Client client, IOptions<SupabaseSettings> options)
        {
            _settings = options.Value;

            _client = client;
        }

        public async Task<UploadObjectResult> UploadPortraitAsync(
            Guid sheetId,
            byte[] bytes,
            string contentType,
            string originalFileName)
        {
            // Extensão (fallback)
            var ext = Path.GetExtension(originalFileName);
            if (string.IsNullOrWhiteSpace(ext)) ext = contentType switch
            {
                "image/png" => ".png",
                "image/jpeg" => ".jpg",
                "image/webp" => ".webp",
                _ => ".bin"
            };

            var objectPath = $"sheets/{sheetId}/portrait{ext}".ToLowerInvariant();

            // Upload bytes
            await _client.Storage
                .From(_settings.Bucket)
                .Upload(
                bytes,
                objectPath,
                new StorageFileOptions
                {
                    ContentType = contentType,
                    Upsert = true
                }
            );

            // URL final
            if (_settings.IsBucketPublic)
            {
                var publicUrl = _client.Storage
                    .From(_settings.Bucket)
                    .GetPublicUrl(objectPath);

                return new UploadObjectResult(objectPath, publicUrl);
            }
            else
            {
                // URL assinada (expira)
                var signed = await _client.Storage
                    .From(_settings.Bucket)
                    .CreateSignedUrl(objectPath, _settings.SignedUrlSeconds);

                return new UploadObjectResult(objectPath, signed);
            }
        }
    }
}