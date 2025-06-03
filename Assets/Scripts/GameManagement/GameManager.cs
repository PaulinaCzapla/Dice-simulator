using System.Collections.Generic;
using System.Linq;
using Die;
using GameManagement.Startup;
using InputManagement;
using UI;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameManagement
{
    public sealed class GameManager : MonoBehaviour
    {
        private const string UNDEFINED_ERROR = "Undefined";

        [SerializeField] private List<Vector3> possibleRollVelocities;
        [SerializeField] private List<Vector3> possibleRollAngularVelocities;
        [SerializeField] private Bootstrap bootstrap;
        [SerializeField] private HudUI hud;

        private List<DieController> _diceOnScene;

        private int _resultsTotal;
        private int _finishedDiceCounter;
        private bool _anyFailed;
        private int _diceThrewCount;
        private int _resultSum;
        
        private void Awake()
        {
            bootstrap.BuildMap();
            hud.UpdateResult(string.Empty);
            hud.UpdateTotal(string.Empty);
        }
        
        private void OnEnable()
        {
            _diceOnScene = FindObjectsOfType<DieController>().ToList();
            hud.OnRollClicked.AddListener(Roll);
            foreach (var die in _diceOnScene)
            {
                die.OnThrew.AddListener(DieThrew);
                die.OnDieRollFailed.AddListener(DieRollFailed);
                die.OnDieRollSuccess.AddListener(DieRollSuccess);
            }
        }

        private void OnDisable()
        {
            hud.OnRollClicked.AddListener(Roll);
            foreach (var die in _diceOnScene)
            {
                die.OnThrew.RemoveListener(DieThrew);
                die.OnDieRollFailed.RemoveListener(DieRollFailed);
                die.OnDieRollSuccess.RemoveListener(DieRollSuccess);
            }
        }

        private void Roll()
        {
            foreach (var die in _diceOnScene)
            {
                die.Throwable.Throw(possibleRollVelocities[Random.Range(0, possibleRollVelocities.Count)],
                    possibleRollAngularVelocities[Random.Range(0, possibleRollAngularVelocities.Count)]);
            }
        }
        
        private void DieThrew(DieController dieController)
        {
            InputHandler.InputBlocked = true;

            _diceThrewCount++;
            hud.UpdateResult("?");
        }

        private void DieRollFailed(DieController dieController)
        {
            _finishedDiceCounter++;
            _anyFailed = true;
            if (_finishedDiceCounter.Equals(_diceThrewCount))
            {
                FailedResult();
            }
        }
        private void DieRollSuccess(int faceValue, DieController die)
        {
            _finishedDiceCounter++;
            _resultSum += faceValue;
            if (!_finishedDiceCounter.Equals(_diceThrewCount))
            {
                return;
            }
            
            if (_anyFailed)
            {
                FailedResult();
            }
            else
            {
                SuccessResult(_resultSum);
            }
        }

        private void FailedResult()
        {
            hud.UpdateResult(UNDEFINED_ERROR);
            PrepareForNextRoll();
        }

        private void SuccessResult(int resultSum)
        {
            _resultsTotal += resultSum;

            hud.UpdateTotal(_resultsTotal.ToString());
            hud.UpdateResult(resultSum.ToString());

            PrepareForNextRoll();
        }

        private void PrepareForNextRoll()
        {
            InputHandler.InputBlocked = false;
            
            _diceThrewCount = 0;
            _resultSum = 0;
            _finishedDiceCounter = 0;
            _anyFailed = false;
            
            foreach (var die in _diceOnScene)
            {
                die.Throwable.PrepareForThrow();
            }
        }
    }
}