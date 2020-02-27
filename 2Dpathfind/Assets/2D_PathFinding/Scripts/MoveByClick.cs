using System.Collections;
using Photon.Pun; //Pun네트워크 사용
using UnityEngine;
using UnityEngine.UI; //UI사용
using Random = UnityEngine.Random; //랜덤 사용
using Cinemachine; //2D카메라 Cinemachine 사용

public class MoveByClick : MonoBehaviourPunCallbacks, IPunObservable //변수 동기화를 위해 IPunObservable상속받음
{
    public int score = 0;
    private int Cx, Cy, Tx, Ty;
    private AStar_PathFinding pf;
    public PhotonView pv;
    public Text nickname;
    public AnimatorOverrideController coronaAnim;
    public RuntimeAnimatorController playerAnim;
    public StackScript stack;
    private bool count = false;
    private Vector3 curPos; //부드러운 위치조정
    private int r;
    private int isright = 0, isup = 2;
    public float shotCool;
    private float cool = 0;
    private Transform min, max;
    void Start()
    {
        min = GameObject.Find("Min").transform;
        max = GameObject.Find("Max").transform;
        nickname.text = pv.IsMine ? PhotonNetwork.NickName : pv.Owner.NickName; //닉네임 설정, 자기 닉네임이 아니면 상대 닉네임으로
        nickname.color = pv.IsMine ? Color.white : Color.yellow; //닉네임 색깔 설정, 자기 닉네임이면 초록색, 아니면 빨강색
        if (pv.IsMine)
        {
            transform.position=new Vector3((int)Random.Range(min.position.x,max.position.x),(int)Random.Range(min.position.y,max.position.y),0);
            StartCoroutine(ScoreUp());
            StartCoroutine(startSuper());
            pf = FindObjectOfType<AStar_PathFinding>();
            //카메라 Cinemachine을 찾아 자신을 따라오도록 함
            var CM = GameObject.Find("CMCamera").GetComponent<CinemachineVirtualCamera>();
            CM.Follow = transform;
            CM.LookAt = transform;
            FindObjectOfType<AStar_PathFinding>().Charactor = gameObject;


            int gaycount = 0;
            int playercount = 0;
            MoveByClick[] mvs = FindObjectsOfType<MoveByClick>();
            foreach (MoveByClick ms in mvs)
            {
                if (ms.gameObject.CompareTag("Gay"))
                {
                    gaycount++;
                }
                else if (ms.gameObject.CompareTag("Player"))
                {
                    playercount++;
                }
            }

            if (playercount>gaycount*5) //플레이어 수가 6명 이상이면 좀비2개소환
            {
                pv.RPC("gayRPC", RpcTarget.AllBuffered);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pv.IsMine)
        {
            if(cool<shotCool) 
                cool += Time.deltaTime;
            //print(pf.dir);
            if (pf.dir == Vector3.zero)
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W))
                {
                    Cx = Mathf.RoundToInt(pf.Charactor.transform.position.x);
                    Cy = Mathf.RoundToInt(pf.Charactor.transform.position.y);
                    Tx = Mathf.RoundToInt(pf.Charactor.transform.position.x);
                    Ty = Mathf.RoundToInt(pf.Charactor.transform.position.y);
                    if (Input.GetKey(KeyCode.A))
                    {
                        Tx -= 1;
                        isright = 2;
                        isup = 0;
                    }

                    else if (Input.GetKey(KeyCode.S))
                    {
                        Ty -= 1;
                        isright = 0;
                        isup = 2;
                    }

                   else if (Input.GetKey(KeyCode.D))
                    {
                        Tx += 1;
                        isright = 1;
                        isup = 0;
                    }

                    else if (Input.GetKey(KeyCode.W))
                    {
                        Ty += 1;
                        isup = 1;
                        isright = 0;
                    }
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

            if (gameObject.CompareTag("Player"))
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    pv.RPC("bulletRPC",RpcTarget.AllBuffered);
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
        else if ((transform.position - curPos).sqrMagnitude >= 10)
        {
            transform.position = curPos;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
        }
    }

    IEnumerator countreset()
    {
        yield return new WaitForSeconds(0.2f);
        count =false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Gay") && gameObject.CompareTag("Player")) //게이한테 닿으면
        {
            pv.RPC("scorezero", RpcTarget.AllBuffered);
            pv.RPC("gayRPC", RpcTarget.AllBuffered);
            for(int i=0;i<30;i++) 
                other.GetComponent<MoveByClick>().pv.RPC("scoreUp", RpcTarget.AllBuffered);
        }
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(score);
        }
        else
        {
            curPos = (Vector3) stream.ReceiveNext();
            score = (int) stream.ReceiveNext();
        }
    }
    [PunRPC]
    public void gayRPC()
    {
        GetComponent<Animator>().runtimeAnimatorController = coronaAnim;
        gameObject.tag = "Gay";
    }
    [PunRPC]
    public void manRPC()
    {
        GetComponent<Animator>().runtimeAnimatorController = playerAnim;
        gameObject.tag = "Player";
    }

    [PunRPC]
    public void bulletRPC()
    {
        if (shotCool < cool)
        {
            int plus = 0;
            if (isright == 0)
            {
                if (isup == 1)
                {
                    plus = 90;
                }
                else
                {
                    plus = -90;
                }
            }

            if (isup == 0)
            {
                if (isright == 1)
                {
                }
                else
                {
                    plus = -180;
                }
            }

            GameObject b = PhotonNetwork.Instantiate("bullet", transform.position,  Quaternion.Euler(0, 0,135 + plus));
            cool = 0;
        }
    }
    IEnumerator startSuper()
    {
        yield return new WaitForSeconds(1f);
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    public IEnumerator invisible()
    {
        Color color;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>(); //스프라이트로 함
        color.r = 255;
        color.g = 255;
        color.b = 255;
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 0.5f;
        sprite.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1f;
        sprite.color = color;
        yield return new WaitForSeconds(0.1f);
    }

    IEnumerator ScoreUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (gameObject.CompareTag("Player"))
                pv.RPC("scoreUp", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void scoreUp()
    {
        score += 100;
    }
    [PunRPC]
    public void scorezero()
    {
        score = 0;
    }
}
