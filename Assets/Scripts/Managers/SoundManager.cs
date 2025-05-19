using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    // para que se muestre en el editor
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
    }

    public List<Sound> sounds;
    public AudioSource sfxSource;
    public AudioSource ambientSource;

    private Dictionary<string, AudioClip> soundDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        soundDictionary = new Dictionary<string, AudioClip>();
        foreach (Sound s in sounds)
        {
            if (!soundDictionary.ContainsKey(s.name))
                soundDictionary[s.name] = s.clip;
        }
    }

    public void Play(string soundName, float volume = .5f)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }

    public void PlayAmbient(string soundName, float volume = .5f)
    {
        if (soundDictionary.TryGetValue(soundName, out AudioClip clip))
        {
            ambientSource.PlayOneShot(clip, volume);
        }
    }
}
