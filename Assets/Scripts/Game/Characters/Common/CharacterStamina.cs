using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace CombatAI.Game.Characters
{
    public class CharacterStamina : MonoBehaviour
    {
        [TitleGroup("Parameters")]
        [FoldoutGroup("Parameters/Main")] [SerializeField] private float _maxStamina;
        [FoldoutGroup("Parameters/Main")] [SerializeField] private float _currentStamina;
        [FoldoutGroup("Parameters/Main")] [SerializeField] private float _staminaRegenerationRate;
        [FoldoutGroup("Parameters/Costs")] [SerializeField] private float _jumpCost;
        [FoldoutGroup("Parameters/Costs")] [SerializeField] private float _dashCost;
        [FoldoutGroup("Parameters/Costs")] [SerializeField] private float _attackCost;

        [Title("References")]
        [SerializeField] private Slider _characterStaminaSlider;
        [SerializeField] private ParticleSystem _noStaminaVFX;

        #region Properties
        public float jumpCost => _jumpCost;
        public float dashCost => _dashCost;
        public float attackCost => _attackCost;

        public bool canRegenerate
        {
            get => _canRegenerate;
            set { _canRegenerate = value; }
        }

        public float currentStamina
        {
            get => _currentStamina;
            set
            {
                _currentStamina = value;
                _characterStaminaSlider.value = value;
            }
        } 
        #endregion

        private bool _canRegenerate = true;
        private Animator _animator;

        private void Awake()
        {
            _animator = _characterStaminaSlider.GetComponentInParent<Animator>();
        }

        private void Start()
        {
            SetSliderValues();
        }

        private void Update()
        {
            if (canRegenerate)
                StaminaRegeneration();
        }

        private void SetSliderValues()
        {
            _characterStaminaSlider.maxValue = _maxStamina;
            _characterStaminaSlider.value = _currentStamina;
        }

        public void UseStamina(float amount)
        {
            currentStamina -= amount;

            if (currentStamina <= 0)
                currentStamina = 0;
        }

        private void StaminaRegeneration()
        {
            if (currentStamina < _maxStamina)
                currentStamina += Time.deltaTime * _staminaRegenerationRate;
        }

        public bool EnoughStamina(float requiredStaminaAmount)
        {
            bool enoughStamina = (_currentStamina >= requiredStaminaAmount);

            if (!enoughStamina)
            {
                _animator.SetTrigger("NoStamina");
                _noStaminaVFX.Stop();
                _noStaminaVFX.Play();
            }

            return enoughStamina;
        }
    }
}