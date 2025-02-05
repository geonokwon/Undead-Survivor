using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리팹 보관 변수
    public GameObject[] prefabs;
    // .. 풀 담당을 하는 리스트
    private List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int index = 0; index < pools.Length; index++) {
            pools[index] = new List<GameObject>();
        }
        Debug.Log(pools.Length);
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // .. 선택한 풀에 놀고 있는 (비활성화 된) 게임오브젝트 접근
        foreach (GameObject item in pools[index]) {
            if (!item.activeSelf) {
                // .. 발견하면 select 변수에 할당
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // .. 못 찾았으면 ?
        if (!select) {
            // .. 발견하면 select 변수에 할당
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }






}
