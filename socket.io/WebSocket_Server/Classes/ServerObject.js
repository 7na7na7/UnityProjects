var shortID=require('shortid');
var Vector2=require('./Vector2.js');

module.exports=class ServerObject
{
    constructor()
    {
        //서버오브젝트 공통항목
        this.id=shortID.generate(); //id만들기
        this.name='ServerObject'; //이름
        this.position=new Vector2(); //위치
    }
}