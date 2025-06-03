using System;
using InputManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class HudUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private TextMeshProUGUI totalText;
        [SerializeField] private Button rollButton;

        public UnityEvent OnRollClicked { get; private set; } = new();

        private void OnEnable()
        {
            rollButton.onClick.AddListener(RollClicked);
        }

        private void OnDisable()
        {
            rollButton.onClick.RemoveListener(RollClicked);
        }

        private void RollClicked()
        {
            if (!InputHandler.InputBlocked)
                OnRollClicked?.Invoke();
        }

        public void UpdateResult(string result)
        {
            resultText.text = result;
        }

        public void UpdateTotal(string total)
        {
            totalText.text = total;
        }
    }
}