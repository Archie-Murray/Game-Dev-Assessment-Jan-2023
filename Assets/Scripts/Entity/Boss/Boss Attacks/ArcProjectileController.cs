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