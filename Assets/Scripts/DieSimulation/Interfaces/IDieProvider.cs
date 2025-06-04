namespace DieSimulation.Interfaces
{
    public interface IDieProvider
    {
        public IThrowable Throwable { get; }
        public IResolvable Resolvable { get; }
    }
}