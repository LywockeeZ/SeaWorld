using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public List<Transform> Flocks = new List<Transform>();
    public string controllingFlockRank;
    public float moveSpeed = 1f;
    public GameObject FlockPrefab;


    public Vector3 flockCenter { get; set; }
    public Vector3 mousePos { get; set; }
    public Vector3 flockDirection { get; set; }
    public Quaternion flockRotation { get; set; }
    public float shutDownBoundary = 45f;
    public float visualBoundary = 10f;
    public Vector3 visualBoundaryOffset = Vector3.zero;
    public GameObject _flockPrefab { get; set; }
    public float yaw = 0;
    public float pitch = 0;

    protected static FlockManager _instance;
    public static FlockManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<FlockManager>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<FlockManager>();
                }
            }
            return _instance;
        }
    }

    void Awake()
    {
        _flockPrefab = GameObjectUtil.Instantiate(FlockManager.Instance.FlockPrefab, Vector3.zero);
    }

    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        flockCenter = CalculateCenter();
        mousePos = GetMousePosition();
        
        if (Flocks.Count != 0)
        {
            flockRotation = Flocks[0].rotation;
            //yaw = Flocks[0].GetComponent<SharkController>().yaw;
            //pitch = Flocks[0].GetComponent<SharkController>().pitch;
        }
        
    }


    public void Init()
    {
        var _flockAI = _flockPrefab.GetComponent<FlockAI>();
        Flocks.Add(_flockPrefab.transform);
        _flockPrefab.layer = 0;
        flockDirection = new Vector3(1,0,0);
        flockRotation = _flockPrefab.transform.rotation;
        controllingFlockRank = _flockAI.Rank;
        _flockAI.CanMove = false;
        _flockAI.isInFlock = true;
        flockCenter = _flockPrefab.transform.position;
    }

    //计算鱼群中心坐标
    public Vector3 CalculateCenter()
    {
        var centerBefore = flockCenter;
        Vector3 center = Vector3.zero;
        if (Flocks.Count != 0)
        {
            foreach (Transform item in Flocks)
            {
                center += item.position;
            }
            center /= Flocks.Count;
        }
        else
        {
            center = centerBefore;
        }

        return center;
    }

    //获得鼠标位置
    public Vector3 GetMousePosition()
    {

            //如果是正交摄像机用以下代码
            //mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            LayerMask mask = 1 << LayerMask.NameToLayer("Mask");
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 200, mask))
            {
                mousePos = hitInfo.point;
            }
        
        return mousePos;
    }

    //每点击一次，刷新一次方向
    public Vector3 GetDirection()
    {
        if (Input.GetMouseButton(0))
        {
            flockDirection = mousePos - flockCenter;
        }
        
        return flockDirection;
    }


    public void SetPitchAndYaw()
    {

            yaw += Input.GetAxis("Mouse X") * 0.1f;
            pitch -= Input.GetAxis("Mouse Y") * 0.1f;
            yaw = Mathf.Clamp(yaw, -1f, 1f);
            pitch = Mathf.Clamp(pitch, -1f, 1f);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(flockCenter + visualBoundaryOffset, visualBoundary);
        Gizmos.DrawWireSphere(flockCenter + visualBoundaryOffset, shutDownBoundary);
    }
}
