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
                if (Input.GetButtonDown("AttackDown"))
                    Attack(Data.Attacks.Types.AttackDown);

                if (Input.GetButtonDown("AttackUp"))
                    Attack(Data.Attacks.Types.AttackUp);
            }

            if (!blocking)
            {
                if (Input.GetButtonDown("BlockDown"))
                    StartBlock(Data.Blocks.Types.BlockDown);

                if (Input.GetButtonDown("BlockUp"))
                    StartBlock(Data.Blocks.Types.BlockUp);
            }

            else
            {

                if (Input.GetButtonUp("BlockDown"))
                    EndBlock(Data.Blocks.Types.BlockDown);

                if (Input.GetButtonUp("BlockUp"))
                    EndBlock(Data.Blocks.Types.BlockUp);
            }
        }
    }
}