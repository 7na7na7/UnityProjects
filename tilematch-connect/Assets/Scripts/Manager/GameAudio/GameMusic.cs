using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Music_", menuName = "GameAudio/Music", order = 1)]
public class GameMusic : GameAudio
{
    public GameAudioManager.MusicTag tag;
    public bool isLoop;
}
