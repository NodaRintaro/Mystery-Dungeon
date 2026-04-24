using CRI_Sound_System;
using UnityEngine;

public class InGameInitializer : MonoBehaviour
{
    private void Awake()
    {
        ServiceLocator.RegisterService<AssetsLoader>(new AssetsLoader());
        ServiceLocator.RegisterService<CSVDateLoader>(new CSVDateLoader());
    }

    private void OnDestroy()
    {
        ServiceLocator.UnregisterService<AssetsLoader>();
        ServiceLocator.UnregisterService<CSVDateLoader>();
    }
}
