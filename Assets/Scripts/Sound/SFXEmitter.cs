using System;
using System.Collections;

using UnityEngine;

using Utilities;

public class SFXEmitter : MonoBehaviour {
    [SerializeField] private AudioSource _audioSource;

    const float PITCH_BEND_AMOUNT = 10f;

    private void Start() {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.outputAudioMixerGroup = Globals.Instance.SoundManager.SFX;
    }

    public void Play(SoundEffectType soundEffect) {
        if (_audioSource.clip == null) {
            _audioSource.clip = Globals.Instance.SoundManager.GetClip(soundEffect);
            _audioSource.Play();
            StartCoroutine(ResetClip());
        }
    }

    public void Play(SoundEffectType soundEffect, float pitchRandomisation) {
        if (_audioSource.clip == null) {
            _audioSource.clip = Globals.Instance.SoundManager.GetClip(soundEffect);
            _audioSource.pitch += pitchRandomisation / PITCH_BEND_AMOUNT;
            _audioSource.Play();
            StartCoroutine(ResetClip());
        }
    }

    private IEnumerator ResetClip() {
        while (_audioSource.isPlaying) {
            yield return Yielders.WaitForSeconds(0.1f);
        }
        _audioSource.clip = null;
        _audioSource.pitch = 1;
    }
}