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
    public class DepartmentController : BaseAdminController
    {
        Pub_DepartmentBLL deptBLL = new Pub_DepartmentBLL();
        FSRep rep = new FSRep()
        {
            errorNo = "0",
            results = new FSRepResult()
        };

        // GET: /Admin/Department/
       [Authorization(new string[] { DepartmentAuth.LIST, DepartmentAuth.ADD, DepartmentAuth.EDIT, DepartmentAuth.REMOVE})]
        public ActionResult Index(string selectNode="D000001")
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
       [Authorization(new string[] { DepartmentAuth.ADD })]
        public ActionResult Add(string parentCode,string parentName)
        {
            ViewBag.parentName = parentName;
            ViewBag.parentCode = parentCode;
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Pub_Department model)
        {
            model.DeptCode = deptBLL.GetCode();
            model.StopFlag = false;
            model.Lmdt = DateTime.Now;
            model.ParentCode = Request["ids"];
            model.Lmid = NBCZUser.UserCode + "-" + NBCZUser.UserName;
         
            if (string.IsNullOrEmpty(model.ParentCode))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "父节点不能为空！";
            }

            var r = deptBLL.Insert(model);
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
        [Authorization(new string[] { DepartmentAuth.EDIT })]
        [ValidateInput(false)]
        public ActionResult Edit(string DeptCode)
        {
            var model= deptBLL.Get(DeptCode);
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(Pub_Department model)
        {
            model.StopFlag = false;
            model.Lmdt = DateTime.Now;
            model.ParentCode = Request["ids"];
            model.Lmid = NBCZUser.UserCode + "-" + NBCZUser.UserName;
            if (model.DeptCode=="D000001")
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

            var r = deptBLL.Update(model);
            if (r)
            {
                rep.results.data = model.DeptCode;
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
        ///  部门树下拉框
        /// </summary>
        /// <param name="selectNode">默认选中的节点code</param>
        /// <param name="isRequired">是否必填</param>
        /// <param name="isMul">多选</param>
        /// <returns></returns>
        public ActionResult DeptTreeSelect(string selectNode,bool isRequired=true,bool isMul=false) 
        {
            ViewBag.isRequired = isRequired;
            ViewBag.selectNode = selectNode;
            ViewBag.isMul = isMul;
            ViewBag.DeptTreeData =JsonConvert.SerializeObject(GetDeptTreeFSTree());
            return View();
        }

        /// <summary>
        /// 获取部门列表树数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetDeptTree() 
        {
            string sqlstring = Request["where"];

            rep.results.data = GetDeptTreeFSTree();

            return Json(rep,JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取部门列表树数据
        /// </summary>
        /// <returns></returns>

        private List<FSTree> GetDeptTreeFSTree()
        {
           var data= deptBLL.GetList(" StopFlag=0 ");
           List<FSTree> trees = new List<FSTree>();
           if (data==null||data.Count<=0)
           {
               return trees;
           }

           foreach (var item in data)
           {
               FSTree tree = new FSTree();
               tree.id = item.DeptCode;
               tree.isParent = string.IsNullOrEmpty(item.ParentCode) ? "true" : "false";
               tree.pId = item.ParentCode;
               tree.open = "true";
               tree.name = item.DeptName;
               trees.Add(tree);
           }

           return trees;

        }


        /// <summary>
        /// 获取子部门列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetChildList(string menuId)
        {

            rep.results.data = deptBLL.GetList(string.Format(" StopFlag=0 And DeptCode IN (Select DeptCode From f_SearchChildDept('{0}'))", menuId)," DeptCode ");

            return Json(rep);
        }


        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        [Authorization(new string[] { DepartmentAuth.REMOVE })]
        public JsonResult DeleteBatch(string deptCode) 
        {
            if (string.IsNullOrEmpty(deptCode))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "未选择删除的记录！";
                return Json(rep);
            }
            var list = new List<Pub_Department>();
            var r = deptBLL.ChangeSotpStatus("DeptCode in @DeptCode", new { DeptCode = deptCode.Split(',') });

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
        /// <param name="deptCode"></param>
        /// <returns></returns>
        [Authorization(new string[] { DepartmentAuth.REMOVE })]
        public JsonResult Delete(string deptCode)
        {
            if (string.IsNullOrEmpty(deptCode))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "未选择删除的记录！";
                return Json(rep);
            }
            var r = deptBLL.ChangeSotpStatus("DeptCode=@DeptCode", new { DeptCode = deptCode });

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