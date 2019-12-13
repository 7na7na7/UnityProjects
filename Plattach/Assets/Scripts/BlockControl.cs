using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Block
{
    public static float COLLISION_SIZE = 1.0f; //블록의 충돌 크기
    public static float VANISH_TIME = 3.0f; //불 붙고 사라질 때까지의 시간

    public struct iPosition //그리드에서의 좌표를 나타내는 구조체
    {
        public int x; //X좌표
        public int y; //Y좌표
    }


    public enum COLOR //블록 색상
    {
        NONE = -1, //색 지정 없음
        PINK = 0, //분홍색
        BLUE,
        YELLOW,
        GREEN,
        MAGENTA, //마젠타(색)
        ORANGE,
        GRAY,
        NUM,
        FIRST = PINK, //초기 컬러(분홍색)
        LAST = ORANGE, //최종 컬러(주황색)
        NORMAL_COLOR_NUM = GRAY, //보통 컬러(회색 이외의 색)의 수
    };

    public enum DIR4 //상하좌우 네 방향
    {
        NONE = -1, //방향지정 없음
        RIGHT, //우
        LEFT, //좌
        UP, //상
        DOWN, //하
        NUM, //방향이 몇 종류 있는지 나타낸다(=4)
    };

    public enum STEP //블록의 상태 표시
    { 
        NONE=-1,//상태 정보 없음
        IDLE=0,//대기 중
        GRABBED,//잡혀 있음
        RELEASED,//떨어진 순간
        SLIDE,//슬라이드 중
        VACANT,//소멸 중
        RESPAWN,//재생성 중
        FALL,//낙하 중
        LONG_SLIDE,//크게 슬라이드 중
        NUM,//상태가 몇 종류인지 표시
    }
    public static int BLOCK_NUM_X = 9; //블록을 배치할 수 있는 X방향 최대수.
    public static int BLOCK_NUM_Y = 9; //블록을 배치할 수 있는 Y방향 최대수.
}

public class BlockControl : MonoBehaviour
{
    public struct StetpFall
    {
        public float velocity; //낙하 속도
    }
    private StetpFall fall;
    
    public Block.COLOR color = (Block.COLOR) 0; //블록 색
    public BlockRoot block_root = null; //블록의 신
    public Block.iPosition i_Pos; //블록 좌표
    
    public Block.STEP step = Block.STEP.NONE; //지금 상태
    public Block.STEP next_step = Block.STEP.NONE; //다음 상태
    private Vector3 position_offset_initial = Vector3.zero; //교체 전 위치
    public Vector3 position_offset = Vector3.zero; //교체 후 위치

    public float vanish_timer = -1.0f; //블록이 사라질 때까지의 시간
    public Block.DIR4 slide_dir = Block.DIR4.NONE; //슬라이드된 방향
    public float step_timer = 0.0f; //블록이 교체된 때의 이동시간 등

    public Material opague_material; //불투명 메테리얼
    public Material transparent_material; //반투명 메테리얼
    
    void Start()
    {
        setColor(color); //색칠을 한다.
        next_step = Block.STEP.IDLE; //다음 블록을 대기중으로
    }

    private void Update()
    {
        Vector3 mouse_position; //마우스 위치
        block_root.unprojectMousePosition(out mouse_position, Input.mousePosition); //마우스 위치 획득
        
        //획득한 마우스 위치를 X와 Y만으로 한다.
        Vector2 mouse_position_xy=new Vector2(mouse_position.x, mouse_position.y);

        if (vanish_timer >= 0.0f) //타이머가 0이상이면
        {
            vanish_timer -= Time.deltaTime; //타이머의 값을 줄인다.
            if (vanish_timer < 0.0f) //타이머가 0미만이면,
            {
                if (step != Block.STEP.SLIDE) //슬라이드 중이 아니라면,
                {
                    vanish_timer = -1.0f;
                    next_step = Block.STEP.VACANT; //상태를 '소멸 중'으로
                }
                else //타이머의 값이 0이상이면,
                {
                    vanish_timer = 0.0f;
                }
            }
        }

        step_timer += Time.deltaTime;
        float slide_time = 0.2f;

        if (next_step == Block.STEP.NONE) //'상태 정보 없음' 의 경우
        {
            switch (step)
            {
                case Block.STEP.SLIDE: //블록이 슬라이드 중이라면
                    if (step_timer >= slide_time) 
                    {
                        //슬라이드 중인 블록이 소멸되면 VACANT(사라짐) 상태로 이행
                        if (vanish_timer == 0.0f)
                        {
                            next_step = Block.STEP.VACANT;
                        }
                        //vanish_timer가 0이 아니면 IDLE(대기) 상태로 이행
                        else
                            next_step = Block.STEP.IDLE;
                    }
                    break;
                case Block.STEP.IDLE:
                    GetComponent<Renderer>().enabled = true; //보이도록
                    break;
                case Block.STEP.FALL:
                    if (position_offset.y <= 0.0f)
                    {
                        next_step = Block.STEP.IDLE;
                        position_offset.y = 0.0f;
                    }
                    break;
            }
        }
        //'다음 븝록' 상태가 '정보 없음' 이외인 동안,
        //='다음 블록' 상태가 변경된 경우,
        while (next_step!=Block.STEP.NONE)
        {
            step = next_step;
            next_step = Block.STEP.NONE;

            int color_index;
            
            switch (step)
            {
                case Block.STEP.IDLE: //'대기' 상태
                    position_offset=Vector3.zero; //블록 표시 크기를 보통 크기로 한다.
                    transform.localScale = Vector3.one * 1.0f;
                    break;
                case Block.STEP.GRABBED: //'잡힌' 상태
                    transform.localScale = Vector3.one * 1.2f;//블록 표시 크기를 크게 한다.
                    break;
                case Block.STEP.RELEASED: //'떨어져 있는' 상태
                    position_offset = Vector3.zero;
                    transform.localScale = Vector3.one * 1.0f; //블록 표시 크기를 보통 크기로 한다.
                    break;
                case Block.STEP.VACANT: //'사라지는' 상태
                    position_offset = Vector3.zero;
                    setVisible(false);
                    break;
                case Block.STEP.RESPAWN:
                    //색을 랜덤하게 설정하여 블록을 그 색으로 설정
                    color_index = Random.Range(0, (int) Block.COLOR.NORMAL_COLOR_NUM);
                    setColor((Block.COLOR)color_index);
                    next_step = Block.STEP.IDLE;
                    break;
                case Block.STEP.FALL:
                    if (isVIsible() == false)
                    {
                        setVisible(true); //블록을 보이게 표시
                        color_index = Random.Range(0, (int) Block.COLOR.NORMAL_COLOR_NUM);
                        setColor((Block.COLOR)color_index);
                    }
                    fall.velocity = 0.0f; //낙하 속도 리셋
                    break;
            }
            
            step_timer = 0.0f;
        }

        switch (step)
        {
            case Block.STEP.GRABBED: //잡힌 상태
                //잡힌 상태일 때는 항상 슬라이드 방향을 체크
                slide_dir = calcSliderDir(mouse_position_xy);
                break;
            case Block.STEP.SLIDE: //슬라이드(교체) 중
                //블록을 서서히 이동하는 처리
                float rate = step_timer / slide_time;
                rate = Mathf.Min(rate, 1.0f); //rate최솟값 1로 제한
                rate = Mathf.Sin(rate * Mathf.PI / 2.0f); //???
                position_offset = Vector3.Lerp(position_offset_initial, Vector3.zero, rate);
                break;
            case Block.STEP.FALL: //떨어지는 중
                //속도에 중력의 영향을 부여한다.
                fall.velocity += Physics.gravity.y * Time.deltaTime * 0.3f;
                //세로방향 위치를 계산
                position_offset.y += fall.velocity * Time.deltaTime;
                if (position_offset.y < 0.0f) //다 내려왔다면,
                    position_offset.y = 0.0f; //그 자리에 머무른다.
                break;
        }
        //그리드 좌표를 실제 좌표(씬의 좌표)로 변환하고,
        //position_offset을 추가한다.
        Vector3 position = BlockRoot.calcBlockPosition(i_Pos) + position_offset;
        
        //실제 위치를 새로운 위치로 변경한다.
        transform.position = position;
        
        setColor(color);

        if (vanish_timer >= 0.0f)
        {
            //현재 색과 흰색의 중간 색
            Color color0 = Color.Lerp(GetComponent<Renderer>().material.color, Color.white, 0.5f);
            //현재 색과 검은색의 중간 색
            Color color1 = Color.Lerp(GetComponent<Renderer>().material.color, Color.black, 0.5f);
            
            //불붙는 연출 시간이 절반을 지났다면,
            if (vanish_timer < Block.VANISH_TIME / 2.0f)
            {
                //투명도(a)를 설정
                color0.a = vanish_timer / (Block.VANISH_TIME / 2.0f);
                color1.a = color0.a;
                
                //반투명 메테리얼 적용
                GetComponent<Renderer>().material = transparent_material;
            }
            //vanish_timer가 줄어들수록 1에 가까워진다.
            float rate = 1.0f - vanish_timer / Block.VANISH_TIME;
            
            //서서히 색을 바꾼다.
            GetComponent<Renderer>().material.color = Color.Lerp(color0, color1, rate);
        }
    }

    //인수 color의 색으로 블록을 칠한다.
    public void setColor(Block.COLOR color)
    {
        this.color = color; //이번에 지정된 색을 멤버 변수에 보관한다.
        Color color_value; //Color클래스는 색을 나타낸다.
        switch (this.color)
        {
            default:
            case Block.COLOR.PINK:
                color_value=new Color(1.0f,0.5f,0.5f);
                break;
            case Block.COLOR.BLUE:
                color_value=Color.blue;
                break;
            case Block.COLOR.YELLOW:
                color_value=Color.yellow;
                break;
            case Block.COLOR.GREEN:
                color_value=Color.green;
                break;
            case Block.COLOR.MAGENTA:
                color_value=Color.magenta;
                break;
            case Block.COLOR.ORANGE:
                color_value=new Color(1.0f,0.46f,0.0f);
                break;
        }
        //이 게임 오브젝트의 메테리얼 색상을 변경한다.
        GetComponent<Renderer>().material.color = color_value;
    }

    public void beginGrab() 
    {
        next_step = Block.STEP.GRABBED;
    }

    public void endGrab()
    {
        next_step = Block.STEP.IDLE;
    }

    public bool isGrabable()
    {
        bool is_grabbable = false;
        switch (step)
        {
            case Block.STEP.IDLE: //'대기' 상태일 때만
                is_grabbable = true; //true(잡을 수 있다)를 반환한다.
                break;
        }
        
        return is_grabbable;
    }

    public bool isContainedPosition(Vector2 position)
    {
        bool ret = false;
        Vector3 center = transform.position;
        float h = Block.COLLISION_SIZE / 2.0f;
        do
        {
            //X좌표가 자신과 겹치지 않으면 break로 루프를 빠져나간다.
            if (position.x < center.x - h || center.x + h < position.x)
            {
                break;
            }

            //Y좌표가 자신과 겹치지 않으면 break로 루프를 빠져 나간다.
            if (position.y < center.y - h || center.y + h < position.y)
            {
                break;
            }

            //X좌표,Y좌표 모두 겹쳐 있으면 true(겹쳐 있다)를 반환합니다.
            ret = true;
        } while (false);

        return (ret);
    }

    public Block.DIR4 calcSliderDir(Vector2 mouse_position)
    {
        Block.DIR4 dir = Block.DIR4.NONE;
        
        //지정된 mouse_position과 현재 위치의 차를 나타내는 벡터
        Vector2 v = mouse_position - new Vector2(transform.position.x, transform.position.y);
        
        //벡터의 크기가 0.1보다 크면,
        //(그보다 작으면 슬라이드하지 않은 걸로 간주한다.)
        if (v.magnitude > 0.1f)
        {
            if (v.y > v.x)
            {
                if(v.y>-v.x) 
                    dir = Block.DIR4.UP;
                else
                    dir = Block.DIR4.LEFT;
            }
            else
            {

                if (v.y > -v.x)
                    dir = Block.DIR4.RIGHT;
                else
                    dir = Block.DIR4.DOWN;
            }
        }
        return(dir);
    }

    public float calcDirOffset(Vector2 position, Block.DIR4 dir)
    {
        float offset = 0.0f;
        //지정된 위치와 블록의 현재 위치의 차를 나타내는 벡터
        Vector2 v = position - new Vector2(transform.position.x, transform.position.y);

        switch (dir) //지정된 방향에 따라 달라진다.
        {
            case Block.DIR4.RIGHT: 
                offset = v.x;
                break;
            case Block.DIR4.LEFT:
                offset = -v.x;
                break;
            case Block. DIR4.UP:
                offset = v.y;
                break;
            case Block.DIR4.DOWN:
                offset = -v.y;
                break;
        }

        return(offset);
    }

    public void beginSlide(Vector3 offset) //슬라이드 시작할 때 호출
    {
        position_offset_initial = offset;
        position_offset = position_offset_initial;
        //상태를 SLIDE로 변경
        next_step = Block.STEP.SLIDE;
    }

    public void toVanishing() //블록을 지우기 시작할 때, BlockRoot클래스의 checkConnection()에서 호출
    {
        //'사라질 때까지 걸리는 시간'을 규정값으로 리셋
        vanish_timer = Block.VANISH_TIME;
    }

    public bool isVanishing() //블록이 지워지는 중이면 true를 반환, BlockRoot클래스의 checkConnection()에서 호출
    {
        //vanish_timer가 0보타 크면 true
        bool is_vanishing = vanish_timer > 0.0f;
        return (is_vanishing);
    }

    public void rewindVanishTimer() //사라질 때까지 걸리는 시간을 리셋, BlockRoot클래스의 Update()에서 호출
    {
        //'사라질 때까지 걸리는 시간'을 규정값으로 리셋
        vanish_timer = Block.VANISH_TIME;
    }

    public bool isVIsible() //블록이 표시되고 있을 때 true를 반환한다. BlockRoot클래스의 fallBlock()에서 호출
    {
        //그리기 가능(renderer.enabled가 true)상태라면,
        //표시되고 있다.
        bool is_visible = GetComponent<Renderer>().enabled;
        return (is_visible);
    }

    public void setVisible(bool is_visible) //visible설정
    {
        //그리기 가능 설정에 인수를 대입
        GetComponent<Renderer>().enabled = is_visible;
    }
    //BlockControl의 Update(), BlockRoot의 fallBlock()에서 호출
    public bool isIdle() //인수에 true를 지정하면 블록이 표시되고 false를 지정하면 블록이 표시되지 않음. 
    {
        bool is_idle = false;
        //현재 블록 상태가 '대기 중'이고
        //다음 블록 상태가 '없음'이면,
        if (step == Block.STEP.IDLE && next_step == Block.STEP.NONE)
        {
            is_idle = true;
        }

        return (is_idle);
    }

    public void beginFall(BlockControl start) //낙하 시작 처리
    {
        next_step = Block.STEP.FALL;
        //지정된 블록에서 좌표를 계산해 낸다.
        position_offset.y = (float) (start.i_Pos.y - i_Pos.y) * Block.COLLISION_SIZE;
    }

    public void beginRespawn(int start_iPos_y) //색을 바꿔 낙하 처리를 하고 지정한 위치에 재배치한다.
    {
        //지정 위치까지 y좌표를 이동한다.
        position_offset.y = (float) (start_iPos_y - i_Pos.y) * Block.COLLISION_SIZE;

        next_step = Block.STEP.FALL;
        int color_index = Random.Range((int) Block.COLOR.FIRST, (int) Block.COLOR.LAST + 1);
        setColor((Block.COLOR)color_index);
    }

    public bool isVacant() //블록이 비표시(그리드상의 위치가 비어 있을 때)로 되어 있다면 true를 반환한다.
    {
        bool is_vacant = false;
        if (step == Block.STEP.VACANT && next_step == Block.STEP.NONE)
            is_vacant = true;

        return (is_vacant);
    }

    public bool isSliding() //교체 중(슬라이드 중)이면 true를 반환한다.
    {
        bool is_sliding = position_offset.x != 0.0f;
        return(is_sliding);
    }
}
