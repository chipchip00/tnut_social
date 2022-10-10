//const { json } = require("modernizr");

$(document).ready(function () {
    window.scrollTo(0, 0);
    function duyetPost(action, id_post) {
        $.post(
            "DuyetPost",
            {
                action: action,
                id_post: id_post
            },

            function (data) {
                if (data == "1") {
                    $("#post-" + id_post).remove();
                }
            });
    }
    $(".duyet-post").click(function () {
        var id = $(this).attr("id").split("-");
        var id_post = id[2];
        var action = id[0];
        duyetPost(action, id_post);
    });
    $(window).scroll(function () {
        const windowWidth = $(window).width();
        const scrollHeight = $(document).scrollTop();
        if (windowWidth >= 992 && scrollHeight > 400) {
            console.log("sadf");
            $(".list-user-group").addClass("fixed-postion");
            return;
        }
        $(".list-user-group").removeClass("fixed-postion");
    });

});