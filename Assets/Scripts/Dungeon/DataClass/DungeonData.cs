using UnityEngine;

[System.Serializable]
public class DungeonData
{
    /// <summary> ダンジョンを形成する各セクションデータ </summary>
    [Header("ダンジョンを形成する各セクションデータ")]
    [SerializeField] SectionData[,] _sectionDataArray;

    public SectionData[,] SectionDataArray => _sectionDataArray;

    /// <summary> ダンジョンデータの初期化 </summary>
    public void InitDungeonData(int horizontalSectionRange, int verticalSectionRange)
    {
        _sectionDataArray = new SectionData[horizontalSectionRange, verticalSectionRange];
    }

    /// <summary> SectionDataの保存関数 </summary>
    public void SetSectionData(SectionData sectionData, int arrIndexX, int arrIndexY)
    {
        _sectionDataArray[arrIndexX, arrIndexY] = sectionData;
    }
}
