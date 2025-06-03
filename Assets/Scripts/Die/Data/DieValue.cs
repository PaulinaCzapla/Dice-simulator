using UnityEngine;

namespace Die.Data
{
    [CreateAssetMenu(fileName = "DiceValue", menuName = "ScriptableObjects/DiceValue", order = 0)]
    public class DieValue : ScriptableObject
    {
        [SerializeField]
        private int value;

        [Header("Presentation - this value will appear on dice")] 
        [SerializeField]
        private string displayName;

        [SerializeField] private int fontSize;

        public int Value => value;

        public string DisplayName => displayName;

        public int FontSize => fontSize;
    }
}