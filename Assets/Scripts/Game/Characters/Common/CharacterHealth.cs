using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.UI;
using EZCameraShake;
using UnityEngine;

namespace CombatAI.Game.Characters
{
    public class CharacterHealth : MonoBehaviour
    {
        [TitleGroup("Parameters")]
        [FoldoutGroup("Parameters/Main")] [SerializeField] private float _maxHealth = 100f;
        [FoldoutGroup("Parameters/Main")] [SerializeField] private float _currentHealth = 100f;

        [FoldoutGroup("Parameters/Camera Shake")] [SerializeField] private float _magnitude = 4f;
        [FoldoutGroup("Parameters/Camera Shake")] [SerializeField] private float _roughness = 4f;
        [FoldoutGroup("Parameters/Camera Shake")] [SerializeField] private float _fadeInTime = 0.1f;
        [FoldoutGroup("Parameters/Camera Shake")] [SerializeField] private float _fadeOutTime = 1f;

        [Title("References")]
        [SerializeField] private Slider _characterHealthSlider;

        public float currentHealth
        {
            get => _currentHealth;
            set
            {
                _currentHealth = value;
                _characterHealthSlider.value = value;
            }
        }

        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private CharacterMovement _characterMovement;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rigidbody2D = GetComponentInChildren<Rigidbody2D>();
            _characterMovement = GetComponentInChildren<CharacterMovement>();
        }

        private void Start()
        {
            SetSliderValues();
        }

        private void SetSliderValues()
        {
            _characterHealthSlider.maxValue = _maxHealth;
            _characterHealthSlider.value = _currentHealth;
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            _animator.SetTrigger("Hit");

            CameraShaker.Instance.ShakeOnce(_magnitude, _roughness, _fadeInTime, _fadeOutTime);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Death();
            }
        }

        private void Death()
        {
            _characterMovement.canMove = false;
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.isKinematic = true;
            GetComponentInChildren<DamageOnCollision>().doDamage = false;

            do
            {
                transform.Translate(Vector3.down * Time.deltaTime);
            } while (transform.position.y > -1.35f);

            _animator.SetTrigger("Death");
        }
    }
}