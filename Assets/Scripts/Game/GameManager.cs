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

    private const int START_ITEMS_COST = 4; 
    private const int START_MULTIPLIER_VALUE = 1; 
    private const int START_SPAWN_RATE_VALUE = 3; 
    private const int START_COINS_VALUE = 0; 

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI coinsTxt;
    private int coins;

    [Header("Spawn")]
    private Vector2 size = new Vector2(24,24);
    private Vector2 currentPositionToSpawn;
    private GameObject currentCoin;
    private float timeElapsed = 5;

    [Header("Player")]
    [SerializeField] private PlayerController player;

    [Header("Buy Speed")]
    [SerializeField] private TextMeshProUGUI speedTxt;
    [SerializeField] private TextMeshProUGUI speedCostTxt;
    [SerializeField] private Button buySpeedButton;
    private bool hasMaxSpeedLevel;

    private int upgradeSpeedCost;

    [Header("Buy Multiplier")]
    [SerializeField] private TextMeshProUGUI multiplierTxt;
    [SerializeField] private TextMeshProUGUI multiplierCostTxt;
    [SerializeField] private Button buyMultiplierButton;
    private int multiplierCost;
    private int coinMutiplier;
    private int coinMutiplierMax = 10;
    private bool hasMaxMultiplierLevel;

    [Header("Buy SpawnRate")]
    [SerializeField] private TextMeshProUGUI spawnRateTxt;
    [SerializeField] private TextMeshProUGUI spawnRateCostTxt;
    [SerializeField] private Button buySpawnRateButton;
    private int spawnRateCost;
    private float spawnRate;
    private float spawnRateMax = 0.4f;
    private bool hasMaxSpawnRateLevel;



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
        this.coins = START_COINS_VALUE;
        this.coinMutiplier = START_MULTIPLIER_VALUE;
        this.spawnRate = START_SPAWN_RATE_VALUE;

        this.upgradeSpeedCost = START_ITEMS_COST;
        this.multiplierCost = START_ITEMS_COST;
        this.spawnRateCost = START_ITEMS_COST;


        this.UpdateSpeedTxt();
        this.UpdateMultiplierTxt();
        this.UpdateSpawnRateTxt();
        this.UpdateButtonsState();
    }

    void Update()
    {
        this.SpawnCoins();
    }

    public void GainCoin()
    {
        this.coins += 1 * this.coinMutiplier;
        this.UpdateCoinsTxt();
        this.UpdateButtonsState();
    }

    #region UpdateUI

    private void UpdateCoinsTxt()
    {
        this.coinsTxt.text = string.Format("Coins: {0}", this.coins);
    }

    private void UpdateSpeedTxt()
    {
        if (!this.hasMaxSpeedLevel) {
            this.speedTxt.text = string.Format("UFO Speed: {0}", this.player.GetSpeed());
            this.speedCostTxt.text = string.Format("Cost: {0}", this.upgradeSpeedCost);
        } else {
            this.speedTxt.text = "UFO Speed: Max";
            this.speedCostTxt.text = "Max";
        }
    }
    private void UpdateMultiplierTxt()
    {
        if (!this.hasMaxMultiplierLevel) {
            this.multiplierTxt.text = string.Format("Coin \nMultiplier: {0}", this.coinMutiplier);
            this.multiplierCostTxt.text = string.Format("Cost: {0}", this.multiplierCost);
        } else {
            this.multiplierTxt.text = "Coin \nMultiplier: Max";
            this.multiplierCostTxt.text = "Max";
        }
    }

    private void UpdateSpawnRateTxt()
    {
        if (!this.hasMaxSpawnRateLevel) {
            this.spawnRateTxt.text = string.Format("Spawn \nRate: {0}", this.spawnRate);
            this.spawnRateCostTxt.text = string.Format("Cost: {0}", this.spawnRateCost);
        } else {
            this.spawnRateTxt.text = "Spawn \nRate: Max";
            this.spawnRateCostTxt.text = "Max";
        }
    }

    private void UpdateButtonsState()
    {
        if (!this.hasMaxSpeedLevel) {
            if (this.coins >= this.upgradeSpeedCost) {
                this.buySpeedButton.interactable = true;
            } else {
                this.buySpeedButton.interactable = false;
            }
        } else {
            this.buySpeedButton.interactable = false;
        }

        if (!this.hasMaxMultiplierLevel) {
            if (this.coins >= this.multiplierCost) {
                this.buyMultiplierButton.interactable = true;
            } else {
                this.buyMultiplierButton.interactable = false;
            }
        } else {
            this.buyMultiplierButton.interactable = false;
        }

        if (!this.hasMaxSpawnRateLevel) {
            if (this.coins >= this.spawnRateCost) {
                this.buySpawnRateButton.interactable = true;
            } else {
                this.buySpawnRateButton.interactable = false;
            }
        } else {
            this.buySpawnRateButton.interactable = false;
        }
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
        AudioSourcePool.Instance.GetAudioSource().GetComponent<PlaySound>().PlayAudio("Click");
        if (this.player.GetCanUpgradeSpeed()) {
            if (this.coins >= this.upgradeSpeedCost) {
                this.player.UpgradeSpeed();
                this.SetCoins(this.coins - this.upgradeSpeedCost);

                this.upgradeSpeedCost++;
                this.UpdateCoinsTxt();

                if (!this.player.GetCanUpgradeSpeed()) {
                    this.hasMaxSpeedLevel = true;
                    AudioSourcePool.Instance.GetAudioSource().GetComponent<PlaySound>().PlayAudio("MaxLevel");
                }

            }
        } else {
            this.hasMaxSpeedLevel = true;
            Debug.LogError("Speed is in Max Level");
        }
        this.UpdateButtonsState();
        this.UpdateSpeedTxt();
    }

    public void BuyMultiplier()
    {
        AudioSourcePool.Instance.GetAudioSource().GetComponent<PlaySound>().PlayAudio("Click");
        if (!this.hasMaxMultiplierLevel) {
            if (this.coins >= this.multiplierCost) {
                this.coinMutiplier++;
                this.SetCoinMultiplier(this.coinMutiplier);
                this.SetCoins(this.coins - this.multiplierCost);

                this.multiplierCost++;
                this.UpdateCoinsTxt();

                if (this.coinMutiplier >= this.coinMutiplierMax) {
                    this.hasMaxMultiplierLevel = true;
                    AudioSourcePool.Instance.GetAudioSource().GetComponent<PlaySound>().PlayAudio("MaxLevel");
                }
            }
        } else {
            Debug.LogError("multiplier is in Max Level");
        }
        this.UpdateButtonsState();
        this.UpdateMultiplierTxt();
    }

    public void BuyUpgradeSpawnRate()
    {
        AudioSourcePool.Instance.GetAudioSource().GetComponent<PlaySound>().PlayAudio("Click");
        if (!this.hasMaxSpawnRateLevel) {
            if (this.coins >= this.spawnRateCost) {
                this.spawnRate -= 0.3f;
                this.SetSpawnRate(this.spawnRate);
                this.SetCoins(this.coins - this.spawnRateCost);

                this.spawnRateCost++;
                this.UpdateCoinsTxt();
                if (this.spawnRate <= this.spawnRateMax) {
                    this.hasMaxSpawnRateLevel = true;
                    AudioSourcePool.Instance.GetAudioSource().GetComponent<PlaySound>().PlayAudio("MaxLevel");
                }
            }
        } else {
            Debug.LogError("Spawn Rate is in Max level");
        }
        this.UpdateButtonsState();
        this.UpdateSpawnRateTxt();
    }

    public void RestartValues()
    {
        AudioSourcePool.Instance.GetAudioSource().GetComponent<PlaySound>().PlayAudio("Click");
        this.player.RestartSpeed(); // start value
        this.coins = START_COINS_VALUE;
        this.coinMutiplier = START_MULTIPLIER_VALUE;
        this.spawnRate = START_SPAWN_RATE_VALUE;

        this.upgradeSpeedCost = START_ITEMS_COST;
        this.multiplierCost = START_ITEMS_COST;
        this.spawnRateCost = START_ITEMS_COST;

        this.hasMaxSpeedLevel = false;
        this.hasMaxMultiplierLevel = false;
        this.hasMaxSpawnRateLevel = false;

        this.UpdateCoinsTxt();
        this.UpdateSpeedTxt();
        this.UpdateMultiplierTxt();
        this.UpdateSpawnRateTxt();
        this.UpdateButtonsState();

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
