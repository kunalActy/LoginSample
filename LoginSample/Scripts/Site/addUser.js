// Add new user
function AddNewUser() {
    var viewPopup = document.getElementById("ADDuser");
    var userid = $("#NewUser").val();
    var password = $("#NewUserPassword").val();
    var userEmail = $("#NewUserEmail").val();
    var usern = document.getElementById("dropDErr");
    var upass = document.getElementById("dropDErr1");
    var uEmail = document.getElementById("dropDErr2");
    var url = "/Home/AddNewUser/";

    $("#btnUpdate").val('Plesae wait..');
    var emailset = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,64})");
    userid = userid.trim();
    if (userid.length < 3 || userid == "") {
        viewPopup.style.animation = "shake 0.5s";
        usern.style.display = "block";
        return;
    }
    if (userid.length > 20) {
        viewPopup.style.animation = "shake 0.5s";
        usern.style.display = "block";
        return;
    }
    if (userid.length > 3) {

        usern.style.display = "none";
    }
    if (!password.match(regex)) {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        return;
    }
    if (password.match(regex)) {
        upass.style.display = "none";
    }
    if (!emailset.test(userEmail)) {
        viewPopup.style.animation = "shake 0.5s";
        uEmail.style.display = "block";
        return;
    }
    else {
        $.ajax({
            url: url,
            data: { userId: userid, pass: password, userEmail: userEmail },
            cache: false,
            type: "POST",
            success: function (data) {
                if (data == 1) {
                    alert("succefully entered");
                    return window.location.href = "/Home/AdminPage";
                }
                else {
                    alert("Such user already there");
                }
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
        $("#btnUpadate").val('Add');
    }
}