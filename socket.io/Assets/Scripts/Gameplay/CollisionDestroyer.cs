using System;
using System.Collections;
using System.Collections.Generic;
using Project.Networking;
using UnityEngine;

namespace Project.Gameplay
{
    public class CollisionDestroyer : MonoBehaviour
    {
        public NetworkIdentity networkIdentity;
        public WhoActivateMe whoActivedMe;

        private void OnCollisionEnter2D(Collision2D col)
        {
            NetworkIdentity ni = col.gameObject.GetComponent<NetworkIdentity>();
            if (ni == null || ni.GetID() != whoActivedMe.GetActivator()) //충돌체가 서버오브젝트가 아니거나, 서버오브젝트인데 나 자신이 아니라면
            {
                networkIdentity.GetSocket().Emit("collisionDestroy",new JSONObject(JsonUtility.ToJson(new IDdata
                {
                    id=networkIdentity.GetID()
                })));
            }
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            NetworkIdentity ni = col.gameObject.GetComponent<NetworkIdentity>();
            if (ni == null || ni.GetID() != whoActivedMe.GetActivator()) //충돌체가 서버오브젝트가 아니거나, 서버오브젝트인데 나 자신이 아니라면
            {
                networkIdentity.GetSocket().Emit("collisionDestroy",new JSONObject(JsonUtility.ToJson(new IDdata
                {
                    id=networkIdentity.GetID()
                })));
            }
        }
    }
}

