using System;
using UnityEngine;

namespace DieSimulation.Data
{
    [Serializable]
    public sealed class DieFaceValue
    {
        [field: SerializeField] 
        public int Value { get; private set; }
        
        [field: SerializeField] 
        public int FontSize { get; private set; } = 5;
    }
}