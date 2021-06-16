let Connection=require('../Connection');

//로비 입장과 퇴장을 관리하는 클래스, GameLobby가 상속받아 사용한다.
module.exports=class LobbyBase
{
    //생성자 - id, connections(로비의 플레이어들 저장)
    constructor(id)
    {
        this.id=id;
        this.connections=[];
    }

    //로비 들어가기
    onEnterLobby(connection=Connection)
    {
        let lobby=this;
        let player=connection.player;

        console.log('Player '+player.displayPlayerInformation()+' has entered the lobby '+lobby.id);

        lobby.connections.push(connection); 
        
        player.lobby=this.id;
        connection.lobby=lobby;
    }

    //로비 떠나기
    OnLeaveLobby(connection=Connection)
    {
        let lobby=this;
        let player=connection.player;

        console.log('Player '+player.displayPlayerInformation()+' has left the lobby ('+lobby.id+')');

        let index=lobby.connections.indexOf(connection);
        if(index>1)
        {
            lobby.connections.splice(index,1);
        }
    }
}