using System;

namespace AspNetCoreService.CoreModel
{
    public abstract class Entity<T> : IEquatable<T>
        where T : Entity<T>
    {
        public int Id { get; init; }

        public bool Equals(T? other) =>
            other is not null && Id == other.Id;

        public override bool Equals(object? obj) =>
            obj is T entity && Equals(entity);

        public override int GetHashCode() => Id;
    }
}