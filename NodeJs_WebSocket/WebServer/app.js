//설치한 express모듈 불러오기
const express=require('express');
//설치한 socket.io 모듈 불러오기
const socket=require('socket.io');
//Node.js기본 내장 모듈 불러오기
const http=require('http');
//express 객체 생성
const app=express();
//express http 서버 생성
const server=http.createServer(app);
//생성된 서버를 socket.io에 바인딩
const io=socket(server);

//Get방식으로 /경로에 접속하면 실행됨
app.get('/', function(request,response)
{
    console.log("유저가 / 으로 접속하였습니다!");
    response.send("Hello, Express Server!!")
})

//서버를 8080포트로 listen
server.listen(8081, function()
{
    console.log('서버 실행 중...')
})