$(document).ready(function () {
    const host = "https://provinces.open-api.vn/api/";
    var callAPI = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data, "city");
            });
    }
    callAPI('https://provinces.open-api.vn/api/?depth=1');
    var callApiDistrict = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data.districts, "district");
            });
    }
    var callApiWard = (api) => {
        return axios.get(api)
            .then((response) => {
                renderData(response.data.wards, "ward");
            });
    }
    var renderData = (array, select) => {
        let row = `<option disable value="">Chọn</option>`;
        array.forEach(element => {
            row += `<option data-id="${element.code}" value="${element.name}">${element.name}</option>`
        });
        document.querySelector("#" + select).innerHTML = row
    }
    $("#city").change(() => {
        callApiDistrict(host + "p/" + $("#city").find(':selected').data('id') + "?depth=2");
    });
    $("#district").change(() => {
        callApiWard(host + "d/" + $("#district").find(':selected').data('id') + "?depth=2");
    });
    // Lấy ngày hôm nay
    var today = new Date();

    // Khởi tạo datepicker cho trường chọn ngày sinh
    $('.dateOfBirth').datepicker({
        dateFormat: 'yy-mm-dd',
        changeMonth: true,
        changeYear: true,
        yearRange: '1900:2023',
        beforeShowDay: function (date) {
            // Ngày không thể chọn (ngày sau ngày hôm nay) sẽ được làm xám
            if (date > today) {
                return [false, 'date-disabled', 'Ngày không thể chọn'];
            }
            return [true, ''];
        }
    });

    // Xử lý khi form được submit
    $('#registrationForm').on('submit', function (event) {
        const fieldsToValidate = ['#fullName', '#dateOfBirth', '#province', '#district', '#ward', '#address'];

        //for (const fieldId of fieldsToValidate) {
        //    validateField($(fieldId));
        //}

        if (!this.checkValidity()) {
            event.preventDefault();
            event.stopPropagation();
        }

        this.classList.add('was-validated');
    });

    $('#fullName').on('blur', function () {
        validateField($(this));
    });

    // Xử lý validate khi người dùng chuyển ra khỏi trường số nhà/tên đường
    $('#address').on('blur', function () {
        validateField($(this));
    });
    function validateField(field) {
        const value = field.val().trim();
        const invalidFeedback = field.next('.invalid-feedback');

        if (value === '') {
            field.addClass('is-invalid');
            invalidFeedback.text('Vui lòng nhập trường này.');
        } else {
            field.removeClass('is-invalid');
            invalidFeedback.text('');
        }
    }
});
