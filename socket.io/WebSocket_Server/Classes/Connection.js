//클라이언트에 보내는 모든 이벤트를 하나로 묶어 처리하는 클래스
module.exports=class Connection
{
    //생성자 - 소켓, 플레이어, 서버, 로비 전부 가지고있음
    constructor()
    {
        this.socket;
        this.player;
        this.server;
        this.lobby;  
    }

    //모든 이벤트 처리 - disconnect, joinGame, fireBullet, collisionDestroy, updatePosition, updateRotation
    createEvents()
    {
        let connection = this;
        let socket = connection.socket;
        let server = connection.server;
        let player = connection.player;

        socket.on('disconnect', function() {
            server.onDisconnected(connection);
        });

        socket.on('joinGame',function()
        {
            server.onAttemptToJoinGame(connection);
        });
        socket.on('fireBullet',function(data)
        {
            connection.lobby.onFireBullet(connection,data);
        });
        socket.on('collisionDestroy',function(data)
        {
            connection.lobby.onCollisionDestroy(connection,data);
        });
        socket.on('updatePosition',function(data)
        {
            player.position.x=data.position.x;
            player.position.y=data.position.y;

            //로비의 모두에게 전달
            socket.broadcast.to(connection.lobby.id).emit('updatePosition',player);
        }); 
        socket.on('updateRotation',function(data)
        {
            player.tankRotation=data.tankRotation;
            player.barrelRotation=data.barrelRotation;

            //로비의 모두에게 전달
            socket.broadcast.to(connection.lobby.id).emit('updateRotation',player);
        }); 
    }
}