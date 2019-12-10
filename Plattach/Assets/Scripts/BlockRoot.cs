using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRoot : MonoBehaviour
{
    public GameObject BlockPrefab = null; //만들어낼 블록의 프리팹
    public BlockControl[,] blocks; //그리드
    void Start()
    {
        initialSetUp();
    }

    //블록을 만들어 내고 가로 9칸, 세로 9칸에 배치한다.
    public void initialSetUp()
    {
        //그리드의 크기를 9x9로 한다.
        blocks=new BlockControl[Block.BLOCK_NUM_X,Block.BLOCK_NUM_Y];
        
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
                Vector3 position = BlockRoot.calcBlockPosition(block.i_Pos);
                
                //씬의 블록 위치를 이동한다.
                block.transform.position = position;
                
                //블록의 색을 변경한다.
                block.setColor((Block.COLOR)color_index);
                
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
        
    }
}
