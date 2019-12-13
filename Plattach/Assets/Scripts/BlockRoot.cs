using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BlockRoot : MonoBehaviour
{
    public GameObject BlockPrefab = null; //만들어낼 블록의 프리팹
    public BlockControl[,] blocks; //그리드

    private GameObject main_camera = null;
    private BlockControl grabbed_block = null;

    void Start()
    {
        main_camera = Camera.main.gameObject;
        //main_camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update() //마우스 좌표와 겹치는지 체크, 잡을 수 있는 블록을 잡음 
    {
        Vector3 mouse_position; //마우스 위치
        unprojectMousePosition(out mouse_position, Input.mousePosition);

        //가져온 마우스 위치를 하나의 Vector2로 모은다.
        Vector2 mouse_position_xy = new Vector2(mouse_position.x, mouse_position.y);

        if (grabbed_block == null) //잡은 블록이 비었다면(잡지 않았다면)
        {
            if(!is_has_falling_block())
            { 
                if (Input.GetMouseButtonDown(0)) //마우스 버튼이 눌렸으면,
            {
                //blocks배열의 모든 요소를 차례로 처리한다.
                foreach (BlockControl block in blocks)
                {
                    if (!block.isGrabable()) //블록을 잡을 수 없다면
                    {
                        continue; //루프의 처음으로 점프한다.
                    }

                    //마우스 위치가 블록 영역 안이 아니면,
                    if (!block.isContainedPosition(mouse_position_xy))
                    {
                        continue; //루프의 처음으로 점프한다.
                    }

                    //처리 중인 블록을 grabbed_block에 등록한다.
                    grabbed_block = block;
                    //잡았을 때의 처리를 실행한다.
                    grabbed_block.beginGrab();
                    break;
                }
            } 
            }
        }
        else //블록을 잡았을 때
        {
            do
            {
                //슬라이드할 곳의 블록을 가져온다.
                BlockControl swap_target = getNextBlock(grabbed_block, grabbed_block.slide_dir);
                //슬라이드할 곳의 블록이 비어 있으면,
                if (swap_target == null)
                    break; //탈출

                //슬라이드할 곳의 블록이 잡을 수 있는 상태가 아니라면,
                if (!swap_target.isGrabable())
                    break; //탈출

                //현재 위치에서 슬라이드 위치까지의 거리를 얻는다.
                float offset = grabbed_block.calcDirOffset(mouse_position_xy, grabbed_block.slide_dir);

                //수리 거리가 블록 크기의 절반보다 작다면,
                if (offset < Block.COLLISION_SIZE / 2.0f)
                    break; //탈출

                //블록을 교체한다.
                swapBlock(grabbed_block, grabbed_block.slide_dir, swap_target);

                grabbed_block = null; //지금은 블록을 잡고 있지 않다.
            } while (false);

            if (!Input.GetMouseButton(0)) //마우스 버튼이 눌려져 있지 않으면,
            {
                grabbed_block.endGrab(); //블록을 놨을 때의 처리를 실행.
                grabbed_block = null; //grabbed_block을 비우게 설정.
            }
        }
        
        //낙하 중 또는 슬라이드 중이면
        if (is_has_falling_block() || is_has_sliding_block())
        {
            //아무것도 하지 않는다.
        }
        else //낙하 중도 슬라이드 중도 아니면
        {
            int ignite_count = 0; //불붙은 개수
            //그리드 안의 모든 블록에 대해서 처리.
            foreach (BlockControl block in blocks)
            {
                if (!block.isIdle()) //대기 중이면 루프의 처음으로 점프하고,
                    continue;

                //세로 또는 가로에 같은 색 블록이 세 개 이상 나열했다면,
                if (checkConnection(block))
                    ignite_count++; //불붙은 개수를 증가.
            }

            if (ignite_count > 0) //불붙은 개수가 0보타 크면(한 군데라도 맞춰진 곳이 있으면),
            {
                int block_count = 0; //불붙는 중인 블록 수

                //그리드 내의 모든 블록에 대해서 처리
                foreach (BlockControl block in blocks)
                {
                    if (block.isVanishing()) //타는 중이면
                        block.rewindVanishTimer(); //다시 점화!
                }
            }
        }
        
        //하나라도 연소 중인 블록이 있는가?
        bool is_vanishing = is_has_vanishing_block();
        //조건이 만족되면 블록을 떨어뜨리고 싶다.
        
        do
        {
            if (is_vanishing) //연소 중인 블록이 있다면
                break;

            if (is_has_sliding_block()) //교체 중인 블록이 있다면
                break;

            for (int x = 0; x < Block.BLOCK_NUM_X; x++)
            {
                //열에 교체 중인 블록이 있다면 그 열은 처리하지 않고 다음 열로 진행된다.
                if (is_has_Sliding_block_in_column(x))
                    continue;

                //그 열에 있는 블록을 위에서부터 검사한다.
                for (int y = 0; y < Block.BLOCK_NUM_Y - 1; y++)
                {
                    //지정 블록이 비표시라면 다음 블록으로
                    if (!blocks[x, y].isVacant())
                        continue;

                    //지정 블록 아래에 있는 블록을 검사
                    for (int y1 = y + 1; y1 < Block.BLOCK_NUM_Y; y1++)
                    {
                        //아래에 있는 블록이 비표시라면 다음 블록으로
                        if (!blocks[x, y].isVacant())
                            continue;

                        //블록을 교체한다.
                        fallBlock(blocks[x, y], Block.DIR4.UP, blocks[x, y1]);
                        break;
                    }
                }
            }
            
            //보충처리
            for (int x = 0; x < Block.BLOCK_NUM_X; x++)
            {
                int fall_start_y = Block.BLOCK_NUM_Y;
                for (int y = 0; y < Block.BLOCK_NUM_Y; y++)
                {
                    //비표시 블록이 아니라면 다음 블록으로
                    if (!blocks[x, y].isVacant())
                    {
                        continue;
                    }

                    blocks[x, y].beginRespawn(fall_start_y); //블록 부활
                    fall_start_y++;
                }
            }
        } while (false);
    }

    //블록을 만들어 내고 가로 9칸, 세로 9칸에 배치한다.
    public void initialSetUp()
    {
        //그리드의 크기를 9x9로 한다.
        blocks = new BlockControl[Block.BLOCK_NUM_X, Block.BLOCK_NUM_Y];

        //블록의 색 번호
        int color_index = 0;

        for (int y = 0; y < Block.BLOCK_NUM_Y; y++) //처음부터 마지막행까지
        {
            for (int x = 0; x < Block.BLOCK_NUM_X; x++) //왼쪽부터 오른쪽까지
            {
                //BlockPrefab의 인스턴스를 씬에 만든다.
                GameObject game_object = Instantiate(BlockPrefab);

                //위에서 만든 블록의 BlockControl클래스를 가져온다.
                BlockControl block = game_object.GetComponent<BlockControl>();

                //블록을 그리드에 저장한다.
                blocks[x, y] = block;

                //블록의 위치 정보(그리드 값)를 설정한다.
                block.i_Pos.x = x;
                block.i_Pos.y = y;
                //각 BlockControl이 연계할 GamesRoot는 자신이라고 설정한다.
                block.block_root = this;

                //그리드 좌표를 실제 위치(씬의 좌표)로 변환한다.
                Vector3 position = calcBlockPosition(block.i_Pos);

                //씬의 블록 위치를 이동한다.
                block.transform.position = position;

                //블록의 색을 변경한다.
                block.setColor((Block.COLOR) color_index);

                //블록의 이름을 설정(후술)한다.
                block.name = "block(" + block.i_Pos.x + "," + block.i_Pos.y + ")";

                //전체 색 중에서 임의로 하나의 색을 설정한다.
                color_index = Random.Range(0, (int) Block.COLOR.NORMAL_COLOR_NUM);
            }
        }
    }

    //지정된 그리드 좌표로 씬에서의 좌표를 구한다.
    public static Vector3 calcBlockPosition(Block.iPosition i_pos)
    {
        //배치할 왼쪽 위 구석 위치를 초깃값으로 설정한다.
        Vector3 position = new Vector3(-(Block.BLOCK_NUM_X / 2.0f - 0.5f), -(Block.BLOCK_NUM_Y / 2.0f - 0.5f), 0.0f);

        //초깃값 + 그리드 좌표 x 블록 크기
        position.x += (float) i_pos.x * Block.COLLISION_SIZE;
        position.y += (float) i_pos.y * Block.COLLISION_SIZE;

        return (position); //씬에서의 좌표를 반환한다.
    }

    public bool
        unprojectMousePosition(out Vector3 world_position, Vector3 mouse_position) //out이 붙은 world_position은 참조를 전달한다.
    {
        bool ret;

        //판을 작성한다. 이 판은 카메라에 대해서 뒤로 향해서(Vector3.back)
        //블록의 절반 크기만큼 앞에 둔다.
        Plane plane = new Plane(Vector3.back, new Vector3(0, 0, -Block.COLLISION_SIZE / 2.0f));

        //카메라와 마우스를 통과하는 빛을 만든다.
        Ray ray = main_camera.GetComponent<Camera>().ScreenPointToRay(mouse_position);

        float depth; //빛의 깊이

        //광선(ray)이 판(plane)에 닿았다면
        if (plane.Raycast(ray, out depth)) //광선이 닿았으면 depth에 정보가 기록된다. out을 쓰지 않으면 전달되지 않는다!(중요)
        {
            //인수 world_position을 마우스 위치로 덮어쓴다.
            world_position = ray.origin + ray.direction * depth;
            ret = true;
        }
        else //닿지 않았다면
        {
            //인수 world_position을 0인 벡터로 덮어쓴다.
            world_position = Vector3.zero;
            ret = false;
        }

        return (ret);
    }

    public BlockControl getNextBlock(BlockControl block, Block.DIR4 dir) //다음블록 위치반환
    {
        BlockControl next_block = null; //슬라이드할 곳의 블록을 여기에 저장
        switch (dir)
        {
            case Block.DIR4.RIGHT:
                if (block.i_Pos.x < Block.BLOCK_NUM_X - 1) //그리드 안이라면
                    next_block = this.blocks[block.i_Pos.x + 1, block.i_Pos.y];
                break;
            
            case Block.DIR4.LEFT:
                if (block.i_Pos.x > 0) //그리드 안이라면
                    next_block = this.blocks[block.i_Pos.x - 1, block.i_Pos.y];
                break;
            
            case Block.DIR4.UP:
                if (block.i_Pos.y < Block.BLOCK_NUM_Y - 1) //그리드 안이라면
                    next_block = this.blocks[block.i_Pos.x, block.i_Pos.y + 1];
                break;
            
            case Block.DIR4.DOWN:
                if (block.i_Pos.y > 0) //그리드 안이라면
                    next_block = this.blocks[block.i_Pos.x, block.i_Pos.y - 1];
                break;
        }

        return (next_block);
    }

    public static Vector3 getDirVector(Block.DIR4 dir) //방향주면 이동값반환
    {
        Vector3 v = Vector3.zero;

        switch (dir)
        {
            case Block.DIR4.RIGHT: //오른쪽이면
                v = Vector3.right; //오른쪽으로 1이동
                break;
            case Block.DIR4.LEFT: //왼쪽이면
                v = Vector3.left; //왼쪽으로 1이동
                break;
            case Block.DIR4.UP: //위쪽이면
                v = Vector3.up; //위쪽으로 1이동
                break;
            case Block.DIR4.DOWN: //아래쪽이면
                v = Vector3.down; //아래쪽으로 1이동
                break;
        }

        v *= Block.COLLISION_SIZE; //블록의 크기를 곱한다.
        return (v);
    }

    public static Block.DIR4 getOppositeDir(Block.DIR4 dir) //반대방향 반환
    {
        Block.DIR4 opposite = dir;
        switch (dir)
        {
            case Block.DIR4.RIGHT: //오른쪽이면
                opposite = Block.DIR4.LEFT; //왼쪽반환
                break;
            case Block.DIR4.LEFT: //왼쪽이면
                opposite = Block.DIR4.RIGHT; //오른쪽반환
                break;
            case Block.DIR4.UP: //위쪽이면
                opposite = Block.DIR4.DOWN; //아래쪽반환
                break;
            case Block.DIR4.DOWN: //아래쪽이면
                opposite = Block.DIR4.UP; //위쪽반환
                break;
        }

        return (opposite);
    }

    public void swapBlock(BlockControl block0, Block.DIR4 dir, BlockControl block1)
    {
        //각각의 블록 색을 기억해 둔다.
        Block.COLOR color0 = block0.color;
        Block.COLOR color1 = block1.color;

        //각각의 블록의 확대율을 기억해 둔다.
        Vector3 scale0 = block0.transform.localScale;
        Vector3 scale1 = block1.transform.localScale;

        //각각의 블록의 '사라지는 시간'을 기억해 둔다.
        float vanish_timer0 = block0.vanish_timer;
        float vanish_timer1 = block1.vanish_timer;

        //각각의 블록의 이동할 곳을 구한다.
        Vector3 offset0 = BlockRoot.getDirVector(dir);
        Vector3 offset1 = BlockRoot.getDirVector(BlockRoot.getOppositeDir(dir));

        //색을 교체한다.
        block0.setColor(color1);
        block1.setColor(color0);

        //확대율을 교체한다.
        block0.transform.localScale = scale1;
        block1.transform.localScale = scale0;

        //'사라지는 시간'을 교체한다.
        block0.vanish_timer = vanish_timer1;
        block1.vanish_timer = vanish_timer0;

        block0.beginSlide(offset0); //원래 블록 이동을 시작한다.
        block1.beginSlide(offset1); //이동할 위치의 블록 이동을 시작한다.
    }

    public bool checkConnection(BlockControl start)
    {
        bool ret = false;
        int normal_block_num = 0;

        //인수인 블록이 불붙은 다음이 아니라면
        if (!start.isVanishing())
            normal_block_num = 1;

        //그리드 좌표를 기억해 둔다.
        int rx = start.i_Pos.x;
        int lx = start.i_Pos.x;

        //블록의 왼쪽을 검사
        for (int x = lx - 1; x > 0; x--)
        {
            BlockControl next_block = blocks[x, start.i_Pos.y];
            if (next_block.color != start.color) //색이 다르면
                break; //루프를 빠져나간다.

            if (next_block.step == Block.STEP.FALL || next_block.next_step == Block.STEP.FALL) //낙하 중이면
                break; //루프를 빠져나간다

            if (next_block.step == Block.STEP.SLIDE || next_block.next_step == Block.STEP.SLIDE) //슬라이드 중이면
                break; //루프를 빠져나간다

            if (!next_block.isVanishing()) //불붙은 상태가 아니면
                normal_block_num++; //검사용 카운터를 증가

            lx = x;
        }

        //블록의 오른쪽을 검사
        for (int x = rx + 1; x < Block.BLOCK_NUM_X; x++)
        {
            BlockControl next_block = blocks[x, start.i_Pos.y];
            if (next_block.color != start.color) //색이 다르면
                break; //루프를 빠져나간다.

            if (next_block.step == Block.STEP.FALL || next_block.next_step == Block.STEP.FALL) //낙하 중이면
                break; //루프를 빠져나간다

            if (next_block.step == Block.STEP.SLIDE || next_block.next_step == Block.STEP.SLIDE) //슬라이드 중이면
                break; //루프를 빠져나간다

            if (!next_block.isVanishing()) //불붙은 상태가 아니면
                normal_block_num++; //검사용 카운터를 증가

            rx = x;
        }

        do
        {
            if (rx - lx + 1 < 3) //오른쪽 블록의 그리드 번호 - 왼쪽 블록의 그리드 번호 + 중앙 블록(1)을 더한 수가 3미만이면,
                break; //루프 탈출
            if (normal_block_num == 0) //불붙지 않은 블록이 하나도 없으면,
                break; //루프탈출
            for (int x = lx; x < rx + 1; x++)
            {
                //나열된 같은 색 블록을 불붙은 상태로
                blocks[x, start.i_Pos.y].toVanishing();
                ret = true;
            }
        } while (false);

        normal_block_num = 0;
        if (!start.isVanishing())
            normal_block_num = 1;

        int uy = start.i_Pos.y;
        int dy = start.i_Pos.y;

        //블록의 위쪽을 검사
        for (int y = dy - 1; y > 0; y--)
        {
            BlockControl next_block = blocks[start.i_Pos.x, y];
            if (next_block.color != start.color)
                break;
            if (next_block.step == Block.STEP.FALL || next_block.next_step == Block.STEP.FALL)
                break;
            if (next_block.step == Block.STEP.SLIDE || next_block.next_step == Block.STEP.SLIDE)
                break;

            if (!next_block.isVanishing())
                normal_block_num++;

            dy = y;
        }
        //블록의 아래쪽을 검사
        for (int y = uy + 1; y < Block.BLOCK_NUM_Y; y++)
        {
            BlockControl next_block = blocks[start.i_Pos.x, y];
            if (next_block.color != start.color)
                break;
            if (next_block.step == Block.STEP.FALL || next_block.next_step == Block.STEP.FALL)
                break;
            if (next_block.step == Block.STEP.SLIDE || next_block.next_step == Block.STEP.SLIDE)
                break;

            if (!next_block.isVanishing())
                normal_block_num++;

            uy = y;
        }

        do
        {
            if (uy - dy + 1 < 3)
                break;
            if (normal_block_num == 0)
                break;
            for (int y = dy; y < uy + 1; y++)
            {
                blocks[start.i_Pos.x, y].toVanishing();
                ret = true;
            }
        } while (false);
        
        return ret;
    }

    private bool is_has_vanishing_block() //불붙는 중인 블록이 하나라도 있으면 true반환
    {
        bool ret = false;
        foreach (BlockControl block in blocks)
        {
            if (block.vanish_timer > 0.0f)
            {
                ret = true;
                break;
            }
        }

        return (ret);
    }

    private bool is_has_sliding_block() //슬라이딩 중인 블록이 하나라도 있으면 true를 반환
    {
        bool ret = false;
        foreach (BlockControl block in blocks)
        {
            if (block.step == Block.STEP.SLIDE)
            {
                ret = true;
                break;
            }
        }

        return (ret);
    }

    private bool is_has_falling_block() //떨어지는 중인 블록이 하나라도 있으면 true반환
    {
        bool ret = false;
        foreach (BlockControl block in blocks)
        {
            if (block.step == Block.STEP.FALL)
            {
                ret = true;
                break;
            }
        }

        return (ret);
    }


    public void fallBlock(BlockControl block0, Block.DIR4 dir, BlockControl block1)
    {
        //block0과 block1의 색, 크기, 사라질 때까지의 시간, 표시, 비표시, 상태를 기록
        Block.COLOR color0 = block0.color;
        Block.COLOR color1 = block1.color;

        Vector3 scale0 = block0.transform.localScale;
        Vector3 scale1 = block1.transform.localScale;

        float vanish_timer0 = block0.vanish_timer;
        float vanish_timer1 = block1.vanish_timer;

        bool visible0 = block0.isVIsible();
        bool visible1 = block1.isVIsible();

        Block.STEP step0 = block0.step;
        Block.STEP step1 = block1.step;
        
        //block0과 block1의 각종 속성을 교체.
        block0.setColor(color1);
        block1.setColor(color0);

        block0.transform.localScale = scale1;
        block1.transform.localScale = scale0;

        block0.vanish_timer = vanish_timer1;
        block1.vanish_timer = vanish_timer0;

        block0.setVisible(visible1);
        block1.setVisible(visible0);

        block0.step = step1;
        block1.step = step0;
        
        block0.beginFall(block1); //떨어지기시작
    }

    private bool is_has_Sliding_block_in_column(int x)
    {
        bool ret = false;

        for (int y = 0; y < Block.BLOCK_NUM_Y; y++)
        {
            if (blocks[x, y].isSliding()) //슬라이드 중인 블록이 있으면,
            {
                ret = true; //true를 반환
                break;
            }
        }

        return (ret);
    }
}
