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
            // ���ʵ�������ڣ��򽫸ö�����Ϊ���ᱻ���ٵĶ���
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // ���ʵ���Ѵ��ڣ������ٵ�ǰ�����Ա�ֻ֤��һ��ʵ������
            Destroy(gameObject);
        }
    }
}
