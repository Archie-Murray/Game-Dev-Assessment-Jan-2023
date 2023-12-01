﻿using System.Collections;
using System.Linq;

using UnityEngine;

using Utilities;

namespace BossAttack {
    public class BeamController : MonoBehaviour {
        [SerializeField] private float _damage;
        [SerializeField] private CountDownTimer _damageTimer;
        [SerializeField] private float _turnSpeed;
        [SerializeField] private float _totalDegrees;
        [SerializeField] private float _currentDegrees;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _offsetMagnitude;

        private void Start() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _offsetMagnitude = _spriteRenderer.bounds.extents.x * Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.z) + _spriteRenderer.bounds.extents.y * Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
            Init(10f, 0.5f, 60f, 360f);
        }

        private void FixedUpdate() {
            transform.rotation = Quaternion.AngleAxis(transform.rotation.eulerAngles.z + (_turnSpeed * Time.fixedDeltaTime), Vector3.forward);
            _currentDegrees += Time.fixedDeltaTime * _turnSpeed;
            _damageTimer?.Update(Time.fixedDeltaTime);
            if (_currentDegrees >= _totalDegrees) {
                _damageTimer.Stop();
                StartCoroutine(DestroySelf(0.5f));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="damageCooldown"></param>
        /// <param name="turnSpeed"></param>
        /// <param name="totalDegrees"></param>
        public void Init(float damage, float damageCooldown, float turnSpeed, float totalDegrees) {
            _damage = damage;
            _damageTimer = new CountDownTimer(damageCooldown);
            _totalDegrees = totalDegrees;
            _turnSpeed = turnSpeed;
            _currentDegrees = 0f;
            _damageTimer.Start();
            _damageTimer.OnTimerStop += Damage;
        }

        public void Damage() {
            Physics2D.OverlapBoxAll(
                transform.position + transform.up * _offsetMagnitude, _spriteRenderer.bounds.extents * 2f, transform.rotation.eulerAngles.z, Globals.Instance.PlayerLayer
            ).FirstOrDefault().OrNull()?.GetComponent<Health>().OrNull()?.Damage(_damage);
            _damageTimer.Reset();
            _damageTimer.Start();
        }

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