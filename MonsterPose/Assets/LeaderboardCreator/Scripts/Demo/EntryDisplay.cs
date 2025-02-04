﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Dan.Models;

namespace Dan.Demo
{
    public class EntryDisplay : MonoBehaviour
    {
        [SerializeField] private Text _rankText, _usernameText, _scoreText, _timeText;
        [SerializeField] private Image imgProfile;
        public Sprite userSprite;
        public Sprite mineSprite;
        public Sprite[] avt;
        [HideInInspector]
        public int idUser;
        public void SetEntry(Entry entry)
        {
            if(_rankText != null)
            {
                _rankText.text = entry.RankSuffix();
            }
            _usernameText.text = entry.Username;
            _scoreText.text = entry.Score.ToString();
            idUser = int.Parse(entry.Extra);
            imgProfile.sprite = avt[idUser];
            if (_timeText != null)
            {
                var dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(entry.Date);
                _timeText.text = $"{dateTime.Hour:00}:{dateTime.Minute:00}:{dateTime.Second:00} (UTC)\n{dateTime:dd/MM/yyyy}";
            }
            if(userSprite != null || mineSprite != null) 
            {
                GetComponent<Image>().sprite = entry.IsMine() ? mineSprite : userSprite;
            }
            
        }
    }
}