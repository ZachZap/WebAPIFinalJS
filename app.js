var express = require('express');
var app = express();
var router = express.Router();
var path = require('path');
var mongoose = require('mongoose');
var bodyparser = require("body-parser");
var crud = require('./routes/crud');

app.use(express.json());
app.use(bodyparser.urlencoded({extended:true}));
app.use(bodyparser.json());
app.use('/', crud);

//connect to mongodb via mongoose
mongoose.connect("mongodb://127.0.0.1:27017/WebAPI",{
}).then(function(){
    console.log("Connected to MongoDb Database");
}).catch(function(err){
    console.log(err);
}); 



//var num = 6 *6;

//console.log(num);

//Example of Static Route
app.use(express.static(__dirname+'/pages'));

// JavaScript for a route

app.get('/', function(req,res){ 
    //res.send("here would be the page from the route");
    res.sendFile(path.join(__dirname+"/pages/index.html"));
});

app.get('/about', function(req,res){ 
    //res.send("here would be the page from the route");
    res.sendFile(path.join(__dirname+"/pages/about.html"));
});

app.get('/contact', function(req,res){ 
    //res.send("here would be the page from the route");
    res.sendFile(path.join(__dirname+"/pages/contact.html"));
});

app.get('/players', function(req,res){ 
    //res.send("here would be the page from the route");
    res.sendFile(path.join(__dirname+"/pages/players.html"));
});

app.get('/create', function(req,res){ 
    //res.send("here would be the page from the route");
    res.sendFile(path.join(__dirname+"/pages/create.html"));
});

app.get('/update', function(req,res){ 
    //res.send("here would be the page from the route");
    res.sendFile(path.join(__dirname+"/pages/update.html"));
});






//GameModel.find({}).then(function(game){console.log(game)});


app.listen(3000, function(){
    console.log("Running on Port 3000");
})