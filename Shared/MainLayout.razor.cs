using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using STAVerwaltung.Pages;

namespace STAVerwaltung.Shared
{
    public partial class MainLayout
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        protected async System.Threading.Tasks.Task ButtonDashboardClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            NavigationManager.NavigateTo($"/");
        }

        protected async System.Threading.Tasks.Task ButtonAufgabenClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<Aufgaben>("Aufgaben", null, new DialogOptions { Resizable = true, Draggable = true });
        }

        protected async System.Threading.Tasks.Task PanelMenuClick(Radzen.MenuItemEventArgs args)
        {
            if (args.Path != null)
            {
                if (MenüNachAuswahlSchließen) 
                {
                    leftSidebarExpanded = false;
                }
            }
        }

        protected async System.Threading.Tasks.Task PanelMenuAbmeldenClick(Radzen.MenuItemEventArgs args)
        {
            Security.Logout();
        }

        protected async System.Threading.Tasks.Task ImageSTAVerwaltungClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            NavigationManager.NavigateTo($"/");

            // Den Fokus vom Button entfernen
            await JSRuntime.InvokeVoidAsync("removeFocus", "button:focus");
        }

        protected async System.Threading.Tasks.Task ButtonSidebarLeftClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            leftSidebarExpanded = !leftSidebarExpanded;

            // Den Fokus vom Button entfernen
            await JSRuntime.InvokeVoidAsync("removeFocus", "button:focus");
        }

        protected async System.Threading.Tasks.Task ButtonSidebarRightClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            rightSidebarExpanded = !rightSidebarExpanded;

            // Den Fokus vom Button entfernen
            await JSRuntime.InvokeVoidAsync("removeFocus", "button:focus");
        }
    }
}
