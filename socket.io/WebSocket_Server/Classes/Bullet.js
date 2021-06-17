var ServerObject =require('./ServerObject.js');
var Vector2=require('./Vector2.js');

module.exports=class Bullet extends ServerObject
{
    constructor()
    {
        super(); //부모 생성자 호출
        this.direction=new Vector2(); //날아가는방향
        this.speed=0.75; //총알속도
        this.isDestroyed=false; //파괴되었는가?
        this.activator=''; //닿은놈 이름
    }

    onUpdate() //index에서 해당 밀리세컨드마다 호출, 총알이동
    {
        this.position.x+=this.direction.x*this.speed;
        this.position.y+=this.direction.y*this.speed; 
        return this.isDestroyed;
    }
}
