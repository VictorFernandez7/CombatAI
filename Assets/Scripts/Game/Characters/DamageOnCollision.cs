using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CombatAI.Game.Characters
{
    public class DamageOnCollision : MonoBehaviour
    {
        [Title("Parameters")]
        [SerializeField] private float _damage;
        [SerializeField] private float _impulseForce;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Character"))
            {
                collision.GetComponent<CharacterHealth>().TakeDamage(_damage);
            }
        }
    }
}