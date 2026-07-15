using UnityEngine;

namespace View.InGame.Character
{
    public class CharacterAnimator
    {
        public CharacterAnimator(Animator animator)
        {
            _characterAnimator = animator;
        }

        // Animatorパラメータのハッシュ
        private readonly int _hashIsWalk = Animator.StringToHash("IsWalk");
        private readonly int _hashOnDamage = Animator.StringToHash("OnDamage");

        private Animator _characterAnimator;

        /// <summary> 歩くアニメーションの再生フラグ </summary>
        public void OnWalking(bool isWalking = true) 
        { 
            _characterAnimator.SetBool(_hashIsWalk, isWalking); 
        }

        /// <summary> 攻撃アニメーションの再生フラグ </summary>
        public void OnAttack(string attackName)
        {
            _characterAnimator.SetTrigger(attackName);
        }

        /// <summary> ダメージアニメーションの再生フラグ </summary>
        public void OnDamage() 
        { 
            _characterAnimator.SetTrigger(_hashOnDamage); 
        } 

        /// <summary> 死亡アニメーションの再生フラグ </summary>
        public void OnDead() => _characterAnimator.SetTrigger("OnDead");


    }
}