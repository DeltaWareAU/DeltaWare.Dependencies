namespace DeltaWare.Dependencies.Tests.Types
{
    public class CircularDependencyB
    {
        private readonly CircularDependencyA _dependency;

        public CircularDependencyB(CircularDependencyA dependency)
        {
            _dependency = dependency;
        }
    }
}