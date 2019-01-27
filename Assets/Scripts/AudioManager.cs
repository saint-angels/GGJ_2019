using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonComponent<AudioManager>
{
    [SerializeField] private float pitchStep = 1f;
    [SerializeField] private AudioClip lockedClip;
    [SerializeField] private AudioClip shotClip;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayClip(bool locked, int enemyIndex)
    {
        var clip = locked ? lockedClip : shotClip;
        source.pitch = 1f + .05f * enemyIndex;
        source.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
