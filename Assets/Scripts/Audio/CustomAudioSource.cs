using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class CustomAudioSource : MonoBehaviour
    {
        private AudioSource _self;

        private void ResetData()
        {
            // Stop Audio
            _self.Stop();
        }

        public void Init(AudioMixerGroup group)
        {
            if (!_self) _self = GetComponent<AudioSource>();

            _self.outputAudioMixerGroup = group;
        }

        public void PlayOnce(AudioClip clip)
        {
            if (!_self) _self = GetComponent<AudioSource>();
            _self.PlayOneShot(clip);
            StartCoroutine(ResetObject(clip.length + 0.5f));
        }

        public void PlayLooping(AudioClip clip)
        {
            if (!_self) _self = GetComponent<AudioSource>();
            _self.clip = clip;
            _self.loop = true;
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