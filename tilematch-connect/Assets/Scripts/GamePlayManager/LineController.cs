using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    public GameObject prefabLine;
    public static LineController Instance;
    public Transform worldForeground;
    public float destroyTime = 0.5f;

    public void Init(List<Node> nodes, Color lineColor)
    {
        foreach (Node node in nodes)
        {
            
            GameObject _go = Instantiate(
                                prefabLine
                            );

            _go.GetComponent<LineRenderer>().startColor = lineColor;
            _go.GetComponent<LineRenderer>().endColor = lineColor;

            if (node.prev != null)
            {
                float offsetX = FieldDesignManager.fieldXOffset;
                float offsetY = FieldDesignManager.fieldYOffset;
                // 생성 전 세팅
                _go.GetComponent<LineRenderer>().SetPosition(0, new Vector3(node.x + offsetX, node.y + offsetY, 0));
                _go.GetComponent<LineRenderer>().SetPosition(1, new Vector3(node.prev.x + offsetX, node.prev.y + offsetY, 0));
            }
            else
            {
                //_go.GetComponent<LineRenderer>().SetPosition(0, new Vector3(0, 0, 0));
                //_go.GetComponent<LineRenderer>().SetPosition(1, new Vector3(0, 0, 0));
                _go.SetActive(false);
            }

            // worldForeground에 생성한다.
            _go.GetComponent<Transform>().SetParent(worldForeground);

            
            Destroy(_go, destroyTime); // todo: 이후에 삭제 이펙트 및 모션에 대해서 고민해야 함
        }
        
    }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }        
    }

}
