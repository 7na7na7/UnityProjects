using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoGo.Extension;
using System;
using System.Linq;

public class GameAudioManager : MonoBehaviour
{
    protected GameAudioManager() { }
    private static GameAudioManager _instance;
    public static GameAudioManager Instance
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
    private void Awake()
    {
        
        MakeSingleton();

    }
    public void CustomInit()
    {
        InitGameSound();

        //InitUISound();  // 할당된  UI 소스 타입에 맞게 인덱스 정렬
    }
    [Range(0, 1)] public float musicVolume;
    [Range(0, 1)] public float soundVolume;
    public AudioSource BGMSource;
    public AudioSource SFXSource;
    public AudioSource UISource;

    /// <summary>
    /// 볼륨을 0 --> DefaultMusicVolume : 서서히 커지게
    /// </summary>
    public void SetVolumeFadeIn()
    {
        StartCoroutine(RoutineFadeVolume(0, musicVolume));
    }
    /// <summary>
    /// 볼륨을 DefaultMusicVolume --> 0 : 서서히 작아지게
    /// </summary>
    public void SetVolumeFadeOut(float _duration = 0.85f)
    {
        StartCoroutine(RoutineFadeVolume(musicVolume, 0 , _duration));
    }
    IEnumerator RoutineFadeVolume(float _from, float _to, float _duration = 0.85f)
    {
        float from = _from;
        float to = _to;
        float elapsedTime = 0.0f;
        while (elapsedTime <= 1.0f)
        {
            elapsedTime += Time.deltaTime / _duration;
            //BGMSource.volume = Mathf.Lerp(from, to, elapsedTime.Interpolation(SmoothType.EaseInWithCos)); // todo bgm
            yield return null;
        }
    }
    public IEnumerator VolumeFadeChange(float _prevFadeOutTime, float _nextFadeInTime, MusicTag _nextSource, float _targetValue = 0.8f)
    {
        float from = BGMSource.volume;
        float to = 0;

        float elapsedTime = 0f;
        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime / _prevFadeOutTime;
            //BGMSource.volume = Mathf.Lerp(from, to, elapsedTime.Interpolation(SmoothType.SmootherStep)); // todo bgm
            yield return null;
        }

        PlayMusic(_nextSource);
        from = 0;
        to = _targetValue;

        elapsedTime = 0f;
        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime / _nextFadeInTime;
            BGMSource.volume = Mathf.Lerp(from, to, elapsedTime.Interpolation(SmoothType.SmootherStep));
            yield return null;
        }
    }

    #region 사운드 소스 타입
    [Header("Sound_Source")]
    [SerializeField] List<GameSound> soundGroup = null;
    
    public enum SoundType
    {
        UI,
        SFX
    }
    public enum SoundTag
    {
        Stop,
        UI_BTN_Common,
        UI_BTN_Special,
        UI_BTN_Squeeky,
        UI_POP_Result_Positive,
        UI_POP_Result_Negative,
        UI_Pop_Open,
        UI_Pop_Close,
        UI_BTN_Toggle,
        UI_BTN_Roll,
        UI_BTN_Play,
        FX_Tile_Pick,
        FX_Tile_Dead,
        FX_Tile_Undo,
        FX_Tile_Shuffle,
    }

    Func<SoundTag, int> soundTagToIdx = _type =>
    {
        switch (_type)
        {
            default:
                return 0;
            case SoundTag.UI_BTN_Common:
                return 1;
            case SoundTag.UI_BTN_Special:
                return 2;
            case SoundTag.UI_BTN_Squeeky:
                return 3;
            case SoundTag.UI_POP_Result_Positive:
                return 4;
            case SoundTag.UI_POP_Result_Negative:
                return 5;
            case SoundTag.UI_Pop_Open:
                return 6;
            case SoundTag.UI_Pop_Close:
                return 7;

            case SoundTag.UI_BTN_Toggle:
                return 8;
            case SoundTag.UI_BTN_Roll:
                return 9;
            case SoundTag.UI_BTN_Play:
                return 10;
            case SoundTag.FX_Tile_Pick:
                return 11;
            case SoundTag.FX_Tile_Dead:
                return 12;
            case SoundTag.FX_Tile_Undo:
                return 13;
            case SoundTag.FX_Tile_Shuffle:
                return 14;

        }
    };

    private async void InitGameSound()
    {
        soundGroup = await AddressableManager.Instance.LoadAssetsByLabel<GameSound>("GameSound");
        int index = 0; List<GameSound> temp = soundGroup;
        foreach (GameSound sound in soundGroup) sound.SourceID = soundTagToIdx(sound.tag);
        foreach (GameSound sound in temp.OrderBy(t => t.SourceID)) soundGroup[index++] = sound;
    }
    /// <summary>
    /// UI/SFX 사운드 출력 , SoundStop (소리끔)
    /// </summary>
    /// <param name="_tag"></param>
    public void PlaySound(SoundTag _tag)
    {
        if (_tag == SoundTag.Stop)
        {
            if (UISource != null) if (UISource.isPlaying) UISource.Stop();
            if (SFXSource != null) if (SFXSource.isPlaying) SFXSource.Stop();
        }
        else
        {
            int index = soundTagToIdx(_tag);
            switch (soundGroup[index].type)
            {
                case SoundType.UI:
                    UISource.PlayOneShot(soundGroup[index].audioClip);
                    break;
                case SoundType.SFX:
                    SFXSource.PlayOneShot(soundGroup[index].audioClip);
                    break;
            }
        }
    }
    #endregion







    #region BGM 사운드 소스 타입
    [Header("---BGM_Source---")]
    public MusicTag OnPlayBGM;


    const int _RANGE_BGM_Common = 5;
    const int _RANGE_BGM_Play = 2;
    [SerializeField] GameMusic[] BGMGroup_Common = new GameMusic[_RANGE_BGM_Common];
    [SerializeField] GameMusic[] BGMGroup_Play = new GameMusic[_RANGE_BGM_Play];

    public enum MusicTag
    {

        BGM_Stop = 0,
        // BGMGroup_Common
        BGM_Intro = 1,
        BGM_BrandImage = 2,
        BGM_Lobby = 3,
        // BGMGroup_Play
        BGM_Play = 4 
    }

    public void PlayMusic(MusicTag _type, int _subIndex = 0)
    {
        switch (_type)
        {
            case MusicTag.BGM_Stop:
                if (BGMSource != null) if (BGMSource.isPlaying) BGMSource.Stop();
                break;

            case MusicTag.BGM_Intro:
            case MusicTag.BGM_BrandImage:
            case MusicTag.BGM_Lobby:

                if (BGMGroup_Common[BgmToIdx(_type)].isLoop) BGMSource.loop = true;
                else BGMSource.loop = false;
                BGMSource.clip = BGMGroup_Common[BgmToIdx(_type)].audioClip;
                BGMSource.Play();
                OnPlayBGM = _type;

                break;
            case MusicTag.BGM_Play:

                if (BGMGroup_Play[_subIndex].isLoop) BGMSource.loop = true;
                else BGMSource.loop = false;
                BGMSource.clip = BGMGroup_Play[_subIndex].audioClip;
                BGMSource.Play();
                OnPlayBGM = _type;
                break;
        }
    }

    Func<MusicTag, int> BgmToIdx = _type =>
    {
        switch (_type)
        {
            default:
                return 0;
            case MusicTag.BGM_Intro:
                return 1;
            case MusicTag.BGM_BrandImage:
                return 2;
            case MusicTag.BGM_Lobby:
                return 3;
            case MusicTag.BGM_Play:
                return 4;
        }
    };


    #endregion





}
