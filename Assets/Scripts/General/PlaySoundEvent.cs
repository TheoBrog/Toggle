using UnityEngine;

public class PlaySoundEvent : MonoBehaviour
{
    public void Play(AudioClip clip)
    {
        SoundManager.instance.Play(clip);
    }

    public void PlayRandom(AudioClip clip){
        SoundManager.instance.Play(clip, 1, Random.Range(.5f, 1.5f));
    }
}
