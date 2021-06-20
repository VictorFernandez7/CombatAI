using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

using CombatAI.Data;

namespace CombatAI.Game.Characters
{
    public class DamageOnCollision : MonoBehaviour
    {
        [TitleGroup("Parameters")]
        [FoldoutGroup("Parameters/Main")] [SerializeField] private bool _isAnAttack;
        [FoldoutGroup("Parameters/Impact")] [SerializeField] private float _damage = 10f;
        [FoldoutGroup("Parameters/Impact")] [SerializeField] private float _damageImpulseForce = 3.5f;
        [FoldoutGroup("Parameters/Impact")] [SerializeField] private Vector2 _damageImpulseDirection = new Vector2(1.5f, 1.5f);
        [FoldoutGroup("Parameters/Block")] [SerializeField] private float _blockmpulseForce = 2f;
        [FoldoutGroup("Parameters/Block")] [SerializeField] private Vector2 _blockmpulseDirection = new Vector2(1.5f, 0.5f);

        public bool doDamage
        {
            get => _doDamage;
            set { _doDamage = value; }
        }

        private bool _doDamage = true;
        private CharacterAttack _attacker;

        public enum Situations
        {
            Damage,
            Block
        }

        private void Awake()
        {
            _attacker = GetComponentInParent<CharacterAttack>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (doDamage)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && !collision.GetComponent<CharacterMovement>().dashing && !CheckIfCharacterIsBlocking(collision.GetComponent<CharacterAttack>()))
                {
                    collision.GetComponent<CharacterHealth>().TakeDamage(_damage);
                    ApplyForce( Situations.Damage, collision.gameObject, collision.transform.position.x > transform.position.x);
                }
            }
        }

        private void ApplyForce(Situations situation, GameObject targetCharacter, bool targetIsRight)
        {
            Vector2 finalImpulseDirection = Vector2.zero;

            switch (situation)
            {
                case Situations.Damage:
                    finalImpulseDirection = new Vector2(targetIsRight ? _damageImpulseDirection.x : -_damageImpulseDirection.x, _damageImpulseDirection.y);
                    targetCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    targetCharacter.GetComponent<Rigidbody2D>().AddForce(finalImpulseDirection * _damageImpulseForce, ForceMode2D.Impulse);
                    break;
                case Situations.Block:
                    finalImpulseDirection = new Vector2(targetIsRight ? _blockmpulseDirection.x : -_blockmpulseDirection.x, _blockmpulseDirection.y);
                    targetCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    targetCharacter.GetComponent<Rigidbody2D>().AddForce(finalImpulseDirection * _blockmpulseForce, ForceMode2D.Impulse);
                    break;
            }
        }

        /// <summary>
        /// Checks is the defender blocks the attack
        /// </summary>
        /// <param name="defender"></param>
        /// <returns></returns>
        private bool CheckIfCharacterIsBlocking(CharacterAttack defender)
        {
            if (!_isAnAttack)
                return false;

            if ((_attacker.currentAttack == Attacks.Types.AttackDown && defender.blockingDown)
                || (_attacker.currentAttack == Attacks.Types.AttackUp && defender.blockingUp))
            {
                ApplyForce( Situations.Block, defender.gameObject, defender.transform.position.x > transform.position.x);
                return true;
            }

            else
                return false;
        }

        #region Editor
        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_damageImpulseDirection);
        }
        #endregion
    }
}