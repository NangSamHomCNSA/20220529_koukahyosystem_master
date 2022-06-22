using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace koukahyosystem.Controllers
{
    public class OneOnOneKakuninHyouController : Controller
    {
        Models.OneOnOneKakuninHyou onokakunin = new Models.OneOnOneKakuninHyou();
        string cTAISHOSHA = "";
        string loginUser = "";
        // GET: OneOnOneKakuninHyou
        public ActionResult OneOnOneKakuninHyou()
        {
            if (Session["isAuthenticated"] != null)
            {
                //zeee
                if (Session["LoginName"] != null)
                {
                    loginUser = Session["LoginName"].ToString();
                }
                else
                {
                    return RedirectToRoute("Default", new { controller = "Default", action = "Login" });
                }
            }
            var readData = new DateController();
            onokakunin.YearList = readData.YearList("seichou");
            int curYeaVal = 0;//  readData.FindCurrentYearSeichou();
            if (Session["curr_nendou"] != null)
            {
                curYeaVal = int.Parse(Session["curr_nendou"].ToString());
            }
            else
            {
                curYeaVal = int.Parse(System.DateTime.Now.Year.ToString());
            }
            onokakunin.cur_year = curYeaVal.ToString();
            DataTable dt = dtOneList();
            onokakunin.OneonOneList = ReadDataList(dt);
            return View(onokakunin);
        }

        private List<Models.OneOnOneKakuninHyou> ReadDataList(DataTable dt_L)
        {
            List<Models.OneOnOneKakuninHyou> OneonOneList = new List<Models.OneOnOneKakuninHyou>();
            string shaName = "";
            foreach (DataRow dr_L in dt_L.Rows)
            {
                
                OneonOneList.Add(new Models.OneOnOneKakuninHyou
                {
                    cMenName = dr_L["先輩社員"].ToString(),
                    cShainName = dr_L["社員名"].ToString(),
                    cCount = dr_L["件"].ToString(),
                    rowC = Convert.ToInt32(dr_L["row"].ToString()),
                });
            }
            return OneonOneList;
        }
        public DataTable dtOneList()
        {
            DataTable dt_list = new DataTable();
            try
            {
                DataTable dt_perconver = new DataTable();

                string str_start = onokakunin.cur_year + "/4/1";
                DateTime startDate = DateTime.Parse(str_start);

                string str_end = startDate.AddYears(1).Year + "/3/" + DateTime.DaysInMonth(startDate.AddYears(1).Year, 04);
                DateTime endDate = DateTime.Parse(str_end);
                string sql_shain = "";
                sql_shain += "select cSHAIN,sSHAIN from m_shain;";
                var readdata = new SqlDataConnController();
                DataTable dtshain = readdata.ReadData(sql_shain);
                if (dtshain.Rows.Count > 0)
                {
                    dt_list.Columns.Add("先輩社員");
                    dt_list.Columns.Add("社員名");
                    dt_list.Columns.Add("件");
                    dt_list.Columns.Add("row");
                    foreach (DataRow dr in dtshain.Rows)
                    {
                        string cshain = dr["cSHAIN"].ToString();
                        string sshain = dr["sSHAIN"].ToString();
                        
                        string sqlStr = "";
                        sqlStr += "select ms.sSHAIN as SHAIN,ron.cTAISHOSHA,count(ron.cTAISHOSHA) as COUNT ";
                        sqlStr += "from r_oneonone ron ";
                        sqlStr += "inner join m_shain ms ";
                        sqlStr += "on ms.cSHAIN=ron.cTAISHOSHA ";
                        sqlStr += "where dHIDUKE BETWEEN '" + startDate.ToString("yyyy/MM/dd") + "' AND '" + endDate.ToString("yyyy/MM/dd") + "' ";
                        sqlStr += "and ron.cMENDANSHA='"+cshain+"' ";
                        sqlStr += "and ron.fKANRYOU ='1' ";
                        sqlStr += "and ron.fKAKUTEI='1' ";
                        sqlStr += "group by ron.cTAISHOSHA;";
                        dt_perconver = readdata.ReadData(sqlStr);
                        if (dt_perconver.Rows.Count > 0)
                        {
                            int row = dt_perconver.Rows.Count;
                            foreach(DataRow dr1 in dt_perconver.Rows)
                            {
                                DataRow infodr1 = dt_list.NewRow();
                                infodr1["先輩社員"] = sshain;
                                infodr1["社員名"] = dr1["SHAIN"].ToString();
                                infodr1["件"] = dr1["COUNT"].ToString();
                                infodr1["row"] = row;

                                dt_list.Rows.Add(infodr1);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return dt_list;
        }

        [HttpPost]
        public ActionResult OneOnOneKakuninHyou(Models.OneOnOneKakuninHyou one)
        {
            string name = "";
            string loginid = "";
            string selectyear = "";
            if (Session["LoginName"] == null)
            {
                return RedirectToRoute("Default", new { controller = "Default", action = "Login" });
            }
            if (Session["LoginName"] != null)
            {
                name = Session["LoginName"].ToString();
            }
            if (Session["LoginCode"] != null)
            {
                loginid = Session["LoginCode"].ToString();
            }
            if (Request["year_btn"] == "display")
            {
                if (one.cur_year != null)
                {
                    selectyear = one.cur_year;
                }
                ModelState.Clear();
            }
            else if (Request["year_btn"] != null)
            {
                if (one.cur_year != null)
                {

                    if (Request["year_btn"] == "<")
                    {
                        var readDate = new DateController();
                        selectyear = readDate.PreYear(one.cur_year);
                        one.cur_year = selectyear;
                    }
                    else if (Request["year_btn"] == ">")
                    {
                        var readDate = new DateController();
                        selectyear = readDate.NextYear(one.cur_year, "seichou");
                        one.cur_year = selectyear;
                    }
                }
                ModelState.Clear();
            }
            one.cur_year = selectyear;
            onokakunin = one;
            DataTable dt = dtOneList();
            one.OneonOneList = ReadDataList(dt);
            var readData = new DateController();
            one.YearList = readData.YearList("seichou");
            return View(one);
        }
    }
}