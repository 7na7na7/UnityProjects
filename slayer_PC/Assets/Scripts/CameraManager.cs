using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public int rot=1;
    public float rotSpeed;
    public float fastrotSpeed;
    public float savedrotSpeed;
    public float delay;
    public float speed = 2f;
    public GameObject target; //카메라가 따라갈 대상
    public GameObject savedTarget;
    public BoxCollider2D bound; //카메라가 나가지 못하는 영역을 박스 콜라이더로 받음

    private Vector3 targetPosition; //대상의 현재 값
    private Vector3 minBound, maxBound; //박스 콜라이더 영역의 최소/최대 xyz값을 지님
    private float halfWidth, halfHeight; //카메라의 반너비, 반높이 값을 지닐 변수
   public Camera theCamera; //카메라의 반높이값을 구할 속성을 이용하기 위한 변수
    public static CameraManager instance;
    public GameObject posGO;
    public bool canFollow = true;
    private void Start()
    {
        instance = this;
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min; //minbound에 box콜라이더의 영역 최솟값 대입
        maxBound = bound.bounds.max;
        savedrotSpeed=rotSpeed;
        //theCamera.orthographicSize *= 2;
    }

    void Update()
    {
        if (target.gameObject != null&&!FindObjectOfType<GameManager>().isGameOver&&canFollow)
        {
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
                //transform.position = targetPosition;


                if (speed == 0)
                    transform.position = targetPosition;
                else
                    transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

                halfHeight = theCamera.orthographicSize;
                halfWidth = halfHeight * Screen.width / Screen.height; //카메라 반너비 공식
                float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);

                float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight,
                    maxBound.y - halfHeight);
//Mathf.Clamp(10,0,100) 일 경우 값은 10,
//Mathf.Clamp(-10,0,100)일 경우 값은 0이다.
                this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);


                if (rot == 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - Time.deltaTime * rotSpeed);
                }
                else if (rot == 1)
                {
                    if(transform.eulerAngles.z>1||transform.eulerAngles.z<-1)
                    {
                        if(transform.eulerAngles.z>1)
                            transform.eulerAngles=new Vector3(0,0,transform.eulerAngles.z-Time.deltaTime*rotSpeed);
                        else
                            transform.eulerAngles=new Vector3(0,0,transform.eulerAngles.z+Time.deltaTime*rotSpeed);
                    }
                    
                }
                else if (rot == 2)
                {
                    transform.eulerAngles=new Vector3(0,0,transform.eulerAngles.z+Time.deltaTime*rotSpeed);
                }
        }
        
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min; //minbound에 box콜라이더의 영역 최솟값 대입
        maxBound = bound.bounds.max;
    }

    public void sizeup()
    {
        StopAllCoroutines();
       StartCoroutine(sizeupCor());
    }

    public void sizedown()
    {
        StopAllCoroutines();
        StartCoroutine(sizedownCor());
    }
    public void gameOver(GameObject g)
    {
        StopAllCoroutines();
        StartCoroutine(gameoverCor(g));
    }

    public IEnumerator sizeupCor()
    {
        while (theCamera.orthographicSize<7)
        {
            theCamera.orthographicSize += 0.1f;
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }
    
    public IEnumerator sizedownCor()
    {
        while (theCamera.orthographicSize>5)
        {
            theCamera.orthographicSize -= 0.1f;
            yield return new WaitForSeconds(delay);
        }
        yield return null;
    }

    public void closeUp()
    {
        StopAllCoroutines();
        StartCoroutine(closeUpCor());
    }
    public void closeUpSlow()
    {
        StopAllCoroutines();
        StartCoroutine(closeUpSlowCor());
    }
    IEnumerator closeUpSlowCor()
    {
        Time.timeScale = 0.1f;
        while (theCamera.orthographicSize>3f)
            {
                theCamera.orthographicSize -= 0.15f;
                yield return new WaitForSeconds(delay*0.1f);
            }
            while (theCamera.orthographicSize <= 5)
            {
                theCamera.orthographicSize += 0.1f;
                yield return new WaitForSeconds(delay);
            }

            Time.timeScale = 1;
            yield return null;
    }
    IEnumerator closeUpCor()
    {
        if (ComboManager.instance.comboCount >= 2)
        {
            while (theCamera.orthographicSize>6.3f)
            {
                theCamera.orthographicSize -= 0.1f;
                yield return new WaitForSeconds(delay*0.1f);
            }
            while (theCamera.orthographicSize <= 7)
            {
                theCamera.orthographicSize += 0.1f;
                yield return new WaitForSeconds(delay);
            }
            theCamera.orthographicSize = 7;
        }
        else
        {
            while (theCamera.orthographicSize>4.3f)
            {
                theCamera.orthographicSize -= 0.1f;
                yield return new WaitForSeconds(delay*0.1f);
            }
            while (theCamera.orthographicSize <= 5)
            {
                theCamera.orthographicSize += 0.1f;
                yield return new WaitForSeconds(delay);
            }
            theCamera.orthographicSize = 5;
        }

        yield return null;
    }
    
    public IEnumerator gameoverCor(GameObject g)
    {
        targetPosition = target.transform.position;
        GameObject.Find("BGM").GetComponent<AudioSource>().Stop();
        Vector3 p = g.transform.position;
        while (theCamera.orthographicSize > 3.5f)
        {
            theCamera.orthographicSize -= 0.2f;
            targetPosition.Set(p.x, p.y, this.transform.position.z);
            //transform.position = targetPosition;


            if (speed == 0)
                transform.position = targetPosition;
            else
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            halfHeight = theCamera.orthographicSize;
            halfWidth = halfHeight * Screen.width / Screen.height; //카메라 반너비 공식
            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);

            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
//Mathf.Clamp(10,0,100) 일 경우 값은 10,
//Mathf.Clamp(-10,0,100)일 경우 값은 0이다.
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
            yield return new WaitForSeconds(delay);
        }

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        FindObjectOfType<GameOverManager>().canGo = true;
        yield return null;
    }

    public void targetChangeFunc(GameObject g)
    {
        StopAllCoroutines();
        Player.instance.StopAllCoroutines();
        StartCoroutine(targetChange(g));
    }
    public IEnumerator targetChange(GameObject g)
    {
        posGO.transform.position = g.transform.position;
        Player.instance.canTouch = false;
        theCamera.orthographicSize = 5;
        target = posGO;
        Time.timeScale = 0.1f;
        while (theCamera.orthographicSize > 3.5f)
        {
            theCamera.orthographicSize -= 0.1f;
            targetPosition.Set(target.transform.position.x, target.transform.position.y, this.transform.position.z);
            //transform.position = targetPosition;


            if (speed == 0)
                transform.position = targetPosition;
            else
                transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            halfHeight = theCamera.orthographicSize;
            halfWidth = halfHeight * Screen.width / Screen.height; //카메라 반너비 공식
            float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);

            float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
//Mathf.Clamp(10,0,100) 일 경우 값은 10,
//Mathf.Clamp(-10,0,100)일 경우 값은 0이다.
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
            yield return new WaitForSeconds(delay);
        }
        target = savedTarget;
        Time.timeScale = 1;
        sizeup();
        Player.instance.canTouch = true;
    }

    public void OnBound()
    {
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height; //카메라 반너비 공식
        float clampedX = Mathf.Clamp(this.transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);

        float clampedY = Mathf.Clamp(this.transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
//Mathf.Clamp(10,0,100) 일 경우 값은 10,
//Mathf.Clamp(-10,0,100)일 경우 값은 0이다.
        this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
    }
}