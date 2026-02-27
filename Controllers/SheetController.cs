using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RpgSheet.Api.Dtos.Requests;
using RpgSheet.Api.Dtos.Requests.Responses;
using RpgSheet.Api.Security;
using RpgSheet.Api.UseCases.Sheet.Create;
using RpgSheet.Api.UseCases.Sheet.GetById;
using RpgSheet.Api.UseCases.Sheet.ListAll;
using RpgSheet.Api.UseCases.Sheet.UpdateVitals;
using RpgSheet.Api.UseCases.Sheet.UploadPortrait;
using RpgSheet.Api.UseCases.Skill.Create;

namespace RpgSheet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SheetController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(
        [FromBody] CreateSheetRequest req,
        [FromServices] CreateSheetUseCase useCase)
        {

            var sheet = await useCase.ExecuteAsync(req);
            return CreatedAtAction(nameof(GetById), new { id = sheet.Id }, new
            {
                sheet.Id,
                sheet.PlayerName,
                sheet.CharacterName,
                sheet.HpCurrent,
                sheet.HpMax,
                sheet.ManaCurrent,
                sheet.ManaMax
            });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            [FromRoute] Guid id,
            [FromServices] GetSheetByIdUseCase useCase)
        {
            var sheet = await useCase.ExecuteAsync(id);
            return sheet is null ? NotFound() : Ok(sheet);
        }

        // GM: lista tudo
        [HttpGet]
        public async Task<IActionResult> ListForGm(
            [FromQuery] string? gmKey,
            [FromServices] IGMKeyValidator gmKeyValidator,
            [FromServices] ListSheetsForGMUseCase useCase)
        {
            if (!gmKeyValidator.IsValid(gmKey))
                return Forbid();

            var all = await useCase.ExecuteAsync();
            return Ok(all);
        }

        // GM: atualiza PV/Mana
        [HttpPatch("{id:guid}")]
        public async Task<IActionResult> UpdateVitals(
            [FromRoute] Guid id,
            [FromQuery] string? gmKey,
            [FromBody] UpdateVitalsRequest req,
            [FromServices] IGMKeyValidator gmKeyValidator,
            [FromServices] UpdateVitalsUseCase useCase)
        {
            if (!gmKeyValidator.IsValid(gmKey))
                return Forbid();

            try
            {
                var updated = await useCase.ExecuteAsync(id, req);
                return updated is null ? NotFound() : Ok(updated);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GM: adiciona habilidade
        [HttpPost("{id:guid}/skills")]
        public async Task<IActionResult> AddSkill(
            [FromRoute] Guid id,
            [FromQuery] string? gmKey,
            [FromBody] AddSkillRequest req,
            [FromServices] IGMKeyValidator gmKeyValidator,
            [FromServices] AddSkillUseCase useCase)
        {
            if (!gmKeyValidator.IsValid(gmKey))
                return Forbid();

            try
            {
                var skill = await useCase.ExecuteAsync(id, req);
                if (skill is null) return NotFound();

                var response = new SkillResponse
                {
                    Id = skill.Id,
                    SheetId = skill.SheetId,
                    Name = skill.Name,
                    Description = skill.Description,
                    ManaCost = skill.ManaCost,
                    Cooldown = skill.Cooldown,
                    Notes = skill.Notes
                };
                return Created($"/api/sheets/{id}", response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id:guid}/portrait")]
        [RequestSizeLimit(6 * 1024 * 1024)] // 6MB
        public async Task<IActionResult> UploadPortrait(
        [FromRoute] Guid id,
        [FromQuery] string? gmKey,
        [FromForm] IFormFile file,
        [FromServices] IGMKeyValidator gmKeyValidator,
        [FromServices] UploadPortraitUseCase useCase,
        CancellationToken ct)
        {
            if (!gmKeyValidator.IsValid(gmKey))
                return Forbid();

            if (file is null || file.Length == 0)
                return BadRequest("Envie um arquivo no campo 'file'.");

            byte[] bytes;
            await using (var ms = new MemoryStream())
            {
                await file.CopyToAsync(ms, ct);
                bytes = ms.ToArray();
            }

            try
            {
                var result = await useCase.ExecuteAsync(id, bytes, file.ContentType, file.FileName);
                return result is null ? NotFound() : Ok(new { portraitPath = result.Value.Path, portraitUrl = result.Value.Url });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}