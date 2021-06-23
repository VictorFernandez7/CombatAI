using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

using CombatAI.Game.Characters;

namespace CombatAI.Game.Utility
{
    public class AnimationEvents : MonoBehaviour
    {
        CharacterMovement _characterMovement;

        private void Awake()
        {
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
    }
}