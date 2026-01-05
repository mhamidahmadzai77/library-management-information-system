
function saveEdit() {
    
    var serialNo = document.getElementById('updateSerialNo').value;
    var editionId = document.getElementById('updateEditionId').value;
    var segmentId = document.getElementById('updateSegmentId').value;
    var editionNo = document.getElementById('updateEditionNo').value;
    var segmentNo = document.getElementById('updateSegmentNo').value;
    var publicationQuantity = document.getElementById('updatePublicationQuantity').value;
    var publicationPages = document.getElementById('updatePublicationPages').value;
    var CDQuantity = document.getElementById('updateCDQuantity').value;
    var registrationDateType = document.getElementById('updateRegistrationDateType').value;
    var registrationYear = document.getElementById('updateRegistrationYear').value;
    var registrationMonth = document.getElementById('updateRegistrationMonth').value;
    var registrationDay = document.getElementById('updateRegistrationDay').value;
    var publicationDateType = document.getElementById('updatePublicationDateType').value;
    var publicationYear = document.getElementById('updatePublicationYear').value;
    var publicationMonth = document.getElementById('updatePublicationMonth').value;
    var publicationDay = document.getElementById('updatePublicationDay').value;
    var cupboardNo = document.getElementById('updateCupboardNo').value;
    var cellNo = document.getElementById('updateCellNo').value;



    if (segmentNo < 1 || segmentNo == null) {
        document.getElementById('updateSegmentNo').style.border = '1px solid red';
        document.getElementById('updateSegmentNo').nextElementSibling.style.color = "red";

    }
    if (editionNo < 1 || editionNo == null) {
        document.getElementById('updateEditionNo').style.border = '1px solid red';
        document.getElementById('updateEditionNo').nextElementSibling.style.color = "red";
    }
    
    if (publicationQuantity < 1 || publicationQuantity == null) {
        document.getElementById('updatePublicationQuantity').style.border = '1px solid red';
        document.getElementById('updatePublicationQuantity').nextElementSibling.style.color = "red";
    }
    if (publicationPages < 1 || publicationPages == null) {
        document.getElementById('updatePublicationPages').style.border = '1px solid red';
        document.getElementById('updatePublicationPages').nextElementSibling.style.color = "red";
    }
    if (publicationYear < 1 || publicationYear == null) {
        document.getElementById('updatePublicationYear').style.border = '1px solid red';
        document.getElementById('updatePublicationYear').nextElementSibling.style.color = "red";
    }
    if (registrationYear < 1 || registrationYear == null) {
        document.getElementById('updateRegistrationYear').style.border = '1px solid red';
        document.getElementById('updateRegistrationYear').nextElementSibling.style.color = "red";
    }
    if (registrationMonth < 1 || registrationMonth == null) {
        document.getElementById('updateRegistrationMonth').style.border = '1px solid red';
        document.getElementById('updateRegistrationMonth').nextElementSibling.style.color = "red";
    }
    if (registrationDay < 1 || registrationDay == null) {
        document.getElementById('updateRegistrationDay').style.border = '1px solid red';
        document.getElementById('updateRegistrationDay').nextElementSibling.style.color = "red";
    }
    // Display alert if form was not validate
    if (editionNo < 1 || editionNo == null || segmentNo < 1 || publicationQuantity < 1 || publicationPages < 10 || publicationYear == 0 || registrationYear == 0 || registrationMonth == 0 || registrationDay == 0) {
        document.getElementById('alert-danger').style.display = "block";

        // Call the function to scroll to the top with animation
        scrollToTop();

    }
    else {
        document.getElementById('alert-danger').style.display = "none";

        var currentSegmentNo = parseInt(document.getElementById('TDSegmentNo' + editionId).innerHTML);
        var currentEditionNo = parseInt(document.getElementById('TDEditionNo' + editionId).innerHTML);


        
        var segment = {
            s_no: serialNo,
            segment_id: segmentId,
            segment_no: segmentNo
        };
        var edition = {
            edition_id: editionId,
            segment_id: segmentId,
            edition_no: editionNo,
            publication_quantity: publicationQuantity,
            publication_pages: publicationPages,
            cd_quantity: CDQuantity,
            registration_date_type: registrationDateType,
            registration_year: registrationYear,
            registration_month: registrationMonth,
            registration_day: registrationDay,
            publication_date_type: publicationDateType,
            publication_year: publicationYear,
            publication_month: publicationMonth,
            publication_day: publicationDay,
            cupboard_no: cupboardNo,
            cell_no: cellNo
        };

        $.ajax({
            type: "POST",
            url: "/Monograph/EditEditionAndSegment",
            data: { segment: segment, edition: edition, currentSegmentNo: currentSegmentNo, currentEditionNo: currentEditionNo },
            success: function (response) {

                if (response == "updated") {
                    swal("په بریالیتوب سره تغیرات ثبت شول", "", {
                        icon: "success", buttons: "  سمه ده  "
                    });
                    document.getElementById('TDPublicationPages' + editionId).innerHTML =
                        // Update HTML table 
                        document.getElementById('TDSegmentNo' + editionId).innerHTML = document.getElementById('updateSegmentNo').value;
                    document.getElementById('TDEditionNo' + editionId).innerHTML = document.getElementById('updateEditionNo').value;
                    document.getElementById('TDPublicationQuantity' + editionId).innerHTML = document.getElementById('updatePublicationQuantity').value;
                    document.getElementById('TDPublicationPages' + editionId).innerHTML = document.getElementById('updatePublicationPages').value;
                    //document.getElementById('TDAvailableQuantity' + editionId).innerHTML = 
                    document.getElementById('TDCDQuantity' + editionId).innerHTML = document.getElementById('updateCDQuantity').value;
                    document.getElementById('TDRegistrationDateType' + editionId).innerHTML = document.getElementById('updateRegistrationDateType').value;
                    document.getElementById('TDRegistrationYear' + editionId).innerHTML = document.getElementById('updateRegistrationYear').value;
                    document.getElementById('TDRegistrationMonth' + editionId).innerHTML = document.getElementById('updateRegistrationMonth').value;
                    document.getElementById('TDRegistrationDay' + editionId).innerHTML = document.getElementById('updateRegistrationDay').value;
                    document.getElementById('TDPublicationDateType' + editionId).innerHTML = document.getElementById('updatePublicationDateType').value;
                    document.getElementById('TDPublicationYear' + editionId).innerHTML = document.getElementById('updatePublicationYear').value;
                    document.getElementById('TDPublicationMonth' + editionId).innerHTML = document.getElementById('updatePublicationMonth').value;
                    document.getElementById('TDPublicationDay' + editionId).innerHTML = document.getElementById('updatePublicationDay').value;
                    document.getElementById('TDCupboardNo' + editionId).innerHTML = document.getElementById('updateCupboardNo').value;
                    document.getElementById('TDCellNo' + editionId).innerHTML = document.getElementById('updateCellNo').value;

                    // Go to edit view
                    showTab('tab1');
                }
                else if (response == "edition exists") {
                    swal(editionNo + " نمبر چاپ ډېټابېس کې له مخکې نه موجود دی", "", {
                        icon: "error", buttons: "  سمه ده  "
                    });
                }
                else if (response == "segment exists") {
                    swal(segmentNo + " نمبر جلد ډېټابېس کې له مخکې نه موجود دی", "", {
                        icon: "error", buttons: "  سمه ده  "
                    });

                }
                else {
                    swal("ستونزه رامنځه شوه", "Exception: " + response.responseText, {
                        icon: "info", buttons: "  سمه ده  "
                    });
                }

                // Optionally, you can redirect or display a success message here
            },
            error: function (xhr, status, error) {
                swal("ستونزه رامنځه شوه", "Error: " + error, {
                    icon: "error", buttons: "  سمه ده  "
                });
            }
        });
    }



}

function savePublicationEdit() {

    var s_no = $('#s_no').val();
    var ISBN = $('#ISBN').val();
    var DDC = $('#DDC').val();
    var LLC = $('#LLC').val();
    var name = $('#name').val();
    var translator = $('#translator').val();
    var branch = $('#branch').val();
    var language = $('#language').val();
    var description = $('#description').val();
    var monographAndThesisId = $('#monographAndThesisId').val();
    var supervisorFirstname = $('#supervisorFirstname').val();
    var supervisorLastname = $('#supervisorLastname').val();
    var studentRegistrationDate = $('#studentRegistrationDate').val();
    var studentGraduationDate = $('#studentGraduationDate').val();
    var defeceDate = $('#defeceDate').val();
    var mark = $('#mark').val();
    var studentGraduationPeriod = $('#studentGraduationPeriod').val();
 

    var permission = true;
    if (document.getElementById("referenceBook").checked) {
        permission = false;
    }
    else {
        permission = true;
    }


    var authors = $('#select2-disabled-inputs-multiple').val();

    if (name < 1 || name == null) {
        document.getElementById('name').style.border = '1px solid red';
        document.getElementById('name').nextElementSibling.style.color = "red";
    }

    if (authors == "" || authors == null) {

        document.getElementById('lableAuthor').style.color = "red";
    }

    if (supervisorFirstname == "" || supervisorFirstname == null) {

        document.getElementById('supervisorFirstname').style.color = "red";
        document.getElementById('supervisorFirstname').nextElementSibling.style.color = "red";
    }

   

    if (name == "" || authors == "" || supervisorFirstname == "" ) {

        document.getElementById('alert-danger').style.display = "block";

        // Call the function to scroll to the top with animation
        scrollToTop();
    }
    else {
        document.getElementById('alert-danger').style.display = "none";

        var bookProperties = {
            s_no: s_no,
            ISBN: ISBN,
            DDC_classificationNo: DDC,
            LLC_classificationNo: LLC,
            publication_name: name,
            translator: translator,
            branch_id: branch,
            publication_language: language,
            publication_description: description,
            permission: permission
        };

        var monographAndThesis = {
            monograph_thesis_id: monographAndThesisId,
            supervisor_firstname: supervisorFirstname,
            supervisor_lastname: supervisorLastname,
            student_registration_year: studentRegistrationDate,
            student_graduation_year: studentGraduationDate,
            defence_data: defeceDate,
            mark: mark,
            graduation_period: studentGraduationPeriod
        };

        

        $.ajax({
            type: "POST",
            url: "/Monograph/EditPublication",
            data: { bookProperties: bookProperties, monographAndThesis: monographAndThesis, authors: authors },
            success: function (response) {

                if (response.redirectTo) {
                    window.location.href = response.redirectTo;
                }
                else {
                    swal("ستونزه رامنځه شوه", "Exception: " + response.responseText, {
                        icon: "info", buttons: "  سمه ده  "
                    });
                }

                // Optionally, you can redirect or display a success message here
            },
            error: function (xhr, status, error) {
                swal("ستونزه رامنځه شوه", "Error: " + error, {
                    icon: "error", buttons: "  سمه ده  "
                });
            }
        });


    }



}


function addSegment() {

    var serialNo = document.getElementById('addSerialNo').value;
    var segmentNo = document.getElementById('addSegmentNo').value;
    var editionNo = document.getElementById('addEditionNo').value;
    var publicationQuantity = document.getElementById('addPublicationQuantity').value;
    var publicationPages = document.getElementById('addPublicationPages').value;
    var CDQuantity = document.getElementById('addCDQuantity').value;
    var registrationDateType = document.getElementById('addRegistrationDateType').value;
    var registrationDate = document.getElementById('addRegistrationDate').value;
    var publicationDateType = document.getElementById('addPublicationDateType').value;
    var publicationYear = document.getElementById('addPublicationYear').value;
    var publicationMonth = document.getElementById('addPublicationMonth').value;
    var publicationDay = document.getElementById('addPublicationDay').value;
    var cupboardNo = document.getElementById('addCupboardNo').value;
    var cellNo = document.getElementById('addCellNo').value;
    

    if (segmentNo < 1 || segmentNo == null) {
        document.getElementById('addSegmentNo').style.border = '1px solid red';
        document.getElementById('addSegmentNo').nextElementSibling.style.color = "red";

    }
    if (editionNo < 1 || editionNo == null) {
        document.getElementById('addEditionNo').style.border = '1px solid red';
        document.getElementById('addEditionNo').nextElementSibling.style.color = "red";
    }
    
    if (publicationQuantity < 1 || publicationQuantity == null) {
        document.getElementById('addPublicationQuantity').style.border = '1px solid red';
        document.getElementById('addPublicationQuantity').nextElementSibling.style.color = "red";
    }
    if (publicationPages < 1 || publicationPages == null) {
        document.getElementById('addPublicationPages').style.border = '1px solid red';
        document.getElementById('addPublicationPages').nextElementSibling.style.color = "red";
    }
    if (publicationYear < 1 || publicationYear == null) {
        document.getElementById('addPublicationYear').style.border = '1px solid red';
        document.getElementById('addPublicationYear').nextElementSibling.style.color = "red";
    }
    if (registrationDate < 1 || registrationDate == null) {
        document.getElementById('addRegistrationDate').style.border = '1px solid red';
        document.getElementById('addRegistrationDate').nextElementSibling.style.color = "red";
    }
    
    // Display alert if form was not validate
    if (editionNo < 1 || segmentNo < 1 || publicationQuantity < 1 || publicationPages < 1 || publicationYear < 1 || registrationDate == "") {
        

        // Call the function to scroll to the top with animation
        scrollToTop();

        
    }
    else {
        document.getElementById('alert-danger').style.display = "none";
        
        var editionsProperty = {
            s_no: serialNo,
            segmentNo: segmentNo,
            editionNo: editionNo,
            publicationQuantity: publicationQuantity,
            publicationPages: publicationPages,
            CDQuantity: CDQuantity,
            registrationDateType: registrationDateType,
            registrationDate: registrationDate,
            publicationDateType: publicationDateType,
            publicationYear: publicationYear,
            publicationMonth: publicationMonth,
            publicationDay: publicationDay,
            cupboardNo: cupboardNo,
            cellNo: cellNo
        };
        $.ajax({
            type: "POST",
            url: "/Monograph/AddSegment",
            data: { editionsProperty: editionsProperty },
            success: function (response) {

                if (response.redirectTo) {
                    window.location.href = response.redirectTo;
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

function EditEdition(s_no, edition_id, segment_id, edition_no, segment_no, publication_quantity, publication_pages, cd_quantity, publication_date_type, publication_year, publication_month, publication_day, registration_date_type, registration_year, registration_month, registration_day, cupboard_no, cell_no) {
    showTab("tab3");


    document.getElementById('updateSerialNo').value = s_no;
    document.getElementById('updateEditionId').value = edition_id;
    document.getElementById('updateSegmentId').value = segment_id;
    document.getElementById('updateSegmentNo').value = segment_no;
    document.getElementById('updateEditionNo').value = edition_no;
    document.getElementById('updatePublicationQuantity').value = publication_quantity;
    document.getElementById('updatePublicationPages').value = publication_pages;
    document.getElementById('updateCDQuantity').value = cd_quantity;
    document.getElementById('updatePublicationDateType').value = publication_date_type;
    document.getElementById('updatePublicationYear').value = publication_year;
    document.getElementById('updatePublicationMonth').value = publication_month;
    document.getElementById('updatePublicationDay').value = publication_day;
    document.getElementById('updateRegistrationDateType').value = registration_date_type;
    document.getElementById('updateRegistrationYear').value = registration_year;
    document.getElementById('updateRegistrationMonth').value = registration_month;
    document.getElementById('updateRegistrationDay').value = registration_day;
    document.getElementById('updateCupboardNo').value = cupboard_no;
    document.getElementById('updateCellNo').value = cell_no;

}



function DeleteEdition(serialNo, segmentId, editionId) {
    swal({
        title: "ایا تاسو ډاډه یاست؟",
        text: "",
        icon: "warning",
        buttons: ["نه", "هو"],
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                // If user clicked yes button

                $.ajax({
                    type: 'POST',
                    url: '/Monograph/DeleteEdition',
                    data: { s_no: serialNo, segmentId: segmentId, editionId: editionId },
                    success: function (response) {
                        if (response.redirectTo) {

                            window.location.href = response.redirectTo;

                        }
                        else if (response == false) {
                            swal("د یوې ستونزې له ستاسو غوښتنې صورت ونه موند.", {
                                icon: "error", buttons: "سمه ده"
                            });
                        }
                        else {
                            swal("ستونزه رامنځته شوه!", "Exception: " + response.responseText, {
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


