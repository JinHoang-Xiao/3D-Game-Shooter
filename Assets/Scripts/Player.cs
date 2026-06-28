using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement data")]
    public float moveSpeed;
    public float rotationSpeed;


    private float verticalInput;
    private float horizontalInput;
    [Space]
    public LayerMask whatIsAimMask;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame // ví dụ như 120 fps thì sẽ là 120 mỗi 1s
    void Update()
    {
       UpdateAim();
       verticalInput = Input.GetAxis("Vertical");
       horizontalInput = Input.GetAxis("Horizontal");

       if(verticalInput<0)
        {
            horizontalInput = -Input.GetAxis("Horizontal");
        }
    }
    // Chỉ số timestep trong mục project settings -> time
    private void FixedUpdate()
    {
        Vector3 movement = transform.forward * moveSpeed * verticalInput;
        rb.linearVelocity = movement;


        transform.Rotate(0,horizontalInput * rotationSpeed,0);
    }

    private void UpdateAim()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, whatIsAimMask))
        {
            Debug.Log(hit.point);
        }
    }
}
