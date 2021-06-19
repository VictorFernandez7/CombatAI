using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace CombatAI.Game.Characters
{
    public class CharacterHealth : MonoBehaviour
    {
        [Title("Parameters")]
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _currentHealth;

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