using Domain;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Application
{
[Serializable]
public abstract class State<TStateType>
{
    protected StateMachine<TStateType> _stateMachine;

    /// <summary> このステートに遷移した時に行う処理 </summary>
    public virtual async UniTask OnEnter()
    {
        await UniTask.CompletedTask;
    }

    /// <summary> このステートにいる間、毎フレーム行う処理 </summary>
    public virtual async UniTask OnUpdate()
    {
        await UniTask.CompletedTask;
    }

    /// <summary> 別のステートへの遷移時に行う処理 </summary>
    public virtual async UniTask OnExit()
    {
        await UniTask.CompletedTask;
    }
}


public abstract class StateMachine<TStateType>
{
    protected State<TStateType> _currentState;

    protected Dictionary<TStateType, State<TStateType>> _stateDict = new Dictionary<TStateType, State<TStateType>>();

    /// <summary> 現在のステートの処理を終了し、次のステートへ移行する </summary>
    public abstract UniTask ChangeState(TStateType stateType);
}

}




