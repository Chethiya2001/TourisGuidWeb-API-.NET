using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Data.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Data.Repositories
{
    public class Repository<T, EID>(IMongoDatabase mongoDatabase, string collectionName) : IRepository<T, EID> where T : IEntity<EID>
    {

        private readonly IMongoCollection<T> _dbCollection = mongoDatabase.GetCollection<T>(collectionName);
        private readonly FilterDefinitionBuilder<T> _filterBuilder = Builders<T>.Filter;
        public async Task CreateAsync(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            await _dbCollection.InsertOneAsync(data);
        }

        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null)
        {
            if (filter == null)
            {
                return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
            }

            return await _dbCollection.Find(filter).ToListAsync();
        }
        public IQueryable<T> GetAllQueryable()
        {
            return _dbCollection.AsQueryable();
        }

        public async Task<T> GetAsync(EID id)
        {
            if (!ObjectId.TryParse(id!.ToString(), out var objectId))
                throw new ArgumentException("Invalid ObjectId format", nameof(id));

            var filter = _filterBuilder.Eq("_id", objectId);
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbCollection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(EID id)
        {
            if (!ObjectId.TryParse(id!.ToString(), out var objectId))
                throw new ArgumentException("Invalid ObjectId format", nameof(id));
            var filter = _filterBuilder.Eq("_id", objectId);
            await _dbCollection.DeleteOneAsync(filter);
        }

        public async Task UpdateAsync(T data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            var id = data.GetId();
            // Validate ObjectId format
            if (!ObjectId.TryParse(id!.ToString(), out var objectId))
                throw new ArgumentException("Invalid ObjectId format", nameof(id));
            var filter = _filterBuilder.Eq("_id", objectId);
            await _dbCollection.ReplaceOneAsync(filter, data);
        }
    }
}