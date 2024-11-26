using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace QFramework.Example
{
	// Generate Id:689a0908-c36a-4f18-ad2e-a98e8f9c288b
	public partial class UILevelClear
	{
		public const string Name = "UILevelClear";
		
		[SerializeField]
		public UnityEngine.UI.Button BtnNext;
		
		private UILevelClearData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BtnNext = null;
			
			mData = null;
		}
		
		public UILevelClearData Data
		{
			get
			{
				return mData;
			}
		}
		
		UILevelClearData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UILevelClearData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
