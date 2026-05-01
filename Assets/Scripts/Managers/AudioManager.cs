using System.Collections;
using Audio;
using UI.Settings;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

namespace Managers
{
    public sealed class AudioManager : EverlastingSingleton<AudioManager>
    {
        [SerializeField] private GameObject audioSourceObject;

        [Header("Mixers")] [SerializeField] private AudioMixer masterMixer;

        public const string MusicKey = "MusicVolume";
        public const string SfxKey = "SfxVolume";
        public const string AmbientKey = "AmbientVolume";

        public CustomAudioSource Play(AudioClip clip, AudioMixerGroup mixerGroup, bool looping = true, float volume = 1, CustomAudioSource presetAudioSource = null)
        {
            CustomAudioSource audioSource = presetAudioSource ? presetAudioSource : Setup(mixerGroup);
            if (audioSource == null) return null;

            if (looping)
            {
                audioSource.PlayLooping(clip, volume);
            }
            else
            {
                audioSource.PlayOnce(clip, volume);
            }

            return audioSource;
        }

        public CustomAudioSource Setup(AudioMixerGroup mixerGroup)
        {
            if (audioSourceObject is null)
            {
                Debug.LogError("AudioManager: audioSourceObject prefab is not assigned.");
                return null;
            }

            GameObject gO = Instantiate(audioSourceObject);
            // Use the existing component from the prefab if present; only add if absent.
            CustomAudioSource audioSource = gO.GetComponent<CustomAudioSource>() ?? gO.AddComponent<CustomAudioSource>();
            audioSource.Init(mixerGroup);
            return audioSource;
        }

        protected override void Awake()
        {
            base.Awake();
            StartCoroutine(LoadVolumes());
        }

        private IEnumerator LoadVolumes()
        {
            if (masterMixer == null)
            {
                Debug.LogError("AudioManager: masterMixer is not assigned — volumes will not be set.");
                yield break;
            }

            float musicVol   = PlayerPrefs.GetFloat(MusicKey,   0.5f);
            float sfxVol     = PlayerPrefs.GetFloat(SfxKey,     0.5f);
            float ambientVol = PlayerPrefs.GetFloat(AmbientKey, 0.5f);

            // Wait one frame — AudioMixer parameters cannot be set reliably on the same frame as scene load.
            yield return null;

            // Clamp to a small positive value so Log10 never produces -Infinity (which silences the mixer).
            masterMixer.SetFloat(VolumeSettings.MixerMusic,   Mathf.Log10(Mathf.Max(musicVol,   0.0001f)) * 20);
            masterMixer.SetFloat(VolumeSettings.SfxMusic,     Mathf.Log10(Mathf.Max(sfxVol,     0.0001f)) * 20);
            masterMixer.SetFloat(VolumeSettings.AmbientMusic, Mathf.Log10(Mathf.Max(ambientVol, 0.0001f)) * 20);
        }
    }
}
