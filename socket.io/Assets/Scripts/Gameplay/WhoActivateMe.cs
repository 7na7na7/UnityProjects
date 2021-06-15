using System.Collections;
using System.Collections.Generic;
using Project.Utility.Attributes;
using UnityEngine;

namespace Project.Gameplay
{
    public class WhoActivateMe : MonoBehaviour
    {
        [GreyOut]
        private string whoActivateMe; //총알쏜사람 ID저장

        public void SetActivator(string ID)
        {
            whoActivateMe = ID;
        }

        public string GetActivator()
        {
            return whoActivateMe;
        }
    }
}
