using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.AI
{
    public class AIMovement : CharacterMovement
    {
        private AIBrains _aIBrains;

        public override void Awake()
        {
            base.Awake();

            _aIBrains = GetComponent<AIBrains>();
        }
    }
}