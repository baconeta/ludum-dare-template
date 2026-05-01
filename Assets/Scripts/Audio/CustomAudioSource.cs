using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class CustomAudioSource : MonoBehaviour
    {
        private AudioSource _self;
        private Coroutine _resetCoroutine;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void ResetData()
        {
            if (_self != null)
            {
                _self.Stop();
            }
            _resetCoroutine = null;
            Destroy(gameObject);
        }

        public void Init(AudioMixerGroup group)
        {
            if (!_self) _self = GetComponent<AudioSource>();
            _self.outputAudioMixerGroup = group;
        }

        public void PlayOnce(AudioClip clip, float volume)
        {
            if (!_self) _self = GetComponent<AudioSource>();

            // Cancel any pending destroy so this clip gets its full duration.
            if (_resetCoroutine != null)
            {
                StopCoroutine(_resetCoroutine);
            }

            _self.PlayOneShot(clip, volume);
            _resetCoroutine = StartCoroutine(ResetObject(clip.length + 0.5f));
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
            if (_resetCoroutine != null)
            {
                StopCoroutine(_resetCoroutine);
            }
            _resetCoroutine = StartCoroutine(ResetObject(0f));
        }

        private IEnumerator ResetObject(float delay)
        {
            yield return new WaitForSeconds(delay);
            ResetData();
        }
    }
}
