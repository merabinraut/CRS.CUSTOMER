function showNotification(message, title, type) {

    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }

    switch (type) {
        case "SUCCESS":
            toastr.success(message, "");
            break;
        case "ERROR":
            toastr.error(message, "");
            break;
        case "INFORMATION":
            toastr.info(message, "");
            break;
        case "WARNING":
            toastr.warning(message, "");
            break;
    }
}