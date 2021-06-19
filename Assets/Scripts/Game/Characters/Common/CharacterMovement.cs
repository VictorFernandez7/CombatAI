using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters
{
    public class CharacterMovement : MonoBehaviour
    {
        [TitleGroup("Parameters")]
        [FoldoutGroup("Parameters/Movement")] [SerializeField] private float _movementSpeed = 5f;
        [FoldoutGroup("Parameters/Jump")] [SerializeField] private float _jumpForce = 12f;
        [FoldoutGroup("Parameters/Jump")] [SerializeField] private Vector2 _jumpDirection = new Vector2(1.5f, 2f);
        [FoldoutGroup("Parameters/Jump")] [SerializeField] private float _jumpLinearDrag = 1f;
        [FoldoutGroup("Parameters/Dash")] [SerializeField] private float _dashForce = 12f;
        [FoldoutGroup("Parameters/Dash")] [SerializeField] private float _dashLinearDrag = 2.5f;

        #region Properties
        public CharacterStamina characterStamina => _characterStamina;

        public bool canMove
        {
            get => _canMove;
            set { _canMove = value; }
        }

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

        #region Private variables
        private bool _canMove;
        private bool _dashing;
        private bool _grounded;
        private float _horizontalDirection;
        private Animator _animator;
        private Transform _visuals;
        private Rigidbody2D _rigidbody2D;
        private CharacterStamina _characterStamina; 
        #endregion

        public virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _visuals = _animator.transform;
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _characterStamina = GetComponent<CharacterStamina>();
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                grounded = true;
                canMove = true;
            }
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
            {
                grounded = false;
                canMove = false;
            }
        }

        #region Horizontal Movement
        public virtual void Move()
        {
            transform.Translate(Vector3.right * horizontalDirection * _movementSpeed * Time.deltaTime);
        }

        public virtual Vector2 GetInput()
        {
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        #endregion

        #region Dash
        public virtual IEnumerator Dash()
        {
            dashing = true;
            canMove = false;
            _rigidbody2D.drag = _dashLinearDrag;
            _rigidbody2D.AddForce(Vector2.right * Mathf.Sign(_visuals.localScale.x) * _dashForce, ForceMode2D.Impulse);
            _characterStamina.UseStamina(_characterStamina.dashCost);

            yield return new WaitForSeconds(0.75f);

            dashing = false;
            canMove = true;
        }
        #endregion

        #region Jump
        public virtual void Jump()
        {
            _rigidbody2D.drag = _jumpLinearDrag;
            _rigidbody2D.AddForce(_jumpDirection.normalized * _jumpForce, ForceMode2D.Impulse);
            _characterStamina.UseStamina(_characterStamina.jumpCost);
        }
        #endregion

        #region Editor
        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_jumpDirection);
        }
        #endregion
    }
}