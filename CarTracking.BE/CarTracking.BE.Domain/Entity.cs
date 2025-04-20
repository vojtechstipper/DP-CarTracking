using CarTracking.BE.Domain.Utils;

namespace CarTracking.BE.Domain;

public class Entity
{
    public string Id { get; set; } = IdGenerator.GenerateId();
}