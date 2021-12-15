namespace DeltaWare.Dependencies.Tests.Types
{
    public class CircularDependencyA
    {
        private readonly CircularDependencyB _dependency;

        public CircularDependencyA(CircularDependencyB dependency)
        {
            _dependency = dependency;
        }
    }
}