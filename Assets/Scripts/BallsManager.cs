using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour
{
    #region Singleton

    private static BallsManager _instance;

    public static BallsManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    [SerializeField]
    private Ball _ballPrefab;

    private Ball _initialBall;

    private Rigidbody2D _initialBallRb;

    public float initialBallSpeed = 250;

    public List<Ball> Balls { get; set; }

    private void Start()
    {
        InitBall();
    }

    private void Update()
    {
        if(!GameManager.Instance.isGameStarted)
        {
            //Align ball to the paddle position
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + 1f, 0);
            _initialBall.transform.position = ballPosition;

            if(Input.GetMouseButtonDown(0))
            {
                _initialBallRb.isKinematic = false;
                _initialBallRb.AddForce(new Vector2(0, initialBallSpeed));
                GameManager.Instance.isGameStarted = true;
            }
        }
    }
    private void InitBall()
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + 1f, 0);
        _initialBall = Instantiate(_ballPrefab, startingPosition, Quaternion.identity);
        _initialBallRb = _initialBall.GetComponent<Rigidbody2D>();

        this.Balls = new List<Ball>
        {
            _initialBall
        };
    }
}
