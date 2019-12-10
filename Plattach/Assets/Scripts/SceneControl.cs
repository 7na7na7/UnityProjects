using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
    
    private BlockRoot block_root = null;
    void Start()
    {
        //BlockRoot스크립트를 가져온다.
        block_root = gameObject.GetComponent<BlockRoot>();
        
        //BlockRoot스크립트의 initialSetUp()을 호출한다.
        block_root.initialSetUp();

    }
}
