let Connection=require('./Connection');
let Player=require('./Player');
//로비
let LobbyBase=require('./Lobbies/LobbyBase');
let GameLobby=require('./Lobbies/GameLobby');
let GameLobbySettings=require('./Lobbies/GameLobbySettings');
module.exports=class Server
{
    constructor()
    {
        this.connections=[];
        this.lobbys=[];

        this.lobbys[0]=new LobbyBase(0);
    }
    //index에서 intervall로 실행
    onUpdate()
    {
        let server=this;
        
        for(let id in server.lobbys)
        {
            server.lobbys[id].onUpdate();
        }
    }
    //연결되었을때
    onConnected(socket)
    {
        let server=this; //자기자신을 server에 넣어둠
        let connection=new Connection(); //모든 정보를 가지고있는 커넥션 생성!
        //소켓설정, 플레이어생성, 서버설정
        connection.socket=socket; 
        connection.player=new Player();
        connection.server=server;

        let player=connection.player; 
        let lobbys=this.lobbys;

        console.log('Added new player to the server ('+player.id+')');
        server.connections[player.id]=connection; //커넥션배열[플레이어id]에 커넥션저장

        socket.join(player.lobby); //로비접속
        connection.lobby=lobbys[player.lobby]; //커넥션에 현재로비등록
        connection.lobby.onEnterLobby(connection); 

        return connection; //커넥션 리턴
    }
    //연결 끊겼을때
    onDisconnected(connection=Connection)
    {
        let server=this;
        let id=connection.player.id;

        delete server.connections[id];
        console.log('Player '+connection.player.displayPlayerInformation()+' has disconnected.');

        //해당 플레이어가 있었던 로비의 플레이어 모두에게 알림
        connection.socket.broadcast.to(connection.player.lobby).emit('disconnected',
        {
            id:id
        });

        server.lobbys[connection.player.lobby].OnLeaveLobby(connection);
    }
    //게임접속 시도, 클라에서 버튼으로 실행
    onAttemptToJoinGame(connection=Connection)
    {
        let server=this;
        let lobbyFound=false;

        let gameLobbies=server.lobbys.filter(item=>
        {
            return item instanceof GameLobby;
        })
        console.log('Found ('+gameLobbies.length+') lobbies on the server.');

        gameLobbies.forEach(l=> //로비 수만큼 반복
            {
                if(!lobbyFound) //로비를 못찾은상태라면 접속시도
                {
                    let canJoin=l.canEnterLobby(connection); //들어갈수있나?

                    if(canJoin) //들어갈수있으면
                    {
                        lobbyFound=true;
                        server.onSwitchLobby(connection,l.id); //들어감
                    }
                }
            });

            if(!lobbyFound) //여전히 로비를 못찾았으면
            {
                console.log('Making a new game Lobby');
                //그냥 새로 하나 만들어서 들어감
                let gamelobby=new GameLobby(gameLobbies.length+1,new GameLobbySettings('FFA',2));
                server.lobbys.push(gamelobby);
                server.onSwitchLobby(connection,gamelobby.id);
            }
    }
    //로비 바꾸기
    onSwitchLobby(connection=Connection, lobbyId)
    {
        let server=this;
        let lobbys=server.lobbys;
        connection.socket.join(lobbyId); //새 로비에 join
        connection.lobby=lobbys[lobbyId];

        lobbys[connection.player.lobby].OnLeaveLobby(connection); //로비에서 떠났다고 메시지보냄
        lobbys[lobbyId].OnEnterLobby(connection); //지금들어온로비에 들어왔다 알림
    }
}