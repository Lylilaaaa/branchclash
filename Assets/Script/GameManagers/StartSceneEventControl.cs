using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartSceneEventControl : MonoBehaviour
{
    public VideoClip[] videoList;
    public VideoPlayer vp;
    public RawImage ri;
    public Transform roleChoosingPanel;
    public Button skipButton;
    // Start is called before the first frame update
    void Start()
    {
        vp.loopPointReached += EndReached;
        vp.prepareCompleted += Prepare;
        skipButton.onClick.AddListener(SkipToLastSecond);
        vp.gameObject.SetActive(false);
        roleChoosingPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void ConnectToWallet()
    {
        vp.gameObject.SetActive(true);
        vp.clip = videoList[0];
        vp.Prepare();
    }

    public void ChoseRole(int roleIndex)
    {
        roleChoosingPanel.gameObject.SetActive(false);
        vp.gameObject.SetActive(true);
        vp.clip = videoList[roleIndex+1];
        vp.Prepare();
    }

    void EndReached(VideoPlayer vPlayer)
    {
        roleChoosingPanel.gameObject.SetActive(true);
        vp.gameObject.SetActive(false);
    }
    void Prepare(VideoPlayer vPlayer)
    {
        vp.Play();
    }
    public void SkipToLastSecond()
    {
        double lastSecond = vp.clip.length - 1.0;
        vp.time = lastSecond;
        
        if (!vp.isPlaying)
        {
            vp.Play();
        }
    }
}
