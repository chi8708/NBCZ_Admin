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
var _4=$("<span class=\"checkbox inputbox\">"+"<span class=\"checkbox-inner\">"+"<svg xml:space=\"preserve\" focusable=\"false\" version=\"1.1\" viewBox=\"0 0 24 24\"><path d=\"M4.1,12.7 9,17.6 20.3,6.3\" fill=\"none\" stroke=\"white\"></path></svg>"+"</span>"+"<input type=\"checkbox\" class=\"checkbox-value\">"+"</span>").insertAfter(_3);
var t=$(_3);
t.addClass("checkbox-f").hide();
var _5=t.attr("name");
if(_5){
t.removeAttr("name").attr("checkboxName",_5);
_4.find(".checkbox-value").attr("name",_5);
}
return _4;
};
function _6(_7){
var _8=$.data(_7,"checkbox");
var _9=_8.options;
var _a=_8.checkbox;
var _b="_easyui_checkbox_"+(++_1);
_a.find(".checkbox-value").attr("id",_b);
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
$(_7).checkbox("setValue",_9.value);
_c(_7,_9.checked);
_d(_7,_9.disabled);
};
function _e(_f){
var _10=$.data(_f,"checkbox");
var _11=_10.options;
var _12=_10.checkbox;
_12.unbind(".checkbox").bind("click.checkbox",function(){
if(!_11.disabled){
_c(_f,!_11.checked);
}
});
};
function _13(_14){
var _15=$.data(_14,"checkbox");
var _16=_15.options;
var _17=_15.checkbox;
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
var _1a=$.data(_18,"checkbox");
var _1b=_1a.options;
var _1c=_1a.checkbox;
_1c.find(".checkbox-value")._propAttr("checked",_19);
var _1d=_1c.find(".checkbox-inner").css("display",_19?"":"none");
if(_19){
_1d.addClass("checkbox-checked");
}else{
_1d.removeClass("checkbox-checked");
}
if(_1b.checked!=_19){
_1b.checked=_19;
_1b.onChange.call(_18,_19);
}
};
function _d(_1e,_1f){
var _20=$.data(_1e,"checkbox");
var _21=_20.options;
var _22=_20.checkbox;
var rv=_22.find(".checkbox-value");
_21.disabled=_1f;
if(_1f){
$(_1e).add(rv)._propAttr("disabled",true);
_22.addClass("checkbox-disabled");
}else{
$(_1e).add(rv)._propAttr("disabled",false);
_22.removeClass("checkbox-disabled");
}
};
$.fn.checkbox=function(_23,_24){
if(typeof _23=="string"){
return $.fn.checkbox.methods[_23](this,_24);
}
_23=_23||{};
return this.each(function(){
var _25=$.data(this,"checkbox");
if(_25){
$.extend(_25.options,_23);
}else{
_25=$.data(this,"checkbox",{options:$.extend({},$.fn.checkbox.defaults,$.fn.checkbox.parseOptions(this),_23),checkbox:_2(this)});
}
_25.options.originalChecked=_25.options.checked;
_6(this);
_e(this);
_13(this);
});
};
$.fn.checkbox.methods={options:function(jq){
var _26=jq.data("checkbox");
return $.extend(_26.options,{value:_26.checkbox.find(".checkbox-value").val()});
},setValue:function(jq,_27){
return jq.each(function(){
$(this).val(_27);
$.data(this,"checkbox").checkbox.find(".checkbox-value").val(_27);
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
var _28=$(this).checkbox("options");
_c(this,_28.originalChecked);
});
}};
$.fn.checkbox.parseOptions=function(_29){
var t=$(_29);
return $.extend({},$.parser.parseOptions(_29,["label","labelPosition","labelAlign",{labelWidth:"number"}]),{value:(t.val()||undefined),checked:(t.attr("checked")?true:undefined),disabled:(t.attr("disabled")?true:undefined)});
};
$.fn.checkbox.defaults={width:20,height:20,value:null,disabled:false,checked:false,label:null,labelWidth:"auto",labelPosition:"before",labelAlign:"left",onChange:function(_2a){
}};
})(jQuery);

