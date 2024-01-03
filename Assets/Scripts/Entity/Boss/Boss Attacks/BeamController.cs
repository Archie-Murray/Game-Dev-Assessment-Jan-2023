using System;
using System.Collections;
using System.Linq;

using UnityEngine;

using Utilities;

namespace Boss {
    public class BeamController : MonoBehaviour {
        [SerializeField] private float _damage;
        [SerializeField] private CountDownTimer _damageTimer;
        [SerializeField] private CountDownTimer _durationTimer;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _offsetMagnitude;
        [SerializeField] private Vector2 _size;

        private Vector3 RotatedPos => (Vector3) Helpers.FromRadians(-Mathf.Deg2Rad * transform.rotation.eulerAngles.z) * _offsetMagnitude + transform.position;

        private void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _size = new Vector2(_spriteRenderer.sprite.bounds.size.x * transform.localScale.x, _spriteRenderer.sprite.bounds.size.y * transform.localScale.y);
            _offsetMagnitude = _size.magnitude / 2f;
            GetComponent<Health>().OnDamage += (float amount) => GameManager.Instance.ResetCombatTimer();
        }

        private void FixedUpdate() {
            transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + (_turnSpeed * Time.fixedDeltaTime), Vector3.forward);
            _durationTimer.Update(Time.fixedDeltaTime);
            _damageTimer?.Update(Time.fixedDeltaTime);
            if (_durationTimer.IsFinished) {
                _damageTimer.Stop();
                StartCoroutine(DestroySelf(0.5f));
            }
        }

        /// <summary>
        /// Initialises beam with params, used like a constructor as this is a MonoBehaviour
        /// </summary>
        /// <param name="damage">Damage to do per collision check</param>
        /// <param name="damageCooldown">Interval between collision checks</param>
        /// <param name="turnSpeed">Speed to turn at, negative values will make it turn the other way</param>
        /// <param name="totalDegrees">Total unsigned degrees to turn</param>
        /// <param name="duration">Duration of beam</param>
        public void Init(float damage, float damageCooldown, float turnSpeed, float totalDegrees, float duration) {
            _damage = damage;
            _durationTimer = new CountDownTimer(duration);
            _damageTimer = new CountDownTimer(damageCooldown);
            _turnSpeed = Mathf.Max(0.1f, totalDegrees / duration) * Mathf.Sign(turnSpeed);
            _damageTimer.Start();
            _durationTimer.Start();
            _damageTimer.OnTimerStop += Damage;
        }

        private void Damage() {
            Health entityHealth = Physics2D.OverlapBoxAll(
                RotatedPos,
                _size,
                transform.rotation.eulerAngles.z,
                Globals.Instance.PlayerLayer
            ).Where((Collider2D collision) => collision.gameObject.HasComponent<PlayerController>())?
             .FirstOrDefault().OrNull()?
             .GetComponent<Health>().OrNull();

            if (entityHealth != null) {
                entityHealth.Damage(_damage);
                Instantiate(Assets.Instance.HitParticles, entityHealth.transform.position, Quaternion.LookRotation(-transform.up));
            }
            _damageTimer.Reset();
            _damageTimer.Start();
        }

        //private void OnDrawGizmos() {
        //    if (!Application.isPlaying) return;
        //    Gizmos.DrawSphere(RotatedPos, 1f);
        //    Gizmos.matrix = Matrix4x4.Translate(transform.position) * Matrix4x4.Rotate(Quaternion.AngleAxis(transform.eulerAngles.z, Vector3.forward));
        //    Gizmos.DrawCube(Vector3.up * _offsetMagnitude, _size);
        //    Gizmos.matrix = Matrix4x4.identity;
        //}

        private IEnumerator DestroySelf(float time) {
            float destoryTime = 1f / time;
            while (time > 0f) {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.zero, Time.fixedDeltaTime * destoryTime);
                time -= Time.fixedDeltaTime;
                yield return Yielders.WaitForFixedUpdate;
            }
            Destroy(gameObject);
        }
    }
}