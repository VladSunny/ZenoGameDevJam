using UnityEngine.Audio;
using UnityEngine;
using System;
using DG.Tweening;

namespace Scripts.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private float _fadeInTime = 1.5f;
        [SerializeField] private Sound[] _sounds;

        private void Awake()
        {
            foreach (Sound s in _sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;

                if (s.playOnAwake)
                    s.source.Play();
            }
        }

        public void Play(string name)
        {
            Sound s = Array.Find(_sounds, sound => sound.name == name);
            s.source.volume = 0f;
            s.source.Play();
            DOTween.To(() => s.source.volume, x => s.source.volume = x, s.volume, _fadeInTime).SetTarget(s.source).SetUpdate(true);
        }

        public void Stop(string name)
        {
            Sound s = Array.Find(_sounds, sound => sound.name == name);
            DOTween.To(() => s.source.volume, x => s.source.volume = x, 0f, _fadeInTime).OnComplete(() => s.source.Stop()).SetTarget(s.source).SetUpdate(true);
        }
    }
}
