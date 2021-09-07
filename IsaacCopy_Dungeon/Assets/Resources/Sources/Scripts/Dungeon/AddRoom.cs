using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddRoom : MonoBehaviour
{
    public Vector2 offset;
    public Vector2 BoxSize;
    //public Sprite minimapRoom;
    private RoomTemplates templates;

    public void SetRoom(int specialvalue)
    {
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        templates.rooms.Add(this.gameObject);
        SetRoomProps(specialvalue); //생성
    }

    void SetRoomProps(int specialvalue)
    {

        if (specialvalue != -1) //특별한 방이면
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<RoomSpawner>())
                {
                    if (transform.GetChild(i).GetComponent<RoomSpawner>().spawned)
                    {

                        Spawn(templates.SpecialRooms[specialvalue], transform, transform.GetChild(i).transform.position);

                    }
                }
            }

        }
        else //특별한 방이 아니면
        {
            int randomAreaIndex = Random.Range(0, templates.RoomProps.Length);
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<RoomSpawner>())
                {
                    if (transform.GetChild(i).GetComponent<RoomSpawner>().spawned)
                    {

                        Spawn(templates.RoomProps[randomAreaIndex], transform, transform.GetChild(i).transform.position);
                        Spawn(templates.Areas[randomAreaIndex].props[Random.Range(0, templates.Areas[randomAreaIndex].props.Length)], transform, transform.GetChild(i).transform.position);

                    }
                }
            }
        }
    }
    void Spawn(GameObject go, Transform tr, Vector3 pos)
    {
        GameObject GO = Instantiate(go, tr);
        GO.transform.position = pos;
    }
}
