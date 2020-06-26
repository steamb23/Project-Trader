﻿using UnityEngine;
using System.Collections;
using System;

public class Scrap : ClickableObject
{
    [Tooltip("회수중일 경우 true")]
    [SerializeField] bool isCollecting;

    /// <summary>
    /// 클릭이 잠겼을경우
    /// </summary>
    static bool isClickLock;

    public bool IsCollecting
    {
        get => this.isCollecting;
        set => this.isCollecting = value;
    }

    public override void Click()
    {
        if (!isCollecting && !isClickLock)
        {
            isClickLock = true;
            var scrapManager = FindObjectOfType<ScrapManager>();
            scrapManager.CollectTrash(gameObject, 5f, (timer) =>
            {
                isClickLock = false;
            });
        }
    }
}