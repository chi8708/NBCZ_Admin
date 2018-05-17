
using NBCZ.BLL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NBCZ.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class RoleController : BaseAdminController
    {
        private Pub_RoleBLL roleBLL = new Pub_RoleBLL();
        private Pub_RoleFunctionBLL roleFunctionBLL = new Pub_RoleFunctionBLL();
        private Pub_FunctionBLL functionBLL = new Pub_FunctionBLL();

        FSRep rep = new FSRep()
        {
            errorNo = "0",
            results = new FSRepResult()
        };

       [Authorization(new string[] { RoleAuth.LIST, RoleAuth.ADD, RoleAuth.EDIT, RoleAuth.REMOVE, RoleAuth.AUTH })]
        // GET: /Admin/User/
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取角色分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPage(int pageNum = 1, int pageSize = 10, string field = "id", string order = " desc ")
        {
            var pageDatas = roleBLL.GetPage(GetWhereStr(), (field + " " + order), pageNum, pageSize);
            FSRep rep = pageDatas.MapTo<Pub_Role>();
            return Json(rep);
        }

        //分页搜索
        private string GetWhereStr()
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            sb.Append(" and StopFlag=0 ");
           
            var createDate = Request["createDate"];
            if (!string.IsNullOrEmpty(createDate))
            {
                var arr = createDate.Split('~');
                var fromDate = arr[0];
                var toDate = arr[1];
                sb.AppendFormat("and Lmdt>='{0}' and Lmdt<='{1} 23:59:59' ", fromDate, toDate);
            }

            sb.AppendFormat(" and {0} ", this.HttpContext.GetWhereStr());

            return sb.ToString();
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        [Authorization(new string[] { RoleAuth.ADD})]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Pub_Role model, FormCollection form)
        {
            model.RoleCode = roleBLL.GetCode();
            model.StopFlag = false;
            model.Lmdt = model.Crdt = DateTime.Now;
            model.Lmid = model.Crid = NBCZUser.UserCode + "-" + NBCZUser.UserName;

            var r = roleBLL.Insert(model);
            if (r != null)
            {
                rep.results.data = model.RoleCode;
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
        [Authorization(new string[] { RoleAuth.EDIT })]
        public ActionResult Edit(int id)
        {
            var model = roleBLL.Get(id);
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(Pub_Role model, FormCollection form)
        {
            model.Lmdt = DateTime.Now;
            model.Lmid = NBCZUser.UserCode + "-" + NBCZUser.UserName;
          
            var r = roleBLL.Update(model);
            if (r)
            {
              
                rep.results.data = model.Id;
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
        /// 批量删除
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        [Authorization(new string[] { RoleAuth.REMOVE })]
        public JsonResult DeleteBatch(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "未选择删除的记录！";
                return Json(rep);
            }
            var list = new List<Pub_Department>();
            var r = roleBLL.ChangeSotpStatus("Id in @Id", new { Id = id.Split(',') });

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
        [Authorization(new string[] { RoleAuth.REMOVE })]
        public JsonResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "未选择删除的记录！";
                return Json(rep);
            }
            var r = roleBLL.ChangeSotpStatus("Id=@Id", new { Id = id });

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
        /// 获取角色权限列表树数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoleFunctionTree(string roleCode)
        {

            rep.results.data = GetFunctionTreeFSTree();
            var userFunctions = GetRoleFunction(roleCode);
            ViewBag.selectNode = string.Join(",", userFunctions.Select(p => p.FunctionCode));

            return Json(rep, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 角色授权
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
       [Authorization(new string[] { RoleAuth.AUTH })]
        public ViewResult RoleFunctionEdit(string roleCode)
        {
            ViewBag.RoleCode = roleCode;
            ViewBag.selectNode = string.Join(",", GetRoleFunction(roleCode).Select(p => p.FunctionCode));
            return View();
        }

        [HttpPost]
        public ActionResult RoleFunctionEdit(FormCollection form)
        {
            var roleCode = form["RoleCode"];
            var r = roleFunctionBLL.DeleteByWhere("RoleCode='" + roleCode + "'");

            List<Pub_RoleFunction> roleFunction = new List<Pub_RoleFunction>();
            var functions = form["selectValues"].ToString().Split(',');
            foreach (var item in functions)
            {
                roleFunction.Add(new Pub_RoleFunction()
                {
                    RoleCode = roleCode,
                    FunctionCode = item
                });

            }
            roleFunctionBLL.InsertBatch(roleFunction);
            rep.errorInfo = "保存成功";

            return Json(rep);
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        private List<Pub_RoleFunction> GetRoleFunction(string roleCode)
        {
            var data = roleFunctionBLL.GetList("RoleCode='" + roleCode + "'");

            return data;
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
    }
}