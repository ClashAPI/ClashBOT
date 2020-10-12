using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Services
{
    public interface IPatchNoteService
    {
        Task<IList<PatchNote>> GetAllAsync();
        Task<PatchNote> GetByIdAsync(Guid id);
        Task DeleteByIdAsync(Guid id);
        Task<PatchNote> UpdateByIdAsync(Guid id, PatchNote patchNote);
        Task<PatchNote> AddAsync(PatchNote patchNote);
    }
}