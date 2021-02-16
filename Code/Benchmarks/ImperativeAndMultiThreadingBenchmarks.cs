using System;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;

namespace Benchmarks
{
    public class ImperativeAndMultiThreadingBenchmarks
    {
        private static readonly object LockObject = new();
        public static int Count;
        private Counter? _counter;
        private BaseCounter? _counterViaBaseClass;
        private Func<int>? _counterViaDelegate;
        private ICounter? _counterViaInterface;


        [Benchmark (Baseline = true)]
        public int CallStaticMethod() => IncrementCount();

        private static int IncrementCount() => Count++;

        [GlobalSetup(Target = nameof(CallInstanceMethod))]
        public void SetupCounter() => _counter = new Counter(Count);

        [Benchmark]
        public int CallInstanceMethod() => _counter!.Increment();

        [GlobalSetup(Target = nameof(CallInterfaceMethod))]
        public void SetupCounterViaInterface() => _counterViaInterface = new Counter(Count);

        [Benchmark]
        public int CallInterfaceMethod() => _counterViaInterface!.IncrementViaInterfaceCall();

        [GlobalSetup(Target = nameof(CallViaOverriddenMethod))]
        public void SetupCounterViaBaseClass() => _counterViaBaseClass = new Counter(Count);

        [Benchmark]
        public int CallViaOverriddenMethod() => _counterViaBaseClass!.IncrementViaOverriddenMethod();

        [GlobalSetup(Target = nameof(CallDelegate))]
        public void SetupCounterViaDelegate() => _counterViaDelegate = IncrementCount;

        [Benchmark]
        public int CallDelegate() => _counterViaDelegate!();

        [Benchmark]
        public int InstantiateStructAndCallMethod()
        {
            var counter = new CounterStruct(Count);
            return counter.Increment();
        }

        [Benchmark]
        public int InstantiateObjectAndCallMethod()
        {
            var counter = new Counter(Count);
            return counter.Increment();
        }

        [Benchmark]
        public int IncrementWithLock()
        {
            lock (LockObject)
            {
                return Count++;
            }
        }

        [Benchmark]
        public int ThrowAndCatchException()
        {
            try
            {
                throw new Exception("Something went wrong");
            }
            catch
            {
                Count++;
            }

            return Count;
        }

        [Benchmark]
        public int IncrementOnThreadPool() =>
            Task.Run(() => Count++).Result;

        [Benchmark]
        public int IncrementOnNewThread()
        {
            var thread = new Thread(() => Count++) { IsBackground = true };
            thread.Start();
            thread.Join();
            return Count;
        }
    }

    public interface ICounter
    {
        int IncrementViaInterfaceCall();
    }

    public abstract class BaseCounter
    {
        public abstract int IncrementViaOverriddenMethod();
    }

    public class Counter : BaseCounter, ICounter
    {
        private int _count;

        public Counter(int count) => _count = count;

        public int IncrementViaInterfaceCall() => _count++;

        public int Increment() => _count++;

        public override int IncrementViaOverriddenMethod() => _count++;
    }

    public struct CounterStruct
    {
        private int _count;

        public CounterStruct(int count) => _count = count;

        public int Increment() => _count++;
    }
}