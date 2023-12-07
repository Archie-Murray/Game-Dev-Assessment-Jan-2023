using System;
using System.Collections.Generic;

using UnityEngine;
[Serializable] public enum SoundEffectType { NONE, HIT, SHOOT, HEAVY_SHOOT, ELITE_SHOOT, DESTROY }

[Serializable] public class SoundEffect {
    public AudioClip Clip;
    public SoundEffectType Type;
    public string Name;
}