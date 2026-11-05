using UnityEngine;

namespace ZooWorld.UI.TastyLabel
{
    public sealed class TastyLabelLayer : MonoBehaviour
    {
        [SerializeField] private RectTransform _root;
        [SerializeField] private TastyLabelView _labelPrefab;

        public TastyLabelView CreateLabel() =>
            Instantiate(_labelPrefab, _root);
    }
}
