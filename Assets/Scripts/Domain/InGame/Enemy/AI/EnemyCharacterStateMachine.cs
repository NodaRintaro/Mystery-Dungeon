using Cysharp.Threading.Tasks;
using Domain.Common.Interface;
using Domain.InGame.Character;
using Domain.InGame.System;

namespace Domain.InGame.Enemy.AI
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
        public EnemyCharacterStateMachine(ICharacterData enemyCharacterData)
        {
            _enemyCharacterData = enemyCharacterData;
        }

        private ICharacterData _enemyCharacterData;

        private CharacterMovement _characterMove;

        public ICharacterData EnemyCharacterData => _enemyCharacterData;
        public CharacterMovement CharacterMove => _characterMove;

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
            // 
            ChangeState(CharacterStateType.Idle).Forget();
        }

        private void Update()
        {
            // 
            _currentState.OnUpdate().Forget();
        }

        /// <summary>  </summary>
        /// <param name="stateType">  </param>
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








