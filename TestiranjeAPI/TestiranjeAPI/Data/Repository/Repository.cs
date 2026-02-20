using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TestiranjeAPI.Models;
using TestiranjeAPI.Repository.Interfaces;

namespace TestiranjeAPI.Repository;

public class Repository<T> : IRepository<T>
    where T : class
{
    protected readonly PartyContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(PartyContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }
}