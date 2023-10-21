using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioBuilderSystem : MonoBehaviour
    {
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private AudioMixerGroup musicMixerGroup;

        private List<AudioClip> _builtClips;
        private CustomAudioSource _customAudioSource;

        private void Awake()
        {
            _builtClips = new List<AudioClip>();
            _customAudioSource = audioManager.Setup(musicMixerGroup, false);
        }

        public void AddClipToBuilder(AudioClip clip)
        {
            clip.LoadAudioData();
            _builtClips.Add(clip);
        }

        public float PlayBuiltClips()
        {
            float longestClip = 0;
            foreach (AudioClip clip in _builtClips)
            {
                if (clip.length > longestClip) longestClip = clip.length;
                _customAudioSource.PlayOnce(clip);
            }

            _builtClips.Clear();
            return longestClip;
        }
    }
}