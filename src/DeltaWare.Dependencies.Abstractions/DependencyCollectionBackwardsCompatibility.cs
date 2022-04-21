using DeltaWare.Dependencies.Abstractions.Descriptor.Enums;
using DeltaWare.Dependencies.Abstractions.Registration;
using System;

namespace DeltaWare.Dependencies.Abstractions
{
    [Obsolete("This has been replace with the Register FluentApi")]
    public static class DependencyCollectionBackwardsCompatibility
    {
        #region Transient

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddTransient<TImplementation, TDefinition>(this IDependencyCollection collection, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register<TImplementation>()
                    .DefineAs<TDefinition>()
                    .AsTransient()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddTransient<TImplementation, TDefinition>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register(builder)
                     .DefineAs<TDefinition>()
                     .AsTransient()
                     .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddTransient<TImplementation, TDefinition>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register(builder)
                    .DefineAs<TDefinition>()
                    .AsTransient()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddTransient<TImplementation>(this IDependencyCollection collection, Binding binding = Binding.Bound)
        {
            collection.Register<TImplementation>()
                    .AsTransient()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddTransient<TImplementation>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound)
        {
            collection.Register(builder)
                    .AsTransient()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddTransient<TImplementation>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound)
        {
            collection.Register(builder)
                    .AsTransient()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddTransient<TImplementation, TDefinition>(this IDependencyCollection collection, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister<TImplementation>()
                        .DefineAs<TDefinition>()
                        .AsTransient()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddTransient<TImplementation, TDefinition>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister(builder)
                        .DefineAs<TDefinition>()
                        .AsTransient()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddTransient<TImplementation, TDefinition>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister(builder)
                        .DefineAs<TDefinition>()
                        .AsTransient()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddTransient<TImplementation>(this IDependencyCollection collection, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister<TImplementation>()
                        .AsTransient()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddTransient<TImplementation>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister(builder)
                        .AsTransient()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddTransient<TImplementation>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister(builder)
                        .AsTransient()
                        .SetBinding(binding);
            });
        }

        #endregion

        #region MyRegion

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddScoped<TImplementation, TDefinition>(this IDependencyCollection collection, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register<TImplementation>()
                    .DefineAs<TDefinition>()
                    .AsScoped()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddScoped<TImplementation, TDefinition>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register(builder)
                    .DefineAs<TDefinition>()
                    .AsScoped()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddScoped<TImplementation, TDefinition>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register(builder)
                    .DefineAs<TDefinition>()
                    .AsScoped()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddScoped<TImplementation>(this IDependencyCollection collection, Binding binding = Binding.Bound)
        {
            collection.Register<TImplementation>()
                    .AsScoped()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddScoped<TImplementation>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound)
        {
            collection.Register(builder)
                    .AsScoped()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddScoped<TImplementation>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound)
        {
            collection.Register(builder)
                    .AsScoped()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddScoped<TImplementation, TDefinition>(this IDependencyCollection collection, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister<TImplementation>()
                        .DefineAs<TDefinition>()
                        .AsScoped()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddScoped<TImplementation, TDefinition>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister(builder)
                        .DefineAs<TDefinition>()
                        .AsScoped()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddScoped<TImplementation, TDefinition>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister(builder)
                        .DefineAs<TDefinition>()
                        .AsScoped()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddScoped<TImplementation>(this IDependencyCollection collection, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister<TImplementation>()
                        .AsScoped()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddScoped<TImplementation>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister(builder)
                        .AsScoped()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddScoped<TImplementation>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister(builder)
                        .AsScoped()
                        .SetBinding(binding);
            });
        }

        #endregion

        #region Singleton

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddSingleton<TImplementation, TDefinition>(this IDependencyCollection collection, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register<TImplementation>()
                    .DefineAs<TDefinition>()
                    .AsSingleton()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddSingleton<TImplementation, TDefinition>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register(builder)
                    .DefineAs<TDefinition>()
                    .AsSingleton()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddSingleton<TImplementation, TDefinition>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            collection.Register(builder)
                    .DefineAs<TDefinition>()
                    .AsSingleton()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddSingleton<TImplementation>(this IDependencyCollection collection, Binding binding = Binding.Bound)
        {
            collection.Register<TImplementation>()
                    .AsSingleton()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddSingleton<TImplementation>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound)
        {
            collection.Register(builder)
                    .AsSingleton()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static void AddSingleton<TImplementation>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound)
        {
            collection.Register(builder)
                    .AsSingleton()
                    .SetBinding(binding);
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddSingleton<TImplementation, TDefinition>(this IDependencyCollection collection, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister<TImplementation>()
                        .DefineAs<TDefinition>()
                        .AsSingleton()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddSingleton<TImplementation, TDefinition>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister(builder)
                        .DefineAs<TDefinition>()
                        .AsSingleton()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddSingleton<TImplementation, TDefinition>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound) where TDefinition : TImplementation
        {
            return collection.CheckIfAdded<TDefinition>(c =>
            {
                c.TryRegister(builder)
                        .DefineAs<TDefinition>()
                        .AsSingleton()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddSingleton<TImplementation>(this IDependencyCollection collection, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister<TImplementation>()
                        .AsSingleton()
                        .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddSingleton<TImplementation>(this IDependencyCollection collection, Func<TImplementation> builder, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister(builder)
                    .AsSingleton()
                    .SetBinding(binding);
            });
        }

        [Obsolete("This has been replace with the Register FluentApi")]
        public static bool TryAddSingleton<TImplementation>(this IDependencyCollection collection, Func<IDependencyProvider, TImplementation> builder, Binding binding = Binding.Bound)
        {
            return collection.CheckIfAdded<TImplementation>(c =>
            {
                c.TryRegister(builder)
                    .AsSingleton()
                    .SetBinding(binding);
            });
        }

        #endregion

        private static bool CheckIfAdded<T>(this IDependencyCollection collection, Action<IDependencyCollection> register)
        {
            bool existing = collection.HasDependency<T>();

            register.Invoke(collection);

            if (existing)
            {
                return false;
            }

            return collection.HasDependency<T>();
        }

        private static IRegistrationInitialization<T> SetBinding<T>(this IRegistrationBinding<T> registrationBinding, Binding binding)
        {
            if (binding == Binding.Unbound)
            {
                return registrationBinding.DoNotBind();
            }

            return registrationBinding;
        }
    }
}
