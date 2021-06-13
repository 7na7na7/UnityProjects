var shortID=require('shortid');

//Player클래스 배출
module.exports=class Player
{
    constructor()
    {
        this.username='ASDDWEQWEASD';
        //식별을 위해 도와줌
        this.id=shortID.generate();
    }
}