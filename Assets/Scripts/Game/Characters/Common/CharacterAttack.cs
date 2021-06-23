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
        [TitleGroup("Class Parameters")]
        [FoldoutGroup("Class Parameters/Current Attack")] [SerializeField] [ReadOnly] [HideLabel] [EnumToggleButtons] private Attacks.Types _currentAttack;

        [FoldoutGroup("Class Parameters/Camera Shake")] [SerializeField] private float _magnitude = 4f;
        [FoldoutGroup("Class Parameters/Camera Shake")] [SerializeField] private float _roughness = 4f;
        [FoldoutGroup("Class Parameters/Camera Shake")] [SerializeField] private float _fadeInTime = 0.1f;
        [FoldoutGroup("Class Parameters/Camera Shake")] [SerializeField] private float _fadeOutTime = 1f;

        [TitleGroup("Class References")]
        [FoldoutGroup("Class References/Effects")] [SerializeField] private Animator _blockAnimator;

        #region Properties
        public bool blockingUp => _blockingUp;
        public bool blockingDown => _blockingDown;
        public Animator blockAnimator => _blockAnimator;
        public Attacks.Types currentAttack => _currentAttack;

        public bool attacking
        {
            get => _attacking;
            set
            {
                _attacking = value;
            }
        }

        public bool blocking
        {
            get => _blocking;
            set
            {
                _blocking = value;
                _characterStamina.canRegenerate = !value;
            }
        } 
        #endregion

        private bool _blocking;
        private bool _attacking;
        private bool _blockingUp;
        private bool _blockingDown;
        private string _animatorParameter;
        private Animator _animator;
        private CharacterHealth _characterHealth;
        private CharacterStamina _characterStamina;
        private CharacterMovement _characterMovement;

        public virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _characterStamina = GetComponent<CharacterStamina>();
            _characterMovement = GetComponent<CharacterMovement>();
            _characterHealth = GetComponent<CharacterHealth>();
        }

        #region Attack
        public virtual void Attack(Attacks.Types attackType)
        {
            if (_characterHealth.currentHealth <= 0f)
                return;

            switch (attackType)
            {
                case Attacks.Types.AttackDown:
                    PerformAttack("AttackingDown");
                    _currentAttack = Attacks.Types.AttackDown;
                    break;
                case Attacks.Types.AttackUp:
                    PerformAttack("AttackingUp");
                    _currentAttack = Attacks.Types.AttackUp;
                    break;
            }

            _characterStamina.UseStamina(_characterStamina.attackCost);
        }

        public virtual void PerformAttack(string animatorParameter)
        {
            _attacking = true;
            _animator.SetFloat("RandomAttackDown", (int)Random.Range(0, 2));
            _animator.SetBool(animatorParameter, true);
            _animatorParameter = animatorParameter;
        }

        public virtual void EndAttack()
        {
            _attacking = false;
            _animator.SetBool(_animatorParameter, false);
            _currentAttack = Attacks.Types.None;
        }

        public virtual void AttackCameraShake()
        {
            CameraShaker.Instance.ShakeOnce(_magnitude, _roughness, _fadeInTime, _fadeOutTime);
        }
        #endregion

        #region Block
        public virtual void StartBlock(Blocks.Types blockType)
        {
            blocking = true;

            switch (blockType)
            {
                case Blocks.Types.BlockDown:
                    _blockingDown = true;
                    _animator.SetBool("BlockingDown", true);
                    break;
                case Blocks.Types.BlockUp:
                    _blockingUp = true;
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
                    _blockingDown = false;
                    _animator.SetBool("BlockingDown", false);
                    break;
                case Blocks.Types.BlockUp:
                    _blockingUp = false;
                    _animator.SetBool("BlockingUp", false);
                    break;
            }
        } 
        #endregion
    }
}