using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

using CombatAI.Game.Characters;

namespace CombatAI.Game.Utility
{
    public class AnimationEvents : MonoBehaviour
    {
        CharacterAttack _characterAttack;
        CharacterMovement _characterMovement;

        private void Awake()
        {
            _characterAttack = GetComponentInParent<CharacterAttack>();
            _characterMovement = GetComponentInParent<CharacterMovement>();
        }

        public void RemoveInputDetection()
        {
            _characterMovement.canMove = false;
        }

        public void GiveBackInputDetection()
        {
            _characterMovement.canMove = true;
        }

        public void FinishAttack()
        {
            _characterAttack.EndAttack();
        }
    }
}