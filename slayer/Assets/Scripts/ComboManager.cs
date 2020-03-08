using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    public int comboCount = 0; //콤보횟수
    public bool canCombo = false; //콤보중인지 판단
    public float comboDelay; //콤보 간 지속될 수 있는 시간

    public void comboIniitailize()
    {
        StopAllCoroutines();
        StartCoroutine(comboInitialization());
    }
    IEnumerator comboInitialization()
    {
        canCombo = true;
        comboCount++;
        yield return new WaitForSeconds(comboDelay);
        canCombo = false;
        Time.timeScale = 1; 
        comboCount = 0;
    }
}
