

// Validation and submition of lock screen password to controller
document.getElementById("EditPersonalInfoForm").addEventListener('submit', function (event) {
    event.preventDefault();

    var username = $('#username').val();
    var email = $('#email').val();

    if ((username == "") || (username.length > 16 || username.length < 3) ) {
        document.getElementById('username').style.border = "1px solid red";
        document.getElementById('usernameText').innerHTML = "نوم مو داخل کړئ.";
        if (username.length > 16 || username.length < 3) {
            document.getElementById('usernameText').innerHTML = "نوم باید له ۳ حروفو څخه کوچنی او له ۱۶ حروفو څخه لوی نه وي";
        }
    }
   
    if (email == "") {
        document.getElementById('email').style.border = "1px solid red";
        if (validateEmail(email)) {
            document.getElementById('emailError').innerHTML = "";
        }
        else {
            document.getElementById('emailError').innerHTML = "مهرباني وکړئ دقیق ایمیل داخل کړئ.";
        }
    }
    if ((username != "" && email != "") && (username.length > 3 && username.length < 16) && (validateEmail(email))) {
        var obj = {
            username: username,
            email: email
        };
        $.ajax({
            type: "POST",
            url: "/User/EditPersonalInfo",
            data: obj,
            success: function (response) {
                if (response.redirectTo) {
                    window.location.href = response.redirectTo;
                }
                else if (response.type === "string") {
                    $('#errorModal').modal('show');
                }


            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    }
});
$("#username").on("input", function () {
    var usernameText = $('#username').val();
    if (usernameText == "") {
        document.getElementById('username').style.border = "1px solid red";
        document.getElementById('usernameError').innerHTML = "نوم مو داخل کړئ.";
        
    }
    else {
        document.getElementById('username').style.border = "";
        if (usernameText.length > 16 || usernameText.length < 3) {
            document.getElementById('usernameError').innerHTML = "نوم باید له ۳ حروفو څخه کوچنی او له ۱۶ حروفو څخه لوی نه وي";
        }
        else {
            document.getElementById('usernameError').innerHTML = "";
        }
    }
});
$("#email").on("input", function () {
    var emailText = $('#email').val();
    if (emailText != "") {
        document.getElementById('email').style.border = "";
        if (!(validateEmail(emailText))) {
            document.getElementById('emailError').innerHTML = "مهرباني وکړئ دقیق ایمیل داخل کړئ.";
        }
        else {
            document.getElementById('emailError').innerHTML = "";
        }
    }
    else if (emailText == "") {
        document.getElementById('email').style.border = "1px solid red";
        document.getElementById('emailError').innerHTML = "ایمیل مو داخل کړئ.";
    }
});


function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}





