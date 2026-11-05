using UnityEngine;

namespace ZooWorld.UI.TastyLabel
{
    public sealed class TastyLabelView : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Vector2 _screenOffset;

        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);

        public void SetScreenPosition(Vector3 screenPosition) =>
            _rect.position = (Vector2)screenPosition + _screenOffset;
    }
}
