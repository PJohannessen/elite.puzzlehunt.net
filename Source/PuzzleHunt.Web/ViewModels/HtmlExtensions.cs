using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace PuzzleHunt.Web.Models
{
    public static class HtmlExtensions
    {
        public static IHtmlString TimeZoneDropdown<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ICollection<SelectListItem> items = new List<SelectListItem>();
            ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones(); 
            foreach (TimeZoneInfo timeZone in timeZones)
            {
                bool isSelected = timeZone.Id == "UTC";
                items.Add(new SelectListItem {Value = timeZone.Id, Text = timeZone.DisplayName, Selected = isSelected});
            }
            return htmlHelper.DropDownListFor(expression, items);
        }
    }
}