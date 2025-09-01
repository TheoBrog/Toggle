using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] AudioSource sfxObject;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Play(AudioClip audioClip, float volume = 1, float pitch = 1, bool loop = false)
    {
        PrivatePlayClip(audioClip, volume, pitch, loop);
    }

    public void PlayRandom(AudioClip[] audioClip, float volume = 1, float pitch = 1, bool loop = false)
    {
        PrivatePlayClip(audioClip[Random.Range(0, audioClip.Length)], volume, pitch, loop);
    }

    void PrivatePlayClip(AudioClip _audioClip, float _volume = 1, float _pitch = 1, bool _loop = false)
    {
        AudioSource source = Instantiate(sfxObject, Vector3.zero, Quaternion.identity);

        source.clip = _audioClip;
        source.volume = _volume;
        source.pitch = _pitch;
        source.loop = _loop;
        source.Play();

        float clipLength = 0;
        if (source != null)
            clipLength = source.clip.length;
        Destroy(source.gameObject, clipLength);
    }
}
