using System;
using System.Collections;
using System.Collections.Generic;
using Project.Networking;
using Project.Utility;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Player
{
    public class PlayerManager : MonoBehaviour
    {
        private const float BARREL_PIVOT_OFFSET = 90.0f;

        [Header("Data")] 
        public float moveSpeed;
        public float rotateSpeed;
        public float rotation = 60;
        
        [Header("Object References")] 
        public Transform barrelPivot;
        public Transform bulletSpawnPoint;
        [Header("Class References")] 
        public NetworkIdentity networkidentity;

        private float lastRotation;
        
        //총알발싸
        private Cooldown shootingCooldown;
        private BulletData bulletData;
        
        public void Start()
        {
            //쿨다운 기본값은 1초, 시작시 쿨 안돔
            shootingCooldown = new Cooldown(1);
            bulletData=new BulletData();
            bulletData.position=new Position();
            bulletData.direction=new Position();
        }

        public void Update()
        {
            if (networkidentity.isControlling) //내꺼면 이동
            {
                Move();
                CheckAiming();
                CheckShooting();
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
            transform.position += -transform.up * input.y*moveSpeed * Time.deltaTime;
            transform.Rotate(new Vector3(0, 0, -input.x *rotateSpeed* rotation * Time.deltaTime));
        }

        private void CheckAiming() //각도체크
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dif = mousePosition - transform.position;
            dif.Normalize();
            float rot = Mathf.Atan2(dif.y, dif.x) * Mathf.Rad2Deg;
            lastRotation = rot;
            barrelPivot.rotation=Quaternion.Euler(0,0,rot+BARREL_PIVOT_OFFSET);
        }

        void CheckShooting() //총알발사
        {
             shootingCooldown.CooldownUpdate();
             //좌클릭했고 쿨이안돌았으면
             if (Input.GetMouseButton(0)&&!shootingCooldown.IsOnCooldown())
             {
                 shootingCooldown.StartCooldown(); //쿨돔
                 
                 //총알위치, 방향넣어줌
                 bulletData.position.x = bulletSpawnPoint.position.x.TwoDecimals();
                 bulletData.position.y = bulletSpawnPoint.position.y.TwoDecimals();
                 bulletData.direction.x = -bulletSpawnPoint.up.x;
                 bulletData.direction.y = -bulletSpawnPoint.up.y;
                 
                 //동기화, 서버에 fireBullet호출하라고 보냄
                 networkidentity.GetSocket().Emit("fireBullet",new JSONObject(JsonUtility.ToJson(bulletData)));
             }
        }
    }
}
