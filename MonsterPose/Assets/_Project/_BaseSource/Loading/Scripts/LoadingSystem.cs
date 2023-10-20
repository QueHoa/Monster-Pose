using OneHit;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

public class LoadingSystem : Singleton<LoadingSystem>
{
     [Header("Components")]
     [SerializeField] private CanvasGroup canvasGroup;
     [SerializeField] private LoadingLogo loadingLogo;
     [SerializeField] private LoadingBar loadingBar;

     [Header("Time loading")]
     [SerializeField] private bool isFirstTimeLoading = true;
     [SerializeField] private float firstLoadingDuration = 5f;
     [SerializeField] private float normalLoadingDuration = 1f;

     private void Start()
     {
          canvasGroup.gameObject.SetActive(true);
          canvasGroup.alpha = 1f;
          loadingLogo.Reset();
          loadingBar.Reset();

#if UNITY_EDITOR
          // If in EDITOR, need quick loading
          firstLoadingDuration = normalLoadingDuration;
#endif

          //! Start in scene Loading => Main
          if (PlayerPrefs.GetInt("FirstTimePlay", 1) == 1)
          {
               LoadSceneAsync("Play");
          }
          else
               LoadSceneAsync("Lobby");
     }

     public async void LoadSceneAsync(string sceneName)
     {
          // Start loading the scene
          var scene = SceneManager.LoadSceneAsync(sceneName);
          scene.allowSceneActivation = false;

          // Show the loading screen
          canvasGroup.gameObject.SetActive(true);
          canvasGroup.DOFade(1f, 0.3f);
          await UniTask.Delay(300);
          loadingLogo.Show();

          //! First time loading => need more time, else just normal
          float duration = isFirstTimeLoading ? firstLoadingDuration : normalLoadingDuration;
          do
          {
               loadingBar.UpdateProgress(scene.progress / 0.9f, duration);
               await UniTask.Delay(duration.Millisecond());
          }
          while (scene.progress < 0.9f);


          // Phải show xong open ad mới cho chuyển scene
          if (isFirstTimeLoading)
          {
               MasterControl.Instance.ShowOpenAd(async success =>
               {
                    // Activate the next scene
                    scene.allowSceneActivation = true;
                    await UniTask.Delay(300);

                    canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
                    {
                         isFirstTimeLoading = false;
                         canvasGroup.gameObject.SetActive(false);
                         loadingLogo.Reset();
                         loadingBar.Reset();
                    });
               });
          }
          else
          {
               // Activate the next scene
               scene.allowSceneActivation = true;
               await UniTask.Delay(300);

               canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
               {
                    canvasGroup.gameObject.SetActive(false);
                    loadingLogo.Reset();
                    loadingBar.Reset();
               });
          }

          //MasterControl.Instance.ShowOpenAd(success =>
          //{
          //     canvasGroup.DOFade(0f, 0.3f).OnComplete(() =>
          //     {
          //          isFirstTimeLoading = false;
          //          canvasGroup.gameObject.SetActive(false);
          //          loadingLogo.Reset();
          //          loadingBar.Reset();
          //     });

          //     //! update game state
          //     GameManager.Instance.UpdateGameState(sceneName);
          //});
     }
}