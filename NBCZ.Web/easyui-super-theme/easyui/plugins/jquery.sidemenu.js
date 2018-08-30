/**
 * EasyUI for jQuery 1.6.2
 * 
 * Copyright (c) 2009-2018 www.jeasyui.com. All rights reserved.
 *
 * Licensed under the freeware license: http://www.jeasyui.com/license_freeware.php
 * To use it on other terms please contact us: info@jeasyui.com
 *
 */
(function($){
var _1=1;
function _2(_3){
$(_3).addClass("sidemenu");
};
function _4(_5,_6){
var _7=$(_5).sidemenu("options");
if(_6){
$.extend(_7,{width:_6.width,height:_6.height});
}
$(_5)._size(_7);
$(_5).find(".accordion").accordion("resize");
};
function _8(_9,_a,_b){
var _c=$(_9).sidemenu("options");
var tt=$("<ul class=\"sidemenu-tree\"></ul>").appendTo(_a);
tt.tree({data:_b,animate:_c.animate,onBeforeSelect:function(_d){
if(_d.children){
return false;
}
},onSelect:function(_e){
_12(_9,_e.id);
},onExpand:function(_f){
_23(_9,_f);
},onCollapse:function(_10){
_23(_9,_10);
},onClick:function(_11){
if(_11.children){
if(_11.state=="open"){
$(_11.target).addClass("tree-node-nonleaf-collapsed");
}else{
$(_11.target).removeClass("tree-node-nonleaf-collapsed");
}
$(this).tree("toggle",_11.target);
}
}});
tt.unbind(".sidemenu").bind("mouseleave.sidemenu",function(){
$(_a).trigger("mouseleave");
});
_12(_9,_c.selectedItemId);
};
function _13(_14,_15,_16){
var _17=$(_14).sidemenu("options");
$(_15).tooltip({content:$("<div></div>"),position:_17.floatMenuPosition,valign:"top",data:_16,onUpdate:function(_18){
var _19=$(this).tooltip("options");
var _1a=_19.data;
_18.accordion({width:_17.floatMenuWidth,multiple:false}).accordion("add",{title:_1a.text,collapsed:false,collapsible:false});
_8(_14,_18.accordion("panels")[0],_1a.children);
},onShow:function(){
var t=$(this);
var tip=t.tooltip("tip").addClass("sidemenu-tooltip");
tip.children(".tooltip-content").addClass("sidemenu");
tip.find(".accordion").accordion("resize");
tip.add(tip.find("ul.tree")).unbind(".sidemenu").bind("mouseover.sidemenu",function(){
t.tooltip("show");
}).bind("mouseleave.sidemenu",function(){
t.tooltip("hide");
});
t.tooltip("reposition");
},onPosition:function(_1b,top){
var tip=$(this).tooltip("tip");
if(!_17.collapsed){
tip.css({left:-999999});
}else{
if(top+tip.outerHeight()>$(window)._outerHeight()+$(document).scrollTop()){
top=$(window)._outerHeight()+$(document).scrollTop()-tip.outerHeight();
tip.css("top",top);
}
}
}});
};
function _1c(_1d,_1e){
$(_1d).find(".sidemenu-tree").each(function(){
_1e($(this));
});
$(_1d).find(".tooltip-f").each(function(){
var tip=$(this).tooltip("tip");
if(tip){
tip.find(".sidemenu-tree").each(function(){
_1e($(this));
});
$(this).tooltip("reposition");
}
});
};
function _12(_1f,_20){
var _21=$(_1f).sidemenu("options");
_1c(_1f,function(t){
t.find("div.tree-node-selected").removeClass("tree-node-selected");
var _22=t.tree("find",_20);
if(_22){
$(_22.target).addClass("tree-node-selected");
_21.selectedItemId=_22.id;
t.trigger("mouseleave.sidemenu");
_21.onSelect.call(_1f,_22);
}
});
};
function _23(_24,_25){
_1c(_24,function(t){
var _26=t.tree("find",_25.id);
if(_26){
t.tree(_25.state=="open"?"expand":"collapse",_26.target);
}
});
};
function _27(_28){
var _29=$(_28).sidemenu("options");
$(_28).empty();
if(_29.data){
$.easyui.forEach(_29.data,true,function(_2a){
if(!_2a.id){
_2a.id="_easyui_sidemenu_"+(_1++);
}
if(!_2a.iconCls){
_2a.iconCls="sidemenu-default-icon";
}
if(_2a.children){
_2a.nodeCls="tree-node-nonleaf";
if(!_2a.state){
_2a.state="closed";
}
if(_2a.state=="open"){
_2a.nodeCls="tree-node-nonleaf";
}else{
_2a.nodeCls="tree-node-nonleaf tree-node-nonleaf-collapsed";
}
}
});
var acc=$("<div></div>").appendTo(_28);
acc.accordion({fit:_29.height=="auto"?false:true,border:_29.border,multiple:_29.multiple});
var _2b=_29.data;
for(var i=0;i<_2b.length;i++){
acc.accordion("add",{title:_2b[i].text,selected:_2b[i].state=="open",iconCls:_2b[i].iconCls,onBeforeExpand:function(){
return !_29.collapsed;
}});
var ap=acc.accordion("panels")[i];
_8(_28,ap,_2b[i].children);
_13(_28,ap.panel("header"),_2b[i]);
}
}
};
function _2c(_2d,_2e){
var _2f=$(_2d).sidemenu("options");
_2f.collapsed=_2e;
var acc=$(_2d).find(".accordion");
var _30=acc.accordion("panels");
acc.accordion("options").animate=false;
if(_2f.collapsed){
$(_2d).addClass("sidemenu-collapsed");
for(var i=0;i<_30.length;i++){
var _31=_30[i];
if(_31.panel("options").collapsed){
_2f.data[i].state="closed";
}else{
_2f.data[i].state="open";
acc.accordion("unselect",i);
}
var _32=_31.panel("header");
_32.find(".panel-title").html("");
_32.find(".panel-tool").hide();
}
}else{
$(_2d).removeClass("sidemenu-collapsed");
for(var i=0;i<_30.length;i++){
var _31=_30[i];
if(_2f.data[i].state=="open"){
acc.accordion("select",i);
}
var _32=_31.panel("header");
_32.find(".panel-title").html(_31.panel("options").title);
_32.find(".panel-tool").show();
}
}
acc.accordion("options").animate=_2f.animate;
};
function _33(_34){
$(_34).find(".tooltip-f").each(function(){
$(this).tooltip("destroy");
});
$(_34).remove();
};
$.fn.sidemenu=function(_35,_36){
if(typeof _35=="string"){
var _37=$.fn.sidemenu.methods[_35];
return _37(this,_36);
}
_35=_35||{};
return this.each(function(){
var _38=$.data(this,"sidemenu");
if(_38){
$.extend(_38.options,_35);
}else{
_38=$.data(this,"sidemenu",{options:$.extend({},$.fn.sidemenu.defaults,$.fn.sidemenu.parseOptions(this),_35)});
_2(this);
}
_4(this);
_27(this);
_2c(this,_38.options.collapsed);
});
};
$.fn.sidemenu.methods={options:function(jq){
return jq.data("sidemenu").options;
},resize:function(jq,_39){
return jq.each(function(){
_4(this,_39);
});
},collapse:function(jq){
return jq.each(function(){
_2c(this,true);
});
},expand:function(jq){
return jq.each(function(){
_2c(this,false);
});
},destroy:function(jq){
return jq.each(function(){
_33(this);
});
}};
$.fn.sidemenu.parseOptions=function(_3a){
var t=$(_3a);
return $.extend({},$.parser.parseOptions(_3a,["width","height"]));
};
$.fn.sidemenu.defaults={width:200,height:"auto",border:true,animate:true,multiple:true,collapsed:false,data:null,floatMenuWidth:200,floatMenuPosition:"right",onSelect:function(_3b){
}};
})(jQuery);

