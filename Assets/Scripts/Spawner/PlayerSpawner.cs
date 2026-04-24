using Data.Character;
using Roguelike.Dungeon.System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour, IFactry<CharacterData>
{
    [SerializeField] private PlayerCharacterView _playerPrefab;

    private CharacterRepositry _characterDataRepositry = null;

    private DungeonData _dungeonData;

    private const int _playerID = 1;

    public DungeonData DungeonData => _dungeonData;

    private void Start()
    {
        
    }

    public void Init(DungeonData dungeonData)
    {
        _dungeonData = dungeonData;
    }

    public CharacterData Spawn(int spawnObjId, Vector2Int spawnPosition)
    {
        CharacterData characterData = null;
        CharacterVisualData assetData = null;

        if(!_characterDataRepositry.TryCreateCharacter(spawnObjId, out assetData, out characterData))
        {
            return null;
        }

        PlayerCharacterView playerCharacterView = Instantiate(_playerPrefab);
        GameObject characterObject = Instantiate(assetData.CharacterPrefab);

        characterObject.GetComponent<Animator>().runtimeAnimatorController = assetData.AnimatorController;
        characterObject.transform.SetParent(playerCharacterView.transform);

        // PlayerCharacterのViewを生成
        if (_playerPrefab != null)
            characterObject.transform.SetParent(Instantiate(_playerPrefab).gameObject.transform);

        PlayerController playerController = new PlayerController(characterData, _dungeonData, playerCharacterView);

        return characterData;
    }
}
