using Dan.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    private string publicKey = "";
    //public string privateKey = ""; // use for getLeaderboard on web
    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, (users) =>
        {
            foreach(var user in users)
            {
                Debug.Log($"Rank {user.Rank} username {user.Username} score {user.Score}");
                //do anything you want
            }
        });
    }
}
