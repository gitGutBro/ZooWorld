using UnityEngine;

namespace ZooWorld.World
{
    public sealed class ScreenBounds : IScreenBounds
    {
        private static readonly Vector3 BottomLeftViewport = new(0f, 0f, 0f);
        private static readonly Vector3 TopRightViewport = new(1f, 1f, 0f);

        private readonly Camera _camera;
        private readonly Plane _ground = new(inNormal: Vector3.up, inPoint: Vector3.zero);

        private Rect _area;
        private Vector2Int _cachedScreenSize = new(-1, -1);

        public ScreenBounds(Camera camera) =>
            _camera = camera;

        public Rect Area
        {
            get
            {
                RefreshIfScreenChanged();
                return _area;
            }
        }

        public Vector3 Center
        {
            get
            {
                Vector2 center = Area.center;
                return new Vector3(center.x, 0f, center.y);
            }
        }

        public bool Contains(Vector3 point) =>
            Area.Contains(new Vector2(point.x, point.z));

        private void RefreshIfScreenChanged()
        {
            Vector2Int screenSize = new Vector2Int(Screen.width, Screen.height);

            if (screenSize == _cachedScreenSize)
                return;

            _cachedScreenSize = screenSize;
            _area = CalculateGroundArea();
        }

        private Rect CalculateGroundArea()
        {
            Vector3 first = ProjectToGround(BottomLeftViewport);
            Vector3 second = ProjectToGround(TopRightViewport);

            Vector2 min = new Vector2(Mathf.Min(first.x, second.x), Mathf.Min(first.z, second.z));
            Vector2 max = new Vector2(Mathf.Max(first.x, second.x), Mathf.Max(first.z, second.z));
            return Rect.MinMaxRect(min.x, min.y, max.x, max.y);
        }

        private Vector3 ProjectToGround(Vector3 viewportPoint)
        {
            Ray ray = _camera.ViewportPointToRay(viewportPoint);
            return _ground.Raycast(ray, out float distance) ? ray.GetPoint(distance) : ray.origin;
        }
    }
}
