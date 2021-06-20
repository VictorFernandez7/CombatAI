using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.AI
{
    public class AIMovement : CharacterMovement
    {
        public float currentDirection
        {
            get => _currentDirection;
            set { _currentDirection = value; }
        }

        private AIBrains _aIBrains;
        private float _currentDirection;

        public override void Awake()
        {
            base.Awake();

            _aIBrains = GetComponent<AIBrains>();
        }

        private void Update()
        {
            if (currentDirection != 0)
                Move(currentDirection);

            else
                animator.SetFloat("Speed", 0f);
        }

        public override void Move(float desiredDirection)
        {
            base.Move(desiredDirection);

            animator.SetFloat("Speed", 1f);
        }
    }
}