//socket.io 라이브러리 갖고옴, 포트적용
var io=require('socket.io')(process.env.PORT || 52300);

//커스텀 클래스
var Player=require('./Classes/Player.js');

console.log('Server has started');

var players=[];
var sockets=[];

io.on('connection',function(socket)
{
    //연결되었을 때 실행
   console.log('connection success!');

   //플레이어 생성
   var player=new Player();
   var thisPlayerID=player.id;

   players[thisPlayerID]=player;
   sockets[thisPlayerID]=socket;

   //emit은 나포함 전부한테보내고 broadcast는 나빼고 전부한테 보냄
   socket.emit('register',{id:thisPlayerID}); //register이벤트, e.data["id"]로 접근가능
   socket.emit('spawn',player); //스폰이벤트
   socket.broadcast.emit('spawn',player); //나빼고 전부한테

   //나말고 다른플레이어 스폰
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