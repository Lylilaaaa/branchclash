using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInformation : MonoBehaviour
{
    public static UserInformation _instance;
    public UserData userRoleData;
    private void Awake()
    {
        if (_instance == null)
        {
            // 如果实例不存在，则将该对象标记为不会被销毁的对象
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 如果实例已存在，则销毁当前对象，以保证只有一个实例存在
            Destroy(gameObject);
        }
    }
}
