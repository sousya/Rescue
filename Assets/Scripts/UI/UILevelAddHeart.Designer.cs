using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:fc87ca76-5489-4fa3-a1f8-93f7997dc9da
	public partial class UILevelAddHeart
	{
		public const string Name = "UILevelAddHeart";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnNext;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtCost;
		[SerializeField]
		public UnityEngine.UI.Button BtnClose;
		[SerializeField]
		public UnityEngine.UI.Button BtnAD;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtVitality;
		
		private UILevelAddHeartData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnNext = null;
			TxtCost = null;
			BtnClose = null;
			BtnAD = null;
			TxtVitality = null;
			
			mData = null;
		}
		
		public UILevelAddHeartData Data
		{
			get
			{
				return mData;
			}
		}
		
		UILevelAddHeartData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UILevelAddHeartData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
