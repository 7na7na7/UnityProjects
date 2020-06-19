using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject panel2;
    public Text mal;
    public int hearLength;
    
    public Animator anim;
    public AudioClip aud;
    private int sampleRate = 44100;
    private float[] samples;
    public float rmsValue; //말을 크게 하면 이 값이 올라감
    public float modulate; //rmsValue값에 곱해줄 조정값

    public int resultValue;
    public int cutValue; //이값보다 크면 녹음

    public bool isRecording = true;

    private bool second = false;
    void Start()
    {
        
        samples=new float[sampleRate];
        aud = Microphone.Start(Microphone.devices[0].ToString(), true, 1, sampleRate);
    }

   
    void Update()
    {
        
                    if (!isRecording)
                    {
                        if (second)
                        {
                            isRecording = true;
                        }
                        else
                        {
                            isRecording = true;
                            StartCoroutine(recordAndPlay());   
                        }
                    }
                
        }

    IEnumerator recordAndPlay()
    {
        second = true;
        FindObjectOfType<PlaySound>().Record();
        anim.Play("hear");
        yield return new WaitForSeconds(hearLength);
        FindObjectOfType<PlaySound>().Play(false);
        anim.Play("speak");
        yield return new WaitForSeconds(hearLength*(1/FindObjectOfType<PlaySound>().pitch));
        anim.Play("Idle");
        aud = Microphone.Start(Microphone.devices[0].ToString(), true, 1, sampleRate);
        mal.text = "\'" +FindObjectOfType<GameManager>().name + "\' 에게 하고 싶은 말이 있나요?";
        panel2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        rmsValue = 0;
        resultValue = 0;
        isRecording = false;
        yield return new WaitUntil(()=>isRecording);
        FindObjectOfType<PlaySound>().Record();
        anim.Play("hear");
        yield return new WaitForSeconds(hearLength);
        FindObjectOfType<PlaySound>().Play(true);
        anim.Play("speak");
        yield return new WaitForSeconds(hearLength*(1/FindObjectOfType<PlaySound>().pitch));
        SceneManager.LoadScene("BlockBreaking");
    }


}
