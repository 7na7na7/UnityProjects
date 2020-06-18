using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    void Start()
    {
        
        samples=new float[sampleRate];
        aud = Microphone.Start(Microphone.devices[0].ToString(), true, 1, sampleRate);
    }

   
    void Update()
    {
        aud.GetData(samples, 0); //-1f ~ 1f
                float sum = 0;
                for (int i = 0; i < samples.Length; i++)
                {
                    sum += samples[i] * samples[i]; //제곱함
                }

                rmsValue = Mathf.Sqrt(sum / samples.Length); //제곱근 구함
                rmsValue = rmsValue * modulate; //값이 작으니 조정값 곱해줌
                rmsValue = Mathf.Clamp(rmsValue, 0, 100); //0과 100이내에서 값 제한
                resultValue = Mathf.RoundToInt(rmsValue); //소수점 버려주고 Int형으로 만들어줌

                if (resultValue > cutValue) //cutValue보다 작으면
                {
                    if (!isRecording)
                    {
                        isRecording = true;
                        StartCoroutine(recordAndPlay());
                    }
                }
        }

    IEnumerator recordAndPlay()
    {
       FindObjectOfType<PlaySound>().Record();
        anim.Play("hear");
        yield return new WaitForSeconds(hearLength);
        FindObjectOfType<PlaySound>().Play();
        anim.Play("speak");
        yield return new WaitForSeconds(hearLength*(1/FindObjectOfType<PlaySound>().pitch));
        anim.Play("Idle");
        aud = Microphone.Start(Microphone.devices[0].ToString(), true, 1, sampleRate);
        rmsValue = 0;
        resultValue = 0;
        mal.text = "\'" +FindObjectOfType<GameManager>().name + "\' 에게 하고 싶은 말이 있나요?";
        panel2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        FindObjectOfType<PlaySound>().Record();
        anim.Play("hear");
        yield return new WaitForSeconds(hearLength*(1/FindObjectOfType<PlaySound>().pitch));
        FindObjectOfType<PlaySound>().Play();
        anim.Play("speak");
        yield return new WaitForSeconds(hearLength*0.75f);
    }
   
    
   
}
