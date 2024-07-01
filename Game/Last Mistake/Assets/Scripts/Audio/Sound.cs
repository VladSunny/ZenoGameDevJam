using UnityEngine.Audio;
using UnityEngine;

namespace Scripts.Audio
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)] public float volume = 1f;
        [Range(.1f, 3f)] public float pitch = 1f;
        public bool loop = false;
        public bool playOnAwake = false;

        [HideInInspector] public AudioSource source;
    }
}
