using UnityEngine;

namespace ZooWorld.UI.DeathCounter
{
    public sealed class DeathCounterView : MonoBehaviour
    {
        [SerializeField] private RectTransform _itemsRoot;
        [SerializeField] private CounterItemView _itemPrefab;

        public CounterItemView CreateItem() =>
            Instantiate(_itemPrefab, _itemsRoot);
    }
}
