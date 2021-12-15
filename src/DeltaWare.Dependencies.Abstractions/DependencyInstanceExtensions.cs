namespace DeltaWare.Dependencies.Abstractions
{
    public static class DependencyInstanceExtensions
    {
        public static TInstance Instance<TInstance>(this IDependencyInstance instance) where TInstance : class
        {
            return (TInstance)instance.Instance;
        }
    }
}