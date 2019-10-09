function jsApiCall()
{
    WeixinJSBridge.invoke(
    'getBrandWCPayRequest',
    '<%=wxJsApiParam%>',//josn串
    function (res)
    {
        if (res.err_msg == "get_brand_wcpay_request:ok") {
            //微信支付成功
            window.location.replace("../userdd.aspx");
        }else if (res.err_msg == "get_brand_wcpay_request:cancel") {  
            alert("已取消微信支付!");
        }
        else{
            alert("支付失败!错误原因："+res.err_code + res.err_desc + res.err_msg);
            window.location.replace("../userdd.aspx");
        }
        WeixinJSBridge.log(res.err_msg);
            //alert(res.err_code + res.err_desc + res.err_msg);
        }
    );
}

function callpay()
{
    if (typeof WeixinJSBridge == "undefined")
    {
        if (document.addEventListener)
        {
            document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
        }
        else if (document.attachEvent)
        {
            document.attachEvent('WeixinJSBridgeReady', jsApiCall);
            document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
        }
    }
    else
    {
        jsApiCall();
    }
}