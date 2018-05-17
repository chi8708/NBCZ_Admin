/**
 @Name：layui.form 表单组件
 @Author：贤心
 @License：MIT

 */

layui.define('layer', function(exports){
    "use strict";

    var $ = layui.$
        ,layer = layui.layer
        ,hint = layui.hint()
        ,device = layui.device()
        ,valueStr = []

        ,MOD_NAME = 'form', ELEM = '.layui-form', THIS = 'layui-this', SHOW = 'layui-show', HIDE = 'layui-hide', DISABLED = 'layui-disabled'

        , Form = function () {
        this.config = {
            verify: {
                required: [
                    /[\S]+/
                    ,'必填项不能为空'
                ]
                ,phone: [
                    /^1\d{10}$/
                    ,'请输入正确的手机号'
                ]
                ,email: [
                    /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/
                    ,'邮箱格式不正确'
                ]
                ,url: [
                    /(^#)|(^http(s*):\/\/[^\s]+\.[^\s]+)/
                    ,'链接格式不正确'
                ]
                ,number: function(value){
                    if(!value || isNaN(value)) return '只能填写数字'
                }
                ,date: [
                    /^(\d{4})[\u4e00-\u9fa5]|[-\/](\d{1}|0\d{1}|1[0-2])([\u4e00-\u9fa5]|[-\/](\d{1}|0\d{1}|[1-2][0-9]|3[0-1]))*$/
                    ,'日期格式不正确'
                ]
                ,identity: [
                    /(^\d{15}$)|(^\d{17}(x|X|\d)$)/
                    ,'请输入正确的身份证号'
                ]
            }
        };
    };

    //全局设置
    Form.prototype.set = function(options){
        var that = this;
        $.extend(true, that.config, options);
        return that;
    };

    //验证规则设定
    Form.prototype.verify = function(settings){
        var that = this;
        $.extend(true, that.config.verify, settings);
        return that;
    };

    //表单事件监听
    Form.prototype.on = function(events, callback){
        return layui.onevent.call(this, MOD_NAME, events, callback);
    };

    //查找指定的元素在数组中的位置
    Array.prototype.indexOfElem = function(val){
        for (var i = 0; i < this.length; i++) {
            if (this[i] == val) return i;
        }
        return -1;
    }

    //得到元素的索引
    Array.prototype.removeElem = function(val) {
        var index = this.indexOfElem(val);
        if (index > -1) {
            this.splice(index, 1);
        }
    };

    //placeholder是否显示
    Form.prototype.hidePlaceholder = function(title,select){
        //判断是否全部删除，如果全部删除则展示提示信息
        if(title.find(".multiSelect a").length != 0){
            title.children("input.layui-input").attr("placeholder","");
        }else{
            title.children("input.layui-input").attr("placeholder",select.find("option:eq(0)").text());
        }
    }

    //表单控件渲染
    Form.prototype.render = function(type, filter){
        var that = this
            ,elemForm = $(ELEM + function(){
            return filter ? ('[lay-filter="' + filter +'"]') : '';
        }())
            ,items = {

            //下拉选择框
            select: function(){
                var TIPS = '请选择', CLASS = 'layui-form-select', TITLE = 'layui-select-title'
                    ,NONE = 'layui-select-none', initValue = '', thatInput

                    ,selects = elemForm.find('select'), hide = function(e, clear){
                    if(!$(e.target).parent().hasClass(TITLE) || clear){
                        $('.'+CLASS).removeClass(CLASS+'ed ' + CLASS+'up');
                        thatInput && initValue && thatInput.val(initValue);
                    }
                    thatInput = null;
                }

                    ,events = function(reElem, disabled, isSearch){
                    var select = $(this)
                        ,title = reElem.find('.' + TITLE)
                        ,input = title.find('input')
                        ,multiSelect = title.find(".multiSelect")
                        ,dl = reElem.find('dl:eq(0)')
                        ,dds = dl.find('div>dd').length==0 ? dl.find('dd') : dl.find('div>dd');

                    that.hidePlaceholder(title,select);
                    if(disabled) return;

                    //展开下拉
                    var showDown = function(){
                        var top = reElem.offset().top + reElem.outerHeight() + 5 - win.scrollTop()
                            ,dlHeight = dl.outerHeight();
                        $("body").find("."+CLASS).removeClass(CLASS+'ed');
                        reElem.addClass(CLASS+'ed');
                        dds.removeClass(HIDE);

                        //上下定位识别
                        if(top + dlHeight > win.height() && top >= dlHeight){
                            reElem.addClass(CLASS + 'up');
                        }
                    }, hideDown = function(choose){
                        reElem.removeClass(CLASS+'ed ' + CLASS+'up');
                        input.blur();

                        if(choose) return;

                        notOption(input.val(), function(none){
                            if(none){
                                initValue = dl.find('.'+THIS).html();
                                input && input.val(initValue);
                            }
                        });
                    };

                    //点击标题区域
                    title.on('click', function(e){
                        if(typeof select.attr('multiple') && typeof select.attr('multiple') === 'string') {
                            reElem.hasClass(CLASS + 'ed') ? hideDown(true): showDown();
                            dl.find(".layui-input").val("");
                            setTimeout(function () {  // 直接写focus，会失败。。
                                dl.find(".layui-input").focus();
                            },500)
                        }else{
                            reElem.hasClass(CLASS + 'ed') ? (
                                hideDown()
                            ) : (
                                hide(e, true),
                                    showDown()
                            );
                            dl.find('.' + NONE).remove();
                        }
                        e.stopPropagation();
                        //选择完毕关闭下拉
                        $(document).not(title.find(".layui-anim")).off('click', hide).on('click', hide);
                    });

                    //点击箭头获取焦点
                    title.find('.layui-edge').on('click', function(){
                        input.focus();
                    });

                    //键盘事件
                    input.on('keyup', function(e){
                        var keyCode = e.keyCode;
                        //Tab键
                        if(keyCode === 9){
                            showDown();
                        }
                    }).on('keydown', function(e){
                        var keyCode = e.keyCode;
                        //Tab键
                        if(keyCode === 9){
                            hideDown();
                        } else if(keyCode === 13){ //回车键
                            e.preventDefault();
                        }
                    });

                    //检测值是否不属于select项
                    var notOption = function(value, callback, origin){
                        var num = 0;
                        var ignoreCase =  typeof(select.attr("lay-case"))=="undefined"; // 忽略大小写
                        layui.each(dds, function(){
                            var othis = $(this)
                                ,text = othis.text()
                                ,not = ignoreCase?text.toLowerCase().indexOf(value.toLowerCase()) === -1 : text.indexOf(value) === -1;
                            if(value === '' || (origin === 'blur') ? value !== text : not) num++;
                            origin === 'keyup' && othis[not ? 'addClass' : 'removeClass'](HIDE);
                        });
                        var none = num === dds.length;
                        return callback(none), none;
                    };

                    //搜索匹配
                    var search = function(e){
                        var value = this.value, keyCode = e.keyCode;

                        if(keyCode === 9 || keyCode === 13
                            || keyCode === 37 || keyCode === 38
                            || keyCode === 39 || keyCode === 40
                        ){
                            return false;
                        }

                        notOption(value, function(none){
                            if(none){
                                dl.find('.'+NONE)[0] || dl.append('<p class="'+ NONE +'">无匹配项</p>');
                            } else {
                                dl.find('.'+NONE).remove();
                            }
                        }, 'keyup');

                        if(value === ''){
                            dl.find('.'+NONE).remove();
                        }
                    };
                    if(isSearch){
                        input.on('keyup', search).on('blur', function(e){
                            thatInput = input;
                            initValue = dl.find('.'+THIS).html();
                            setTimeout(function(){
                                notOption(input.val(), function(none){
                                    if(none && !initValue){
                                        // input.val(''); //todo 不知道什么用处，影响到我的简化多选下拉框了
                                    }
                                }, 'blur');
                            }, 200);
                        });
                    }

                    //多选删除
                    title.delegate(".multiSelect a i","click",function(e){
                        var valueStr = select.val() || [];
                        var _this = $(this);
                        e.stopPropagation();
                        title.find("dd").each(function(){
                            if($(this).find("span").text() == _this.siblings("span").text()){
                                $(this).find("input").removeAttr("checked");
                                $(this).find(".layui-form-checkbox").removeClass("layui-form-checked");
                                valueStr.removeElem($(this).attr("lay-value"));
                                select.val(valueStr);
                                layui.event.call(this, MOD_NAME, 'select('+ select.attr('lay-filter') +')', {
                                    elem: select[0]
                                    ,value: valueStr
                                    ,othis: reElem
                                });
                            }
                        });
                        $(this).parent("a").remove();
                        that.hidePlaceholder(title,select);
                    });

                    //选择
                    var omit = typeof select.attr("lay-omit") === 'undefined'; // 简写为 已选择x条
                    dds.on('click', function(event){
                        var othis = $(this), value = othis.attr('lay-value'),valueStr = select.val() || [];
                        var filter = select.attr('lay-filter'); //获取过滤器
                        if(typeof select.attr('multiple') && typeof select.attr('multiple') === 'string'){
                            if(othis.hasClass(DISABLED)) return false;
                            if(othis.find("input[type='checkbox']").is(':checked')){
                                if (omit) {
                                    multiSelect.html(multiSelect.html() + "<a href='javascript:;'><span>"+othis.find("span").text()+"</span><i></i></a>");
                                } else {
                                    input.eq(0).val("已选择"+othis.parent().find('[type=checkbox]:checked').length+"条");
                                }
                                valueStr.push(value);
                            }else{
                                if (omit) {
                                    multiSelect.find("a").each(function(){
                                        if($(this).find("span").text() == othis.find("span").text()){
                                            $(this).remove();
                                            valueStr.removeElem(value);
                                        }
                                    })
                                } else {
                                    var num =othis.parent().find('[type=checkbox]:checked').length;
                                    if (num == 0) {
                                        input.eq(0).val("");
                                    } else {
                                        input.eq(0).val("已选择"+num+"条");
                                    }
                                    valueStr.removeElem(value);
                                }

                            }
                            select.val(valueStr).removeClass('layui-form-danger');
                            layui.event.call(this, MOD_NAME, 'select('+ filter +')', {
                                elem: select[0]
                                ,value: valueStr
                                ,othis: reElem
                            });
                            that.hidePlaceholder(title,select);
                        }else{
                            if(othis.hasClass(DISABLED)) return false;
                            if(othis.hasClass('layui-select-tips')){
                                input.val('');
                            } else {
                                input.val(othis.text());
                                othis.addClass(THIS);
                            }
                            othis.siblings().removeClass(THIS);
                            select.val(value).removeClass('layui-form-danger');
                            layui.event.call(this, MOD_NAME, 'select('+ filter +')', {
                                elem: select[0]
                                ,value: value
                                ,othis: reElem
                            });
                            hideDown(true);
                            return false;
                            reElem.find('dl>dt').on('click', function(e){
                                return false;
                            });
                        }
                    });
                    dl.on('click', function (e) {
                        if(typeof select.attr('multiple') && typeof select.attr('multiple') === 'string') {
                            e.stopPropagation(); // 阻止事件冒泡，使下拉框长显示
                        }
                    })
                }

                selects.each(function(index, select){
                    var othis = $(this)
                        ,hasRender = othis.next('.'+CLASS)
                        ,disabled = this.disabled
                        ,selected = $(select.options[select.selectedIndex]) //获取当前选中项
                        ,optionsFirst = select.options[0];
                    if(typeof othis.attr('multiple') && typeof othis.attr('multiple') === 'string'){
                        var value = $(select).val()
                    }else{
                        var value = select.value
                    }
                    var isSearchInput = typeof othis.attr('lay-search') != 'undefined';

                    if(typeof othis.attr('lay-ignore') === 'string') return othis.show();

                    var isSearch = typeof othis.attr('lay-search') === 'string'
                        ,isMultiple = typeof othis.attr('multiple') === 'string'
                        ,placeholder = optionsFirst ? (
                        optionsFirst.value ? TIPS : (optionsFirst.innerHTML || TIPS)
                    ) : TIPS;

                    //替代元素
                    var inputValue =  !(typeof $(select).attr("lay-omit") === 'undefined')&&value!=null&&value.length>0 ? '已选择'+value.length+"条" : "";
                    if(typeof othis.attr('multiple') && typeof othis.attr('multiple') === 'string') {
                        var reElem = $(['<div class="' + (isMultiple ? '' : 'layui-unselect ') + CLASS + (disabled ? ' layui-select-disabled' : '') + '">'
                            , '<div class="' + TITLE + '"><input type="text" class="layui-input" value="'+inputValue+'" placeholder="' + placeholder + '"><div class="layui-input multiSelect" >' + function(){
                                var aLists = [];
                                if(typeof $(select).attr("lay-omit")==='undefined' && value != null && value != undefined && value.length != 0){
                                    for(var aList = 0;aList<value.length;aList++){
                                        if(value[aList]){
                                            var chooseText = '';
                                            $(select).find('option').each(function (i, ele) {
                                               if (typeof $(this).attr('value') == 'undefined') {
                                                    if ($(this).text() == value[aList]) {
                                                        chooseText = $(this).text();
                                                        return false;
                                                    }
                                               } else if ($(this).attr('value') == value[aList]) {
                                                   chooseText = $(this).text();
                                                   return false;
                                               }
                                            });
                                            aLists.push("<a href='javascript:;'><span>"+chooseText+"</span><i></i></a>")
                                        }
                                    }
                                }
                                return aLists.join('');
                            }(othis.find('*')) + '<i class="layui-edge"></i></div>'
                            , '<dl class="layui-anim layui-anim-upbit' + (othis.find('optgroup')[0] ? ' layui-select-group' : '') + '" '+ (isSearchInput?'style="overflow-y: hidden;padding: 5px 0px 0px;"':'') +'>' + function (options) {
                                var arr = [];
                                layui.each(options, function (index, item) {
                                    if (index === 0 && !item.value) {
                                        if (isSearchInput) {
                                            arr.push('<dd lay-value="" class="layui-select-tips" style="padding-right: 10px; margin-bottom: 5px;"><input class="layui-input" placeholder="关键字搜索"></dd>');
                                        } else {
                                            arr.push('<dd lay-value="" class="layui-select-tips">' + (item.innerHTML || TIPS) + '</dd>');
                                        }
                                    }else {
                                        if (index ===1 && isSearchInput) {
                                            arr.push('<div  style="max-height: 247px; overflow-y: auto" >')
                                        }
                                        if(value != null && value != undefined && value.length != 0) {
                                            for (var checkedVal = 0; checkedVal < value.length; checkedVal++) {
                                                if (value[checkedVal] == item.value) {
                                                    arr.push('<dd lay-value="' + item.value + '">' + '<input type="checkbox" ' + (item.disabled ? "disabled" : "") + ' checked lay-filter="searchChecked" title="' + item.innerHTML + '" lay-skin="primary"></dd>');
                                                    return false;
                                                }
                                            }
                                        }
                                        arr.push('<dd lay-value="' + item.value + '">' + '<input type="checkbox" ' + (item.disabled ? "disabled" : "") + ' lay-filter="searchChecked" title="' + item.innerHTML + '" lay-skin="primary"></dd>');
                                    }
                                });
                                arr.length === 0 && arr.push('<dd lay-value="" class="' + DISABLED + '">没有选项</dd>');
                                if (isSearchInput) {
                                    arr.join("</div>");
                                }
                                return arr.join('');
                            }(othis.find('*')) + '</dl>'
                            , '</div>'].join(''));

                        hasRender[0] && hasRender.remove(); //如果已经渲染，则Rerender
                        othis.after(reElem);
                        events.call(this, reElem, disabled, isMultiple);
                    }else{
                        var reElem = $(['<div class="' + (isSearch ? '' : 'layui-unselect ') + CLASS + (disabled ? ' layui-select-disabled' : '') + '">'
                            , '<div class="' + TITLE + '"><input type="text" placeholder="' + placeholder + '" value="' + (value ? selected.html() : '') + '" ' + (isSearch ? '' : 'readonly') + ' class="layui-input' + (isSearch ? '' : ' layui-unselect') + (disabled ? (' ' + DISABLED) : '') + '">'
                            , '<i class="layui-edge"></i></div>'
                            , '<dl class="layui-anim layui-anim-upbit' + (othis.find('optgroup')[0] ? ' layui-select-group' : '') + '">' + function (options) {
                                var arr = [];
                                layui.each(options, function (index, item) {
                                    if (index === 0 && !item.value) {
                                        arr.push('<dd lay-value="" class="layui-select-tips">' + (item.innerHTML || TIPS) + '</dd>');
                                    } else if (item.tagName.toLowerCase() === 'optgroup') {
                                        arr.push('<dt>' + item.label + '</dt>');
                                    } else {
                                        arr.push('<dd lay-value="' + item.value + '" class="' + (value === item.value ? THIS : '') + (item.disabled ? (' ' + DISABLED) : '') + '">' + item.innerHTML + '</dd>');
                                    }
                                });
                                arr.length === 0 && arr.push('<dd lay-value="" class="' + DISABLED + '">没有选项</dd>');
                                return arr.join('');
                            }(othis.find('*')) + '</dl>'
                            , '</div>'].join(''));

                        hasRender[0] && hasRender.remove(); //如果已经渲染，则Rerender
                        othis.after(reElem);
                        events.call(this, reElem, disabled, isSearch);
                    }
                });
            }
            //复选框/开关
            ,checkbox: function(){
                var CLASS = {
                    checkbox: ['layui-form-checkbox', 'layui-form-checked', 'checkbox']
                    ,_switch: ['layui-form-switch', 'layui-form-onswitch', 'switch']
                }
                    ,checks = elemForm.find('input[type=checkbox]')

                    ,events = function(reElem, RE_CLASS){
                    var check = $(this);

                    //勾选
                    reElem.on('click', function(){
                        var filter = check.attr('lay-filter') //获取过滤器
                            ,text = (check.attr('lay-text')||'').split('|');

                        if(check[0].disabled) return;

                        check[0].checked ? (
                            check[0].checked = false
                                ,reElem.removeClass(RE_CLASS[1]).find('em').text(text[1])
                        ) : (
                            check[0].checked = true
                                ,reElem.addClass(RE_CLASS[1]).find('em').text(text[0])
                        );

                        layui.event.call(check[0], MOD_NAME, RE_CLASS[2]+'('+ filter +')', {
                            elem: check[0]
                            ,value: check[0].value
                            ,othis: reElem
                        });
                    });
                }

                checks.each(function(index, check){
                    var othis = $(this), skin = othis.attr('lay-skin')
                        ,text = (othis.attr('lay-text')||'').split('|'), disabled = this.disabled;
                    if(skin === 'switch') skin = '_'+skin;
                    var RE_CLASS = CLASS[skin] || CLASS.checkbox;

                    if(typeof othis.attr('lay-ignore') === 'string') return othis.show();

                    //替代元素
                    var hasRender = othis.next('.' + RE_CLASS[0]);
                    var reElem = $(['<div class="layui-unselect '+ RE_CLASS[0] + (
                        check.checked ? (' '+RE_CLASS[1]) : '') + (disabled ? ' layui-checkbox-disbaled '+DISABLED : '') +'" lay-skin="'+ (skin||'') +'">'
                        ,{
                            _switch: '<em>'+ ((check.checked ? text[0] : text[1])||'') +'</em><i></i>'
                        }[skin] || ((check.title.replace(/\s/g, '') ? ('<span>'+ check.title +'</span>') : '') +'<i class="layui-icon">'+ (skin ? '&#xe605;' : '&#xe618;') +'</i>')
                        ,'</div>'].join(''));

                    hasRender[0] && hasRender.remove(); //如果已经渲染，则Rerender
                    othis.after(reElem);
                    events.call(this, reElem, RE_CLASS);
                });
            }
            //单选框
            ,radio: function(){
                var CLASS = 'layui-form-radio', ICON = ['&#xe643;', '&#xe63f;']
                    ,radios = elemForm.find('input[type=radio]')

                    ,events = function(reElem){
                    var radio = $(this), ANIM = 'layui-anim-scaleSpring';

                    reElem.on('click', function(){
                        var name = radio[0].name, forms = radio.parents(ELEM);
                        var filter = radio.attr('lay-filter'); //获取过滤器
                        var sameRadio = forms.find('input[name='+ name.replace(/(\.|#|\[|\])/g, '\\$1') +']'); //找到相同name的兄弟

                        if(radio[0].disabled) return;

                        layui.each(sameRadio, function(){
                            var next = $(this).next('.'+CLASS);
                            this.checked = false;
                            next.removeClass(CLASS+'ed');
                            next.find('.layui-icon').removeClass(ANIM).html(ICON[1]);
                        });

                        radio[0].checked = true;
                        reElem.addClass(CLASS+'ed');
                        reElem.find('.layui-icon').addClass(ANIM).html(ICON[0]);

                        layui.event.call(radio[0], MOD_NAME, 'radio('+ filter +')', {
                            elem: radio[0]
                            ,value: radio[0].value
                            ,othis: reElem
                        });
                    });
                };

                radios.each(function(index, radio){
                    var othis = $(this), hasRender = othis.next('.' + CLASS), disabled = this.disabled;

                    if(typeof othis.attr('lay-ignore') === 'string') return othis.show();

                    //替代元素
                    var reElem = $(['<div class="layui-unselect '+ CLASS + (radio.checked ? (' '+CLASS+'ed') : '') + (disabled ? ' layui-radio-disbaled '+DISABLED : '') +'">'
                        ,'<i class="layui-anim layui-icon">'+ ICON[radio.checked ? 0 : 1] +'</i>'
                        ,'<span>'+ (radio.title||'未命名') +'</span>'
                        ,'</div>'].join(''));

                    hasRender[0] && hasRender.remove(); //如果已经渲染，则Rerender
                    othis.after(reElem);
                    events.call(this, reElem);
                });
            }
        };
        type ? (
            items[type] ? items[type]() : hint.error('不支持的'+ type + '表单渲染')
        ) : layui.each(items, function(index, item){
            item();
        });
        return that;
    };
    //表单提交校验
    var submit = function(){
        var button = $(this), verify = form.config.verify, stop = null
            ,DANGER = 'layui-form-danger', field = {} ,elem = button.parents(ELEM)

            ,verifyElem = elem.find('*[lay-verify]') //获取需要校验的元素
            ,formElem = button.parents('form')[0] //获取当前所在的form元素，如果存在的话
            ,fieldElem = elem.find('input,select,textarea') //获取所有表单域
            ,filter = button.attr('lay-filter'); //获取过滤器

        //开始校验
        //layui.each(verifyElem, function(_, item){
        //    var othis = $(this), ver = othis.attr('lay-verify').split('|');
        //    var tips = '', value = othis.val();
        //    othis.removeClass(DANGER);
        //    layui.each(ver, function(_, thisVer){
        //        var isFn = typeof verify[thisVer] === 'function';
        //        if(verify[thisVer] && (isFn ? tips = verify[thisVer](value, item) : (typeof(value) == "string" ? !verify[thisVer][0].test(value) : (value == null ? true : !verify[thisVer][0].test(value[0])))) ){
        //            layer.msg(tips || verify[thisVer][1], {
        //                icon: 5
        //                ,shift: 6
        //            });
        //            //非移动设备自动定位焦点
        //            if(!device.android && !device.ios){
        //                item.focus();
        //            }
        //            othis.addClass(DANGER);
        //            return stop = true;
        //        }
        //    });
        //    if(stop) return stop;
        //});

        //开始校验 cts修改
        layui.each(verifyElem, function (_, item) {
            var othis = $(this)
          , vers = othis.attr('lay-verify').split('|')
          , verType = othis.attr('lay-verType') //提示方式
         , value = othis.val();

            layui.each(vers, function (_, thisVer) {
                var isTrue //是否命中校验
                , errorText = '' //错误提示文本
                , isFn = typeof verify[thisVer] === 'function';

                //匹配验证规则
                if (verify[thisVer]) {
                    var isTrue = isFn ? errorText = verify[thisVer](value, item) : !verify[thisVer][0].test(value);
                    errorText = errorText || verify[thisVer][1];

                    //如果是必填项或者非空命中校验，则阻止提交，弹出提示
                    if (isTrue) {
                        //提示层风格
                        if (verType === 'tips') {
                            layer.tips(errorText, function () {
                                if (typeof othis.attr('lay-ignore') !== 'string') {
                                    if (item.tagName.toLowerCase() === 'select' || /^checkbox|radio$/.test(item.type)) {
                                        return othis.next();
                                    }
                                }
                                return othis;
                            }(), { tips: 1 });
                        } else if (verType === 'alert') {
                            layer.alert(errorText, { title: '提示', shadeClose: true });
                        } else {
                            layer.msg(errorText, { icon: 5, shift: 6 });
                        }
                        if (!device.android && !device.ios) item.focus(); //非移动设备自动定位焦点
                        othis.addClass(DANGER);
                        return stop = true;
                    }
                }
            });
            if (stop) return stop;
        });
        if(stop) return false;

        layui.each(fieldElem, function(_, item){
            if(!item.name) return;
            if(/^checkbox|radio$/.test(item.type) && !item.checked) return;
            if((item.type).indexOf("multiple") != -1) {
                field[item.name] = $(item).val();
            }else{
                field[item.name] = item.value;
            }
        });

        //获取字段
        return layui.event.call(this, MOD_NAME, 'submit('+ filter +')', {
            elem: this
            ,form: formElem
            ,field: field
        });
    };

    //自动完成渲染
    var form = new Form()
        ,dom = $(document), win = $(window);

    form.render();

    //表单reset重置渲染
    dom.on('reset', ELEM, function(){
        var filter = $(this).attr('lay-filter');
        setTimeout(function(){
            form.render(null, filter);
        }, 50);
    });

    //表单提交事件
    dom.on('submit', ELEM, submit)
        .on('click', '*[lay-submit]', submit);

    exports(MOD_NAME, form);
});