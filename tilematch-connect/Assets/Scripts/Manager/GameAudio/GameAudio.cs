using UnityEngine;

public abstract class GameAudio : ScriptableObject
{
    private int _sourceID;
    public AudioClip audioClip;
    public int SourceID
    {
        get
        {
            return _sourceID;
        }
        set
        {
            _sourceID = value;
        }
    }
}
