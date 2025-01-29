using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float hSpeed;
    [SerializeField] private float hMouseSpeed;

    private float horiInput;
    private float currentSpeed;

    [SerializeField] private float magnetPower;

    [SerializeField] private GameObject visuals;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
        GetMagnetInput();
        UpdateVisuals();
    }

    public void GetMovementInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float hMouse = Input.GetAxis("Mouse X");

        if (h != 0)
        {
            PlayerMoveWithButtons(h);
        }
        else if( hMouse!= 0)
        {
            PlayerMoveWithMouse(hMouse);
        }
    }
    
    public void PlayerMoveWithButtons(float h)
    {
        transform.position += Vector3.right * h * hSpeed * Time.deltaTime;
        float xPos = Mathf.Clamp(transform.position.x, -8.0f, 8.0f);
        transform.position = new Vector3(xPos, GameManager.Instance.BottomYLimit, 0);
    }

    public void PlayerMoveWithMouse(float h)
    {
        float mousePosInWorld = GameManager.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition).x;
        float xPos = Mathf.Clamp(mousePosInWorld, -8.0f, 8.0f);
        transform.position = new Vector3(xPos, GameManager.Instance.BottomYLimit, 0);
    }

    public void GetMagnetInput()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !Input.GetKey(KeyCode.Mouse1))
        {
            //Attract
            GameManager.Instance.PullBall(this.transform, magnetPower);

        }
        if (Input.GetKey(KeyCode.Mouse1) && !Input.GetKey(KeyCode.Mouse0))
        {
            //Repel
            GameManager.Instance.PushBall(this.transform, magnetPower);
        }
    }


    public void UpdateVisuals()
    {
        Vector2 lookDir = GameManager.Instance.Ball.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        visuals.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
