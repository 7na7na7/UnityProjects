var shortID=require('shortid');
var Vector2=require('./Vector2');
//Player클래스 배출
module.exports=class Player
{
    constructor()
    {
        this.username=''; //플레이어이름
        this.id=shortID.generate(); //식별 ID
        this.position=new Vector2(); //위치
        this.tankRotation=new Number(0); //탱크회전값
        this.barrelRotation=new Number(0); //주포회전값
    }
}