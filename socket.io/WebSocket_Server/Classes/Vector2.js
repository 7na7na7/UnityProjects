module.exports=class Vector2
{
    constructor(X=0,Y=0)
    {
        this.x=X;
        this.y=Y;
    }
    Magnitude() //벡터크기구하기
    {
        return Math.sqrt((this.x*this.x)+(this.y*this.y));
    }
    Normalized() //정규화
    {
        var mag=this.Magnitude();
        return new Vector2(this.x/mag,this.y/mag);
    }
    Distance(OtherVec=Vector2()) //해당벡터까지의 거리구하기
    {
        var direction=new Vector2(); //방향구함
        direction.x=OtherVec.x-this.x;
        direction.y=OtherVec.y-this.y;
        return direction.Magnitude();
    }
    ConsoleOutput()
    {
        return '{'+this.x+'.'+this.y+'}';
    }
}