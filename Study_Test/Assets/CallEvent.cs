using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEvent : MonoBehaviour
{
    void Start()
    {
        Event @event = GetComponent<Event>();
        @event.OnSpacePressed += OnSpacePressed;
        //Event스크립트의 OnSpacePressed이벤트에 자신의 OnSpacePressed함수 추가!
    }

    void OnSpacePressed(object sender, EventArgs e)
    {
        print("스페이스바 눌림!");
    }
}
