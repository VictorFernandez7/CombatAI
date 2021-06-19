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

        public float jumpCost => _jumpCost;
        public float dashCost => _dashCost;

        [Title("References")]
        [SerializeField] private Slider _characterStaminaSlider;

        public float currentStamina
        {
            get => _currentStamina;
            set
            {
                _currentStamina = value;
                _characterStaminaSlider.value = value;
            }
        }

        private void Start()
        {
            SetSliderValues();
        }

        private void Update()
        {
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
            return (_currentStamina >= requiredStaminaAmount);
        }
    }
}