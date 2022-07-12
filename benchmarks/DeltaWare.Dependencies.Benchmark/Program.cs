using BetterConsoleTables;
using DeltaWare.Dependencies.Abstractions;
using DeltaWare.Dependencies.Benchmark.Mocking;
using DeltaWare.SDK.Benchmarking.Results;
using DeltaWare.SDK.Core.Helpers;
using System;

namespace DeltaWare.Dependencies.Benchmark
{
    internal class Program
    {
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

            IBenchmarkResult results = benchmark.Run(iterations);

            PrintResult(results);
        }

        private static void Main(string[] args)
        {
            Console.Write("Runs: ");

            int runs = int.Parse(Console.ReadLine() ?? "1");

            DependencyBenchmark(runs);
        }
    }
}