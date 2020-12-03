using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    #region[참조변수]
    CharacterUI uiPlayer;
    PlayerController player;
    [SerializeField] Transform playerSpawnPoint;
    #endregion

    public static IngameManager instance;

    


    public enum eGameState
    {
        NONE=0,
        READY,
        PLAYER_APPEAR,
        MONSTER_SPAWN,
        CAMERA_MOVE,
        START,
        PLAY,
        END,
        RESULT,
    }
    eGameState currentGameState;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        instance.playerAppearance();
        instance.initGame();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initGame()
    {
        currentGameState = eGameState.NONE;

        GameObject go = GameObject.Find("CharaterMiniUI");
        uiPlayer = go.GetComponent<CharacterUI>();

        uiPlayer.enableWindow(false);
    }

    public void readyGame()
    {
        currentGameState = eGameState.READY;
    }

    public void playerAppearance()
    {
        currentGameState = eGameState.PLAYER_APPEAR;

        GameObject prefabPlayer = Resources.Load("Prefab/Unit/Player") as GameObject;
        GameObject go = Instantiate(prefabPlayer, playerSpawnPoint.position, playerSpawnPoint.rotation);
        player = go.GetComponent<PlayerController>();
        uiPlayer.initData(player.name, player.hpRate, player.finishdamage);
    }
}
