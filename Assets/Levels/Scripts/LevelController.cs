using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab_;
    [SerializeField] private GameObject playerCursorPrefab_;
    [SerializeField] private PlayerCameraController playerCamera_;
    [SerializeField] private Vector3 playerSpawnPoint_;
    [SerializeField] private LevelExitTrigger levelExitTrigger_;
    [SerializeField] private string nextLevel_;

    private IMortal playerMortal_;

    void Awake()
    {
        StartLevel();
    }

    void OnEnable()
    {
        Subscribe();
    }

    void OnDisable()
    {
        UnSubscribe();
    }

    private void StartLevel()
    {
        spawnPlayer();
        Instantiate(playerCursorPrefab_, new Vector3(0, 0, -2), Quaternion.identity);
    }

    private void Subscribe()
    {
        //playerMortal_.onPlayerDeath += onPlayerDeath;
        levelExitTrigger_.onExitLevel += startNextLevel;
    }

    private void UnSubscribe()
    {
        playerMortal_.onPlayerDeath -= onPlayerDeath;
        levelExitTrigger_.onExitLevel -= startNextLevel;
    }

    private void onPlayerDeath()
    {
        StartCoroutine("playerDeathTimer", 1.0f);
    }

    private IEnumerator playerDeathTimer(float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        spawnPlayer();
    }

    private void spawnPlayer()
    {
        var player = Instantiate(playerPrefab_, playerSpawnPoint_, Quaternion.identity);
        playerMortal_ = player.GetComponent<PlayerSystemsController>();
        playerCamera_.playerTransform_ = player.transform;

        playerMortal_.onPlayerDeath += onPlayerDeath;
    }
    private IEnumerator nextLevelTimer(float time)
    {   
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene(nextLevel_);
    }

    private void startNextLevel()
    {
        playerCamera_.FadeIn();
        var nextLevel = nextLevelTimer(1f);
        StartCoroutine(nextLevel);
    }
}
