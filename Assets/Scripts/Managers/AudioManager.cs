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
        
        public CustomAudioSource Play(AudioClip clip, AudioMixerGroup mixerGroup, bool looping = true, CustomAudioSource presetAudioSource = null)
        {
            CustomAudioSource audioSource = presetAudioSource ? presetAudioSource : Setup(mixerGroup, looping);

            if (looping)
            {
                audioSource.PlayLooping(clip);
            }
            else
            {
                audioSource.PlayOnce(clip);
            }

            return audioSource;
        }

        public CustomAudioSource Setup(AudioMixerGroup mixerGroup, bool looping = true)
        {
            if (audioSourceObject is null)
            {
                Debug.LogError("No custom object for audio");
                return null;
            }

            GameObject gO = Instantiate(audioSourceObject);
            CustomAudioSource audioSource = gO.AddComponent<CustomAudioSource>();
            audioSource.Init(mixerGroup);
            return audioSource;
        }

        protected override void Awake()
        {
            base.Awake();
            LoadVolumes();
        }

        private void LoadVolumes() // Volume is saved in VolumeSettings.cs
        {
            float musicVol = PlayerPrefs.GetFloat(MusicKey, 0.5f);
            float sfxVol = PlayerPrefs.GetFloat(SfxKey, 0.5f);
            float ambientVol = PlayerPrefs.GetFloat(AmbientKey, 0.5f);

            masterMixer.SetFloat(VolumeSettings.MixerMusic, Mathf.Log10(musicVol) * 20);
            masterMixer.SetFloat(VolumeSettings.SfxMusic, Mathf.Log10(sfxVol) * 20);
            masterMixer.SetFloat(VolumeSettings.AmbientMusic, Mathf.Log10(ambientVol) * 20);
        }
    }
}