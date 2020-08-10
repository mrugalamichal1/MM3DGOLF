using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoulController : MonoBehaviour
{
    public GameFlowController gameFlowController;
    public MovementController movementController;

    void Start()
    {
        gameFlowController = GameObject.FindObjectOfType<GameFlowController>();
        movementController = GameObject.FindObjectOfType<MovementController>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameFlowController.Strikes += gameFlowController.levelPAR;
            movementController.new_player_normalize();

        }
    }

}
