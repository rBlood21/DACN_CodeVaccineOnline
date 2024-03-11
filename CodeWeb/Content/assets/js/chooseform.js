$(document).ready(function () {
    // Ẩn mặc định form tiêm hộ
    $('#registrationFormProxy').hide();

    // Xử lý sự kiện khi radio button thay đổi
    $('input[type=radio][name=registrationType]').change(function () {
        if (this.value === 'self') {
            // Nếu chọn tiêm cho bản thân, ẩn form tiêm hộ và hiện form cho bản thân
            $('#registrationFormSelf').show();
            $('#registrationFormProxy').hide();
        } else if (this.value === 'proxy') {
            // Nếu chọn tiêm hộ, ẩn form cho bản thân và hiện form tiêm hộ
            $('#registrationFormSelf').hide();
            $('#registrationFormProxy').show();
        }
    });
});
