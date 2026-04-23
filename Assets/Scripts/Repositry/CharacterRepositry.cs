using UnityEngine;
using System.Linq;
using Data.Character;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CharacterRepositry 
{
    private const string _csvDataPath = "CSVData/CharacterDataCSV";
    private const string _characterAssetsPath = "Character";

    private string[,] _characterDataCSV = null;
    private CharacterAssetData[] _characterAssetDataArray = null;

    private List<CharacterData> _characterDataList = new List<CharacterData>();

    public IReadOnlyCollection<CharacterData> CharacterDataList => _characterDataList;

    public CharacterRepositry()
    {
        LoadData().Forget();
    }

    public bool TryCreateCharacter(int characterID, out CharacterAssetData characterAssetData, out CharacterData characterData)
    {
        if(!TryGetCharacterData(characterID, out characterData) || !TryGetCharacterAssetData(characterID, out characterAssetData))
        {
            characterAssetData = null;
            characterData = null;
            return false;
        }
        else
        {
            _characterDataList.Add(characterData);
            return true;
        }
    }

    private async UniTask LoadData()
    {
        TextAsset csvFile = null;
        ResourceRequest request = Resources.LoadAsync<TextAsset>(_csvDataPath);

        await request.ToUniTask();

        if (request.asset == null)
        {
            Debug.LogError($"Asset not found at: {_csvDataPath}");
            return;
        }

        csvFile = (TextAsset)request.asset;

        _characterDataCSV = CSVDateLoader.ParseCsv(csvFile.text);
        _characterAssetDataArray = Resources.LoadAll<CharacterAssetData>(_characterAssetsPath).ToArray();
    }

    private bool TryGetCharacterAssetData(int characterID, out CharacterAssetData characterAssetData)
    {
        characterAssetData = null;
        foreach (var assetData in _characterAssetDataArray)
        {
            if (assetData.CharacterID == characterID)
            {
                characterAssetData = assetData;
                return true;
            }
        } 
        return false;
    }

    private bool TryGetCharacterData(int characterID, out CharacterData characterData)
    {
        characterData = null;

        for (int i = 1; i < _characterDataCSV.GetLength(0); i++)
        {
            if(int.TryParse(_characterDataCSV[i, 0], out int id) && id == characterID)
            {
                characterData = new CharacterData
                (
                    // キャラクターのステータスをCSVから取得してCharacterStatusクラスのインスタンスを生成
                    new CharacterStatus
                    (
                        id,
                        int.Parse(_characterDataCSV[i, 1]),
                        int.Parse(_characterDataCSV[i, 2]),
                        int.Parse(_characterDataCSV[i, 3]),
                        int.Parse(_characterDataCSV[i, 4]),
                        int.Parse(_characterDataCSV[i, 5]),
                        int.Parse(_characterDataCSV[i, 6]),
                        int.Parse(_characterDataCSV[i, 7]),
                        int.Parse(_characterDataCSV[i, 8]),
                        int.Parse(_characterDataCSV[i, 9]),
                        int.Parse(_characterDataCSV[i, 10]),
                        int.Parse(_characterDataCSV[i, 11]),
                        int.Parse(_characterDataCSV[i, 12])
                    ),

                    // キャラクターの成長曲線をCSVから取得してCharacterGrowthRatesクラスのインスタンスを生成
                    new CharacterGrowthRates
                    (
                        int.Parse(_characterDataCSV[i, 13]),
                        int.Parse(_characterDataCSV[i, 14]),
                        int.Parse(_characterDataCSV[i, 15]),
                        int.Parse(_characterDataCSV[i, 16]),
                        int.Parse(_characterDataCSV[i, 17]),
                        int.Parse(_characterDataCSV[i, 18]),
                        int.Parse(_characterDataCSV[i, 19])
                    )
                );

                return true;
            }
        }

        return false;
    }
}
