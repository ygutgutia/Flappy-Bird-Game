using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] //Adds a rigidbody element to a game object without it.
public class TapController : MonoBehaviour
{

    public delegate void PlayerDelegate();
    public static event PlayerDelegate OnPlayerDied;
    public static event PlayerDelegate OnPlayerScored;

    public float tapForce = 150;
    public float tiltSmooth = 3; //For rotating bird whern it falls
    public Vector3 startPos; //Restart position after it gets out

    GameManager game;

    Rigidbody2D rigidbody;
    Quaternion downRotation;
    Quaternion forwardRotation;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -40); //Converts vector3 normal angles into quaternion angles
        forwardRotation = Quaternion.Euler(0, 0, 70);
        game = GameManager.Instance;
    }

    void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
    }

    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;//Falsed when player dies
    }

    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;//Check
        transform.rotation = Quaternion.identity;


    }
    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void Update()
    {
        if (game.GameOver) return;//No Update If Game is over(Tilt problem when restart is pressed
        if(Input.GetMouseButton(0))//0 is for LMB and tap on android, 1 is for RMB
        {
            transform.rotation = forwardRotation;
            rigidbody.velocity = Vector3.zero; //Otherwise Force keeps adding up
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);//2nd parameter is type of force(acc change or velocity change)

        }
        transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmooth * Time.deltaTime);//Transit from initial position to falling position
    }

     void OnTriggerEnter2D(Collider2D col) //Called when GameObj collides with another GameObj(col)
    {
        if(col.gameObject.tag=="ScoreZone")
        {
            OnPlayerScored();//Event sent to Game Manager
            //Increase Score
        }
        if (col.gameObject.tag == "DeadZone")
        {
            rigidbody.simulated = false;
            //Endgame
            OnPlayerDied();//Same
        }
    }
}
