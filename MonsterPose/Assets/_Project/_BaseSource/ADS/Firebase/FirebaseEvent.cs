namespace OneHit {

     public static class FirebaseEvent {

          #region FIXED - Đã đặt trong source code ADS (không cần thêm ở đâu nữa)
          public static readonly string OPEN_AD = "OPEN_AD";
          public static readonly string NATIVE_ADS = "NATIVE_ADS";
          public static readonly string ADS_REWARD = "ADS_REWARD";
          public static readonly string ADS_INTERSTITIAL = "ADS_INTERSTITIAL";
          public static readonly string PURCHASE_SUCCESS_NOADS = "PURCHASE_SUCCESS_NOADS";
          #endregion

          //----------TRACKING: Ở những nơi cần log event, gọi:
          // FirebaseManager.Instance.LogEvent(FirebaseEvent.TEN_EVENT);

          public static readonly string TOTAL_COLLECT_COIN = "TOTAL_COLLECT_COIN";
          public static readonly string TOTAL_UNLOCK_BY_REWARD = "TOTAL_UNLOCK_BY_REWARD";
          public static readonly string TOTAL_UNLOCK_BY_COIN = "TOTAL_UNLOCK_BY_COIN";

          public static readonly string OPEN_SETTING = "OPEN_SETTING";
          public static readonly string OPEN_COLLECT_COIN = "OPEN_COLLECT_COIN";

          public static readonly string PLAY_BACK_HOME = "PLAY_BACK_HOME";
          public static readonly string END_GAME = "END_GAME";
          public static readonly string START_PLAY = "START_PLAY";
          public static readonly string REPLAY = "REPLAY";

          public static readonly string END_OTHER_STAGE = "END_OTHER_STAGE";
          public static readonly string END_OTHER_MUSIC = "END_OTHER_MUSIC";
          public static readonly string END_BACK_HOME = "END_BACK_HOME";

          public static readonly string OPEN_CAPTURE_COUNT = "OPEN_CAPTURE_COUNT";
          public static readonly string CAPTURE_COUNT = "CAPTURE_COUNT";
          public static readonly string ALBUM_OPEN_COUNT = "ALBUM_OPEN_COUNT";
          public static readonly string VIEW_CAPTURE_COUNT = "OPEN_CAPTURE_COUNT";
          public static readonly string SAVE_CAPTURE_COUNT = "SAVE_CAPTURE_COUNT";

          public static readonly string OPEN_DAILY_REWARD = "OPEN_DAILY_REWARD";

          public static readonly string OPEN_BATTLE_MODE = "OPEN_BATTLE_MODE";
          public static readonly string PLAY_BATTLE_MODE = "PLAY_BATTLE_MODE";
          public static readonly string FINDING_OPPONENT = "FINDING_OPPONENT";
          public static readonly string CLAIM_REWARD_BATTLE = "CLAIM_REWARD_BATTLE";
          public static readonly string SKIP_REWARD_BATTLE = "SKIP_REWARD_BATTLE";

          public static string RewardDayX(int day) {
               return "REWARD_DAY_" + day.ToString();
          }
     }
}