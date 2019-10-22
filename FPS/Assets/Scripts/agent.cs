using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class agent : MonoBehaviour
{
    //타겟설정
    public GameObject target;
    private NavMeshAgent navagent;
    void Start()
    {
        navagent=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        navagent.destination = target.transform.position;
    }
}
