//const { json } = require("modernizr");

const hostname = location.protocol + '//' + location.host;
$(document).ready(function () {
    //window.scrollTo(0, 0);

    $(document).on('click', '.show_comment', function () {
        var id_post = $(this).attr("id").split("-")[2];
        $("#comment-section-" + id_post).css("display", "block");
    });
  
    $(document).on('click', '.delete-post', function () {
        if (confirm('Bạn có chắc chắn muốn xóa')) {
            var id = $(this).attr("id").split("-")[2];
            // Save it!
            $.post(`${hostname}/Home/DeletePost`,
                {
                    id_post:id
                },
                function (data) {
                    if (data == "1") {
                        Swal.fire("Thành công", "Đã xóa bài viết!", "success");
                        $("#a-post-" + id).remove();
                        return;
                    }
                    Swal.fire("Thất bại", "Xóa bài viết không thành công", "error");
                    return;
                }
            )
        } else {
            // Do nothing!
            
        }
    });

     $(document).on('click', '.send-comment',function () {
        var id_post = $(this).attr("id").split("-")[2];
         var nd_comment = $("#input-comment-" + id_post).val();
         console.log(id_post, nd_comment);
         $.post(`${hostname}/Home/AddComment`,

            {
                id_post: id_post,
                nd_comment : nd_comment
            },
            function (data) {
                dt = JSON.parse(data);
                alert(data);
                var result = dt.status;
                var commmentCount = dt.sl_comment;
                var ten = dt.ten;
                var avatar = dt.avatar;
                if (result != "success") {
                    alert(data);
                    return;
                }
                $("#comment-count-" + id_post).html(commmentCount);
                var str = "";
                str += '<div class="a-comment">';
                str += `<div class="img-avatar-comment "><img src="${hostname}/asset/images/avatar/` + avatar + '" /></div>';
                str += '<div class="nd-cmt"><b>'+ten+'</b><p class="noi-dung-comment">' + nd_comment + '</p></div>';
                str += ' </div>';
                $("#comment-section-" + id_post).prepend(str);
                $("#comment-section-" + id_post).css("display", "block");
                $("#input-comment-" + id_post).val("");
        });

    });
    $(".input-comment").keyup(function (event) {
        if (event.keyCode === 13) {
            var id_post = $(this).attr("id").split("-")[2];
            console.log(id_post);
            $("#send-comment-" + id_post).click();
        }
    });
    $(document).on('click', '.like-button', function () {
        console.log("like...");
        var id_post = $(this).attr("id").split("-")[2];
        console.log(id_post);
        $.post(`${hostname}/home/updatelike`,
            {
                id_post: id_post
            },
            function (data) {
                var like_data = data.split("-");

                if (like_data[0] == "like") {
                    console.log("liked");
                    $("#like-post-" + id_post).addClass("bi-heart-fill");
                    $("#like-post-" + id_post).removeClass("bi-heart");
                    $("#like-count-" + id_post).html(like_data[1]);

                    return;
                }
                else if (like_data[0] == "unlike") {
                    $("#like-post-" + id_post).addClass("bi-heart");
                    $("#like-post-" + id_post).removeClass("bi-heart-fill");
                    $("#like-count-" + id_post).html(like_data[1]);
                    return;
                }
                alert(data);
            });

    });
    //$(document).on('click', '.show-comment', function () {

    $("#upload_avatar_img").click(function () {

        $('#imgupload').trigger('click');

    });
    $("#imgupload").change(function () {
        //$("#imgupload").on('change', function () {
        ///// Your code
        $('#submit_avatar').trigger('click');

    });
    $("#change_pass").click(function () {
        $(".main").css("opacity", "0.3");
        $("#dialog").css("display", "block");
        $('#dialog').dialog({
            title: 'Đổi mật khẩu',
            width: 400,
            height: 300,
            close: function (event, ui) { $(".main").css("opacity", "1");; }
            
        });
    });
    $('#btn_change_pass').click(function () {
        var old_pass = $("#old_pass").val();
        var new_pass = $("#new_pass").val();
        var new_pass_retype = $("#retype_new_pass").val();
        if (new_pass != new_pass_retype) {
            alert("Mật khẩu nhập lại không trùng khớp!");
            return;
        }
        $.post(`${hostname}/home/changepass`,
            {
                old_pass: old_pass,
                new_pass: new_pass,
                new_pass_retype: new_pass_retype
            },
            function (data) {
                if (data == "True") {
                    //$(".main").css("opacity", "1");
                    //$("#dialog").css("display", "none");
                    $("#dialog").dialog("close");
                    $("#old_pass").val("");
                    $("#new_pass").val("");
                    $("#retype_new_pass").val("");
                    Swal.fire("Thành công", "Chúc mừng bạn đã cập nhật mật khẩu thành công!", "success");
                    return;
                }
                $("#dialog").dialog("close");
                $("#old_pass").val("");
                $("#new_pass").val("");
                $("#retype_new_pass").val("");
                Swal.fire("Thất bại", "Mật khẩu cũ không đúng hoặc thông tin không hợp lệ!", "error");
                return;
            }
        )
    });
    $("#btnSearch").click(function () {

    });
});