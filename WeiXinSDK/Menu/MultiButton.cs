using System.Collections.Generic;

namespace WeiXinSDK.Menu
{
    public class MultiButton:BaseButton
    {
        public List<SingleButton> sub_button = new List<SingleButton>();

        public void AddClickButton(ClickButton clickBtn)
        {
            sub_button.Add(clickBtn);
        }
        public void AddScanCodePushButton(ScanCodePushButton scanBtn)
        {
            sub_button.Add(scanBtn);
        }
        public void AddViewButton(ViewButton viewBtn)
        {
            sub_button.Add(viewBtn);
        }

    }
}
