using Application.InGame.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using View.InGame.Character;


namespace View.InGame.Player
{
    public class PlayerCharacterView : MonoBehaviour
    {
        [Header("キャラクターのAnimator")]
        [SerializeField] private Animator _animator = null;

        [Header("キャラクターのTransform")]
        [SerializeField] private Transform _playerTransform = null;

        // キャラクターのAnimator管理クラス
        private CharacterAnimator _characterAnimator;

        // プレーヤーのControllerクラス
        private PlayerController _playerController;

        // 使用中のキーボード
        private Keyboard _currentKeyboard = null;

        // 初期化完了フラグ
        private bool _isInit = false;

        public void Init(PlayerController playerController, Animator animator)
        {
            _playerTransform = transform;
            _playerController = playerController;

            _characterAnimator = new CharacterAnimator(animator);

            _isInit = true;
        }

        private void Update()
        {
            if (!_isInit) return;
            if (_playerController == null) return;

            _currentKeyboard = Keyboard.current;
            _playerController.OnUpDate(_currentKeyboard);
        }
    }
}









