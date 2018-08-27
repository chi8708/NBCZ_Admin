
//获取form的json的数据
function getFormData($form) {
    var unindexed_array = $form.serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        if (n['value']) {
            indexed_array[n['name']] = n['value'];
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
    $from = $("#form1");
    if (form) {
        $from = $(from);
    }
    if (!$from.form('validate')) {
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
    $("#btn-SearchMore").click(function () {
        $("#btn-SearchMore").toggleClass("btn-search-click");
        $(".search-More").toggle();
    });

    $("#btn-cancel").click(function () {
        $("#btn-SearchMore").click();
    })

    $("#btn-reset").click(function () {
        $(this).closest('form')[0].reset();
    })
})

