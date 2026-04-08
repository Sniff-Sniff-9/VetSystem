using Microsoft.EntityFrameworkCore;
using VetSystemApi.Services.Interfaces;
using VetSystemInfrastructure.Configuration;
using VetSystemModels.Dto.Pet;
using VetSystemModels.Entities;

namespace VetSystemApi.Services
{
    public class PetsService : IPetsService
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
            var pets = await _context.Pets.Include(p => p.Species).Include(p => p.Gender).Include(p => p.Client).ToListAsync();
            return pets.Select(p => ToPetDto(p)).ToList();
        }

        public async Task<PetDto?> GetPetByIdAsync(int id)
        {
            var pet = await _context.Pets.Include(p => p.Species).Include(p => p.Gender).Include(p => p.Client).FirstOrDefaultAsync(p => p.PetId == id);
            if (pet == null)
            {
                return null;
            }
            return ToPetDto(pet);
        }
        
        public async Task<List<PetDto>> GetPetsByClientIdAsync(int id)
        {
            var clientPets = await _context.Pets.Include(p => p.Species).Include(p => p.Gender).Include(p => p.Client).Where(p => p.ClientId == id).ToListAsync() ;
            return clientPets.Select(p => ToPetDto(p)).ToList();
        }

        public async Task<PetDto> CreatePetAsync(CreateUpdatePetDto petDto)
        {
            if (petDto.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ArgumentException($"Birth date can't be larger than {DateOnly.FromDateTime(DateTime.UtcNow)}.");
            }

            var speciesExists = await _context.Species.AnyAsync(s => s.SpeciesId == petDto.SpeciesId);
            var genderExists = await _context.Genders.AnyAsync(g => g.GenderId == petDto.GenderId);
            var clientExists = await _context.Clients.AnyAsync(c => c.ClientId == petDto.ClientId);

            if (!genderExists)
            {
                throw new ArgumentException("Gender doesn't exist.");
            }
            if (!speciesExists)
            {
                throw new ArgumentException("Species doesn't exist.");
            }
            if (!clientExists)
            {
                throw new ArgumentException("Client doesn't exist.");
            }

            var pet = new Pet
            {
                Name = petDto.Name,
                SpeciesId = petDto.SpeciesId,
                Breed = petDto.Breed,
                BirthDate = petDto.BirthDate,
                GenderId = petDto.GenderId,
                ClientId = petDto.ClientId
            };
            try
            {
                _context.Add(pet);
                await _context.SaveChangesAsync();
                var result = await _context.Pets.Include(p => p.Species).Include(p => p.Gender).Include(p => p.Client).FirstAsync(p => p.PetId == pet.PetId);
                return ToPetDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pet can't be created.");
                throw;
            }
        }


        public async Task<PetDto> UpdatePetAsync(int id, CreateUpdatePetDto petDto)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(u => u.PetId == id);
            var speciesExists = await _context.Species.AnyAsync(s => s.SpeciesId == petDto.SpeciesId);
            var genderExists = await _context.Genders.AnyAsync(g => g.GenderId == petDto.GenderId);
            var clientExists = await _context.Clients.AnyAsync(c => c.ClientId == petDto.ClientId);

            if (!genderExists)
            {
                throw new ArgumentException("Gender doesn't exist.");
            }
            if (!speciesExists)
            {
                throw new ArgumentException("Species doesn't exist.");
            }
            if (!clientExists)
            {
                throw new ArgumentException("Client doesn't exist.");
            }

           
            if (pet == null)
            {
                throw new ArgumentNullException("Pet not found.");
            }

            pet.Name = petDto.Name;
            pet.SpeciesId = petDto.SpeciesId;
            pet.GenderId = petDto.GenderId;
            pet.Breed = petDto.Breed;
            pet.BirthDate = petDto.BirthDate;
            pet.ClientId = petDto.ClientId;

            try
            {
                await _context.SaveChangesAsync();
                var result = await _context.Pets.Include(p => p.Species).Include(p => p.Gender).Include(p => p.Client).FirstAsync(p => p.PetId == pet.PetId);
                return ToPetDto(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pet can't be updated.");
                throw;
            }
        }

        public async Task DeletePetAsync(int id)
        {
            var pet = await _context.Pets.FirstOrDefaultAsync(p => p.PetId == id);
            if (pet == null)
            {
                throw new ArgumentNullException("Pet not found.");
            }
            pet.IsDeleted = true;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Pet can't be deleted.");
                throw;
            }
        }

        private PetDto ToPetDto(Pet pet)
        {
            return new PetDto
            {
                PetId = pet.PetId,
                Name = pet.Name,
                SpeciesId = pet.SpeciesId,
                SpeciesName = pet.Species?.SpeciesName ?? "undefined",
                Breed = pet.Breed,
                BirthDate = pet.BirthDate,
                GenderId = pet.GenderId,
                GenderName = pet.Gender?.GenderName ?? "undefined",
                ClientId = pet.ClientId,
                ClientName = pet.Client != null 
                ? $"{pet.Client.LastName} {pet.Client.FirstName} {pet.Client.MiddleName}" : "undefined",
                ClientUserId = pet.Client?.UserId ?? 0
            };
        }
    }
}
