﻿using System;

namespace DeltaWare.Dependencies.Abstractions
{
    public interface IDependencyProvider
    {
        object GetDependency(Type type);

        ILifetimeScope CreateScope();
    }
}
