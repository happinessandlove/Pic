/**************************************************
 * v3.3
 * by 丁浩
 * 2015-07-25
 * 
 * z-index:
 *  spinner:1
 *  dropdown:49,50
 *  tab:1,2,3,4
 *  float-cart:400
 *  cart-package:401
 *  masker:500
 *  floatLayer:1000
 *
 *  header-cart-count:1
 * **************************************************/

try { document.execCommand('BackgroundImageCache', false, true); } catch (e) { }
var userAgent = window.navigator.userAgent.toLocaleLowerCase();
var regBrowser = userAgent.match(/(msie) ([\d.]+)/) || userAgent.match(/(chrome)\/([\d.]+)/) || userAgent.match(/(firefox)\/([\d.]+)/) || userAgent.match(/(safari)\/([\d.]+)/) || userAgent.match(/(opera)\/([\d.]+)/) || userAgent.match(/(trident)\/([\d.]+)/);
var browser = { ie: regBrowser[1] == "msie" || regBrowser[1] == "trident" ? true : false, chrome: regBrowser[1] == "chrome" ? true : false, firefox: regBrowser[1] == "firefox" ? true : false, safari: regBrowser[1] == "safari" ? true : false, opera: regBrowser[1] == "opera" ? true : false, version: regBrowser[2] };
var IE6 = browser.ie && browser.version <= 6;
var IE7 = browser.ie && browser.version <= 7;
var ReturnType = { Invalid: 'Invalid', NotLogin: 'NotLogin', NoPermission: 'NoPermission', Success: 'Success', Failure: 'Failure', Error: 'Error', CreateSuccess: "CreateSuccess", CreateFailure: "CreateFailure", EditSuccess: "EditSuccess", EditFailure: "EditFailure", DeleteSuccess: "DeleteSuccess", DeleteFailure: "DeleteFailure", DeletesSuccess: "DeletesSuccess", DeletesFailure: "DeletesFailure", RankUpSuccess: "RankUpSuccess", RankUpFailure: "RankUpFailure", Other: 'Other' };

function getIe6DocumentSize()
{
    var doc = document.documentElement,
        width = doc.clientWidth > doc.scrollWidth ? doc.clientWidth : doc.scrollWidth,
        height = doc.clientHeight > doc.scrollHeight ? doc.clientHeight : doc.scrollHeight;
    return { width: width, height: height };
}

function getRandom()
{
    return (Math.random() + '').replace('.', '');
}

function setCookie(name, value)
{
    var argv = setCookie.arguments;
    var argc = setCookie.arguments.length;
    var expires = (argc > 2) ? argv[2] : null;
    var path = (argc > 3) ? argv[3] : null;
    var domain = (argc > 4) ? argv[4] : null;
    var secure = (argc > 5) ? argv[5] : false;
    if (expires != null)
    {
        var largeExpDate = new Date();
        largeExpDate.setTime(largeExpDate.getTime() + (expires * 1000 * 3600 * 24));
    }
    document.cookie
        = name + "=" + escape(value)
        + ((expires == null) ? "" : ("; expires=" + largeExpDate.toGMTString()))
        + ((path == null) ? "" : ("; path=" + path))
        + ((domain == null) ? "" : ("; domain=" + domain))
        + ((secure == true) ? "; secure" : "");
}
function getCookie(name)
{
    var search = name + "=";
    if (document.cookie.length > 0)
    {
        offset = document.cookie.indexOf(search);
        if (offset != -1)
        {
            offset += search.length;
            end = document.cookie.indexOf(";", offset);
            if (end == -1) end = document.cookie.length;
            return unescape(document.cookie.substring(offset, end));
        }
        else return null;
    }
    else return null;
}
function deleteCookie(name)
{
    var expdate = new Date();
    expdate.setTime(expdate.getTime() - 10000);
    setCookie(name, "", expdate);
}

$(function ()
{
    $('html').addClass(browser.ie && browser.version <= 6 ? 'no-fixed' : 'fixed');
    $.ajaxSetup({ timeout: 8000 })
});