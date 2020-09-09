using UnityEngine;
using UnityEngine.UI;

public class Bgm_BgsManager : MonoBehaviour
{
    public Toggle bgmTg, seTg;
    
    private AudioSource bgm;
    void Start()
    {
        bgm = FindObjectOfType<BGM>().GetComponent<AudioSource>();
        if (SoundMgr.instance.savedBgm == 1)
            bgmTg.isOn = true;
        else
            bgmTg.isOn = false;
        if (SoundMgr.instance.savedSE == 1)
            seTg.isOn = true;
        else
            seTg.isOn = false;
    }

    public void BGMUp()
    {
        SoundMgr.instance.bgmValue(bgmTg.isOn == true ? 1:0);
    }
   
    public void SEUp()
    {
        SoundMgr.instance.seValue(seTg.isOn == true ? 1:0);
    }
}
