using System;
using System.Collections.Generic;

using UnityEngine;

public class SoundManager {
    private readonly Dictionary<SoundEffectType, AudioClip> _clips = new Dictionary<SoundEffectType, AudioClip>(); 
    public SoundManager(SoundEffect[] soundEffects) {
        foreach (SoundEffect soundEffect in  soundEffects) {
            Debug.Log($"Adding sound effect type: {soundEffect.Type}, clip: {soundEffect.Name}");
            _clips.Add(soundEffect.Type, soundEffect.Clip);
        }
    }

    public AudioClip GetClip(SoundEffectType type) {
        return _clips[type];
    }
}