using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }



    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI coinsTxt;
    private float coins;

    [Header("Spawn")]
    private Vector2 size = new Vector2(24,24) ;
    private Vector2 currentPositionToSpawn;
    private GameObject currentCoin;
    private float spawnDelay = 5;
    private float timeElapsed = 5;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(instance.gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        this.SpawnCoins();
    }

    private void UpdateCoinsTxt()
    {
        this.coinsTxt.text = string.Format("Coins: {0}", coins);
    }


    #region SpawnCoins
    private void SpawnCoins()
    {
        if (this.timeElapsed >= this.spawnDelay) {
            currentPositionToSpawn = new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
            this.currentCoin = CoinPool.Instance.GetCoin();
            this.currentCoin.transform.position = currentPositionToSpawn;
            this.timeElapsed = 0;
        }
        this.timeElapsed += Time.deltaTime;
    }
    #endregion


    #region Get&Set
    public float Coins
    {
        get
        {
            return coins;
        }
    }

    public void GainCoin()
    {
        this.coins++;
        this.UpdateCoinsTxt();
    }

    #endregion

}
