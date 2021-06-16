let LobbyBase=require('./LobbyBase');
let GameLobbySettings=require('./GameLobbySettings');
let Connection=require('../Connection');
let Bullet=require('../Bullet');

//로비에 입장한 후, 게임플레이를 위한 클래스
module.exports=class GameLobby extends LobbyBase
{
    //GameLobbySettings를 상속받은 생성자
    constructor(id,settings=GameLobbySettings)
    {
        super(id); //LobbyBase의 생성자 호출
        this.settings=settings;
        this.bullets=[];
    }
    onUpdate()
    {
        let lobby=this;
        
        lobby.updateBullets();
        lobby.updateDeadPlayers();
    }
    //로비에 들어갈 수 있는지 확인
    canEnterLobby(connection=Connection)
    {
        let lobby=this;
        let maxPlayerCount=lobby.settings.maxPlayers; //로비최대플레이어수
        let currentPlayerCount=lobby.connections.length; //현재플레이어수

        if(currentPlayerCount+1>maxPlayerCount) //현재플레이어에내가들어갔을 때 초과라면
        {
            return false; //안돼!
        } 
        return true; //돼!
    }
    OnEnterLobby(connection=Connection) //로비들어가기
    {
        let lobby=this;

        super.onEnterLobby(connection);

        lobby.addPlayer(connection);
    }
    OnLeaveLobby(connection=Connection) //로비나오기
    {
        let lobby=this;

        super.OnLeaveLobby(connection);

        lobby.removePlayer(connection);
    }

    //총알포지션 업데이트
    updateBullets() 
    {
        let lobby=this;
        let bullets=lobby.bullets;
        let connection=lobby.connections;

        bullets.forEach(b => 
        {
            let isDestroyed=b.onUpdate();
            if(isDestroyed)
            {
                lobby.despawnBullet(b);
            }
            else
            {
                var returnData=
                {
                    id:b.id,
                    position:
                    {
                        x:b.position.x,
                        y:b.position.y
                    }
                }
                this.connections.forEach(c=>
                    {
                        c.socket.emit('updatePosition',returnData);
                    })
            }
        });
    }

    updateDeadPlayers()
    {
        let lobby=this;
        let connection=lobby.connections;
 
        this.connections.forEach(c=> //연결되어있는 모두에게
        {
            let player=c.player;
            if(player.isDead) //죽었으면
            {
                let isRespawn=player.respawnCounter(); //리스폰카운터업
                if(isRespawn) //리스폰카운터가 다찼으면
                {
                    //리스폰
                    let socket=c.socket;
                    let returnData=
                    {
                        id:player.id,
                        position:
                        {
                            x:player.position.x,
                            y:player.position.y
                        }
                    }
                    socket.emit('playerRespawn',returnData); //자신에게 리스폰실행
                    socket.broadcast.to(lobby.id).emit('playerRespawn',returnData); //해당로비의 다른사람에게 리스폰실행
                }
            }
        })
    }
    onFireBullet(connection=Connection,data)
    {
        let lobby=this;
        
        //총알정보설정
        let bullet=new Bullet();
        bullet.name='Bullet';
        bullet.activator=data.activator;
        bullet.position.x=data.position.x;
        bullet.position.y=data.position.y;
        bullet.direction.x=data.direction.x;
        bullet.direction.y=data.direction.y;

        lobby.bullets.push(bullet); //총알추가

        var returnData=
        {
            name:bullet.name, //이름
            id:bullet.id, //id
            activator:bullet.activator, //쏜사람id
            position: //위치
            {
                x:bullet.position.x,
                y:bullet.position.y
            },
            direction: //방향
            {
                x:bullet.direction.x,
                y:bullet.direction.y
            }
        }

        //동기화
        connection.socket.emit('serverSpawn',returnData);
        connection.socket.broadcast.to(lobby.id).emit('serverSpawn',returnData);
    }
    onCollisionDestroy(connection=Connection,data)
    {
        let lobby=this;

        let returnBullets=lobby.bullets.filter(bullet=>{
            //총알중 해당 id와 같은 총알을 고름
            return bullet.id==data.id;
        });
   
        returnBullets.forEach(bullet=>
        {
            let playerHit=false;

            lobby.connections.forEach(c=>
            {
                let player=c.player;

                if(bullet.activator!=player.id) //총알이랑부딪힌사람이 총알쏜사람이 아니면검사
                {
                    let distance=bullet.position.Distance(player.position);

                    if(distance<0.65) //총알이 충돌했을때 그거랑 가까우면
                    {
                        let isDead=player.dealDamage(50);
                        if(isDead) //죽었으면
                        {
                            console.log('Player with id : '+player.id+'has died');
                            let returnData=
                            {
                                id:player.id
                            }
                            //얘 죽었대요 ㅋㅋ 호출
                            c.socket.emit('playerDead',returnData);
                            c.socket.broadcast.to(lobby.id).emit('playerDead',returnData);
                        }
                        else
                        {
                            console.log('Player with id : '+player.id+'has ('+player.health+') left');
                        }
                        lobby.despawnBullet(bullet); //총알삭제
                    }
                }
            });
            if(!playerHit)
            {
                bullet.isDestroyed=true;
            }
        })
    }
    despawnBullet(bullet=Bullet) //총알삭제
    {
        let lobby=this;
        let bullets=lobby.bullets;
        let connections=lobby.connections;

        console.log('Destroying bullet ('+bullet.id+')');
        var index=bullets.indexOf(bullet); 
        if(index>-1) //총알이 존재하면
        {
            bullets.splice(index,1); //삭제

            var returnData=
            {
                id:bullet.id
            }

            connections.forEach(c=>
            {
                //모두한테 serverUnspawn호출
                c.socket.emit('serverUnspawn',returnData);
            })
        }
    }
    addPlayer(connection=Connection) //플레이어스폰
    {    
        let lobby=this;
        let connections=lobby.connections;
        let socket=connection.socket;
        console.log(connection.player.id);
        var returnData=
        {
            id:connection.player.id
        }
        socket.emit('spawn',returnData); //자기자신 스폰
        socket.broadcast.to(lobby.id).emit('spawn',returnData); //다른사람한테 자기자신 스폰

        //다른사람들 스폰
        connections.forEach(c=>
        {
            //자신이 아니라면 스폰
            if(c.player.id!=connection.player.id)
            {
                socket.emit('spawn',
                {
                    id:c.player.id
                });
            }
        })
    }
    removePlayer(connection=Connection)
    {    
        let lobby=this;

        connection.socket.broadcast.to(lobby.id).emit('disconnected',
        {
            id:connection.player.id
        })
    }
}
