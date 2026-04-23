using Character.Action;
using Cysharp.Threading.Tasks;
using Data.Character;
using Roguelike.Dungeon;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

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
        if (_isInit)
        {
            _currentKeyboard = Keyboard.current;
            if (_currentKeyboard == null) return;

            OnInputMove();
        }
    }

    /// <summary> キャラクターの移動入力 </summary>
    private void OnInputMove()
    {
        // カメラから見て前方に進む
        if (_currentKeyboard.wKey.wasPressedThisFrame)
        {
            
        }

        // カメラから見て後方に進む
        if (_currentKeyboard.sKey.wasPressedThisFrame)
        {
            
        }

        // カメラから見て左方向に進む
        if (_currentKeyboard.aKey.wasPressedThisFrame)
        {
            
        }

        // カメラから見て右方向に進む    
        if (_currentKeyboard.dKey.wasPressedThisFrame)
        {
            
        }
    }

    /// <summary> キャラクターの攻撃入力 </summary>
    public async UniTask OnInputAttack()
    {

    }
}
