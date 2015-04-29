using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;

public class CustomJsonResult : JsonResult
{
    public CustomJsonResult(object json)
    {
        _json = json;
        _partials = new List<KeyValuePair<string, object>>();
        _results = new List<string>();
    }

    private readonly object _json;
    private readonly IList<KeyValuePair<string, object>> _partials;
    private readonly IList<string> _results;

    public CustomJsonResult WithHtml(string partialViewName = null, object model = null)
    {
        _partials.Add(new KeyValuePair<string, object>(partialViewName, model));
        return this;
    }

    public override void ExecuteResult(ControllerContext context)
    {
        foreach (var partial in _partials)
        {
            var html = RenderPartialToString(context, partial.Key, partial.Value);
            _results.Add(html);
        }
        base.Data = Data;
        base.ExecuteResult(context);
    }

    public new object Data
    {
        get
        {
            return new
            {
                Html = _results,
                Json = _json
            };
        }
    }

    public static string RenderPartialToString(ControllerContext context, string viewName, object model)
    {
        var controller = context.Controller;

        var partialView = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);

        var stringBuilder = new StringBuilder();
        using (var stringWriter = new StringWriter(stringBuilder))
        {
            using (var htmlWriter = new HtmlTextWriter(stringWriter))
            {
                controller.ViewData.Model = model;
                partialView.View.Render(
                    new ViewContext(
                        controller.ControllerContext,
                        partialView.View,
                        controller.ViewData,
                        new TempDataDictionary(),
                        htmlWriter),
                    htmlWriter);
            }
        }
        return stringBuilder.ToString();
    }

}

