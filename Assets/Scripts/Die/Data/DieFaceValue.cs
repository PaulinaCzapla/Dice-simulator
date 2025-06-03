using System;
using UnityEngine;

namespace Die.Data
{
    [Serializable]
    public sealed class DieFaceValue
    {
        [field: SerializeField] public int Value { get; private set; }
        [field: SerializeField] public int FontSize { get; private set; } = 5;
    }
}