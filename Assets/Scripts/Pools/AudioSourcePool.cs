using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
    private static AudioSourcePool instance;

    public static AudioSourcePool Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private GameObject audioSourceObject;
    [SerializeField]
    private int size;
    private List<GameObject> audioSourceList;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            PrepareAudioSources();
        } else {
            Destroy(gameObject);
        }
    }

    private void PrepareAudioSources()
    {
        audioSourceList = new List<GameObject>();
        for (int i = 0; i < size; i++)
            AddAudioSource();
    }

    public GameObject GetAudioSource()
    {
        if (audioSourceList.Count == 0)
            AddAudioSource();
        return AllocateAudioSource();
    }

    public void ReleaseAudioSOurce(GameObject coin)
    {
        coin.gameObject.SetActive(false);
        audioSourceList.Add(coin);
    }

    private void AddAudioSource()
    {
        GameObject instance = Instantiate(audioSourceObject);
		instance.transform.position=this.transform.position;
        instance.gameObject.SetActive(false);
        audioSourceList.Add(instance);
    }

    private GameObject AllocateAudioSource()
    {
        GameObject audioSource = audioSourceList[audioSourceList.Count - 1];
        audioSourceList.RemoveAt(audioSourceList.Count - 1);
        audioSource.gameObject.SetActive(true);
        return audioSource;
    }
}
