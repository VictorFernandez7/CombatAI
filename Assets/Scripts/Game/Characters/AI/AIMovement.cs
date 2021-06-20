using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.AI
{
    public class AIMovement : CharacterMovement
    {
        [Title("Current State")]
        [SerializeField] [HideLabel] private CombatAI.Data.AI.States _currentState;

        [TitleGroup("AI Parameters")]
        [FoldoutGroup("AI Parameters/Main")] [SerializeField] private float _timeBetweenDecisions;
        [FoldoutGroup("AI Parameters/Move & Attack")] [SerializeField] private float _attackDistance;

        [TitleGroup("References")]
        [FoldoutGroup("References/Player")] [SerializeField] private Transform _player;
        [FoldoutGroup("References/Detection")] [SerializeField] private CircleCollider2D _circleCollider2D;

        private bool _ignoreLook;
        private float _playerDirection;

        private void Start()
        {
            StartCoroutine(DecisionLoop());
        }

        private void Update()
        {
            LookToPlayer();
        }

        private IEnumerator DecisionLoop()
        {
            do
            {
                TakeDecision();
                yield return new WaitForSeconds(_timeBetweenDecisions);
            } while (characterHealth.currentHealth > 0);
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

        private void LookToPlayer()
        {
            _playerDirection = _player.position.x > transform.position.x ? 1 : -1;

            if (!_ignoreLook)
                visuals.localScale = new Vector2(_playerDirection, visuals.localScale.y);
        }

        #region Editor
        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackDistance);
        }
        #endregion
    }
}