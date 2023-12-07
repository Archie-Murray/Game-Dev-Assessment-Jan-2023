using System;
using System.Collections;

using UnityEngine;

using Utilities;

public class SFXEmitter : MonoBehaviour {
    [SerializeField] private AudioSource _audioSource;

    private void Start() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void Play(SoundEffectType soundEffect) {
        if (_audioSource.clip == null) {
            _audioSource.clip = Globals.Instance.SoundManager.GetClip(soundEffect);
            _audioSource.Play();
            StartCoroutine(ResetClip());
        }
    }

    private IEnumerator ResetClip() {
        while (_audioSource.isPlaying) {
            yield return Yielders.WaitForSeconds(0.1f);
        }
        _audioSource.clip = null;
    }
}