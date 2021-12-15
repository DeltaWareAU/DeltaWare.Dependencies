using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Benchmark.Mocking;
using System;
using System.Diagnostics;

namespace DeltaWare.Dependencies.Benchmark
{
    internal class Program
    {
        public static void DependencyBenchmark(int iterations)
        {
            decimal registrationTime = 0;
            decimal buildTime = 0;
            decimal disposeTime = 0;

            for (int i = 0; i < iterations; i++)
            {
                IDependencyCollection dependencies = new DependencyCollection();

                Stopwatch stopwatch = new Stopwatch();

                stopwatch.Start();

                dependencies.AddScoped<IAddressService, AddressService>();
                dependencies.AddScoped<IEmailService, EmailService>();
                dependencies.AddScoped<IMessagingService, MessagingService>();
                dependencies.AddScoped<IPostalService, PostalService>();

                stopwatch.Stop();

                registrationTime += stopwatch.ElapsedTicks;

                IDependencyScope scope = dependencies.CreateScope();
                IDependencyProvider provider = scope.BuildProvider();

                stopwatch.Restart();

                provider.GetDependency<IMessagingService>();

                stopwatch.Stop();

                buildTime += stopwatch.ElapsedTicks;

                stopwatch.Restart();

                provider.Dispose();

                stopwatch.Stop();

                disposeTime += stopwatch.ElapsedTicks;

                scope.Dispose();
            }

            decimal totalTime = registrationTime + buildTime + disposeTime;

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("--------------------------Benchmark Completed---------------------------");
            Console.WriteLine($"Total Time: [{Math.Round(totalTime / 10000)}ms] | [{Math.Round(totalTime)} ticks]");
            Console.WriteLine($"Total Registration Time: [{Math.Round(registrationTime / 10000)}ms] | [{Math.Round(registrationTime)} ticks]");
            Console.WriteLine($"Total Build Time: [{Math.Round(buildTime / 10000)}ms] | [{Math.Round(buildTime)} ticks]");
            Console.WriteLine($"Total Dispose Time: [{Math.Round(disposeTime / 10000)}ms] | [{Math.Round(disposeTime)} ticks]");
            Console.WriteLine("------------------------------------------------------------------------");

            totalTime /= iterations;
            registrationTime /= iterations;
            buildTime /= iterations;
            disposeTime /= iterations;

            Console.WriteLine($"Average Time: [{Math.Round(totalTime / 10000)}ms] | [{Math.Round(totalTime)} ticks]");
            Console.WriteLine($"Average Registration Time: [{Math.Round(registrationTime / 10000)}ms] | [{Math.Round(registrationTime)} ticks]");
            Console.WriteLine($"Average Build Time: [{Math.Round(buildTime / 10000)}ms] | [{Math.Round(buildTime)} ticks]");
            Console.WriteLine($"Average Dispose Time: [{Math.Round(disposeTime / 10000)}ms] | [{Math.Round(disposeTime)} ticks]");
            Console.WriteLine("------------------------------------------------------------------------");

            Console.ReadKey();
        }

        public void DependencyActionBenchmark()
        {
        }

        private static void Main(string[] args)
        {
            Console.Write("Runs: ");

            int runs = int.Parse(Console.ReadLine() ?? "1");

            DependencyBenchmark(runs);
        }
    }
}