document.getElementById("EditUserInfo").addEventListener('submit', function (event) {
    event.preventDefault();
    
    var status = document.getElementById("userStatus");
    var isChecked = status.checked;
    var status = "";
    if (isChecked) {
        status = "active";
    }
    else {
        status = "inactive";
    }

    var user_id = $('#userId').val();
    var username = $('#username').val();
    var email = $('#email').val();
    var level = $('#level').val();
    
    if ((username == "") || (username.length > 16 || username.length < 3)) {
        document.getElementById('username').style.border = "1px solid red";
        document.getElementById('usernameText').innerHTML = "نوم مو داخل کړئ.";
        if (username.length > 16 || username.length < 3) {
            document.getElementById('usernameText').innerHTML = "نوم باید له ۳ حروفو څخه کوچنی او له ۱۶ حروفو څخه لوی نه وي";
        }
    }
    if (email == "") {
        document.getElementById('email').style.border = "1px solid red";
        document.getElementById('emailError').innerHTML = "ایمیل داخل کړئ.";
    }
    else if (!(validateEmail(email))) {
        
        document.getElementById('email').style.border = "1px solid red";
        document.getElementById('emailError').innerHTML = "مهرباني وکړئ دقیق ایمیل داخل کړئ.";
    }
    
    if ((username != "" && email != "") && (username.length > 3 && username.length < 17) && (validateEmail(email))) {

        var obj = {
            user_id: user_id,
            username: username,
            email: email,
            level: level,
            status: status
        };
    
        $.ajax({
            type: "POST",
            url: "/User/EditUserInfo",
            data: obj,
            success: function (response) {
                if (response.redirectTo) {
                    window.location.href = response.redirectTo;
                }
                else if (response == false) {
                    swal("خبرتیا", "تاسو په حساب کې تغیرات ندي راوستي!" ,{
                        icon: "info", buttons: "سمه ده"
                    });

                    swal({
                        title: "خبرتیا",
                        text: "تاسو په حساب کې تغیرات رانه وستل!",
                        icon: "info",
                        buttons: "سمه ده"
                        ,
                        infoMode: true,
                    })
                        .then((willDelete) => {
                            if (willDelete) {
                                // If user clicked yes button then go to userview page
                                window.location.href = "/User/userview";

                            }
                        });

                }


            },
            error: function (xhr, status, error) {
                swal("ستونزه رامنځته شوه!", "Error: " + error.responseText, {
                    icon: "error", buttons: "سمه ده"
                });

            }
        });
    }

});


function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}


