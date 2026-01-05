const { stat } = require("fs/promises");



function DeleteConfirmation(id) {
   
    swal({
        title: "ایا تاسو ډاډه یاست؟",
        text: "",
        icon: "warning",
        buttons: ["نه","هو"]
,
        dangerMode: true,
    })
        .then((willDelete) => {
            if (willDelete) {
                // If user clicked yes button
                
                $.ajax({
                    type: 'POST',
                    url: '/Book/BookDeletion',
                    data: {id : id},
                    success: function (response) {
                        if (response == true) {

                            document.getElementById(id).remove();
                            swal("په بریالیتوب سره کارونکی له سېسټم څخه لرې شو.","", {
                                icon: "success",
                                buttons: "سمه ده"
                            });

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

