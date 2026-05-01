using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Utils;

namespace Audio
{
    public class AudioWrapper : EverlastingSingleton<AudioWrapper>
    {
        [SerializeField] private List<SoundData> allSoundData;
        [SerializeField] private CustomAudioSource customAudioSource;
        private Dictionary<string, SoundData> _soundDict = new();

        private bool _dictionaryInitialised;

        protected override void Awake()
        {
            base.Awake();
            if (_dictionaryInitialised) return;

            foreach (SoundData sound in allSoundData)
            {
                if (!_soundDict.TryAdd(sound.name, sound))
                {
                    Debug.LogWarning($"AudioWrapper: Duplicate sound name '{sound.name}' — skipping entry.");
                }
            }

            _dictionaryInitialised = true;
        }

        public CustomAudioSource PlaySound(string soundName)
        {
            if (_soundDict.TryGetValue(soundName, out SoundData sound))
            {
                return AudioManager.Instance.Play(sound.sound, sound.mixer, sound.loop, sound.volume);
            }

            Debug.LogWarning($"AudioWrapper: Sound '{soundName}' does not exist.");
            return null;
        }

        public void PlaySoundVoid(string soundName)
        {
            if (customAudioSource == null)
            {
                Debug.LogError("AudioWrapper: customAudioSource is not assigned in the inspector.");
                return;
            }

            if (_soundDict.TryGetValue(soundName, out SoundData sound))
            {
                AudioManager.Instance.Play(sound.sound, sound.mixer, sound.loop, sound.volume, customAudioSource);
            }
            else
            {
                Debug.LogWarning($"AudioWrapper: Sound '{soundName}' does not exist.");
            }
        }

        public void PlaySound(string soundName, float delay)
        {
            if (delay > 0)
            {
                StartCoroutine(PlayDelayed(soundName, delay));
            }
            else
            {
                PlaySound(soundName);
            }
        }

        private IEnumerator PlayDelayed(string soundName, float delay)
        {
            yield return new WaitForSeconds(delay);
            PlaySound(soundName);
        }

        public void StopAllAudio()
        {
            CustomAudioSource[] allAudio = FindObjectsByType<CustomAudioSource>(FindObjectsSortMode.None);
            foreach (CustomAudioSource sound in allAudio)
            {
                sound.StopAudio();
            }
        }
    }
}
