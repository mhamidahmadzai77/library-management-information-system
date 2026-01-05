
/*document.addEventListener('click', function (event) {
    if (event.target.classList.contains('item')) {
        event.target.parentNode.setAttribute('class', 'active open');
    }
});*/



// Close reset password form error alert 
document.getElementById("closeAlert").addEventListener('click', function () {
    var errorAlertTag = document.getElementById('emptyFieldsAlert');
    errorAlertTag.classList.remove("show");
});

// Close confirm password form error alert 
document.getElementById("closeAlert").addEventListener('click', function () {
    var errorAlertTag = document.getElementById('emptyAlert');
    errorAlertTag.classList.remove("show");
});

// Validation and submition of resetPasswordForm to controller
$(document).ready(function () {
    $("#resetPasswordForm").validate({
        rules: {
            email: {
                required: true,
                email: true
            },
            username: {
                required: true,
                minlength: 3,
                maxlength: 16
            }
        },
        messages: {
            email: {
                required: "ایمیل داخل کړئ.",
                email: "مهرباني وکړئ دقیق ایمیل داخل کړئ."
            },
            username: {
                required: "نوم داخل کړئ.",
                minlength: "نوم باید له ۳ حروفو څخه لوی وي.",
                maxlength: "نوم باید له ۱۶ حروفو څخه کوچنی وي."
            }
        },
        submitHandler: function (form) {
            var username = $('#username').val();
            var email = $('#email').val();
            if (email === "" && username === "") {
                $("#emptyFieldsAlert").removeClass("d-none");
            }
            var obj = {
                username: username,
                email: email
            }

            $.ajax({
                type: "POST",
                url: "/User/ResetPassword",
                data: obj,
                success: function (response) {
                    if (response.redirectTo) {
                        window.location.href = response.redirectTo;
                    }
                    else if (response == "invalideAccount") {
                        swal("نوم یا ایمیل ادرس مو سم ندی، مهرباني وکړئ بیا هڅه وکړئ!","", {
                            icon: "error", buttons: "  سمه ده  "
                        });
                    }
                    else if (response == "password did not change") {
                        swal("د یوې ستونزې له امله مو کوډ بدل نه شو، بیا هڅه وکړئ", "", {
                            icon: "info", buttons: "  سمه ده  "
                        });
                        

                    } else {
                        swal("ستونزه رامنځه شوه", "Exception: " + response.responseText, {
                            icon: "error", buttons: "  سمه ده  "
                        });
                    }

                    
                    $('#username').val('');
                    $('#email').val('');
                    
                    // Optionally, you can redirect or display a success message here
                },
                error: function (xhr, status, error) {
                    swal("ستونزه رامنځه شوه", "Error: " + error, {
                        icon: "error", buttons: "  سمه ده  "
                    });
                }
            });
        }
    });
    // Close the alert when the user starts typing  in the email or username fields
    $('#submitBtn').click(function () {
        var email = $('#email').val();
        var username = $('#username').val();
        if (email == "" && username == "") {
            document.getElementById('emptyFieldsAlert').classList.add("show");
        }
    });
    $("#email,#username").on("input", function () {
        $("#emptyFieldsAlert").addClass("d-none");
    });
});

function showModal() {
    ('#myModal').modal('show');
}


function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

function validateUsername(username) {
    const re = /^[a-zA-Z0-9_-]{3,16}$/;
    return re.test(username);
}






// Validation and submition of confirmPassword to controller
$(document).ready(function () {
    $("#confirmPasswordForm").submit(function (event) {
        event.preventDefault();
        var password = $('#password').val();
        if (password.length != 6) {
            $('#password').val('');
            document.getElementById('emptyAlert').classList.add("show");
        }
        else {
            $.ajax({
                type: "POST",
                url: "/User/ConfirmPassword",
                data: {password: password},
                success: function (response) {
                    if (response == "invalidPassword") {
                        swal("کوم کوډ مو چې داخل کړ سم ندی", "", {
                            icon: "warning", buttons: "  سمه ده  "
                        });
                    }
                    else if (response.redirectTo) {
                        window.location.href = response.redirectTo;
                    }
                    else {
                        swal("ستونزه رامنځه شوه", "Exception: " + response.responseText, {
                            icon: "warning", buttons: "  سمه ده  "
                        });    
                    }
                    $('#password').val('');

                    
                },
                error: function (xhr, status, error) {
                    swal("ستونزه رامنځه شوه", "Error: " + error, {
                        icon: "error", buttons: "  سمه ده  "
                    });
                }
            });
        }
    });
    $("#password").on("input", function () {
        document.getElementById('emptyAlert').classList.remove("show");

    });
});

// Validation and submition of lock screen password to controller
document.getElementById("lockscreenPasswordForm").addEventListener('submit', function (event) {
    event.preventDefault();
        
        var password = $('#lockscreenPassword').val();
        if (password == "") {
            document.getElementById('emptyPasswordError').innerHTML = "کوډ مو داخل کړئ.";
            
        }
        else {
            $.ajax({
                type: "POST",
                url: "/User/UnLock",
                data: {password: password},
                success: function (response) {
                    if (response == "true") {
                        window.history.back();
                    }
                    else if (response == "false") {
                        $('#incorrectPassworModal').modal('show');
                    }
                    $('#lockscreenPassword').val('');

                    
                },
                error: function (xhr, status, error) {
                    console.error("Error:", error);
                }
            });
        }
    });
    $("#lockscreenPassword").on("input", function () {
        document.getElementById('emptyPasswordError').innerHTML = "";

    });






