using R3;
using UnityEngine;
using ZooWorld.Animals.FoodChain.Roles;
using ZooWorld.Animals.Movement.Strategies;
using ZooWorld.Animals.Timing;

namespace ZooWorld.Animals.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class Animal : MonoBehaviour, IAnimal
    {
        [SerializeField] private Rigidbody _body;

        private readonly Subject<AnimalCollision> _collided = new();
        private readonly Subject<Animal> _died = new();
        private readonly CountdownTimer _knockback = new();

        private IMovementStrategy _movement;

        public AnimalConfig Config { get; private set; }
        public FoodChainRole Role => Config.Role;
        public bool IsAlive { get; private set; }
        public bool IsKnockedBack => !_knockback.IsFinished;
        public Vector3 Position => _body.position;
        public Vector3 Velocity { get; private set; }
        public Rigidbody Body => _body;
        public Observable<AnimalCollision> Collided => _collided;
        public Observable<Animal> Died => _died;

        private void OnCollisionEnter(Collision collision)
        {
            if (!IsAlive || !collision.rigidbody)
                return;

            _collided.OnNext(new AnimalCollision(self: this, collision.rigidbody));
        }

        private void OnValidate()
        {
            if (_body)
                return;

            _body = GetComponent<Rigidbody>();
        }

        private void OnDestroy()
        {
            _collided.Dispose();
            _died.Dispose();
        }

        public void Initialize(AnimalConfig config, IMovementStrategy movement)
        {
            Config = config;
            _movement = movement;
            _body.velocity = Vector3.zero;
            _body.angularVelocity = Vector3.zero;
            _knockback.Restart(durationSeconds: 0f);
            Velocity = Vector3.zero;
            IsAlive = true;
        }

        public void Place(Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            _body.position = position;
            _body.rotation = rotation;
        }

        public void Tick(float deltaTime)
        {
            if (!IsAlive)
                return;

            TickMovement(deltaTime);
            Velocity = _body.velocity;
        }

        public void Push(Vector3 velocityChange, float recoverySeconds)
        {
            if (!IsAlive)
                return;

            _body.AddForce(velocityChange, ForceMode.VelocityChange);
            _knockback.Restart(recoverySeconds);
        }

        public void Kill()
        {
            if (!IsAlive)
                return;

            IsAlive = false;
            _movement = null;
            _died.OnNext(this);
        }

        private void TickMovement(float deltaTime)
        {
            if (IsKnockedBack)
            {
                _knockback.Tick(deltaTime);
                return;
            }

            _movement.Tick(deltaTime);
        }
    }
}
