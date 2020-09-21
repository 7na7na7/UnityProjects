using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    public Image firstfadein;
    public Image[] panelstoclick;
    int clickcount = 0;

    public AudioSource BGM;
    public AudioSource bosssound;

    private void Start()
    {
        StartCoroutine(Firstfadein());
    }

    private IEnumerator Firstfadein()
    {
        while (true)
        {
            firstfadein.color -= new Color(0, 0, 0, Time.deltaTime);
            
            yield return new WaitForSeconds(0.05f);
            if (firstfadein.color.a <= 0)
            {
                firstfadein.gameObject.SetActive(false);
                BGM.Play();
                endingstart();
                break;
            }
        }
    }

    void endingstart()
    {
        StartCoroutine(fadeinpanels(clickcount));
    }

    private IEnumerator fadeinpanels(int i)
    {
        if (i < panelstoclick.Length)
        {
            while (true)
            {
                if (i < panelstoclick.Length - 1)
                {
                    panelstoclick[i].color -= new Color(0, 0, 0, Time.deltaTime * 0.4f);
                }
                else
                {
                    panelstoclick[i].color -= new Color(0, 0, 0, Time.deltaTime * 0.2f);
                }
                yield return null;
                if (panelstoclick[i].color.a <= 0)
                {
                    if (i >= panelstoclick.Length - 1)
                    {

                        BGM.Stop();
                        bosssound.Play();
                        clickcount++;
                        endingstart();
                        break;
                        
                    }
                    panelstoclick[i].gameObject.SetActive(false);
                    clickcount++;
                    if (i >= panelstoclick.Length - 2)
                    {
                        yield return new WaitForSeconds(2f);
                    }
                    endingstart();
                    break;
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene(0);
        }
    }

}
