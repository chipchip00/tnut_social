$(document).ready(function () {
    var page = 1;
    $(window).scroll(function () {
        if ($(window).scrollTop() + $(window).height() == $(document).height()) {
            $.post("Home/ReadMore",

                {
                    page_number: page
                },
                function (data) {
                    //alert(data);
                    if (data == "") {
                        $("#message-post").html("Không còn bài viết nào cũ hơn");
                        return;
                    }
                    dt = JSON.parse(data);
                    str = "";
                    for (var i = 0; i < dt.length; i++) {
                        var class_heart = " bi-heart";
                        if (dt[i].user_like == true) { class_heart = "bi-heart-fill"; }
                        str += '<div class="a-post">';
                        str += '   <div class="user-group">';

                        str += '       <b><a href="/profile?id='+dt[i].id_post+'"><span class="img-avatar"><img src="asset/images/avatar/'+dt[i].avatar+'" /></span></a><span>'+dt[i].nguoi_dang+' >></span><span><a href="/group/chitiet?ma_nhom='+dt[i].ma_nhom+'"> '+dt[i].ten_nhom+'</a></span></b>';
                        str += ' </div>';
                        str += '  <div class="text-post">';
                        str += dt[i].nd;
                        str += ' </div>';
                        str += ' <div class="image-post gallery-' + dt[i].img.length+'">';
                        for (var j = 0; j < dt[i].img.length; j++) {
                            str += ' <img src="asset/images/post/' + dt[i].img[j].link + '" alt="' + dt[i].img[j].mo_ta +'" />';

                        }

                        str += '</div>';
                        str += ' <div class="action-post">';
                        str += '   <i class="bi ' + class_heart + ' fa-2x like-button" id="like-post-' + dt[i].id_post + '" ></i>';
                        str += ' <input type="text" class="input-comment" id="input-comment-' + dt[i].id_post + '" placeholder="Thêm bình luận mới">';
                        str += '      <button id="send-comment-' + dt[i].id_post + '" class="btn btn-info send-comment"><i class="bi bi-send-check-fill"></i></button>';
                        str += '      <i class="bi bi-hand-thumbs-up"></i>';
                        str += '      <span class="badge rounded-pill badge-notification bg-danger" id="like-count-' + dt[i].id_post + '">'+dt[i].like.length+'</span>';
                        str += '       <i class="bi bi-chat-left-text fa-2x show_comment" id="show-comment-' + dt[i].id_post + '"></i>';
                        str += '       <span class="badge rounded-pill badge-notification bg-danger" id="comment-count-' + dt[i].id_post + '">' + dt[i].cmt.length +'</span>';

                        str += ' </div>';
                        str += '          <div class="comment-section" id="comment-section-' + dt[i].id_post + '">';
                        for (var m = 0; m < dt[i].cmt.length; m++) {
                            str += '                 <div class="a-comment">';
                            str += '                     <div class="img-avatar-comment "><a href="/profile?id=' + dt[i].cmt[m].uid + '"><img src="asset/images/avatar/' + dt[i].cmt[m].avatar + '" /></a></div>';
                            str += '                     <div class=" nd-cmt"><b>' + dt[i].cmt[m].ten + '</b><p class="noi-dung-comment">' + dt[i].cmt[m].nd +'</p></div>';

                            str += '                   </div>';
                        }
                        str += '            </div>';


                        str += '                </div>';
                        if (typeof dt[i].cmt[0] !== 'undefined') console.log(dt[i].cmt[0].nd);
                    }
                    str += '              </div>';
                    $(".main-post").append(str);
                });

        //    page++;

        }
    });
});