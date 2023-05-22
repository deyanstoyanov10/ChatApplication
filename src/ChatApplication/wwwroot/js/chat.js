"use strict";

let conn = null;

function sendMessage() {
    let msg = String(document.getElementById('message').value);

    let url = String(window.location.protocol + "//" + window.location.host + "/api/v1/message/send");

    console.log(msg);

    let data = { text: msg };

    fetch(url, {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    }).then(res => {
        console.log("Request complete! response:", res);
    });

    document.getElementById('message').value = '';

    event.preventDefault();
}

function buildMessage(message) {
    let append = '';

    append += '<li class="clearfix">';
    append += '<div class="message-data"><p>' + message.userName + '<span class="message-data-time">' + message.sendDate + '</span></p></div>';
    append += '<div class="message my-message">' + message.text + '</div>';
    append += '</li>';

    document.getElementById('chatHistory').innerHTML += append;
}

function setupConnection() {
    let protocol = window.location.protocol;

    if (protocol.includes("https")) {
        protocol = "wss:";
    }
    else {
        protocol = "ws:";
    }

    let url = String(window.location.protocol + "//" + window.location.host + "/WebSocket");

    conn = new signalR.HubConnectionBuilder().withUrl(url).build();

    conn.on("ReceiveMessages", (response) => {
        let messages = response.data
        messages.forEach(buildMessage)
        console.log(response);
    });

    conn.start().catch(err => console.error(err.toString()));
}

setupConnection();

document.getElementById("sendMessage").addEventListener("click", function () {
    sendMessage();
});

document.getElementById("message").addEventListener("keydown", function (e) {
    if (e.keyCode == 13) {
        sendMessage();
    }
});