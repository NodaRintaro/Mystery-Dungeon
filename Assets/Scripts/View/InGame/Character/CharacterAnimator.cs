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

        /// <summary> 攻撃アニメーションの再生フラグ </summary>
        public void OnAttack(string attackName) => _characterAnimator.SetTrigger(attackName);

        /// <summary> ダメージアニメーションの再生フラグ </summary>
        public void OnDamage() => _characterAnimator.SetTrigger("OnDamage");

        /// <summary> 死亡アニメーションの再生フラグ </summary>
        public void OnDead() => _characterAnimator.SetTrigger("OnDead");
    }
}