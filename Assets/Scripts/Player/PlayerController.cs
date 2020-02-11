using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 10;
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


}
