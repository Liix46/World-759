using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public const int CAM_H_FACTOR = 4;
    public const int CAM_V_FACTOR = 2;

    private float COIN_SPAWN_OFFSET_Y = 1.5f;

    public Camera cam;
    public GameObject coinPrefab;

    private Vector3 _rod;
    private float rodScale;
    private Vector3 _camAngels;
    private Vector3 _ccMove;

    private CharacterController _characterController;
    private Animator _animator;
    private float _camStartAngleY;
    private float _characterSpeed;

    private GameObject toDestroy;
    private float timeout;

    private GameObject _coin;
    private GameObject _player;

    private GameObject pivot;

  

    void Start()
    {
        _camAngels = Vector3.zero;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        pivot = GameObject.Find("CamPivot");
        _rod = pivot.transform.position - cam.transform.position;
        rodScale = 1;

        _camStartAngleY = cam.transform.rotation.eulerAngles.y;
        _characterSpeed = 3;
        toDestroy = null;

        _player = GameObject.Find("Player");
        //first coin
        _coin = GameObject.FindGameObjectWithTag("Coin");

        if (_coin != null)
        {
            //Terrain height @ coin position

            float terrainHeight = Terrain.activeTerrain.SampleHeight(_coin.transform.position);

            // Coin height Offset : position height - Terrarian height @ coin position

            COIN_SPAWN_OFFSET_Y = _coin.transform.position.y - terrainHeight;
        }
        else
        {
            COIN_SPAWN_OFFSET_Y = 1.5f;
        }

        DistanceToCoin(_player.transform.position, _coin.transform.position);

    }

    [Obsolete]
    void Update()
    {
        _camAngels.y += Input.GetAxis("Mouse X") * CAM_H_FACTOR;

        //Debug.Log(_camAngels.x);
        float my = Input.GetAxis("Mouse Y") * CAM_V_FACTOR;
        if (_camAngels.x - my < 1 && _camAngels.x  - my > -1)
        {
            _camAngels.x -= my;
        }

        cam.transform.eulerAngles = _camAngels * 180 / Mathf.PI;

        cam.transform.position = pivot.transform.position -
            (Quaternion.EulerAngles(0, _camAngels.y - _camStartAngleY, 0) * _rod * rodScale);

        Vector3 playerAngles = Vector3.zero;
        playerAngles.y = (_camAngels * 180 / Mathf.PI).z;
        transform.eulerAngles = playerAngles;


        // Character move
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");

        _ccMove = (cam.transform.right * mH)
            + (cam.transform.forward * mV);
        _ = _characterController.SimpleMove(_ccMove * _characterSpeed);
        
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

        DistanceToCoin(_player.transform.position, _coin.transform.position);
    }

    [Obsolete]
    private void LateUpdate()
    {
        if (toDestroy != null)
        {
            if (timeout <= 0)
            {
                GameObject.Destroy(toDestroy);
                toDestroy = null;
            }
            else timeout -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log(
                Terrain.activeTerrain.SampleHeight(this.transform.position)
                + " "
                + Terrain.activeTerrain.terrainData.GetHeight(
                    (int) this.transform.position.x,
                    (int)this.transform.position.y)
                );
        }

        Menu.AngleWatch = CamCoinAngle();

        Vector2 msd = Input.mouseScrollDelta;
        if (msd.magnitude > 0)
        {
            // print(msd);
          
            if (msd.y > 0)
            {
                //_rod -= _rod.normalized / 0.1f;
                if (rodScale > 0.3f)
                {
                    if (rodScale > 1.0f)
                        rodScale -= 0.2f;
                    else
                        rodScale -= 0.1f;
                    print("A  " + rodScale);
                }
               // print("A  " + cam.transform.position.y);
            }
            if (msd.y < 0)
            {
                if (rodScale < 2.0f)
                {
                    if (rodScale > 1.0f && rodScale < 1.9f)
                        rodScale += 0.2f;
                    else
                        rodScale += 0.1f;
                    
                    print("B  " + rodScale);
                }
                //print("B  " + cam.transform.position.y);
            }
           
        }
        if (Input.GetMouseButtonDown(1))
        {
            // Debug.Log("Pressed secondary button.");
            rodScale = 1;
        }
            

        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            if (toDestroy == null)
            {
                other.gameObject.GetComponent<Animator>().SetBool("Disaper", true);

                Vector3 spawnPosition = Vector3.zero;

                do
                {
                    spawnPosition.Set(
                        transform.position.x + Random.Range(-10, 10),
                        transform.position.y,
                        transform.position.z + Random.Range(-10, 10)
                        );
                } while ((spawnPosition - transform.position).magnitude < 5f);

                spawnPosition.y = COIN_SPAWN_OFFSET_Y + Terrain.activeTerrain.SampleHeight(spawnPosition);

                GameObject coin = Instantiate(
                    original: coinPrefab,
                    position: spawnPosition,
                    rotation: Quaternion.identity);

                toDestroy = other.transform.parent.gameObject;
                timeout = 1.5f; // time animation coin

                _coin = coin;
                DistanceToCoin(_player.transform.position, _coin.transform.position);
            }

            Menu.Count++;
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
        }
    }

    private void DistanceToCoin(Vector3 positionPlayer, Vector3 postionCoin)
    {
        Menu.Distance = Math.Sqrt(
            Math.Pow(positionPlayer.x - postionCoin.x, 2)
            + Math.Pow(positionPlayer.z - postionCoin.z, 2)
            );
    }


    /*
     * Angle Between Camera.forward and direction to coin
     */
    private float CamCoinAngle()
    {
        //transform.position - player position
        //_coin.transform.position - coin position
        //cam.transform.position - camera "sight" direction
        //float angle = Acos(v1,v2) / |v1||v2| )
        //v3 = [v1  x  v2] - up(+) down(-) 
        Vector3 v1 = cam.transform.forward;
        v1.y = 0;
        Vector3 v2 = _coin.transform.position - transform.position;
        v2.y = 0;

        float angle = Mathf.Acos(Vector3.Dot(v1,v2) / v1.magnitude / v2.magnitude);
        Vector3 v3 = Vector3.Cross(v1, v2);

        if (v3.y > 0)
        {
            angle = -angle;
        }

        return angle * 180 / Mathf.PI;
    }
}
