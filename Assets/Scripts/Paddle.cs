using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    #region Singleton

    private static Paddle _instance;

    public static Paddle Instance => _instance;

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

    private Camera _mainCamera;
    private float _paddleInitialY;
    private float _defaultPaddleWidthInPixles = 200;
    private float _defaultLeftClamp = 100;
    private float _defaultRightClamp = 1820;
    private SpriteRenderer sr;

    public float bounceSideForce = 250;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = FindObjectOfType<Camera>();
        _paddleInitialY = this.transform.position.y;
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        float paddleShift = (_defaultPaddleWidthInPixles - ((_defaultPaddleWidthInPixles / 2) * this.sr.size.x)) / 2;
        float leftClamp = _defaultLeftClamp - paddleShift;
        float rightClamp = _defaultRightClamp + paddleShift;
        float mousePositionPixels = Mathf.Clamp(Input.mousePosition.x, leftClamp, rightClamp);
        float mousePositionWorldX = _mainCamera.ScreenToWorldPoint(new Vector3(mousePositionPixels, 0, 0)).x;
        this.transform.position = new Vector3(mousePositionWorldX, _paddleInitialY, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            Rigidbody2D ballRb = collision.gameObject.GetComponent<Rigidbody2D>();
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y);

            ballRb.velocity = Vector2.zero;

            float difference = paddleCenter.x - hitPoint.x;

            if(hitPoint.x < paddleCenter.x)
            {
                ballRb.AddForce(new Vector2(-(Mathf.Abs(difference * bounceSideForce)), BallsManager.Instance.initialBallSpeed));
            }
            else
            {
                ballRb.AddForce(new Vector2((Mathf.Abs(difference * bounceSideForce)), BallsManager.Instance.initialBallSpeed));
            }
        }
    }
}
