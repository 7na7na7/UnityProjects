using UnityEngine;

public class EventListener : MonoBehaviour
{
    public delegate void Call_boolean(bool _result);
    public delegate void Call_string(string _result);
    public delegate void Call_integer(int _result);
    public delegate void CallBack();
}
