
using NBCZ.BLL;
using NBCZ.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NBCZ.Web.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : BaseAdminController
    {
        private Pub_UserBLL userBLL = new Pub_UserBLL();
        private V_PubUser_DeptBLL userDeptBLL = new V_PubUser_DeptBLL();
        private Pub_UserRoleBLL userRoleBLL = new Pub_UserRoleBLL();
        private Pub_RoleBLL roleBLL = new Pub_RoleBLL();
        private Pub_FunctionBLL functionBLL = new Pub_FunctionBLL();
        private Pub_UserFunctionBLL userFunctionBLL = new Pub_UserFunctionBLL();

        FSRep rep = new FSRep()
        {
            errorNo = "0",
            results = new FSRepResult()
        };

        // GET: /Admin/User/
        [Authorization(new string[] { UserInfoAuth.LIST, UserInfoAuth.ADD, UserInfoAuth.EDIT, UserInfoAuth.REMOVE, UserInfoAuth.AUTH })]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取用户分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetPage(int pageNum = 1, int pageSize = 10, string field = "id", string order = " desc ")
        {
            var users = userDeptBLL.GetPage(GetWhereStr(), (field + " " + order), pageNum, pageSize);

            var usersData = users.data;
            var roles = userRoleBLL.GetUserRoles(" UserCode in  @UserCode", new { RoleCode = usersData.Select(p => p.UserCode) });

            foreach (var item in usersData)
            {
                var itemRoles = roles.Where(p => p.UserCode == item.UserCode);
                item.RoleCodes = string.Join(",", itemRoles.Select(p => p.RoleCode));
                item.RoleNames = string.Join(",", itemRoles.Select(p => p.RoleName));
            }
            FSRep rep = users.MapTo<V_PubUser_Dept>();
            return Json(rep);
        }

        //分页搜索
        private string GetWhereStr()
        {
            StringBuilder sb = new StringBuilder(" 1=1 ");
            sb.Append(" and StopFlag=0 ");

            var deptCode = Request["ids"];
            if (!string.IsNullOrEmpty(deptCode))
            {
                sb.AppendFormat(" and DeptCode='{0}' ", deptCode);
            }
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
        [Authorization(new string[] { UserInfoAuth.ADD })]
        public ActionResult Add()
        {
            ViewBag.Roles = GetRolesSelect();
            return View();
        }

        [HttpPost]
        public ActionResult Add(Pub_User model, FormCollection form)
        {
            model.UserCode = userBLL.GetCode();
            model.StopFlag = false;
            model.Lmdt = model.Crdt = DateTime.Now;
            model.DeptCode = form["ids"];
            model.Lmid = model.Crid = NBCZUser.UserCode + "-" + NBCZUser.UserName;
            model.UserName = model.UserName.Trim();
            if (IsUserNameRepeat(model.UserName))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "用户名不能重名,请重新输入";
                return Json(rep);
            }
            var r = userBLL.Insert(model);
            if (r != null)
            {
                //插用户角色表
                var roles = form["Roles[]"].ToString().Split(',');
                AddUserRoles(model.UserCode, roles);

                rep.results.data = model.UserCode;
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
        /// 检查用户名是否重复
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private bool IsUserNameRepeat(string userName)
        {
            var list = userBLL.GetList(string.Format("UserName='{0}'", userName), limits: 1);
            return (list != null && list.Count >= 1);
        }

        //添加用户角色
        private void AddUserRoles(string userCode, string[] roles)
        {
            List<Pub_UserRole> userRoles = new List<Pub_UserRole>();
            foreach (var item in roles)
            {
                userRoles.Add(new Pub_UserRole { UserCode = userCode, RoleCode = item });
            }
            if (userRoles.Count > 0)
            {
                userRoleBLL.InsertBatch(userRoles);
            }
        }

        public IEnumerable<SelectListItem> GetRolesSelect(List<string> selectRoles = null)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var data = roleBLL.GetList(" StopFlag=0 ");
            foreach (var item in data)
            {
                SelectListItem model = new SelectListItem()
                {
                    Text = item.RoleName,
                    Value = item.RoleCode
                };
                if (selectRoles != null)
                {
                    model.Selected = selectRoles.Contains(item.RoleCode);
                }


                list.Add(model);
            }

            return list;

        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="parentCode"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        [Authorization(new string[] { UserInfoAuth.EDIT })]
        public ActionResult Edit(int id)
        {
            var model = userBLL.Get(id);
            var userRoles = userRoleBLL.GetList(string.Format("UserCode='{0}'", model.UserCode)).Select(p => p.RoleCode);
            ViewBag.Roles = GetRolesSelect(userRoles.ToList());
            ViewBag.UserRoleCodes = string.Join(",", userRoles);
            return View(model);
        }


        [HttpPost]
        public ActionResult Edit(Pub_User model, FormCollection form)
        {
            model.Lmdt = DateTime.Now;
            model.Lmid = NBCZUser.UserCode + "-" + NBCZUser.UserName;
            model.DeptCode = form["ids"];
            model.UserName = model.UserName.Trim();
            if (form["oldUserName"] != model.UserName)
            {
                if (IsUserNameRepeat(model.UserName))
                {
                    rep.errorNo = "-1";
                    rep.errorInfo = "用户名不能重名,请重新输入";
                    return Json(rep);
                }
            }
            var r = userBLL.Update(model);
            if (r)
            {
                //角色已修改
                if (form["UserRoleCodes"] != form["Roles1[]"])
                {
                    r = userRoleBLL.DeleteByWhere("UserCode='" + model.UserCode + "'");
                    //插用户角色表
                    var roles = form["Roles1[]"].ToString().Split(',');
                    AddUserRoles(model.UserCode, roles);

                }

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
        [Authorization(new string[] { UserInfoAuth.REMOVE })]
        public JsonResult DeleteBatch(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "未选择删除的记录！";
                return Json(rep);
            }
            var list = new List<Pub_Department>();
            var r = userBLL.ChangeSotpStatus("Id in @Id", new { Id = id.Split(',') });

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

        [Authorization(new string[] { UserInfoAuth.REMOVE })]
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="deptCode"></param>
        /// <returns></returns>
        public JsonResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                rep.errorNo = "-1";
                rep.errorInfo = "未选择删除的记录！";
                return Json(rep);
            }
            var r = userBLL.ChangeSotpStatus("Id=@Id", new { Id = id });

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
        /// 用户授权
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        [Authorization(new string[] { UserInfoAuth.AUTH })]
        public ViewResult UserFunctionEdit(string userCode)
        {
            ViewBag.UserCode = userCode;
            ViewBag.selectNode = string.Join(",", GetUserFunction(userCode).Select(p => p.FunctionCode));
            return View();
        }

        [HttpPost]
        public ActionResult UserFunctionEdit(FormCollection form)
        {
            var userCode = form["userCode"];
            var r = userFunctionBLL.DeleteByWhere("UserCode='" + userCode + "'");

            List<Pub_UserFunction> userFunction = new List<Pub_UserFunction>();
            var functions = form["selectValues"].ToString().Split(',');
            foreach (var item in functions)
            {
                userFunction.Add(new Pub_UserFunction()
                {
                    UserCode = userCode,
                    FunctionCode = item
                });

            }
            userFunctionBLL.InsertBatch(userFunction);
            rep.errorInfo = "保存成功";

            return Json(rep);
        }

        /// <summary>
        /// 获取用户权限列表树数据
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUserFunctionTree(string userCode)
        {

            rep.results.data = GetFunctionTreeFSTree();
            var userFunctions = GetUserFunction(userCode);
            ViewBag.selectNode = string.Join(",", userFunctions.Select(p => p.FunctionCode));

            return Json(rep, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户权限列表
        /// </summary>
        /// <param name="userCode"></param>
        /// <returns></returns>
        private List<Pub_UserFunction> GetUserFunction(string userCode)
        {
            var data = userFunctionBLL.GetList("UserCode='" + userCode + "'");

            return data;
        }


        /// <summary>
        /// 获取用户权限列表树数据
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
        /// 修改个人密码
        /// </summary>
        /// <returns></returns>
        public ActionResult EditPassWord() 
        {
            return View();
        }

        /// <summary>
        /// 修改个人密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPassWord(FormCollection form)
        {
            var oldPwd = form["OldPassWord"];
            var newPwd = form["PassWord"];
            var newPwd2 = form["PassWord2"];
            if (newPwd!=newPwd2)
            {
                rep.errorNo = "-1";
                rep.errorInfo = "两次输入密码不一致！";
                return Json(rep);
            }
            string strWhere=string.Format("UserCode='{0}' AND UserPwd='{1}'",NBCZUser.UserCode,oldPwd);
            var userCount= userBLL.GetList(strWhere, limits: 1).Count;
            if (userCount <= 0)
            {
                rep.errorNo = "-1";
                rep.errorInfo = "旧密码不正确！";
                return Json(rep);
            }
           var r=userBLL.EditPassWord(NBCZUser.UserCode, newPwd);
           if (r)
           {
               Session.Abandon();
               Session.Clear();
               Session.RemoveAll();
               Session["Admin"] = null;
               FormsAuthentication.SignOut();
              // return JavaScript(string.Format("window.location.href='../Login/Index'"));
               rep.errorInfo = "修改成功！";
           }
           else 
           {
               rep.errorNo = "-1";
               rep.errorInfo = "修改失败！";

           }
           return Json(rep);
        }
    }
}