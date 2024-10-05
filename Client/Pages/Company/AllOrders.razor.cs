using Client.Services.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Shared.Common.General;
using Shared.DTOs;

namespace Client.Pages.Company
{
    public partial class AllOrders : ComponentBase
    {
        [Inject] private IBOrderService BOrderService { get; set; } = default!;

        private TableData<OrderDto> OrdersTableData { get; set; } = new TableData<OrderDto>
        {
            Items = [],
            TotalItems = 0,
        };

        private PaginationParams PaginationParams { get; set; } = new PaginationParams
        {
            PageIndex = 0,
            PageSize = 10,
        };

        private int Counter { get; set; } = 0;

        protected override async Task OnInitializedAsync()
        {
            var data = await BOrderService.GetOrdersCountAsync();

            data.Match(
                success =>
                {
                    if (success.IsSuccess)
                    {
                        OrdersTableData.TotalItems = (int)success.Data;
                    }

                    return new Unit();
                },
                failure =>
                {
                    return new Unit();
                }
            );
        }

        private async Task<TableData<OrderDto>> ReloadServerDataAsync(TableState state)
        {
            PaginationParams.PageIndex = state.Page;
            PaginationParams.PageSize = state.PageSize;

            var data = await BOrderService.GetPaginatedOrdersAsync(PaginationParams, false);

            _ = data.Match(
                    success =>
                    {
                        if (success.IsSuccess)
                        {
                            OrdersTableData.Items = success
                            .Data?
                            .OrderBy(x => x.User.UserName, StringComparer.Ordinal)
                            .ToList() ?? [];

                            Counter = state.Page * state.PageSize;
                        }

                        return OrdersTableData;
                    },
                    failure =>
                    {
                        return OrdersTableData;
                    }
                );

            return OrdersTableData;
        }
    }
}
