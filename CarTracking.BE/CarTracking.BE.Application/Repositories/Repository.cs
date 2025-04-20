using CarTracking.BE.Infrastructure;
using CarTracking.BE.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CarTracking.BE.Application.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly CarTrackingDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(CarTrackingDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetById(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Add(T entity)
    {
        _dbSet.Add(entity);
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