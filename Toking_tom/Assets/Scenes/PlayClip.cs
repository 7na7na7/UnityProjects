using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayClip : MonoBehaviour
{
    private AudioSource source;
    public GameObject panel2;
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void plyClip()
    {
        source.pitch = FindObjectOfType<ClipSaver>().savedPitch;
        source.clip = FindObjectOfType<ClipSaver>().savedClip;
        source.Play();
    }

    public void no()
    {
        panel2.SetActive(true);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void retrn()
    {
        Destroy(FindObjectOfType<ClipSaver>().gameObject);
        SceneManager.LoadScene("SampleScene");
    }
}
