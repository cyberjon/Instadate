//put in satart-a-conversation page

var StartAConversation;
var WindowBlongToSender = $("#sendFrom").val();
var WindowBlongToReceiver = $("#sendTo").val();
var hasaccepted = false;
var hasdiscard = false;
var waitingAnimation;

$(function () {

    //toggle leave message button
 

    //disable send button when page load
    $("#sendMessage").prop('disabled', true);

    // add waiting animation;
    waitingAnimation = setInterval(function () {
        if(! $('.end').length){
            $('.wait-animation').append('<span class="end">....</span>');
        } else {
            $('.end').remove();
        }     
    }, 800);

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


    //start the hub connection
    var chat = $.connection.chatHub;

    chat.client.informedAccept = function (accept_receiver, accept_sender) {
        if (WindowBlongToSender == accept_receiver && WindowBlongToReceiver == accept_sender) {
            if (hasaccepted == false) {
                $('#conversation-container').append('<p id="wait"><i class="fa fa-bullhorn"></i> ' + accept_sender + ' accepeted the invitation. Now you can talk to each other</p>');
                $("#sendMessage").prop('disabled', false);
                $('.fa-lock').addClass('hidden');
                $('.fa-unlock').removeClass('hidden');
                hasaccepted = true;
                clearInterval(waitingAnimation);
                if ($('.end').length) {
                    $('.end').remove();
                }
            }
        }
    }

    chat.client.informedDiscard = function (discard_receiver, discard_sender) {
        if (WindowBlongToSender == discard_receiver && WindowBlongToReceiver == discard_sender) {
            if (hasdiscard == false) {
                $('#conversation-container').append('<p id="wait"><i class="fa fa-bullhorn"></i> ' + discard_sender + ' refuse the invitation.</p>');
                hasdiscard = true;
                clearInterval(waitingAnimation);
                if ($('.end').length) {
                    $('.end').remove();
                }
            }
        }

    };

    chat.client.sendMessage = function (sender, message, time) {
        var checksender = $("#sendTo").val();
        if (sender == checksender)//prevent append to other people's div
            $('#conversation-container').append('<li class="receive"><strong>' + sender + '</strong>' + ': ' + '<span>' + message + '</span>' + '<span style="margin-left:20px">' + time + '</span>');
    };

    $.connection.hub.start().done(function () {
        /*--test-waiting-for-accept*/
        $('.start_a_conversation').click(function (event) {
            event.preventDefault();
            window.open($(this).attr("href"), 'startAConversation', "width=600,height=600,scrollbars=yes");
            start_receiver = $("#sendTo").val();
            start_sender = $("#sendFrom").val();
            chat.server.startAConversation(start_receiver, start_sender);
        });

        /*---*/
        $('#sendMessage').click(function () {
            // Call the Send method on the hub.
            receiver = $("#sendTo").val();
            sender = $("#sendFrom").val();
            var d = new Date();
            var time = d.toLocaleString();
           // var message = $('textarea#instant-message').val();
            var message = $('.inputDiv').html();
            $('#conversation-container').append('<li class="send"><strong>' + sender + '</strong>' + ': ' + '<span>' + message + '</span>' + '<span style="margin-left:20px">' + time + '</span></li>');
            chat.server.send(receiver, sender, message, time);
        //    $('textarea#instant-message').val('').focus();
            $(".inputDiv").text("").focus();
        });
    });

});