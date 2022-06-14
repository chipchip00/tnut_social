//        $(function () {

//             Reference the auto-generated proxy for the hub.
//            var chat = $.connection.chatHub;
//        // Create a function that the hub can call back to display messages.
//        chat.client.addNewMessageToPage = function (message) {
//                var s = '<li class="clearfix">';
//            s += '<div class="message-data text-right" >';
//                s += '                       <span class="message-data-time">10:10 AM, Today</span>';
//                s += '                  </div>';
//            s += '                 <div class="message other-message float-right">' + htmlEncode(message) + ' </div>';
//            s += '              </li >';
//            $('#discussion').append(s);
//            };
//        // Get the user name and store it to prepend to messages.
//        //$('#displayname').val(prompt('Enter your name:', ''));
//        // Set initial focus to message input box.
//        $('#message').focus();
//        // Start the connection.
//        $.connection.hub.start().done(function () {
//            $('#sendmessage').click(function () {
//                // Call the Send method on the hub.
//                chat.server.send($('#message').val());
//                // Clear text box and reset focus for next comment.
//                $('#message').val('').focus();

//            });
//            });
//        });
//        // This optional function html-encodes messages for display in the page.
//        function htmlEncode(value) {
//            var encodedValue = $('<div />').text(value).html();
//        return encodedValue;
//}
