一、框架概述
-------------
1. NBCZ是一个通用权限系统，用vs2013+sqlserver2008开发工具。
2. 标准三层结构：
   1. Repository（DAL仓储层）使用DapperExtensions+Dapper开发。
   2. WEB主要使用MVC5.0+fsLayui+layUi开发。（已添加easyui文件，可自行切换）
--------  

二、配置使用
-------------------
1. 项目文件基本配置
    1. git clone项目，修改文件夹及sln、csproj、user文件名称为项目命名空间。

    2. 修改sln、csproj内容 将NBCZ修改为项目命名空间。

    3. 用vs打开项目，整个解决方案替换将NBCZ修改为项目命名空间。

    4. 删除web下的obj和bin 重新生成项目。不修改可能报错
路由集合中已存在名为“Admin_default”的路由。路由名称必须唯一。
   
    5. 若单个项目生成成功，但生成解决方案失败。解决方案属性→修改Web项目依赖。

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

四、参考文档
--------------------------
* Dapper：https://github.com/StackExchange/Dapper
* Dapper-Extensions：https://github.com/tmsmith/Dapper-Extensions
* layui：http://www.layui.com/doc/
* fslayui：http://www.itcto.cn/docs/fslayui#/
