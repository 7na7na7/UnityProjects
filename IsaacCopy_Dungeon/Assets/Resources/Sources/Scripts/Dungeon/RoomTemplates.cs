using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[Serializable]
public class Area
{
    public GameObject[] props;
}
public class RoomTemplates : MonoBehaviour
{
    public GameObject[] RoomProps;
    public Area[] Areas;
    public GameObject[] SpecialRooms;

    public int StraightCount = 0;
    public int minRoomCount = 7;
    public int maxRoomCount = 50;
    public int maxRoomCountSave;
    public int PlayerSpawnMinusValue = 3;
    //한칸짜리 방들
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    [Header("큰방이 생성된확률(백분율)")]
    public int BigRoomPercent;
    //큰방들
    public GameObject[] bottomRooms_B;
    public GameObject[] topRooms_B;
    public GameObject[] leftRooms_B;
    public GameObject[] rightRooms_B;


    public GameObject closedRoom;

    public List<GameObject> rooms;

    public float waitTime;
    public float DestroyerWaitTime;
    public float ReLoadTime;
    public GameObject boss;

    public int privateCount;
    public int publicCount;

    public Vector2 oneBox;
    private void Start()
    {

        Invoke("Spawn", waitTime);
        Invoke("ReLoad", ReLoadTime);

    }

    void ReLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Spawn()
    {

        Instantiate(boss, rooms[rooms.Count - 1].transform.position, quaternion.identity);

    }

}
