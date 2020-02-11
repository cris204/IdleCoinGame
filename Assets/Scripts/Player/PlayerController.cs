using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private int speed = 10;
    [SerializeField] private Vector2 playerVelocity;
    private Rigidbody2D rb;
    private CoinBehaviour currentCoin;

    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        this.Movement();
    }


    private void Movement()
    {
        playerVelocity.x = Input.GetAxis("Horizontal") * speed;
        playerVelocity.y = Input.GetAxis("Vertical") * speed;
        this.rb.velocity = this.playerVelocity;
    }


    #region Stats

    public void UpgradeSpeed()
    {
        this.speed++;
    }

    #endregion

    #region Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin") {
            GameManager.Instance.GainCoin();
            this.currentCoin = collision.GetComponent<CoinBehaviour>();
            if (this.currentCoin != null) {
                this.currentCoin.ReturnToPool();
            }
        }
    }

    #endregion

    #region Get&Set
    public int GetSpeed()
    {
        return this.speed;
    }

    public void SetSpeed(int value)
    {
        this.speed = value;
    }


    #endregion

}
