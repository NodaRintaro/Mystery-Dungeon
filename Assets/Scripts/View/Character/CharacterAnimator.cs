using UnityEngine;

namespace Layer.View
{
    public class CharacterAnimator
    {
        public CharacterAnimator(Animator animator)
        {
            _characterAnimator = animator;
        }

        private Animator _characterAnimator;

        /// <summary> 歩くアニメーションの再生フラグ </summary>
        public void OnWalking(bool isWalking = true) => _characterAnimator.SetBool("IsWalk", isWalking);


        public void OnAttack(string attackName) => _characterAnimator.SetTrigger(attackName);

        public void OnDamage() => _characterAnimator.SetTrigger("Damage");

        public void AttackAnimation()
        {


        }
    }
}