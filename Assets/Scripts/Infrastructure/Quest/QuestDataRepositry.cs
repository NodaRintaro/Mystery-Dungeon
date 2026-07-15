using Cysharp.Threading.Tasks;

using Domain.Common.Interface;
using Infrastructure;


namespace InGame.Infrastructure 
{ 
    public class QuestDataRepositry : IQuestDataRepositry
    {
        private AssetsLoader _assetsLoader = null;

        private string[] _questMasterDataCSV = null;

        /// <summary> 初期化 </summary>
        public void Init()
        {
            _assetsLoader.LoadAssetAsync<string[]>(AAGCSVMasterDataGroup.kAssets_MasterData_CSV_QuestDataCSV).Forget();

            
        }

        public IQuestData GetData(int id)
        {
            

            return default;
        }
    }
}
