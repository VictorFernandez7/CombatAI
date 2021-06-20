using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.AI
{
    public class AIBrains : MonoBehaviour
    {
        [Title("Current State")]
        [SerializeField] [HideLabel] private CombatAI.Data.AI.States _currentState;

        [TitleGroup("AI Parameters")]
        [FoldoutGroup("AI Parameters/Main")] [SerializeField] private float _timeBetweenDecisions;
        [FoldoutGroup("AI Parameters/Move & Attack")] [SerializeField] private float _attackDistance;

        [TitleGroup("References")]
        [FoldoutGroup("References/Player")] [SerializeField] private Transform _player;

        public float playerDirection => _playerDirection;

        public bool ignoreLook
        {
            get => _ignoreLook;
            set { _ignoreLook = value; }
        }

        private bool _ignoreLook;
        private float _playerDirection;
        private Transform _visuals;
        private CharacterHealth _characterHealth;
        private CharacterStamina _characterStamina;

        private void Awake()
        {
            _visuals = GetComponentInChildren<SpriteRenderer>().transform;
            _characterHealth = GetComponent<CharacterHealth>();
            _characterStamina = GetComponent<CharacterStamina>();
        }

        private void Start()
        {
            StartCoroutine(DecisionLoop());
        }

        private void Update()
        {
            LookToPlayer();
        }

        #region Brain Core
        private IEnumerator DecisionLoop()
        {
            do
            {
                TakeDecision();
                yield return new WaitForSeconds(_timeBetweenDecisions);
            } while (_characterHealth.currentHealth > 0);
        }

        private void TakeDecision()
        {
            int randomDecision = Random.Range(0, 3);

            if (randomDecision == 0)
                _currentState = Data.AI.States.MovingAndAttacking;

            else if (randomDecision == 1)
                _currentState = Data.AI.States.Blocking;

            else if (randomDecision == 2)
                _currentState = Data.AI.States.MantainingDistance;

            PerformAction();
        }

        private void PerformAction()
        {
            switch (_currentState)
            {
                case Data.AI.States.MovingAndAttacking:
                    break;
                case Data.AI.States.Blocking:
                    break;
                case Data.AI.States.MantainingDistance:
                    break;
            }
        } 
        #endregion

        #region Utility
        private void LookToPlayer()
        {
            _playerDirection = _player.position.x > transform.position.x ? 1 : -1;

            if (!_ignoreLook)
                _visuals.localScale = new Vector2(_playerDirection, _visuals.localScale.y);
        } 
        #endregion

        #region Editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackDistance);
        }
        #endregion
    }
}