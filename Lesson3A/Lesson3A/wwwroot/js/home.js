var totalPage = 1;
var lst = null;
function selectclass(ctl) {
    let lop = $(ctl).val();
    if (lop != "0" && lop != undefined && lop != null) {
        getCourse(lop, 1);
        $("#tblResult").show(500);
    }
    else {
        $("#tblResult").hide(500);
    }
}
function getCourse(grp, p) {
    $.ajax({
        type: "POST",
        url: "/Home/get_course",
        data: { 'Group': grp, 'Page': p, 'Size': 5 },
        async: false,
        success: function (res) {
            if (res.success) {
                let data = res.data;
                if (data.data != null && data.data != undefined) {
                    let stt = (p - 1) * 5 + 1;
                    let data1 = [];
                    for (var i = 0; i < data.data.length; i++) {
                        let item = data.data[i];
                        item.STT = stt;
                        data1.push(item);
                        stt++;
                    }
                    lst = data1;
                    $("#tblResult tbody").html("");
                    $("#courseTemplate").tmpl(data1).appendTo("#tblResult tbody");
                }
                totalPage = data.totalPage;
                $("#curPage").text(p);
            }
            else
                alert(res.message);
        },
        failure: function (res) { },
        error: function (res) { }
    });
}

function goPrev() {
    var curPage = parseInt($("#curPage").text());
    if (curPage == 1)
        alert("Dang o trang dau tien!!");
    else {
        var p = curPage - 1;
        var grp = $("#selClass").val();
        getCourse(grp, p);
    }
}

function goNext() {
    var curPage = parseInt($("#curPage").text());
    if (curPage == totalPage)
        alert("Dang o trang cuoi!!");
    else {
        var p = curPage + 1;
        var grp = $("#selClass").val();
        getCourse(grp, p);
    }
}

function openModal(id) {
    $("#btnSave").show();
    $("#btnInsert").hide();

    if (lst != null && id != null && id > 0) {
        var item = $.grep(lst, function (obj) {
            return obj.id == id;
        })[0];

        $("#txtId").val(item.id);
        $("#txtName").val(item.courseName);
        $("#txtCredit").val(item.credit);
        $("#txtNote").val(item.note);
        $("#txtGroup").val(item.group);
        $("#txtMajor").val(item.major);
        $("#txtCode").val(item.subCode);
    }
}

function save() {
    var item = {
        id: $("#txtId").val(),
        courseName: $("#txtName").val(),
        group: $("#txtGroup").val(),
        credit: $("#txtCredit").val(),
        subCode: $("#txtCode").val(),
        major: $("#txtMajor").val(),
        note: $("#txtNote").val(),
    };
    $.ajax({
        type: "POST",
        url: "/Home/update_course",
        data: { 'course': item },
        async: false,
        success: function (res) {
            if (res.success) {
                alert("Update success!!")
                let c = res.data;
                var i;
                for (i = 0; i < lst.length; i++) {
                    if (lst[i].id == c.id) {
                        c.stt = lst[i].stt;
                        break;
                    }
                }
                lst[i] = c;
                $("#txtResult tbody").html("");
                $("#txtcourseTemplate").tmpl(data).appendTo("#tblResult tbody");
            } else
                alert(res.message);
        },
        failure: function (res) { },
        error: function (res) { }
    });
}
function addNew() {

    $("#btnSave").hide();
    $("#btnInsert").show();

    $("#txtId").val("");
    $("#txtName").val("");
    $("#txtCredit").val("");
    $("#txtNote").val("");
    $("#txtGroup").val($("#selClass").val());
    $("#txtMajor").val("");
    $("#txtCode").val("");
}

function Insert() {
    var item = {
        id: 0,
        courseName: $("#txtName").val(),
        group: $("#txtGroup").val(),
        credit: $("#txtCredit").val(),
        subCode: $("#txtCode").val(),
        major: $("#txtMajor").val(),
        note: $("#txtNote").val(),
    };
    $.ajax({
        type: "POST",
        url: "/Home/insert_course",
        data: { 'course': item },
        async: false,
        success: function (res) {
            if (res.success) {
                alert("Insert success!!")
                let c = res.data;
                $("#txtId").val(c.id);
                var grp = $("#selClass").val();
                getCourse(grp, 1);
            }
            else
                alert(res.message);
        },
        failure: function (res) { },
        error: function (res) { }
    });
}
function deleteCourse(id) {
    if (confirm("Xoa Cai Nay Hong Ba?") == false)
        return;
    if (id != null && id != undefined && id > 0) {
        $.ajax({
            type: "POST",
            url: "/Home/delete_course",
            data: { 'id': id },
            async: false,
            success: function (res) {
                if (res.success) {
                    alert("Delete success!!")
                    var grp = $("#selClass").val();
                    var page = parseInt($("#curPage").text());
                    getCourse(grp, page);
                }
                else
                    alert(res.message);
            },
            failure: function (res) { },
            error: function (res) { }
        });
    }
}