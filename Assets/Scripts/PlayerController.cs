using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float hSpeed;
    [SerializeField] private float hMouseSpeed;

    private float horiInput;
    private float currentSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GetMovementInput();
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
        float xPos = Mathf.Clamp(transform.position.x, -6.4f, 6.4f);
        transform.position = new Vector3(xPos, -4.5f, 0);
    }

    public void PlayerMoveWithMouse(float h)
    {
        float mousePosInWorld = GameManager.Instance.GetMainCamera().ScreenToWorldPoint(Input.mousePosition).x;
        float xPos = Mathf.Clamp(mousePosInWorld, -6.4f, 6.4f);
        transform.position = new Vector3(xPos, -4.5f, 0);
    }
}
