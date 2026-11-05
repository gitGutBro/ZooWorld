using UnityEngine;
using UnityEngine.UI;

namespace ZooWorld.UI.DeathCounter
{
    public sealed class CounterItemView : MonoBehaviour
    {
        [SerializeField] private Text _title;
        [SerializeField] private Text _value;

        public void SetTitle(string title) =>
            _title.text = title;

        public void SetValue(int value) =>
            _value.text = value.ToString();
    }
}
