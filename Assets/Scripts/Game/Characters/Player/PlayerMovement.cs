using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters.Player
{
    public class PlayerMovement : CharacterMovement
    {
        private CharacterAttack _characterAttack;

        public override void Awake()
        {
            base.Awake();

            _characterAttack = GetComponent<CharacterAttack>();
        }

        private void Update()
        {
            if (canMove)
                Move(horizontalDirection != 0f ? Mathf.Sign(horizontalDirection) : 0f);

            if (Input.GetButtonDown("Jump")) // CHECK INPUT
            {
                if (grounded && !_characterAttack.attacking && !_characterAttack.blocking) // CHECK CONDITIONS
                {
                    if (characterStamina.EnoughStamina(characterStamina.jumpCost)) // CHECK STAMINA
                        Jump();
                }
            }

            if (Input.GetButtonDown("Dash")) // CHECK INPUT
            {
                if (grounded && !dashing && !_characterAttack.attacking && !_characterAttack.blocking) // CHECK CONDITIONS
                {
                    if (characterStamina.EnoughStamina(characterStamina.dashCost)) // CHECK STAMINA
                        StartCoroutine(Dash());
                }
            }
        }

        private void FixedUpdate()
        {
            horizontalDirection = GetInput().x;
        }
    }
}