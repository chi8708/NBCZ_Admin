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
var _4=$("<span class=\"radiobutton inputbox\">"+"<span class=\"radiobutton-inner\" style=\"display:none\"></span>"+"<input type=\"radio\" class=\"radiobutton-value\">"+"</span>").insertAfter(_3);
var t=$(_3);
t.addClass("radiobutton-f").hide();
var _5=t.attr("name");
if(_5){
t.removeAttr("name").attr("radiobuttonName",_5);
_4.find(".radiobutton-value").attr("name",_5);
}
return _4;
};
function _6(_7){
var _8=$.data(_7,"radiobutton");
var _9=_8.options;
var _a=_8.radiobutton;
var _b="_easyui_radiobutton_"+(++_1);
_a.find(".radiobutton-value").attr("id",_b);
if(_9.label){
if(typeof _9.label=="object"){
_8.label=$(_9.label);
_8.label.attr("for",_b);
}else{
$(_8.label).remove();
_8.label=$("<label class=\"textbox-label\"></label>").html(_9.label);
_8.label.css("textAlign",_9.labelAlign).attr("for",_b);
if(_9.labelPosition=="after"){
_8.label.insertAfter(_a);
}else{
_8.label.insertBefore(_7);
}
_8.label.removeClass("textbox-label-left textbox-label-right textbox-label-top");
_8.label.addClass("textbox-label-"+_9.labelPosition);
}
}else{
$(_8.label).remove();
}
$(_7).radiobutton("setValue",_9.value);
_c(_7,_9.checked);
_d(_7,_9.disabled);
};
function _e(_f){
var _10=$.data(_f,"radiobutton");
var _11=_10.options;
var _12=_10.radiobutton;
_12.unbind(".radiobutton").bind("click.radiobutton",function(){
if(!_11.disabled){
_c(_f,true);
}
});
};
function _13(_14){
var _15=$.data(_14,"radiobutton");
var _16=_15.options;
var _17=_15.radiobutton;
_17._size(_16,_17.parent());
if(_16.label&&_16.labelPosition){
if(_16.labelPosition=="top"){
_15.label._size({width:_16.labelWidth},_17);
}else{
_15.label._size({width:_16.labelWidth,height:_17.outerHeight()},_17);
_15.label.css("lineHeight",_17.outerHeight()+"px");
}
}
};
function _c(_18,_19){
if(_19){
var f=$(_18).closest("form");
var _1a=$(_18).attr("radiobuttonName");
f.find(".radiobutton-f[radiobuttonName=\""+_1a+"\"]").each(function(){
if(this!=_18){
_1b(this,false);
}
});
_1b(_18,true);
}else{
_1b(_18,false);
}
function _1b(b,c){
var _1c=$(b).radiobutton("options");
var _1d=$(b).data("radiobutton").radiobutton;
_1d.find(".radiobutton-inner").css("display",c?"":"none");
_1d.find(".radiobutton-value")._propAttr("checked",c);
if(_1c.checked!=c){
_1c.checked=c;
_1c.onChange.call($(b)[0],c);
}
};
};
function _d(_1e,_1f){
var _20=$.data(_1e,"radiobutton");
var _21=_20.options;
var _22=_20.radiobutton;
var rv=_22.find(".radiobutton-value");
_21.disabled=_1f;
if(_1f){
$(_1e).add(rv)._propAttr("disabled",true);
_22.addClass("radiobutton-disabled");
}else{
$(_1e).add(rv)._propAttr("disabled",false);
_22.removeClass("radiobutton-disabled");
}
};
$.fn.radiobutton=function(_23,_24){
if(typeof _23=="string"){
return $.fn.radiobutton.methods[_23](this,_24);
}
_23=_23||{};
return this.each(function(){
var _25=$.data(this,"radiobutton");
if(_25){
$.extend(_25.options,_23);
}else{
_25=$.data(this,"radiobutton",{options:$.extend({},$.fn.radiobutton.defaults,$.fn.radiobutton.parseOptions(this),_23),radiobutton:_2(this)});
}
_25.options.originalChecked=_25.options.checked;
_6(this);
_e(this);
_13(this);
});
};
$.fn.radiobutton.methods={options:function(jq){
var _26=jq.data("radiobutton");
return $.extend(_26.options,{value:_26.radiobutton.find(".radiobutton-value").val()});
},setValue:function(jq,_27){
return jq.each(function(){
$(this).val(_27);
$.data(this,"radiobutton").radiobutton.find(".radiobutton-value").val(_27);
});
},enable:function(jq){
return jq.each(function(){
_d(this,false);
});
},disable:function(jq){
return jq.each(function(){
_d(this,true);
});
},check:function(jq){
return jq.each(function(){
_c(this,true);
});
},uncheck:function(jq){
return jq.each(function(){
_c(this,false);
});
},clear:function(jq){
return jq.each(function(){
_c(this,false);
});
},reset:function(jq){
return jq.each(function(){
var _28=$(this).radiobutton("options");
_c(this,_28.originalChecked);
});
}};
$.fn.radiobutton.parseOptions=function(_29){
var t=$(_29);
return $.extend({},$.parser.parseOptions(_29,["label","labelPosition","labelAlign",{labelWidth:"number"}]),{value:(t.val()||undefined),checked:(t.attr("checked")?true:undefined),disabled:(t.attr("disabled")?true:undefined)});
};
$.fn.radiobutton.defaults={width:20,height:20,value:null,disabled:false,checked:false,label:null,labelWidth:"auto",labelPosition:"before",labelAlign:"left",onChange:function(_2a){
}};
})(jQuery);

