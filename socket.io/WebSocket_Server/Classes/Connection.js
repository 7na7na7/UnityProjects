module.exports=class Connection
{
    constructor()
    {
        //소켓, 플레이어, 서버, 로비 전부 가지고있음
        this.socket;
        this.player;
        this.server;
        this.lobby;  
    }

    //이벤트 처리
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