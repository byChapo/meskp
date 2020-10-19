$(() => {
    init();
});


function init() {
    $.ajaxSetup({ cache: false });
    $("#userModal").on("click", OnUserModalCalled);
    $(".delete_contact").on("click", OnDeleteContact);
}


function OnUserModalCalled(e) {
    e.preventDefault();
    $.get("/Manage/ContactPartial", function (data) {
        $('#dialogContent').html(data);
        $('#modDialog').modal('show');

        $(".findContact").click(function (e) {
            $.get("/Manage/GetUsersByEmail", { email: $("#email_input").val() })
                .done((data) => {
                    $(".modal-body").html(UsersFromJson(data));

                    $(".possibleContact").click(AddNewContact);
                });
        });

        $(".findContact").click();
        $("#email_input").keyup(OnPressInputEnter);
    });
}

function OnDeleteContact(e) {
    if (confirm('Are you sure?')) {
        $.post("/Manage/DeleteContact", { contactId: $(this).parent().attr("id") })
            .done((data) => {
                if (data === "true")
                    window.location.reload();
                else
                    alert("Something wrong");
            });
    }
}

function AddNewContact(e) {
    if (confirm('Are you sure?')) {
        $.post("/Manage/AddContact", { contactId: $(this).attr("id") })
            .done((data) => {
                if (data === "true")
                    window.location.reload();
                else
                    alert("Something wrong");
            });
    }
}

function UsersFromJson(jsonString) {    
    let output = "";
    let input = JSON.parse(jsonString);
    input.forEach(e => {
        output += `
        <div id="${e.Id}" class="possibleContact btn">
            <p>${e.FirstName} ${e.LastName}</p>
            <h5>${e.Email}</h5>
        </div>`;
    });

    return output.length !== 0 ? output : "<p>User is not found</p>";
}

function OnPressInputEnter(event) {
    if (event.keyCode === 13) {
        $(".findContact").click();
    }
}
