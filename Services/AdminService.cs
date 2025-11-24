// TaskOrganizer.Services/AdminService.cs
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskOrganizer.Models;
using System.Threading.Tasks; // <--- KRITIKAL: Idinagdag para sa Task return type

namespace TaskOrganizer.Services
{
    public class AdminService
    {
        private readonly IMongoCollection<Admin> _admins;

        public AdminService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _admins = database.GetCollection<Admin>("Admins");
        }

        public async Task<Admin?> GetByEmailAsync(string email)
        {
            return await _admins.Find(a => a.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Admin admin)
        {
            await _admins.InsertOneAsync(admin);
            // Walang return statement dahil ang return type ay Task (parang void)
        }

        // Para sa Login Validation
        public async Task<bool> ValidateLogin(string email, string passwordHash)
        {
            var admin = await _admins.Find(a => a.Email == email && a.PasswordHash == passwordHash)
                                     .FirstOrDefaultAsync();
            return admin != null;
        }
    }
}