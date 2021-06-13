using System.Collections;
using System.Collections.Generic;
using Project.Networking;
using UnityEngine;

namespace Project.Player
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Data")] 
        public float speed = 4;

        [Header("Class References")] 
        public NetworkIdentity networkidentity;

        public void Update()
        {
            if (networkidentity.isControlling) //내꺼면 이동
            {
                Move();
            }
        }

        private void Move() //이동함수
        {
            Vector3 input=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
            transform.position += input * speed * Time.deltaTime;
        }
    }
}
