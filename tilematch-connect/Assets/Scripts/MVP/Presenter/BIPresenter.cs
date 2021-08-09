using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIPresenter : MonoBehaviour
{
    public Animator biAnimator;
    public GameObject bi;

    private void Awake()
    {
        // todo: 애니메이션 시작시 중간부터 시작하는게 있어서 이게 맞는지 체크해봐야함
        bi.SetActive(false);
    }

    private void Start()
    {
        Debug.Log("BI load");
        biAnimator.SetTrigger("Idle");
        Invoke("Stay", 2.5f);
        Invoke("NextScene", 4.0f);
    }

    private void Stay()
    {
        bi.SetActive(true);
        Debug.Log("BI stay");
        biAnimator.SetTrigger("Stay");
    }

    private void NextScene()
    {
        GameSceneManager.Instance.LoadScene(GameSceneManager.SceneType.Lobby);
    }
}
