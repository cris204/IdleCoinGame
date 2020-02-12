using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const int INITIAL_SPEED = 10;

    private int speed;
    private int speedLimit = 50;
    [SerializeField] private Vector2 playerVelocity;
    private Rigidbody2D rb;
    private CoinBehaviour currentCoin;


    [Header("AI")]
    private RaycastHit2D hit;
    public List<Transform> coinsPosition = new List<Transform>();
    public Transform currentCoinPosition;
    public bool isMoving;
    public float radius=30;
    public LayerMask layer;



    private void Awake()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.speed = INITIAL_SPEED;
    }

    void Update()
    {
        //this.Movement();
        this.GetNewTarget();
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

    #region AI

    private void GetNewTarget()
    {
        this.hit = Physics2D.CircleCast(this.transform.position, radius, this.transform.forward, radius, layer);
        if (this.hit && this.hit.transform != null) {
            if (!this.coinsPosition.Contains(this.hit.transform)) {
                this.coinsPosition.Add(this.hit.transform);
            }
        }

        if (this.currentCoinPosition == null && this.coinsPosition.Count > 0 && this.coinsPosition[0] != null && !this.isMoving) {
         
            this.currentCoinPosition = this.coinsPosition[0];

        } else if (this.currentCoinPosition != null && this.currentCoinPosition.gameObject.activeInHierarchy) {

            this.isMoving = true;
            this.transform.position = Vector2.MoveTowards(this.transform.position, this.currentCoinPosition.position, speed * Time.deltaTime);

        } else if(this.currentCoinPosition != null && !this.currentCoinPosition.gameObject.activeInHierarchy) {
            this.coinsPosition.Remove(currentCoinPosition);
            this.currentCoinPosition = null;
            this.isMoving = false;
        }

    }

    #endregion

    #region Collisions

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin") {
            GameManager.Instance.GainCoin();
            this.currentCoin = collision.GetComponent<CoinBehaviour>();
            if (this.currentCoin != null) {
                PlaySound currentCoinSound = AudioSourcePool.Instance.GetAudioSource().GetComponent<PlaySound>();
                if (currentCoinSound != null) {
                    currentCoinSound.PlayAudio("Coin");
                }
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

    public void RestartSpeed()
    {
        this.speed = INITIAL_SPEED;
    }

    public bool GetCanUpgradeSpeed()
    {
        return this.speed < this.speedLimit;
    }


    #endregion

}
