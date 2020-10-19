$(() => {
    init();
});

var existing_users = [];

function init() {
    $.ajaxSetup({ cache: false });
    $("#userModal").on('click', OnUserModalCalled);
    $(".delete_contact").on('click', OnDeleteContact);
}


function OnUserModalCalled(e) {
    e.preventDefault();
    $.get("/Manage/ContactPartial", function (data) {
        $('#dialogContent').html(data);
        $('#modDialog').modal('show');

        $(".findContact").click(function (e) {
            $.get("/Chat/GetUsersByEmail", {
                email: $("#email_input").val(),
                availableOnly: $("#chat_is_private").is(":checked")
            })
            .done((data) => {
                $(".modal-body").html(UsersFromJson(data));
                $(".possibleContact").click(OnAppendContact);
            });
        });

        $(".findContact").click();
        $("#email_input").keyup(OnPressInputEnter);
    });
}

function OnDeleteContact(e) {
    if (confirm('Are you sure?')) {
        $.post("/Manage/DeleteContact", {
            contactId: $(this).parent().attr("id")
        })
        .done((data) => {
            if (data === "true")
                window.location.reload();
            else
                alert("Something wrong");
        });
    }
}

function OnAppendContact(e) {
    let userId = $(this).attr("id");
    let userName = $(this).children("p").text();

    AddNewContact(userId, userName);
}

function OnRemoveChatUser(e) {
    let userId = $(this).parent().attr("id").split('_')[1];
    existing_users.splice(existing_users.indexOf(userId), 1);
    $(`#input_${userId}`).remove();

    RewriteRemoveButtonEvent();
}

function AddNewContact(userId, userName) {
    existing_users.push(userId);

    let viewUserInfo = userName.length === 0 ? "" : `<span class="remove_user"></span>
                                                <h4>${userName}</h4>`;
    
    $("#userList").append(`
        <div id="input_${userId}">
            ${viewUserInfo}
            <input type="hidden" id="user_${userId}" name="users" value="${userId}"/>
        </div>
    `);
    
    RewriteRemoveButtonEvent();
}

function UsersFromJson(jsonString) {
    let output = "";
    let input = JSON.parse(jsonString);
    input.forEach(e => {
        if (!IsExists(e.Id, existing_users)) {
            output += `
                <div id="${e.Id}" class="possibleContact btn" data-dismiss="modal">
                    <p>${e.FirstName} ${e.LastName}</p>
                    <h5>${e.Email}</h5>
                </div>`;
        }
    });

    return output.length !== 0 ? output : "<p>User is not found</p>";
}

function OnPressInputEnter(event) {
    if (event.keyCode === 13) {
        $(".findContact").click();
    }
}

function IsExists(value, array) {
    for (let i in array)
        if (array[i] === value)
            return true;

    return false;
}

function RewriteRemoveButtonEvent() {
    $(".remove_user").off('click', OnRemoveChatUser);
    $(".remove_user").on('click', OnRemoveChatUser);
}
