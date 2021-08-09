using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Leguar.TotalJSON;
using GoGo.Extension;
public class GameDataManager : BaseSingleton<GameDataManager>
{
    public GameDataManager() { }
   
     
    
    public enum TableKey
    {
    
        Null,
        Monster,
        Reward

    }


    readonly string FILE_PATH = "Assets/Resources/JsonFile/";
    readonly string DATA_SHEET_NAME = "TestDataSheet";

    public void CustomInit()
    {
              
        string _jsonString = ReadJsonFile(DATA_SHEET_NAME);
        DeserializeData(_jsonString);

    }

          

    // public List<MonsterData> myDataTestList = new List<MonsterData>();

    

    /// <summary>
    /// 경로로 부터 Json 파일 이름을 읽어 스트링으로 변환
    /// </summary>
    /// <param name="_sheetName">export 된 json 시트 파일 이름</param>
    /// <returns></returns>
    private string ReadJsonFile(string _sheetName)
    {
        string _path = FILE_PATH + _sheetName + ".json";
        StreamReader reader = new StreamReader(_path);
        string _jsonAsString = reader.ReadToEnd();
        reader.Close();
        return _jsonAsString;
    }

    private void DeserializeData(string _jsonString)
    {
        JSON sheetRoot =  JSON.ParseString(_jsonString);

        ///-> 스프레드 시트에서 각 테이블 키 데이타 파싱
        LoadGameData(sheetRoot, TableKey.Monster);


    }

        


    private void LoadGameData(JSON _jsonRoot, TableKey _tableKey)
    {
        string sheetKey = _tableKey.ToString();

        JSON sheetTable = _jsonRoot.GetJSON(sheetKey);

        switch (_tableKey) // 타입별 데이타 캐시
        {
            case TableKey.Monster:
          
                for (int i = 0; i < sheetTable.Count; i++)
                {
                    string _index = string.Format("{0}_{1}", sheetKey, i);
                    // MonsterData _data = sheetTable.GetJSON(_index).Deserialize<MonsterData>();
                    // _data.SetMonsterKey(sheetKey, i);
                    // myDataTestList.Add(_data);
                }

                break;
        }
              
     }

}
