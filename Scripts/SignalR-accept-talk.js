

$(function () {

    var newConversation = false;//prevent other people's messages get to here
    var setSender = $("#sendTo").val();;

    //add emoji face
    $(".inputDiv").focus();

    $('.emoji-button').click(function () {
        $("#emoji-container").toggle(500);
    });

    $("#emoji-container .emoji-face").click(
        function () {
            var emotion = $(this);
            var src = $(this).attr("src");
            $(".inputDiv").append('<img class="emoji-face" src=' + src + '>');
            $("#emoji-container").toggle(500);
        }
        );


    var chatAccept = $.connection.chatHub;

    chatAccept.client.sendMessage = function (sender, message, time) {
        // Add the message to the page.
        if (setSender == sender) {
            $('#conversation-container').append('<li class="receive"><strong>' + sender + '</strong>' + ': ' + '<span>' + message + '</span>' + '<span style="margin-left:20px">' + time + '</span>');
            setSender = sender;
        }
    };

    $.connection.hub.start().done(function () {
        $('#sendMessage').click(function () {
            // Call the Send method on the hub.

            var receiver = $("#sendTo").val();
            var sender = $("#sendFrom").val();
            var d = new Date();
            var time = d.toLocaleString();
            // var message = $('textarea#instant-message').val();
            var message = $('.inputDiv').html();
            $('#conversation-container').append('<li class="send"><strong>' + sender + '</strong>' + ': ' + '<span>' + message + '</span>' + '<span style="margin-left:20px">' + time + '</span></li>');

            chatAccept.server.send(receiver, sender, message, time);
            // Clear text box and reset focus for next comment.
            //  $('textarea#instant-message').val('').focus();
            $(".inputDiv").text("").focus();
        });
    });

});