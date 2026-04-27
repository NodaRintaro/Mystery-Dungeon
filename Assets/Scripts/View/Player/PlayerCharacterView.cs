using Application;
using Domain;
using UnityEngine;
using UnityEngine.InputSystem;


namespace View
{
    public class PlayerCharacterView : MonoBehaviour
    {
        private Transform _playerTransform;

        private CharacterAnimator _characterAnimator;

        private PlayerController _playerController;

        private Keyboard _currentKeyboard = null;

        private bool _isInit = false;

        public Keyboard CurrentKeyboard => _currentKeyboard;

        public void Init(Animator animator, PlayerController playerController)
        {
            _playerTransform = transform;
            _characterAnimator = new CharacterAnimator(animator);
            _playerController = playerController;

            _isInit = true;
        }

        private void Awake()
        {

        }

        private void Update()
        {
            if (_isInit) return;

            _currentKeyboard = Keyboard.current;




        }
    }
}






