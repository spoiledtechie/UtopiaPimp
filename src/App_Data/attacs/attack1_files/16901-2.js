rubicon_cb = Math.random();
rubicon_rurl = document.referrer;
if(top.location==document.location){rubicon_rurl = document.location;}
rubicon_rurl = escape(rubicon_rurl);
window.rubicon_zone = "http://optimized-by.rubiconproject.com/a/6659/10283/16901-2" + "." + "js";
window.rubicon_ad = "682901" + "." + "js";
if (window.rubicon_olds && (window.rubicon_dk_zone != window.rubicon_zone))
window.rubicon_olds = null;
url = "<!-- BEGIN TAG - 728x90 - www.swirve.com/ - DO NOT MODIFY -->\n<script type=\"text/javascript\" src=\"http://optimizedby.rmxads.com/st?ad_type=ad&ad_size=728x90&section=355975\"><\/script>\n<!-- END TAG -->\n<img src=\"http://pixel.quantserve.com/pixel/p-e4m3Yko6bFYVc.gif?labels=Games,Entertainment\" style=\"display: none;\" border=\"0\" height=\"1\" width=\"1\" alt=\"Quantcast\"/>";
url = url.replace(/##RUBICON_CB##/g,rubicon_cb); 
document.write(url);
