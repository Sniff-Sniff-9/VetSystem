using Microsoft.EntityFrameworkCore;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class PetsService/*: IPetsService*/
    {

        private readonly AppDbContext _context;
        private readonly ILogger<PetsService> _logger;

        public PetsService(AppDbContext context, ILogger<PetsService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<PetDto>> GetPetsAsync()
        { 
            var pets = await _context.Pets.Include(p => p.Species).Include(p => p.Gender).ToListAsync();
            return pets.Select(p => ToPetDto(p)).ToList();
        }

        public async Task<PetDto?> GetPetByIdIdAsync(int id)
        {
            var pet = await _context.Pets.Include(p => p.Species).Include(p => p.Gender).FirstOrDefaultAsync(p => p.PetId == id);
            if (pet == null)
            {
                return null;
            }
            return ToPetDto(pet);
        }
        
        public async Task<List<PetDto>> GetPetsByClientIdAsync(int id)
        {
            var clientPets = await _context.Pets.Include(p => p.Species).Include(p => p.Gender).Where(p => p.ClientId == id).ToListAsync() ;
            return clientPets.Select(p => ToPetDto(p)).ToList();
        }

        public async Task<PetDto> CreatePetAsync(PetDto petDto, int clientId)
        {
            if (petDto.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException($"Birth date can't be larger than {DateOnly.FromDateTime(DateTime.UtcNow)}");
            }
            var species = await _context.Species.FirstOrDefaultAsync(s => s.SpeciesName == petDto.SpeciesName);
            var gender = await _context.Genders.FirstOrDefaultAsync(g => g.GenderName == petDto.GenderName);

            if (gender == null)
            {
                throw new ArgumentException("Gender doesn't exist");
            }
            if (species == null)
            {
                throw new ArgumentException("Species doesn't exist");
            }

            var pet = new Pet
            {
                Name = petDto.Name,
                SpeciesId = species.SpeciesId,
                Breed = petDto.Breed,
                BirthDate = petDto.BirthDate,
                GenderId = gender.GenderId,
                ClientId = clientId
            };
            try
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                return ToPetDto(pet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pet can't be created.");
                throw;
            }
        }

        
        //public Task<PetDto> UpdatePetAsync(int id, PetDto petDto);
        //public Task DeletePetAsync(int id);

        private PetDto ToPetDto(Pet pet)
        {
            return new PetDto
            {
                Name = pet.Name,
                SpeciesName = pet.Species?.SpeciesName ?? "undefined",
                Breed = pet.Breed,
                BirthDate = pet.BirthDate,
                GenderName = pet.Gender?.GenderName ?? "undefined"
            };
        }
    }
}
