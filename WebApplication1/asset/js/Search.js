$(document).ready(function () {
    $("#khoa").change(function () {
        var khoa = $("#khoa").val();
        $.post("/search/qKhoa", {
            khoa: khoa
        },
            function (data) {
                if (data == "null") return;
                var s = '<option value="all">Tất cả</option>';
                dt = JSON.parse(data);
                for (var i = 0; i < dt.length; i++) {
                    s += '<option value="'+dt[i].ma_nganh+'">'+dt[i].ten_nganh+'</option>';
                }
                $("#nganh").html(s);
            });
    });
    $("#nganh").change(function () {
        var nganh = $("#nganh").val();
        $.post("/search/qNganh", {
            nganh: nganh
        },
            function (data) {
                if (data == "null") return;
                dt = JSON.parse(data);
                var s = '<option value="all">Tất cả</option>';
                for (var i = 0; i < dt.length; i++) {
                    s += '<option value="' + dt[i].ma_lop + '">' + dt[i].ten_lop + '</option>';
                }
                $("#lop").html(s);
            });
    });
    $(".filter").change(function () {
        var searchString = $("#txtInputSearch").val();
        var lop = $("#lop").val();
        var nganh = $("#nganh").val();
        var khoa = $("#khoa").val();
        var gioi_tinh = $("#gender").val();
        var role = $("#role").val();
        //console.log(searchString + lop + nganh + khoa + gioi_tinh + role);
        $.post("/search/reloadSearch",
            {
                searchString: searchString,
                maLop: lop,
                ma_nganh: nganh,
                ma_khoa: khoa,
                gioi_tinh: gioi_tinh,
                role: role
            },
            function (data) {
                if (data == "") return;
                dt = JSON.parse(data);
                var s = "";
                for (var i = 0; i < dt.length; i++) {
                    s += '<div class="col-md-6">';
                    s += '    <div class="a-user">';
                    s += '   <span><img src="asset/images/avatar/' + dt[i].anh +'" style="width:70px; height:70px" /></span>';
                    s += '   <span><a href="profile?id=' + dt[i].uid+'">'+dt[i].ho_ten+'</a></span>';
                    s += '   <div class="mo-ta-ngan">';
                    s += '       ' + dt[i].role + ' ' + dt[i].maLop + ' ngành ' + dt[i].tenNganh + ' khoa ' + dt[i].tenKhoa +'.';
                    s += '   </div>';
                    s += '</div>';
                    s += '</div>';
                }
                $("#sub-user-section").html(s);
            });

    });
});