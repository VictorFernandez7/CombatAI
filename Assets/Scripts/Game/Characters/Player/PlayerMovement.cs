using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.Player
{
    public class PlayerMovement : CharacterMovement
    {
        private void Update()
        {
            if (canMove)
                Move(horizontalDirection);

            if (Input.GetButtonDown("Jump") && grounded) // CHECK INPUT
            {
                if (characterStamina.EnoughStamina(characterStamina.jumpCost)) // CHECK STAMINA
                    Jump();
            }

            if (Input.GetButtonDown("Dash") && grounded && !dashing) // CHECK INPUT
            {
                if (characterStamina.EnoughStamina(characterStamina.dashCost)) // CHECK STAMINA
                    StartCoroutine(Dash());
            }
        }

        private void FixedUpdate()
        {
            horizontalDirection = GetInput().x;
        }
    }
}