using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRS.CUSTOMER.APPLICATION.Library
{
    public class ProjectGrid
    {
        public static IDictionary<string, string> column { get; set; }
        /// <summary>
        /// Make Grid/table 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">List Of object</param>
        /// <param name="ControlerName">Controller Name</param>
        /// <param name="Search">Search Value(not used)</param>
        /// <param name="Pagesize">page size(not used)</param>
        /// <param name="allowAdd">allow add button</param>
        /// <param name="DateField"></param>
        /// <param name="RowId"></param>
        /// <param name="breadcrumb1"></param>
        /// <param name="breadcrumb2"></param>
        /// <param name="breadcrumb2url"></param>
        /// <param name="addPageUrl">if allowAdd is true addPageUrl is visible and can be customized</param>
        /// <param name="className">Class name for table</param>
        /// <returns></returns>
        public static string MakeGrid<T>(List<T> obj, string ControlerName, string Search, int Pagesize, bool allowAdd = true
            , string DateField = "", string RowId = "", string breadcrumb1 = "", string breadcrumb2 = "", string breadcrumb2url = ""
            , string addPageUrl = "", string className = "datatable", string hideSN = "false", bool showtotal = false
            , string customButtonName = "", string customButtonId = "", string customButtonFavIcon = "", bool showCustomButton = false
            , bool allowFilter = false, bool addbackbutton = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"breadcrumb-line breadcrumb-line-light header-elements-md-inline\">");
            sb.AppendLine("<div class=\"card card-box\" >");
            if (ControlerName.ToLower() != "hidebreadcrumb")
            {
                string headerCss = string.Empty;
                if (className.ToUpper() == "DATATABLE")
                {
                    headerCss = "style=\"border:0px; margin-top: -20px;\"";
                }
                sb.AppendLine("<div class='card-header header-elements-inline '" + headerCss + ">");
                if (allowFilter)
                {
                    sb.AppendLine("<button class=\"btn btn-primary float-left\" id=\"searchToggle\" onClick=\"showFilter()\">Show Filters</button>");
                }
                else
                {
                    sb.AppendLine("<h5 class='card-title'>" + ControlerName + "</h5>");
                }
                if (allowAdd)
                {
                    //if (ApplicationUtilities.HasPageRight(addPageUrl))
                    //{
                    //    sb.AppendLine("<a href='" + (String.IsNullOrEmpty(addPageUrl) ? "#" : addPageUrl) + "' class=\"btn btn-primary float-right\"><i class=\"fas fa-plus mr-2\"></i> Add New</a>");
                    //}
                    sb.AppendLine("<a href='" + (String.IsNullOrEmpty(addPageUrl) ? "#" : addPageUrl) + "' class=\"btn btn-primary float-right\"><i class=\"fas fa-plus mr-2\"></i> Add New</a>");
                }
                else if (showCustomButton)
                {
                    sb.AppendLine("<a href='" + (String.IsNullOrEmpty(addPageUrl) ? "#" : addPageUrl) + "' class=\"btn btn-primary float-right\" id=\"" + customButtonId + "\" ><i class=\"" + customButtonFavIcon + "\"></i> " + customButtonName + " </a>");
                }
                sb.AppendLine("</div>");
            }
            sb.AppendLine("<div class=\"panel-body list-body table-responsive p-0\" style=\"font-size: 10px\">");
            sb.AppendLine("<table id = \"DataTableId\" class=\"table " + className + "\">");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            if (hideSN.ToLower() == "false")
            {
                sb.AppendLine("<th>S.N</th>");
            }
            foreach (var item in column)
            {
                sb.AppendLine("<th>" + item.Value + "</th>");
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            int SN = 0;

            foreach (var item in obj)
            {
                SN++;
                int num = 0;
                if (hideSN.ToLower() == "false")
                {
                    sb.AppendLine("<tr id=" + SN + ">");
                    sb.AppendLine("<td>" + SN + "</td>");
                }

                Type t = item.GetType();
                foreach (var col in column)
                {
                    num++;
                    var value = item.GetType().GetProperty(col.Key).GetValue(item, null);
                    if (!string.IsNullOrWhiteSpace(DateField) && DateField.Split('|').Contains(num.ToString()))
                    {
                        if (null != value)
                        {
                            //value = StaticData.DBToFrontDate(value.ToString());
                            value = (string.IsNullOrWhiteSpace(value.ToString()) ? "" : value.ToString().Substring(0, 10));
                        }
                    }
                    if (col.Key.Contains("IsActive") && null != value)
                    {
                        value = (value.ToString() == "True" ? "Yes" : (value.ToString() == "1" ? "Yes" : "No"));
                        //value = (value.ToString()=="1" ? "Yes" : "No");
                    }
                    //if (col.Key.Contains("CommissionValue") && null != value)
                    //{
                    //    
                    //    value += "$nbsp;<i class=\"fas fa-edit\" id=\"manage" + num + "\">";
                    //}
                    if (col.Value.ToLower().Contains("npr"))
                    {
                        sb.AppendLine("<td style=\"text-align:left;\">" + value + "</td>");
                    }
                    else
                    {
                        sb.AppendLine("<td>" + value + "</td>");
                    }
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");
            if (showtotal == true && SN != 0)
            {
                sb.AppendLine("<tfoot >");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th  style = \"text - align:left\" > Total:");
                sb.AppendLine("</th> ");
                //sb.AppendLine("<th></th>");
                foreach (var item in column)
                {
                    sb.AppendLine("<th style=\"text-align: left;\"></th>");
                }
                sb.AppendLine("</tr>");

                sb.AppendLine("</tfoot>");
            }
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
            if (addbackbutton)
            {
                sb.AppendLine(" <div class=\"row mt-1 mb-4 ml-2\">");
                sb.AppendLine("<button type=\"button\" class=\"btn btn-light btn-group-lg bg-primary\" onclick=\"window.history.back();\"><i class=\"fa fa-arrow-left mr-2\"></i> Back</button>");
                sb.AppendLine("</div>");
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }
        public static string Log_MakeGrid<T>(List<T> obj, string ControlerName, string Search, int Pagesize, bool allowAdd = true
        , string DateField = "", string RowId = "", string breadcrumb1 = "", string breadcrumb2 = "", string breadcrumb2url = ""
        , string addPageUrl = "", string className = "datatable", string hideSN = "false", bool showtotal = false
        , string customButtonName = "", string customButtonId = "", string customButtonFavIcon = "", bool showCustomButton = false
        , bool allowFilter = false, bool addbackbutton = false)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("<div class=\"breadcrumb-line breadcrumb-line-light header-elements-md-inline\">");

            //if (ControlerName.ToLower() != "hidebreadcrumb")
            //{
            //    sb.AppendLine("<div class=\"d-flex\">");
            //    sb.AppendLine("<div class=\"breadcrumb\">");
            //    sb.AppendLine("<a href=\"" + ApplicationUtilities.GenerateUrl("~/") + "\" class=\"breadcrumb-item\"><i class=\"icon-home2 mr-2\"></i>" + (breadcrumb1 == "" ? "Dashboard" : breadcrumb1) + "</a>");
            //    sb.AppendLine(breadcrumb2 != "" ? "<a href=\"" + (breadcrumb2url == "" ? "#" : breadcrumb2url) + "\" class=\"breadcrumb-item\">" + breadcrumb2 + "</a>" : "");
            //    sb.AppendLine("<span class=\"breadcrumb-item active\">" + ControlerName + "</span>");
            //    sb.AppendLine("</div>");

            //    sb.AppendLine("<a href=\"#\" class=\"header-elements-toggle text-default d-md-none\"><i class=\"icon-more\"></i></a>");
            //    sb.AppendLine("</div>");

            //    sb.AppendLine("<div class=\"header-elements d-none\">");
            //    sb.AppendLine("<div class=\"breadcrumb justify-content-center\">");
            //    //sb.AppendLine("<div class=\"breadcrumb-elements-item dropdown p-0\"><a href=\"#\" class=\"breadcrumb-elements-item\" onclick=\"GoBack();\">");
            //    //sb.AppendLine("<i class=\"icon-arrow-left52 mr-2\"></i>");
            //    //sb.AppendLine("Back");
            //    //sb.AppendLine("</a></div>");
            //    if (allowAdd && ApplicationUtilities.HasPageRight(addPageUrl.Contains("?") ? addPageUrl.Split('?')[0] : addPageUrl))
            //    {
            //        sb.AppendLine("<div class=\"breadcrumb-elements-item dropdown p-0\"><a href='" + (String.IsNullOrEmpty(addPageUrl) ? "#" : addPageUrl) + "' class=\"btn btn-primary\"><i class=\"icon-plus2 mr-2\"></i> Add New</a></div>");
            //    }
            //    //sb.AppendLine("<div class=\"breadcrumb-elements-item dropdown p-0\">");
            //    //	sb.AppendLine("<a href=\"#\" class=\"breadcrumb-elements-item dropdown-toggle\" data-toggle=\"dropdown\">");
            //    //		sb.AppendLine("<i class=\"icon-gear mr-2\"></i>");
            //    //		sb.AppendLine("Settings");
            //    //	sb.AppendLine("</a>");

            //    //	sb.AppendLine("<div class=\"dropdown-menu dropdown-menu-right\">");
            //    //		sb.AppendLine("<a href=\"#\" class=\"dropdown-item\"><i class=\"icon-user-lock\"></i> Account security</a>");
            //    //		sb.AppendLine("<a href=\"#\" class=\"dropdown-item\"><i class=\"icon-statistics\"></i> Analytics</a>");
            //    //		sb.AppendLine("<a href=\"#\" class=\"dropdown-item\"><i class=\"icon-accessibility\"></i> Accessibility</a>");
            //    //		sb.AppendLine("<div class=\"dropdown-divider\"></div>");
            //    //		sb.AppendLine("<a href=\"#\" class=\"dropdown-item\"><i class=\"icon-gear\"></i> All settings</a>");
            //    //	sb.AppendLine("</div>");
            //    //sb.AppendLine("</div>");
            //    sb.AppendLine("</div>");
            //    sb.AppendLine("</div>");
            //}
            //sb.AppendLine("</div><br /><br />");

            sb.AppendLine("<div class=\"card card-box\" >");
            if (ControlerName.ToLower() != "hidebreadcrumb")
            {
                string headerCss = string.Empty;
                if (className.ToUpper() == "DATATABLE")
                {
                    headerCss = "";
                }
                sb.AppendLine("<div class='card-header header-elements-inline '" + headerCss + ">");
                if (allowFilter)
                {
                    sb.AppendLine("<button class=\"btn btn-primary float-left\" id=\"searchToggle\" onClick=\"showFilter()\">Show Filters</button>");
                }
                else
                {
                    sb.AppendLine("<h5 class='card-title'>" + ControlerName + "</h5>");
                }
                if (allowAdd)
                {
                    if (ApplicationUtilities.HasPageRight(addPageUrl))
                    {
                        sb.AppendLine("<a href='" + (String.IsNullOrEmpty(addPageUrl) ? "#" : addPageUrl) + "' class=\"btn btn-primary float-right\"><i class=\"fas fa-plus mr-2\"></i> Add New</a>");
                    }
                }
                else if (showCustomButton)
                {
                    sb.AppendLine("<a href='" + (String.IsNullOrEmpty(addPageUrl) ? "#" : addPageUrl) + "' class=\"btn btn-primary float-right\" id=\"" + customButtonId + "\" ><i class=\"" + customButtonFavIcon + "\"></i> " + customButtonName + " </a>");
                }
                sb.AppendLine("</div>");
            }

            sb.AppendLine("<div class=\"panel-body list-body table-responsive p-0\" style=\"font-size: 10px\">");
            sb.AppendLine("<div class=\"table-log\">");
            sb.AppendLine("<table id = \"DataTableId\" class=\"table " + className + "\">");
            //sb.AppendLine("<div class=\"card-body table-responsive p-0\">");
            //sb.AppendLine("<table class=\"table table-hover text-nowrap table-striped " + className + "\">");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            if (hideSN.ToLower() == "false")
            {
                sb.AppendLine("<th>S.N</th>");
            }
            foreach (var item in column)
            {
                sb.AppendLine("<th>" + item.Value + "</th>");
                //sql += ", @" + item.Key + " = " + wDao.FilterString(item.Value);
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");

            int SN = 0;

            foreach (var item in obj)
            {
                SN++;
                int num = 0;
                if (hideSN.ToLower() == "false")
                {
                    sb.AppendLine("<tr id=" + SN + ">");
                    sb.AppendLine("<td>" + SN + "</td>");
                }

                Type t = item.GetType();
                foreach (var col in column)
                {
                    num++;
                    var value = item.GetType().GetProperty(col.Key).GetValue(item, null);
                    if (!string.IsNullOrWhiteSpace(DateField) && DateField.Split('|').Contains(num.ToString()))
                    {
                        if (null != value)
                        {
                            //value = StaticData.DBToFrontDate(value.ToString());
                            value = (string.IsNullOrWhiteSpace(value.ToString()) ? "" : value.ToString().Substring(0, 10));
                        }
                    }
                    if (col.Key.Contains("IsActive") && null != value)
                    {
                        value = (value.ToString() == "True" ? "Yes" : (value.ToString() == "1" ? "Yes" : "No"));
                        //value = (value.ToString()=="1" ? "Yes" : "No");
                    }
                    //if (col.Key.Contains("CommissionValue") && null != value)
                    //{
                    //    
                    //    value += "$nbsp;<i class=\"fas fa-edit\" id=\"manage" + num + "\">";
                    //}
                    if (col.Value.ToLower().Contains("npr"))
                    {
                        sb.AppendLine("<td style=\"text-align:left;\">" + value + "</td>");
                    }
                    else
                    {
                        sb.AppendLine("<td>" + value + "</td>");
                    }
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");
            if (showtotal == true && SN != 0)
            {
                sb.AppendLine("<tfoot>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th  style = \"text - align:right\" > Total:");
                sb.AppendLine("</th> ");
                //sb.AppendLine("<th></th>");
                foreach (var item in column)
                {
                    sb.AppendLine("<th style=\"text-align: left;\"></th>");
                }
                sb.AppendLine("</tr>");

                sb.AppendLine("</tfoot>");
            }
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            if (addbackbutton)
            {
                sb.AppendLine(" <div class=\"row mt-1 mb-4 ml-2\">");
                sb.AppendLine("<button type=\"button\" class=\"btn btn-light btn-group-lg bg-primary\" onclick=\"window.history.back();\"><i class=\"fa fa-arrow-left mr-2\"></i> Back</button>");
                sb.AppendLine("</div>");
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }

        public static string MakeGrid1<T>(List<T> obj, string _Value)
        {
            foreach (var item in obj)
            {
                Type t = item.GetType();
                foreach (var col in column)
                {
                    var a = item.GetType().GetProperty(col.Key).GetValue(item, null);

                }
            }



            return "";
        }

        public static string GetBreadCum(string breadcrumb = "Home|Management|Manage", string ButtonName = "Submit")
        {
            var label = breadcrumb.Split('|');
            string breadCum = "";
            breadCum += "<div class='page-header' id='stickHeader'>";
            breadCum += "<div class='row'>";
            breadCum += "<div class='col-sm-6'>";
            breadCum += "<ol class='breadcrumb'>";
            breadCum += "<li><a href='#'>" + label[0] + "</a></li>";
            breadCum += "<li><a href='#'>" + label[1] + "</a></li>";
            breadCum += "<li class='active'>" + label[2] + "</li>";
            breadCum += "</ol>";
            breadCum += "</div>";
            breadCum += "<div class='col-sm-6'>";
            breadCum += "<div class='form-group form-action'>";
            breadCum += "<a onclick='GoBack();' class='btn btn-default'><i class='mdi mdi mdi-arrow-left'></i> Back </a>";
            if (!string.IsNullOrWhiteSpace(ButtonName))
            {
                breadCum += "<button type='submit' class='btn btn-success'><i class='mdi mdi-check-circle-outline'></i> " + ButtonName + "</button>";
            }
            breadCum += "</div>";
            breadCum += "</div>";
            breadCum += "</div>";
            breadCum += "</div>";
            return breadCum;

        }
        public static string Project_MakeGrid<T>(List<T> obj, string ControlerName, string Search, int Pagesize, bool allowAdd = true
        , string DateField = "", string RowId = "", string breadcrumb1 = "", string breadcrumb2 = "", string breadcrumb2url = ""
        , string addPageUrl = "", string className = "datatable", string hideSN = "false", bool showtotal = false
        , string customButtonName = "", string customButtonId = "", string customButtonFavIcon = "", bool showCustomButton = false
        , bool allowFilter = false, bool addbackbutton = false)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div class=\"breadcrumb-line breadcrumb-line-light header-elements-md-inline\">");
            sb.AppendLine("<div class=\"card card-box\" >");
            if (ControlerName.ToLower() != "hidebreadcrumb")
            {
                string headerCss = string.Empty;
                if (className.ToUpper() == "DATATABLE")
                {
                    headerCss = "style=\"border:0px; margin-top: -20px;\"";
                }
                sb.AppendLine("<div class='card-header header-elements-inline '" + headerCss + ">");
                if (allowFilter)
                {
                    sb.AppendLine("<button class=\"btn btn-primary float-left\" id=\"searchToggle\" onClick=\"showFilter()\">Show Filters</button>");
                }
                else
                {
                    sb.AppendLine("<h5 class='card-title'>" + ControlerName + "</h5>");
                }
                if (allowAdd)
                {
                    if (ApplicationUtilities.HasPageRight(addPageUrl))
                    {
                        sb.AppendLine("<a href='" + (String.IsNullOrEmpty(addPageUrl) ? "#" : addPageUrl) + "' class=\"btn btn-primary float-right\"><i class=\"fas fa-plus mr-2\"></i> Add New</a>");
                    }
                }
                else if (showCustomButton)
                {
                    sb.AppendLine("<a href='" + (String.IsNullOrEmpty(addPageUrl) ? "#" : addPageUrl) + "' class=\"btn btn-primary float-right\" id=\"" + customButtonId + "\" ><i class=\"" + customButtonFavIcon + "\"></i> " + customButtonName + " </a>");
                }
                sb.AppendLine("</div>");
            }
            sb.AppendLine("<div class=\"panel-body list-body table-responsive p-0\" style=\"font-size: 10px\">");
            sb.AppendLine("<div class=\"table-log\">");
            sb.AppendLine("<table id =\"DataTableId\" class=\"table " + className + "\">");
            sb.AppendLine("<thead>");
            sb.AppendLine("<tr>");
            if (hideSN.ToLower() == "false")
            {
                sb.AppendLine("<th>S.N</th>");
            }
            foreach (var item in column)
            {
                sb.AppendLine("<th>" + item.Value + "</th>");
            }
            sb.AppendLine("</tr>");
            sb.AppendLine("</thead>");
            sb.AppendLine("<tbody>");
            int SN = 0;
            foreach (var item in obj)
            {
                SN++;
                int num = 0;
                if (hideSN.ToLower() == "false")
                {
                    sb.AppendLine("<tr id=" + SN + ">");
                    sb.AppendLine("<td>" + SN + "</td>");
                }
                Type t = item.GetType();
                foreach (var col in column)
                {
                    num++;
                    var value = item.GetType().GetProperty(col.Key).GetValue(item, null);
                    if (!string.IsNullOrWhiteSpace(DateField) && DateField.Split('|').Contains(num.ToString()))
                    {
                        if (null != value)
                        {
                            value = (string.IsNullOrWhiteSpace(value.ToString()) ? "" : value.ToString().Substring(0, 10));
                        }
                    }
                    if (col.Key.Contains("IsActive") && null != value)
                    {
                        value = (value.ToString() == "True" ? "Yes" : (value.ToString() == "1" ? "Yes" : "No"));
                        //value = (value.ToString()=="1" ? "Yes" : "No");
                    }
                    if (col.Value.ToLower().Contains("npr"))
                    {
                        sb.AppendLine("<td style=\"text-align:left;\">" + value + "</td>");
                    }
                    else
                    {
                        sb.AppendLine("<td>" + value + "</td>");
                    }
                }
                sb.AppendLine("</tr>");
            }
            sb.AppendLine("</tbody>");
            if (showtotal == true)
            {
                sb.AppendLine("<tfoot>");
                sb.AppendLine("<tr>");
                sb.AppendLine("<th  style = \"text-align:left\" > Total:");
                sb.AppendLine("</th> ");
                foreach (var item in column)
                {

                    sb.AppendLine("<th style=\"text-align: left;\"></th>");
                }
                sb.AppendLine("</tr>");
                sb.AppendLine("</tfoot>");
            }
            sb.AppendLine("</table>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            if (addbackbutton)
            {
                sb.AppendLine(" <div class=\"row mt-1 mb-4 ml-2\">");
                sb.AppendLine("<button type=\"button\" class=\"btn btn-light btn-group-lg bg-primary\" onclick=\"window.history.back();\"><i class=\"fa fa-arrow-left mr-2\"></i> Back</button>");
                sb.AppendLine("</div>");
            }
            sb.AppendLine("</div>");
            return sb.ToString();
        }
    }
}