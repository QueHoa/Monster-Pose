using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace OneHit.Framework
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;
        public static int x;
        void Awake()
        {
            Instance = this;
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = 1;
                s.source.loop = s.loop;
            }
            x = Random.Range(1, 3);
        }

        private void Start()
        {
            
        }

        public List<Sound> sounds = new List<Sound>();
        public static void PlayBGM()
        {
            if (x == 1)
            {
                Sound s = Instance.sounds.Find(sound => sound.name == "BGM");
                if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
                {
                    s.source?.Play();
                }
            }else if (x == 2)
            {
                Sound s = Instance.sounds.Find(sound => sound.name == "BGM1");
                if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
                {
                    s.source?.Play();
                }
            }else if (x == 3)
            {
                Sound s = Instance.sounds.Find(sound => sound.name == "BGM2");
                if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
                {
                    s.source?.Play();
                }
            }
            
        }
        public static void UpdateMusic()
        {
            if (x == 1)
            {
                Sound s = Instance.sounds.Find(sound => sound.name == "BGM");
                if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
                {
                    s.source?.Stop();
                }
                else
                    s.source?.Play();
            }
            else if (x == 2)
            {
                Sound s = Instance.sounds.Find(sound => sound.name == "BGM1");
                if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
                {
                    s.source?.Stop();
                }
                else
                    s.source?.Play();
            }
            else if (x == 3)
            {
                Sound s = Instance.sounds.Find(sound => sound.name == "BGM2");
                if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
                {
                    s.source?.Stop();
                }
                else
                    s.source?.Play();
            }

            
        }
        public static void AdjustMusicVolume(float volume, float time = 0.5f)
        {
            Sound s = Instance.sounds.Find(sound => sound.name == "BGM");
            if (PlayerPrefs.GetInt("MusicOn", 1) == 1)
            {
                s.source.DOFade(volume, time);
            }
        }
        public static void ChangePitch(float pitch)
        {
            foreach (Sound s in Instance.sounds)
            {
                s.source.pitch = pitch;
            }
        }
        public static void PlayOneShot(string name)
        {
            Sound s = Instance.sounds.Find(sound => sound.name == name);

            if (PlayerPrefs.GetInt("SoundOn", 1) == 1)
            {
                s.source?.PlayOneShot(s.clip);
            }
        }
        public static void Play(string name)
        {
            Sound s = Instance.sounds.Find(sound => sound.name == name);
            if (PlayerPrefs.GetInt("SoundOn", 1) == 1)
            {
                s.source?.Play();
            }
        }

        public static void Stop(string name)
        {
            Sound s = Instance.sounds.Find(sound => sound.name == name); // Use Find method on the list
            if (PlayerPrefs.GetInt("SoundOn", 1) == 1)
            {
                s.source?.Stop();
            }
        }

        public void ClickSound()
        {
            Play("Click");
        }
        public AudioClip[] audioClips;
#if UNITY_EDITOR
        [Button("Add Sounds From List")]
        private void AddSoundsFromList()
        {
            sounds.Clear();
            foreach (AudioClip audioClip in audioClips)
            {
                Sound newSound = new Sound
                {
                    name = audioClip.name,
                    clip = audioClip,
                    volume = 1f, // Set the desired default volume for the new sound
                    loop = false // Set the desired default loop value for the new sound
                };
                if (newSound.name == "BGM")
                    newSound.loop = true;
                if (newSound.name == "BGM1")
                    newSound.loop = true;
                if (newSound.name == "BGM2")
                    newSound.loop = true;

                sounds.Add(newSound); // Use Add method to add new sound to the list
            }
            GenerateAudioNamesFile();
        }

        private void GenerateAudioNamesFile()
        {
            string filePath = "Assets/OneHit/Framework/AudioManager/AudioNames.cs";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("public static class AudioNames");
                writer.WriteLine("{");

                // Add your audio names here
                for (int i = 0; i < sounds.Count; i++)
                {
                    writer.WriteLine($"\tpublic static readonly string {sounds[i].name} = \"{sounds[i].name}\";");
                }

                writer.WriteLine("}");
            }

            UnityEditor.AssetDatabase.Refresh();
        }
#endif
    }
}