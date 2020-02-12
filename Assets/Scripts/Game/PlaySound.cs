using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        this.audioSource = this.GetComponent<AudioSource>();
    }
    public void PlayAudio(string audioName, float volume = 0.1f)
    {
        this.audioSource.clip = Resources.Load<AudioClip>("Sounds/" + audioName);
        this.audioSource.volume=volume;
        this.audioSource.Play();
        StartCoroutine(ReturnToPool());
    }

    IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(this.audioSource.clip.length);
        AudioSourcePool.Instance.ReleaseAudioSOurce(this.gameObject);
    }

}
