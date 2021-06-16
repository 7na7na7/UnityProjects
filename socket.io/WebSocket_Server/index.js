//socket.io 라이브러리 갖고옴, 포트적용
var io=require('socket.io')(process.env.PORT || 52300);
let Server=require('./Classes/Server');


console.log('Server has started');

let server=new Server();
setInterval(()=>
{
    server.onUpdate();

},10,0);
io.on('connection',function(socket) //소켓 연결
{
    let connection=server.onConnected(socket); //서버연결시 설정을위해호출
    connection.createEvents(); //이벤트처리
    connection.socket.emit('register',{id:connection.player.id}) //clientId설정
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