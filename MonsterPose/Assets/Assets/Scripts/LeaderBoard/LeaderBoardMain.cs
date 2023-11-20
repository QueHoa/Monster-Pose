using Cysharp.Threading.Tasks;
using Dan.Demo;
using Dan.Main;
using Dan.Models;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class LeaderBoardMain : MonoBehaviour
{
    public static string publicKey = "1991033828723703cb446128d58473e7221553cb92b8d8e5738cab1744c0dff2";
    public static string privateKey = "8cd05be36e56101b9c59a2a289553814335558cce3ee2b7ee25eedb5a03c735672ebad9afbebcf995cabe42659773d5e61abecf8a14370c440b4f74c2651210c5d206334d0895b1e524307c5b0028ef864c17ee31780fc00517cddf345eee9d178b5f46a5880790059584c59c798a58ac4bdf3c71ac26584ee311b5f723ed648";
    // use for getLeaderboard on web

    public GameObject home;
    public RectTransform buttonBack;
    public RectTransform title;
    public Transform board;
    public Transform _entryDisplayParent;
    public EntryDisplay _entryDisplayPrefab;
    public EntryDisplay top1;
    public EntryDisplay top2;
    public EntryDisplay top3;
    private int _pageInput = 1;
    public int _entriesToTakeInput;

    private int _playerScore;

    private Coroutine _personalEntryMoveCoroutine;
    List<EntryDisplay> _entries = new List<EntryDisplay>();
    private void Awake()
    {
        GenFakeUser();
    }
    private void OnEnable()
    {
        buttonBack.anchoredPosition = new Vector3(-112, buttonBack.anchoredPosition.y, 0);
        title.anchoredPosition = new Vector3(title.anchoredPosition.x, 160, 0);
        board.localScale = Vector3.zero;
        Load();
        buttonBack.DOAnchorPosX(112, 0.75f).SetEase(Ease.OutQuart);
        title.DOAnchorPosY(-160, 0.75f).SetEase(Ease.OutQuart);
        board.DOScale(1, 0.75f).SetEase(Ease.OutQuart);
    }
    
    public void GetLeaderBoard()
    {
        LeaderboardCreator.GetLeaderboard(publicKey, (users) =>
        {
            foreach(var user in users)
            {
                Debug.Log($"Rank {user.Rank} username {user.Username} score {user.Score}");
                //do anything you want
            }
            foreach (Transform t in _entryDisplayParent) Destroy(t.gameObject);

            top1.SetEntry(users[0]);
            _entries.Add(top1);
            top2.SetEntry(users[1]);
            _entries.Add(top2);
            top3.SetEntry(users[2]);
            _entries.Add(top3);
            for (int i = 3; i < 20; i++)
            {
                var entryDisplay = Instantiate(_entryDisplayPrefab.gameObject, _entryDisplayParent);
                entryDisplay.GetComponent<EntryDisplay>().SetEntry(users[i]);
                _entries.Add(entryDisplay.GetComponent<EntryDisplay>());
            }
            
        });
    }
    public void Load()
    {
        var timePeriod = Dan.Enums.TimePeriodType.AllTime;

        var pageNumber = _pageInput;

        var take = _entriesToTakeInput;

        var searchQuery = new LeaderboardSearchQuery
        {
            Skip = (pageNumber - 1) * take,
            Take = take,
            TimePeriod = timePeriod
        };
        GetLeaderBoard();
        //Leaderboards.DemoSceneLeaderboard.GetEntries(searchQuery, OnLeaderboardLoaded, ErrorCallback);
    }
    public void ChangePageBy(int amount)
    {
        var pageNumber = _pageInput;
        pageNumber += amount;
        if (pageNumber < 1) return;
    }
    public void Back()
    {
        StartCoroutine(Effectback());
    }
    IEnumerator Effectback()
    {
        buttonBack.DOAnchorPosX(-112, 0.75f).SetEase(Ease.OutQuart);
        title.DOAnchorPosY(160, 0.75f).SetEase(Ease.OutQuart);
        board.DOScale(0, 0.75f).SetEase(Ease.OutQuart);
        yield return new WaitForSeconds(0.75f);
        home.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Submit()
    {
        //Leaderboards.DemoSceneLeaderboard.UploadNewEntry(_playerUsernameInput.text, _playerScore, Callback, ErrorCallback);
    }

    public void DeleteEntry()
    {
        Leaderboards.DemoSceneLeaderboard.DeleteEntry(Callback, ErrorCallback);
    }

    public void ResetPlayer()
    {
        LeaderboardCreator.ResetPlayer();
    }

    private void Callback(bool success)
    {
        if (success)
            Load();
    }

    private void ErrorCallback(string error)
    {
        Debug.LogError(error);
    }
    public static async void GenFakeUser()
    {
        string[] nicknames = new string[]
        {
         "Emma", "Liam", "Ava", "Noah", "Mia", "Ethan", "Ella", "Aiden", "Lily", "James",
        "Lucy", "Sophia", "Mason", "Chloe", "Olivia", "Emily", "Harper", "Grace", "Daniel", "Oliver",
        "Riley", "Hannah", "Samuel", "David", "Sarah", "Caleb", "Henry", "Evelyn", "Natalie", "Claire",
        "Andrew", "Sofia", "Avery", "Scarlett", "Abigail", "Victoria", "Michael", "Elijah", "Jackson",
        "Amelia", "Joseph", "Stella", "Madison", "Hannah", "Mason", "Grace", "Evelyn", "David",
        "Samuel", "Olivia", "William", "Evelyn", "Victoria", "Madison", "Scarlett", "Abigail",
        "Elizabeth", "Addison", "Natalie", "Harper", "Scarlett", "Natalie", "Evelyn", "Madison",
        "Gabriel", "Zachary", "Allison", "Kennedy", "Scarlett", "Gabriel", "Benjamin", "Victoria",
        "Michael", "Nicholas", "Kimberly", "Nicholas", "Kennedy",
        "Jonathan","Christian",  "Nicholas", "Christian", "Sebastian"

        };
        int score = 5000;
        for (int i = 1; i <= 50; i++)
        {
            LeaderboardCreator.ResetPlayer();
            LeaderboardCreator.UploadNewEntry(LeaderBoardMain.publicKey,
                    nicknames[Random.Range(0, nicknames.Length - 1)],
                    score,
                    Random.Range(0,7).ToString()
                    );

            await UniTask.Delay(350);

            score += 10 * (Random.Range(0, 100) > 50 ? 1 : 2);
        }

    }

}
