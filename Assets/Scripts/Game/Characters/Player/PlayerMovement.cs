using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Title("Horizontal Movement Parameters")]
        [SerializeField] private float _movementAcceleration = 50f;
        [SerializeField] private float _maxMoveSpeed = 5f;
        [SerializeField] private float _groundLinearDrag = 20f;

        [Title("Jump Parameters")]
        [SerializeField] private float _jumpForce = 10f;
        [SerializeField] private float _fallMultiplier = 8f;
        [SerializeField] private float _lowJumpFallMultiplier = 5f;
        [SerializeField] private float _airLinearDrag = 2.5f;
        [SerializeField] private float _groundRaycastLength = 0.75f;
        [SerializeField] private LayerMask _groundLayer;

        public bool changingDirection => (_rigidbody2D.velocity.x > 0f && horizontalDirection < 0f) || (_rigidbody2D.velocity.x < 0f && horizontalDirection > 0f);

        public float horizontalDirection
        {
            get => _horizontalDirection;
            set
            {
                _horizontalDirection = value;
                _animator.SetFloat("Speed", Mathf.Abs(value));
                if (value != 0)
                    _visuals.localScale = new Vector3(value, _visuals.localScale.y, _visuals.localScale.z);
            }
        }

        private bool _grounded;
        private float _horizontalDirection;
        private Animator _animator;
        private Transform _visuals;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _visuals = _animator.transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            horizontalDirection = GetInput().x;

            if (Input.GetButtonDown("Jump") && _grounded)
                Jump();
        }

        private void FixedUpdate()
        {
            Move();
            CheckForGround();

            if (_grounded)
                ApplyGroundLinearDrag();

            else
            {
                ApplyAirLinearDrag();
                FallMultiplier();
            }
        }

        #region Horizontal Movement
        private void Move()
        {
            _rigidbody2D.AddForce(new Vector2(horizontalDirection, 0f) * _movementAcceleration);

            if (Mathf.Abs(_rigidbody2D.velocity.x) > _maxMoveSpeed)
                _rigidbody2D.velocity = new Vector2(Mathf.Sign(_rigidbody2D.velocity.x) * _maxMoveSpeed, _rigidbody2D.velocity.y);
        }

        private void ApplyGroundLinearDrag()
        {
            if (Mathf.Abs(horizontalDirection) < 0.4f || changingDirection)
                _rigidbody2D.drag = _groundLinearDrag;

            else
                _rigidbody2D.drag = 0f;
        }

        private Vector2 GetInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        #endregion

        #region Jump
        private void Jump()
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        private void ApplyAirLinearDrag()
        {
            _rigidbody2D.drag = _airLinearDrag;
        }

        private void FallMultiplier()
        {
            if (_rigidbody2D.velocity.y < 0f)
                _rigidbody2D.gravityScale = _fallMultiplier;

            else if (_rigidbody2D.velocity.y > 0f && !Input.GetButton("Jump"))
                _rigidbody2D.gravityScale = _lowJumpFallMultiplier;

            else
                _rigidbody2D.gravityScale = 1f;
        }

        private void CheckForGround()
        {
            _grounded = Physics2D.Raycast(transform.position * _groundRaycastLength, Vector2.down, _groundRaycastLength, _groundLayer);
        }
        #endregion

        #region Editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundRaycastLength);
        }
        #endregion
    }
}