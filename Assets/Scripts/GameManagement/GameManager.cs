using System.Collections.Generic;
using DieSimulation.Interfaces;
using GameManagement.Startup;
using InputManagement;
using UI;
using UnityEngine;

namespace GameManagement
{
    public sealed class GameManager : MonoBehaviour
    {
        private const string UNDEFINED_ERROR = "-";
        private const string DURING_ROLL = "?";

        [SerializeField] private List<Vector3> possibleRollVelocities;
        [SerializeField] private List<Vector3> possibleRollAngularVelocities;
        [SerializeField] private Generator generator;
        [SerializeField] private HudUI hud;
        
        private IDieProvider[] _dieProviders;
        private bool _anyFailed;
        private int _diceThrewCount;
        private int _finishedDiceCounter;
        private int _resultsTotal;
        private int _resultSum;

        private void Awake()
        {
            var map = generator.BuildMap();
            _dieProviders = map.Dice;

            hud.SetResultText(string.Empty);
            hud.SetTotalText(string.Empty);

            Subscribe();
        }

        private void OnDestroy()
        {
            hud.OnRollClicked.RemoveListener(Roll);
            foreach (var dieProvider in _dieProviders)
            {
                dieProvider.Throwable.OnThrew.RemoveListener(DieThrew);
                dieProvider.OnRolled.RemoveListener(DieRolled);
            }
        }

        private void Subscribe()
        {
            hud.OnRollClicked.AddListener(Roll);
            foreach (var dieProvider in _dieProviders)
            {
                dieProvider.Throwable.OnThrew.AddListener(DieThrew);
                dieProvider.OnRolled.AddListener(DieRolled);
            }
        }

        private void Roll()
        {
            foreach (var dieProvider in _dieProviders)
            {
                dieProvider.Throwable.ThrowSingle(
                    possibleRollVelocities[Random.Range(0, possibleRollVelocities.Count)],
                    possibleRollAngularVelocities[Random.Range(0, possibleRollAngularVelocities.Count)]
                );
            }
        }

        private void DieThrew()
        {
            InputHandler.InputBlocked = true;

            _diceThrewCount++;
            hud.SetResultText(DURING_ROLL);
        }

        private void DieRolled(int? result)
        {
            _finishedDiceCounter++;

            if (result.HasValue)
            {
                DieRollSuccess(result.Value);
            }
            else
            {
                _anyFailed = true;
                if (_finishedDiceCounter.Equals(_diceThrewCount))
                {
                    SetFailedResult();
                }
            }
        }

        private void DieRollSuccess(int faceValue)
        {
            _resultSum += faceValue;
            if (!_finishedDiceCounter.Equals(_diceThrewCount))
            {
                return;
            }

            if (_anyFailed)
            {
                SetFailedResult();
            }
            else
            {
                SetSuccessResult(_resultSum);
            }
        }

        private void SetFailedResult()
        {
            hud.SetResultText(UNDEFINED_ERROR);
            PrepareForNextRoll();
            ResetThrowables();
        }

        private void SetSuccessResult(int resultSum)
        {
            _resultsTotal += resultSum;

            hud.SetTotalText(_resultsTotal.ToString());
            hud.SetResultText(resultSum.ToString());

            PrepareForNextRoll();
        }

        private void PrepareForNextRoll()
        {
            InputHandler.InputBlocked = false;

            _diceThrewCount = 0;
            _resultSum = 0;
            _finishedDiceCounter = 0;
            _anyFailed = false;
        }

        private void ResetThrowables()
        {
            foreach (var die in _dieProviders)
            {
                die.Throwable.Reset();
            }
        }
    }
}