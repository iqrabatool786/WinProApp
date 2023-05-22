//// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
//// for details on configuring this project to bundle and minify static web assets.

//// Write your JavaScript code.

$(document).on('keypress', '.numberonly', function (e) {
    var charCode = (e.which) ? e.which : e.keyCode
    if (String.fromCharCode(charCode).match(/[^0-9]/g))
        return false;
});

$(document).on('keypress', '.decimalTwoDigits', function () {
    let isSelected = true;
    if (!window.getSelection().toString()) {
        isSelected = false;
    }
    if (isSelected) {
        $(this).val('');
    }
    if (this.value.includes('.')) {
        if (this.value.split('.')[1].length > 1) {
            return false;
        }
    };
});
