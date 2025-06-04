using UnityEngine.Events;

namespace DieSimulation.Interfaces
{
    public interface IResolvable
    {
        public UnityEvent<int?> OnResolved { get; }
        public void StartResolving();
    }
}