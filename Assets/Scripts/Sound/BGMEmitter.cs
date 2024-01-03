using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Utilities;

public class BGMEmitter : MonoBehaviour {
    [SerializeField] private BGM[] _bgmList;

    [SerializeField] private BGMType _currentlyPlaying = BGMType.NONE;
    [SerializeField] private BGMType _target = BGMType.NONE;
    [SerializeField] private Dictionary<BGMType, AudioSource> _audioSources;

    private Coroutine _mix;

    private void Awake() {
        foreach (BGM bgm in _bgmList) { 
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.clip = bgm.Clip;
            _audioSources.Add(bgm.Type, audioSource);
        }
    }

    private void Start() {
        if (_bgmList.Length == 1) {
            PlayBGM(_bgmList.First().Type);
        }
    }

    public void PlayBGM(BGMType type, float fadeDuration = 1f) {
        if ((type == _target || type == _currentlyPlaying) && type != BGMType.NONE) {
            return;
        }
        _target = type;
        if (_mix == null) {
            StopCoroutine(_mix);
        } 
        _mix = StartCoroutine(MixBgm(fadeDuration));
    }

    private IEnumerator MixBgm(float duration) {
        AudioSource current = _audioSources[_currentlyPlaying];
        AudioSource target = _audioSources[_target];
        CountDownTimer timer = new CountDownTimer(duration);
        target.volume = 0f;
        target.Play();
        timer.Start();
        while (timer.IsRunning) {
            timer.Update(Time.fixedDeltaTime);
            yield return Yielders.WaitForFixedUpdate;
            current.volume = 1f - timer.Progress;
            target.volume = timer.Progress;
        }
        current.Stop();
        current.volume = 1f;
    }
}

[Serializable] public enum BGMType { NONE, MAIN_MENU, PASSIVE, COMBAT }
[Serializable] public class BGM {
    public BGMType Type;
    public AudioClip Clip;
}