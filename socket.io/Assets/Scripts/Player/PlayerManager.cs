using System.Collections;
using System.Collections.Generic;
using Project.Networking;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private const float BARREL_PIVOT_OFFSET = 90.0f;
        
        [Header("Data")] 
        public float speed = 4;
        public float rotation = 60;
        
        [Header("Object References")] 
        public Transform barrelPivot;
        
        [Header("Class References")] 
        public NetworkIdentity networkidentity;

        private float lastRotation;

        public void Update()
        {
            if (networkidentity.isControlling) //내꺼면 이동
            {
                Move();
                checkAiming();
            }
        }
        public float GetLastRotation()
        {
            return lastRotation;
        }
        public void SetRotation(float Value)
        { 
            barrelPivot.rotation=Quaternion.Euler(0,0,Value+BARREL_PIVOT_OFFSET);
        }
        private void Move() //이동함수
        {
            Vector3 input=new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical")); 
            //input.Normalize();
            transform.position += -transform.up * input.y*speed * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, -input.x * rotation * Time.deltaTime));
        }

        private void checkAiming() //각도체크
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
            lastRotation = rot;
            barrelPivot.rotation=Quaternion.Euler(0,0,rot+BARREL_PIVOT_OFFSET);
        }
    }
}
