using UnityEngine;
using UnityEngine.UI;
using QFramework;
using System;
using GameDefine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.IO;
using System.Collections;

namespace QFramework.Example
{
	public class UILevelMainData : UIPanelData
	{
	}
	public partial class UILevelMain : UIPanel, ICanRegisterEvent, ICanGetUtility, ICanSendEvent
    {
        [SerializeField]
        Image scrollBar;
        [SerializeField]
        Image showImg;
        [SerializeField]
        List<Sprite> showImgs = new List<Sprite>();
        [SerializeField]
        Transform showClick;
        int printNum = 0;
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UILevelMainData ?? new UILevelMainData();


        }

        private void Start()
        {

            TxtLevel.font.material.shader = Shader.Find(TxtLevel.font.material.shader.name);
            RegisterEvents();
            RegisterButton();
            RefreshUI();
            ShowNextItem();
        }

        void RegisterEvents()
        {
            this.RegisterEvent<LevelStartEvent>(e =>
            {
                RefreshUI();
                ShowNextItem();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
            
            this.RegisterEvent<LevelClearEvent>(e =>
            {
                Clear.gameObject.SetActive(true);
            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            this.RegisterEvent<LoadRawImage>(e =>
            {
                ShowNextItem();
            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        void RegisterButton()
        {
            BtnRestart.onClick.AddListener(() =>
            {
                LevelManager.Instance.Restart();
            });

            BtnSkip.onClick.AddListener(() =>
            {
                TopOnADManager.Instance.rewardAction = SkipLevel;
                TopOnADManager.Instance.ShowRewardAd();
            });

            BtnVitality.onClick.AddListener(() =>
            {
                UIKit.OpenPanel("UILevelAddHeart", UILevel.Common, "uileveladdheart_prefab");
            });

        }

        void SkipLevel()
        {
            this.SendEvent<SkipLevel>();
        }

        void RefreshUI()
        {
            Clear.gameObject.SetActive(false);
            TxtLevel.text = "Level " + LevelManager.Instance.levelNow;
            int test = LevelManager.Instance.levelNow % 5;
            switch (test)
            {
                case 1:
                    scrollBar.fillAmount = 0.2f;
                    break;
                case 2:
                    scrollBar.fillAmount = 0.39f;
                    break;
                case 3:
                    scrollBar.fillAmount = 0.58f;
                    break;
                case 4:
                    scrollBar.fillAmount = 0.8f;
                    break;
                case 0:
                    scrollBar.fillAmount = 1f;
                    break;
            }
        }

        void ShowNextItem()
        {
            if(LevelManager.Instance != null)
            {
                int checkNum = (LevelManager.Instance.levelNow - 1) / 5;
                if(checkNum < showImgs.Count)
                {
                    showImg.gameObject.SetActive(true);
                    showImg.sprite = showImgs[checkNum];
                }
                else
                {
                    showImg.gameObject.SetActive(false);
                }
            }

        }

        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }

        protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
		}
		
		protected override void OnClose()
		{
		}

        private void Update()
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPos = Input.mousePosition;
                var pos = QUICameraUtil.UICamera.ScreenToWorldPoint(Input.mousePosition);
                pos.z = showImg.transform.position.z;
                showClick.position = pos;
                showClick.gameObject.SetActive(false);
                showClick.gameObject.SetActive(true);
            }


            if (Input.GetKeyDown(KeyCode.K))
            {
                StartCoroutine(PrintScreen());
            }

            int lastVitalityNum = this.GetUtility<SaveDataUtility>().GetVitalityNum();
            TxtVitality.text = this.GetUtility<SaveDataUtility>().GetVitalityNum() + "";

            if (lastVitalityNum < 5)
            {
                long recoveryTime = this.GetUtility<SaveDataUtility>().GetVitalityTime() + (5 - lastVitalityNum) * GameConst.RecoveryTime;
                long timeOffset = recoveryTime - this.GetUtility<SaveDataUtility>().GetNowTime();
                //Debug.Log("体力 " + lastVitalityNum + " " + timeOffset);
                if(timeOffset > 0)
                {
                    long checkTime = this.GetUtility<SaveDataUtility>().GetNowTime() - this.GetUtility<SaveDataUtility>().GetVitalityTime();
                    int addNum = Mathf.FloorToInt((float)checkTime / GameConst.RecoveryTime);

                    //Debug.Log("体力 " + addNum + " " + timeOffset);

                    if (addNum > GameConst.MaxVitality)
                    {
                        addNum = GameConst.MaxVitality;
                        this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
                    }
                    else if(addNum >= 1)
                    {
                        this.GetUtility<SaveDataUtility>().SetVitality(lastVitalityNum + addNum, (this.GetUtility<SaveDataUtility>().GetVitalityTime() + (addNum) * GameConst.RecoveryTime) + "");
                    }
                }
                else
                {
                    this.GetUtility<SaveDataUtility>().SetVitality(GameConst.MaxVitality);
                }
                timeOffset = timeOffset % GameConst.RecoveryTime;
                if(timeOffset == 0)
                {
                    timeOffset = GameConst.RecoveryTime;
                }
                string minuteStr = (int)(timeOffset / 60) + "";
                string secondStr = timeOffset % 60 + "";
                if (minuteStr.Length == 1)
                {
                    minuteStr = "0" + minuteStr;
                }
                if (secondStr.Length == 1)
                {
                    secondStr = "0" + secondStr;
                }
                TxtTime.text = minuteStr + ":" + secondStr;

            }
            else
            {
                TxtTime.text = "";
            }

            if(Input.GetKeyDown(KeyCode.K))
            {
                this.GetUtility<SaveDataUtility>().UseVitality();
            }
            


        }

        IEnumerator PrintScreen()
        {
            yield return new WaitForEndOfFrame();
            //printNum++;
            //Texture2D ss = new Texture2D((int)Screen.width, (int)Screen.height, TextureFormat.RGB24, false);

            //ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
            //ss.Apply();

            //string filePath = Path.Combine(Application.temporaryCachePath, printNum + ".png");


            //File.WriteAllBytes(filePath, ss.EncodeToPNG());
            //Destroy(ss);
        }
    }
}
