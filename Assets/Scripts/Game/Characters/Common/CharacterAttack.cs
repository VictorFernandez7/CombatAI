using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

using CombatAI.Data;

namespace CombatAI.Game.Characters
{
    public class CharacterAttack : MonoBehaviour
    {
        [TitleGroup("Parameters")]
        [FoldoutGroup("Parameters/Attacks Duration")] [SerializeField] private float _attackDownDuration;
        [FoldoutGroup("Parameters/Attacks Duration")] [SerializeField] private float _attackUpDuration;

        public bool attacking
        {
            get => _attacking;
            set
            {
                _attacking = value;
                _characterMovement.canMove = value;
            }
        }

        private bool _attacking;
        private Animator _animator;
        private CharacterMovement _characterMovement;

        public virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _characterMovement = GetComponent<CharacterMovement>();
        }

        public virtual void Attack(Attacks.Types attackType)
        {
            switch (attackType)
            {
                case Attacks.Types.AttackDown:
                    StartCoroutine(AttackProcess("AttackingDown", _attackDownDuration));
                    break;
                case Attacks.Types.AttackUp:
                    StartCoroutine(AttackProcess("AttackingUp", _attackUpDuration));
                    break;
            }
        }

        public virtual IEnumerator AttackProcess(string animatorParameter, float attackDuration)
        {
            _attacking = true;
            _animator.SetBool(animatorParameter, true);

            yield return new WaitForSeconds(attackDuration);

            _attacking = false;
            _animator.SetBool(animatorParameter, false);
        }
    }
}