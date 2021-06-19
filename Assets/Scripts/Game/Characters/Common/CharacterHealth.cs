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

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
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

        }
    }
}