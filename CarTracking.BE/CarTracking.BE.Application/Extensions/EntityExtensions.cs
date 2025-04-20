using CarTracking.BE.Domain;
using CarTracking.BE.Infrastructure.Exceptions;

namespace CarTracking.BE.Application.Extensions;

public static class EntityExtensions
{
    public static void ValidateIfNotNull<T>(this T? entity, string id) where T : Entity
    {
        if (entity is null)
        {
            throw new NotFoundException($"Entity {typeof(T).Name} with Id: {id} was not found");
        }
    }   
}