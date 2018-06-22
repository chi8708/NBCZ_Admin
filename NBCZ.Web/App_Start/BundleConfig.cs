using System.Web;
using System.Web.Optimization;

namespace NBCZ.Web
{
    public class BundleConfig
    {
        // 有关绑定的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));

            //layui
            bundles.Add(new StyleBundle("~/fsLayui/css").Include(
                "~/fsLayui/plugins/pace/pace-theme-flash.css",
                "~/fsLayui/plugins/layui/css/layui.css",
                "~/fsLayui/plugins/contextMenu/jquery.contextMenu.min.css",
                "~/fsLayui/css/fs.css",
                "~/fsLayui/plugins/ztree/css/zTreeStyle/zTreeStyle.css",
                "~/fsLayui/plugins/font-awesome/css/font-awesome.min.css"
                ));
            bundles.Add(new StyleBundle("~/fsLayui/css/layui").Include(
               "~/fsLayui/plugins/layui/css/layui.css"
               ));

            bundles.Add(new ScriptBundle("~/fsLayui/js").Include(
                "~/fsLayui/plugins/pace/pace.min.js",
                "~/fsLayui/plugins/jquery/jquery.min.js",
                "~/fsLayui/plugins/contextMenu/jquery.contextMenu.min.js",
                "~/fsLayui/plugins/layui/layui.js",
                "~/fsLayui/plugins/frame/js/fsDict.js",
                "~/fsLayui/plugins/ztree/js/jquery.ztree.all.min.js",
                "~/fsLayui/plugins/frame/js/fs.js"
                ));

            bundles.Add(new ScriptBundle("~/fsLayui/js/layui").Include(
           "~/fsLayui/plugins/layui/layui.js"
   ));

            //easyui
            bundles.Add(new ScriptBundle("~/easyui/js").Include(
                "~/easyui-super-theme/easyui/jquery.min.js",
                "~/easyui-super-theme/easyui/jquery.easyui.min.js",
                "~/easyui-super-theme/easyui/locale/easyui-lang-zh_CN.js"
                ));
            bundles.Add(new StyleBundle("~/easyui/css").Include(
             "~/easyui-super-theme/easyui/themes/icons/css/font-awesome.min.css",
             "~/easyui-super-theme/easyui/themes/gray/easyui.css"
             ));

            bundles.Add(new ScriptBundle("~/easyui-super/js").Include(
               "~/easyui-super-theme/easyui/jquery.min.js",
               "~/easyui-super-theme/easyui/jquery.easyui.min.js",
               "~/easyui-super-theme/easyui/locale/easyui-lang-zh_CN.js",
               "~/easyui-super-theme/easyui-extensions/jeasyui.extensions.ty.js",
               "~/easyui-super-theme/js/super.js"
              ));
            bundles.Add(new StyleBundle("~/easyui-super/css").Include(
              "~/easyui-super-theme/easyui/themes/gray/easyui.css",
              "~/easyui-super-theme/css/superBlue.css",
              "~/easyui-super-theme/easyui/themes/icons/css/font-awesome.min.css"
            
             ));
        }
    }
}
