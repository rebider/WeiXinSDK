﻿using System.Collections.Generic;

namespace WeiXinSDK.Menu
{
    public class CustomMenu
    {
        public List<BaseButton> button = new List<BaseButton>();

        public void AddMulitButton(MultiButton multiBtn)
        {
            button.Add(multiBtn);
        }

        public void AddClickButton(ClickButton clickBtn)
        {
            button.Add(clickBtn);
        }

        public void AddScanCodePushButton(ScanCodePushButton scanBtn)
        {
            button.Add(scanBtn);
        }

        public void AddViewButton(ViewButton viewBtn)
        {
            button.Add(viewBtn);
        }


        public virtual string GetJSON()
        {
            return Util.ToJson(this);
        }
    }
}
