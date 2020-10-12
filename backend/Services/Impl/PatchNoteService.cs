using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class PatchNoteService : IPatchNoteService
    {
        private readonly IPatchNoteRepository _patchNoteRepository;

        public PatchNoteService(IPatchNoteRepository patchNoteRepository)
        {
            _patchNoteRepository = patchNoteRepository;
        }
        public async Task<IList<PatchNote>> GetAllAsync()
        {
            return await _patchNoteRepository.GetAllAsync();
        }

        public async Task<PatchNote> GetByIdAsync(Guid id)
        {
            return await _patchNoteRepository.GetByIdAsync(id);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            await _patchNoteRepository.DeleteByIdAsync(id);
            await _patchNoteRepository.SaveAllAsync();
        }

        public async Task<PatchNote> UpdateByIdAsync(Guid id, PatchNote patchNote)
        {
            return await _patchNoteRepository.UpdateByIdAsync(id, patchNote);
        }

        public async Task<PatchNote> AddAsync(PatchNote patchNote)
        {
            var dbObject = await _patchNoteRepository.AddAsync(patchNote);
            var result = await _patchNoteRepository.SaveAllAsync();

            return result ? dbObject : null;
        }
    }
}