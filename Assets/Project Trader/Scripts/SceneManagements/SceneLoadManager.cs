﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using ProjectTrader;

public class SceneLoadManager : MonoBehaviour
{
    public static ShopScene CurrentLoadedShopScene
    {
        get; private set;
    } = (ShopScene)(-1);

static SceneLoadManager()
    {
        GameObject gameObject = new GameObject("SceneLoadManager");
        gameObject.AddComponent<SceneLoadManager>();

        // 게임오브젝트 인스턴스화
        //gameObject = Instantiate(gameObject);

        // 정적 인스턴스 프로퍼티 설정
        Instance = gameObject.GetComponent<SceneLoadManager>();
    }

    public static SceneLoadManager Instance { get; }

    public enum ShopScene
    {
        Shop1,
        Shop2,
        Shop3
    }

    public void OpenTownScene()
    {
        var townSceneCameraManager = GameObject.Find("TownSceneCameraManager").GetComponent<CameraManager>();
        var gameSceneCameraManager = GameObject.Find("GameSceneCameraManager").GetComponent<CameraManager>();

        townSceneCameraManager.CameraActive(true);
        gameSceneCameraManager.CameraActive(false);

        // FloatCanvas 캔버스 렌더링 끄기
        var floatingCanvasGameObject = GameObject.Find("FloatingCanvas");
        var floatingCanvas = floatingCanvasGameObject.GetComponent<Canvas>();
        floatingCanvas.enabled = false;
    }

    public void CloseTownScene()
    {
        var townSceneCameraManager = GameObject.Find("TownSceneCameraManager").GetComponent<CameraManager>();
        var gameSceneCameraManager = GameObject.Find("GameSceneCameraManager").GetComponent<CameraManager>();

        townSceneCameraManager.CameraActive(false);
        gameSceneCameraManager.CameraActive(true);

        // FloatCanvas 캔버스 렌더링 켜기
        var floatingCanvasGameObject = GameObject.Find("FloatingCanvas");
        var floatingCanvas = floatingCanvasGameObject.GetComponent<Canvas>();
        floatingCanvas.enabled = true;
    }

    public void LoadGameScene()
    {
        switch (PlayData.CurrentData.Level)
        {
            default:
            case 1:
                LoadGameScene(ShopScene.Shop1);
                break;
            case 2:
                LoadGameScene(ShopScene.Shop2);
                break;
        }
    }

    public void LoadGameScene(ShopScene shopScene)
    {
        ShowLoadingScene(() =>
        {
        // 초기화 후 다시 불러오기
        SceneManager.LoadScene("GameScene");
            string shopSceneName;
            switch (shopScene)
            {
                case ShopScene.Shop1:
                    shopSceneName = "Shop1Scene";
                    break;
                case ShopScene.Shop2:
                    shopSceneName = "Shop2Scene";
                    break;
                case ShopScene.Shop3:
                    shopSceneName = "Shop3Scene";
                    break;
                default:
                    shopSceneName = "";
                    break;
            }
            SceneManager.LoadScene(shopSceneName, LoadSceneMode.Additive);
            SceneManager.LoadScene("TownScene", LoadSceneMode.Additive);

            CurrentLoadedShopScene = shopScene;
        });
    }

    public void LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        ShowLoadingScene(() =>
        {
            SceneManager.LoadScene(sceneName, loadSceneMode);
        });
    }

    public void ShowLoadingScene(System.Action action)
    {
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Additive);
        StartCoroutine(LoadingSceneCoroutine());
        IEnumerator LoadingSceneCoroutine()
        {
            yield return null;
            action();
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    //public static void UnloadShopScene()
    //{
    //    var sceneCount = SceneManager.sceneCount;
    //    for(int i = 0; i < sceneCount; i++)
    //    {
    //        var scene = SceneManager.GetSceneAt(i);
    //        switch (scene.name)
    //        {
    //            case "Shop1Scene":
    //            case "Shop2Scene":
    //            case "Shop3Scene":
    //                break;
    //        }
    //    }
    //}


    //    private ShopScenes shopScene;

    //    public ShopScenes ShopScene => shopScene;
    //    private void LoadScene(ShopScenes shopScene)
    //    {
    //        switch (ShopScene)
    //        {
    //            case ShopScenes.Shop1:
    //            case ShopScenes.Shop2:
    //            case ShopScenes.Shop3:
    //                break;
    //        }
    //    }
    //    private void UnloadScene()
    //    {
    //        var shop1sceneAsync = SceneManagement.SceneManager.UnloadSceneAsync("Shop1Scene");
    //        var shop2sceneAsync = SceneManagement.SceneManager.UnloadSceneAsync("Shop2Scene");
    //        var shop3sceneAsync = SceneManagement.SceneManager.UnloadSceneAsync("Shop3Scene");
    //    }

    //#if UNITY_EDITOR
    //    private void Update()
    //    {

    //    }
    //#endif
}
