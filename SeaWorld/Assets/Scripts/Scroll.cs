using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Scroll : MonoBehaviour 
{
    [Header("Select amount of your objects")]
    [Range(1, 100)]
    public int amount;
    [Header("Select smooth speed")]
    [Range(0.05f, 0.5f)]
    public float smoothSpeed;

    [Header("Select distance between objects")]
    [Range(5, 100)]
    public int distance;

    [Header("Select names for your objects")]
    public string[] names;
	public GameObject[] obj;
    private GameObject[] instatiatedObj;
    private Vector2[] points;
    public GameObject parentScroll;
    public Text characterName;
    public bool canRotate = false;
    public Transform camTrans;
    private float smoothedX, smoothedScale;
    private Vector3[] defaultScale, bigScale;
    private bool isSelected = false;
    private string presentSelectedName;       //记录当前选中角色的名字
    private GameObject original;
    
    
    void Start()
    {
        presentSelectedName = names[0];
        original = SpawnerManager.Instance.FindOriginal(presentSelectedName);
        SpawnerManager.Instance.TraverseAllAndReplace(presentSelectedName, original, obj[0]);
        instatiatedObj = new GameObject[amount];
        points = new Vector2[amount + 1];
        defaultScale = new Vector3[amount];
        bigScale = new Vector3[amount];
        for(int i = 0; i < amount; i++)
        {
            
            if (i == 0) instatiatedObj[i] = Instantiate(obj[i], new Vector3(camTrans.position.x, parentScroll.transform.position.y, 70), Quaternion.Euler(0,90,0));
            if (i != 0) instatiatedObj[i] = Instantiate(obj[i], new Vector3(instatiatedObj[i-1].transform.position.x + distance,
                    instatiatedObj[i-1].transform.position.y, instatiatedObj[i-1].transform.position.z), Quaternion.Euler(0,90,0));
            instatiatedObj[i].transform.parent = parentScroll.transform;
            defaultScale[i] = new Vector3(instatiatedObj[i].transform.localScale.x  , instatiatedObj[i].transform.localScale.y , instatiatedObj[i].transform.localScale.z );
            bigScale[i] = new Vector3(instatiatedObj[i].transform.localScale.x * 4, instatiatedObj[i].transform.localScale.y * 4, instatiatedObj[i].transform.localScale.z * 4);
            InitState(instatiatedObj[i]);
            
        }
        for (int y = 0; y < amount + 1 ; y++)
        {
            if (y == 0) points[y] = new Vector2(parentScroll.transform.position.x + distance / 2, parentScroll.transform.position.y);
            if (y != 0) points[y] = new Vector2(points[y - 1].x - distance, parentScroll.transform.position.y);
        }

    }

    void Update()
    {

        try
        {
            for (int i = 0; i < amount; i++)
            {
                if (canRotate)
                {
                    instatiatedObj[i].transform.Rotate(0, 1, 0);
                }

                if (parentScroll.transform.position.x < points[i].x && parentScroll.transform.position.x > points[i + 1].x)
                {
                    smoothedX = Mathf.SmoothStep(parentScroll.transform.position.x, points[i].x - distance / 2, smoothSpeed);
                    smoothedScale = Mathf.SmoothStep(bigScale[i].x , defaultScale[i].x, smoothSpeed );
                    characterName.text = names[i];
                    if (isSelected)
                    {
                        ChangeToSelected(obj[i], names[i]);
                    }
                }
                else smoothedScale = Mathf.SmoothStep(defaultScale[i].x, bigScale[i].x, smoothSpeed);
                    instatiatedObj[i].transform.localScale = new Vector3(smoothedScale, smoothedScale, smoothedScale);
            }
        }
        catch
        {
        }
        parentScroll.transform.position = new Vector2(smoothedX, parentScroll.transform.position.y);

    }
    public void ButtonClick()
    {
        if(EventSystem.current.currentSelectedGameObject.name == "Buy") // CODE FOR "BUY" BUTTON
        {
            print("buy");
            // WRITE HERE
        }
        if(EventSystem.current.currentSelectedGameObject.name == "Select") // CODE FOR "SELECT" BUTTON
        {
            print("select");
            // WRITE HERE
            isSelected = true;
        }
    }

    public void ChangeToSelected(GameObject selected , string name)
    {
        var flockPrefabBefore = FlockManager.Instance.Flocks[0];
        FlockManager.Instance._flockPrefab.SetActive(false);
        var flockPrefab = GameObjectUtil.Instantiate(selected, Vector3.zero);
        FlockManager.Instance._flockPrefab = flockPrefab;
        FlockManager.Instance.Flocks.Remove(flockPrefabBefore);
        //先找到之前prefab，替换回去，再将目标prefab替换成现在选择的
        original = SpawnerManager.Instance.FindOriginal(presentSelectedName);
        SpawnerManager.Instance.TraverseAllAndReplace(name, original, selected);
        presentSelectedName = name;
        FlockManager.Instance.FlockPrefab = selected;
        FlockManager.Instance.Init();
        isSelected = false;
        UIManager.Instance.camAnim.StartBackToMainFromShop();
        


    }


    public void InitState(GameObject gameObject)
    {
        var flockAI = gameObject.GetComponent<FlockAI>();
        var destroyOffScreen = gameObject.GetComponent<DestroyOffscreen>();
        flockAI.CanMove = false;
        flockAI.isInFlock = false;
        destroyOffScreen.canShutDown = false;
    }

}
