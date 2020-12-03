using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public enum eSceneType
    {
        LobbyScene=0,
        IngameScene=1,
    }
    public enum eSceneLoadState
    {
        none=0,
        SceneLoadStart=1,
        KindSceneLoading,
        KindSceneEnd,
        UnloadingStage,
        UnloadEndStage,
        LoadingStage,
        LoadEndStage,
        FinishLoad,
    }

    eSceneType currentSceneType;
    eSceneType prevSceneType;
    eSceneLoadState currentLoadState;
    int currentStageNum=0;

    static SceneController insatnce;

    public static SceneController _insatnce
    {
        get { return insatnce; }
    }

    private void Awake()
    {
        insatnce = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartIngameScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartLobbyScene()
    {
        prevSceneType = currentSceneType;
        currentSceneType = eSceneType.LobbyScene;
        currentLoadState = eSceneLoadState.none;
        StartCoroutine(LoadingScene("LobbyScene"));
    }

    public void StartIngameScene(int stageNum)
    {
        prevSceneType = currentSceneType;
        currentSceneType = eSceneType.IngameScene;
        currentLoadState = eSceneLoadState.none;
        StartCoroutine(LoadingScene("IngameScene", stageNum));
    }

    IEnumerator LoadingScene(string sceneName,int stageNum=0)
    {
        Scene activeScene;
        AsyncOperation asOperation;
        string stage = "Stage";


        //Scene처리
        currentLoadState = eSceneLoadState.SceneLoadStart;
        //로딩창 열기
        currentLoadState = eSceneLoadState.KindSceneLoading;
        asOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        while(!asOperation.isDone)
        {
            yield return null;
        }
        currentLoadState = eSceneLoadState.KindSceneEnd;
        activeScene = SceneManager.GetSceneByName(sceneName);

        //stage처리
        if(currentSceneType==eSceneType.IngameScene)
        {
            
            //이전 씬이 인게임이었을경우 스테이지 언로드를 먼저 실행
            if(prevSceneType==eSceneType.IngameScene)
            {
                currentLoadState = eSceneLoadState.UnloadingStage;
                stage += currentStageNum.ToString();
                asOperation = SceneManager.UnloadSceneAsync(stage);
                while (!asOperation.isDone)
                    yield return null;
                currentLoadState = eSceneLoadState.UnloadEndStage;
            }

            currentLoadState = eSceneLoadState.LoadingStage;
            stage += stageNum.ToString();
            currentStageNum = stageNum;
            asOperation = SceneManager.LoadSceneAsync(stage, LoadSceneMode.Additive);
            while(!asOperation.isDone)
            {
                yield return null;
            }
            currentLoadState = eSceneLoadState.LoadEndStage;
            activeScene = SceneManager.GetSceneByName(stage);
        }
        //setActiveScene처리
        SceneManager.SetActiveScene(activeScene);

        currentLoadState = eSceneLoadState.FinishLoad;
        //로딩창을 닫는다
        //씬 로딩이 끝났으면
        //인게임씬의 init함수 실행
    }
}
