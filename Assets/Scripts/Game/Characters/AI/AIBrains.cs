using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.AI
{
    public class AIBrains : MonoBehaviour
    {
        [TitleGroup("Current State")]
        [SerializeField] [HideLabel] [EnumToggleButtons] [ReadOnly] private Data.AI.States _currentState;

        [TitleGroup("AI Parameters")]
        [FoldoutGroup("AI Parameters/Main")] [SerializeField] private float _timeBetweenDecisions;
        [FoldoutGroup("AI Parameters/Main")] [SerializeField] [ProgressBar(0, 10, 0, 1, 0, Segmented = true)] private int _lowStamina;
        [FoldoutGroup("AI Parameters/Move & Attack")] [SerializeField] private float _attackDistance = 0.75f;
        [FoldoutGroup("AI Parameters/Block")] [SerializeField] [MinMaxSlider(0.5f, 5f, true)] private Vector2 _blockDuration;
        [FoldoutGroup("AI Parameters/MantainDistance")] [SerializeField] private float _safeDistanceFromPlayer = 2f;
        [FoldoutGroup("AI Parameters/MantainDistance")] [SerializeField] private float _safeDistanceFromWalls = 2f;
        [FoldoutGroup("AI Parameters/MantainDistance")] [SerializeField] [MinMaxSlider(2f, 10f, true)] private Vector2 _mantainDistanceDuration;
        [FoldoutGroup("AI Parameters/MantainDistance")] [SerializeField] private LayerMask _wallLayer;

        [TitleGroup("References")]
        [FoldoutGroup("References/Player")] [SerializeField] private Transform _player;

        #region Properties
        public float playerDirection
        {
            get => _playerDirection;
            set { _playerDirection = value; }
        }

        public bool ignoreLook
        {
            get => _ignoreLook;
            set { _ignoreLook = value; }
        }

        public bool performingAction
        {
            get => _performingAction;
            set
            {
                _performingAction = value;
                if (!value)
                    _currentState = Data.AI.States.Thinking;
            }
        }
        #endregion

        #region Private Variables
        private bool _ignoreLook;
        private bool _performingAction;
        private float _playerDirection;
        private float _distanceToPlayer;
        private float _distanceToWall;
        private Transform _visuals;
        private AIMovement _aIMovement;
        private CharacterHealth _characterHealth;
        private CharacterAttack _characterAttack;
        private CharacterStamina _characterStamina;
        private CharacterMovement _characterMovement;
        #endregion

        private void Awake()
        {
            _visuals = GetComponentInChildren<SpriteRenderer>().transform;
            _characterHealth = GetComponent<CharacterHealth>();
            _characterStamina = GetComponent<CharacterStamina>();
            _characterAttack = GetComponent<CharacterAttack>();
            _aIMovement = GetComponent<AIMovement>();
            _characterMovement = GetComponent<CharacterMovement>();
        }

        private void Start()
        {
            _currentState = Data.AI.States.Thinking;
            StartCoroutine(DecisionLoop());
        }

        private void Update()
        {
            LookToPlayer();

            if (_currentState == Data.AI.States.MovingAndAttacking && CheckAttackRange())
                AttackCheck();

            if (_currentState == Data.AI.States.MantainingDistance)
                MantainDistance();
        }

        #region Brain Core
        private IEnumerator DecisionLoop()
        {
            do
            {
                yield return new WaitForSeconds(_timeBetweenDecisions);
                if (!performingAction)
                    TakeDecision();
            } while (_characterHealth.currentHealth > 0);
        }

        private void TakeDecision()
        {
            if (CheckStamina())
            {
                float randomDecision = Random.Range(0f, 1f);

                if (randomDecision <= 40f)
                    _currentState = Data.AI.States.MovingAndAttacking;

                else if (randomDecision <= 80)
                    _currentState = Data.AI.States.Blocking;

                else
                    _currentState = Data.AI.States.MantainingDistance;
            }

            _currentState = Data.AI.States.MantainingDistance;
            PerformAction();
        }

        private void PerformAction()
        {
            switch (_currentState)
            {
                case Data.AI.States.MovingAndAttacking:
                    AttackPlayer();
                    break;
                case Data.AI.States.Blocking:
                    StartCoroutine(Block());
                    break;
                case Data.AI.States.MantainingDistance:
                    break;
            }

            performingAction = true;
        }
        #endregion

        #region Actions
        private void AttackPlayer()
        {
            _aIMovement.currentDirection = playerDirection;
        }

        private void AttackCheck()
        {
            _aIMovement.currentDirection = 0f;
            _characterAttack.Attack(Data.Attacks.Types.AttackUp);
            _currentState = Data.AI.States.Thinking;
            performingAction = false;
        }

        private IEnumerator Block()
        {
            _characterAttack.StartBlock(Data.Blocks.Types.BlockUp);
            yield return new WaitForSeconds(Random.Range(_blockDuration.x, _blockDuration.y));
            _characterAttack.EndBlock(Data.Blocks.Types.BlockUp);
            performingAction = false;
        }

        private void MantainDistance()
        {
            CheckForWalls();
            CheckForPlayer();
        }

        private void CheckForWalls()
        {
            RaycastHit2D rightWallCheck = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, _wallLayer);
            RaycastHit2D leftWallCheck = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, _wallLayer);
            _distanceToWall = Mathf.Min(rightWallCheck.distance, leftWallCheck.distance);

            _aIMovement.currentDirection = -playerDirection;

            if (_distanceToWall <= _safeDistanceFromWalls)
            {
                _aIMovement.currentDirection = 0f;

                float randomMovement = Random.Range(0f, 1f);

                if (randomMovement <= 0.5f)
                    _characterMovement.Jump();

                else
                    _characterMovement.Dash();

                performingAction = false;
            }
        }

        private void CheckForPlayer()
        {

        }
        #endregion

        #region Resources
        private bool CheckStamina()
        {
            return _characterStamina.currentStamina > (_characterStamina.maxStamina * (_lowStamina / 10f));
        }

        private bool CheckAttackRange()
        {
            return _distanceToPlayer < _attackDistance;
        }
        #endregion

        #region Utility
        private void LookToPlayer()
        {
            playerDirection = _player.position.x > transform.position.x ? 1 : -1;
            _distanceToPlayer = Vector2.Distance(transform.position, _player.position);

            if (!_ignoreLook && _characterHealth.currentHealth > 0f)
                _visuals.localScale = new Vector2(playerDirection, _visuals.localScale.y);
        }
        #endregion

        #region Editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackDistance);

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _safeDistanceFromPlayer);
        }
        #endregion
    }
}