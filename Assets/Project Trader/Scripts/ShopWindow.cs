﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ProjectTrader;
using ProjectTrader.Datas;
using ProjectTrader.SpriteDatas;


public class ShopWindow : MonoBehaviour
{
    //플레이어 돈을 받아와서 구매처리하는 코드도 당연히 필요

    public GameObject shopWindow;
    public GameObject shopslot;
    public GameObject shopPopup;

    //슬롯에 필요
    GameObject[] shopItem;
    Item[] shopItemInfo;
    ItemData[] shopItemData;

    int[] buyNum;          //플레이어가 구매한 개수
    int[] itemMaxnum;      //구매가능한 개수(max)
    float[] timeDelay;
    bool setslot = false;  //슬롯세팅이 되었는지
    bool setwindow = false;
    Image[] slotImage;
    TextMeshProUGUI[] slottext;
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetShopslot();
        }
    }

    public void OpenShopWindow()
    {
        shopWindow.SetActive(true);
        if (setslot == false)
        {
            SetShopslot();
            setslot = true;
        }
    }

    public void CloseShopWindow()
    {
        setwindow = false;
        shopWindow.SetActive(false);
    }

    //슬롯
    void SetShopslot()
    {
        //임시 지정
        shopItem = new GameObject[5];
        shopItemInfo = new Item[5];
        shopItemData = new ItemData[5];
        buyNum = new int[5];
        itemMaxnum = new int[5];
        timeDelay = new float[5];

        for(int i = 0; i < 5; i++)
        {
            //임시 지정
            shopItemInfo[i].Code = i + 1;
            shopItemData[i] = shopItemInfo[i].GetData();
            buyNum[i] = 0;
            itemMaxnum[i] = 30;
            timeDelay[i] = 125f;

            shopItem[i] = Instantiate(shopslot) as GameObject;
            //슬롯세팅
            SetshopslotData(i);
            SlotInDataSet(i);
            SlotImage(i);
            shopItem[i].transform.SetParent((GameObject.Find("ShopContent")).transform);
            shopItem[i].transform.localScale = Vector3.one;
            
        }

    }
    
    //슬롯초기화
    public void SetshopslotData(int i)
    {
        slottext=shopItem[i].GetComponentsInChildren<TextMeshProUGUI>();
        slottext[0].text = shopItemData[i].Name;
        slottext[1].text = (itemMaxnum[i] - buyNum[i]).ToString() + "/" + itemMaxnum[i].ToString();
        slottext[2].text = "00:00";
        slottext[3].text = shopItemData[i].ShopPrice.ToString();
        
    }
    void SlotImage(int i)
    {
        Image[] slotImage = shopItem[i].GetComponentsInChildren<Image>();
        slotImage[5].sprite = shopItemData[i].GetSprite();
    }
    //타이머출력
    public void SetTimePrint(int i, bool timer,float timedelay)
    {
        slottext = shopItem[i].GetComponentsInChildren<TextMeshProUGUI>();
        if (timer == true)
        {
            float minute;
            float second;

            minute = (int)(timedelay / 60);
            second = timedelay - (60 * minute);
            slottext[2].text = string.Format("{0:00}", minute) + ":" + string.Format("{0:00}", second);
        }
        else
            slottext[2].text = "00:00";
    }

    public void OpenShopPopup()
    {
        shopPopup.SetActive(true);
        setwindow = true;
        shopPopup.GetComponent<MakePopScript>().ShopPopupOpen();
    }

    void SlotInDataSet(int i)
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Slot");
        UnityEngine.Debug.Log(go.Length.ToString());
        go[i].GetComponent<SlotIn>().SetSlotInData(itemMaxnum[i] - buyNum[i], shopItemData[i].Code);
    }

    //여기서 타이머로 신호 전달
    public void SetbuyNum(int cod, int value)
    {
        int i;
        for (i = 0; i < shopItemInfo.Length; i++)
        {
            if (shopItemInfo[i].Code == cod)
            {
                buyNum[i] += value;
                if (setwindow==true)
                {
                    SetshopslotData(i);
                    SlotInDataSet(i);
                }
                //UnityEngine.Debug.Log( cod.ToString()+": 코드 , "+(i).ToString() + " 번째 슬롯 재설정");
                return;
            }
        }
    }


}