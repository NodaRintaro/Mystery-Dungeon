using Cysharp.Threading.Tasks;
using Layer.Domain;

namespace Layer.Application
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
        public EnemyCharacterStateMachine(CharacterData enemyCharacterData)
        {
            _enemyCharacterData = enemyCharacterData;
        }

        private CharacterData _enemyCharacterData;

        private CharacterMovement _characterMove;

        public CharacterData EnemyCharacterData => _enemyCharacterData;
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
            // 蛻晄悄State繧棚dle縺ｫ險ｭ螳・
            ChangeState(CharacterStateType.Idle).Forget();
        }

        private void Update()
        {
            // 迴ｾ蝨ｨ縺ｮState蜀・・OnUpDate繧貞他縺ｳ蜃ｺ縺・
            _currentState.OnUpdate().Forget();
        }

        /// <summary> State螟画峩蜃ｦ逅・</summary>
        /// <param name="stateType"> 螟画峩蠕後・State </param>
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








