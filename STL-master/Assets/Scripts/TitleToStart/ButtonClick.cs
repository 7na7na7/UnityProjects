using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public Image fadeout;

    public void buttonClick()
    {
        StartCoroutine(clickeffect());
    }

    private IEnumerator clickeffect()
    {
        transform.localScale *= 0.8f;
        yield return new WaitForSeconds(0.1f);
        transform.localScale /= 0.8f;

        fadeout.raycastTarget = true;
        while (true)
        {
            fadeout.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
            if (fadeout.color.a >= 1)
            {
                SceneManager.LoadScene(1);
                break;
            }
        }
    }

}
