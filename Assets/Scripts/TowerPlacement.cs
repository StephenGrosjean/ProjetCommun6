using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private GameObject selector, tower;
    [SerializeField] private GameManager GM;
    [SerializeField] private int towerPrice;

    private bool canPlace, isSelected;
    public bool IsSelected {
        set { isSelected = value; }
    }

    private Transform place;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {
            if (isSelected) {
                Transform objectHit = hit.transform;
                if (objectHit.tag == "Tile") {
                    canPlace = true;
                    selector.SetActive(true);
                    selector.transform.position = objectHit.position;
                    place = objectHit;
                }
                else {
                    canPlace = false;
                    selector.SetActive(false);
                    place = null;
                }
            }
            else {
                canPlace = false;
                selector.SetActive(false);
                place = null;
            }

            if (Input.GetMouseButtonDown(0) && canPlace && GM.Money >= towerPrice) {
                isSelected = false;
                GM.Money -= towerPrice;
                Instantiate(tower, place.position, transform.rotation);
                GM.UpdateMoney();
            }
        }
    }
}
