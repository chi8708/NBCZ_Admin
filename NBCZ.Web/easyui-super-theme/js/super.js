/*easyui样式初始化*/
$.fn.tabs.defaults.tabHeight = 32; //tab标签条高度
$.fn.linkbutton.defaults.height = 32; //按钮高度
$.fn.menu.defaults.itemHeight = 28; //menu高度

$.fn.validatebox.defaults.height = 32;
$.fn.textbox.defaults.height = 32;
$.fn.textbox.defaults.iconWidth = 24;
$.fn.datebox.defaults.height = 32;
$.fn.numberspinner.defaults.height = 32;
$.fn.timespinner.defaults.height = 32;
$.fn.numberbox.defaults.height = 32;
$.fn.combobox.defaults.height = 32;
$.fn.passwordbox.defaults.height = 32;
$.fn.filebox.defaults.height = 32;

$.fn.menu.defaults.noline = true
$.fn.progressbar.defaults.height = 18; //进度条

$(function() {
	/*左侧导航分类选中样式*/
	$(".panel-body .accordion-body>ul>li").on('click', function() {
		$(this).siblings().removeClass('super-accordion-selected');
		$(this).addClass('super-accordion-selected');

		//新增一个选项卡
		var tabUrl = $(this).data('url');
		var tabTitle = $(this).text();
		//tab是否存在
		//if($("#tt").tabs('exists', tabTitle)) {
		//	$("#tt").tabs('select', tabTitle);
		//} else {
		//	$('#tt').tabs('add', {
		//		title: tabTitle,
		//		href: tabUrl,
		//		closable: true
		//	});
		//}
	});

	/*设置按钮的下拉菜单*/
	$('.super-setting-icon').on('click', function() {
		$('#mm2').menu('show', {
			top: 50,
			left: document.body.scrollWidth - 130
		});
	});

	/*修改主题*/
	$('#themeSetting').on('click', function() {
		var themeWin = $('#win').dialog({
			width: 460,
			height: 260,
			modal: true,
			title: '主题设置',
			buttons: [{
				text: '保存',
				id: 'btn-sure',
				handler: function() {
					themeWin.panel('close');
				}
			}, {
				text: '关闭',
				handler: function() {
					themeWin.panel('close');
				}
			}],
			onOpen: function() {
				$(".themeItem").show();
			}
		});
	});
	$(".themeItem ul li").on('click', function() {
		$(".themeItem ul li").removeClass('themeActive');
		$(this).addClass('themeActive');
	});

	/*退出系统*/
	$("#logout").on('click', function() {
		$.messager.confirm('提示', '确定退出系统？', function(r) {
			if(r) {
				console.log('确定退出')
			}
		});
	});
});
$.parser.onComplete = function() {
	$("#index").css('opacity', 1);
}

/**
 * 初始化示例
 */
function initDemo() {
	/*初始化示例div*/
	var demoPanelId = 'demoPanel' + (new Date()).getTime();
	$('#demoPanel').attr('id', demoPanelId);
	var demoPaneCodeId = 'demoPanelCode' + (new Date()).getTime();
	$('#demoPanelCode').attr('id', demoPaneCodeId);

	/*示例导航选中样式*/
	$(".demo-list>ul>li").on('click', function() {
		$('#et-demo').tabs('select', '预览');

		$(this).siblings().removeClass('super-accordion-selected');
		$(this).addClass('super-accordion-selected');
		//加载页面
		$('#' + demoPanelId).panel('open').panel('refresh', $(this).data('url'));
	});
}