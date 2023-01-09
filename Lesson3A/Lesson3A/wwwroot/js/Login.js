function login() {
    let uid = $("#txtUsername").val();
    let pwd = $("#txtPassword").val();
    console.log("Username:", uid);
    console.log("Password:", pwd);
    let s = check(uid, pwd);
    if (s == "") {
        //do login
        let datalogin = { "username": uid, "password": pwd };
        $.ajax({
            type: "POST",
            url: "/Login/doLogin",
            data: {
                "Login": datalogin
            },
            async: false,
            success: function (res) {
                if (res.success) {
                    let usr = res.user;
                    alert("Xin Chao: " + usr.fullname);
                    document.location.href = "/Home";
                } else
                    alert(res.message); 
            },
            failure: function (res) {
                
            },
            error: function (res) {

            }
        })
    }
    else {
        alert(s);
    }
}

function check(email, pass) {
    var s = "";
    if (email == "" || email == undefined || email == null)
        s += "Chua nhap email!   ";
    if (pass == "" || pass == undefined || pass == null)
        s += "Chua nhap pass!";
    return s;
}

// Example starter JavaScript for disabling form submissions if there are invalid fields
(function () {
    'use strict'

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('.needs-validation')

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms)
        .forEach(function (form) {
            form.addEventListener('submit', function (event) {
                if (!form.checkValidity()) {
                    event.preventDefault()
                    event.stopPropagation()
                }

                form.classList.add('was-validated')
            }, false)
        })
})()