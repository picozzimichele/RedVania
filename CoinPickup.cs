using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    [SerializeField] int coinScorePoints = 200;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);//Playing the SFX clip at the Vector3 position of the camera
        FindObjectOfType<GameSession>().AddToScore(coinScorePoints);
        Destroy(gameObject);
    }
}
