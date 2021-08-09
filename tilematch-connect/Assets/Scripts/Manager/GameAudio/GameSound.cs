using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound_", menuName = "GameAudio/Sound", order = 2)]
public class GameSound : GameAudio
{
    public GameAudioManager.SoundTag tag;
    public GameAudioManager.SoundType type;
}
