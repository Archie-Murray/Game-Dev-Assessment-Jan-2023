using System;
using System.Collections.Generic;

using UnityEngine;
[Serializable] public enum SoundEffectType { NONE, HIT, PLAYER_LIGHT, PLAYER_HEAVY, PLAYER_ELITE, DESTROY, BOSS_BEAM, BOSS_PROJECTILE }

[Serializable] public class SoundEffect {
    public AudioClip Clip;
    public SoundEffectType Type;
}