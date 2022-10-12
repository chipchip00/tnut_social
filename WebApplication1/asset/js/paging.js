function readMore(page) {
    let searchParams = new URLSearchParams(window.location.search)
    let maNhom = searchParams.has('ma_nhom') ? searchParams.get('ma_nhom') : null;
    $.post(`${hostname}/Home/GetPosts`,
        {
            page_number: page,
            maNhom
        },
        function (data) {
            dt = JSON.parse(data);
            if (dt.length == 0 && $('#message-post').length == 0) {
                var msg = '<p id="message-post">Không còn bài viết nào để xem</p>';
                $(".main-post").append(msg);
                return;
            }
            str = "";
            for (var i = 0; i < dt.length; i++) {
                var class_heart = " bi-heart";
                if (dt[i].user_like) { class_heart = "bi-heart-fill"; }
                str += `<div class="a-post" id="a-post-${dt[i].id_post}">
                                    <div class="user-group">
                                        <a href="/profile?id=${dt[i].id}">
                                            <span class="img-avatar">
                                                <img src="${hostname}/asset/images/avatar/${dt[i].avatar}" />
                                            </span>
                                        </a>
                                        <div>
                                            <span><b>${dt[i].nguoi_dang}</b>>></span>
                                            <p class="time-span-label">${dt[i].timeSpan}</p>
                                        </div>
                                        <span>
                                            <b>
                                                <a href="/group/chitiet?ma_nhom=${dt[i].ma_nhom}"> ${dt[i].ten_nhom}</a>
                                            </b>
                                        </span>`;
                if (dt[i].isAuthor) {
                    str += `<i title="Xóa bài viết" class="bi bi-trash delete-post" id="delete-post-${dt[i].id_post}"></i>`;
                }
                str += `</div>
                                    <div class="post-content">
                                        <div class="text-post">
                                            ${dt[i].nd}
                                        </div>
                                        <div class="image-post gallery-${dt[i].img.length}">`
                for (var j = 0; j < dt[i].img.length; j++) {
                    str += ` <img src="${hostname}/asset/images/post/${dt[i].id_post}/${dt[i].img[j].link}" alt="${dt[i].img[j].mo_ta}" />`;
                }

                str += `</div>
                                    </div>
                                    <div class="action-post">
                                        <i class="bi ${class_heart} fa-2x like-button" id="like-post-${dt[i].id_post}"></i>
                                        <input type="text" class="input-comment" id="input-comment-${dt[i].id_post}" placeholder="Thêm bình luận mới">
                                            <button id="send-comment-${dt[i].id_post}" class="btn btn-info send-comment">
                                                <i class="bi bi-send-check-fill"></i>
                                            </button>
                                            <i class="bi bi-hand-thumbs-up fa-2x"></i>
                                            <span class="badge rounded-pill badge-notification bg-danger" id="like-count-${dt[i].id_post}">${dt[i].like.length}</span>
                                            <i class="bi bi-chat-left-text fa-2x show_comment" id="show-comment-${dt[i].id_post}"></i>
                                            <span class="badge rounded-pill badge-notification bg-danger" id="comment-count-${dt[i].id_post}">${dt[i].cmt.length}</span>

                                    </div>
                                    <div class="comment-section" id="comment-section-${dt[i].id_post}">`;
                for (var m = 0; m < dt[i].cmt.length; m++) {
                    str += `<div class="a-comment">
                                            <div class="img-avatar-comment ">
                                                <a href="${hostname}/profile?id=${dt[i].cmt[m].uid}">
                                                    <img src="${hostname}/asset/images/avatar/${dt[i].cmt[m].avatar}" />
                                                </a>
                                            </div>
                                            <div class=" nd-cmt">
                                                <b>${dt[i].cmt[m].ten}</b>
                                                <p class="noi-dung-comment">${dt[i].cmt[m].nd}</p>
                                            </div>
                                        </div>`;
                }
                str += `            </div>
                                </div>`;
            }
            $(".main-post").append(str);
        });
}
$(document).ready(function () {
    var page = 0;
    if (page == 0) {
        readMore(page);
        page++;
    }

    $(window).scroll(function () {
        console.log(page);
        var isScrollToBottom = $(window).scrollTop() + $(window).height() == $(document).height();
        if (isScrollToBottom && page != 0) {
            readMore(page);
            page++;
        }
    });
});