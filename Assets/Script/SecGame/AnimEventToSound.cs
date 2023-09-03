using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventToSound : MonoBehaviour
{
    public AudioClip tubeLoadingSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void tubeSoundPlay()
    {
        SoundManager._instance.PlayEffectSound(tubeLoadingSound);
    }
}
