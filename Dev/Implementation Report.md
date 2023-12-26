## Extensions
I created a set of extension methods to ease development, one of the key ones being an extension to `Unity.Object` allowing me to use null propagation, this cannot normally be done as when an object is null in Unity it is just marked for deletion rather than truly null and the boolean comparison is operator overloaded so you use `if (component) { // Logic }` syntax
```cs
/// <summary>
    /// Gets, or adds if doesn't contain a component
    /// </summary>
    /// <typeparam name="T">Component Type</typeparam>
    /// <param name="gameObject">GameObject to get component from</param>
    /// <returns>Component</returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        T component = gameObject.GetComponent<T>();
        if (!component) {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }
    
    /// <summary>
    /// Returns true if a GameObject has a component of type T
    /// </summary>
    /// <typeparam name="T">Component Type</typeparam>
    /// <param name="gameObject">GameObject to check for component on</param>
    /// <returns>If the component is present</returns>
    public static bool HasComponent<T>(this GameObject gameObject) where T : Component { 
        return gameObject.GetComponent<T>() != null; 
    }

    /// <summary>
    /// Allows for use of null propogation on Unity Components as Unity uses
    /// null as 'marked for destroying', for example:
    /// <code>
    /// float value = GetComponent&lt;MagicType&gt;().OrNull&lt;MagicType&gt;()?.MagicFloatField ?? _defaultMagicFloatValue;
    /// </code>
    /// </summary>
    /// <typeparam name="T">Type of UnityObject to check for being actually bull</typeparam>
    /// <param name="obj">Object to check for null reference on</param>
    /// <returns>T or null if marked as null</returns>
    public static T OrNull<T>(this T obj) where T : UnityEngine.Object => obj ? obj : null;

    ///<summary>
    ///Normalises a Vector3 if its magnitude is larger than one
    ///</summary>
    ///<param name="vector">Vector to clamp</param>
    public static void ClampToNormalised(this Vector3 vector) {
        if (vector.magnitude > 1f) {
            vector.Normalize();
        }
    }

    ///<summary>
    ///Modifies the specified component(s) of a vector
    ///</summary>
    ///<param name="vector">Vector to modifiy</param>
    ///<param name="x">New x value if specified</param>
    ///<param name="y">New y value if specified</param>
    ///<param name="z">New z value if specified</param>
    ///<returns>Modified vector</returns>
    public static Vector3 With(this Vector3 vector, float? x, float? y, float? z) {
        return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
        
    }

    ///<summary>
    ///Adds to the specified component(s) of a vector
    ///</summary>
    ///<param name="vector">Vector to modifiy</param>
    ///<param name="x">Increase in x value if specified</param>
    ///<param name="y">Increase in y value if specified</param>
    ///<param name="z">Increase in z value if specified</param>
    ///<returns>Modified vector</returns>
    public static Vector3 Add(this Vector3 vector, float? x = 0, float? y = 0, float? z = 0) {
        return new Vector3(vector.x + (x ?? 0f), vector.y + (y ?? 0f), vector.z + (z ?? 0f));
    }

    ///<summary>
    ///Adds to the specified component(s) of a vector
    ///</summary>
    ///<param name="vector">Vector to modifiy</param>
    ///<param name="x">Increase in x value if specified</param>
    ///<param name="y">Increase in y value if specified</param>
    ///<param name="z">Increase in z value if specified</param>
    ///<returns>Modified vector</returns>
    public static Vector2 Multiply(this Vector2 vector, float? x = 1f, float? y = 1f) {
        return new Vector2(vector.x * (x ?? 1f), vector.y * (y ?? 1f));
    }

    public static Vector2 ClampMagnitude(this Vector2 vector, float magnitude) {
        if (vector.sqrMagnitude > (magnitude * magnitude)) {
            vector.Normalize();
            vector *= magnitude;
        }
        return vector;
    }

    ///<summary>
    ///Changes the colour of the material on the provided SpriteRenderer for the specified time
    ///using a coroutine that must have the MonoBehaviour to attach the coroutine to
    ///</summary>
    ///<param name="spriteRenderer">SpriteRenderer to change material colour of</param>
    ///<param name="colour">Colour to change SpriteRenderer material to</param>
    ///<param name="time">Time until colour changes back</param>
    ///<param name="monoBehaviour">MonoBehaviour to start coroutine on</param>
    public static void FlashColour(this SpriteRenderer spriteRenderer, Color colour, float time, MonoBehaviour monoBehaviour) {
        if (monoBehaviour.OrNull() == null) {
            Debug.Log("Provided MonoBehaviour was null!");
            return;
        }
        monoBehaviour.StartCoroutine(Flash(spriteRenderer, colour, time));
    }

    public static void FlashColour(this SpriteRenderer spriteRenderer, Color flashColour, Color originalColour, float time, MonoBehaviour monoBehaviour) {
        if (monoBehaviour.OrNull() == null) {
            Debug.Log("Provided MonoBehaviour was null!");
            return;
        }
        monoBehaviour.StartCoroutine(Flash(spriteRenderer, flashColour, originalColour, time));
    }


    private static IEnumerator Flash(SpriteRenderer spriteRenderer, Color colour, float time) { 
        Color original = spriteRenderer.color;
        spriteRenderer.color = colour;
        yield return Yielders.WaitForSeconds(time);
        spriteRenderer.color = original;
    }

    private static IEnumerator Flash(SpriteRenderer spriteRenderer, Color colour, Color originalColour, float time) { 
        spriteRenderer.color = colour;
        yield return Yielders.WaitForSeconds(time);
        spriteRenderer.color = originalColour;
    }
```

## Enemy State Machine
As the enemies in the game have very fixed logic the state pattern lends itself well to implementing a controller for them. The state machine exists as an `EnemyState` instance inside the `EnemyController` class which is then updated by the class:
```cs
using System;
using System.Collections.Generic;


using UnityEngine;
namespace Enemy {
    public abstract class EnemyState : IEnemyState {
        protected readonly EnemyController _controller;
        protected readonly SpriteRenderer _spriteRenderer;
        protected readonly EnemyManager _enemyManager;
        protected readonly EnemyStateFactory _enemyStateFactory;

        public EnemyState(EnemyController controller, SpriteRenderer spriteRenderer, EnemyManager enemyManager, EnemyStateFactory enemyStateFactory) {
            _controller = controller;
            _spriteRenderer = spriteRenderer;
            _enemyManager = enemyManager;
            _enemyStateFactory = enemyStateFactory;
        }

        public abstract void Start();
        public abstract void FixedUpdate();
        public abstract void Update();
        public abstract void CheckTransitions();
        public abstract void Exit();

        public void SwitchState(EnemyState state) {
            _controller.State?.Exit();
            _controller.State = state;
            _controller.State.Start();
        }

        public class EnemyStateFactory {

            private Dictionary<Type, EnemyState> _states;

            public EnemyStateFactory(EnemyController controller, SpriteRenderer renderer, EnemyManager manager) {
                _states = new Dictionary<Type, EnemyState>() {
                { typeof(EnemyIdleState), new EnemyIdleState(controller, renderer, manager, this) },
                { typeof(EnemyPatrolState), new EnemyPatrolState(controller, renderer, manager, this) },
                { typeof(EnemyChaseState), new EnemyChaseState(controller, renderer, manager, this) },
                { typeof(EnemyAttackState), new EnemyAttackState(controller, renderer, manager, this) },
            };
            }

            public void ValidateStates() {
                foreach (EnemyState state in _states.Values) {
                    Debug.Log($"{state.GetType()} is valid: {(state._enemyStateFactory == null ? "no" : "yes")}");
                }
            }

            public T State<T>() where T : EnemyState {
                if (!_states.ContainsKey(typeof(T))) {
                    Debug.Log($"Could not find state: {typeof(T)}");
                    return _states[typeof(EnemyIdleState)] as T;
                }
                return _states[typeof(T)] as T;
            }
        }
    }
}
```

In the `EnemyController` class is responsible for calling the concrete implementation of the abstract methods defined in `EnemyState` along with managing some other references, providing some properties for the states to use and rotating the enemy as the `NavMeshAgent` does not work with 2D rotation:
```cs
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.AI;

using Utilities;

namespace Enemy {
    public class EnemyController : MonoBehaviour {

        [Header("Component References")]
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private EnemyManager _enemyManager;

        [Header("Enemy Parameters")]
        [SerializeField] private float _maxSpeed = 200f;
        [SerializeField] private float _maxAcceleration = 60f;
        [SerializeField] private float _maxTurnSpeed = 5f;
        [SerializeField] private float _chaseRange = 10f;
        [SerializeField] private float _attackRange = 5f;
        [SerializeField] private float _fireRate = 1f;
        [SerializeField] private float _damage = 1f;

        [Header("Gameplay Variables")]
        [SerializeField] private CountDownTimer _attackTimer;
        private EnemyState _state;
        private EnemyState.EnemyStateFactory _stateFactory;

        public EnemyState State { get { return _state; } set { _state = value; } }
        public NavMeshAgent Agent { get { return _agent; } }
        public CountDownTimer AttackTimer { get { return _attackTimer; } }
        public float Damage => _damage;
        public bool InChaseRange => HasTarget && Vector3.Distance(_enemyManager.Target.OrNull()?.position ?? new Vector3(Mathf.Infinity, Mathf.Infinity), transform.position) <= _chaseRange;
        public bool InAttackRange => HasTarget && Vector3.Distance(_enemyManager.Target.OrNull()?.position ?? new Vector3(Mathf.Infinity, Mathf.Infinity), transform.position) <= _attackRange;
        public bool HasTarget => _enemyManager.Target != null;

        public void Awake() {
            _agent = GetComponent<NavMeshAgent>();
            _agent.acceleration = _maxAcceleration;
            _agent.speed = _maxSpeed;
            _agent.updateUpAxis = false;
            _agent.updateRotation = false;
            _agent.stoppingDistance = _attackRange - 0.5f;
            _enemyManager = transform.parent.GetComponent<EnemyManager>();
            _attackTimer = new CountDownTimer(_fireRate);
        }

        public void Start() {
            _stateFactory = new EnemyState.EnemyStateFactory(this, GetComponent<SpriteRenderer>(), _enemyManager);
            _state = _stateFactory.State<EnemyPatrolState>();
            if (_state == null) {
                Debug.LogError("State was not initialised!");
            }
            _state?.Start();
        }

        public void Update() {
            _state?.Update();
            _state?.CheckTransitions();
            UpdateRotation();
        }

        public void FixedUpdate() {
            _state?.FixedUpdate();
            _attackTimer.Update(Time.fixedDeltaTime);
        }

        public void UpdateRotation() {
            if (!HasTarget) {
                return;
            }
            float angle = Vector2.SignedAngle((Vector2)(_enemyManager.Target.position - transform.position), Vector2.up);
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.back);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * _maxTurnSpeed);
        }
    }
}
```

## Boss Attack Strategies
The boss attacks in the game are stored as a list of `BossAttacks` which is an abstract class inheriting `ScriptableObject` and providing an abstract `Attack(Transform origin)` method that concrete implementations must implement.
```cs
using UnityEngine;
namespace Boss {

    public abstract class BossAttack : ScriptableObject {
        public float Cooldown = 1f;
        public abstract void Attack(Transform origin);
    }
}
```

Then using this as a base I can create the concrete implementations of the attacks and choose which components I should add and also add any prefab references statically in the `ScriptableObject`:
```cs
using System.Linq;

using UnityEngine;

namespace Boss {
    public class ArcProjectileController : MonoBehaviour {
        [SerializeField] private float _speed;
        [SerializeField] private float _radius;
        [SerializeField] private float _damage;
        [SerializeField] private Vector3 _target;
        [SerializeField] private Vector3 _linearPosition;
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private float _progress = 0f;
        [SerializeField] private float _height;
        [SerializeField] private Rigidbody2D _rb2D;

        private void Awake() {
            _rb2D = GetComponent<Rigidbody2D>();
            _radius = GetComponent<SpriteRenderer>().bounds.extents.x;
        }

        public void Init(float speed, float damage, float height, Vector3 target) {
            _speed = speed;
            _damage = damage;
            _target = target;
            _height = height;
            _linearPosition = transform.position;
            _initialPosition = transform.position;
        }

        public void FixedUpdate() {
            if (Vector2.Distance(_target, transform.position) < _radius) {
                Physics2D.OverlapCircleAll(transform.position, _radius, Globals.Instance.PlayerLayer)
                    .Where((Collider2D entity) => entity.gameObject.HasComponent<PlayerController>())
                    .FirstOrDefault().OrNull()?
                    .GetComponent<Health>().OrNull()?
                    .Damage(_damage);
                GameObject hitParticles = Instantiate(Assets.Instance.HitParticles, transform.position, Quaternion.LookRotation(-transform.up));
                Destroy(hitParticles, 1f);
                Destroy(gameObject);
            } else {
                //TODO: Fix this to use an arc
                _progress = Mathf.Clamp01(Vector2.Distance(_linearPosition, _target) / Vector2.Distance(_initialPosition, _target));
                _linearPosition = Vector3.MoveTowards(_linearPosition, _target, Time.fixedDeltaTime * _speed);
                _rb2D.MovePosition(_linearPosition + _height * Mathf.Sin(_progress * Mathf.PI) * Vector3.up);
            }
        }
    }

}
```