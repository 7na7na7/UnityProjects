using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePoolManager : MonoBehaviour
{
    #region 싱글턴 
    private static GamePoolManager _instance;
    public static GamePoolManager Instance
    { get { return _instance; } }
    private void MakeSingleton()
    {
        if (_instance != null) DestroyImmediate(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Awake()
    {
        MakeSingleton();
    }
    #endregion

    /// <summary>
    /// 오브젝트 풀 생성 초기화
    /// </summary>
    public void CustomInit()
    {
        if (OBJContainer != null && OBJContainer.isActiveAndEnabled)
        {
            OBJContainer.InitPoolContainer();
            OBJContainer.InitPoolTypeObject();
        }
        if (EFFContainer != null && EFFContainer.isActiveAndEnabled)
        {
            EFFContainer.InitPoolContainer();
            EFFContainer.InitPoolTypeObject();
        }
    }

    /// <summary>
    /// 오브젝트 제자리로 돌리기 이벤트 할당
    /// </summary>
    public delegate void ReturnPoolObjects();
    public ReturnPoolObjects OnReturnPoolObjects;


    #region 오브젝트 컨테이너 종류
    public OBJPoolContainer OBJContainer;
    public EFFPoolcontainer EFFContainer;
    #endregion

    

}
