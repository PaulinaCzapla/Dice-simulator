using DieSimulation.Interfaces;
using Interactions;
using UnityEngine;

namespace DieSimulation.Components
{
    [RequireComponent(typeof(DieValuesHandler))]
    [RequireComponent(typeof(IDraggable))]
    [RequireComponent(typeof(IResolvable))]
    [RequireComponent(typeof(Rigidbody))]
    public sealed class DieController : MonoBehaviour, IDieProvider
    {
        private IDraggable _dieDraggable;
        private IThrowable _throwable;
        private IResolvable _resolvable;

        public IThrowable Throwable => _throwable;
        public IResolvable Resolvable => _resolvable;
        
        private void Awake()
        {
            _dieDraggable = GetComponent<IDraggable>();
            _resolvable = GetComponent<IResolvable>();
            _throwable = new DieThrowable(GetComponent<Rigidbody>(), _dieDraggable.GetConfig());

            _dieDraggable.OnDragEnded.AddListener(_throwable.ThrowAdditive);
            _throwable.OnThrew.AddListener(_resolvable.StartResolving);
        }

        private void OnDestroy()
        {
            _dieDraggable.OnDragEnded.RemoveListener(_throwable.ThrowAdditive);
            _throwable.OnThrew.RemoveListener(_resolvable.StartResolving);
        }
    }
}