

function DeleteConfirmation(id) {

    swal({
        title: "ایا تاسو ډاډه یاست؟",
        text: "د مجلې اړونده ټول جلدونه او چاپونو به د همیشه لپاره له منځه لاړ شي!",
        icon: "warning",
        buttons: ["  نه  ", "  هـــو  "]
        ,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                // If user clicked yes button
                alert("Deleting");
                $.ajax({
                    type: 'POST',
                    url: '/Magazine/MagazineDeletion',
                    data: { id: id },
                    success: function (response) {
                        if (response == true) {

                            document.getElementById(id).remove();
                            swal("د کتاب اړونده معلومات په بریالیتوب سره له منځه لاړل", "", {
                                icon: "success",
                                buttons: "سمه ده"
                            });

                        }
                        else if (response == false) {
                            swal("د یوې ستونزې له امله ستاسو غوښتنې صورت ونه موند.", "", {
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

