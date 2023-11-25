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

        // private bool sidebarExpanded = true;

        [Inject]
        protected SecurityService Security { get; set; }

        //void SidebarToggleClick()
        //{
        //    sidebarExpanded = !sidebarExpanded;
        //}

        protected void ProfileMenuClick(RadzenProfileMenuItem args)
        {
            if (args.Value == "Logout")
            {
                Security.Logout();
            }
        }

        protected async System.Threading.Tasks.Task ButtonDashboardClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            NavigationManager.NavigateTo($"/dashboard");
        }

        protected async System.Threading.Tasks.Task ButtonAufgabenClick(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
        {
            await DialogService.OpenAsync<Aufgaben>("Aufgaben", null, new DialogOptions { Resizable = true, Draggable = true });
        }

        protected async System.Threading.Tasks.Task PanelMenu0Click(Radzen.MenuItemEventArgs args)
        {
            if (args.Path != null)
            {
                if (MenüNachAuswahlSchließen) 
                {
                    leftSidebarExpanded = false;
                }
            }
        }
    }
}
