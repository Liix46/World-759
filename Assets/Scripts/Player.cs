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

    void Start()
    {
        _camAngels = Vector3.zero;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _rod = _characterController.transform.position - cam.transform.position;
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

        cam.transform.position = _characterController.transform.position -
            (Quaternion.EulerAngles(0, _camAngels.y - _camStartAngleY, 0) * _rod);

        Vector3 playerAngles = Vector3.zero;
        playerAngles.y = (_camAngels * 180 / Mathf.PI).z;
        transform.eulerAngles = playerAngles;


        // Character move
        float mH = Input.GetAxis("Horizontal");
        float mV = Input.GetAxis("Vertical");

        _ccMove = (cam.transform.right * mH)
            + (cam.transform.forward * mV);
        _ = _characterController.SimpleMove(_ccMove * _characterSpeed);


        //foreach (AnimatorClipInfo clipInfo in _animator.GetCurrentAnimatorClipInfo(0))
        //{
        //    Debug.Log(
        //   "name: "
        //   + clipInfo.clip.name
        //   + "\tnormalizedTime: "
        //   + _animator.GetCurrentAnimatorStateInfo(0).normalizedTime
        //   + "\tlength: "
        //   + _animator.GetCurrentAnimatorStateInfo(0).length
        //   + "\tPlayerState: "
        //   + _animator.GetInteger("PlayerState"));
        //}
        

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

        Vector3 targetPos = _coin.transform.position;

        //targetPos.y = cam.transform.position.y;
        //targetPos.z = cam.transform.position.z;

        Vector3 targetDir = targetPos - cam.transform.position;
        Vector3 forward = cam.transform.forward;

        float angleBetween = Vector3.SignedAngle(targetDir, forward, Vector3.up);

        print(angleBetween);

        Menu.AngleWatch = angleBetween;
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
            //Debug.Log("MAX PASIVE");
        }
    }

    private void DistanceToCoin(Vector3 positionPlayer, Vector3 postionCoin)
    {
        Menu.Distance = Math.Sqrt(
            Math.Pow(positionPlayer.x - postionCoin.x, 2)
            + Math.Pow(positionPlayer.z - postionCoin.z, 2)
            );
    }

}
