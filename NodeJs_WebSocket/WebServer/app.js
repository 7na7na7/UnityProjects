//설치한 express모듈 불러오기
const express=require('express');
//설치한 socket.io 모듈 불러오기
const socket=require('socket.io');
//Node.js기본 내장 모듈 불러오기
const http=require('http');
//Node.js 기본 내장 모듈 불러오기, 파일관련처리 모듈
const fs=require('fs');
//express 객체 생성
const app=express();
//express http 서버 생성
const server=http.createServer(app);
//생성된 서버를 socket.io에 바인딩
const io=socket(server);

//정적파일 제공을 위해 미들웨어 사용
app.use("/css",express.static('./static/css'));
app.use('/js',express.static('./static/js'));

//Get방식으로 /경로에 접속하면 실행됨
app.get('/', function(request,response)
{
    //fs 모듈을 사용해 index.html 파일을 읽고 클라에게 전달
    fs.readFile('./static/index.html',function(err,data)
    {
        if(err)
        {
            response.send('에러');
        }
        else
        {
            //헤더로 보낼 파일이 HTML파일인 것을 알리고,
            response.writeHead(200,{"Content-Type":"text/html"});
            //이제 HTML데이터를 보냄
            response.write(data);
            //모두 끝나면 완료됨을 알림
            response.end();
        }
    })
    
    // console.log("유저가 / 으로 접속하였습니다!");
    // response.send("Hello, Express Server!!")
})

//서버를 8080포트로 listen
server.listen(8080, function()
{
    console.log('서버 실행 중...')
})