const { event } = require("jquery");
const { platform } = require("os");
const { monitorEventLoopDelay } = require("perf_hooks");

function showTab(tabId) {
    var tabContent = document.getElementById(tabId);

    const tabsToHide = document.getElementsByClassName('tab-content');

    Array.from(tabsToHide).forEach(element => {
        element.style.display = "none";
    });

    tabContent.style.display = "block";


}


function selectPerson(personId, personFirstname, personLastname) {
    document.getElementById('personId').value = personId;
    document.getElementById('personName').value = personFirstname + " " + personLastname;
    showTab("tab1");
}

function selectPublication(editionId, publicationName) {
    document.getElementById('editionId').value = editionId;
    document.getElementById('publicationName').value = publicationName;
    showTab("tab1");

}

function saveDealing() {
    var person_id =  document.getElementById("personId").value;
    var edition_id = document.getElementById("editionId").value;
    var issue_date = document.getElementById("issueDate").value;
    var return_date = document.getElementById("returnDate").value;
    var paid_money = document.getElementById("paidMoney").value;
    var availableQuantityId = "availableQuantityId" + edition_id;
    
    if (person_id == "") {
        document.getElementById("personName").style.border = "1px solid red";
        document.getElementById("personName").nextElementSibling.style.color = "red";
    }
    if (edition_id == "") {
        document.getElementById("publicationName").style.border = "1px solid red";
        document.getElementById("publicationName").nextElementSibling.style.color = "red";
    }
    if (issue_date == "") {
        document.getElementById("issueDate").style.border = "1px solid red";
        document.getElementById("issueDate").nextElementSibling.style.color = "red";
    }
    if (return_date == "") {
        document.getElementById("returnDate").style.border = "1px solid red";
        document.getElementById("returnDate").nextElementSibling.style.color = "red";
    }

    if (person_id == "" || edition_id == "" || issue_date == "" || return_date == "") {
        scrollToTop();
    }
    else {

        var dealing = {
            person_id: person_id,
            edition_id: edition_id,
            issue_date: issue_date,
            return_date: return_date,
            paid_money: paid_money,
            returned: false
        }

        $.ajax({
            type: "POST",
            url: "/Dealing/DealingRegistration",
            data: { dealing: dealing },
            success: function (response) {

                if (response == true) {
                    swal("په بریالیتوب سره ثبت شو", "", {
                        icon: "success", buttons: "  سمه ده  "
                    });

                    document.getElementById("personId").value = "";
                    document.getElementById("editionId").value = "";
                    document.getElementById("personName").value = "";
                    document.getElementById("publicationName").value = "";
                    document.getElementById("paidMoney").value = "";

                    document.getElementById("personName").style.border = "";
                    document.getElementById("personName").nextElementSibling.style.color = "";
                    document.getElementById("publicationName").style.border = "";
                    document.getElementById("publicationName").nextElementSibling.style.color = "";
                    document.getElementById("issueDate").style.border = "";
                    document.getElementById("issueDate").nextElementSibling.style.color = "";
                    document.getElementById("returnDate").style.border = "";
                    document.getElementById("returnDate").nextElementSibling.style.color = "";
                    document.getElementById("returnDate").style.border = "";
                    document.getElementById("returnDate").nextElementSibling.style.color = "";

                    var availableQuantity = document.getElementById(availableQuantityId).innerHTML;
                    availableQuantity = parseInt(availableQuantity);
                    document.getElementById(availableQuantityId).innerHTML = availableQuantity - 1;
                    if (availableQuantity - 1 == 0) {
                        $("#btnSelectId" + edition_id).attr("disabled", false);
                    }
                }
                else if (response == false) {
                    swal("د یوې ستونزې له امله مو معلومات ثبت نشول، مهرباني وکړئ بیا هڅه وکړئ", "", {
                        icon: "error", buttons: "  سمه ده  "
                    });
                }
                else {
                    swal("ستونزه رامنځه شوه", "Exception: " + response.responseText, {
                        icon: "info", buttons: "  سمه ده  "
                    });
                }

            },
            error: function (xhr, status, error) {
                swal("ستونزه رامنځه شوه", "Error: " + error, {
                    icon: "error", buttons: "  سمه ده  "
                });
            }
        });

    }

}


function bookReturned(dealing_id, disableButtonId) {
    swal({
        title: "ایا ډاډه یاست؟",
        text: "",
        icon: "warning",
        buttons: [" نه ", "  هـــو  "]
        ,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                // If user clicked yes button
                
                $.ajax({
                    type: 'POST',
                    url: '/Dealing/bookReturned',
                    data: { dealing_id: dealing_id },
                    success: function (response) {
                        if (response == true) {
                            
                            $("#" + disableButtonId).attr("disabled", fal);
                        }
                        else if (response == false) {
                            swal("د یوې ستونزې له امله ستاسو غوښتنې صورت ونه موند.", "", {
                                icon: "error", buttons: "سمه ده"
                            });
                        }
                        else {
                            swal("ستونزه رامنځته شوه!", "Exception: " + response, {
                                icon: "error", buttons: "سمه ده"
                            });
                        }


                    },
                    error: function (error) {
                        swal("ستونزه رامنځته شوه!", "Error: " + error.responseText, {
                            icon: "error", buttons: "سمه ده"
                        });

                    }
                });




            }
        });
}


function scrollToTop() {
    const scrollDuration = 500; // Duration of the scroll animation in milliseconds
    const scrollStep = -window.scrollY / (scrollDuration / 15); // Step size for each iteration

    const scrollInterval = setInterval(function () {
        if (window.scrollY !== 0) {
            window.scrollBy(0, scrollStep); // Scroll by the step size
        } else {
            clearInterval(scrollInterval); // Stop the interval when at the top of the page
        }
    }, 15); // Interval time for smooth animation
}


function scaleDeleteButton(id) {
    document.getElementById("btnDelete" + id).style.transform = "scale(1.2,1.2)";
}
function scaleEditButton(id) {
    document.getElementById("btnEdit" + id).style.transform = "scale(1.2,1.2)";
}
function scaleDeleteButtonBack(id) {
    document.getElementById("btnDelete" + id).style.transform = "scale(1,1)";
}
function scaleEditButtonBack(id) {
    document.getElementById("btnEdit" + id).style.transform = "scale(1,1)";
}





