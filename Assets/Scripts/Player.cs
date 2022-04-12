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
    private Animator _animator;
    private float _camStartAngleY;
    private float _characterSpeed;

    void Start()
    {
        _camAngels = Vector3.zero;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _rod = _characterController.transform.position - cam.transform.position;
        _camStartAngleY = cam.transform.rotation.eulerAngles.y;
        _characterSpeed = 3;
    }

    [System.Obsolete]
    void Update()
    {
       // _animator.Update(0f);

        _camAngels.y += Input.GetAxis("Mouse X") * CAM_H_FACTOR;
        _camAngels.x -= Input.GetAxis("Mouse Y") * CAM_V_FACTOR;
        

        cam.transform.position = _characterController.transform.position -
            (Quaternion.EulerAngles(0, _camAngels.y - _camStartAngleY, 0) * _rod);

        //cam.transform.eulerAngles = _camAngels;
        //transform.eulerAngles = _camAngels;

        cam.transform.eulerAngles = _camAngels * 180 / Mathf.PI;
        //transform.eulerAngles = _camAngels * 180 / Mathf.PI;

        Vector3 playerAngles = Vector3.zero;
        playerAngles.y = (_camAngels * 180 / Mathf.PI).z;
        transform.eulerAngles = playerAngles;


        // Character move
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");

        _ccMove = (cam.transform.right * mH)
            + (cam.transform.forward * mV);
        _ = _characterController.SimpleMove(_ccMove * _characterSpeed);


        //Debug.Log("_ccMove: " + _ccMove);
        //Debug.Log(_characterController.velocity.magnitude);
        //Debug.Log("PlayerState: " + _animator.GetInteger("PlayerState"));

        foreach (AnimatorClipInfo clipInfo in _animator.GetCurrentAnimatorClipInfo(0))
        {
            Debug.Log(
           "name: "
           + clipInfo.clip.name
           + "\tnormalizedTime: "
           + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime
           + "\tlength: "
           + _animator.GetCurrentAnimatorStateInfo(0).length
           + "\tPlayerState: "
           + _animator.GetInteger("PlayerState"));
        }
        

        if (_characterController.velocity.magnitude > 0.4f)
        {
            if (mV == 0f)
            {
                _animator.SetInteger("PlayerState", 2);

                if (_animator.GetCurrentAnimatorStateInfo(0).length < _animator.GetCurrentAnimatorStateInfo(0).normalizedTime
                    && _animator.GetCurrentAnimatorStateInfo(0).IsName("LeftTurn"))
                {
                    _animator.SetInteger("PlayerState", 3);
                }
            }
            else
            {
                Jump(4, 1);
            }
        }
        else
        {
            Jump(4, 0);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale =
                Time.timeScale == 0
                ? 1
                : 0;
            Debug.Log(Time.timeScale);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            //var posX = transform.position.x + Random.Range(-10, 10);
            //var posZ = transform.position.z + Random.Range(-10, 10);
            //do
            //{
            //    posX = transform.position.x + Random.Range(-10, 10);
            //} while (posX > -5 && posX < 5);

            //do
            //{
            //    posZ = transform.position.z + Random.Range(-10, 10);
            //} while (posZ > -5 && posZ < 5);

            Vector3 posX;
            Vector3 posZ;

            do
            {
                posX = Vector3.left * Random.Range(-10, 10);
                posZ = Vector3.forward * Random.Range(-10, 10);
            } while ((posX - posZ).magnitude < 5f);
            


            Debug.Log($"x {posX}\t y {posZ}\t magnitude {(posX - posZ).magnitude}");
            //Instantiate(
            //    original: other.gameObject,
            //    position: ((Vector3.left * Random.Range(5, 10)) - (Vector3.forward * Random.Range(5, 10))).magnitude
            //    + other.gameObject.transform.position,
            //    rotation: Quaternion.identity);

            Instantiate(
              original: other.gameObject,
              position: posX - posZ + other.gameObject.transform.position,
              rotation: Quaternion.identity);

            Debug.Log("pos" + (posX - posZ + other.gameObject.transform.position));
            //Destroy(other.gameObject);
            //posX + posZ + other.gameObject.transform.position.x +,

        }
    }

    private void Jump(int playerStat, int backPlayerStat)
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetInteger("PlayerState", playerStat);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _animator.SetInteger("PlayerState", backPlayerStat);
        }
        else
        {
            _animator.SetInteger("PlayerState", backPlayerStat);
            //Debug.Log("MAX PASIVE");
        }
    }
}
