using Layer.Infrastructure;
using UnityEngine;

public class QuestSaveDataInitializer : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.RegisterService(new QuestSaveData());
    }

    private void OnDestroy()
    {
        ServiceLocator.UnregisterService<QuestSaveData>();
    }
}
