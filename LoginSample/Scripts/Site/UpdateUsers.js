
// Button display
var defUserName;
var selectedRows = [];
var selectedIndex = [];
var editor = document.getElementById("EditUser");
function HideButtons() {
    if ($('.isCheck:checked').length == 1) {
        document.getElementById("DelUser").disabled = false;
        document.getElementById("EditUser").disabled = false;
    }
    else if ($('.isCheck:checked').length > 1) {
        document.getElementById("DelUser").disabled = false;
        document.getElementById("EditUser").disabled = true;
        
    }
    else {
        document.getElementById("DelUser").disabled = true;
        document.getElementById("EditUser").disabled = true;      
    }
}


// Check box 
$(document).ready(function () {

    $('#display-table').DataTable({
        'columnDefs': [{
            orderable: false,
            searchable: false,
            scrolly: '200px',
            scrollCollapse: true,
            className: 'select-checkboxes',
            targets: 0,
            'className': '',
            'render': function (data, type, full, meta) {
                return '<input type="checkbox" class="isCheck">';
            }
        }],
        'order': [[1, 'asc']]

    });
    var table = $('#display-table').DataTable();
    var tblData = document.getElementById("display-table");
    var chks = tblData.getElementsByClassName("isCheck");

    // Multiple check or all selection
    $('#all').click(function (e) {
        selectedRows = [];
        selectedIndex = [];
        $('#display-table tbody :checkbox').prop('checked', $(this).is(':checked'));
        HideButtons();
        for (var i = 0; i < chks.length; i++) {
            if (chks[i].checked) {
                var data = table.row($(chks[i]).closest('tr')).data();
                selectedRows.push(data[1]);
                selectedIndex.push(data[3]);
                console.log("hello");
                console.log(data[3]);
            }
        }
    });

    $('.isCheck').change(function () {
        HideButtons();
    });

    // Manual selection
    $('.isCheck').on('change', function () {
        selectedRows = [];
        selectedIndex = [];
        for (var i = 0; i < chks.length; i++) {
            if (chks[i].checked) {
                var data = table.row($(chks[i]).closest('tr')).data();
                selectedRows.push(data[1]);
                selectedIndex.push(data[3]);
                var editdata = [];
                editdata.push(data[1], data[2]);
                for (var un = 0; un < data.length; un++) {
                    // selectedRows.push(data[0]);
                }
            }
        }
    });

    // Delete selected users
    $('#DelUser').click(function () {
        var url = "/Home/DeleteUser/";
        console.log(selectedRows);
        let text = "Are you sure you want to delete selected user(s) ?";
        if (confirm(text) == true) {
            $.ajax({
                url: url,
                data: { userid: selectedRows },
                cache: false,
                type: "POST",
                success: function (data) {
                    alert("succefully Deleted");
                    return window.location.href = "/Home/AdminPage";
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                }
            });
        } else {
            text = "You canceled!";
        }
    });  
});

// CURD buttons
function display() {
    var viewPopup = document.getElementById("ADDuser");
    viewPopup.style.display = 'block';
    viewPopup.style.animation = "fadeIn 1s"
}
function hide() {
    var viewPopup = document.getElementById("ADDuser");
    viewPopup.style.display = "none";
}
function EditThisUser() {
    var viewPopup = document.getElementById("userEdit");
    document.addEventListener("keydown", function (event) {
        if (event.key === "Enter") {
            event.preventDefault(); // Prevent form submission
            UpdateSelectedUser() // Trigger the form submission
        }
    });

    // Edit user details
    var url = "/Home/GetThisUser/";
    $.ajax({
        url: url,
        data: { uid: selectedIndex },
        cache: false,
        type: "POST",
        dataType: "JSON",
        success: function (data) {
            console.log('india');
            $.each(data, function (index, model) {
                var selname = document.getElementById("EditUserName").value;
                if (model == false) {
                    viewPopup.style.display = "none";
                    alert(data.error);
                    return window.location.href = "/Home/AdminPage";
                }
                else {
                    defUserName = model.SelectedUser;
                    console.log(model.SelectedUser);
                    document.getElementById("EditUserName").value = model.SelectedUser;
                    document.getElementById("EditUserEmail").value = model.UserEmail;
                    document.getElementById("EditUserPassword").value = model.UserPassword;
                    viewPopup.style.display = 'block';
                    viewPopup.style.animation = "fadeIn 1s"
                }
            });
        },
        error: function (reponse) {
            alert("error : " + reponse);
        }
    });
}
function hideEdituser() {
    var viewPopup = document.getElementById("userEdit");
    viewPopup.style.display = "none";

}

// To edit details of existing user
function UpdateSelectedUser() {
    var viewPopup = document.getElementById("userEdit");
    // Userid label
    var usern = document.getElementById("EditErr");
    // user password label
    var upass = document.getElementById("EditErr1");
    // user email label
    var uEmail = document.getElementById("EditErr2");
    var userid = $("#EditUserName").val();
    var password = $("#EditUserPassword").val();
    var uemail = $("#EditUserEmail").val();
    var url = "/Home/UpdateSelectedUser/";
    var emailset = /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/;
    var regex = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,64})");
    userid = userid.trim();
    if (userid.length < 3 || userid == "") {
        usern.textContent = "*Required"
        viewPopup.style.animation = "shake 0.5s";
        usern.style.display = "block";
        return;
    }
    if (userid.length > 20) {
        usern.textContent = "*Oops! Username conatains max 20 char!";
        viewPopup.style.animation = "shake 0.5s";
        usern.style.display = "block";
        return;
    }
    if (userid.length > 3) {
        usern.style.display = "none";
    }
    if (password == "") {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        upass.textContent = "*Required";
        return;
    }
    if (!password.match(regex)) {
        viewPopup.style.animation = "shake 0.5s";
        upass.style.display = "block";
        upass.textContent = "*Oops! Too weak password ! for refrence Ex: Acty@1947";
        return;
    }
    if (password.match(regex)) {
        upass.style.display = "none";
    }
    if (!emailset.test(uemail)) {
        viewPopup.style.animation = "shake 0.5s";
        uEmail.style.display = "block";
        uEmail.textContent = "*Email should be like: Acty@gmail.com";
        return;
    }
    else {
        $.ajax({
            url: url,
            data: { uid: selectedIndex, SelectedUname: defUserName, newUname: userid, newPassword: password, newEmail: uemail },
            cache: false,
            type: "POST",
            success: function (data) {
                if (data == 1) {
                    alert("Data Updated successfully");
                    return window.location.href = "/Home/AdminPage";
                }
                else {
                    alert("Such username already exist change username")
                }
            },
            error: function (reponse) {
                alert("error : " + reponse);
            }
        });
    }
}