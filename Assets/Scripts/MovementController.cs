using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovementController : MonoBehaviour
{
    public GameObject player;
    public GameObject arrow;
    public GameObject mainCamera;
    public GameObject hole;

    public bool ball_needsNormalization = false;
    public bool newPlayer = false;
    public int Force = 0;

    public bool ManualCamera;

    public GameFlowController gameFlowController;
    public UIController uIController;


    public Vector3 StartingPos;
    public Vector3 StartingPosArrowOffset;
    public Quaternion StartingPosCameraAngle;
    public Vector3 cameraoffset = new Vector3(10f, -7f, 0f);


    public void Awake()
    {


        uIController = GameObject.FindObjectOfType<UIController>();
        gameFlowController = GameObject.FindObjectOfType<GameFlowController>();
        player = GameObject.Find("player");
        arrow = GameObject.Find("arrow");
        mainCamera = GameObject.Find("Main Camera");
        hole = GameObject.Find("hole");


        mainCamera.transform.position = player.transform.position - cameraoffset;
        StartingPos = player.transform.position;
        StartingPosArrowOffset = StartingPos - arrow.transform.position;
        StartingPosCameraAngle = mainCamera.transform.rotation;
        ManualCamera = true;
    }
    public void normalize_ball_position()
    {
        if (DetectMovement() && ball_needsNormalization && !newPlayer)
        {
            ManualCamera = true;
            player.GetComponent<Transform>().rotation = Quaternion.Euler(0, 0, 0);
            arrow.GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);
            mainCamera.transform.position = player.transform.position - cameraoffset;
            ball_needsNormalization = false;
            arrow.transform.position = player.transform.position - StartingPosArrowOffset;
            arrow.SetActive(true);
        }
    }

    public void new_player_normalize()
    {
        ManualCamera = true;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);
        player.GetComponent<Transform>().rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        player.transform.position = StartingPos;
        player.GetComponent<Rigidbody>().isKinematic = false;

        arrow.GetComponent<Transform>().rotation = Quaternion.Euler(0, 180, 0);
        arrow.transform.position = player.transform.position - StartingPosArrowOffset;
        arrow.SetActive(true);

        mainCamera.transform.rotation = StartingPosCameraAngle;
        mainCamera.transform.position = player.transform.position - new Vector3(10f, -7f, 0);

        newPlayer = false;
        ball_needsNormalization = false;

    }


    public void Strike()
    {
        if (player.GetComponent<Rigidbody>().isKinematic) Force = 0;
        if (DetectMovement() && gameFlowController.CanAct && !player.GetComponent<Rigidbody>().isKinematic)
        {

            if (Input.GetKey(KeyCode.Space))
            {
                if (Force >= 1500)
                {
                    Force = 1500;
                }
                else
                {
                    Force += 15;
                    uIController.IncreasePowerBar();
                }

            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ManualCamera = false;
                arrow.SetActive(false);
                player.GetComponent<Rigidbody>().AddRelativeForce(Force * Time.deltaTime, 0, 0, ForceMode.Impulse);
                gameFlowController.Strikes++;   
                Force = 0;
                uIController.ResetPowerBar();
                StartCoroutine(Sleep_ball());
            }
            
        }
    }


    public bool DetectMovement()
    {
        if (player.GetComponent<Rigidbody>().velocity == new Vector3(0, 0, 0))
        {
            return true;
        }
        return false;
    }

    public void RotateBall()
    {
        if (DetectMovement() && gameFlowController.CanAct)
        {
            player.GetComponent<Rigidbody>().isKinematic = false;
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                player.GetComponent<Rigidbody>().isKinematic = true;
                player.GetComponent<Transform>().RotateAround(
                    new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y, player.GetComponent<Transform>().position.z),
                    new Vector3(0, player.GetComponent<Transform>().position.y, 0),
                    -2.5f);
                arrow.GetComponent<Transform>().RotateAround(
                    new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y, player.GetComponent<Transform>().position.z),
                    new Vector3(0, player.GetComponent<Transform>().position.y, 0),
                    -2.5f);


            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                player.GetComponent<Rigidbody>().isKinematic = true;

                player.GetComponent<Transform>().RotateAround(
                    new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y, player.GetComponent<Transform>().position.z),
                    new Vector3(0, player.GetComponent<Transform>().position.y, 0),
                    2.5f);
                arrow.GetComponent<Transform>().RotateAround(
                    new Vector3(player.GetComponent<Transform>().position.x, player.GetComponent<Transform>().position.y, player.GetComponent<Transform>().position.z),
                    new Vector3(0, player.GetComponent<Transform>().position.y, 0),
                    2.5f);

            }

        }


    }

    public void StopSlowBall()
    {
        if (player.GetComponent<Rigidbody>().velocity.magnitude < 0.15)
            player.GetComponent<Rigidbody>().velocity = new Vector3(0, player.GetComponent<Rigidbody>().velocity.y, 0);
    }


    private IEnumerator Sleep_ball()
    {
        yield return new WaitForSeconds(0.1f);
        ball_needsNormalization = true;
    }




    public void Update()
    {
        Strike();
        //RotateBall();


    }
    public void FixedUpdate()
    {
        DetectMovement();
        normalize_ball_position();
        StopSlowBall();
        
    }

    

    public void LateUpdate()
    {
        if (ManualCamera && DetectMovement())
        {
            Quaternion camangle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 2, Vector3.up);
            cameraoffset = camangle * cameraoffset;
            Vector3 newpos = player.transform.position - cameraoffset;
            mainCamera.transform.position = Vector3.Slerp(mainCamera.transform.position, newpos, 0.5f);
            mainCamera.transform.LookAt(player.transform.position);
        }
        RotateBall();
    }
}
