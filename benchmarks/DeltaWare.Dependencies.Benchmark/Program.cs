using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Benchmark.Mocking;
using System;
using System.Diagnostics;
using BetterConsoleTables;
using DeltaWare.SDK.Benchmarking.Results;
using DeltaWare.SDK.Core.Helpers;

namespace DeltaWare.Dependencies.Benchmark
{
    internal class Program
    {
        private static decimal ToMilliseconds(long ticks)
        {
            return Math.Round((decimal)ticks / 10000, 2);
        }        
        
        private static decimal ToMilliseconds(decimal ticks)
        {
            return Math.Round(ticks / 10000, 2);
        }

        private static void PrintResult(IBenchmarkResult result)
        {
            Table table = new Table("Name", "Total Ticks", "Average", "Min", "Max");

            table.AddRow(
                result.Name,
                TickHelper.ToHumanReadableTime(result.TotalTicks),
                TickHelper.ToHumanReadableTime(Convert.ToInt64(result.AverageTicks)),
                TickHelper.ToHumanReadableTime(result.MinimumTicks),
                TickHelper.ToHumanReadableTime(result.MaximumTicks));

            foreach (IBenchmarkResult metric in result.Results)
            {
                table.AddRow(
                    metric.Name,
                    TickHelper.ToHumanReadableTime(metric.TotalTicks),
                    TickHelper.ToHumanReadableTime(Convert.ToInt64(metric.AverageTicks)),
                    TickHelper.ToHumanReadableTime(metric.MinimumTicks),
                    TickHelper.ToHumanReadableTime(metric.MaximumTicks));
            }

            table.Config = TableConfiguration.UnicodeAlt();

            Console.WriteLine(table.ToString());            
            
            table = new Table("Name", "Total Ticks", "Average", "Min", "Max");

            table.AddRow(
                result.Name,
                result.TotalTicks,
                result.AverageTicks,
                result.MinimumTicks,
                result.MaximumTicks);

            foreach (IBenchmarkResult metric in result.Results)
            {
                table.AddRow(
                    metric.Name,
                    metric.TotalTicks,
                    metric.AverageTicks,
                    metric.MinimumTicks,
                    metric.MaximumTicks);
            }

            table.Config = TableConfiguration.UnicodeAlt();

            Console.WriteLine(table.ToString());
            Console.Read();
        }

        public static void DependencyBenchmark(int iterations)
        {
            SDK.Benchmarking.Benchmark benchmark = new SDK.Benchmarking.Benchmark(metrics =>
            {
                IDependencyCollection dependencies = new DependencyCollection();
                ILifetimeScope scope = null;
                IDependencyProvider provider = null;

                metrics
                    .AddMetric("Registration")
                    .Measure(() =>
                    {
                        dependencies.Register<AddressService>().DefineAs<IAddressService>().AsScoped();
                        dependencies.Register<EmailService>().DefineAs<IEmailService>().AsScoped();
                        dependencies.Register<MessagingService>().DefineAs<IMessagingService>().AsScoped();
                        dependencies.Register<PostalService>().DefineAs<IPostalService>().AsScoped();
                    });

                metrics
                    .AddMetric("Create Scope")
                    .Measure(() =>
                    {
                        scope = dependencies.CreateScope();
                        provider = scope.BuildProvider();
                    });
                
                metrics
                    .AddMetric("1st Get Dependency")
                    .Measure(() =>
                    {
                        provider.GetDependency<IMessagingService>();
                    });
                
                metrics
                    .AddMetric("2nd Get Dependency")
                    .Measure(() =>
                    {
                        provider.GetDependency<IMessagingService>();
                    });

                metrics
                    .AddMetric("Dispose Scope")
                    .Measure(() =>
                    {
                        scope.Dispose();
                    });
            });

            IBenchmarkResult results = benchmark.Measure(iterations);

            PrintResult(results);











            //decimal registrationTime = 0;
            //decimal buildTime = 0;
            //decimal disposeTime = 0;

            //for (int i = 0; i < iterations; i++)
            //{
            //    IDependencyCollection dependencies = new DependencyCollection();

            //    Stopwatch stopwatch = new Stopwatch();

            //    stopwatch.Start();

            //    dependencies.Register<AddressService>().DefineAs<IAddressService>().AsScoped();
            //    dependencies.Register<EmailService>().DefineAs<IEmailService>().AsScoped();
            //    dependencies.Register<MessagingService>().DefineAs<IMessagingService>().AsScoped();
            //    dependencies.Register<PostalService>().DefineAs<IPostalService>().AsScoped();

            //    stopwatch.Stop();

            //    registrationTime += stopwatch.ElapsedTicks;

            //    ILifetimeScope scope = dependencies.CreateScope();
            //    IDependencyProvider provider = scope.BuildProvider();

            //    stopwatch.Restart();

            //    provider.GetDependency<IMessagingService>();

            //    stopwatch.Stop();

            //    buildTime += stopwatch.ElapsedTicks;

            //    stopwatch.Restart();

            //    provider.Dispose();

            //    stopwatch.Stop();

            //    disposeTime += stopwatch.ElapsedTicks;

            //    scope.Dispose();
            //}

            //decimal totalTime = registrationTime + buildTime + disposeTime;

            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine("--------------------------Benchmark Completed---------------------------");
            //Console.WriteLine($"Total Time: [{Math.Round(totalTime / 10000)}ms] | [{Math.Round(totalTime)} ticks]");
            //Console.WriteLine($"Total Registration Time: [{Math.Round(registrationTime / 10000)}ms] | [{Math.Round(registrationTime)} ticks]");
            //Console.WriteLine($"Total Build Time: [{Math.Round(buildTime / 10000)}ms] | [{Math.Round(buildTime)} ticks]");
            //Console.WriteLine($"Total Dispose Time: [{Math.Round(disposeTime / 10000)}ms] | [{Math.Round(disposeTime)} ticks]");
            //Console.WriteLine("------------------------------------------------------------------------");

            //totalTime /= iterations;
            //registrationTime /= iterations;
            //buildTime /= iterations;
            //disposeTime /= iterations;

            //Console.WriteLine($"Average Time: [{Math.Round(totalTime / 10000)}ms] | [{Math.Round(totalTime)} ticks]");
            //Console.WriteLine($"Average Registration Time: [{Math.Round(registrationTime / 10000)}ms] | [{Math.Round(registrationTime)} ticks]");
            //Console.WriteLine($"Average Build Time: [{Math.Round(buildTime / 10000)}ms] | [{Math.Round(buildTime)} ticks]");
            //Console.WriteLine($"Average Dispose Time: [{Math.Round(disposeTime / 10000)}ms] | [{Math.Round(disposeTime)} ticks]");
            //Console.WriteLine("------------------------------------------------------------------------");

            //Console.ReadKey();
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