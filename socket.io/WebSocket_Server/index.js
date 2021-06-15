//socket.io 라이브러리 갖고옴, 포트적용
var io=require('socket.io')(process.env.PORT || 52300);

//커스텀 클래스
var Player=require('./Classes/Player.js');
var Bullet=require('./Classes/Bullet.js');

console.log('Server has started');

var players=[];
var sockets=[];
var bullets=[];

setInterval(()=>
{
    bullets.forEach(bullet=>
        {
            var isDestroyed=bullet.onUpdate();

            if(isDestroyed) //파괴된 상태라면
            {
                var index=bullets.indexOf(bullet);
                if(index>-1)
                {
                    bullets.splice(index,1); //제거

                    var returnData=
                    {
                        id:bullet.id
                    }

                    for(var playerID in players)
                    {
                        sockets[playerID].emit('serverUnSpawn',returnData);
                    }
                }
            }
            else
            {
                var returnData=
                {
                    id:bullet.id,
                    position:
                    {
                        x:bullet.position.x,
                        y:bullet.position.y
                    }
                }
                for(var playerID in players)
                {
                    sockets[playerID].emit('updatePosition',returnData);
                }
            }
        });
},10,0) //10밀리세컨드마다 실행

io.on('connection',function(socket)
{
    //연결되었을 때 실행
   console.log('connection success!');

   //플레이어 생성
   var player=new Player();
   var thisPlayerID=player.id;

   players[thisPlayerID]=player;
   sockets[thisPlayerID]=socket;

   //emit은 나한테보내고 broadcast는 나빼고 전부한테 보냄
   socket.emit('register',{id:thisPlayerID}); //register이벤트, e.data["id"]로 접근가능
   socket.emit('spawn',player); //나를 스폰
   socket.broadcast.emit('spawn',player); //나를 다른사람에게 스폰

   //다른플레이어를 나에게 스폰
   for(var playerID in players)
   {
       if(playerID!=thisPlayerID) //자기말고
       {
           socket.emit('spawn',players[playerID]); //다른플레이어 스폰
       }
   }

   socket.on('updatePosition',function(data)
   {
       //플레이어포지션 동기화
       player.position.x=data.position.x;
       player.position.y=data.position.y;

       socket.broadcast.emit('updatePosition',player);
   })

  
   socket.on('updateRotation',function(data)
   {
       //회전값 동기화
       player.tankRotation=data.tankRotation;
       player.barrelRotation=data.barrelRotation;

       socket.broadcast.emit('updateRotation',player);
   })
   socket.on('fireBullet',function(data)
   {
       var bullet=new Bullet();
       bullet.name='Bullet'; //이름설정
       bullet.activator=data.activator; //총알쏜사람
       bullet.position.x=data.position.x; //총알위치
       bullet.position.y=data.position.y;
       bullet.direction.x=data.direction.x; //총알방향
       bullet.direction.y=data.direction.y;

       bullets.push(bullet); //bullets에 bullet추가

       var returnData= //모두에게 뿌릴 총알생성데이터
       {
           name:bullet.name,
           id:bullet.id,
           activator:bullet.activator,
           position:   
           {
               x:bullet.position.x,
               y:bullet.position.y
           },
           direction: //총알에만 있는 direction
           {
               x:bullet.direction.x,
               y:bullet.direction.y
           }
       }
       socket.emit('serverSpawn',returnData); //자신에게스폰
       socket.broadcast.emit('serverSpawn',returnData); //다른사람들에게 스폰
   });
   socket.on('collisionDestroy',function(data)
   {
       console.log("Collision with bullet id : "+data.id); 
       let returnBullets = bullets.filter(bullet=>{
           //총알중 해당 id와 같은 총알을 고름
           return bullet.id==data.id;
       });
       returnBullets.forEach(bullet=>
        {
            bullet.isDestroyed=true; //다음검사에 해당 총알이 파괴되도록 함
        })
   })
   socket.on('disconnect',function()
   {
       //플레이어가 나갔을 때 실행
       console.log('A player has disconnected');
       //나갈 때 플레이어도 삭제
       delete players[thisPlayerID];
       delete sockets[thisPlayerID];
       socket.broadcast.emit('disconnected',player);
   })
});

function Interval(func, wait, times)
{
    var interv=function(w,t)
    {
        return function()
        {
            if(typeof(t === "undefined" || t-- > 0))
            {
                setTimeout(interv,w); //w초후에 interv함수실행
                try
                {
                    func.call(null);
                }
                catch(e)
                {
                    t=0;
                    throw e.toString();
                }
            }
        };
    }(wait,times);

    setTimeout(interv,wait);
}