// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function signout() {
    if (confirm("Chắc thoát không ba? Thoát đó nha!") == true) {
        $.ajax({
            type: "POST",
            url: "/Login/signout",
            async: false,
            success: function (res) {
                if (res.success) {
                    document.location.href = "/";
                }
                else
                    alert(res.message);
            },
            failure: function (res) {

            },
            error: function (res) {

            }
        });
    }
}
function changePass() {
    username = $("#txtUserName").val();
    oldpass = $("#txtOldPass").val();
    pass1 = $("#txtPass1").val();
    pass2 = $("#txtPass2").val();
    if (oldpass == "" || oldpass == undefined || oldpass == null || oldpass.length < 3) {
        alert("Vui Long Nhap Lai Mat Khau Cu!!");
        return;
    }
    if (pass1 == "" || pass1 == undefined || pass1 == null || pass1.length < 3) {
        alert("Vui Long Nhap Lai Mat Khau Moi!!");
        return;
    }
    if (pass2 == "" || pass2 == undefined || pass2 == null || pass2.length < 3) {
        alert("Vui Long Nhap Lai Mat Khau Moi!!");
        return;
    }
    if (pass1 != pass2) {
        alert("Mat Khau Moi Khong Trung Khop!!");
        return;
    }
    if (oldpass == pass1 == pass2) {
        alert("Mat Khau Cu Va Mat Khau Moi Trung Nhau!!");
        return;
    }
    if (confirm("Chắc đổi không ba? Đổi đó nha!") == true) {
        $.ajax({
            type: "POST",
            url: "/Login/change_pass",
            data: { 'username': username, 'oldpass': oldpass, 'newpass': pass1 },
            async: false,
            success: function (res) {
                if (res.success) {
                    document.location.href = "/";
                }
                else
                    alert(res.message);
            },
            failure: function (res) {

            },
            error: function (res) {

            }
        });
    }
}