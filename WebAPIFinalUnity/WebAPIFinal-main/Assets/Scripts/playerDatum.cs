using System;
using System.Collections.Generic;
[Serializable]
public class PlayerDatum
{
    public string _id;
    public int score;
    public int wins;
    public string userName;
    public string firstName;
    public string lastName;
    public int __v;

    public void SetTo(PlayerDatum dataToBe)
    {
        this._id = dataToBe._id;
        this.firstName = dataToBe.firstName;
        this.lastName = dataToBe.lastName;
        this.userName = dataToBe.userName;
        this.score = dataToBe.score;
        this.wins = dataToBe.wins;
        this.__v = dataToBe.__v;
    }
}
[Serializable]
public class Root
{
    public PlayerDatum[] playerdata;
}