    

var StartAConversation;

$(function () {

    var chat = $.connection.chatHub;

    chat.client.sendInvitation = function (start_receiver, start_sender) {

        var start_receiver = start_receiver;
        var start_sender = start_sender;
        var alertAConversation;
        var link ="/home/AcceptAConversation/?sender=" + start_sender + "&&receiver=" + start_receiver;
        var target =start_sender;
              
        $(".alertCenter").append('<li class=' + start_sender + '><span><i class="fa fa-bullhorn"></i> '
            + start_sender
            + ' invites a conversation</span><br>'
            + '<a class="btn btn-info openInNewWindow'
            + start_sender
            + '" href="#"' +' link='+link+' target=' + target + '>' + "Accept</a>" +
           '<a class="btn btn-default discard'+start_sender +'">'+'Discard</a>' + '</li>');
             
        alertAConversation = setInterval(function () {

            console.log("#fa-bell");
            $("#fa-bell").effect("shake");
        }, 2000);
            
        $('#alert-center').click(function () {

            clearInterval(alertAConversation);
            var classname = "openInNewWindow" + start_sender;
            var discard = "discard" + start_sender;

            $("." + classname).click(function (event) {
                event.preventDefault();
                window.open($(this).attr("link"), this.target, "width=800,height=600,scrollbars=yes");
                var accept_sender = start_receiver;
                var accept_receiver = start_sender;
                var chat1 = $.connection.chatHub;
                $.connection.hub.start().done(function () {
                    chat1.server.acceptConversation(accept_receiver, accept_sender);
                });

                $(this).parent('li').remove();
            });

            $("." + discard).click(function () {
                var discard_sender = start_receiver;
                var discard_receiver = start_sender;                      
                var chat2 = $.connection.chatHub;
                $.connection.hub.start().done(function () {
                    chat2.server.discardConversation(discard_receiver, discard_sender);
                });
                $(this).parent('li').remove();
            });
        });
    }

    $.connection.hub.start().done(function () {
    });
           
});
