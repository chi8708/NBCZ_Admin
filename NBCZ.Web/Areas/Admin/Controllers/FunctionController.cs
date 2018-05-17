using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NBCZ.BLL;
using NBCZ.Model;
using Newtonsoft.Json;
namespace NBCZ.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class FunctionController : BaseAdminController
    {
        Pub_FunctionBLL functionBLL = new Pub_FunctionBLL();
        FSRep rep = new FSRep()
        {
            errorNo = "0",
            results = new FSRepResult()
        };

        // GET: /Admin/Department/
         [Authorization(new string[] { FunctionAuth.LIST, FunctionAuth.ADD, FunctionAuth.EDIT, FunctionAuth.REMOVE})]
        public ActionResult Index(string selectNode = "FC001")
        {
            ViewBag.selectNode = selectNode;
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        [Authorization(new string[] {FunctionAuth.ADD})]
        public ActionResult Add(string parentCode, string parentName)
        {
            ViewBag.parentName = parentName;
            ViewBag.parentCode = parentCode;
            return View();
        }

        [HttpPost]
        public ActionResult Add(Pub_Function model)
        {
            model.ParentCode = Request["ids"];
            model.FunctionCode = functionBLL.GetCode(model.ParentCode);
            model.StopFlag = false;
            model.editdate = DateTime.Now;
            model.editor = NBCZUser.UserCode + "-" + NBCZUser.UserName;
            if (!ModelState.IsValid)
            {
                rep.errorNo = "-1";
                rep.errorInfo = "数据未通过验证！";
                return Json(rep);
            }
            
            if (string.IsNullOrEmpty(model.ParentCode))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "父节点不能为空！";
                return Json(rep);
            }

            var r = functionBLL.Insert(model);
            if (!string.IsNullOrEmpty(r))
            {
                rep.results.data = model.ParentCode;
                rep.errorInfo = "添加成功";
            }
            else
            {
                rep.errorNo = "-1";
                rep.errorInfo = "添加失败";
            }
            return Json(rep);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
      [Authorization(new string[] { FunctionAuth.EDIT })]
        public ActionResult Edit(string FunctionCode)
        {
            var model = functionBLL.Get(FunctionCode);
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(Pub_Function model)
        {
            model.StopFlag = false;
            model.editdate = DateTime.Now;
            model.ParentCode = Request["ids"];
            model.editor = NBCZUser.UserCode + "-" + NBCZUser.UserName;
            if (model.FunctionCode == "FC001")
            {
                model.ParentCode = "";
            }
            else
            {
                if (string.IsNullOrEmpty(model.ParentCode))
                {
                    rep.errorNo = "-1";
                    rep.errorInfo = "父节点不能为空！";
                }
            }

            var r = functionBLL.Update(model);
            if (r)
            {
                rep.results.data = model.FunctionCode;
                rep.errorInfo = "修改成功";
            }
            else
            {
                rep.errorNo = "-1";
                rep.errorInfo = "修改失败";
            }
            return Json(rep);
        }


        /// <summary>
        ///  权限树下拉框
        /// </summary>
        /// <param name="selectNode">默认选中的节点code</param>
        /// <param name="isRequired">是否必填</param>
        /// <param name="isMul">多选</param>
        /// <returns></returns>
        public ActionResult FunctionTreeSelect(string selectNode, bool isRequired = true, bool isMul = false)
        {
            ViewBag.isRequired = isRequired;
            ViewBag.selectNode = selectNode;
            ViewBag.isMul = isMul;
            ViewBag.TreeData = JsonConvert.SerializeObject(GetFunctionTreeFSTree());
            return View();
        }

        /// <summary>
        /// 获取权限列表树数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFunctionTree()
        {
            string sqlstring = Request["where"];

            rep.results.data = GetFunctionTreeFSTree();

            return Json(rep, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取权限列表树数据
        /// </summary>
        /// <returns></returns>

        private List<FSTree> GetFunctionTreeFSTree()
        {
            var data = functionBLL.GetList(" StopFlag=0 ", " sortidx ASC,FunctionCode ASC ");
            List<FSTree> trees = new List<FSTree>();
            if (data == null || data.Count <= 0)
            {
                return trees;
            }

            foreach (var item in data)
            {
                FSTree tree = new FSTree();
                tree.id = item.FunctionCode;
                tree.isParent = string.IsNullOrEmpty(item.ParentCode) ? "true" : "false";
                tree.pId = item.ParentCode;
                tree.open = "true";
                tree.name = item.FunctionChina;
                trees.Add(tree);
            }

            return trees;

        }


        /// <summary>
        /// 获取子列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetChildList(string menuId)
        {

            rep.results.data = functionBLL.GetList(string.Format(" StopFlag=0 And FunctionCode IN (Select FunctionCode From f_SearchChildFunction('{0}'))", menuId), " sortidx ASC,FunctionCode ASC ");

            return Json(rep);
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="FunctionCode"></param>
        /// <returns></returns>
        [Authorization(new string[] { FunctionAuth.REMOVE })]
        public JsonResult DeleteBatch(string FunctionCode)
        {
            if (string.IsNullOrEmpty(FunctionCode))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "未选择删除的记录！";
                return Json(rep);
            }
            var list = new List<Pub_Function>();
            var r = functionBLL.ChangeSotpStatus("FunctionCode in @FunctionCode", new { FunctionCode = FunctionCode.Split(',') });

            if (r)
            {
                rep.errorInfo = "删除成功";
            }
            else
            {
                rep.errorNo = "-1";
                rep.errorInfo = "删除失败";
            }

            return Json(rep);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="FunctionCode"></param>
        /// <returns></returns>
       [Authorization(new string[] { FunctionAuth.REMOVE })]
        public JsonResult Delete(string FunctionCode)
        {
            if (string.IsNullOrEmpty(FunctionCode))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "未选择删除的记录！";
                return Json(rep);
            }
            var r = functionBLL.ChangeSotpStatus("FunctionCode=@FunctionCode", new { FunctionCode = FunctionCode });

            if (r)
            {
                rep.errorInfo = "删除成功";
            }
            else
            {
                rep.errorNo = "-1";
                rep.errorInfo = "删除失败";
            }

            return Json(rep);
        }
    }
}