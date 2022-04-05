using UnityEngine;

public class Player : MonoBehaviour
{
    public const int CAM_H_FACTOR = 4;
    public const int CAM_V_FACTOR = 2;
    public Camera cam;

    private Vector3 rod;
    private Vector3 camAngels;

    void Start()
    {
        camAngels = Vector3.zero;
    }

    void Update()
    {
        //float dH = Input.GetAxis("Mouse X") * CAM_H_FACTOR;
        //float dV = Input.GetAxis("Mouse Y") * CAM_V_FACTOR;
        //cam.transform.Rotate(-dV, dH, 0);

        camAngels.y += Input.GetAxis("Mouse X") * CAM_H_FACTOR;
        camAngels.x -= Input.GetAxis("Mouse Y") * CAM_V_FACTOR;
        cam.transform.eulerAngles = camAngels;
    }
}
