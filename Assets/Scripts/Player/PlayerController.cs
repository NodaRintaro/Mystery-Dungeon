using Character.Action;
using Cysharp.Threading.Tasks;
using Data.Character;
using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController
{
    public PlayerController(ICharacterData characterData, DungeonData dungeonData, PlayerCharacterView playerCharacterView)
    {
        _characterData = characterData;
        _playerCharacterView = playerCharacterView;

        _playerApplication = new PlayerApplication(_characterData, dungeonData, _playerCharacterView.transform);
    }

    private PlayerApplication _playerApplication = null;

    private PlayerCharacterView _playerCharacterView = null;

    private ICharacterData _characterData = null;


    /// <summary> 舞フレーム行われる処理 </summary>
    /// <param name="keyboard"></param>
    public void OnUpDate(Keyboard keyboard)
    {
        // 行動可能でない場合は入力を受け付けない
        if (!_characterData.CanAction) return;

        OnInputMoveKeys(keyboard);

        OnInputAttack(keyboard).Forget();
    }

    /// <summary> キャラクターの移動入力 </summary>
    private void OnInputMoveKeys(Keyboard keyboard)
    {
        // カメラから見て前方に進む
        if (keyboard.wKey.wasPressedThisFrame)
        {
            
        }

        // カメラから見て後方に進む
        if (keyboard.sKey.wasPressedThisFrame)
        {
            
        }

        // カメラから見て左方向に進む
        if (keyboard.aKey.wasPressedThisFrame)
        {
            
        }

        // カメラから見て右方向に進む    
        if (keyboard.dKey.wasPressedThisFrame)
        {
            
        }
    }

    /// <summary> キャラクターの攻撃入力 </summary>
    public async UniTask OnInputAttack(Keyboard keyboard)
    {

    }


}
