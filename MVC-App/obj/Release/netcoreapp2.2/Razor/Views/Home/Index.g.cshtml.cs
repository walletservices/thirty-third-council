#pragma checksum "C:\Users\LiamBell\source\repos\MVC-App\MVC-App\MVC-App\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6e96bbdb4f9d8994dd29535d52fd521c31a7155f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(MVC_App.Pages.Home.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Views/Home/Index.cshtml", typeof(MVC_App.Pages.Home.Views_Home_Index))]
namespace MVC_App.Pages.Home
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#line 1 "C:\Users\LiamBell\source\repos\MVC-App\MVC-App\MVC-App\Views\_ViewImports.cshtml"
using MVC_App;

#line default
#line hidden
#line 1 "C:\Users\LiamBell\source\repos\MVC-App\MVC-App\MVC-App\Views\Home\Index.cshtml"
using Microsoft.AspNetCore.Authentication;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6e96bbdb4f9d8994dd29535d52fd521c31a7155f", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9ffa56aa7a40c25d030ccc4377ac454fd3dc8443", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IndexModel>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-page", "/Step1", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 3 "C:\Users\LiamBell\source\repos\MVC-App\MVC-App\MVC-App\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "IS Claims and Attestations";

#line default
#line hidden
            BeginContext(125, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 7 "C:\Users\LiamBell\source\repos\MVC-App\MVC-App\MVC-App\Views\Home\Index.cshtml"
 if(User.Identity.IsAuthenticated)
{

#line default
#line hidden
            BeginContext(166, 38, true);
            WriteLiteral("<input id=\"access_token\" type=\"hidden\"");
            EndContext();
            BeginWriteAttribute("value", " value=\"", 204, "\"", 280, 1);
#line 9 "C:\Users\LiamBell\source\repos\MVC-App\MVC-App\MVC-App\Views\Home\Index.cshtml"
WriteAttributeValue("", 212, ViewContext.HttpContext.GetTokenAsync(".AspNetCore.Cookies").Result, 212, 68, false);

#line default
#line hidden
            EndWriteAttribute();
            BeginContext(281, 5, true);
            WriteLiteral(" />\r\n");
            EndContext();
#line 10 "C:\Users\LiamBell\source\repos\MVC-App\MVC-App\MVC-App\Views\Home\Index.cshtml"
}

#line default
#line hidden
            BeginContext(289, 62, true);
            WriteLiteral("<div class=\"text-center\">\r\n    <h1 class=\"display-4\">Welcome  ");
            EndContext();
            BeginContext(352, 18, false);
#line 12 "C:\Users\LiamBell\source\repos\MVC-App\MVC-App\MVC-App\Views\Home\Index.cshtml"
                              Write(User.Identity.Name);

#line default
#line hidden
            EndContext();
            BeginContext(370, 79, true);
            WriteLiteral("</h1>\r\n</div>\r\n\r\n<div class=\"card-group\">\r\n    <div class=\"card-body btn-info\">");
            EndContext();
            BeginContext(449, 47, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6e96bbdb4f9d8994dd29535d52fd521c31a7155f5178", async() => {
                BeginContext(470, 22, true);
                WriteLiteral("Apply for a Blue Badge");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(496, 136, true);
            WriteLiteral("</div>\r\n</div>\r\n<p />\r\n<div class=\"card-header \">\r\n\r\n    <div class=\"card-group\">\r\n        <div class=\"card-body col-lg-12 btn-success\">");
            EndContext();
            BeginContext(632, 56, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6e96bbdb4f9d8994dd29535d52fd521c31a7155f6714", async() => {
                BeginContext(653, 31, true);
                WriteLiteral("Apply for a NIC card via Claims");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(688, 105, true);
            WriteLiteral("</div>\r\n    </div>\r\n\r\n    <div class=\"card-group\">\r\n        <div class=\"card-body col-lg-12 btn-success\">");
            EndContext();
            BeginContext(793, 63, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "6e96bbdb4f9d8994dd29535d52fd521c31a7155f8222", async() => {
                BeginContext(814, 38, true);
                WriteLiteral("AApply for a NIC card via Attestations");
                EndContext();
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Page = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(856, 28, true);
            WriteLiteral("</div>\r\n    </div>\r\n</div>\r\n");
            EndContext();
            DefineSection("scripts", async() => {
                BeginContext(901, 186, true);
                WriteLiteral("\r\n    <script>\r\n        var key = \"idToken\";\r\n        var accessToken = document.getElementById(\"access_token\");\r\n        sessionStorage.setItem(key, accessToken.value);\r\n    </script>\r\n");
                EndContext();
            }
            );
            BeginContext(1090, 4, true);
            WriteLiteral("\r\n\r\n");
            EndContext();
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IndexModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
