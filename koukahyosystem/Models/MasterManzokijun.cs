using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace koukahyosystem.Models
{
    public class MasterManzokijun
    {
        public IEnumerable<SelectListItem> jubanList { get; set; }
        public List<mz_kijun_list> Mz_kijun_List { get; set; }
        public List<mz_kj_copy_list> manzo_copy_list { get; set; }
        public string save_allow { get; set; }
        public string manzoqname { get; set; }
        public string newmanzoqname { get; set; }
        public string selectjuban { get; set; }
        public string manzo_searchname { get; set; }
        public IEnumerable<SelectListItem> yearList { get; set; }
        public IEnumerable<SelectListItem> m_copy_yearList { get; set; }
        public string sort { get; set; }
        public string headername { get; set; }
        public string qname { get; set; }
        public string Year { set; get; }
        public string main_Year { set; get; }
        public string m_copy_Year { set; get; }
        public string tensu_radio { get; set; }
    }
    public class mz_kijun_list
    {
        public string manzoq_code { get; set; }
        public string manzoq_name { get; set; }
        public string njubun { get; set; }
    }
    public class mz_kj_copy_list
    {
        public bool fcopy { get; set; }
        public string q_copy_code { get; set; }
        public string q_copy_name { get; set; }
    }
}