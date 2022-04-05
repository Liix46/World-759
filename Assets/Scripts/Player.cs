using UnityEngine;

public class Player : MonoBehaviour
{
    public const int CAM_H_FACTOR = 4;
    public const int CAM_V_FACTOR = 2;
    public Camera cam;

    private Vector3 _rod;
    private Vector3 _camAngels;
    private Vector3 _ccMove;

    private CharacterController _characterController;
    private float _camStartAngleY;
    private float _characterSpeed;

    void Start()
    {
        _camAngels = Vector3.zero;
        _characterController = GetComponent<CharacterController>();
        _rod = _characterController.transform.position - cam.transform.position;
        _camStartAngleY = cam.transform.root.eulerAngles.y;

        _characterSpeed = 3;
    }

    [System.Obsolete]
    void Update()
    {

        //float dH = Input.GetAxis("Mouse X") * CAM_H_FACTOR;
        //float dV = Input.GetAxis("Mouse Y") * CAM_V_FACTOR;
        //cam.transform.Rotate(-dV, dH, 0);

        _camAngels.y += Input.GetAxis("Mouse X") * CAM_H_FACTOR;
        _camAngels.x -= Input.GetAxis("Mouse Y") * CAM_V_FACTOR;
        cam.transform.eulerAngles = _camAngels;

        cam.transform.position = _characterController.transform.position -
            (Quaternion.EulerAngles(0, _camAngels.y - _camStartAngleY, 0) * _rod);

        // Character move
        _ccMove = (cam.transform.right * Input.GetAxis("Horizontal"))
            + (cam.transform.forward * Input.GetAxis("Vertical"));
        _ = _characterController.SimpleMove(_ccMove * _characterSpeed);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale =
                Time.timeScale == 0
                ? 1
                : 0;
            Debug.Log(Time.timeScale);
        }
    }
}
