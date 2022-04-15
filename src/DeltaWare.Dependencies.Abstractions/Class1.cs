using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DeltaWare.Dependencies.Abstractions.Descriptors;

namespace DeltaWare.Dependencies.Abstractions
{
    internal class Class1
    {
        public void Temp()
        {
            IDependencyCollection collection;

            collection.RegisterType<DependencyDescriptorBase>()
                .DefineAs<IDependencyDescriptor>()
                .AsSingleton()
                .DoNotBind();
        }
    }
}
