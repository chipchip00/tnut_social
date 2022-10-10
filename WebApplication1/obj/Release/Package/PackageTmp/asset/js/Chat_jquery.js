
$(document).ready(function () {
    $("#message").keyup(function (event) {
        if (event.keyCode === 13) {
            $("#sendmessage").click();
        }
    });

    var d = $(".chat-history");
    d.scrollTop(d.prop("scrollHeight"))
    $(".nguoi_nhan").on('click', function () {
        $(".nguoi_nhan").removeClass("active");
        $(this).addClass("active");
        window.location.href = "chat?id_nguoi_nhan=" + $(this).attr('id');

    });
    function htmlEncode(value) {
        var encodedValue = $('<div />').text(value).html();
        return encodedValue;
    }
    $("#sendmessage").on('click', function () {
        let searchParams = new URLSearchParams(window.location.search);
        let id_nguoi_nhan = searchParams.get('id_nguoi_nhan');
        var message = $("#message").val();
        
        $.post("chat/save",
            {
                id_nguoi_nhan: id_nguoi_nhan,
                message: message
            },
            function (data) {
                if (data == "True") {
                    var chat = $.connection.chatHub;
                    // Create a function that the hub can call back to display messages.
                    chat.client.addNewMessageToPage = function (message) {
                        var s = '<li class="clearfix">';
                        s += '                 <div class="message other-message float-right">' + htmlEncode(message) + ' </div>';
                        s += '              </li >';
                        $('#discussion').append(s);
                        var d = $(".chat-history");
                        d.scrollTop(d.prop("scrollHeight"));
                    };
                    // Get the user name and store it to prepend to messages.
                    //$('#displayname').val(prompt('Enter your name:', ''));
                    // Set initial focus to message input box.
                    //$('#message').focus();
                    // Start the connection.
                    $.connection.hub.start().done(function () {

                            // Call the Send method on the hub.
                            chat.server.send($('#message').val());
                            // Clear text box and reset focus for next comment.
                            $('#message').val('').focus();

                    });

                }
        });
    });
})