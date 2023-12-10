using System;

using UnityEngine;

[Serializable]
public class Settings {
    private const string GLOBAL_VOLUME_PARMETER_NAME = "Global Volume";
    private const string SFX_VOLUME_PARMETER_NAME  = "SFX Volume";
    private const string BGM_VOLUME_PARMETER_NAME = "BGM Volume";

    public float GlobalVolume;
    public float SFXVolume;
    public float BGMVolume;
    public bool TutorialMode;

    public Settings(float globalVolume, float sFXVolume, float bGMVolume, bool tutorialMode) {
        GlobalVolume = globalVolume;
        SFXVolume = sFXVolume;
        BGMVolume = bGMVolume;
        TutorialMode = tutorialMode;
    }

    public static readonly Settings Defaults = new Settings(1f, 1f, 1f, true);

    public static void ApplySettings(Settings settings) {
        Debug.Log($"Main Mixer null: {Globals.Instance.SoundManager.MainMixer == null}");
        Globals.Instance.SoundManager.MainMixer.SetFloat(GLOBAL_VOLUME_PARMETER_NAME, settings.GlobalVolume);
        Globals.Instance.SoundManager.MainMixer.SetFloat(SFX_VOLUME_PARMETER_NAME, settings.SFXVolume);
        Globals.Instance.SoundManager.MainMixer.SetFloat(BGM_VOLUME_PARMETER_NAME, settings.BGMVolume);
        Globals.Instance.TutorialMode = settings.TutorialMode;
    }
}