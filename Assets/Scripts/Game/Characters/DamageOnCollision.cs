using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters
{
    public class DamageOnCollision : MonoBehaviour
    {
        [Title("Parameters")]
        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _impulseForce = 5f;
        [SerializeField] private Vector2 _impulseDirection = new Vector2(1.5f, 1.5f);

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Character") && !collision.GetComponent<CharacterMovement>().dashing)
            {
                collision.GetComponent<CharacterHealth>().TakeDamage(_damage);
                ApplyForce(collision.gameObject, collision.transform.position.x > transform.position.x);
            }
        }

        private void ApplyForce(GameObject targetCharacter, bool targetIsRight)
        {
            Vector2 finalImpulseDirection = new Vector2(targetIsRight ? _impulseDirection.x : -_impulseDirection.x, _impulseDirection.y);
            targetCharacter.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            targetCharacter.GetComponent<Rigidbody2D>().AddForce(finalImpulseDirection * _impulseForce, ForceMode2D.Impulse);
        }

        #region Editor
        public virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)_impulseDirection);
        }
        #endregion
    }
}