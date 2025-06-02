using UnityEngine;

namespace Dice.Data
{
    [CreateAssetMenu(fileName = "DiceValue", menuName = "ScriptableObjects/DiceValue", order = 0)]
    public class DiceValue : ScriptableObject
    {
        [Tooltip("This name will be displayed to user after getting a dice throw result.")] 
        [SerializeField]
        private string displayName;

        [Header("Presentation - this value will appear on dice")] 
        [SerializeField]
        private string text;

        [SerializeField] private int fontSize;

        public string DisplayName => displayName;

        public string Text => text;

        public int FontSize => fontSize;
    }
}