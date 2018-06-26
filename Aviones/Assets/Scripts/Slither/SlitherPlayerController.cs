using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class SlitherPlayerController : MonoBehaviour
{

    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    float playerSpeed;

    [SerializeField]
    Text puntuationPlayer;

    

    [SerializeField]
    GameObject bulletPrefab;

    public static SlitherPlayerController instance;

    public float puntuation { get; set; }

    int direction;

    public int activePlayer;
    int inActivePlayer;

    public List<Transform> aircraftList;

    public bool IsSecondActive { get; set; }

    float height;
    float width;

    public bool isAttcking;

    Camera mainCamera;

    [SerializeField]
    float airPlaneSize;

    //inicializamos variables con sus valores correspondientes
    private void Awake()
    {
        isAttcking = false;
        instance = this;
        mainCamera = Camera.main;

        height = mainCamera.orthographicSize;
        width = (height * mainCamera.aspect);

        activePlayer = 0;
        inActivePlayer = 1;

        direction = 1;

        IsSecondActive = false;
    }

    //Comprueba los limites de la camara
    //detecta los inputs para el manejo de player
    void Update()
    {
        CameraLimits();
        #region inputs
        if (Input.GetKey(KeyCode.D))
        {
            UpdateDirection(1);
        }
        if(Input.GetKey(KeyCode.A))
        {
            UpdateDirection(-1);
        }
        if(Input.GetKey(KeyCode.W))
        {
            aircraftList[activePlayer].position += aircraftList[activePlayer].up * Time.deltaTime * playerSpeed;

            if (IsSecondActive)
            {
                aircraftList[inActivePlayer].position += aircraftList[inActivePlayer].up * Time.deltaTime * playerSpeed;
            }
        }

        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Attack();
        }
        #endregion
    }

    //asigna la misma rotacion a las dos naves player
    void UpdateDirection(int direction)
    {
        aircraftList[0].Rotate(0, 0, -direction * rotationSpeed * Time.deltaTime);
        aircraftList[1].Rotate(0, 0, -direction * rotationSpeed * Time.deltaTime);
    }

    //incrementa la puntuacion del player al recoger la pildora
    public void IncreaseScale()
    {
        puntuation++;
        puntuationPlayer.text = "Player: " + puntuation.ToString();     
    }

    //Activa el aeroplano que este dentro de la pantalla
    public int ChangeAircraft()
    {
        switch (activePlayer)
        {
            case 0:
                activePlayer = 1;
                inActivePlayer = 0;
                return activePlayer;
            case 1:
                activePlayer = 0;
                inActivePlayer = 1;
                return activePlayer;
        }

        return 0;
    }

    //Instancia las balas del player, mismo funcionamiento que en enemigo
    private void Attack()
    {
        if(puntuation > 0) {

            puntuation--;
            puntuationPlayer.text = "Player: " + puntuation.ToString();

            isAttcking = true;

            GameObject bullet = Instantiate(bulletPrefab, aircraftList[activePlayer].position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().AddForce(aircraftList[activePlayer].up * 10, ForceMode2D.Impulse);

            Destroy(bullet, 1f);
            Invoke("CancelAttack", 1);
        }
    }
    //cambia el valor del bool isAttacking a false
    private void CancelAttack()
    {
        isAttcking = false;
    }

    //Mismo funcionamiento que la funcion enemiga pero aplicada al player
    private void CameraLimits()
    {
        #region check if player leaves window
        if (aircraftList[activePlayer].position.x >= width)
        {
            inActivePlayer = activePlayer;

            activePlayer = ChangeAircraft();
            IsSecondActive = true;
            aircraftList[activePlayer].position = new Vector3(-width, aircraftList[inActivePlayer].position.y, 0);
        }
        else if (aircraftList[activePlayer].position.x <= -width)
        {
            inActivePlayer = activePlayer;

            activePlayer = ChangeAircraft();
            IsSecondActive = true;
            aircraftList[activePlayer].position = new Vector3(width, aircraftList[inActivePlayer].position.y, 0);
        }
        else if (aircraftList[activePlayer].position.y >= height)
        {
            inActivePlayer = activePlayer;

            activePlayer = ChangeAircraft();
            IsSecondActive = true;
            aircraftList[activePlayer].position = new Vector3(aircraftList[inActivePlayer].position.x, -height, 0);
        }
        else if (aircraftList[activePlayer].position.y <= -height)
        {
            inActivePlayer = activePlayer;

            activePlayer = ChangeAircraft();
            IsSecondActive = true;
            aircraftList[activePlayer].position = new Vector3(aircraftList[inActivePlayer].position.x, height, 0);
        }
        #endregion


        #region stop the player out of window
        if (aircraftList[inActivePlayer].position.x >= width + airPlaneSize)
        {
            IsSecondActive = false;
        }
        else if (aircraftList[inActivePlayer].position.x <= -width - airPlaneSize)
        {
            IsSecondActive = false;
        }
        else if (aircraftList[inActivePlayer].position.y >= height + airPlaneSize)
        {
            IsSecondActive = false;
        }
        else if (aircraftList[inActivePlayer].position.y <= -height - airPlaneSize)
        {
            IsSecondActive = false;
        }
        #endregion
    }
}

