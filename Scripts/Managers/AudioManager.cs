using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _audioSource;
    private List<AudioClip> _clips;

    // Start is called before the first frame update
    void Start()
    {
        _clips = new List<AudioClip>();
        foreach (var clip in Resources.LoadAll<AudioClip>("Audio"))
        {
            clip.LoadAudioData();
            _clips.Add(clip);
        }
        _audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Loads and plays a sound using a sound prefix
    /// Perfect for playing a variety of sound effects
    /// </summary>
    /// <param name="soundNamePrefix"></param>
    public void PlaySound(string soundNamePrefix, bool pitchRandomizer = true)
    {
        try
        {
            var soundFiles = _clips.Where(w => w.name.StartsWith(soundNamePrefix)).ToList();
            var randomSound = soundFiles.ElementAt(Random.Range(0, soundFiles.Count()));

            if (pitchRandomizer) _audioSource.pitch = Random.Range(.85f, 1.10f);
            else _audioSource.pitch = 1;


            if (randomSound.LoadAudioData())
            {
                if (randomSound.loadState == AudioDataLoadState.Loaded)
                    _audioSource.PlayOneShot(randomSound);
            }

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

}
