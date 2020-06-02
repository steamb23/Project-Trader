﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ProjectTrader;
using ProjectTrader.Datas;
using ProjectTrader.SpriteDatas;
using System.Linq;

//새로 사용할 공방 스크립트

public class MakerUI : MonoBehaviour
{

    public GameObject slot;
    public GameObject materialSlot;
    public GameObject makerpopupwindow;
    public GameObject makewindow;

    Item[] slotItem;           //슬롯 아이템
    ItemData[] recipeData;     //슬롯 데이터 받아올 아이템데이터

    GameObject con;

    Image[] itemImage;

    ItemData disRecipeData;
    Item materialSample;

    //제작할 아이템
    ItemData[] makeItemData;
    Item[] makeItem;


    TextMeshProUGUI[] slotText;     //슬롯에 있는 텍스트
    GameObject[] recipe;            //레시피 > 데이터로 받아오기
    GameObject[] material;          //재료   > 데이터로 받아오기

    int materialNum;                //재료 갯수 > 슬롯에서 데이터로 받아오기
    int[] maNeeds;

    bool[] employeeInfo; //임시로 알바생이 있다는 표시
    bool[] working;     //슬롯이 일하고 있다면
    int clickEmployee=1;//알바선택창

    public Sprite b_on;
    public Sprite b_off;
    RectTransform[] rt;

    bool canMake=false;

    GameObject tim;
    void Start()
    {
        con = GameObject.Find("RecipeContent");
        makeItemData = new ItemData[3];
        makeItem = new Item[3];
        employeeInfo = new bool[3];
        working = new bool[3];
        tim = GameObject.Find("makeroom");
        //임시 초기화
        for (int i = 0; i < 3; i++)
        {
            working[i] = false;  //일하고 있지 않고
            employeeInfo[i] = true; //알바생이 있다
        }
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetRecipeScroll();
        }
    }


    //재료창 생성
    void SetMaterial()
    {
        if (material != null)
        {
            for (int i = 0; i < material.Length; i++)
                Destroy(material[i]);
        }

        GameObject m_standard = GameObject.Find("MaterialmakeSlot");
        RectTransform standard = m_standard.GetComponent<RectTransform>();
        Image[] standardImage = m_standard.GetComponentsInChildren<Image>();
        standardImage[1].sprite = disRecipeData.GetSprite();
        switch (materialNum)
        {
            case 2:
                material = new GameObject[2];
                for (int i = 0; i < 2; i++)
                {
                    material[i] = Instantiate(materialSlot) as GameObject;
                    material[i].transform.SetParent((GameObject.Find("MakerRoom")).transform);
                    material[i].transform.localScale = Vector3.one;
                    RectTransform tbPos = material[i].GetComponent<RectTransform>();
                    tbPos.anchoredPosition = new Vector3(standard.anchoredPosition.x - (210 + 120 * i), standard.anchoredPosition.y);
                }
                break;
            case 3:
                material = new GameObject[3];
                for (int i = 0; i < 3; i++)
                {
                    material[i] = Instantiate(materialSlot) as GameObject;
                    material[i].transform.SetParent((GameObject.Find("MakerRoom")).transform);
                    material[i].transform.localScale = Vector3.one;
                    RectTransform tbPos = material[i].GetComponent<RectTransform>();
                    if (i != 2)
                        tbPos.anchoredPosition = new Vector3(standard.anchoredPosition.x - (210 + 120 * i), standard.anchoredPosition.y - 60);
                    else
                        tbPos.anchoredPosition = new Vector3(standard.anchoredPosition.x - 270, standard.anchoredPosition.y + 50);
                }
                break;
            case 4:
                material = new GameObject[4];
                for (int i = 0; i < 4; i++)
                {
                    material[i] = Instantiate(materialSlot) as GameObject;
                    material[i].transform.SetParent((GameObject.Find("MakerRoom")).transform);
                    material[i].transform.localScale = Vector3.one;
                    RectTransform tbPos = material[i].GetComponent<RectTransform>();
                    if (i < 2)
                        tbPos.anchoredPosition = new Vector3(standard.anchoredPosition.x - (210 + 120 * i), standard.anchoredPosition.y - 60);
                    else
                        tbPos.anchoredPosition = new Vector3(standard.anchoredPosition.x - (210 + 120 * (i - 2)), standard.anchoredPosition.y + 50);
                }
                break;
            default:
                break;
        }
        //여기에 제작창에 뜨는 것들 바꾸는 코드까지 추가+시간변화같은거
    }
    
    //스크롤에 슬롯 생성
    void SetRecipeScroll()
    {
        PushOneButton();//첫번째 눌러놓기
        recipe = new GameObject[6];
        slotItem = new Item[5];
        recipeData = new ItemData[5];

        for (int i = 0; i < 4; i++)
        {
            recipe[i] = Instantiate(slot) as GameObject;
            SetRecipeSlot(i);

            //재료별로 갯수받아와서 가능한 갯수출력후
            //setslotindata로 슬롯에 데이터 부여

            recipe[i].transform.SetParent((GameObject.Find("RecipeContent")).transform);
            recipe[i].transform.localScale = Vector3.one;
            SlotImageSet(i);
        }
        INSlotScriptSet();
    }

    //슬롯 이미지 바꾸기
    void SlotImageSet(int i)
    {

        itemImage = recipe[i].GetComponentsInChildren<Image>();//5
        itemImage[4].sprite = ItemSpriteData.GetItemSprite(slotItem[i].Code);
    }


    //순서대로 이름,소모 피로도,최대 제작 가능 개수
    //데이터 받아오기
    void SetRecipeSlot(int i)
    {
        slotItem[i].Code = i + 1;
        recipeData[i] = slotItem[i].GetData();
        slotText = recipe[i].GetComponentsInChildren<TextMeshProUGUI>();
        slotText[0].text = recipeData[i].Name;
        slotText[1].text = recipeData[i].Tier.ToString();
        slotText[2].text = 5.ToString();//제작가능한 숫자->재료들의 양을 계산해서 최솟값찾기, 이걸 반환해서 별다른 계산없이 팝업에서 사용하도록
    }



    //임시코드
    //타이머 추가
    //데이터 관리 추가
    //슬롯 데이터 부여 추가


    //슬롯 세팅할때
    void INSlotScriptSet()
    {
        GameObject[] slotsetting = GameObject.FindGameObjectsWithTag("Slot");
        if (slotsetting == null)
            UnityEngine.Debug.Log("슬롯없음");
        UnityEngine.Debug.Log("슬롯 갯수  :  "+slotsetting.Length.ToString());
        for (int i = 0; i < slotsetting.Length; i++)
        {
            int k = UnityEngine.Random.Range(5, 10);
            slotsetting[i].GetComponent<SlotIn>().SetSlotInData(k,slotItem[i].Code);
            if (i == 0)
                slotsetting[i].GetComponent<SlotIn>().MakerslotPushButton();//어느정도 수정을 해봅시다
        }
    }

    //슬롯에서 받아와 제작창 세팅
    public void SetMakerBg(int cunt, int cod)
    {
        int i;
        
        if (materialNum == null)
            return;
        maNeeds = new int[4];
        materialSample.Code = cod;
        disRecipeData= materialSample.GetData();
        maNeeds = disRecipeData.MaterialNeeds;

        for(i = 0; i < maNeeds.Length; i++)
        {
            UnityEngine.Debug.Log(maNeeds[i].ToString());
            if (maNeeds[i] == 0)
                break;
        }
        materialNum = i;
        //materialNum=disRecipeData.
        SetMaterial();
    }

    //직원이 일할 슬롯 선택(버튼으로 설정)
    void SetWorkingSlot()
    {

    }

    //재료창을 수정해서 출력
    void MaterialSlotSetting()
    {

    }
    
    //만들 아이템코드와 갯수 설정 하고 팝업으로
    void MakeItemInfo(int cod, int count)
    {
        makeItem[clickEmployee].Code = cod;
        makeItem[clickEmployee].Count = count;
        makeItemData[clickEmployee] = makeItem[clickEmployee].GetData();
        if(working[clickEmployee]==false && employeeInfo[clickEmployee] == true)
        {
            canMake = true;
        }
    }

    //팝업만들기
    public void CreateMakePopup()
    {
        MakeItemInfo(materialSample.Code, 5);
        if (canMake == true)
        {
            makerpopupwindow.SetActive(true);
            makerpopupwindow.GetComponent<MakePopScript>().OpenMakePopup();
            makerpopupwindow.GetComponent<MakePopScript>().SetMakerPopupData(5, materialSample.Code, clickEmployee);//갯수코드슬롯
        }
    }


    //임시 알바제작창 버튼

    public void PushOneButton()
    {
        clickEmployee = 1;
        ChangeButtonInfo();
        tim.GetComponent<MakerTimer>().NumSet(0);
    }

    public void PushTwoButton()
    {
        clickEmployee = 2;
        ChangeButtonInfo();
        tim.GetComponent<MakerTimer>().NumSet(1);
    }

    public void PushThreeButton()
    {
        clickEmployee = 3;
        ChangeButtonInfo();
        tim.GetComponent<MakerTimer>().NumSet(2);
    }

    //버튼 위치 스프라이트 바꿔주기..
    void ChangeButtonInfo()
    {
        GameObject[] bu = GameObject.FindGameObjectsWithTag("MPEB");
        Image[] img=new Image[3];
        GameObject go = GameObject.Find("alba_bg");
        RectTransform bg = go.GetComponent<RectTransform>();
        rt = new RectTransform[3];
        for (int j = 0; j < 3; j++)
        {
            img[j] = bu[j].GetComponentInChildren<Image>();
            rt[j] = bu[j].GetComponent<RectTransform>();
        }
        for (int i = 0; i < bu.Length; i++)
        {
            if (clickEmployee == i+1)
            {
                img[i].sprite = b_on;
                rt[i].anchoredPosition = new Vector3(bg.anchoredPosition.x-180, rt[i].anchoredPosition.y);
            }
            else
            {
                img[i].sprite = b_off;
                rt[i].anchoredPosition = new Vector3(bg.anchoredPosition.x-160,rt[i].anchoredPosition.y);
            }
        }
    }


    public void OpenMakeRoom()
    {
        makewindow.SetActive(true);
        SetRecipeScroll();
    }

    public void CloseMakeRoom()
    {
        for (int i = 0; i < recipe.Length; i++)
            Destroy(recipe[i]);
        makewindow.SetActive(false);
    }
}