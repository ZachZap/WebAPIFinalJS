var express = require('express');
var router = express.Router();
const mongoose = require('mongoose');
var dataToSend;

//Sets up routes to Database Schema
require('../models/GameData')
require('../models/UnityGameData')
var GameModel = mongoose.model("unityplayers");
var UnityModel = mongoose.model("unityplayers");

//All CRUD operations

router.get('/getdata',(req,res)=>{
    GameModel.find({}).then(function(games){
        console.log(games)
        res.json({games});
    }).catch(function(err){
        console.error(err);
        res.status(500).json({ error: 'Internal Server Error' });
    });
});

router.post('/deletegame', function(req,res){
    console.log(req.body.game._id);
    GameModel.findByIdAndDelete(req.body.game._id).exec();
    res.redirect('players.html');
})


router.post('/updategame', function(req,res){
    console.log(req.body);
    GameModel.findByIdAndUpdate(req.body.id,{userName:req.body.userName, score:req.body.score, wins:req.body.wins, firstName:req.body.firstName, lastName:req.body.lastName}).then(function(){
        res.redirect('players.html');
    });
});

router.post('/saveGame', function(req,res){
    //logs data
    console.log(req.body);
    //saves data in database
    new GameModel(req.body).save().then(function(){
        res.redirect('players.html');
    });
});

//Unity Data below
router.post('/unity', function(req,res){
    console.log("Unity Posted Data: ");
    
    //saves data in database
    new UnityModel(req.body).save().then(function(){    
        dataToSend = req.body;
        console.log(req.body);
    });
    
})

router.get('/getUnity', function(req,res){

    console.log("Data being sent");
    //console.log(dataToSend);
    //res.json(dataToSend);
    UnityModel.find({}).then(function(playerdata){
        console.log(playerdata)
        res.json({playerdata});
    }).catch(function(err){
        console.error(err);
        res.status(500).json({ error: 'Internal Server Error' });
    });
});

router.post('/updateUnity', function(req,res){
    console.log(req.body);
    dataToSend = req.body;
    UnityModel.findByIdAndUpdate(dataToSend._id,{userName:req.body.userName, score:req.body.score, wins:req.body.wins, firstName:req.body.firstName, lastName:req.body.lastName}).then(function(){
        
    });
});

router.post('/deleteUnity', function(req,res){
    console.log(req.body);
    dataToSend = req.body;
    UnityModel.findByIdAndDelete(dataToSend._id).exec();
    
});

module.exports = router