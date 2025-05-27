using EgyEagles.Application.Interfaces.Companies;
using EgyEagles.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EgyEagles.Infrastructure.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly IMongoCollection<Company> _collection;

        public CompanyRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<Company>("Companies");
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(string id)
        {
            return await _collection.Find(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Company company)
        {
            await _collection.InsertOneAsync(company);
        }

        public async Task UpdateAsync(Company company)
        {
            await _collection.ReplaceOneAsync(c => c.Id == company.Id, company);
        }

        public async Task DeleteAsync(string id)
        {
            await _collection.DeleteOneAsync(c => c.Id == id);
        }
    }

}
