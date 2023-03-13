一、框架概述 <a href="https://github.com/chi8708/NBCZ_Admin_NetCore">转至.net core+vue 前后端分离项目</a>
-------------
1. NBCZ是一个通用权限系统，用vs2017+sqlserver2012开发工具。
2. 标准三层结构：
   1. Repository（DAL仓储层）使用DapperExtensions+Dapper开发。
   2. WEB主要使用MVC5.0+fsLayui+layUi开发。（已添加easyui文件，可自行切换）
3. <a href="http://106.14.91.48:8918/" target="_blank" >在线预览</a> 登录名：admin 密码：123123
4. ![#f03c15](https://via.placeholder.com/15/f03c15/000000?text=+)
   *注：此版本不再维护，已升级至<a href="https://github.com/chi8708/CNet_Admin" >.net5前后端分离项目</a>* </sub> 
--------  

二、配置使用
-------------------
1. 项目文件基本配置
    1. git clone项目，修改文件夹及sln、csproj、user文件名称为项目命名空间。

    2. 修改sln、csproj内容 将NBCZ修改为项目命名空间。

    3. 用vs打开项目，整个解决方案替换将NBCZ修改为项目命名空间。

    4. 删除web下的obj和bin 重新生成项目。不修改可能报错
路由集合中已存在名为“Admin_default”的路由。路由名称必须唯一。
   
    5. 若单个项目生成成功，但生成解决方案失败。解决方案属性 → 修改Web项目依赖。

2. 数据库配置
    1. 还原数据库db→NBCZ
    2. 配置 DBUtility项目下的DbEntity.ttinclude
    3. 配置web项目下的web.config 数据库连接

3. 代码生成
    1. 按Model、DAL、BLL的顺序, 分别保存项目文件 T4.DapperExt→后缀为.tt文件。
----------

三、主界面如下：
---------------
![avatar](https://github.com/chi8708/NBCZ/blob/master/1529981048.jpg)

四、版本
---------------
<table>
<tr><th>语言/框架</th><th>地址</th><th>协议</th><th>备注</th></tr>
<tr>
	<td>.Net Framework</td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin" >.net framework+layui+dapper-extensions</a> </td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin/blob/master/LICENSE" target="_blank" >MIT</a></td>
	<td></td>	
</tr>
<tr>
	<td>.Net Framework + Vue </td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin_Vue" >.net framework+vue+dapper.contrib</a> </td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin_Vue/blob/master/LICENSE" target="_blank" >MIT</a></td>
	<td>前后端分离</td>	
</tr>
<tr>
	<td>.Net Core</td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin_NetCore" >.net core+iview+dapper.contrib</a> </td>
	<td><a href="https://github.com/chi8708/NBCZ_Admin_NetCore/blob/master/LICENSE" target="_blank" >MIT</a></td>
	<td>前后端分离</td>
</tr>
<tr>
	<td>.Net5</td>
	<td><a href="https://github.com/chi8708/CNet_Admin" >.net5+iview+dapper.contrib</a> </td>
	<td><a href="https://github.com/chi8708/CNet_Admin/blob/main/LICENSE" target="_blank" >MIT</a></td>
	<td>前后端分离</td>
</tr>
</table>

五、依赖/中间件
--------------------------
- Dapper：https://github.com/StackExchange/Dapper
- Dapper-Extensions：https://github.com/tmsmith/Dapper-Extensions
* layui：http://www.layui.com/doc/
* fslayui：http://www.itcto.cn/docs/fslayui#/


交流提升
-------------
QQ群:点击加群  <a href='https://jq.qq.com/?_wv=1027&k=4je1frWy' target="_blank" >851743573  </a>

给Aigu赞赏
-----------------
<img src="https://github.com/chi8708/NBCZ/blob/master/zs.jpg" />



