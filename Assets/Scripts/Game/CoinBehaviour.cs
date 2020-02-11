using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    
    public void ReturnToPool()
    {
        Destroy(this.gameObject);
    }


}
