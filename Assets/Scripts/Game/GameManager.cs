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
    private int coins;

    [Header("Spawn")]
    private Vector2 size = new Vector2(24,24) ;
    private Vector2 currentPositionToSpawn;
    private GameObject currentCoin;
    private float timeElapsed = 5;

    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("Buy Speed")]
    [SerializeField] private TextMeshProUGUI speedTxt;
    [SerializeField] private TextMeshProUGUI speedCostTxt;
    private int upgradeSpeedCost = 4;

    [Header("Buy Multiplier")]
    [SerializeField] private TextMeshProUGUI multiplierTxt;
    [SerializeField] private TextMeshProUGUI multiplierCostTxt;
    private int multiplierCost = 4;
    private int coinMutiplier = 1;

    [Header("Buy SpawnRate")]
    [SerializeField] private TextMeshProUGUI spawnRateTxt;
    [SerializeField] private TextMeshProUGUI spawnRateCostTxt;
    private int spawnRateCost = 4;
    private float spawnRate = 1;



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
        this.UpdateSpeedTxt();
        this.UpdateMultiplierTxt();
        this.UpdateSpawnRateTxt();
    }

    void Update()
    {
        this.SpawnCoins();
    }

    public void GainCoin()
    {
        this.coins += 1 * this.coinMutiplier;
        this.UpdateCoinsTxt();
    }

    #region UpdateUI

    private void UpdateCoinsTxt()
    {
        this.coinsTxt.text = string.Format("Coins: {0}", this.coins);
    }

    private void UpdateSpeedTxt()
    {
        this.speedTxt.text = string.Format("UFO Speed: {0}", this.player.GetSpeed());
        this.speedCostTxt.text= string.Format("Cost: {0}", this.upgradeSpeedCost);
    }
    private void UpdateMultiplierTxt()
    {
        this.multiplierTxt.text = string.Format("Coin \nMultiplier: {0}", this.coinMutiplier);
        this.multiplierCostTxt.text = string.Format("Cost: {0}", this.multiplierCost);
    }

    private void UpdateSpawnRateTxt()
    {
        this.spawnRateTxt.text = string.Format("Spawn \nRate: {0}", this.spawnRate);
        this.spawnRateCostTxt.text = string.Format("Cost: {0}", this.spawnRateCost);
    }

    #endregion

    #region SpawnCoins
    private void SpawnCoins()
    {
        if (this.timeElapsed >= this.spawnRate) {
            currentPositionToSpawn = new Vector2(Random.Range(-size.x / 2, size.x / 2), Random.Range(-size.y / 2, size.y / 2));
            this.currentCoin = CoinPool.Instance.GetCoin();
            this.currentCoin.transform.position = currentPositionToSpawn;
            this.timeElapsed = 0;
        }
        this.timeElapsed += Time.deltaTime;
    }
    #endregion

    #region ButtonsEvents (Buy stats)

    public void BuyUFOSpeed()
    {
        if (this.coins >= this.upgradeSpeedCost) {
            this.player.UpgradeSpeed();
            this.SetCoins(this.coins - this.upgradeSpeedCost);
            
            this.upgradeSpeedCost++;
            this.UpdateCoinsTxt();
            this.UpdateSpeedTxt();

        } else {
            Debug.LogError("Can't upgrade the speed");
        }
    }

    public void BuyMultiplier()
    {
        if (this.coins >= this.multiplierCost) {
            this.coinMutiplier++;
            this.SetCoinMultiplier(this.coinMutiplier);
            this.SetCoins(this.coins - this.multiplierCost);

            this.multiplierCost++;
            this.UpdateCoinsTxt();
            this.UpdateMultiplierTxt();
        } else {
            Debug.LogError("Can't upgrade the multiplier");
        }
    }

    public void BuyUpgradeSpawnRate()
    {
        if (this.coins >= this.spawnRateCost) {
            this.spawnRate-=0.5f;
            this.SetSpawnRate(this.spawnRate);
            this.SetCoins(this.coins - this.spawnRateCost);

            this.spawnRateCost++;
            this.UpdateCoinsTxt();
            this.UpdateSpawnRateTxt();
        } else {
            Debug.LogError("Can't upgrade the Spawn Rate");
        }
    }

    #endregion

    #region Get&Set
    public int GetCoins()
    {
        return coins;
    }

    public void SetCoins(int value)
    {
        this.coins = value;
    }

    public int GetCoinMutiplier()
    {
        return coinMutiplier;
    }

    public void SetCoinMultiplier(int value)
    {
        this.coinMutiplier = value;
    }

    public float GetSpawnRate()
    {
        return this.spawnRate;
    }

    public void SetSpawnRate(float value)
    {
        this.spawnRate = value;
    }

    #endregion

}
