var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    if (url.includes("inprocess")) {
        loadDataTable("inprocess")
    }
    else {
        if (url.includes("completed")) {
            loadDataTable("completed")
        }
        else {
            if (url.includes("pending")) {
                loadDataTable("pending")
            }
            else {
                if (url.includes("approved")) {
                    loadDataTable("approved")
                }
                else {
                    loadDataTable("all")
                }
            }
        }
    }
    
})

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getAll?status' + status },
        "columns": [
            { data: 'id', "width": "5%" },
            { data: 'name', "width": "25%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "width": "10%",
                "render": function (data) {
                    return `
                        <div class="w-75 btn-group" role="group">
                            <a href="/admin/order/details?orderId=${data}" class="btn btn-success mx-2"
                                asp-route-id="@obj.Id">
                                <i class="bi bi-pencil-square"></i> Details
                            </a>
                        </div>
                    `
                }
            }
        ]
    });
}
