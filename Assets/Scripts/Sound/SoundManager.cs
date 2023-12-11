using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

[Serializable] public class SoundManager {

    public AudioMixer MainMixer;
    public AudioMixerGroup BGM;
    public AudioMixerGroup SFX;

    [SerializeField] private readonly Dictionary<SoundEffectType, AudioClip> _clips = new Dictionary<SoundEffectType, AudioClip>();
    public SoundManager(SoundEffect[] soundEffects, AudioMixer audioMixer, AudioMixerGroup sfx, AudioMixerGroup bgm) {
        Debug.Log("inited dict");
        MainMixer = audioMixer;
        SFX = sfx;
        BGM = bgm;
        foreach (SoundEffect soundEffect in soundEffects) {
            Debug.Log($"Adding sound effect type: {soundEffect.Type}, clip: {soundEffect.Name}");
            _clips.Add(soundEffect.Type, soundEffect.Clip);
        }
    }

    public AudioClip GetClip(SoundEffectType type) {
        if (!_clips.ContainsKey(type)) {
            Debug.Log("Could not load clip " + type);
            return null;
        } else { 
            return _clips[type]; 
        }
    }
}