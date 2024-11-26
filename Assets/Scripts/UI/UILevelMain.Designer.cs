using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:8f4c546f-4fa2-42df-8104-ce5181b8292b
	public partial class UILevelMain
	{
		public const string Name = "UILevelMain";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnRestart;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtLevel;
		[SerializeField]
		public UnityEngine.UI.Button BtnSkip;
		[SerializeField]
		public UnityEngine.UI.Image ScrollBar;
		[SerializeField]
		public RectTransform ObjectNode;
		[SerializeField]
		public UnityEngine.UI.Button BtnVitality;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtTime;
		[SerializeField]
		public TMPro.TextMeshProUGUI TxtVitality;
		[SerializeField]
		public Animator Clear;
		
		private UILevelMainData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnRestart = null;
			TxtLevel = null;
			BtnSkip = null;
			ScrollBar = null;
			ObjectNode = null;
			BtnVitality = null;
			TxtTime = null;
			TxtVitality = null;
			Clear = null;
			
			mData = null;
		}
		
		public UILevelMainData Data
		{
			get
			{
				return mData;
			}
		}
		
		UILevelMainData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UILevelMainData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
