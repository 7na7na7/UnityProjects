using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Chat : MonoBehaviour
{
    public static Chat instance;

    private void Awake() => instance = this;
    public InputField SendInput;
    public RectTransform ChatContent;
    public Text ChatText;
    public ScrollRect ChatScrollRect;

    public void ShowMessage(string data)
    {
        ChatText.text += ChatText.text == "" ? data : '\n' + data;
        //비어 있으면 그냥데이터 넣고, 아니면 한줄 띄우고 데이터 넣기
        
        Fit(ChatText.GetComponent<RectTransform>());
        Fit(ChatContent);
        Invoke("ScrollDelay", 0.03f);
    }
    

    void Fit(RectTransform Rect) => LayoutRebuilder.ForceRebuildLayoutImmediate(Rect);
    //버그 방지를 위해 업데이트를 수동으로 해줌

    void ScrollDelay() => ChatScrollRect.verticalScrollbar.value = 0;
    //맨 아래로 이동

}
