using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioBuilderSystem : MonoBehaviour
    {
        public int maxStageSpotsAudioCache = 10;

        [SerializeField] private AudioMixerGroup musicMixerGroup;

        private AudioManager _audioManager;
        private CustomAudioSource _customAudioSource;
        private List<AudioClip> _builtClips;

        public bool ReadyToPlay => AllTracksLoaded();

        private bool AllTracksLoaded()
        {
            return _builtClips.All(clip => clip == null || clip.loadState == AudioDataLoadState.Loaded);
        }

        private void Awake()
        {
            _audioManager = FindFirstObjectByType<AudioManager>();
            _builtClips = new List<AudioClip>(new AudioClip[maxStageSpotsAudioCache]);
            _customAudioSource = _audioManager.Setup(musicMixerGroup, false);
        }
        
        public void UpdateClipAtIndex(AudioClip clip, int index)
        {
            if (_builtClips[index] != null)
            {
                _builtClips[index].UnloadAudioData();
            }
            _builtClips[index] = null;
            clip?.LoadAudioData(); // Since clip can be null we use null prop
            _builtClips[index] = clip;
        }

        public void AddClipToBuilder(AudioClip clip)
        {
            clip.LoadAudioData();
            _builtClips.Add(clip);
        }

        public float PlayBuiltClips()
        {
            float longestClip = 0;
            foreach (var clip in _builtClips.Where(clip => clip is not null))
            {
                if (clip.length > longestClip) longestClip = clip.length;
                _customAudioSource.PlayOnce(clip, 1f);
            }
            
            return longestClip;
        }
    }
}