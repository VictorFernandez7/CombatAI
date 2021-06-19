using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.Player
{
    public class PlayerAttack : CharacterAttack
    {
        private void Update()
        {
            if (!attacking)
            {
                if (Input.GetButton("AttackDown"))
                    Attack(Data.Attacks.Types.AttackDown);

                if (Input.GetButton("AttackUp"))
                    Attack(Data.Attacks.Types.AttackUp);
            }
        }
    }
}