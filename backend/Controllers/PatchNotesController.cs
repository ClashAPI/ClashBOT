using System;
using System.IO;
using System.Threading.Tasks;
using backend.Helpers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/v1/patch-notes")]
    [Authorize]
    [TypeFilter(typeof(SuperuserFilter))]
    public class PatchNotesController : ControllerBase
    {
        private readonly IPatchNoteService _patchNoteService;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<User> _userManager;

        public PatchNotesController(
            IPatchNoteService patchNoteService, 
            IWebHostEnvironment env, 
            UserManager<User> userManager)
        {
            _patchNoteService = patchNoteService;
            _env = env;
            _userManager = userManager;
        }

        [HttpPost("image-upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile formFile)
        {
            var extension = Path.GetExtension(formFile.FileName);
            var subPath = Path.GetRandomFileName() + extension;
            var filePath = Path.Combine(_env.ContentRootPath, "Static", subPath);
            using (var stream = System.IO.File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }

            return Ok(new {url = "https://localhost:5001/Static/" + subPath});
        }
        
        [HttpGet]
        public async Task<IActionResult> GetPatchNotes()
        {
            return Ok(await _patchNoteService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatchNoteById([FromRoute] Guid id)
        {
            return Ok(await _patchNoteService.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PatchNote patchNote)
        {
            patchNote.Author = await _userManager.GetUserAsync(User);
            return Ok(await _patchNoteService.AddAsync(patchNote));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] PatchNote patchNote)
        {
            return Ok(await _patchNoteService.UpdateByIdAsync(id, patchNote));
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            await _patchNoteService.DeleteByIdAsync(id);
            return Ok();
        }
    }
}