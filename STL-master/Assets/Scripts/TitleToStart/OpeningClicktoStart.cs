using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OpeningClicktoStart : MonoBehaviour
{
    public Image thispanel;
    public GameObject nextpanel;
    public GameObject beforeImage;
    public Image[] panelstoclick;
    public bool gotogamestart;
    int clickcount = 0;

    public Image fadeoutPanel;

    //public void OnPointerClick(PointerEventData pointerEventData)
    //{
    //    StartCoroutine(fadeinpanels(clickcount));
    //}

    public void startauto()
    {
        StartCoroutine(fadeinpanels(clickcount));

    }

    private IEnumerator fadeinpanels(int i)
    {
        if(i < panelstoclick.Length)
        {
            thispanel.raycastTarget = false;

            while (true)
            {
                panelstoclick[i].color -= new Color(0, 0, 0, Time.deltaTime);
                yield return null;
                if (panelstoclick[i].color.a <= 0)
                {
                    panelstoclick[i].gameObject.SetActive(false);
                    clickcount++;
                    thispanel.raycastTarget = true;
                    startauto();
                    break;
                }
            }
        }
        else
        {
            if (!gotogamestart)
            {
                yield return new WaitForSeconds(2f);
                beforeImage.SetActive(false);
                nextpanel.SetActive(true);
                nextpanel.GetComponent<OpeningClicktoStart>().startauto();
                gameObject.SetActive(false);
            }
            else
            {
                yield return new WaitForSeconds(2f);
                while (true)
                {
                    fadeoutPanel.color += new Color(0, 0, 0, Time.deltaTime);
                    yield return null;
                    if (fadeoutPanel.color.a >= 1)
                    {
                        SceneManager.LoadScene(2);
                        break;
                    }
                }
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("play");
        }
    }
}
