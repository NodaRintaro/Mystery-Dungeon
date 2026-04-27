using Domain;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure
{
    /// <summary> ステージ全体の更新クラス </summary>
    public class DungeonManager : MonoBehaviour
    {
        [Header("ダンジョン生成クラス")]
        [SerializeField] private StageGenerator _dungeonGenerator;

        [Header("スポナー各種")]
        [SerializeField] private PlayerSpawner _playerSpawner;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private ItemSpawner _itemSpawner;

        private DungeonData _currentStageData;

        // 現在の階層
        private int _currentStageFloorNum;

        private void Awake()
        {
            // スポナーの初期化
            _playerSpawner.Init(_currentStageData);
            _enemySpawner.Init(_currentStageData);
            _itemSpawner.Init(_currentStageData);
        }

        private void Start()
        {
            _currentStageFloorNum = 0;

            // ステージの生成
            CreateNewStage().Forget();

            //_playerSpawner.Spawn();
        }



        private void Update()
        {

        }

        /// <summary> 次の階層に行く関数 </summary>
        public async UniTask NextFloorStage()
        {
            await DestroyCurrentStage();

            _currentStageFloorNum++;

            // ステージの生成
            await CreateNewStage();
        }

        /// <summary> ステージの生成を行うメソッド </summary>
        public async UniTask CreateNewStage()
        {
            _currentStageData = new(await _dungeonGenerator.GenerateStage(), _dungeonGenerator.GridSize);


        }

        /// <summary> ステージの削除を行うメソッド </summary>
        public async UniTask DestroyCurrentStage()
        {

        }
    }
}





