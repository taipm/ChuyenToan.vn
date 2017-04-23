(function($) {
  "use strict"; 
/* ==============================================
BANNER -->
=============================================== */
  $(document).ready(function() {
    $('[data-toggle="tooltip"]').tooltip()
  });

/* ==============================================
BACK TOP -->
=============================================== */
    jQuery(window).scroll(function(){
    if (jQuery(this).scrollTop() > 1) {
    jQuery('.topbutton').css({bottom:"25px"});
    } else {
    jQuery('.topbutton').css({bottom:"-100px"});
    }
    });
    jQuery('.topbutton').click(function(){
    jQuery('html, body').animate({scrollTop: '0px'}, 800);
    return false;
    });
})(jQuery);

/************************************************
RETINA DISPLAY
************************************************/
!function(){function t(){}function e(t){return r.retinaImageSuffix+t}function i(t,i){if(this.path=t||"","undefined"!=typeof i&&null!==i)this.at_2x_path=i,this.perform_check=!1;else{if(void 0!==document.createElement){var n=document.createElement("a");n.href=this.path,n.pathname=n.pathname.replace(o,e),this.at_2x_path=n.href}else{var a=this.path.split("?");a[0]=a[0].replace(o,e),this.at_2x_path=a.join("?")}this.perform_check=!0}}function n(t){this.el=t,this.path=new i(this.el.getAttribute("src"),this.el.getAttribute("data-at2x"));var e=this;this.path.check_2x_variant(function(t){t&&e.swap()})}var a="undefined"==typeof exports?window:exports,r={retinaImageSuffix:"@2x",check_mime_type:!0,force_original_dimensions:!0};a.Retina=t,t.configure=function(t){null===t&&(t={});for(var e in t)t.hasOwnProperty(e)&&(r[e]=t[e])},t.init=function(t){null===t&&(t=a);var e=t.onload||function(){};t.onload=function(){var t,i,a=document.getElementsByTagName("img"),r=[];for(t=0;t<a.length;t+=1)i=a[t],i.getAttributeNode("data-no-retina")||r.push(new n(i));e()}},t.isRetina=function(){var t="(-webkit-min-device-pixel-ratio: 1.5), (min--moz-device-pixel-ratio: 1.5), (-o-min-device-pixel-ratio: 3/2), (min-resolution: 1.5dppx)";return a.devicePixelRatio>1?!0:a.matchMedia&&a.matchMedia(t).matches?!0:!1};var o=/\.\w+$/;a.RetinaImagePath=i,i.confirmed_paths=[],i.prototype.is_external=function(){return!(!this.path.match(/^https?\:/i)||this.path.match("//"+document.domain))},i.prototype.check_2x_variant=function(t){var e,n=this;return this.is_external()?t(!1):this.perform_check||"undefined"==typeof this.at_2x_path||null===this.at_2x_path?this.at_2x_path in i.confirmed_paths?t(!0):(e=new XMLHttpRequest,e.open("HEAD",this.at_2x_path),e.onreadystatechange=function(){if(4!==e.readyState)return t(!1);if(e.status>=200&&e.status<=399){if(r.check_mime_type){var a=e.getResponseHeader("Content-Type");if(null===a||!a.match(/^image/i))return t(!1)}return i.confirmed_paths.push(n.at_2x_path),t(!0)}return t(!1)},e.send(),void 0):t(!0)},a.RetinaImage=n,n.prototype.swap=function(t){function e(){i.el.complete?(r.force_original_dimensions&&(i.el.setAttribute("width",i.el.offsetWidth),i.el.setAttribute("height",i.el.offsetHeight)),i.el.setAttribute("src",t)):setTimeout(e,5)}"undefined"==typeof t&&(t=this.path.at_2x_path);var i=this;e()},t.isRetina()&&t.init(a)}();

/****************************************!
 * Parallax.js
****************************************/

function fullscreenFix(){var a=$("body").height();$(".content-b").each(function(i){$(this).innerHeight()<=a&&$(this).closest(".fullscreen").addClass("not-overflow")})}function backgroundResize(){var a=$(window).height();$(".paralbackground").each(function(i){var t=$(this),e=t.width(),s=t.height(),o=t.attr("data-img-width"),n=t.attr("data-img-height"),r=o/n,l=parseFloat(t.attr("data-diff"));l=l?l:0;var c=0;if(t.hasClass("parallax")&&!$("html").hasClass("touch")){c=a-s}n=s+c+l,o=n*r,e>o&&(o=e,n=o/r),t.data("resized-imgW",o),t.data("resized-imgH",n),t.css("background-size",o+"px "+n+"px")})}function parallaxPosition(a){var i=$(window).height(),t=$(window).scrollTop(),e=t+i,s=(t+e)/2;$(".parallax").each(function(a){var o=$(this),n=o.height(),r=o.offset().top,l=r+n;if(e>r&&l>t){var c=(o.data("resized-imgW"),o.data("resized-imgH")),d=0,h=-c+i,u=i>n?c-n:c-i;r-=u,l+=u;var g=d+(h-d)*(s-r)/(l-r),w=o.attr("data-oriz-pos");w=w?w:"50%",$(this).css("background-position",w+" "+g+"px")}})}"ontouchstart"in window&&(document.documentElement.className=document.documentElement.className+" touch"),$("html").hasClass("touch")||$(".parallax").css("background-attachment","fixed"),$(window).resize(fullscreenFix),fullscreenFix(),$(window).resize(backgroundResize),$(window).focus(backgroundResize),backgroundResize(),$("html").hasClass("touch")||($(window).resize(parallaxPosition),$(window).scroll(parallaxPosition),parallaxPosition());

/************************************************
TICKER
************************************************/

(function(e){e.fn.tickerme=function(t){var n=e.extend({},e.fn.tickerme.defaults,t);return this.each(function(){function a(){e(t).hide();e("body").prepend(r).prepend(i);var n='<div id="ticker_container">';n+='<div id="newscontent"><div id="news"></div></div>';n+='<div id="controls">';n+='<a href="#" id="pause_trigger"><svg class="icon icon-pause" viewBox="0 0 32 32"><use xlink:href="#icon-pause"></use></svg></a>';n+='<a href="#" id="play_trigger" style="display:none"><svg class="icon icon-play" viewBox="0 0 32 32"><use xlink:href="#icon-play"></use></svg></a>';n+='<a href="#" id="prev_trigger"><svg class="icon icon-prev" viewBox="0 0 32 32"><use xlink:href="#icon-prev"></use></svg></a>';n+='<a href="#" id="next_trigger"><svg class="icon icon-next" viewBox="0 0 32 32"><use xlink:href="#icon-next"></use></svg></a>';n+="</div>";n+="</div>";e(n).insertAfter(t);e(t).children().each(function(t){s[t]=e(this).html()});f()}function f(){if(o==s.length-1){o=0}else{o++}if(n.type=="fade"){e("#news").fadeOut(n.fade_speed,function(){e("#newscontent").html('<div id="news">'+s[o]+"</div>");e("#news").fadeIn(n.fade_speed)})}u=setTimeout(f,n.duration)}var t=e(this);var r='<svg display="none" version="1.1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="224" height="32" viewBox="0 0 224 32"><defs><g id="icon-play"><path class="path1" d="M6 4l20 12-20 12z"></path></g><g id="icon-pause"><path class="path1" d="M4 4h10v24h-10zM18 4h10v24h-10z"></path></g><g id="icon-prev"><path class="path1" d="M18 5v10l10-10v22l-10-10v10l-11-11z"></path></g><g id="icon-next"><path class="path1" d="M16 27v-10l-10 10v-22l10 10v-10l11 11z"></path></g></defs></svg>';var i='<style type="text/css">#ticker_container{width:100%}#newscontent{float:left}#news{display:none}#controls{float:right;height:16px}.icon{display:inline-block;width:16px;height:16px;fill:'+n.control_colour+"}.icon:hover{fill:"+n.control_rollover+"}</style>";var s=[];var o=-1;var u;a();e("a#pause_trigger").click(function(){clearTimeout(u);e(this).hide();e("#play_trigger").show();return false});e("a#play_trigger").click(function(){f();e(this).hide();e("#pause_trigger").show();return false});e("a#prev_trigger").click(function(){if(o==0){o=s.length-1}else{o--}e("#newscontent").html('<div id="news" style="display:block">'+s[o]+"</div>");if(n.auto_stop)e("a#pause_trigger").trigger("click");return false});e("a#next_trigger").click(function(){if(o==s.length-1){o=0}else{o++}e("#newscontent").html('<div id="news" style="display:block">'+s[o]+"</div>");if(n.auto_stop)e("a#pause_trigger").trigger("click");return false})})};e.fn.tickerme.defaults={fade_speed:500,duration:3e3,auto_stop:true,type:"fade",control_colour:"#333333",control_rollover:"#666666"}})(jQuery)

// TICKER
  $('#ticker').tickerme();