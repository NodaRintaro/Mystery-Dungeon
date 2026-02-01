using System.Collections.Generic;
using UnityEngine;

namespace RandomWeightPick
{
    [System.Serializable]
    public struct RandomPickItem<T>
    {
        [SerializeField] private T _itemValue;
        [SerializeField] private float _weight;

        public T ItemValue => _itemValue;
        public float Weight => _weight;

        public RandomPickItem(T value, float weight)
        {
            _itemValue = value;
            _weight = weight;
        }
    }

    public static class RandomPickItem
    {
        public static T SelectRandomItem<T>(List<RandomPickItem<T>> ItemList, bool isRemoveItem)
        {
            RandomPickItem<T> pickItem = default;
            float totalWeight = 0;
            foreach (var item in ItemList)
            {
                totalWeight += item.Weight;
            }

            float randomValue = Random.Range(0f, totalWeight);
            foreach (var item in ItemList)
            {
                randomValue -= item.Weight;
                if (randomValue <= 0)
                { 
                    pickItem = item;
                    break;
                }
            }
            if (isRemoveItem) 
                ItemList.Remove(pickItem);

            return pickItem.ItemValue;
        }
    }

}
