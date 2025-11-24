using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TaskOrganizer.Models;
using System.Threading.Tasks;

namespace TaskOrganizer.Services
{
    public class EmployeeService
    {
        private readonly IMongoCollection<Employee> _employees;

        public EmployeeService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _employees = database.GetCollection<Employee>("Employees");
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            return await _employees.Find(e => e.Email == email).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Employee employee)
        {
            await _employees.InsertOneAsync(employee);
        }

        public async Task<bool> ValidateLogin(string email, string passwordHash)
        {
            var employee = await _employees.Find(e => e.Email == email && e.PasswordHash == passwordHash)
                                         .FirstOrDefaultAsync();
            return employee != null;
        }

    }
}