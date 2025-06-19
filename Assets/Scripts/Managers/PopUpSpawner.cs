using System.Collections.Generic;
using UnityEngine;

public class PopUpSpawner : MonoBehaviour
{
    public static PopUpSpawner Instance { get; private set; }

    [Header("Core")]
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private int maxCount = 15;
    private Queue<GameObject> popUpList = new Queue<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < maxCount; i++)
        {
            GameObject popup = Instantiate(popupPrefab, Vector3.zero, Quaternion.identity);
            popup.SetActive(false);
            popup.transform.SetParent(transform);
            popUpList.Enqueue(popup);
        }
    }
    public void GetPopUp(Vector3 position,int damage)
    {
        if (popUpList.Count > 0)
        {
            var popUp = popUpList.Dequeue();
            popUp.transform.position = position;
            popUp.SetActive(true);
            popUp.GetComponent<PopUp>().StartAnim(damage);
        }
    }

    public void ReturnObject(GameObject gameObject)
    {
        popUpList.Enqueue(gameObject);
    }
}

