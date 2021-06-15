var ServerObject =require('./ServerObject.js');
var Vector2=require('./Vector2.js');

module.exports=class Bullet extends ServerObject
{
    constructor()
    {
        super(); //부모 생성자 호출
        this.direction=new Vector2(); //날아가는방향
        this.speed=0.1; //총알속도
    }

    onUpdate() //매프레임 호출, 총알이동
    {
        this.position.x+=this.direction.x*this.speed;
        this.position.y+=this.direction.y*this.speed;
        return false;
    }
}
