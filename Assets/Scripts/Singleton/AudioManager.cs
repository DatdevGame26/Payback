using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();


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
        }

    }

    void Start()
    {
        LoadAudioClips();
    }

    private void LoadAudioClips()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (var clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }
    }

    public void PlaySound(string clipName, GameObject emitEntity, bool randomPitch = true)
    {
        AudioSource audioSource = emitEntity.GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = emitEntity.AddComponent<AudioSource>();
        }

        if (audioClips.ContainsKey(clipName))
        {
            audioSource.clip = audioClips[clipName];
            if(randomPitch) audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Audio clip not found: " + clipName);
        }
    }

    public void createSFXgameObject(string soundName, Vector3 spawnPos, float maxDistance = 50)
    {
        if (!audioClips.ContainsKey(soundName))
        {
            Debug.LogWarning("Audio clip not found: " + soundName);
            return;
        }

        GameObject sfxObject = new GameObject("SFX_" + soundName);
        sfxObject.transform.position = spawnPos;

        AudioSource audioSource = sfxObject.AddComponent<AudioSource>();
        audioSource.clip = audioClips[soundName];
        audioSource.spatialBlend = 1;
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        audioSource.minDistance = 5;
        audioSource.maxDistance = maxDistance;
        audioSource.pitch = Random.Range(0.9f, 1.1f);

        audioSource.Play();

        Destroy(sfxObject, audioSource.clip.length);
    }
}
