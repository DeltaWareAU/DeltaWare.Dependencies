using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeltaWare.Dependencies.Abstractions.Configuration
{
    public interface IConfiguration
    {
        Action<IDependencyProvider, object> Configurator { get; }
    }
}
