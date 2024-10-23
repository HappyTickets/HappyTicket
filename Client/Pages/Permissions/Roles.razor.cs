//using Client.Components.Dialogs.Permessions;
//using Client.Services.Interfaces;
//using LanguageExt;
//using Microsoft.AspNetCore.Components;
//using MudBlazor;
//using Shared.DTOs.Authorization.Response;
//using Shared.ResourceFiles;

//namespace Client.Pages.Permessions
//{
//    public partial class Roles
//    {
//        private List<RoleDto> roles = [];
//        private bool isEditModalVisible;
//        private bool isCreateModalVisible;

//        private string selectedRoleId;

//        [Inject] private BIAuthorizationService AuthService { get; set; }

//        protected override async Task OnInitializedAsync()
//        {
//            await LoadRolesAsync();
//        }

//        private async Task LoadRolesAsync()
//        {
//            try
//            {
//                var response = await AuthService.GetRolesList();
//                _ = response.Match(
//               succ =>
//               {
//                   roles = succ.Data?.ToList() ?? [];
//                   if (!succ.IsSuccess)
//                   {
//                       Snackbar.Show(Resource.Roles_Load_Fail, Severity.Error);
//                   }
//                   Snackbar.Show(succ.Title, succ.IsSuccess ? Severity.Info : Severity.Error);
//                   succ.ErrorList?.ToList().ForEach(x => Snackbar.Show($"{x.Title}: {x.Message}", Severity.Error));
//                   return new Unit();
//               },
//               fail =>
//               {
//                   Snackbar.Show(Resource.Roles_Load_Fail, Severity.Error);
//                   return new Unit();
//               }
//           );

//            }
//            catch (Exception ex)
//            {
//                Snackbar.Show(Resource.Roles_Load_Fail, Severity.Error);
//            }
//        }

//        private void NavigateTo(string action, string roleId)
//        {
//            NavigationManager.NavigateTo($"/role/{action}/{roleId}");
//        }

//        #region Actions 



//        private void CloseModals()
//        {
//            isEditModalVisible = false;
//            isCreateModalVisible = false;
//        }

//        private void OpenCreateModal()
//        {
//            isCreateModalVisible = true;
//        }
//        private void OpenEditModal(string roleId)
//        {
//            selectedRoleId = roleId;
//            isEditModalVisible = true;
//        }


//        private async Task ReloadRoles()
//        {
//            selectedRoleId = null;
//            await LoadRolesAsync();
//        }

//        private async Task OpenConfirmationDeleteDialog(string roleId)
//        {
//            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

//            var parameters = new DialogParameters<DeleteRoleDialog>
//    {
//        { x=>x.RoleId, roleId },
//        { x=>x.OnRoleDeleted, EventCallback.Factory.Create(this, ReloadRoles) }
//    };

//            var dialog = DialogService.Show<DeleteRoleDialog>(Resource.Delete + " " + Resource.Role, parameters, options);
//        }

//        #endregion
//    }

//}
