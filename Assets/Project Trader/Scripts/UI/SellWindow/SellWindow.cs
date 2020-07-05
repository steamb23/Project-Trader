﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ProjectTrader;
using ProjectTrader.Datas;
using ProjectTrader.SpriteDatas;
using System.Diagnostics;

public class SellWindow : MonoBehaviour
{
    //배치할때 값주고받을 아이템
    Item arrowC;
    public Item[] slotItem;

    ItemData[] slotItemData;
    //배치할 아이템
    Item display;

    public GameObject itemSlot;
    public GameObject arrow;
    public TextMeshProUGUI setPrice;
    public Image setImage;
    public GameObject popupWindow;

    GameObject tableData;
    GameObject setTable;
    GameObject savedata;

    GameObject[] itemnum;
    TextMeshProUGUI[] slotText;
    Image[] ItemImage;
    GameObject[] arrowSprite; //화살표
    public GameObject makerWindow;

    //임시로 선언하는 화살표용 bool
    bool setArrow = false;
    //임시로 선언하는 슬롯세팅 bool
    bool setslot = false;

    void Start()
    {
        setTable = GameObject.Find("selltimewindow");
        savedata = GameObject.Find("SaveData");
    }


    void Update()
    {
        SetsettingButton();

    }    

    public void OpenMakerWindow()
    {
        makerWindow.SetActive(true);
        SetItemslot();
    }

    public void CloseMakerWindow()
    {
        //슬롯 리로드하는 코드도 추가로 작성
        makerWindow.SetActive(false);
    }

    //슬롯 세팅(임시로 이미 생성되어있으면 추가로 생성하지 않도록 함) 아이템개수를 받아오면좋을거같은디,,
    //code양만큼 생성하고 count가 0이면 표시하지 않도록 함->슬롯밖에서 설정
    void SetItemslot()
    {
        savedata.GetComponent<DataSave>().SetItemList();
        if (setslot == false)
        {
            itemnum = new GameObject[5];
            //slotItem = new Item[5];
            slotItemData = new ItemData[5];

            //아이템 가진 수만큼
            for (int i = 0; i < slotItem.Length; i++)
            {
                itemnum[i] = Instantiate(itemSlot) as GameObject;

                itemnum[i].transform.SetParent((GameObject.Find("ItemContent")).transform);
                itemnum[i].transform.localScale = Vector3.one;
                SetItemInfo(i);


            }
            setslot = true;
            itemnum[0].GetComponent<SlotIn>().PushButton();
        }
    }

    //슬롯생성할때 멤버 바꾸기> 가지고있는 아이템 표시(재료 제외>일정 코드 이상부터 count가 1이상만 표시)
    void SetItemInfo(int i)
    {
        if (itemnum[i].activeSelf == false)
            itemnum[i].SetActive(true);
        slotText = itemnum[i].GetComponentsInChildren<TextMeshProUGUI>();

        slotItemData[i] = slotItem[i].GetData();
        slotText[0].text = slotItemData[i].SellPrice.ToString();

        slotText[1].text = "x" + slotItem[i].Count.ToString();
        SpriteChange(i);
        SlotScriptSet(i);
        if (slotItem[i].Count <= 0)
        {
            itemnum[i].SetActive(false);
        }

    }

    //슬롯내 스크립트 수정
    void SlotScriptSet(int i)
    {
        itemnum[i].GetComponent<SlotIn>().SetSlotInData(slotItem[i].Count, slotItem[i].Code);
    }

    void SpriteChange(int i)
    {
        ItemImage = itemnum[i].GetComponentsInChildren<Image>();
        ItemImage[4].sprite = ItemSpriteData.GetItemSprite(slotItem[i].Code);

    }

    void SetsettingButton()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("Item");

        if (setArrow == false)
        {
            arrowSprite = new GameObject[go.Length];
            for (int j = 0; j < go.Length; j++)
            {
                arrowSprite[j] = Instantiate(arrow) as GameObject;
                arrowSprite[j].transform.position = new Vector3(go[j].transform.position.x, go[j].transform.position.y + 0.6f, -1);
                arrowSprite[j ].SetActive(false);
            }
            setArrow = true;
        }

        for (int i = 0; i < go.Length; i++)
        {
            arrowC = go[i].GetComponent<DisplayedItem>().Item;
            if (arrowC.Count <= 0)
            {
                arrowSprite[i ].SetActive(true);
            }
            else
            {
                arrowSprite[i ].SetActive(false);
            }

        }

    }

    //디스플레이할 아이템 보여주기

    public void SetCheckdisplay(int num, int code)
    {
        display.Code = code;
        display.Count = num;
        ItemData displayData = display.GetData();
        setPrice.text = displayData.SellPrice.ToString();
        setImage.sprite= ItemSpriteData.GetItemSprite(display.Code);
        //텍스트랑 image찾아서 변경
    }


    public void SetPopUpWindow()
    {
        tableData = setTable.GetComponent<TableCheck>().choiceTable;
        popupWindow.SetActive(true);
        popupWindow.GetComponent<MakePopScript>().Openpopup();
        //아이템을 하나 보내주는 걸로
        popupWindow.GetComponent<MakePopScript>().SetPopupItem(display.Count, display.Code,tableData);
    }

    //아이템코드에 따라 조금 수정-아이템 초기화 코드
    public void SetItem(Item[] pItem)
    {
        slotItem = new Item[pItem.Length];
        for(int i = 0; i < slotItem.Length; i++)
        {
            slotItem[i] = pItem[i];
        }
    }

    //아이템 제거,회수용
    public void DisItemCheck(int cod, int value)
    {
        savedata.GetComponent<DataSave>().UseItem(cod, value);
        savedata.GetComponent<DataSave>().SetItemList();
        SetItemInfo(cod - 1);
    }
}

