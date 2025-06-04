using UnityEngine.Events;

namespace DieSimulation.Interfaces
{
    public interface IDieProvider
    {
        public IThrowable Throwable { get; }
        public UnityEvent<int?> OnRolled { get; }
    }
}