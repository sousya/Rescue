using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UILevelAddHeartData : UIPanelData
	{
	}
	public partial class UILevelAddHeart : UIPanel, ICanGetUtility, ICanSendEvent
    {
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UILevelAddHeartData ?? new UILevelAddHeartData();
			// please add init code here
		}

        private void Start()
        {
            TxtVitality.text = this.GetUtility<SaveDataUtility>().GetVitalityNum() + "";

			BindButton();
        }

		public void BindButton()
		{
			TopOnADManager.Instance.rewardAction = AddVitality;

            BtnAD.onClick.AddListener(() =>
			{
                TopOnADManager.Instance.ShowRewardAd();
            });

			BtnClose.onClick.AddListener(() =>
			{
                UIKit.ClosePanel("UILevelAddHeart");
            });

			BtnNext.onClick.AddListener(() =>
			{
				ShopManager.Instance.BuyHeart();

            });
		}

		void AddVitality()
		{
            this.SendEvent<AddVitality>();
        }

        protected override void OnOpen(IUIData uiData = null)
		{
		}
		
		protected override void OnShow()
		{
		}
		
		protected override void OnHide()
		{
			TopOnADManager.Instance.rewardAction = null;
        }
		
		protected override void OnClose()
		{
            TopOnADManager.Instance.rewardAction = null;
        }

        public IArchitecture GetArchitecture()
        {
            return GameMainArc.Interface;
        }
    }
}
