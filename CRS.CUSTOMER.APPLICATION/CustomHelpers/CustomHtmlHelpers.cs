using CRS.CUSTOMER.APPLICATION.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace CRS.CUSTOMER.APPLICATION.CustomHelpers
{
    public static class CustomHtmlHelpers
    {
        public static MvcHtmlString NTextBoxForSingleRow<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlattr, int col = 2, string helptext = null, string inputicon = null, bool disable = false)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName;
            var required = metadata.IsRequired;

            string coltab = (12 / col).ToString();
            var borderClass = htmlHelper.NAddClassIfPropertyInError(expression, "border-danger");
            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass("form-group row");

            TagBuilder labletag = new TagBuilder("label");
            string labeltext = resolvedLabelText + (required == true ? " <span class='text-danger'>*</span>" : "") + (string.IsNullOrEmpty(helptext) == true ? "" : " <small><i>" + helptext + "</small></i>");
            labletag.InnerHtml = labeltext;
            labletag.AddCssClass(borderClass.Replace("border", "text"));
            labletag.AddCssClass(string.Format("col-sm-2 col-form-label", coltab));
            outerdiv.InnerHtml = labletag.ToString();

            TagBuilder inputBoxdiv = new TagBuilder("div");
            inputBoxdiv.AddCssClass($"col-sm-{coltab}");

            //TagBuilder validationBoxdiv = new TagBuilder("div");
            //validationBoxdiv.AddCssClass("invalid-feedback");



            if (!string.IsNullOrEmpty(inputicon))
            {
                TagBuilder icondiv1 = new TagBuilder("div");
                icondiv1.AddCssClass("input-group-prepend");

                TagBuilder icondiv2 = new TagBuilder("div");
                icondiv2.AddCssClass("input-group-text");

                TagBuilder icondiv3 = new TagBuilder("i");
                icondiv3.AddCssClass(inputicon);

                icondiv2.InnerHtml = icondiv3.ToString();
                icondiv1.InnerHtml = icondiv2.ToString();
                inputBoxdiv.InnerHtml = icondiv1.ToString();
            }
            var routeValueDictionary = new System.Web.Routing.RouteValueDictionary(htmlattr);
            //if (routeValueDictionary.Keys.Where(x => x.ToLower() == "hidden") != null)
            //{
            //    outerdiv.AddCssClass("d-none");
            //}

            if (routeValueDictionary.Keys.Where(x => x.ToLower() == "class") != null && routeValueDictionary.Keys.Where(x => x.ToLower() == "class").Count() > 0)
            {
                var @class = routeValueDictionary.Keys.Where(x => x.ToLower() == "class").FirstOrDefault();
                if (!string.IsNullOrEmpty(borderClass))
                {
                    routeValueDictionary[@class] = routeValueDictionary[@class].ToString().Replace(" " + borderClass, "");
                    routeValueDictionary[@class] += " " + borderClass;
                    if (disable)
                    {
                        routeValueDictionary[@class] += " disabled";//("disabled", "disabled");
                    }
                }

            }
            else
            {
                routeValueDictionary.Add("class", " " + borderClass);
                if (disable)
                {
                    routeValueDictionary.Add("class", " disabled");//[@class] += " disabled";//("disabled", "disabled");
                }
            }

            if (disable)
            {
                routeValueDictionary.Add("disabled", "disabled");
            }
            inputBoxdiv.InnerHtml += htmlHelper.TextBoxFor(expression, routeValueDictionary).ToString();
            //validationBoxdiv.InnerHtml = htmlHelper.ValidationMessageFor(expression, null, new { @class = "form-text text-danger" }).ToString();
            inputBoxdiv.InnerHtml += htmlHelper.ValidationMessageFor(expression, null, new { @class = "form-text text-danger" }).ToString(); ;
            outerdiv.InnerHtml += inputBoxdiv.ToString();

            //outerdiv.InnerHtml = middlediv.ToString();
            return new MvcHtmlString(outerdiv.ToString());
        }
        public static MvcHtmlString NTextBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlattr, int col = 2, string helptext = null, string inputicon = null, bool disable = false, bool showLabel = true, string displayName = "")
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string resolvedLabelText = !string.IsNullOrEmpty(displayName) ? displayName : metadata.DisplayName ?? metadata.PropertyName;
            var required = metadata.IsRequired;

            string coltab = (12 / col).ToString();
            var borderClass = htmlHelper.NAddClassIfPropertyInError(expression, "border-danger");
            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass(string.Format("col-lg-{0}", coltab));

            TagBuilder middlediv = new TagBuilder("div");
            middlediv.AddCssClass("form-group");
            middlediv.AddCssClass(name + "divClass");

            if (showLabel)
            {
                TagBuilder labletag = new TagBuilder("label");
                string labeltext = resolvedLabelText + (required == true ? " <span class='text-danger'>*</span>" : "") + (string.IsNullOrEmpty(helptext) == true ? "" : " <small><i>" + helptext + "</small></i>");
                labletag.InnerHtml = labeltext;
                labletag.AddCssClass(borderClass.Replace("border-danger", "text"));
                middlediv.InnerHtml = labletag.ToString();
            }

            TagBuilder inputBoxdiv = new TagBuilder("div");
            inputBoxdiv.AddCssClass("input-group");

            TagBuilder validationBoxdiv = new TagBuilder("form-control-feedback");
            validationBoxdiv.AddCssClass("form-text text-danger");



            if (!string.IsNullOrEmpty(inputicon))
            {
                TagBuilder icondiv1 = new TagBuilder("div");
                icondiv1.AddCssClass("input-group-prepend");

                TagBuilder icondiv2 = new TagBuilder("div");
                icondiv2.AddCssClass("input-group-text");

                TagBuilder icondiv3 = new TagBuilder("i");
                icondiv3.AddCssClass(inputicon);

                icondiv2.InnerHtml = icondiv3.ToString();
                icondiv1.InnerHtml = icondiv2.ToString();
                inputBoxdiv.InnerHtml = icondiv1.ToString();
            }
            var routeValueDictionary = new System.Web.Routing.RouteValueDictionary(htmlattr);
            //if (routeValueDictionary.Keys.Where(x => x.ToLower() == "hidden") != null)
            //{
            //    outerdiv.AddCssClass("d-none");
            //}

            if (routeValueDictionary.Keys.Where(x => x.ToLower() == "class") != null && routeValueDictionary.Keys.Where(x => x.ToLower() == "class").Count() > 0)
            {
                var @class = routeValueDictionary.Keys.Where(x => x.ToLower() == "class").FirstOrDefault();
                if (!string.IsNullOrEmpty(borderClass))
                {
                    routeValueDictionary[@class] = routeValueDictionary[@class].ToString().Replace(" " + borderClass, "");
                    routeValueDictionary[@class] += " " + borderClass;
                    if (disable)
                    {
                        routeValueDictionary[@class] += " disabled";//("disabled", "disabled");
                    }
                }

            }
            else
            {
                routeValueDictionary.Add("class", " " + borderClass);
                if (disable)
                {
                    routeValueDictionary.Add("class", " disabled");//[@class] += " disabled";//("disabled", "disabled");
                }
            }

            if (disable)
            {
                routeValueDictionary.Add("disabled", "disabled");
            }
            inputBoxdiv.InnerHtml += htmlHelper.TextBoxFor(expression, routeValueDictionary).ToString();
            validationBoxdiv.InnerHtml = htmlHelper.ValidationMessageFor(expression, null, new { @class = "form-text text-danger" })?.ToString();
            middlediv.InnerHtml += inputBoxdiv.ToString();
            middlediv.InnerHtml += validationBoxdiv.ToString();

            outerdiv.InnerHtml = middlediv.ToString();
            return new MvcHtmlString(outerdiv.ToString());
        }
        public static MvcHtmlString NTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlattr, int col = 2, string helptext = null, string inputicon = null, bool showLabel = true, bool disable = false)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName;
            var required = metadata.IsRequired;

            string coltab = (12 / col).ToString();
            var borderClass = htmlHelper.NAddClassIfPropertyInError(expression, "border-danger");
            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass(string.Format("col-lg-{0}", coltab));

            TagBuilder middlediv = new TagBuilder("div");
            middlediv.AddCssClass("form-group");

            TagBuilder labletag = new TagBuilder("label");
            string labeltext = resolvedLabelText + (required == true ? " <span class='text-danger'>*</span>" : "") + (string.IsNullOrEmpty(helptext) == true ? "" : " <small><i>" + helptext + "</small></i>");
            labletag.InnerHtml = labeltext;
            labletag.AddCssClass(borderClass.Replace("border", "text"));

            TagBuilder inputBoxdiv = new TagBuilder("div");
            inputBoxdiv.AddCssClass("input-group");

            TagBuilder validationBoxdiv = new TagBuilder("form-control-feedback");
            validationBoxdiv.AddCssClass("form-text text-danger");



            middlediv.InnerHtml = showLabel ? labletag.ToString() : "";
            if (!string.IsNullOrEmpty(inputicon))
            {
                TagBuilder icondiv1 = new TagBuilder("div");
                icondiv1.AddCssClass("input-group-prepend");

                TagBuilder icondiv2 = new TagBuilder("div");
                icondiv2.AddCssClass("input-group-text");

                TagBuilder icondiv3 = new TagBuilder("i");
                icondiv3.AddCssClass(inputicon);

                icondiv2.InnerHtml = icondiv3.ToString();
                icondiv1.InnerHtml = icondiv2.ToString();
                inputBoxdiv.InnerHtml = icondiv1.ToString();
            }
            var routeValueDictionary = new System.Web.Routing.RouteValueDictionary(htmlattr);

            if (routeValueDictionary.Keys.Where(x => x.ToLower() == "class") != null && routeValueDictionary.Keys.Where(x => x.ToLower() == "class").Count() > 0)
            {
                var @class = routeValueDictionary.Keys.Where(x => x.ToLower() == "class").FirstOrDefault();
                if (!string.IsNullOrEmpty(borderClass))
                {
                    routeValueDictionary[@class] = routeValueDictionary[@class].ToString().Replace(" " + borderClass, "");
                    routeValueDictionary[@class] += " " + borderClass;
                }

            }
            else
            {
                routeValueDictionary.Add("class", " " + borderClass);
            }
            if (disable)
            {
                routeValueDictionary.Add("disabled", "disabled");
            }
            inputBoxdiv.InnerHtml += htmlHelper.TextAreaFor(expression, routeValueDictionary).ToString();
            validationBoxdiv.InnerHtml = htmlHelper.ValidationMessageFor(expression, null, new { @class = "form-text text-danger" })?.ToString();
            middlediv.InnerHtml += inputBoxdiv.ToString();
            middlediv.InnerHtml += validationBoxdiv.ToString();

            outerdiv.InnerHtml = middlediv.ToString();
            return new MvcHtmlString(outerdiv.ToString());
        }
        public static MvcHtmlString NDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> dropdownlist, object htmlattr, int col = 2, string helptext = null, string inputicon = null, bool disable = false, bool showLabel = true)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName;
            var required = metadata.IsRequired;

            string coltab = (12 / col).ToString();
            var borderClass = htmlHelper.NAddClassIfPropertyInError(expression, "border-danger");
            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass(string.Format("col-lg-{0}", coltab));

            TagBuilder middlediv = new TagBuilder("div");
            middlediv.AddCssClass("form-group");
            middlediv.AddCssClass(name + "divClass");

            if (showLabel)
            {
                TagBuilder labletag = new TagBuilder("label");
                string labeltext = resolvedLabelText + (required == true ? " <span class='text-danger'>*</span>" : "") + (string.IsNullOrEmpty(helptext) == true ? "" : " <small><i>" + helptext + "</small></i>");
                labletag.InnerHtml = labeltext;
                labletag.AddCssClass(borderClass.Replace("border-danger", "text"));
                middlediv.InnerHtml = labletag.ToString();
            }

            TagBuilder inputBoxdiv = new TagBuilder("div");
            inputBoxdiv.AddCssClass("input-group");

            TagBuilder validationBoxdiv = new TagBuilder("form-control-feedback");
            validationBoxdiv.AddCssClass("form-text text-danger");



            if (!string.IsNullOrEmpty(inputicon))
            {
                TagBuilder icondiv1 = new TagBuilder("div");
                icondiv1.AddCssClass("input-group-prepend");

                TagBuilder icondiv2 = new TagBuilder("div");
                icondiv2.AddCssClass("input-group-text");

                TagBuilder icondiv3 = new TagBuilder("i");
                icondiv3.AddCssClass(inputicon);

                icondiv2.InnerHtml = icondiv3.ToString();
                icondiv1.InnerHtml = icondiv2.ToString();
                inputBoxdiv.InnerHtml = icondiv1.ToString();
            }
            var routeValueDictionary = new System.Web.Routing.RouteValueDictionary(htmlattr);
            if (routeValueDictionary.Keys.Where(x => x.ToLower() == "class") != null && routeValueDictionary.Keys.Where(x => x.ToLower() == "class").Count() > 0)
            {
                var @class = routeValueDictionary.Keys.Where(x => x.ToLower() == "class").FirstOrDefault();
                if (!string.IsNullOrEmpty(borderClass))
                {
                    routeValueDictionary[@class] = routeValueDictionary[@class].ToString().Replace(" " + borderClass, "");
                    routeValueDictionary[@class] += " " + borderClass;
                }
            }
            else
            {
                routeValueDictionary.Add("class", " " + borderClass);
            }
            if (!(dropdownlist != null && dropdownlist.Count() > 0))
            {
                dropdownlist = new List<SelectListItem> { new SelectListItem { Value = "", Text = "Select " + ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData).DisplayName } };
            }
            if (disable)
            {
                routeValueDictionary.Add("disabled", "disabled");
            }
            inputBoxdiv.InnerHtml += htmlHelper.DropDownListFor(expression, dropdownlist, routeValueDictionary).ToString();
            validationBoxdiv.InnerHtml = htmlHelper.ValidationMessageFor(expression, null, new { @class = "form-text text-danger" })?.ToString();
            middlediv.InnerHtml += inputBoxdiv.ToString();
            middlediv.InnerHtml += validationBoxdiv.ToString();

            outerdiv.InnerHtml = middlediv.ToString();
            return new MvcHtmlString(outerdiv.ToString());
        }
        public static string NAddClassIfPropertyInError<TModel, TProperty>(
        this HtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression,
        string errorClassName)
        {
            var expressionText = ExpressionHelper.GetExpressionText(expression);
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData
                .TemplateInfo.GetFullHtmlFieldName(expressionText);
            var state = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
            if (state == null)
            {
                return String.Empty;
            }

            if (state.Errors.Count == 0)
            {
                return String.Empty;
            }

            return errorClassName;
        }
        public static MvcHtmlString NCheckBoxGroupFor(this HtmlHelper htmlHelper, string RadioGroupName, IEnumerable<SelectListItem> selectlists, int col = 2, string id = "")
        {
            string coltab = (12 / col).ToString();

            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass(string.Format("col-md-{0} col-12", coltab));


            int r_id = 0;

            var grps = selectlists.Select(x => x.Group).Distinct();
            foreach (var item in grps)
            {
                TagBuilder formdiv = new TagBuilder("div");
                formdiv.AddCssClass("form-group");

                TagBuilder labletag = new TagBuilder("label");
                labletag.AddCssClass("font-weight-bold");

                string labeltext = item.Name;
                labletag.InnerHtml = labeltext;
                if (id == "Role")
                {
                    formdiv.InnerHtml += "<button type='button' class='dataTrigger' id=\"" + labeltext + "\">" + labletag.ToString() + "<i class='fa fa-plus icons pull-right' id=\"" + labeltext + "_icon\" aria-hidden='true'></i>" + "</Button> <br/>";
                    formdiv.InnerHtml += "<br/>";
                    formdiv.InnerHtml += "<div class='data' id=\"" + labeltext + "_data\" style='display:none;'>";
                    formdiv.InnerHtml += "<a href='#' data-name='" + labeltext + "' class='selectallmenu' style='color: blue; cursor: pointer'>[Select All]</a>" +
                       "&nbsp;&nbsp;<a href='#' data-name='" + labeltext + "' class='clearallmenu' style='color: blue; cursor: pointer'>[Clear All]</a> <br/>";
                }
                else
                {
                    formdiv.InnerHtml += labletag.ToString() + "&nbsp;&nbsp;<a href='#' data-name='" + labeltext + "' class='selectallmenu' style='color: blue; cursor: pointer'>[Select All]</a>" +
                    "&nbsp;&nbsp;<a href='#' data-name='" + labeltext + "' class='clearallmenu' style='color: blue; cursor: pointer'>[Clear All]</a> <br/>";

                }


                TagBuilder lableTag2 = new TagBuilder("label");
                lableTag2.AddCssClass("float-right");

                foreach (SelectListItem lst in selectlists.Where(x => x.Group.Name == labeltext))
                {
                    TagBuilder fieldset = new TagBuilder("div");
                    fieldset.AddCssClass("form-check form-check-inline");

                    TagBuilder innerLabel = new TagBuilder("label");
                    innerLabel.AddCssClass("form-check-label");

                    TagBuilder innerDiv = new TagBuilder("div");
                    //innerDiv.AddCssClass("uniform-checker");

                    TagBuilder spanElement = new TagBuilder("span");
                    if (lst.Selected)
                    {
                        spanElement.AddCssClass("checked");
                    }
                    string tick = lst.Selected == true ? "checked" : "";
                    spanElement.InnerHtml += "<input type='checkbox' data-type='" + labeltext + "'  id ='" + RadioGroupName + r_id + "' name='" + RadioGroupName + "'" + tick + " value='" + lst.Value + "'/>";
                    //spanElement.InnerHtml += "<input type='checkbox' class='form-check-input-styled'  id='" + RadioGroupName + r_id + "' name='" + RadioGroupName + "'" + tick + " value='" + lst.Value + "'/>";

                    innerDiv.InnerHtml += spanElement.ToString();
                    innerLabel.InnerHtml += innerDiv.ToString();
                    innerLabel.InnerHtml += lst.Text;
                    fieldset.InnerHtml += innerLabel.ToString();
                    formdiv.InnerHtml += fieldset.ToString();
                    r_id++;
                }

                outerdiv.InnerHtml += "<br/>" + formdiv.ToString();



            }
            return new MvcHtmlString(outerdiv.ToString());
        }
        public static MvcHtmlString NHyperLink(this HtmlHelper htmlHelper, string HyperLinkText, object htmlAttrs, string RightsUrl, string LinkUrl = "#", bool showAttrIcon = false, string iconClass = "")
        {
            if (LinkUrl != "#")
                LinkUrl = Library.ApplicationUtilities.GenerateUrl(LinkUrl);
            string hyperLink = "";
            bool hasRight = true;
            //if (!string.IsNullOrEmpty(RightsUrl))
            //{
            //    hasRight = Library.ApplicationUtilities.HasPageRight(RightsUrl);
            //}
            if (hasRight)
            {
                TagBuilder hyperlinkElement = new TagBuilder("a");
                if (htmlAttrs != null)
                    hyperlinkElement.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));
                hyperlinkElement.Attributes.Add("href", LinkUrl);
                hyperlinkElement.Attributes.Add("data-toggle", "tooltip");
                hyperlinkElement.Attributes.Add("data-placement", "bottom");
                hyperlinkElement.Attributes.Add("title", HyperLinkText);
                if (showAttrIcon)
                    hyperlinkElement.InnerHtml = "<i class='" + iconClass + "'></i> ";
                //hyperlinkElement.InnerHtml += HyperLinkText;
                hyperLink = hyperlinkElement.ToString();
            }
            return new MvcHtmlString(hyperLink);
        }
        public static MvcHtmlString NHyperLinkDwload(this HtmlHelper htmlHelper, string HyperLinkText, object htmlAttrs, string RightsUrl, string LinkUrl = "#", bool showAttrIcon = false, string iconClass = "")
        {
            if (LinkUrl != "#")
                LinkUrl = ApplicationUtilities.GetAppConfigValue("") + LinkUrl;
            string hyperLink = "";
            bool hasRight = true;
            if (!string.IsNullOrEmpty(RightsUrl))
            {
                hasRight = Library.ApplicationUtilities.HasPageRight(RightsUrl);
            }
            if (hasRight)
            {
                TagBuilder hyperlinkElement = new TagBuilder("a");
                if (htmlAttrs != null)
                    hyperlinkElement.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttrs));
                hyperlinkElement.Attributes.Add("href", LinkUrl);
                hyperlinkElement.Attributes.Add("data-toggle", "tooltip");
                hyperlinkElement.Attributes.Add("data-placement", "bottom");
                hyperlinkElement.Attributes.Add("title", HyperLinkText);
                if (showAttrIcon)
                    hyperlinkElement.InnerHtml = "<i class='" + iconClass + "'></i> ";
                hyperLink = hyperlinkElement.ToString();
            }
            return new MvcHtmlString(hyperLink);
        }
        public static MvcHtmlString NSwitchBoxFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, bool ticked, string val = "true", int col = 4, string onchangefunc = null, string helptext = null, bool disable = false)
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName;
            var required = metadata.IsRequired;

            string coltab = (12 / col).ToString();

            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass(string.Format("col-lg-{0}", coltab));

            TagBuilder middlediv = new TagBuilder("div");
            middlediv.AddCssClass("form-group");

            TagBuilder lastDiv = new TagBuilder("div");
            lastDiv.AddCssClass("form-check-switchery");
            lastDiv.AddCssClass("form-check");

            TagBuilder labeltag = new TagBuilder("label");
            labeltag.AddCssClass("form-check-label");

            string labeltext = resolvedLabelText + (required == true ? " <span class='text-danger'>*</span>" : "") + (string.IsNullOrEmpty(helptext) == true ? "" : " <small><i>" + helptext + "</small></i>");
            string tick = ticked == true ? "checked" : "";
            string disabled = disable ? "disabled='disabled'" : "";
            labeltag.InnerHtml += "<input class='form-check-input-switchery' type='checkbox' " + disabled + "  id='" + htmlHelper.IdFor(expression) + "' name='" + htmlHelper.IdFor(expression) + "'" + tick + " value='" + val + "'" + (string.IsNullOrEmpty(onchangefunc) ? "" : onchangefunc) + " data-fouc/>";
            labeltag.InnerHtml += labeltext;

            lastDiv.InnerHtml += labeltag.ToString();

            middlediv.InnerHtml = lastDiv.ToString();

            outerdiv.InnerHtml = middlediv.ToString();
            return new MvcHtmlString(outerdiv.ToString());
        }
        public static MvcHtmlString NLabelFor<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlattr, int col = 2, string helptext = null, string inputicon = null, string displayName = "")
        {
            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string resolvedLabelText = !string.IsNullOrEmpty(displayName) ? displayName : metadata.DisplayName ?? metadata.PropertyName;
            var required = metadata.IsRequired;

            string coltab = (12 / col).ToString();
            var borderClass = htmlHelper.NAddClassIfPropertyInError(expression, "border-danger");
            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass(string.Format("col-lg-{0}", coltab));

            TagBuilder middlediv = new TagBuilder("div");
            middlediv.AddCssClass("form-group");
            middlediv.AddCssClass(name + "divClass");

            TagBuilder labletag = new TagBuilder("label");
            labletag.Attributes["style"] = "font-weight: bold; font-size: 120%;";
            string labeltext = resolvedLabelText;//+ (required == true ? " <span class='text-danger'>*</span>" : "") + (string.IsNullOrEmpty(helptext) == true ? "" : " <small><i>" + helptext + "</small></i>");
            labletag.InnerHtml = labeltext;
            labletag.AddCssClass(borderClass.Replace("border", "text"));

            TagBuilder inputBoxdiv = new TagBuilder("div");
            inputBoxdiv.AddCssClass("input-group cssforlabel " + name + "labelVal");

            TagBuilder validationBoxdiv = new TagBuilder("form-control-feedback");
            validationBoxdiv.AddCssClass("form-text text-danger");



            middlediv.InnerHtml = labletag.ToString();
            if (!string.IsNullOrEmpty(inputicon))
            {
                TagBuilder icondiv1 = new TagBuilder("div");
                icondiv1.AddCssClass("input-group-prepend");

                TagBuilder icondiv2 = new TagBuilder("div");
                icondiv2.AddCssClass("input-group-text");

                TagBuilder icondiv3 = new TagBuilder("i");
                icondiv3.AddCssClass(inputicon);

                icondiv2.InnerHtml = icondiv3.ToString();
                icondiv1.InnerHtml = icondiv2.ToString();
                inputBoxdiv.InnerHtml = icondiv1.ToString();
            }
            var routeValueDictionary = new System.Web.Routing.RouteValueDictionary(htmlattr);

            if (routeValueDictionary.Keys.Where(x => x.ToLower() == "class") != null && routeValueDictionary.Keys.Where(x => x.ToLower() == "class").Count() > 0)
            {
                var @class = routeValueDictionary.Keys.Where(x => x.ToLower() == "class").FirstOrDefault();
                if (!string.IsNullOrEmpty(borderClass))
                {
                    routeValueDictionary[@class] = routeValueDictionary[@class].ToString().Replace(" " + borderClass, "");
                    routeValueDictionary[@class] += " " + borderClass;
                }

            }
            else
            {
                routeValueDictionary.Add("class", " " + borderClass);
            }
            inputBoxdiv.InnerHtml += htmlHelper.DisplayFor(expression, routeValueDictionary).ToString();
            validationBoxdiv.InnerHtml = htmlHelper.ValidationMessageFor(expression, null, new { @class = "form-text text-danger" })?.ToString();
            middlediv.InnerHtml += inputBoxdiv.ToString();
            middlediv.InnerHtml += validationBoxdiv.ToString();

            outerdiv.InnerHtml = middlediv.ToString();
            return new MvcHtmlString(outerdiv.ToString());
        }
        public static MvcHtmlString NLabelForDualModel<TModel, TValue>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TValue>> expression, object htmlattr, int col = 2, string helptext = null, string inputicon = null)
        {

            var name = ExpressionHelper.GetExpressionText(expression);
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string resolvedLabelText = metadata.DisplayName ?? metadata.PropertyName;
            var required = metadata.IsRequired;

            string coltab = (12 / col).ToString();
            var borderClass = htmlHelper.NAddClassIfPropertyInError(expression, "border-danger");
            TagBuilder outerdiv = new TagBuilder("div");
            outerdiv.AddCssClass(string.Format("col-lg-{0}", coltab));

            TagBuilder middlediv = new TagBuilder("div");
            middlediv.AddCssClass("form-group");
            middlediv.AddCssClass(name + "divClass");

            TagBuilder labletag = new TagBuilder("label");
            labletag.Attributes["style"] = "font-weight: bold; font-size: 120%;";
            string labeltext = resolvedLabelText;//+ (required == true ? " <span class='text-danger'>*</span>" : "") + (string.IsNullOrEmpty(helptext) == true ? "" : " <small><i>" + helptext + "</small></i>");
            labletag.InnerHtml = labeltext;
            labletag.AddCssClass(borderClass.Replace("border", "text"));

            TagBuilder inputBoxdiv = new TagBuilder("div");
            inputBoxdiv.AddCssClass("input-group " + name + "labelVal");

            TagBuilder validationBoxdiv = new TagBuilder("form-control-feedback");
            validationBoxdiv.AddCssClass("form-text text-danger");



            middlediv.InnerHtml = labletag.ToString();
            if (!string.IsNullOrEmpty(inputicon))
            {
                TagBuilder icondiv1 = new TagBuilder("div");
                icondiv1.AddCssClass("input-group-prepend");

                TagBuilder icondiv2 = new TagBuilder("div");
                icondiv2.AddCssClass("input-group-text");

                TagBuilder icondiv3 = new TagBuilder("i");
                icondiv3.AddCssClass(inputicon);

                icondiv2.InnerHtml = icondiv3.ToString();
                icondiv1.InnerHtml = icondiv2.ToString();
                inputBoxdiv.InnerHtml = icondiv1.ToString();
            }
            var routeValueDictionary = new System.Web.Routing.RouteValueDictionary(htmlattr);

            if (routeValueDictionary.Keys.Where(x => x.ToLower() == "class") != null && routeValueDictionary.Keys.Where(x => x.ToLower() == "class").Count() > 0)
            {
                var @class = routeValueDictionary.Keys.Where(x => x.ToLower() == "class").FirstOrDefault();
                if (!string.IsNullOrEmpty(borderClass))
                {
                    routeValueDictionary[@class] = routeValueDictionary[@class].ToString().Replace(" " + borderClass, "");
                    routeValueDictionary[@class] += " " + borderClass;
                }

            }
            else
            {
                routeValueDictionary.Add("class", " " + borderClass);
            }
            if (!string.IsNullOrEmpty(helptext) && (htmlHelper.DisplayFor(expression, routeValueDictionary).ToString() != helptext))
            {
                inputBoxdiv.InnerHtml += htmlHelper.DisplayFor(expression, routeValueDictionary).ToString() + " " + " <b style=\"font-style:italic; font-family:Source Sans Pro;\"> (" + helptext + ")</b>";
            }
            else
            {
                inputBoxdiv.InnerHtml += htmlHelper.DisplayFor(expression, routeValueDictionary).ToString();
            }
            validationBoxdiv.InnerHtml = htmlHelper.ValidationMessageFor(expression, null, new { @class = "form-text text-danger" })?.ToString();
            middlediv.InnerHtml += inputBoxdiv.ToString();
            middlediv.InnerHtml += validationBoxdiv.ToString();

            outerdiv.InnerHtml = middlediv.ToString();
            return new MvcHtmlString(outerdiv.ToString());
        }
    }
}