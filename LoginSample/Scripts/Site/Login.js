﻿function ValidateUser() {
    var view = document.getElementById("view");
    var userid = $("#SelectedUser").val();
    var password = $("#UserPassword").val();
    var usertype = $("#SelectedGroup").val();
    var url = "/Home/ValidateUser/";
    if (usertype == "Admin" || usertype == "User") {
        dropDErr.style.display = "none";
    }
    else {
        dropDErr.style.display = "block";
        return;
    }

    if (userid.length == 0 || userid.length > 64) {
        view.style.display = "block";
        return;
    }

    $("#btnlogin").val('Plesae wait..');
    $.ajax({
        url: url,
        data: { userId: userid, pass: password },
        cache: false,
        type: "POST",
        success: function (data) {

            if (data == "1") {
                if (usertype == "Admin") {
                    return window.location.href = "/Home/AdminPage";
                }
                else {
                    alert("User page is under process");
                }
            } else {
                view.textContent = " *Wrong password";
                view.style.display = "block";
            }
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
    $("#btnlogin").val('Login');
}