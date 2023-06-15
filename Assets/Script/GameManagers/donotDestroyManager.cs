using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class donotDestroyManager : MonoBehaviour
{
    private static donotDestroyManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            // 如果实例不存在，则将该对象标记为不会被销毁的对象
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 如果实例已存在，则销毁当前对象，以保证只有一个实例存在
            Destroy(gameObject);
        }
    }
}
