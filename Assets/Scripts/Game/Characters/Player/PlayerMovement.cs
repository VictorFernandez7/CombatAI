using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [TitleGroup("Parameters")]
        [FoldoutGroup("Parameters/Movement")] [SerializeField] private float _movementSpeed = 5f;
        [FoldoutGroup("Parameters/Jump")][SerializeField] private float _jumpForce = 5f;
        [FoldoutGroup("Parameters/Jump")] [SerializeField] private Vector2 _jumpDirection;
        [FoldoutGroup("Parameters/Jump")] [SerializeField] private float _jumpLinearDrag = 1f;
        [FoldoutGroup("Parameters/Dash")] [SerializeField] private float _dashForce = 5f;
        [FoldoutGroup("Parameters/Dash")] [SerializeField] private float _dashLinearDrag = 2.5f;

        #region Properties
        public float horizontalDirection
        {
            get => _horizontalDirection;
            set
            {
                _horizontalDirection = value;
                _animator.SetFloat("Speed", Mathf.Abs(value));
                if (value != 0 && grounded && !_dashing)
                {
                    _visuals.localScale = new Vector3(Mathf.Sign(value), _visuals.localScale.y);
                    if (Mathf.Sign(_jumpDirection.x) != Mathf.Sign(value))
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
                if (!value)
                    _visuals.localScale = new Vector3(Mathf.Sign(_rigidbody2D.velocity.x), _visuals.localScale.y);
            }
        }

        public bool dashing
        {
            get => _dashing;
            set
            {
                _dashing = value;
                _animator.SetBool("Dashing", value);
            }
        } 
        #endregion

        private bool _canMove;
        private bool _dashing;
        private bool _grounded;
        private float _horizontalDirection;
        private Animator _animator;
        private Transform _visuals;
        private Rigidbody2D _rigidbody2D;
        private CharacterStamina _characterStamina;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _visuals = _animator.transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _characterStamina = GetComponent<CharacterStamina>();
        }

        private void Update()
        {
            if (_canMove)
                Move();

            if (Input.GetButtonDown("Jump") && grounded) // CHECK INPUT
            {
                if (_characterStamina.EnoughStamina(_characterStamina.jumpCost)) // CHECK STAMINA
                    Jump();
            }

            if (Input.GetButtonDown("Dash") && _grounded && !_dashing) // CHECK INPUT
            {
                if (_characterStamina.EnoughStamina(_characterStamina.dashCost)) // CHECK STAMINA
                    StartCoroutine(Dash());
            }
        }

        private void FixedUpdate()
        {
            horizontalDirection = GetInput().x;
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

        #region Dash
        private IEnumerator Dash()
        {
            dashing = true;
            _canMove = false;
            _rigidbody2D.drag = _dashLinearDrag;
            _rigidbody2D.AddForce(Vector2.right * Mathf.Sign(_visuals.localScale.x) * _dashForce, ForceMode2D.Impulse);
            _characterStamina.UseStamina(_characterStamina.dashCost);

            yield return new WaitForSeconds(0.75f);

            dashing = false;
            _canMove = true;
        }
        #endregion

        #region Jump
        private void Jump()
        {
            _rigidbody2D.drag = _jumpLinearDrag;
            _rigidbody2D.AddForce(_jumpDirection.normalized * _jumpForce, ForceMode2D.Impulse);
            _characterStamina.UseStamina(_characterStamina.jumpCost);
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