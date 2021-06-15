using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Utility
{
    public class Cooldown
    {
        private float length;
        private float currentTime;
        private bool onCooldown;
       
        public Cooldown(float Length=1, bool StartWithCooldown=false)
        {
            currentTime=0; 
            length=Length; 
            onCooldown=StartWithCooldown;
        }

        public void CooldownUpdate() //쿨타임감소
        {
            if (onCooldown) //쿨타임 감소중이면
            {
                currentTime += Time.deltaTime;

                if (currentTime >= length) //쿨타임끝나면
                {
                    currentTime = 0; //다시 0으로
                    onCooldown = false;
                }
            }
        }

        public bool IsOnCooldown() //클타임중인지아닌지 
        {
            return onCooldown;
        }

        public void StartCooldown()
        {
            onCooldown = true;
            currentTime = 0;
        }
    }
}
