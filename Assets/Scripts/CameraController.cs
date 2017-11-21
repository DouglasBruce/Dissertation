using UnityEngine;

public class CameraController : MonoBehaviour {

    public float scrollSpeed = 15f;
    public float scrollEdge = 0.02f;
    public float panSpeed = 10f;

    public Vector2 zoomRange = new Vector2(-5, 10);
    public float currentZoom = 0f;
    public float zoomSpeed = 1f;
    public float zoomRotation = 1f;
    public Vector2 zoomAngleRange = new Vector2(20, 40);

	public float minX;
	public float maxX;
	public float minZ;
	public float maxZ;

    private Vector3 initialPosition;
    private Vector3 initialRotation;

    public bool doMovement = true;

    // Use this for initialization
    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollX = Input.GetAxis("Horizontal");
        float scrollY = Input.GetAxis("Vertical");

        if (GameManager.GameIsOver)
        {
            this.enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            doMovement = !doMovement;
        }

        if (!doMovement)
        {
            return;
        }
        
        if (Input.GetMouseButton(0))
        {
            transform.Translate(Vector3.right * Time.deltaTime * panSpeed * (Input.mousePosition.x - Screen.width * 0.5f) / (Screen.width * 0.5f), Space.World);
            transform.Translate(Vector3.forward * Time.deltaTime * panSpeed * (Input.mousePosition.y - Screen.height * 0.5f) / (Screen.height * 0.5f), Space.World);
        }
        else
        {
            if (scrollX > 0 || Input.mousePosition.x >= Screen.width * (1 - scrollEdge))
            {
                transform.Translate(Vector3.right * Time.deltaTime * panSpeed, Space.World);
            }
            else if (scrollX < 0 || Input.mousePosition.x <= Screen.width * scrollEdge)
            {
                transform.Translate(Vector3.left * Time.deltaTime * panSpeed, Space.World);
            }

            if (scrollY > 0 || Input.mousePosition.y >= Screen.height * (1 - scrollEdge))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * panSpeed, Space.World);
            }
            else if (scrollY < 0 || Input.mousePosition.y <= Screen.height * scrollEdge)
            {
                transform.Translate(Vector3.back * Time.deltaTime * panSpeed, Space.World);
            }
        }
		
		Vector3 tempPos = transform.position;
		tempPos.x = Mathf.Clamp(transform.position.x, minX, maxX);
		tempPos.z = Mathf.Clamp(transform.position.z, minZ, maxZ);
		transform.position = tempPos;

        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime * 1000 * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, zoomRange.x, zoomRange.y);

        transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - (initialPosition.y + currentZoom)) * 0.1f, transform.position.z);

        float x = transform.eulerAngles.x - (transform.eulerAngles.x - (initialRotation.x + currentZoom * zoomRotation)) * 0.1f;
        x = Mathf.Clamp(x, zoomAngleRange.x, zoomAngleRange.y);

        transform.eulerAngles = new Vector3(x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
