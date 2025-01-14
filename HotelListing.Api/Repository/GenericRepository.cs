﻿using HotelListing.Api.Contracts;
using HotelListing.Api.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.Api.Repository
{
    public class GenericRepository<T>  : IGenericRepository<T> where T : class
    {
        private readonly HotelListingDbContext _context;
        public GenericRepository(HotelListingDbContext context)
        {
           this._context=context;
        }

        public async Task<T> GetAsync(int id)
        {
            if (id == null)
            {
                return null;
            }

            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity; 
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var enitiy = await GetAsync(id);
             _context.Set<T>().Remove(enitiy);
             _context.SaveChangesAsync();
        }    

        public async Task<bool> Exists(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;

        }
    }
}
