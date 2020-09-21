using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpeningFadeIN : MonoBehaviour
{
    public Image fadein;
    public OpeningClicktoStart firstpanel;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeinfunction());
    }

    private IEnumerator fadeinfunction()
    {
        yield return new WaitForSeconds(3f);

        while (true)
        {
            fadein.color -= new Color(0, 0, 0, Time.deltaTime);
            yield return null;
            if (fadein.color.a <= 0)
            {
                fadein.gameObject.SetActive(false);
                firstpanel.startauto();
                break;
            }
        }
    }
}
