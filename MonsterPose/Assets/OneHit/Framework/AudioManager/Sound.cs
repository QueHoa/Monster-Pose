using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace OneHit.Framework
{
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1;
        public float pitch = 1;

        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }
}