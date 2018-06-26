using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPrefab : MonoBehaviour
{
    float height;
    float width;

    float scale;

    [SerializeField]
    float scaleMin;
    [SerializeField]
    float scaleMax;

    private void Awake()
    {
        height = Camera.main.orthographicSize;
        width = (height * Camera.main.aspect);

        Repositionate();
    }

    //Detecta si una pildora es cogida por el player o la IA
    // llama a la funcion reposicionar
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            collider.GetComponentInParent<SlitherPlayerController>().IncreaseScale();
            Repositionate();
        }
        else if(collider.tag == "IA")
        {
            collider.GetComponentInParent<Enemigo>().IncreaseScale();
            Repositionate();
        }
    }
    //Distribuye pildoras entre los limites de la camara
    private void Repositionate()
    {
        transform.position = new Vector2(Random.Range(width, -width), Random.Range(height, -height));

        scale = Random.Range(scaleMin, scaleMax);
        transform.localScale = new Vector3(scale, scale, 1);
    }
}
