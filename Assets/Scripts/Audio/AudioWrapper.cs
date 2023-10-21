using System;
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

            // Prep the dictionary 
            foreach (SoundData sound in allSoundData)
            {
                _soundDict.Add(sound.name, sound);
            }

            _dictionaryInitialised = true;
        }

        public CustomAudioSource PlaySound(string soundName)
        {
            CustomAudioSource audioSource = null;
            if (_soundDict.TryGetValue(soundName, out SoundData sound))
            {
                audioSource = AudioManager.Instance.Play(sound.sound, sound.mixer, sound.loop);
            }
            else
            {
                Debug.Log($"Sound {soundName} does not exist in the AudioWrapper.");
            }

            return audioSource;
        }
        
        public void PlaySoundVoid(string soundName)
        {
            CustomAudioSource audioSource = null;
            if (_soundDict.TryGetValue(soundName, out SoundData sound))
            {
                audioSource = AudioManager.Instance.Play(sound.sound, sound.mixer, sound.loop, customAudioSource);
            }
            else
            {
                Debug.Log($"Sound {soundName} does not exist in the AudioWrapper.");
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
    }
}