//add to userProfile page

$(function () {
    var chatStart = $.connection.chatHub;

    $.connection.hub.start().done(function () {
        $('.start_a_conversation').click(function (event) {
            event.preventDefault();
            
            window.open($(this).attr("link"), this.target, "width=800,height=600,scrollbars=yes");
           
            var start_receiver = $("#sendTo").val();
            var start_sender = $("#sendFrom").val();
            chatStart.server.startAConversation(start_receiver, start_sender);
        });
    });
});