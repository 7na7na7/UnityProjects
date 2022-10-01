using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleSheetDataManager : MonoBehaviour
{
    void Awake()
    {
        TestTable.Data.Load();
        //TestTable의 로컬 데이터 불러오기!
        // UnityGoogleSheet.LoadAllData(); 
        // or UnityGoogleSheet.Load<TestTable.Data.Load>(); it's same!
        // or call TestTable.Data.Load(); it's same!
    }

    void Start()
    {
        //리스트 형식으로 불러오기!
        foreach (var value in TestTable.Data.DataList)
        {
            Debug.Log(value.index + "," + value.intValue + "," + value.strValue);
        }

        //딕셔너리 형식으로 불러오기!
        var dataFromMap = TestTable.Data.DataMap[0];
        Debug.Log("dataFromMap : " + dataFromMap.index + ", " + dataFromMap.intValue + "," + dataFromMap.strValue);
    }
}
