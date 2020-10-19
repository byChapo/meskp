$(() => {
    init();
});

var ChatConnection;
var ChatId;
var CurrentUserId;

const message_types = {
    "Text": (m) => PrintText(m),
    "Image": (m) => PrintImage(m),
    "File": (m) => PrintFile(m),
}

function init() {
    CurrentUserId = window.userId;

    $(".chat_info").on("click", OnChatSelected);
    $("#input_message").keyup(OnPressTextEnter);
    $("#input_btn_send").on("click", OnChatButtonClick);
    $("#input_btn_img").on("click", OnImageButtonClick);
    $("#upload_file").on("change", OnFileUpload);
}

// --events--

function OnChatSelected(e) {
    ChatConnection = $.connection.chatHub;
    InitializeChatHandlers(ChatConnection);

    $(this).addClass("active");
    $(".chat_input").css("pointer-events", "all");
    $(`#chat_${ChatId}`).removeClass("active");

    if (ChatId)
        ChatConnection.server.disconnect(ChatId);

    let chat_id = $(this).attr("id").split("_")[1];
    ChatId = chat_id;

    GetChatContent(chat_id);
}

function OnChatButtonClick(e) {
    let chat_text = $("#input_message").val();
    if (chat_text.search(/.+/) >= 0) {
        ChatConnection.server.send({
            Content: chat_text,
            Type: "Text",
            authorId: CurrentUserId
        }, ChatId);
    }

    $("#input_message").val('');
}

function OnImageButtonClick(e) {
    $("#upload_file").click();
}

function OnFileUpload(e) {
    var file = e.target.files[0];
    var data = new FormData();
    data.append("file", file);

    $.ajax({
        type: "POST",
        url: '/Home/UploadFile',
        contentType: false,
        processData: false,
        data: data,
        success: (result) => {
            ChatConnection.server.send({
                Content: result.file_name,
                Type: result.file_type,
                authorId: CurrentUserId
            }, ChatId);
        }
    });
}

function OnPressTextEnter(event) {
    if (event.keyCode === 13) {
        $("#input_btn_send").click();
    }
}

function InitializeChatHandlers(chat) {
    chat.client.addChatMessage = function (message) {
        Print(message);
    }

    chat.client.addInfoMessage = function (message) {
        PrintInfo(message);
    }
}

// --helpers--

function GetChatContent(chat_id) {
    $.post("/Home/GetChatContent", {
        chatId: parseInt(chat_id)
    })
    .done(data => {
        $(".chat_body").empty();
        ConnectWithWebSocket(JSON.parse(data));
    });
}

function ConnectWithWebSocket(data) {
    $.connection.hub.start()
    .done(() => {
        ChatConnection.server.connect(ChatId);
        ShowChatUsers(data['chat_users']);
        data['messages'].forEach((elem) => {
            Print(elem);
        });
    });
}

function ShowChatUsers(users) {
    $(".chat_header").empty();
    users.forEach((elem) => {
        $(".chat_header").append(`
            <div><h4>${elem.FirstName} ${elem.LastName}(${elem.Email})</h4></div>
        `);
    });
}

function Print(message) {
    $(".chat_body").append(`
        <div class="speech bubble${message.AuthorId === CurrentUserId ? "-my" : ""}">
            <div class="message_sender">
                ${message.Author}
            </div>
            <div class="message_content">
                ${message_types[message.Type](message.Content)}
            </div>
            <div class="message_date">
                ${message.CreatedAt}
            </div>
        </div>
    `);
    $(".chat_body").scrollTop($(".chat_body").get(0).scrollHeight);
}

function PrintText(message) {
    return message;
}

function PrintImage(message) {
    return `
        <a href="/data/images/${message}" target="_blank">
            <img class="message_img" src="/data/images/${message}">
        </a>
    `;
}

function PrintFile(message) {
    return `
        <a href="/data/files/${message}" target="_blank">
            <b>${message}</b>
        </a>
    `;
}

function PrintInfo(message) {
    $(".chat_body").append(`
        <center>
            <p>${message}</p>
        </center>
    `);
    $(".chat_body").scrollTop($(".chat_body").get(0).scrollHeight);
}
