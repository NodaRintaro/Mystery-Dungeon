using Application;
using Domain;
using UnityEngine;


namespace Infrastructure
{
    public class AssetsLoadInitializer : MonoBehaviour
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
}




