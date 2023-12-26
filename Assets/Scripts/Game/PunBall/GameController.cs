using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    #region Inspector variables
    
    [SerializeField] private Data waveData;
    [SerializeField] private int waveIndex = 0;
    [SerializeField] private float defaultBulletText = 0f; //set this variables to ScriptableObject in future
    
    #endregion Inspector variables

    #region private variables

    private Player player;
    private BulletsController bulletsController;
    private SaveLoadController saveLoadController;
    private ObjectPool objectPool;
    private PunBallPoolCells punBallPoolCells;
    private SpawnController spawnController;
    private StatsController statsController;
    private UIController uiController;
    private MatchThreeButtons matchThreeButtons;

    #endregion private variables

    #region properties

    public Player Player => player;
    public SaveLoadController SaveLoadController => saveLoadController;
    public PunBallPoolCells PunBallPoolCells => punBallPoolCells;
    public ObjectPool ObjectPool => objectPool;
    public UIController UIController => uiController;
    public int WaveIndex => waveIndex;
    public Data WaveData => waveData;

    #endregion properties
    
    #region Unity functions

    private void OnEnable()
    {
        SceneManager.sceneLoaded += LoadAfterGameSceneWasLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= LoadAfterGameSceneWasLoaded;
    }

    private void Start()
    {
        SetEnoughBulletsSprite();
        SetWaveData();
    }

    #endregion Unity functions

    #region public functions

    #endregion public functions
    
    #region private functions

    private void SetVariables()
    {
        if (bulletsController == null)
        {
            bulletsController = FindObjectOfType<BulletsController>();
        }
        
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }



        if (punBallPoolCells == null)
        {
            punBallPoolCells = FindObjectOfType<PunBallPoolCells>();
        }

        if (saveLoadController == null)
        {
            saveLoadController = FindObjectOfType<SaveLoadController>();
        }

        if (objectPool == null)
        {
            objectPool = FindObjectOfType<ObjectPool>();
        }

        if (statsController == null)
        {
           statsController = saveLoadController.GetComponent<StatsController>();
           statsController.SetGameController(this);
        }
    }

    private void SetEnoughBulletsSprite()
    {
        //Debug.Log($"player.MaxBulletTypeCount.Count = {player.MaxBulletTypeCount.Count}");
        for (int i = 0; i < player.MaxBulletTypeCount.Count; i++)
        {
            bulletsController.AddBulletByType();
            bulletsController.SetBulletTextForLastBullet(defaultBulletText.ToString());
        }
        bulletsController.SetAvalibleCountBullets();
    }

    private void SetWaveData()
    {
        Debug.Log($"[SetWaveData] waveIndex = {waveIndex}");
        waveData = saveLoadController.GetWaveData(waveIndex,saveLoadController.LastCompleteLevel);
    }


    private void LoadAfterGameSceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene [{scene.name}] was loaded");
        if (scene.buildIndex == 1)
        {
            if (spawnController == null)
            {
                spawnController = FindObjectOfType<SpawnController>();
            }

            if (uiController == null)
            {
                uiController = FindObjectOfType<UIController>();
                uiController.SetGameController(this);
            }

            if (matchThreeButtons == null)
            {
                matchThreeButtons = FindObjectOfType<MatchThreeButtons>();
            }
            SetVariables();
            SetActions();
        }
    }
    /// <summary>
    /// call only after Game scene was loaded
    /// </summary>
    private void SetActions()
    {
        spawnController.SetActionAfterSpawn(
            ()=> statsController.SetLastWaveSpawnedList(spawnController.LastWaveSpawnedList)
            );
        player.SetActionsOnShoot(
            () =>player.SetActiveBullet(
                ObjectPool.GetObjectByType(
                ObjectType.Bullet, bulletsController.GetFirstBulletElementType())
                ),
            ()=>bulletsController.SetBulletTextLesserByOneForFirstUnzero()
                );
        bulletsController.SetActionWhenAllBulletsColored(player.ChangeCanShootState);
        player.SetActionAfterShootingWhenBulletsZero(CompleteWave);
        player.SetActionAfterShootAllBullets(bulletsController.CheckBulletsForZero);
        matchThreeButtons.ButtonClosePanel.GetComponent<Button>().onClick.AddListener(player.ChangePanelClosedState);
        matchThreeButtons.ButtonOpenPanel.GetComponent<Button>().onClick.AddListener(player.ChangePanelClosedState);
    }

    private void CompleteWave()
    {
        if (waveIndex < saveLoadController.GetFullLevelData(saveLoadController.LastCompleteLevel).GetWaveCount())
        {
            waveIndex++; 
        }
        else if (waveIndex == saveLoadController.GetFullLevelData(saveLoadController.LastCompleteLevel).GetWaveCount() - 1)
        {
            saveLoadController.GetFullLevelData(saveLoadController.LastCompleteLevel).SetLevelComplete();
        }
        
        SetWaveData();
        spawnController.MovePreviousEnemyForward();
        spawnController.Spawn();
        statsController.SetStatsToSpawnedEnemy();
        player.ChangeCanShootState();
        bulletsController.SetAvalibleCountBullets();
    }

    #endregion private functions
}
