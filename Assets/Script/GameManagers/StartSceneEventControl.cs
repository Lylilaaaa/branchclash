
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
    
    private bool hasChosenRole;

    private int _role;
    // Start is called before the first frame update
    void Awake()
    {
        vp.loopPointReached += EndReached;
        vp.prepareCompleted += Prepare;
        skipButton.onClick.AddListener(SkipToLastSecond);
        vp.gameObject.SetActive(false);
        roleChoosingPanel.gameObject.SetActive(false);
        hasChosenRole = false;
        isOldPlayer = false;
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

        if (_checkUserData(userAddress) == null)
        {
            //记得改过来！！！！
            //skipButton.gameObject.SetActive(false);
            isOldPlayer = false;
        }
        else
        {
            isOldPlayer = true;
            skipButton.gameObject.SetActive(true);
            hasChosenRole = true;
            UserInformation._instance.userRoleData = _checkUserData(userAddress);
        }

    }

    public void ChoseRole(int roleIndex)
    {
        roleChoosingPanel.gameObject.SetActive(false);
        vp.gameObject.SetActive(true);
        vp.clip = videoList[roleIndex+1];
        vp.Prepare();
        hasChosenRole = true;
        _createUserData(roleIndex);
        _role = roleIndex;
        //UserInformation._instance.userRole = roleIndex;
    }

    private UserData _checkUserData(string address)
    {
        string assetPath = "Assets/Resources/" + address + ".asset";
        Object loadedAsset = Resources.Load(assetPath);
        if (loadedAsset != null && loadedAsset is ScriptableObject) // 检查资源是否是ScriptableObject的实例
        {
            UserData loadedObject = loadedAsset as UserData;
            return loadedObject;
        }
        else
        {
            Debug.Log("ScriptableObject not found at: " + assetPath);
            return null;
        }
        
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
        if (isOldPlayer == false)
        {
            roleChoosingPanel.gameObject.SetActive(true);
            vp.gameObject.SetActive(false);
        }
        // else
        // {
        //     vp.clip = videoList[_checkUserData(userAddress).role+1];
        //     vp.Prepare();
        // }
        if (hasChosenRole == true)
        {//切换关卡了
            if (_role == 0)
            {
                SceneManager.LoadScene("HomePage");
            }
            else if (_role == 1)
            {
                SceneManager.LoadScene("SecHomePage");
            }
        }

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
