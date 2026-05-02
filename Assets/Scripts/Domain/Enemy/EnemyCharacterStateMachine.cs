using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Domain
{
    public enum CharacterStateType
    {
        Idle,
        Walk,
        Attack,
        Hurt,
        Die
    }

    public class EnemyCharacterStateMachine : StateMachine<CharacterStateType>
    {
        private GameObject _characterObject;

        private CharacterMovement _characterMove;

        private CharacterSkill _characterAttack;

        public GameObject CharacterObject => _characterObject;
        public CharacterMovement CharacterMove => _characterMove;
        public CharacterSkill CharacterAttack => _characterAttack;

        private void Awake()
        {
            _stateDict.Add(CharacterStateType.Idle, new CharacterIdleState(this));
            _stateDict.Add(CharacterStateType.Walk, new CharacterWalkState(this));
            _stateDict.Add(CharacterStateType.Attack, new CharacterAttackState(this));
            _stateDict.Add(CharacterStateType.Hurt, new CharacterHurtState(this));
            _stateDict.Add(CharacterStateType.Die, new CharacterDieState(this));
        }

        private void Start()
        {
            // 初期StateをIdleに設定
            ChangeState(CharacterStateType.Idle).Forget();
        }

        private void Update()
        {
            // 現在のState内のOnUpDateを呼び出す
            _currentState.OnUpdate().Forget();
        }

        /// <summary> State変更処理 </summary>
        /// <param name="stateType"> 変更後のState </param>
        public override async UniTask ChangeState(CharacterStateType stateType)
        {
            if (_currentState != null)
            {
                await _currentState.OnExit();
            }

            _currentState = _stateDict[stateType];
            await _currentState.OnEnter();
        }

        #region CharacterState_Idle
        public class CharacterIdleState : State<CharacterStateType>
        {
            public CharacterIdleState(StateMachine<CharacterStateType> stateMachine) => _stateMachine = stateMachine;

            public override async UniTask OnEnter()
            {
                await UniTask.CompletedTask;
            }

            public override async UniTask OnUpdate()
            {
                await UniTask.CompletedTask;
            }

            public override async UniTask OnExit()
            {
                await UniTask.CompletedTask;
            }
        }
        #endregion

        #region CharacterState_Action
        public class CharacterActionState : State<CharacterStateType>
        {
            public CharacterActionState(StateMachine<CharacterStateType> stateMachine) => _stateMachine = stateMachine;

            public override async UniTask OnEnter()
            {
                await UniTask.CompletedTask;
            }

            public override async UniTask OnUpdate()
            {
                await UniTask.CompletedTask;
            }

            public override async UniTask OnExit()
            {
                await UniTask.CompletedTask;
            }
        }
        #endregion

        #region CharacterState_Walk
        public class CharacterWalkState : State<CharacterStateType>
        {
            public CharacterWalkState(StateMachine<CharacterStateType> stateMachine) => _stateMachine = stateMachine;
        }
        #endregion

        #region CharacterState_Attack
        public class CharacterAttackState : State<CharacterStateType>
        {
            public CharacterAttackState(StateMachine<CharacterStateType> stateMachine) => _stateMachine = stateMachine;
        }
        #endregion

        #region CharacterState_Hurt
        public class CharacterHurtState : State<CharacterStateType>
        {
            public CharacterHurtState(StateMachine<CharacterStateType> stateMachine) => _stateMachine = stateMachine;
        }
        #endregion

        #region CharacterState_Die
        public class CharacterDieState : State<CharacterStateType>
        {
            public CharacterDieState(StateMachine<CharacterStateType> stateMachine) => _stateMachine = stateMachine;
        }
        #endregion
    }
}





