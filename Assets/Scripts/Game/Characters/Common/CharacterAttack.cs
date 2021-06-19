using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Collections;
using EZCameraShake;
using UnityEngine;

using CombatAI.Data;

namespace CombatAI.Game.Characters
{
    public class CharacterAttack : MonoBehaviour
    {
        [TitleGroup("Parameters")]
        [FoldoutGroup("Parameters/Attacks Duration")] [SerializeField] private float _attackDownDuration;
        [FoldoutGroup("Parameters/Attacks Duration")] [SerializeField] private float _attackUpDuration;

        [FoldoutGroup("Parameters/Camera Shake")] [SerializeField] private float _magnitude = 4f;
        [FoldoutGroup("Parameters/Camera Shake")] [SerializeField] private float _roughness = 4f;
        [FoldoutGroup("Parameters/Camera Shake")] [SerializeField] private float _fadeInTime = 0.1f;
        [FoldoutGroup("Parameters/Camera Shake")] [SerializeField] private float _fadeOutTime = 1f;

        #region Properties
        public bool attacking
        {
            get => _attacking;
            set
            {
                _attacking = value;
                _characterMovement.canMove = value;
            }
        }

        public bool blocking
        {
            get => _blocking;
            set
            {
                _blocking = value;
                _characterMovement.canMove = !value;
                _characterStamina.canRegenerate = !value;
            }
        } 
        #endregion

        private bool _blocking;
        private bool _attacking;
        private Animator _animator;
        private CharacterStamina _characterStamina;
        private CharacterMovement _characterMovement;

        public virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _characterStamina = GetComponent<CharacterStamina>();
            _characterMovement = GetComponent<CharacterMovement>();
        }

        #region Attack
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

            _characterStamina.UseStamina(_characterStamina.attackCost);
            CameraShaker.Instance.ShakeOnce(_magnitude, _roughness, _fadeInTime, _fadeOutTime);
        }

        public virtual IEnumerator AttackProcess(string animatorParameter, float attackDuration)
        {
            _attacking = true;
            _animator.SetFloat("RandomAttackDown", (int)Random.Range(0, 2));
            _animator.SetBool(animatorParameter, true);

            yield return new WaitForSeconds(attackDuration);

            _attacking = false;
            _animator.SetBool(animatorParameter, false);
        }
        #endregion

        #region Block
        public virtual void StartBlock(Blocks.Types blockType)
        {
            blocking = true;

            switch (blockType)
            {
                case Blocks.Types.BlockDown:
                    _animator.SetBool("BlockingDown", true);
                    break;
                case Blocks.Types.BlockUp:
                    _animator.SetBool("BlockingUp", true);
                    break;
            }
        }

        public virtual void EndBlock(Blocks.Types blockType)
        {
            blocking = false;

            switch (blockType)
            {
                case Blocks.Types.BlockDown:
                    _animator.SetBool("BlockingDown", false);
                    break;
                case Blocks.Types.BlockUp:
                    _animator.SetBool("BlockingUp", false);
                    break;
            }
        } 
        #endregion
    }
}