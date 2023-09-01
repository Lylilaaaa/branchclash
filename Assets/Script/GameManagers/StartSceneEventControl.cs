
using System.Collections;
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
        vp.gameObject.SetActive(false);
        roleChoosingPanel.gameObject.SetActive(false);
        _finishMainVideo = false;
        isOldPlayer = false;
        loadingGameObj.SetActive(false);
    }

    public void ConnectToWallet()
    {
        vp.gameObject.SetActive(true);
        vp.clip = videoList[0];
        vp.Prepare();

        if (_checkUserData(userAddress) == null)
        {
            skipButton.gameObject.SetActive(false);
            isOldPlayer = false;
        }
        else
        {
            isOldPlayer = true;
            skipButton.gameObject.SetActive(true);
            UserInformation._instance.userRoleData = _checkUserData(userAddress);
            _role = UserInformation._instance.userRoleData.role;
        }

    }

    public void ChoseRole(int roleIndex)
    {
        roleChoosingPanel.gameObject.SetActive(false);
        vp.gameObject.SetActive(true);
        vp.clip = videoList[roleIndex+1];
        vp.Prepare();
        _createUserData(roleIndex);
        _role = roleIndex;
        //UserInformation._instance.userRole = roleIndex;
    }

    private UserData _checkUserData(string address)
    {
        UserData[] allMyDataObjects = Resources.LoadAll<UserData>("");
        foreach (UserData VARIABLE in allMyDataObjects)
        {
            if (VARIABLE.address == address)
            {
                return VARIABLE;
            }
        }
        Debug.Log("ScriptableObject not found, new user!");
        return null;
    }
    
    private void _createUserData(int roleIndex)
    {
        //需要先检查这个userData在不在已有的数据库内，如果有，直接跳过选角色
        
        //如果没有：
        UserData newUserData = new UserData();
        //newUserData.name = userAddress;
        newUserData.address = userAddress;
        newUserData.process = 1;
        newUserData.role = roleIndex;
        newUserData.isFirstPlay = true;
        string assetPath = "Assets/Resources/" + userAddress + ".asset";
        AssetDatabase.CreateAsset(newUserData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        UserInformation._instance.userRoleData = newUserData;
        
        isOldPlayer = true;
    }

    void EndReached(VideoPlayer vPlayer)
    {
        if (_finishMainVideo == false && isOldPlayer == false) //新玩家，打开选角panel，关闭视频panel
        {
            roleChoosingPanel.gameObject.SetActive(true);
            vp.gameObject.SetActive(false);
            _finishMainVideo = true;
        }
        else if(_finishMainVideo == false && isOldPlayer == true) //老玩家，直接播放下一个视频
        {
            vp.clip = videoList[_checkUserData(userAddress).role+1];
            vp.Prepare();
            _finishMainVideo = true;
        }
        else if (_finishMainVideo == true)
        {
            //切换关卡了
            if (_role == 0)
            {
                _loadNextScene("1_0_HomePage");
            }
            else if (_role == 1)
            {
                _loadNextScene("1_1_SecHomePage");
            }
        }

    }

    void _loadNextScene(string sceneName)
    {
        loadingGameObj.SetActive(true);
        StartCoroutine(LoadLeaver(sceneName));
    }

    IEnumerator LoadLeaver(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            loadingGameObj.transform.GetChild(1).GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
        loadingGameObj.SetActive(false);
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
