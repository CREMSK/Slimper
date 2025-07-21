using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab_;
    [SerializeField] private GameObject playerCursor_;

    [SerializeField] private Vector3 playerSpawnPoint_;

    private IMortal playerMortal_;

    void Awake()
    {
        StartLevel();
    }

    void OnDisable()
    {
        UnSubscribe();
    }

    private void StartLevel()
    {
        spawnPlayer();
        Instantiate(playerCursor_, playerSpawnPoint_, Quaternion.identity);
    }

    private void Subscribe()
    {
        playerMortal_.onPlayerDeath += onPlayerDeath;
    }

    private void UnSubscribe()
    {
        playerMortal_.onPlayerDeath -= onPlayerDeath;
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

        playerMortal_.onPlayerDeath += onPlayerDeath;
    }   
}
