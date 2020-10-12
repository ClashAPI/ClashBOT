using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories.Impl
{
    public class PatchNoteRepository : IPatchNoteRepository
    {
        private readonly ApplicationDbContext _context;

        public PatchNoteRepository(ApplicationDbContext context)
        {
            _context = context;
            _context = context;
        }
        
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<IList<PatchNote>> GetAllAsync()
        {
            return await _context.PatchNotes.ToListAsync();
        }

        public async Task<PatchNote> GetByIdAsync(Guid id)
        {
            return await _context.PatchNotes.FirstAsync(p => p.Id == id);
        }

        public async Task DeleteByIdAsync(Guid id)
        {
            var patchNote = await _context.PatchNotes.FirstAsync(p => p.Id == id);
            _context.Remove(patchNote);
        }

        public async Task<PatchNote> UpdateByIdAsync(Guid id, PatchNote patchNote)
        {
            try
            {
                var dbObject = await GetByIdAsync(id);
                dbObject.Title = patchNote.Title;
                dbObject.Content = patchNote.Content;
                dbObject.CommitId = patchNote.CommitId;

                await SaveAllAsync();
                return dbObject;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<PatchNote> AddAsync(PatchNote patchNote)
        {
            var result = await _context.PatchNotes.AddAsync(patchNote);
            return result.Entity;
        }
    }
}