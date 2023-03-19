var dataTable;

$(document).ready(function () {
    var url = window.location.search;
    var urlParams = {
        "pending": "pending",
        "approved": "approved",
        "cancelled": "cancelled",
        "refunded": "refunded"
    };

    var param;
    for (var key in urlParams) {
        if (url.includes(key)) {
            param = urlParams[key];
            break;
        }
    }
    loadDataTable(param);
});

function loadDataTable(status) {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/Order/GetAll?status=" + status
        },
        "language": {
            "emptyTable": "You did not order anything yet!"
        },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "applicationUserDto.name", "width": "15%" },
            { "data": "applicationUserDto.phoneNumber", "width": "10%" },
            { "data": "applicationUserDto.email", "width": "15%" },
            { "data": "rentStatus", "width": "5%" },
            { "data": "carDto.name", "width": "5%" },
            {
                "data": "startDate",
                "width": "5%",
                "render": function (data) {
                    return Intl.DateTimeFormat('es-ES').format(new Date(data));
                }
            },
            {
                "data": "endDate",
                "width": "5%",
                "render": function (data) {
                    return Intl.DateTimeFormat('es-ES').format(new Date(data));
                }
            },
            { "data": "totalCost", "width": "5%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                    <div class="w-50 btn-group" role="group">
                        <a href="/Admin/Order/Details?rentHeaderId=${data}" class="btn btn-primary mx-2">
                            <i class="bi bi-pencil-square"></i>
                            Details</a>
                    </div>`
                },
                "width": "15%"
            },
        ]
    });
}