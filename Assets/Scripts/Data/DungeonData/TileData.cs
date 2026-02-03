using UnityEngine;

[System.Serializable, CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/TileData")]
public class TileData : ScriptableObject
{
    [Header("Tileのオブジェクト")]
    [SerializeField] private GameObject _tileObject;

    [Header("TileType")]
    [SerializeField] TileType _tileType;

    public GameObject TileObject => _tileObject;
    public TileType TileType => _tileType;
}