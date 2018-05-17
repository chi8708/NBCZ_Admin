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