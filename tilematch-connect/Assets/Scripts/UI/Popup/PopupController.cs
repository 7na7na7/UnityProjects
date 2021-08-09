using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoGo.Extension;

public class PopupController : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> _objectContainer = new List<GameObject>();
    [SerializeField]
    public GameObject _currentShowedObject = null;
    private static PopupController _instance = null;
    public static PopupController Instance
    {
        get
        {
            if (_instance == null) _instance = UICenter.Instance.popupController;
            return _instance;
        }
    }


    /// <summary>
    /// BasePopup 에서 접근 
    /// </summary>
    public static string mCloseEvent = "CloseEvent";

    [Header("Popup_show_canvas")]
    public Canvas PopupCanvas; // 일반적인 팝업 오픈
    public Canvas CoverCanvas; // 최상단 시스템 팝업
    public enum ShowCanvas
    {
        Popup,
        Cover
    }
    /// <summary>
    /// 현재 팝업 닫기
    /// </summary>
    /// <param name="_object"></param>
    public void CloseObject(GameObject _object)
    {
        _objectContainer.Remove(_object);
        if (_objectContainer.Count > 0)
        {
            GameObject lastObject = _objectContainer[_objectContainer.Count - 1];
            _currentShowedObject = lastObject;
            lastObject.SetActive(true);
        }
        else
        {
            _currentShowedObject = null;

        }
        Destroy(_object);
    }
    /// <summary>
    /// 팝업 전체 닫기
    /// </summary>
    public void ClearPopup()
    {
        _objectContainer.Clear();
        CloseObject(_currentShowedObject);
    }



    /// <summary>
    /// 어드레서블 키로 접근하여 팝업 프리팹을 꺼내와 캔버스 위치에 인스턴스 시킴
    /// </summary>
    /// <param name="_name">addressable Key</param>
    /// <param name="_canvas">Prefab을 인스턴스할 캔버스 위치</param>
    /// <returns></returns>
    public GameObject GetPopup(string _name, ShowCanvas _canvas = ShowCanvas.Popup)
    {
        GameObject _prefab = ResourceController.Instance.GetPopupPrefab(_name);
        GameObject popupObject = Instantiate(_prefab, Vector3.zero, Quaternion.identity,
            _canvas == ShowCanvas.Popup ? PopupCanvas.transform : CoverCanvas.transform);
        popupObject.transform.localScale = Vector3.one;

        RectTransform rectTransform = popupObject.GetComponent<RectTransform>();
        rectTransform.localScale = Vector2.one;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;

        _currentShowedObject = popupObject;
        _objectContainer.Add(popupObject);
        return popupObject;
    }


    #region 팝업 오픈하였을 때 부수적인 효과 제어
    private bool _onEffect;
    public bool OnEffect
    {
        get => _onEffect;
        set
        {
            _onEffect = value;
            switch (_onEffect)
            {
                case true:

                    //WorldCenter.Instance.mainCameraScript.translucentImage.preview = true;

                    // UICenter.Instance.ActionFadeScreen(true);

                    break;
                default:
                    //WorldCenter.Instance.mainCameraScript.translucentImage.preview = false;
                    // UICenter.Instance.ActionFadeScreen(false);
                    break;
            }
        }
    }
    #endregion

    #region 팝업 샘플 예제
    public class PopupSample : BasePopup
    {
        public override void OpenPopup(PopupController _controller, EventListener.Call_string _listener = null, EventListener.Call_boolean _openCheck = null)
        {
            base.OpenPopup(_controller, _listener, _openCheck);
            // 추가 사항 작성 
        }
        public void OnClick_A()
        {
            InsertCloseEvent("Click_A");
            ClosePopup();
        }
        public void OnClick_B()
        {
            InsertCloseEvent("Click_B");
            ClosePopup();
        }
    }

    public class PopupSample_2 : BasePopup
    {
        public override void OpenPopup(PopupController _controller, Dictionary<int, string> _eventContainer, EventListener.Call_string _listener)
        {
            base.OpenPopup(_controller, _eventContainer, _listener);
            // 추가 사항 작성 
        }
        public override void OnClickButton(int _index)
        {
            base.OnClickButton(_index);
            // 추가 사항 작성
        }
    }

    public void ShowPopup_Sample_2()
    {
        Dictionary<int, string> _eventContainer = new Dictionary<int, string>();
        _eventContainer[0] = "value_0";
        _eventContainer[1] = "value_1";

        GetPopup("Popup_Sample2").GetComponent<PopupSample_2>().OpenPopup(this, _eventContainer, (string _result) => {
            if (_result == _eventContainer[0])
            {

            }
            else if (_result == _eventContainer[1])
            {

            }
        });
    }

    #endregion

    #region Extra Popup
    public enum ClickPopType
    {
        Default,
        GameReady

    }
    public void ShowClickPop(ClickPopType _clickType, EventListener.Call_boolean _listener = null)
    {
        switch (_clickType)
        {
            case ClickPopType.Default:
                break;
            case ClickPopType.GameReady:
                ShowClickPop_GameReady(_listener);

                break;
        }

    }
    private void ShowClickPop_GameReady(EventListener.Call_boolean _listener)
    {
        //GetPopup("PopClick_GameReady").GetComponent<BaseClickpop>().ShowActionPop(5.0f, "준비", "시작", _listener);
    }

    #endregion

    #region Main Popup


    public void ShowPopup_Test(string _name)
    {
        switch (_name)
        {

            default:
                //ShowPopup_Default();
                break;
        }
    }

    //private void ShowPopup_Default()
    //{
    //    GetPopup("Popup_Default").GetComponent<Popup_Default>().OpenPopup(this, (string _result) =>
    //    {

    //        if (_result == "Close")
    //        {

    //            Debug.Log("Popup_Close".TransColor());
    //        }

    //    });

        //}

        //public void ShowPopup_Settings(EventListener.Call_boolean _closed)
        //{
        //    GetPopup("Popup_Rename").GetComponent<PopupSettings>().OpenPopup(this, (string _result) => {

        //        if (_result == "Close")
        //        {
        //            _closed?.Invoke(true);
        //        }
        //    });
        //}

    public void ShowPopupSetting()
    {
        GetPopup("OptionPopup").GetComponent<PopupSettings>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }

    public void ShowPopupVictory()
    {
        GetPopup("VictoryPopup").GetComponent<PopupVictory>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }
    

    public void ShowPopupDefeat()
    {
        GetPopup("DefeatPopup").GetComponent<PopupDefeat>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }

    public void ShowPopupPersonalInfo()
    {
        GetPopup("PersonalInfoPopup").GetComponent<PopupPersonalInfo>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }

    public void ShowPopupRank()
    {
        GetPopup("RankPopup").GetComponent<PopupRank>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }

    public void ShowPopupStore()
    {
        GetPopup("StorePopup").GetComponent<PopupStore>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }

    public void ShowPopupWorldMap()
    {
        GetPopup("WorldMapPopup").GetComponent<PopupWorldMap>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }

    public void ShowPopupNoAds()
    {
        GetPopup("NoAdsPopup").GetComponent<PopupNoAds>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }

    public void ShowPopupReceive()
    {
        GetPopup("ReceivePopup").GetComponent<PopupReceive>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }

    public void ShowRoulette()
    {
        GetPopup("Roulette").GetComponent<Roulette>().OpenPopup(this, (string _result) => {

            if (_result == "Close")
            {

            }
        });
    }




    //public void ShowPopup_SetProfile()
    //{

    //    GetPopup("Popup_SetProfile").GetComponent<Popup_SetProfile>().OpenPopup(this, (string _result) => {

    //        if (_result == "Close")
    //        {

    //        }
    //    });

    //}


    //public void ShowPopup_Store()
    //{

    //    GetPopup("Popup_Store").GetComponent<Popup_Store>().OpenPopup(this, (string _result) => {

    //        if (_result == "Close")
    //        {

    //        }
    //    });

    //}

    //public void ShowPopup_Setting()
    //{
    //    GetPopup("Popup_Setting").GetComponent<Popup_Setting>().OpenPopup(this, (string _result) => {

    //        if (_result == "Close")
    //        {

    //        }
    //    });
    //}

    #endregion






}
