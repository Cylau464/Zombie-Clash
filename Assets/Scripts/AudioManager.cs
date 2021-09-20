using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("AudioClips")]
    [SerializeField] private AudioClip _winClip = null;
    [SerializeField] private AudioClip _loseClip = null;
    [SerializeField] private AudioClip _fightClip = null;
    [SerializeField] private AudioClip _zombiePickupClip = null;
    [SerializeField] private AudioClip _upgradeSound = null;
    [SerializeField] private AudioClip _siegeClip = null;

    [Header("Mixer Group")]
    [SerializeField] private AudioMixerGroup ambientGroup = null;
    [SerializeField] private AudioMixerGroup musicGroup = null;
    [SerializeField] private AudioMixerGroup SFXGroup = null;

    private AudioSource ambientSource = null;
    private AudioSource musicSource = null;
    private AudioSource SFXSource = null;

    private static AudioManager current;

    private void Awake()
    {
        if (current != null && current != this)
        {
            //...destroy this. There can be only one AudioManager
            Destroy(gameObject);
            return;
        }

        current = this;
        DontDestroyOnLoad(gameObject);

        ambientSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        SFXSource = gameObject.AddComponent<AudioSource>();

        ambientSource.outputAudioMixerGroup = ambientGroup;
        musicSource.outputAudioMixerGroup = musicGroup;
        SFXSource.outputAudioMixerGroup = SFXGroup;
    }

    public static AudioSource PlayClipAtPosition(AudioClip clip, Vector3 position, float volume = 1f, float minDistance = 1f, float pitch = 1f, Transform parent = null)
    {
        GameObject go = new GameObject("One Shot Audio");
        go.transform.position = position;
        go.transform.parent = parent;
        AudioSource source = go.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.spatialBlend = 1f;
        source.minDistance = minDistance;
        source.pitch = pitch;
        source.Play();
        Destroy(go, source.clip.length);

        return source;

    }

    public static void PlayFightSound()
    {
        if (current == null)
            return;

        current.SFXSource.pitch = Random.Range(1.2f, 1.3f);
        current.SFXSource.clip = current._fightClip;
        current.SFXSource.volume = .3f;
        current.SFXSource.loop = false;
        current.SFXSource.Play();
    }

    public static void PlaySiegeSound()
    {
        if (current == null)
            return;

        current.SFXSource.pitch = Random.Range(.85f, 1f);
        current.SFXSource.clip = current._siegeClip;
        current.SFXSource.volume = 1f;
        current.SFXSource.loop = false;
        current.SFXSource.Play();
    }

    public static void PlayWinSound()
    {
        if (current == null)
            return;
        
        current.SFXSource.pitch = 1f;
        current.SFXSource.clip = current._winClip;
        current.SFXSource.volume = .3f;
        current.SFXSource.loop = false;
        current.SFXSource.Play();
    }

    public static void PlayLoseSound()
    {
        if (current == null)
            return;

        current.SFXSource.pitch = 1f;
        current.SFXSource.clip = current._loseClip;
        current.SFXSource.volume = .3f;
        current.SFXSource.loop = false;
        current.SFXSource.Play();
    }

    public static void PlayZombiePickupSound()
    {
        if (current == null)
            return;

        current.SFXSource.pitch = Random.Range(.85f, 1f);
        current.SFXSource.clip = current._zombiePickupClip;
        current.SFXSource.volume = .05f;
        current.SFXSource.loop = false;
        current.SFXSource.Play();
    }

    public static void PlayUpgradeSound()
    {
        if (current == null)
            return;

        current.SFXSource.pitch = 1f;
        current.SFXSource.clip = current._upgradeSound;
        current.SFXSource.volume = .2f;
        current.SFXSource.loop = false;
        current.SFXSource.Play();
    }

    private void OnLevelWasLoaded()
    {
        current.SFXSource.Stop();
    }
}
