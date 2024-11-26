using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	public class UILevelClearData : UIPanelData
	{
	}
	public partial class UILevelClear : UIPanel
	{
		[SerializeField]
		GameObject Nolevel;
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UILevelClearData ?? new UILevelClearData();
			// please add init code here
		}

        private void Start()
        {
			BtnNext.onClick.AddListener(() =>
			{
				LevelManager.Instance.CheckAD();
                UIKit.ClosePanel("UILevelClear");

            });

			Nolevel.SetActive(LevelManager.Instance.levelNow > 135);

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
	}
}
