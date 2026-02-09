using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class DictionaryEntityService<TEntity> where TEntity : class
    {
        private readonly AppDbContext _context;

        public DictionaryEntityService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            var entities = await _context.Set<TEntity>().ToListAsync();
            return entities;
        }

    }
}
