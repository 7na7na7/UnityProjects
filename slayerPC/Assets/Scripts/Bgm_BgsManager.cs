using UnityEngine;
using UnityEngine.UI;

public class Bgm_BgsManager : MonoBehaviour
{
    public Slider bgm, bgs;
    void Start()
    {
        bgm.value = SoundManager.instance.savedBgm;
        bgs.value = SoundManager.instance.savedBgs;
    }

    public void onBgmChange()
    {
        SoundManager.instance.bgmValue(bgm.value);
    }

    public void onBgsChange()
    {
        SoundManager.instance.bgsValue(bgs.value);
    }
}
