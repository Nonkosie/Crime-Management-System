$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#managerData').DataTable({
        "ajax": {
            url: '/CaseManager/GetAllManagers'
        },

        "columns": [
        { data: 'firstName' },
        { data: 'lastName' },
        { 

            data: 'id',
            "render": function (data) {
                return `<div class="btn-group" role="group">
                <a href="/CaseManager/ViewCase?caseManagerId=${data}" class="btn btn-primary" data-bs-theme="dark">View</a>
                </div>`
            }
         },
        //{ data: 'Cases', "width": "15%" },
        
        ]
    });
}



