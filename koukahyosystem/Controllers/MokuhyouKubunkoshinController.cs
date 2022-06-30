using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace koukahyosystem.Controllers
{
    public class MokuhyouKubunkoshinController : Controller
    {
        // GET: MokuhyouKubunkoshin
        public ActionResult MokuhyouKubunkoshin()
        {
            var mysqlcontroller = new SqlDataConnController();
            string s_shainquery = "";
            string u_kubunquery = "";
            s_shainquery += "select mkou.cSHAIN,ms.cKUBUN from m_koukatema mkou ";
            s_shainquery += "left join m_shain ms on ms.cSHAIN=mkou.cSHAIN ";
            s_shainquery += "order by mkou.cSHAIN ";
            DataTable dt_select = new DataTable();
            dt_select = mysqlcontroller.ReadData(s_shainquery);
            foreach(DataRow dr in dt_select.Rows)
            {
                string shain = dr["cSHAIN"].ToString();
                string kubun = dr["cKUBUN"].ToString();
                u_kubunquery += "update m_koukatema set cKUBUN='" + kubun + "' where cSHAIN='" + shain + "'; ";
            }
            if (u_kubunquery != "")
            {
                var insertdata = new SqlDataConnController();
                Boolean f_update = insertdata.inputsql(u_kubunquery);
            }

            #region r_jishitasuku
            string s_shainjishi = "";
            string u_shainjishi = "";
            s_shainjishi += "select rjishi.cSHAIN,ms.cKUBUN from r_jishitasuku rjishi ";
            s_shainjishi += "left join m_shain ms on ms.cSHAIN=rjishi.cSHAIN ";
            s_shainjishi += "order by rjishi.cSHAIN ";
            DataTable dt_jishi = new DataTable();
            dt_jishi = mysqlcontroller.ReadData(s_shainjishi);
            foreach(DataRow dr in dt_jishi.Rows)
            {
                string shain = dr["cSHAIN"].ToString();
                string kubun = dr["cKUBUN"].ToString();
                u_shainjishi += "update r_jishitasuku set cKUBUN='" + kubun + "' where cSHAIN='" + shain + "'; ";
            }
            if (u_shainjishi != "")
            {
                var insertdata = new SqlDataConnController();
                Boolean f_update = insertdata.inputsql(u_shainjishi);
            }
            #endregion
            return View();
        }
    }
}