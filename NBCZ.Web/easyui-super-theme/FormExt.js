

/**
 * ajax默认设置
 * 包括默认提交方式为POST，
 * 判断后台是否是重定向
 */
$.ajaxSetup( {    
    //设置ajax请求结束后的执行动作    
    complete: function (xhr, textStatus) {
        if (xhr.responseText.indexOf("<title>Admin-后台登录</title>") != -1) {
            arr = window.location.pathname.replace(/(^\/)|(\/$)/g, '').split("/");
            if (arr[0] == "CRM" || arr[0] == "crm") {
                top.location = "/CRM/Login";
            } else {
                top.location = "/Login";
            }
        }
    },
}); 

//获取form的json的数据
function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        if (n['value']) {
            if (indexed_array.hasOwnProperty(n['name'])) {
                indexed_array[n['name']] = indexed_array[n['name']] + ","+ n['value'] ;
            }
            else {
                indexed_array[n['name']] = n['value'];
            }
        }

    });

    return indexed_array;
};
//获取form的数组的数据
function getFormDataExt($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = [];
    $.map(unindexed_array, function (n, i) {
        if (n['value']) {
            indexed_array.push([n['name']] + "=" + n['value']);
        }
    });
    return indexed_array;
};

var $btn;
function ajaxFromBegin() {
    $btn = $("#ajaxFromBegin input[type='submit']");
    $btn.val("提交中..").attr("disabled", "disabled");
}

function checkFormVal(form) {
    $form = $("#form1");
    if (form) {
        $form = $(form);
    }
    if (!$form.form('validate')) {
        return false;
    }
    else {
        return true;
    }
}


//权限检查 元素移除 IE8 不支持forEach
function checkAuth(arr) {
    $.each(arr, function (i, v) {
        if (v) {
            var code = v.code;
            var btns = v.btn;
            if (code && btns) {
                if (code == "False") {
                    $.each(btns, function (j, btn) {
                        $(btn).remove();
                    })
                }
            }
        }
    });
}

//显示提示
function ShowTip(msginfo) {
    $.messager.show({
        msg: msginfo,
        timeout: 3000,
        showType: 'fade',
        showSpeed: 300,
        width: 'auto',
        height: 'auto',
        style: {
            left: '45%',
            top: '50%'
        }
    });
}


//人民币金额转大写程序 JavaScript版     
//CopyRight Bardo QI
function numToCny(num) {
    var strOutput = "";
    var strUnit = '仟佰拾亿仟佰拾万仟佰拾元角分';
    num += "00";
    var intPos = num.indexOf('.');
    if (intPos >= 0)
        num = num.substring(0, intPos) + num.substr(intPos + 1, 2);
    strUnit = strUnit.substr(strUnit.length - num.length);
    for (var i = 0; i < num.length; i++)
        strOutput += '零壹贰叁肆伍陆柒捌玖'.substr(num.substr(i, 1), 1) + strUnit.substr(i, 1);
    return strOutput.replace(/零角零分$/, '整').replace(/零[仟佰拾]/g, '零').replace(/零{2,}/g, '零').replace(/零([亿|万])/g, '$1').replace(/零+元/, '元').replace(/亿零{0,3}万/, '亿').replace(/^元/, "零元");
}


//var $inputMust = $(".input-must");//input-must样式控制
//$inputMust.blur(function () {
//    if ($(this).val()) {
//        $(this).addClass("input-must-clear");
//    }
//}).focus(function () {
//    $(this).removeClass("input-must-clear");
//});
//.input-must-clear {border-color:#dedede !important; }
//.input-must-clear:focus{border-color:#3bb4f2 !important; }

(function ($) {
    //只能输入0-9的数字和小数点  
    $.fn.mustFloat = function () {
        return validate($(this), /[^0-9.]/g);
    };
    //只能输入>0的正整数  
    $.fn.mustInt = function () {
        return validate($(this), /\D|^0/g);
    };
    function validate(_obj, reg) {
        _obj.on("keyup blur", function () {
            $(this).val($(this).val().replace(reg, ''));
        }).bind("paste", function () {  //CTR+V事件处理    
            $(this).val($(this).val().replace(reg, ''));
        }).css("ime-mode", "disabled"); //CSS设置输入法不可用    
        return _obj;
    }
})(jQuery);

$(function () {

    //更多搜索
    $("#btn-SearchMore").click(function () {
        $(this).toggleClass("btn-search-click");
        $(".search-More").toggle();
    });

    $("#btn-cancel").click(function () {
        $("#btn-SearchMore").click();
    })

    $("#btn-reset").click(function () {
        //$(this).closest('form')[0].reset();
        $(this).closest('form').form('reset');
    })

    $(".btn-reset").click(function () {
        //$(this).closest('form')[0].reset();
        $(this).closest('form').form('reset');
    })




})

function hideSearchMore() {
    $("#btn-SearchMore").removeClass("btn-search-click");
    $(".search-More").hide();
}






/**
* 扩展combox验证，easyui原始只验证select text的值，不支持value验证
*/
$.extend($.fn.validatebox.defaults.rules, {
    comboxValidate: {
        validator: function (value, param) {
           
            var el = param[0];
            var inval = param[1];//验证空的Text
            console.log(value);
            if (!value) return false;
            if (value == inval) {
                return false;
            }
            return true;
        },
        message: "{2}"
    },
    regex: {
        //扩展textbox正则验证
        validator: function (value, param) {
            var regexp = param[0];
            console.log(regexp);
            return new RegExp(regexp).test(value);
        },
        message: "{1}"
    },
    maxLength: {
        //最大长度验证
        validator: function (value, param) {
            return !value || value.length <= param[0];
        },
        message: "{1}"
    },
    minLength: {
        //最小长度验证
        validator: function (value, param) {
            return value && value.length >= param[0];
        },
        message: "{1}"
    },
    ceiling: {
        validator: function (value, param) {
            var maxNum = param[0];
            if (maxNum && /^\-?(0|([1-9]\d*))(\.\d+)?$/.test(maxNum))
                return value <= maxNum;
            else return true;
        },
        message: "{1}"
    },
    date: {
       validator: function (value, param) {
           return value&&vilidataDate(value);
    },
    message: "日期格式错误"
    }
});
$.extend($.messager, {
    loading: function (msg) {
        if (msg == "close") {
            $(".datagrid-mask").remove();
            $(".datagrid-mask-msg").remove();
            return;
        }
        msg = !msg ? "正在处理，请稍候。。。" : msg;
        $("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: "100%", height: $(window).height(),"z-index":9000000000 }).appendTo("body");
        $("<div class=\"datagrid-mask-msg\"></div>")
            .html(msg).appendTo("body")
            .css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2,"z-index":9000000001 });
    }
});





/**     
 * 对Date的扩展，将 Date 转化为指定格式的String     
 * 月(M)、日(d)、12小时(h)、24小时(H)、分(m)、秒(s)、周(E)、季度(q) 可以用 1-2 个占位符     
 * 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)     
 * eg:     
 * (new Date()).pattern("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423     
 * (new Date()).pattern("yyyy-MM-dd E HH:mm:ss") ==> 2009-03-10 二 20:09:04     
 * (new Date()).pattern("yyyy-MM-dd EE hh:mm:ss") ==> 2009-03-10 周二 08:09:04     
 * (new Date()).pattern("yyyy-MM-dd EEE hh:mm:ss") ==> 2009-03-10 星期二 08:09:04     
 * (new Date()).pattern("yyyy-M-d h:m:s.S") ==> 2006-7-2 8:9:4.18     
 */
Date.prototype.pattern = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份        
        "d+": this.getDate(), //日        
        "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时        
        "H+": this.getHours(), //小时        
        "m+": this.getMinutes(), //分        
        "s+": this.getSeconds(), //秒        
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度        
        "S": this.getMilliseconds() //毫秒        
    };
    var week = {
        "0": "/u65e5",
        "1": "/u4e00",
        "2": "/u4e8c",
        "3": "/u4e09",
        "4": "/u56db",
        "5": "/u4e94",
        "6": "/u516d"
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[this.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}

//根据id复制文本
function copyText(id) {
    var Url2 = document.getElementById(id);
    Url2.select(); // 选择对象
    try {
        document.execCommand("Copy"); // 执行浏览器复制命令
    }
    catch (e) {
        alert("复制失败，请手动复制");
    }
}

//--单据颜色的显示
function datagridcolor(status) {
    var recolorstring = "";
    switch (status) {
        case -1:
            recolorstring = "color:#999999;";
            break;
        case 0:
            recolorstring = "";
            break;

        case 1:
        case 2:
        case 3:
        case 4:
            recolorstring = "color:#FF7621";
            break;

        case 5:
            recolorstring = "color:#008000";
            break;

        default:
            recolorstring = "";
    }
    return recolorstring;
}

//--单据状态的显示
function OrderstatusName(status) {
  
    var recolorstring = "";
    switch (status) {
        case -1:
            recolorstring = "停用";
            break;
        case 0:
            recolorstring = "未提交";
            break;

        case 1:
            recolorstring = "等待复核";
            break;
        case 2:
            recolorstring = "等待审核";
            break;
        case 3:
            recolorstring = "审核中";
            break;
        case 4:
            recolorstring = "审核中";
            break;

        case 5:
            recolorstring = "审核通过";
            break;

        default:
            recolorstring = "";
    }
    return recolorstring;
}

function MaterialTransportStatus(status)
{
    var StatusName = "";
    switch (status)
    {
        case -1: StatusName = "复核拒绝"; break;
        case 0: StatusName = "无需复核"; break;
        case 1: StatusName = "需要复核"; break;
        case 2: StatusName = "复核通过"; break;
    }
    return StatusName;
}

function GetNoticeSteName(statusint) {

    var recolorstring = "";
    switch (statusint) {
        case -1:
            recolorstring = "停用";
            break;
        case 0:
            recolorstring = "未提交";
            break;

        case 1:
            recolorstring = "等待调度";
            break;
        case 2:
            recolorstring = "等待出库";
            break;
        case 3:
            recolorstring = "出库完成";
            break;
        case 4:
            recolorstring = "出库后司机确认";
            break;

        case 5:
            recolorstring = "店长签收";
            break;
        case 6:
            recolorstring = "等待返料物资";
            break;

        case 7:
            recolorstring = "确认返料物资";
            break;

        case 8:
            recolorstring = "返料后司机确认";
            break;

        case 9:
            recolorstring = "等待仓库财务签收";
            break;

        case 80:
            recolorstring = "完成";
            break;

        default:
            recolorstring = "";
    }
    return recolorstring;

}

//--单据颜色的显示
function Noticedatagridcolor(status) {
    var recolorstring = "";
    switch (status) {
        case -1:
            recolorstring = "color:#999999;";
            break;
        case 0:
            recolorstring = "";
            break;

        case 1:
        case 2:
        case 3:
        case 5:
        case 6:
        case 7:
        case 8:
        case 9:
            recolorstring = "color:#FF7621";
            break;


        case 80:
            recolorstring = "color:#008000";
            break;

        default:
            recolorstring = "";
    }
    return recolorstring;
}

//--单据颜色的显示
function datareceipt(status) {
    var recolorstring = "";
    switch (status) {
        case -1:
            recolorstring = "color:#999999;";
            break;
        case 0:
            recolorstring = "";
            break;

        case 1:
            recolorstring = "color:#FF7621";
            break;
        case 2:
            recolorstring = "color:#008000";
            break;

        default:
            recolorstring = "";
    }
    return recolorstring;
}
//获取地址栏参数
function getParam(paramName) {
    paramValue = "", isFound = !1;
    if (this.location.search.indexOf("?") == 0 && this.location.search.indexOf("=") > 1) {
        arrSource = unescape(this.location.search).substring(1, this.location.search.length).split("&"), i = 0;
        while (i < arrSource.length && !isFound) arrSource[i].indexOf("=") > 0 && arrSource[i].split("=")[0].toLowerCase() == paramName.toLowerCase() && (paramValue = arrSource[i].split("=")[1], isFound = !0), i++
    }
    return paramValue == "" && (paramValue = null), paramValue
}

//grid开始编辑
function beginEdit(grids) {
    for (var j = 0; j < grids.length; j++) {
        var grid = $(grids[j]);
        var rows = grid.datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            grid.datagrid('beginEdit', i);
        }
    }
}
//grid结束编辑
function endEdit(grids) {
    for (var j = 0; j < grids.length; j++) {
        var grid = $(grids[j]);
        var rows = grid.datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            grid.datagrid('endEdit', i);
        }
    }
}
//取消编辑
function cancelEdit(grids) {
    //cancelEdit
    for (var j = 0; j < grids.length; j++) {
        var grid = $(grids[j]);
        var rows = grid.datagrid('getRows');
        for (var i = 0; i < rows.length; i++) {
            grid.datagrid('cancelEdit', i);
        }
    }
}


//添加easyui tab
function addTab(title, url,tabId) {
    var $tab = $("#" + (tabId||"tabs"));
    if ($tab.tabs('exists', title)) {
        $tab.tabs('select', title);
        var currTab = $tab.tabs('getSelected');
        var url =url|| $(currTab.panel('options').content).attr('src');
        if (url != undefined && currTab.panel('options').title != 'Home') {
            $tab.tabs('update', {
                tab: currTab,
                options: {
                    content: createFrame(url)
                }
            });
        }
    } else {
        var content = createFrame(url);
        $tab.tabs('add', {
            title: title,
            content: content,
            closable: true
        });
    }
}

function createFrame(url) {
    var s = '<iframe  frameborder="0"  src="' + url + '" style="width:100%;height:99%;"></iframe>';
    return s;
}


//检查日期时间格式是否正确
function datetimevalid(strDateTime) {
    var splited = strDateTime.split(' ');
    var strDate = null;
    var strTime = null;

    if (splited[0] != null)
        strDate = splited[0];
    else
        return false;
    if (splited[1] != null)
        strTime = splited[1];

    if (strDate != null) {
        if (!vilidataDate(strDate)) {
            return false;
        }
    }

    if (strTime != null) {
        if (!validateTime(strTime)) {
            return false;
        }
    }
    return true;
}
//检查日期格式是否正确
function vilidataDate(strdate) {
    var regex = /^((((19|20)\d{2})-(0?(1|[3-9])|1[012])-(0?[1-9]|[12]\d|30))|(((19|20)\d{2})-(0?[13578]|1[02])-31)|(((19|20)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|((((19|20)([13579][26]|[2468][048]|0[48]))|(2000))-0?2-29))$/;
    if (strdate.match(regex) == null)
        return false;
    return true;
}
//检查时间格式是否正确
function validateTime(strTime) {
    var regex = /^(?:[01]\d|2[0-3])(?::[0-5]\d){2}$/;
    var regexShort = /^(?:[01]\d|2[0-3])(?::[0-5]\d)$/;
    if (strTime.match(regex) != null)
        return true;
    if (strTime.match(regexShort) != null)
        return true;
    return false;
}