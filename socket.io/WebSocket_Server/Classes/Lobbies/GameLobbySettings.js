//로비의 상세설정을 담고있는 작은 클래스
module.exports=class GameLobbySettings
{
    //생성자 - 게임모드, 최대 플레이어 수
    constructor(gameMode,maxPlayers) 
    {
        this.gameMode="No gameMode Defined"; 
        this.maxPlayers=maxPlayers; 
    }
}