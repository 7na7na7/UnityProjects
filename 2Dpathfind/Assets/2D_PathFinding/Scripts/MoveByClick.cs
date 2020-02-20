using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MoveByClick : MonoBehaviour
{
    private int Cx, Cy, Tx, Ty;
    private AStar_PathFinding pf;
    private Camera camera;
    public PhotonView pv;
    public Text nickname;

    private bool count = false;
    void Start()
    {
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName; //닉네임 설정, 자기 닉네임이 아니면 상대 닉네임으로
        nickname.color = pv.IsMine ? Color.green : Color.red; //닉네임 색깔 설정, 자기 닉네임이면 초록색, 아니면 빨강색
        if (pv.IsMine)
        {
            pf = FindObjectOfType<AStar_PathFinding>();
            camera = Camera.main.GetComponent<Camera>();
            //카메라 Cinemachine을 찾아 자신을 따라오도록 함
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
            FindObjectOfType<AStar_PathFinding>().Charactor = gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            print(pf.dir);
            if (pf.dir == Vector3.zero)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
                {
                    Cx = Mathf.RoundToInt(pf.Charactor.transform.position.x);
                    Cy = Mathf.RoundToInt(pf.Charactor.transform.position.y);
                    Tx = Mathf.RoundToInt(pf.Charactor.transform.position.x);
                    Ty = Mathf.RoundToInt(pf.Charactor.transform.position.y);
                    if (Input.GetKey(KeyCode.A))
                        Tx -= 1;
                    if (Input.GetKey(KeyCode.S))
                        Ty -= 1;
                    if (Input.GetKey(KeyCode.D))
                        Tx += 1;
                    if (Input.GetKey(KeyCode.W))
                        Ty += 1;
                }
                {
                    if (Tx >= Cx && Ty >= Cy) //오른쪽 위
                    {
                        pf.PathFinding(new Vector2Int(Cx - 10, Cy - 10), new Vector2Int(Tx + 10, Ty + 10),
                            new Vector2Int(Cx, Cy), new Vector2Int(Tx, Ty));
                    }
                    else if (Tx <= Cx && Ty >= Cy) //왼쪽 위
                    {
                        pf.PathFinding(new Vector2Int(Tx - 10, Cy - 10), new Vector2Int(Cx + 10, Ty + 10),
                            new Vector2Int(Cx, Cy), new Vector2Int(Tx, Ty));
                    }
                    else if (Tx >= Cx && Ty <= Cy) //오른쪽 아래
                    {
                        pf.PathFinding(new Vector2Int(Cx - 10, Ty - 10), new Vector2Int(Tx + 10, Cy + 10),
                            new Vector2Int(Cx, Cy), new Vector2Int(Tx, Ty));
                    }
                    else if (Tx <= Cx && Ty <= Cy) //왼쪽 아래
                    {
                        pf.PathFinding(new Vector2Int(Tx - 10, Ty - 10), new Vector2Int(Cx + 10, Cy + 10),
                            new Vector2Int(Cx, Cy), new Vector2Int(Tx, Ty));
                    }
                }
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.W))
            {
                count = true;
            }
            else
            {
                StartCoroutine(countreset());
            }
            
            if(count)
                GetComponent<Animator>().SetBool("iswalk",true);
            else
                GetComponent<Animator>().SetBool("iswalk",false);
        }
    }

    IEnumerator countreset()
    {
        yield return new WaitForSeconds(0.2f);
        count =false;
    }
}
