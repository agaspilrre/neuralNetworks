using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField]
    bool playerBullet;

    //destruye el objeto bala si colisiona con el player o con la IA
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !playerBullet) {

            Destroy(gameObject);

        }else if(other.tag == "IA" && playerBullet) {
            Destroy(gameObject);
        }
    }
}
