using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    private static CoinPool instance;

    public static CoinPool Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private GameObject coinObject;
    [SerializeField]
    private int size;
    private List<GameObject> coins;

    private void Awake()
    {
        if (instance == null) {
            instance = this;
            PrepareCoins();
        } else {
            Destroy(gameObject);
        }
    }

    private void PrepareCoins()
    {
        coins = new List<GameObject>();
        for (int i = 0; i < size; i++)
            AddCoin();
    }

    public GameObject GetCoin()
    {
        if (coins.Count == 0)
            AddCoin();
        return AllocateCoin();
    }

    public void ReleaseCoin(GameObject coin)
    {
        coin.gameObject.SetActive(false);
        coins.Add(coin);
    }

    private void AddCoin()
    {
        GameObject instance = Instantiate(coinObject);
		instance.transform.position=this.transform.position;
        instance.gameObject.SetActive(false);
        coins.Add(instance);
    }

    private GameObject AllocateCoin()
    {
        GameObject coin = coins[coins.Count - 1];
        coins.RemoveAt(coins.Count - 1);
        coin.gameObject.SetActive(true);
        return coin;
    }
}
