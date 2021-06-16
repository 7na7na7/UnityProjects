var shortID=require('shortid');
var Vector2=require('./Vector2.js');
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
        this.health=new Number(100); //체력 100으로 할당
        this.isDead=false; //죽었는가?
        this.respawnTicker=new Number(0); //리스폰시간재는거
        this.respawnTime=new Number(0); //리스폰딜레이
    }

    setHpFull()
    {
        this.health=new Number(100);
    }

    respawnCounter()
    {
        this.respawnTicker+=1;

        if(this.respawnTicker>=100)
        {
            this.respawnTicker=new Number(0);
            this.respawnTime=this.respawnTime+1;

            if(this.respawnTime>=3) //리스폰
            {
                console.log('Respawning player id : '+this.id);
                this.isDead=false; //안죽음
                this.respawnTicker=new Number(0);
                this.respawnTime=new Number(0);
                this.setHpFull(); //체력초기화
                this.position=new Vector2(0,0); //리스폰위치

                return true;
            }
        }
        return false;
    }

    dealDamage(amount=Number) //데미지받는함수
    {
        this.health-=amount; //데미지받았는데
        if(this.health<=0) //체력이 0이하라면
        {
            this.isDead=true; //죽은상태로변함
            this.respawnTicker=new Number(0);
            this.respawnTime=new Number(0);
        }

        return this.isDead;
    }
}