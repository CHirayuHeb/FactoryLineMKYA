
function jsFunctiongetorder(action) {
    const fRequest = document.forms.namedItem("formRequest");

    let viewfRequest = new FormData(fRequest);  // form header

    if (document.getElementById("id_prodOrder").value === ""
        || document.getElementById("id_Material").value === ""
        || document.getElementById("id_delDate").value === "") {
        Swal.fire({
            title: 'แจ้งเตือน',
            icon: 'warning',
            text: "กรุณากรอกข้อมูลให้ครบถ้วน !!!!"
        }).then((result) => {
            if (result.isConfirmed) {
                return false;
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            url: action,
            data: viewfRequest,
            processData: false,
            contentType: false,
            beforeSend: function () {
                Swal.fire({
                    title: 'Loading...',
                    icon: "info",
                    text: 'Please wait',
                    allowOutsideClick: false,
                    allowEscapeKey: false,
                    showConfirmButton: false,
                    didOpen: () => {
                        Swal.showLoading();
                    }

                });
                //            Swal.fire({
                //                title: 'Uploading Data',
                //                html: `
                //    <div style="margin-top:15px">
                //        <div class="progress">
                //            <div class="progress-bar progress-bar-striped progress-bar-animated"
                //                 style="width:100%">
                //            </div>
                //        </div>
                //        <p style="margin-top:10px">Please wait...</p>
                //    </div>
                //`,
                //                showConfirmButton: false,
                //                allowOutsideClick: false,
                //                allowEscapeKey: false
                //            });
                //            Swal.fire({
                //                title: 'Saving Data',
                //                html: `
                //    <div style="font-size:50px">
                //        ⏳
                //    </div>

                //    <div>Saving your data...</div>
                //`,
                //                showConfirmButton: false,
                //                allowOutsideClick: false,
                //                allowEscapeKey: false
                //            });
                //Swal.fire({
                //    title: 'Loading',
                //    html: 'Retrieving information...',
                //    icon: 'info',
                //    allowOutsideClick: false,
                //    allowEscapeKey: false,
                //    showConfirmButton: false,
                //    didOpen: () => {
                //        Swal.showLoading();
                //    }
                //});


            },
            success: async function (config) {
                Swal.fire({
                    title: config.c2,
                    icon: config.c1,
                    text: config.c3
                }).then((result) => {
                    if (result.isConfirmed) {
                        console.log("sucess call function get order");
                    }
                });
            },

            error: function (xhr, status, error) {
                Swal.close();

                Swal.fire({
                    title: 'ERROR',
                    icon: 'error',
                    text: error
                });
            }
        });
    }





}