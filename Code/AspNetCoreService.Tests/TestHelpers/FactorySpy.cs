using System;
using FluentAssertions;

namespace AspNetCoreService.Tests.TestHelpers
{
    public sealed class FactorySpy<T>
    {
        public FactorySpy(T instance)
        {
            Instance = instance;
            GetInstance = GetInstanceInternal;
        }

        public T Instance { get; }

        public Func<T> GetInstance { get; }

        public int GetInstanceCallCount { get; private set; }

        private T GetInstanceInternal()
        {
            GetInstanceCallCount++;
            return Instance;
        }

        public void InstanceMustNotHaveBeenCreated() =>
            GetInstanceCallCount.Should().Be(0);
    }
}