using Domain;
using UnityEngine;

namespace Application
{
    public class CharacterAnimator
    {
        public CharacterAnimator(Animator animator)
        {
            _characterAnimator = animator;
        }

        private Animator _characterAnimator;

        /// <summary> 歩行アニメーション </summary>
        public void OnWalking(bool isWalking = true) => _characterAnimator.SetBool("IsWalk", isWalking);


        public void OnAttack(string attackName) => _characterAnimator.SetTrigger(attackName);

        public void OnDamage() => _characterAnimator.SetTrigger("Damage");

        public void AttackAnimation()
        {


        }
    }
}





