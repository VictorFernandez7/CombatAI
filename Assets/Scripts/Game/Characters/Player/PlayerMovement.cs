using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Title("Parameters")]
        [SerializeField] private float _movementSpeed = 5f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private Vector2 _jumpDirection;

        public float horizontalDirection
        {
            get => _horizontalDirection;
            set
            {
                _horizontalDirection = value;
                _animator.SetFloat("Speed", Mathf.Abs(value));
                if (value != 0)
                {
                    _visuals.localScale = new Vector3(value, _visuals.localScale.y, _visuals.localScale.z);
                    if (Mathf.Sign(_jumpDirection.x) != value)
                        _jumpDirection = new Vector2(_jumpDirection.x * -1, _jumpDirection.y);
                }
            }
        }

        public bool grounded
        {
            get => _grounded;
            set
            {
                _grounded = value;
                _animator.SetBool("Grounded", value);
            }
        }

        private bool _canMove;
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

            if (Input.GetButtonDown("Jump") && grounded)
                Jump();
        }

        private void FixedUpdate()
        {
            if (_canMove)
                Move();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                grounded = true;
                _canMove = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                grounded = false;
                _canMove = false;
            }
        }

        #region Horizontal Movement
        private void Move()
        {
            transform.Translate(Vector3.right * horizontalDirection * _movementSpeed * Time.deltaTime);
        }

        private Vector2 GetInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        #endregion

        #region Jump
        private void Jump()
        {
            _rigidbody2D.AddForce(_jumpDirection.normalized * _jumpForce, ForceMode2D.Impulse);
        }
        #endregion

        #region Editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_jumpDirection);
        }
        #endregion
    }
}