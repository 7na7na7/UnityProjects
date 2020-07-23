using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    public Image goComboImage = null;
    public Text txtCombo = null;

    private int currentCombo = 0;
    
    public void IncreaseCombo(int p_num=1) 
    { 
        currentCombo += p_num; 
        txtCombo.text = string.Format("{0:#,##0}", currentCombo);

        if (currentCombo > 2)
        {
            txtCombo.gameObject.SetActive(true);
            goComboImage.gameObject.SetActive(true);
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        txtCombo.text = "";
        txtCombo.gameObject.SetActive(false);
        goComboImage.gameObject.SetActive(false);
    }

    public int GetCurrentCombo()
    {
        return currentCombo;
    }
}
