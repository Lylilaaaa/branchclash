
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEditor;


public class StartSceneEventControl : MonoBehaviour
{
    public VideoClip[] videoList;
    public VideoPlayer vp;
    public RawImage ri;
    public Transform roleChoosingPanel;
    public Button skipButton;
    public Button connectButton;
    public string userAddress = "0xfd376a919b9a1280518e9a5e29e3c3637c9faa12";
    public bool isOldPlayer;
    public GameObject loadingGameObj;
    
    private bool _finishMainVideo;

    private int _role;
    // Start is called before the first frame update
    void Awake()
    {
        vp.loopPointReached += EndReached;
        vp.prepareCompleted += Prepare;
        skipButton.onClick.AddListener(SkipToLastSecond);
        connectButton.onClick.AddListener(StartConnectWallet);
        vp.gameObject.SetActive(false);
        roleChoosingPanel.gameObject.SetActive(false);
        _finishMainVideo = false;
        isOldPlayer = false;
        loadingGameObj.SetActive(false);
    }

    public void StartConnectWallet()
    {
        StartCoroutine(ConnectToWallet());
    }

    public IEnumerator ConnectToWallet()
    {
        while (GlobalVar._instance.thisUserAddr == "")
        {
            yield return null;
        };
        ContractInteraction._instance.CheckDuty();
        StartCoroutine(_checkDuty());
    }

    public IEnumerator _checkDuty()
    {
        while (ContractInteraction._instance.role==100)
        {
            yield return null;
        }

        _role = ContractInteraction._instance.role+2;  //0（2）：新手；1（3）：士兵；2（4）：研究者
        if (_role == 2)
        {
            isOldPlayer = false;
            skipButton.gameObject.SetActive(false);
        }
        else
        {
            _role -= 3;
            GlobalVar._instance.role = _role;
            isOldPlayer = true;
            skipButton.gameObject.SetActive(true);
        }
        vp.gameObject.SetActive(true);
        //vp.clip = videoList[0];
        vp.url = Path.Combine(Application.streamingAssetsPath, "main.mp4");
        vp.Play();
        vp.Prepare();
    }
    
    public IEnumerator ChoseRole(int roleIndex)
    {
        ContractInteraction._instance.ChooseDuty((roleIndex+1).ToString());
        while (!ContractInteraction._instance.finishChoseDuty)
        {
            yield return null;
        }
        
        roleChoosingPanel.gameObject.SetActive(false);
        vp.gameObject.SetActive(true);
        //vp.clip = videoList[roleIndex+1];
        if (roleIndex == 0)
        {
            vp.url = Path.Combine(Application.streamingAssetsPath, "0.mp4");
        }
        else
        {
            vp.url = Path.Combine(Application.streamingAssetsPath, "1.mp4");
        }
        vp.Prepare();
        _role = roleIndex;
        GlobalVar._instance.role = _role;
        //UserInformation._instance.userRole = roleIndex;
    }
    
    void EndReached(VideoPlayer vPlayer)
    {
        if (_finishMainVideo == false && isOldPlayer == false) //???????????panel????????panel
        {
            roleChoosingPanel.gameObject.SetActive(true);
            vp.gameObject.SetActive(false);
            _finishMainVideo = true;
        }
        else if(_finishMainVideo == false && isOldPlayer) //????????????????????
        {
            vp.clip = videoList[_role+1];
            vp.Prepare();
            _finishMainVideo = true;
        }
        else if (_finishMainVideo == true)
        {
            //?л??????
            if (_role == 0)
            {
                GlobalVar._instance._loadNextScene("1_0_HomePage");
            }
            else if (_role == 1)
            {
                GlobalVar._instance._loadNextScene("1_1_SecHomePage");
            }
        }

    }

    // void _loadNextScene(string sceneName)
    // {
    //     loadingGameObj.SetActive(true);
    //     StartCoroutine(LoadLeaver(sceneName));
    // }
    //
    // IEnumerator LoadLeaver(string sceneName)
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
    //     while (!operation.isDone)
    //     {
    //         loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = operation.progress;
    //         yield return null;
    //     }
    //     loadingGameObj.SetActive(false);
    //     GlobalVar._instance.ReStart();
    // }
    
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
