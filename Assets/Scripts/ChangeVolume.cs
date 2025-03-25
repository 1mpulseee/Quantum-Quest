using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class ChangeVolume : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public SoundType Type = 0;
    [HideInInspector] public Slider GlobalSlider, MusicSlider, EffectsSlider;
    float GlobalVolumeValue, MusicVolumeValue, EffectVolumeValue;
    [SerializeField][Range(-80f, 0f)] float MinVolume = -40f;
    private IEnumerator Start()
    {
        yield return new WaitForEndOfFrame();
        LoadVolume();
    }
    public void LoadVolume()
    {
        if (Type.HasFlag(SoundType.Global))
        {
            if (GlobalSlider == null)
            {
                Debug.LogError("Global Slider is not found");
            }
            else
            {
                GlobalVolumeValue = PlayerPrefs.GetFloat("GlobalVolumeValue", MinVolume / 3f);
                GlobalSlider.minValue = MinVolume;
                GlobalSlider.maxValue = 0f;
                GlobalSlider.onValueChanged.AddListener(ChangeGlobalVolume);
                GlobalSlider.value = GlobalVolumeValue;
                audioMixer.SetFloat(SoundType.Global.ToString(), GlobalSlider.value);
            }
        }
        if (Type.HasFlag(SoundType.Music))
        {
            if (MusicSlider == null)
            {
                Debug.LogError("Music Slider is not found");
            }
            else
            {
                MusicVolumeValue = PlayerPrefs.GetFloat("MusicVolumeValue", MinVolume / 3f);
                MusicSlider.minValue = MinVolume;
                MusicSlider.maxValue = 0f;
                MusicSlider.onValueChanged.AddListener(ChangeMusicVolume);
                MusicSlider.value = MusicVolumeValue;
                audioMixer.SetFloat(SoundType.Music.ToString(), MusicSlider.value);
            }
        }
        if (Type.HasFlag(SoundType.Effects))
        {
            if (EffectsSlider == null)
            {
                Debug.LogError("Effects Slider is not found");
            }
            else
            {
                EffectVolumeValue = PlayerPrefs.GetFloat("EffectVolumeValue", MinVolume / 3f);
                EffectsSlider.minValue = MinVolume;
                EffectsSlider.maxValue = 0f;
                EffectsSlider.onValueChanged.AddListener(ChangeEffectsVolume);
                EffectsSlider.value = EffectVolumeValue;
                audioMixer.SetFloat(SoundType.Effects.ToString(), EffectsSlider.value);
            }
        }
    }
    void ChangeGlobalVolume(float Volume)
    {
        GlobalVolumeValue = Volume;
        audioMixer.SetFloat(SoundType.Global.ToString(), GlobalVolumeValue);
        PlayerPrefs.SetFloat("GlobalVolumeValue", GlobalVolumeValue);
    }
    void ChangeMusicVolume(float Volume)
    {
        MusicVolumeValue = Volume;
        audioMixer.SetFloat(SoundType.Music.ToString(), MusicVolumeValue);
        PlayerPrefs.SetFloat("MusicVolumeValue", MusicVolumeValue);
    }
    void ChangeEffectsVolume(float Volume)
    {
        EffectVolumeValue = Volume;
        audioMixer.SetFloat(SoundType.Effects.ToString(), EffectVolumeValue);
        PlayerPrefs.SetFloat("EffectVolumeValue", EffectVolumeValue);
    }
}
[Flags]
public enum SoundType { Global = 1 << 0, Music = 1 << 1, Effects = 1 << 2 }