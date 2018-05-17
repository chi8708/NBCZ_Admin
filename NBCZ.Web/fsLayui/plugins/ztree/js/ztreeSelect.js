
(function ($) {


    $.fn.extend({
        "treeSelect": function (option) {

            var __DEFT__ = {
                zNodes:null,
                treeId: "treeDemo2",
                menuContent: "#menuContent2",
                checkType:"",
                selectnode:"",
                selectedMulti:false,
                valuesDom:"ids"
            }

            $.extend(this,__DEFT__,option)
            var txtObj = $(this);
            var zNodes = this.zNodes;
            var treeId = this.treeId;
            var menuContent = this.menuContent;
            var checkType = this.checkType;
            var selectnode = this.selectnode;
            var selectedMulti = this.selectedMulti;
            var valuesDom = this.valuesDom;//记录value的控件名

            var setting = {
                view: {
                    selectedMulti: selectedMulti=="True"?selectedMulti:false //是否允许多选
                },
                data: {
                    simpleData: {
                        enable: true
                    }
                },
                callback: {
                    //zTree节点的点击事件
                    onClick: onClick
                }
            };


            if (selectedMulti == "True") {
                checkType = "checkbox";
             //   delete setting.callback.onClick;
                setting.callback.onCheck = onCheck;
            }
            if ("checkbox" == checkType || "radio" == checkType) {
                var check = {};
                check["enable"] = true;
                check["chkStyle"] = "checkbox";
                setting["check"] = check;
            }

            //Ztree的初始数据
          //  $(document).ready(function () {
                $.fn.zTree.init($("#" + treeId), setting, zNodes);
                initEvent();
                hideMenu();
                selectNode(selectnode);
           // });
            //点击某个节点 然后将该节点的名称赋值值文本框
                function onClick(e, treeId, treeNode,search) {
    
                    var zTree = $.fn.zTree.getZTreeObj(treeId);
                //获得选中的节点
                    var nodes = zTree.getSelectedNodes();
                var v = "";
                var ids = "";
                if (selectedMulti=="True") {
                    for (var i = 0, l = nodes.length; i < l; i++) {
                        zTree.checkNode(nodes[i], true, true);
                        zTree.setting.callback.onCheck();
                    }
                    return;
                }
                //根据id排序
                nodes.sort(function compare(a, b) { return a.id - b.id; });
                for (var i = 0, l = nodes.length; i < l; i++) {
                    if ((','+v+',').indexOf(','+nodes[i].name+',')<0) {
                        if (i !== 0) {
                            v += "," + nodes[i].name;
                            ids+= "," + nodes[i].id;
                        }
                        else {
                            v += nodes[i].name;
                            ids+= nodes[i].id;
                        }
                    }
     

                }
                //将选中节点的名称显示在文本框内
                var cityObj = txtObj;
                if (search!==true) {
                    $(cityObj).val(v);
                }
                $(valuesDom).val(ids);
                console.log(ids);
                //隐藏zTree
                hideMenu();
                return false;
            }

            //选中节点
             function onCheck(event, treeId, treeNode) {

                var zTree = $.fn.zTree.getZTreeObj(treeId);
                
                var isReturn = false;
                   //获得选中的节点
                var nodes = zTree.getCheckedNodes();

                var v = "";
                var ids = "";
                //根据id排序
                nodes.sort(function compare(a, b) { return a.id - b.id; });
                for (var i = 0, l = nodes.length; i < l; i++) {
                    if ((',' + v + ',').indexOf(',' + nodes[i].name + ',') < 0) {
                        if (i !== 0) {
                            v += "," + nodes[i].name;
                            ids += "," + nodes[i].id;
                        }
                        else {
                            v += nodes[i].name;
                            ids += nodes[i].id;
                        }
                    }


                }
                //将选中节点的名称显示在文本框内
                var cityObj = txtObj;
                cityObj.val(v);
                $(valuesDom).val(ids);
                console.log(ids);
                return false;
            }

            //显示树
            function showMenu() {
                var cityObj = txtObj;
                var cityOffset = txtObj.offset();
                //$("#menuContent").css({ left: cityOffset.left + "px", top: cityOffset.top + cityObj.outerHeight() + "px" }).slideDown("fast");
                $(menuContent).slideDown("fast");
            }

            //隐藏树
            function hideMenu() {
                $(menuContent).fadeOut("fast");
                //$("body").unbind("mousedown", onBodyDown);
            }

            //还原zTree的初始数据
            function InitialZtree() {
                $.fn.zTree.init($("#" + treeId), setting, zNodes);
            }


            function initEvent() {
                //鼠标获得焦点的时候，显示所有的树
                txtObj.on('focus',function () {
                    txtObj.css("background-color", "#FFFFCC");
                    showMenu();
                });

                txtObj.on("keyup paste", function () {
                    $(valuesDom).val("");
                    txtObj.val(txtObj.val().replace('，', ','));
                    if (txtObj.val().length > 0) {
                        var txtVal = txtObj.val();
                        InitialZtree();
                        var zTree = $.fn.zTree.getZTreeObj(treeId);
                        var nodeList = zTree.getNodesByParamFuzzy("name", txtVal.substr(txtVal.lastIndexOf(',') + 1, 100));
                        //将找到的nodelist节点更新至Ztree内
                        $.fn.zTree.init($("#" + treeId), setting, nodeList);
                        if (nodeList && nodeList.length > 0) {
                            var node3 = zTree.getNodeByParam("id", nodeList[0].id, null);

                        }
                        showMenu();
                    } else {
                        //隐藏树
                        //hideMenu();
                        InitialZtree();
                    }
                });

                //除ztree-select点击 隐藏
                $(document).click(function () {
                    hideMenu();
                });
                $(".ztree-select").click(function (event) {
                    event.stopPropagation();//阻止事件冒泡
                });
            }

            //选中节点
            function selectNode(selectNode) {
                if (!selectNode) {
                    return;
                }

                var arr = selectNode.toString().split(",");
                var txt = "";
                var zTree = $.fn.zTree.getZTreeObj(treeId);
                arr.sort(function compare(a, b) { return a - b; });
                for (var i = 0; i < arr.length; i++) {
                    var node3 = zTree.getNodeByParam("id", arr[i], null);
                    zTree.selectNode(node3);//选中
                    if (selectedMulti=="True") {
                        zTree.checkNode(node3, true, false);
                    }
                    for (var j = 0; j <zNodes.length; j++) {
                        if (zNodes[j].id==arr[i]) {
                            if (i!=0) {
                                txt +=',' + zNodes[j].name;
                            }
                            else {
                                txt += zNodes[j].name;
                            }
                            break;
                        }
                    }
                   
                }
                txtObj.val(txt);
                $(valuesDom).val(selectNode);
                return;
            }
           
        }

    });
})(jQuery);

