using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using GoGo.Extension;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;


public class GameSceneManager : MonoBehaviour
{
    #region Make Singleton
    protected GameSceneManager() { }
    private static GameSceneManager _instance;
    public static GameSceneManager Instance
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
    #endregion

    public SceneType? GetSceneType(string _name) //씬 이름을 주면 씬 타입을 반환해 줌 
    {
        switch (_name)
        {
            default:
                return null;
            case "Intro":
                return SceneType.Intro;
            case "BrandImage":
                return SceneType.BrandImage;
            case "Lobby":
                return SceneType.Lobby;
            case "Play":
                return SceneType.Play;
        }
    }
    public enum SceneType
    {
        Intro = 0,
        BrandImage = 1,
        Lobby = 2,
        Play = 3,
    }


    public void CustomInit()
    {
        //GameAudioManager.Instance.PlayMusic(GameAudioManager.MusicTag.BGM_Intro);
        GameAudioManager.Instance.SetVolumeFadeOut(3.0f);
        ActionFadeOut(1.5f, 1.5f, (bool fadeOut) => {
            if (fadeOut)
            {
                Addressables.LoadSceneAsync(SceneType.BrandImage.ToString(), LoadSceneMode.Single).Completed += OnSceneLoadCompleted;
                if (string.IsNullOrEmpty(_currScene.name)) Task.Delay(1);
            }
        });
    }
    public async void LoadScene(SceneType _sceneType)
    {
        _nextScene = await LoadSceneFade(_sceneType.ToString(), LoadSceneMode.Single);
    }
    [SerializeField]
    private Scene _nextScene;
    [SerializeField]
    private Scene _currScene;

    private Task<Scene> LoadSceneFade(string _sceneName, LoadSceneMode _loadMode)
    {
        _currScene = default;
        _currScene.name = "";

        GameAudioManager.Instance.SetVolumeFadeOut();
        ActionFadeOut(1.5f, 0.0f, (bool fadeOut) => {
            if (fadeOut)
            {
                Addressables.LoadSceneAsync(_sceneName, _loadMode).Completed += OnSceneLoadCompleted;
                if (string.IsNullOrEmpty(_currScene.name)) Task.Delay(1);
            }
        });
        return Task.Run(() => _currScene);
    }
    private void OnSceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded){
            _currScene = obj.Result.Scene;
            var sceneType = GetSceneType(_currScene.name);
            if (sceneType == null) return;
            switch (sceneType)
            {
                case SceneType.Intro:

                    GameAudioManager.Instance.SetVolumeFadeIn();
                    //GameAudioManager.Instance.PlayMusic(GameAudioManager.MusicTag.BGM_Intro); 
                    ActionFadeIn(1.5f, 1.0f, (bool fadeIn) => {
                        if (fadeIn)
                        {
                
                        }
                    });
                    break;
                case SceneType.Lobby:
                    GameAudioManager.Instance.SetVolumeFadeIn();
                    GameAudioManager.Instance.PlayMusic(GameAudioManager.MusicTag.BGM_Lobby);
                    ActionFadeIn(1.5f, 1.0f, (bool fadeIn) => {
                        if (fadeIn)
                        {
                
                        }
                    });


                    break;
                case SceneType.BrandImage:
                    GameAudioManager.Instance.SetVolumeFadeIn();
                    //GameAudioManager.Instance.PlayMusic(GameAudioManager.MusicTag.BGM_BrandImage);
                    ActionFadeIn(1.5f, 1.0f, (bool fadeIn) => {
                        if (fadeIn)
                        {

                        }
                    });
                    break;
                //case SceneType.Menu:
                //    break;
                case SceneType.Play:
                    GameAudioManager.Instance.SetVolumeFadeIn();
                    GameAudioManager.Instance.PlayMusic(GameAudioManager.MusicTag.BGM_Play);
                    ActionFadeIn(1.5f, 1.0f, (bool fadeIn) => {
                        if (fadeIn)
                        {

                        }
                    });
                    break;
            }
        }
    }

    #region Fade In/Out 코루틴 로직 
    
    public SpriteRenderer fadeRenderer;
    public Color screenColor = Color.black;

    
    
    public void ActionFadeIn(float _duration, float _delayTime, EventListener.Call_boolean _result)
    {
        StartCoroutine(RoutineFade(1.0f, 0.0f, _duration, _delayTime, _result));
    }
    public void ActionFadeOut(float _duration, float _delayTime, EventListener.Call_boolean _result)
    {
        StartCoroutine(RoutineFade(0.0f, 1.0f, _duration, _delayTime, _result));
    }
    IEnumerator RoutineFade(float _from, float _to, float _duration, float _delay = 0.0f,
        EventListener.Call_boolean _result = null)
    {
        screenColor.a = _from;
        fadeRenderer.color = screenColor;
        float _elapsedTime = 0;

        if (_delay > 0)
        {
            while (_elapsedTime < 1)
            {
                _elapsedTime += Time.deltaTime / _delay;
                yield return null;
            }
        }
        _elapsedTime = 0;

        while (_elapsedTime < 1.0f)
        {
            _elapsedTime += Time.deltaTime / _duration;
            screenColor.a = Mathf.Lerp(_from, _to, _elapsedTime.Interpolation(SmoothType.Exponential));
            fadeRenderer.color = screenColor;
            yield return null;
        }
        _result?.Invoke(true);
    }
    #endregion


    #region Test Editor Button

    public void EditorBTN_FadeIn()
    {

        ActionFadeIn(1.5f, 0.0f, (bool fadeIn) => {
            if (fadeIn)
            {

            }
        });

    }
    public void EditorBTN_FadeOut()
    {
        ActionFadeOut(1.5f, 0.0f, (bool fadeOut) => {
            if (fadeOut)
            {
                                
            }
        });
    }


    #endregion
}