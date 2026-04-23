using Data.Character;
using Roguelike.Dungeon.System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, IFactry<CharacterData>
{
    [SerializeField] private PlayerCharacterView _playerViewPrefab;

    private CharacterRepositry _characterDataRepositry = null;

    private DungeonData _dungeonData;

    private const int _playerID = 1;

    public DungeonData DungeonData => _dungeonData;

    private void Start()
    {
        ServiceLocator.TryGetService<CharacterRepositry>(out _characterDataRepositry);
    }

    public void Init(DungeonData dungeonData)
    {
        _dungeonData = dungeonData;
    }

    public CharacterData Spawn(int spawnObjId, Vector2Int spawnPosition)
    {
        CharacterData characterData = null;
        CharacterAssetData assetData = null;

        if(!_characterDataRepositry.TryCreateCharacter(spawnObjId, out assetData, out characterData))
        {
            return null;
        }

        GameObject characterObject = Instantiate(assetData.CharacterPrefab);
        characterObject.GetComponent<Animator>().runtimeAnimatorController = assetData.AnimatorController;

        // PlayerCharacterのViewを生成
        if (_playerViewPrefab != null)
            characterObject.transform.SetParent(Instantiate(_playerViewPrefab).gameObject.transform);

        PlayerController playerController = new PlayerController(characterData, characterObject.transform);

        return characterData;
    }
}
