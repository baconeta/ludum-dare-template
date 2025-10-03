using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class CustomAudioSource : MonoBehaviour
    {
        private AudioSource _self;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void ResetData()
        {
            // Stop Audio
            if (_self != null)
            {
                _self.Stop();
                Destroy(gameObject);
            }
        }

        public void Init(AudioMixerGroup group)
        {
            if (!_self) _self = GetComponent<AudioSource>();

            _self.outputAudioMixerGroup = group;
        }

        public void PlayOnce(AudioClip clip, float volume)
        {
            if (!_self) _self = GetComponent<AudioSource>();
            _self.PlayOneShot(clip,volume);
            StartCoroutine(ResetObject(clip.length + 0.5f));
        }

        public void PlayLooping(AudioClip clip, float volume)
        {
            if (!_self) _self = GetComponent<AudioSource>();
            _self.clip = clip;
            _self.loop = true;
            _self.volume = volume;
            _self.Play();
        }

        public void StopAudio()
        {
            StartCoroutine(ResetObject(0f));
        }

        private IEnumerator ResetObject(float delay)
        {
            yield return new WaitForSeconds(delay);

            // Now recycle the object
            ResetData();
        }
    }
}