let Connection=require('../Connection');

module.exports=class LobbyBase
{
    constructor(id)
    {
        this.id=id;
        this.connections=[];
    }
 
    onUpdate()
    {

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