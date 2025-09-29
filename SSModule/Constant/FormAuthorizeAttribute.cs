 
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic; 
using System.Linq; 
using Microsoft.AspNetCore.Http;
using System;
using SSAdmin.Areas;
using SSRepository.Models;
using Newtonsoft.Json;

public class FormAuthorizeAttribute : ActionFilterAttribute
{
    private readonly FormRight _requiredRight;
    private readonly bool _isResponse;

    public FormAuthorizeAttribute(FormRight requiredRight = FormRight.Access, bool isResponse = false)
    {
        _requiredRight = requiredRight;
        _isResponse = isResponse;

    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Detect BaseController
        if (context.Controller is not BaseController baseController)
        {
            HandleUnauthorized(context, "Invalid controller base type.");
            return;
        }

        var fkFormId = baseController.FKFormID;
        var httpContext = context.HttpContext;

        string companyName = httpContext.Session.GetString("CompanyName") ?? "";
        string userId = httpContext.Session.GetString("UserID") ?? "";
        string filePathmenulist = Path.Combine("wwwroot", "Data", companyName, userId, "menulist.json");
        List<RoleDtlModel> menuData = new List<RoleDtlModel>();
        if (System.IO.File.Exists(filePathmenulist))
        {
            string menulist = System.IO.File.ReadAllText(filePathmenulist);

            if (!string.IsNullOrWhiteSpace(menulist))
            {
                 menuData = JsonConvert.DeserializeObject<List<RoleDtlModel>>(menulist);
              
            }
        } 

        var formPermission = FindMenuById(menuData, fkFormId); //menuData?.FirstOrDefault(m => m.PKFormID == fkFormId);

        bool hasAccess = false;

        if (formPermission != null &&  formPermission.IsAccess)
        {

            switch (_requiredRight)
            {
                case FormRight.Browse:
                    hasAccess = formPermission.IsBrowse;
                    break;
                case FormRight.Add:
                    if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is long idVal && idVal > 0)
                    {
                        hasAccess = formPermission.IsEdit;
                    }
                    else if (context.ActionArguments.TryGetValue("model", out var modelObj) && modelObj is BrandModel model && model.PKID > 0)
                    {
                        hasAccess = formPermission.IsEdit; 
                    }
                    else
                    {
                        hasAccess = formPermission.IsCreate;
                    }
                    break;
                case FormRight.Delete:
                    hasAccess = formPermission.IsDelete;
                    break;
                case FormRight.Print:
                    hasAccess = formPermission.IsPrint;
                    break;
                default:
                    hasAccess = true;
                    break;
            }
            baseController.ViewBag.IsBrowse = formPermission.IsBrowse;
            baseController.ViewBag.IsAccess = formPermission.IsAccess;
            baseController.ViewBag.IsEdit = formPermission.IsEdit;
            baseController.ViewBag.IsDelete = formPermission.IsDelete;
            baseController.ViewBag.IsPrint = formPermission.IsPrint;

            
        }

        if (!hasAccess)
        {
            HandleUnauthorized(context, $"You do not have {_requiredRight} permission for {baseController.PageHeading}");
        }

        base.OnActionExecuting(context);
    }

    private void HandleUnauthorized(ActionExecutingContext context, string message)
    {
        if (_isResponse)
        {
            context.Result = new BadRequestObjectResult(message);
        }
        else
        {
            var request = context.HttpContext.Request;

            // Get full URL (scheme + host)
            string baseUrl = $"{request.Scheme}://{request.Host}";

            // Try to get "area" or "controller" from route
            string routeSegment = request.RouteValues["area"]?.ToString()
                                  ?? request.RouteValues["controller"]?.ToString();

            if (!string.IsNullOrEmpty(routeSegment))
            {
                // Remove the controller/area segment from the URL to get app root
                int index = request.Path.Value.IndexOf($"/{routeSegment}", StringComparison.OrdinalIgnoreCase);
                if (index >= 0)
                    baseUrl += request.Path.Value.Substring(0, index);
            }
            string redirectUrl = $"{baseUrl}/Base/NoAccess?error={Uri.EscapeDataString(message)}&title=Access Denied";
            context.Result = new RedirectResult(redirectUrl, permanent: false);
        }
    }

    private RoleDtlModel? FindMenuById(List<RoleDtlModel> menus, long fkFormId)
    {
        if (menus == null) return null;

        foreach (var menu in menus)
        {
            // direct match
            if (menu.FKFormID == fkFormId)
                return menu;

            // search children recursively
            if (menu.SubMenu != null && menu.SubMenu.Any())
            {
                var found = FindMenuById(menu.SubMenu, fkFormId);
                if (found != null)
                    return found;
            }
        }

        return null;
    }


}

public enum FormRight
{
    Access,
    Browse,
    Add,
    Edit,
    Delete,
    Print
}
